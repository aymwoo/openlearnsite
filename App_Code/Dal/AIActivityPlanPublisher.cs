using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using LearnSite.Common;

namespace LearnSite.DAL
{
    public class AIActivityPlanPublisher
    {
        public LearnSite.Model.AIActivityPlanPublishResult Publish(LearnSite.Model.AIActivityPlanPublishRequest request)
        {
            if (request == null || request.Cid <= 0 || request.Hid <= 0 || !AIActivityPlanDraftHelper.IsValidDraft(request.Draft))
            {
                return null;
            }

            AIActivityPlanPublishContentBuilder contentBuilder = new AIActivityPlanPublishContentBuilder();
            string appendContent = contentBuilder.BuildLessonContent(request.SelectedSectionKeys, request.Draft);
            string missionContent = contentBuilder.BuildMissionContent(request.Topic, request.Draft);
            string missionTitle = BuildMissionTitle(request.Topic);
            if (string.IsNullOrEmpty(missionContent))
            {
                return null;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        CourseSnapshot course = LoadCourse(connection, transaction, request.Cid, request.Hid);
                        if (course == null)
                        {
                            transaction.Rollback();
                            return null;
                        }

                        DraftLinkSnapshot draftLink = LoadDraftLink(connection, transaction, request.Cid, request.Hid);
                        int missionId = ResolveMissionId(connection, transaction, request, draftLink, missionTitle, missionContent);
                        int listMenuId = ResolveListMenuId(connection, transaction, request, draftLink, missionId, missionTitle);
                        string updatedCourseContent = AppendCourseContent(course.Ccontent, appendContent);

                        UpdateCourse(connection, transaction, request.Cid, updatedCourseContent);
                        UpdateMissionPublishState(connection, transaction, missionId, request.PublishToStudents, missionTitle, missionContent);
                        UpdateListMenu(connection, transaction, listMenuId, request.Cid, missionId, missionTitle, request.PublishToStudents);
                        SaveDraftLinks(connection, transaction, request, updatedCourseContent, missionId, listMenuId);

                        transaction.Commit();
                        return new LearnSite.Model.AIActivityPlanPublishResult
                        {
                            MissionId = missionId,
                            ListMenuId = listMenuId,
                            MissionTitle = missionTitle,
                            PublishedToStudents = request.PublishToStudents,
                            UpdatedCourseContent = updatedCourseContent
                        };
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        private static string BuildMissionTitle(string topic)
        {
            string boundedTopic = AIActivityPlanPromptBuilder.BoundText(topic, 40);
            if (string.IsNullOrEmpty(boundedTopic))
            {
                boundedTopic = "未命名主题";
            }

            return "AI活动-" + boundedTopic;
        }

        private static string AppendCourseContent(string existingContent, string appendContent)
        {
            if (string.IsNullOrEmpty(appendContent))
            {
                return existingContent ?? string.Empty;
            }

            if (string.IsNullOrEmpty(existingContent))
            {
                return appendContent;
            }

            return existingContent.TrimEnd() + Environment.NewLine + appendContent;
        }

        private static CourseSnapshot LoadCourse(SqlConnection connection, SqlTransaction transaction, int cid, int hid)
        {
            using (SqlCommand command = new SqlCommand("select top 1 Cid,Chid,Ccontent from Courses where Cid=@Cid and Chid=@Hid", connection, transaction))
            {
                command.Parameters.AddWithValue("@Cid", cid);
                command.Parameters.AddWithValue("@Hid", hid);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        return null;
                    }

                    return new CourseSnapshot
                    {
                        Cid = reader.GetInt32(0),
                        Chid = reader.IsDBNull(1) ? 0 : reader.GetInt32(1),
                        Ccontent = reader.IsDBNull(2) ? string.Empty : reader.GetString(2)
                    };
                }
            }
        }

        private static DraftLinkSnapshot LoadDraftLink(SqlConnection connection, SqlTransaction transaction, int cid, int hid)
        {
            using (SqlCommand command = new SqlCommand("select top 1 LinkedMissionId,LinkedListMenuId from CourseActivityPlanDraft where Cid=@Cid and Hid=@Hid", connection, transaction))
            {
                command.Parameters.AddWithValue("@Cid", cid);
                command.Parameters.AddWithValue("@Hid", hid);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        return new DraftLinkSnapshot();
                    }

                    return new DraftLinkSnapshot
                    {
                        LinkedMissionId = reader.IsDBNull(0) ? (int?)null : reader.GetInt32(0),
                        LinkedListMenuId = reader.IsDBNull(1) ? (int?)null : reader.GetInt32(1)
                    };
                }
            }
        }

        private static int ResolveMissionId(SqlConnection connection, SqlTransaction transaction, LearnSite.Model.AIActivityPlanPublishRequest request, DraftLinkSnapshot draftLink, string missionTitle, string missionContent)
        {
            if (draftLink.LinkedMissionId.HasValue && MissionBelongsToCourse(connection, transaction, draftLink.LinkedMissionId.Value, request.Cid))
            {
                return draftLink.LinkedMissionId.Value;
            }

            using (SqlCommand command = new SqlCommand(@"insert into Mission(Mcid,Mtitle,Msort,Mupload,Mcategory,Mpublish,Mcontent,Mfiletype,Mdate,Mhit,Mgroup,Mgid,Microworld,Mexample)
values(@Mcid,@Mtitle,@Msort,@Mupload,@Mcategory,@Mpublish,@Mcontent,@Mfiletype,@Mdate,@Mhit,@Mgroup,@Mgid,@Microworld,@Mexample);
select cast(scope_identity() as int);", connection, transaction))
            {
                command.Parameters.AddWithValue("@Mcid", request.Cid);
                command.Parameters.AddWithValue("@Mtitle", missionTitle);
                command.Parameters.AddWithValue("@Msort", GetNextMissionSort(connection, transaction, request.Cid));
                command.Parameters.AddWithValue("@Mupload", true);
                command.Parameters.AddWithValue("@Mcategory", 0);
                command.Parameters.AddWithValue("@Mpublish", request.PublishToStudents);
                command.Parameters.AddWithValue("@Mcontent", missionContent);
                command.Parameters.AddWithValue("@Mfiletype", "office");
                command.Parameters.AddWithValue("@Mdate", DateTime.Now);
                command.Parameters.AddWithValue("@Mhit", 0);
                command.Parameters.AddWithValue("@Mgroup", false);
                command.Parameters.AddWithValue("@Mgid", 0);
                command.Parameters.AddWithValue("@Microworld", false);
                command.Parameters.AddWithValue("@Mexample", string.Empty);
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        private static int ResolveListMenuId(SqlConnection connection, SqlTransaction transaction, LearnSite.Model.AIActivityPlanPublishRequest request, DraftLinkSnapshot draftLink, int missionId, string missionTitle)
        {
            if (draftLink.LinkedListMenuId.HasValue && ListMenuBelongsToCourse(connection, transaction, draftLink.LinkedListMenuId.Value, request.Cid))
            {
                return draftLink.LinkedListMenuId.Value;
            }

            using (SqlCommand command = new SqlCommand(@"insert into ListMenu(Lcid,Lsort,Ltype,Lxid,Lshow,Ltitle)
values(@Lcid,@Lsort,@Ltype,@Lxid,@Lshow,@Ltitle);
select cast(scope_identity() as int);", connection, transaction))
            {
                command.Parameters.AddWithValue("@Lcid", request.Cid);
                command.Parameters.AddWithValue("@Lsort", GetNextListSort(connection, transaction, request.Cid));
                command.Parameters.AddWithValue("@Ltype", 1);
                command.Parameters.AddWithValue("@Lxid", missionId);
                command.Parameters.AddWithValue("@Lshow", request.PublishToStudents);
                command.Parameters.AddWithValue("@Ltitle", missionTitle);
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        private static void UpdateCourse(SqlConnection connection, SqlTransaction transaction, int cid, string updatedContent)
        {
            using (SqlCommand command = new SqlCommand("update Courses set Ccontent=@Ccontent where Cid=@Cid", connection, transaction))
            {
                command.Parameters.AddWithValue("@Cid", cid);
                command.Parameters.AddWithValue("@Ccontent", updatedContent ?? string.Empty);
                command.ExecuteNonQuery();
            }
        }

        private static void UpdateMissionPublishState(SqlConnection connection, SqlTransaction transaction, int missionId, bool publishToStudents, string missionTitle, string missionContent)
        {
            using (SqlCommand command = new SqlCommand("update Mission set Mtitle=@Mtitle,Mpublish=@Mpublish,Mupload=@Mupload,Mcategory=@Mcategory,Mcontent=@Mcontent,Mfiletype=@Mfiletype where Mid=@Mid", connection, transaction))
            {
                command.Parameters.AddWithValue("@Mid", missionId);
                command.Parameters.AddWithValue("@Mtitle", missionTitle);
                command.Parameters.AddWithValue("@Mpublish", publishToStudents);
                command.Parameters.AddWithValue("@Mupload", true);
                command.Parameters.AddWithValue("@Mcategory", 0);
                command.Parameters.AddWithValue("@Mcontent", missionContent);
                command.Parameters.AddWithValue("@Mfiletype", "office");
                command.ExecuteNonQuery();
            }
        }

        private static void UpdateListMenu(SqlConnection connection, SqlTransaction transaction, int listMenuId, int cid, int missionId, string missionTitle, bool publishToStudents)
        {
            using (SqlCommand command = new SqlCommand("update ListMenu set Lcid=@Lcid,Ltype=@Ltype,Lxid=@Lxid,Lshow=@Lshow,Ltitle=@Ltitle where Lid=@Lid", connection, transaction))
            {
                command.Parameters.AddWithValue("@Lid", listMenuId);
                command.Parameters.AddWithValue("@Lcid", cid);
                command.Parameters.AddWithValue("@Ltype", 1);
                command.Parameters.AddWithValue("@Lxid", missionId);
                command.Parameters.AddWithValue("@Lshow", publishToStudents);
                command.Parameters.AddWithValue("@Ltitle", missionTitle);
                command.ExecuteNonQuery();
            }
        }

        private static void SaveDraftLinks(SqlConnection connection, SqlTransaction transaction, LearnSite.Model.AIActivityPlanPublishRequest request, string updatedCourseContent, int missionId, int listMenuId)
        {
            LearnSite.Model.CourseActivityPlanDraft draftRecord = AIActivityPlanSavedDraftHelper.BuildRecord(
                request.Cid,
                request.Hid,
                request.Topic,
                string.Empty,
                string.Empty,
                string.Empty,
                updatedCourseContent,
                request.Draft,
                missionId,
                listMenuId);
            if (draftRecord == null)
            {
                return;
            }

            using (SqlCommand command = new SqlCommand(@"if exists(select 1 from CourseActivityPlanDraft where Cid=@Cid and Hid=@Hid)
update CourseActivityPlanDraft set Topic=@Topic,DraftJson=@DraftJson,ExistingCourseContentSnapshot=@ExistingCourseContentSnapshot,LinkedMissionId=@LinkedMissionId,LinkedListMenuId=@LinkedListMenuId,UpdatedAt=@UpdatedAt where Cid=@Cid and Hid=@Hid
else
insert into CourseActivityPlanDraft(Cid,Hid,Topic,Grade,Duration,TeachingGoalsInput,ExistingCourseContentSnapshot,DraftJson,LinkedMissionId,LinkedListMenuId,CreatedAt,UpdatedAt)
values(@Cid,@Hid,@Topic,@Grade,@Duration,@TeachingGoalsInput,@ExistingCourseContentSnapshot,@DraftJson,@LinkedMissionId,@LinkedListMenuId,@CreatedAt,@UpdatedAt)", connection, transaction))
            {
                command.Parameters.AddWithValue("@Cid", draftRecord.Cid);
                command.Parameters.AddWithValue("@Hid", draftRecord.Hid);
                command.Parameters.AddWithValue("@Topic", draftRecord.Topic ?? string.Empty);
                command.Parameters.AddWithValue("@Grade", draftRecord.Grade ?? string.Empty);
                command.Parameters.AddWithValue("@Duration", draftRecord.Duration ?? string.Empty);
                command.Parameters.AddWithValue("@TeachingGoalsInput", draftRecord.TeachingGoalsInput ?? string.Empty);
                command.Parameters.AddWithValue("@ExistingCourseContentSnapshot", draftRecord.ExistingCourseContentSnapshot ?? string.Empty);
                command.Parameters.AddWithValue("@DraftJson", draftRecord.DraftJson ?? string.Empty);
                command.Parameters.AddWithValue("@LinkedMissionId", missionId);
                command.Parameters.AddWithValue("@LinkedListMenuId", listMenuId);
                command.Parameters.AddWithValue("@CreatedAt", draftRecord.CreatedAt == DateTime.MinValue ? DateTime.Now : draftRecord.CreatedAt);
                command.Parameters.AddWithValue("@UpdatedAt", draftRecord.UpdatedAt == DateTime.MinValue ? DateTime.Now : draftRecord.UpdatedAt);
                command.ExecuteNonQuery();
            }
        }

        private static bool MissionBelongsToCourse(SqlConnection connection, SqlTransaction transaction, int missionId, int cid)
        {
            using (SqlCommand command = new SqlCommand("select count(1) from Mission where Mid=@Mid and Mcid=@Mcid", connection, transaction))
            {
                command.Parameters.AddWithValue("@Mid", missionId);
                command.Parameters.AddWithValue("@Mcid", cid);
                return Convert.ToInt32(command.ExecuteScalar()) > 0;
            }
        }

        private static bool ListMenuBelongsToCourse(SqlConnection connection, SqlTransaction transaction, int listMenuId, int cid)
        {
            using (SqlCommand command = new SqlCommand("select count(1) from ListMenu where Lid=@Lid and Lcid=@Lcid", connection, transaction))
            {
                command.Parameters.AddWithValue("@Lid", listMenuId);
                command.Parameters.AddWithValue("@Lcid", cid);
                return Convert.ToInt32(command.ExecuteScalar()) > 0;
            }
        }

        private static int GetNextMissionSort(SqlConnection connection, SqlTransaction transaction, int cid)
        {
            using (SqlCommand command = new SqlCommand("select isnull(max(Msort),0)+1 from Mission where Mcid=@Mcid", connection, transaction))
            {
                command.Parameters.AddWithValue("@Mcid", cid);
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        private static int GetNextListSort(SqlConnection connection, SqlTransaction transaction, int cid)
        {
            using (SqlCommand command = new SqlCommand("select isnull(max(Lsort),0)+1 from ListMenu where Lcid=@Lcid", connection, transaction))
            {
                command.Parameters.AddWithValue("@Lcid", cid);
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        private class CourseSnapshot
        {
            public int Cid { get; set; }

            public int Chid { get; set; }

            public string Ccontent { get; set; }
        }

        private class DraftLinkSnapshot
        {
            public int? LinkedMissionId { get; set; }

            public int? LinkedListMenuId { get; set; }
        }
    }
}
