using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using LearnSite.Common;

namespace LearnSite.DAL
{
    public class AIActivityPlanPublisher
    {
        public LearnSite.Model.AIActivityPlanPublishResult Publish(LearnSite.Model.AIActivityPlanPublishRequest request)
        {
            if (request == null || request.Cid <= 0 || request.Hid <= 0)
            {
                return null;
            }

            if (AIActivityPlanDraftHelper.IsValidFullLessonDraft(request.FullLessonDraft))
            {
                return PublishFullLesson(request);
            }

            if (!AIActivityPlanDraftHelper.IsValidDraft(request.Draft))
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
                        UpdateListMenu(connection, transaction, listMenuId, request.Cid, missionId, missionTitle, 1, request.PublishToStudents);
                        SaveSingleDraftLinks(connection, transaction, request, updatedCourseContent, missionId, listMenuId);

                        transaction.Commit();
                        return new LearnSite.Model.AIActivityPlanPublishResult
                        {
                            MissionId = missionId,
                            ListMenuId = listMenuId,
                            MissionTitle = missionTitle,
                            PublishedToStudents = request.PublishToStudents,
                            UpdatedCourseContent = updatedCourseContent,
                            IsFullLessonPublish = false,
                            PublishedBlocks = new List<LearnSite.Model.AIActivityPlanPublishedBlockResult>()
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

        public LearnSite.Model.AIActivityPlanPublishResult PublishFullLesson(LearnSite.Model.AIActivityPlanPublishRequest request)
        {
            if (request == null || request.Cid <= 0 || request.Hid <= 0 || !AIActivityPlanDraftHelper.IsValidFullLessonDraft(request.FullLessonDraft))
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
                        Dictionary<string, ActivityPlanPublishLinkPayload> publishLinks = draftLink.PublishLinks;
                        string appendContent = new AIActivityPlanPublishContentBuilder().BuildFullLessonLessonContent(request.Topic, request.FullLessonDraft);
                        if (string.IsNullOrEmpty(appendContent))
                        {
                            transaction.Rollback();
                            return null;
                        }

                        string updatedCourseContent = AppendOrReplacePublishedFullLessonContent(course.Ccontent, appendContent);
                        List<LearnSite.Model.AIActivityPlanPublishedBlockResult> publishedBlocks = new List<LearnSite.Model.AIActivityPlanPublishedBlockResult>();

                        int nextMissionSort = GetNextMissionSort(connection, transaction, request.Cid);
                        int nextListSort = GetNextListSort(connection, transaction, request.Cid);
                        int teacherClassId = LoadFirstTeacherClassId(connection, transaction, request.Hid);

                        for (int i = 0; i < request.FullLessonDraft.Blocks.Count; i++)
                        {
                            FullLessonDraftBlock block = request.FullLessonDraft.Blocks[i];
                            if (!IsSupportedPublishBlock(block))
                            {
                                transaction.Rollback();
                                return null;
                            }

                            ActivityPlanPublishLinkPayload existingLink = GetPublishLink(publishLinks, block.BlockKey);
                            LearnSite.Model.AIActivityPlanPublishedBlockResult publishResult = PublishBlock(
                                connection,
                                transaction,
                                request,
                                block,
                                existingLink,
                                ref nextMissionSort,
                                ref nextListSort,
                                teacherClassId);
                            if (publishResult == null)
                            {
                                transaction.Rollback();
                                return null;
                            }

                            publishedBlocks.Add(publishResult);
                            publishLinks[block.BlockKey] = new ActivityPlanPublishLinkPayload
                            {
                                BlockKey = block.BlockKey,
                                BlockType = block.BlockType,
                                MissionId = publishResult.MissionId,
                                ExamId = publishResult.ExamId,
                                PaperId = publishResult.PaperId,
                                ListMenuId = publishResult.ListMenuId
                            };
                        }

                        UpdateCourse(connection, transaction, request.Cid, updatedCourseContent);
                        SaveFullLessonDraftLinks(connection, transaction, request, updatedCourseContent, publishLinks, publishedBlocks);

                        transaction.Commit();
                        return new LearnSite.Model.AIActivityPlanPublishResult
                        {
                            MissionId = 0,
                            ListMenuId = 0,
                            MissionTitle = string.Empty,
                            PublishedToStudents = request.PublishToStudents,
                            UpdatedCourseContent = updatedCourseContent,
                            IsFullLessonPublish = true,
                            PublishedBlocks = publishedBlocks
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

        private static LearnSite.Model.AIActivityPlanPublishedBlockResult PublishBlock(
            SqlConnection connection,
            SqlTransaction transaction,
            LearnSite.Model.AIActivityPlanPublishRequest request,
            FullLessonDraftBlock block,
            ActivityPlanPublishLinkPayload existingLink,
            ref int nextMissionSort,
            ref int nextListSort,
            int teacherClassId)
        {
            string blockType = NormalizeBlockType(block.BlockType);
            switch (blockType)
            {
                case "mission":
                    return PublishMissionBlock(connection, transaction, request, block, existingLink, ref nextMissionSort, ref nextListSort);
                case "guidedinquiry":
                    return PublishGuidedInquiryBlock(connection, transaction, request, block, existingLink, ref nextMissionSort, ref nextListSort);
                case "resource-study":
                    return PublishResourceStudyBlock(connection, transaction, request, block, existingLink, ref nextMissionSort, ref nextListSort);
                case "webcourseware":
                    return PublishWebCoursewareBlock(connection, transaction, request, block, existingLink, ref nextMissionSort, ref nextListSort);
                case "quiz":
                    return PublishQuizBlock(connection, transaction, request, block, existingLink, ref nextListSort, teacherClassId);
                default:
                    return null;
            }
        }

        private static LearnSite.Model.AIActivityPlanPublishedBlockResult PublishMissionBlock(SqlConnection connection, SqlTransaction transaction, LearnSite.Model.AIActivityPlanPublishRequest request, FullLessonDraftBlock block, ActivityPlanPublishLinkPayload existingLink, ref int nextMissionSort, ref int nextListSort)
        {
            AIActivityPlanPublishContentBuilder builder = new AIActivityPlanPublishContentBuilder();
            string missionContent = builder.BuildMissionBlockContent(request.Topic, block);
            if (string.IsNullOrEmpty(missionContent))
            {
                return null;
            }

            string title = BuildBlockMissionTitle(block);
            int missionId = ResolveOrCreateMission(
                connection,
                transaction,
                request.Cid,
                existingLink == null ? null : existingLink.MissionId,
                title,
                missionContent,
                true,
                0,
                "office",
                string.Empty,
                request.PublishToStudents,
                ref nextMissionSort);
            int listMenuId = ResolveOrCreateListMenu(
                connection,
                transaction,
                request.Cid,
                existingLink == null ? null : existingLink.ListMenuId,
                missionId,
                title,
                1,
                request.PublishToStudents,
                ref nextListSort);

            return new LearnSite.Model.AIActivityPlanPublishedBlockResult
            {
                BlockKey = block.BlockKey,
                BlockType = block.BlockType,
                Title = title,
                MissionId = missionId,
                ListMenuId = listMenuId,
                ListMenuType = 1,
                RuntimeRouteType = "showmission"
            };
        }

        private static LearnSite.Model.AIActivityPlanPublishedBlockResult PublishGuidedInquiryBlock(SqlConnection connection, SqlTransaction transaction, LearnSite.Model.AIActivityPlanPublishRequest request, FullLessonDraftBlock block, ActivityPlanPublishLinkPayload existingLink, ref int nextMissionSort, ref int nextListSort)
        {
            AIActivityPlanPublishContentBuilder builder = new AIActivityPlanPublishContentBuilder();
            string missionContent = builder.BuildGuidedInquiryMissionContent(request.Topic, block);
            if (string.IsNullOrEmpty(missionContent))
            {
                return null;
            }

            string title = BuildBlockMissionTitle(block);
            int missionId = ResolveOrCreateMission(
                connection,
                transaction,
                request.Cid,
                existingLink == null ? null : existingLink.MissionId,
                title,
                missionContent,
                true,
                0,
                "office",
                string.Empty,
                request.PublishToStudents,
                ref nextMissionSort);
            int listMenuId = ResolveOrCreateListMenu(
                connection,
                transaction,
                request.Cid,
                existingLink == null ? null : existingLink.ListMenuId,
                missionId,
                title,
                1,
                request.PublishToStudents,
                ref nextListSort);

            return new LearnSite.Model.AIActivityPlanPublishedBlockResult
            {
                BlockKey = block.BlockKey,
                BlockType = block.BlockType,
                Title = title,
                MissionId = missionId,
                ListMenuId = listMenuId,
                ListMenuType = 1,
                RuntimeRouteType = "showmission"
            };
        }

        private static LearnSite.Model.AIActivityPlanPublishedBlockResult PublishResourceStudyBlock(SqlConnection connection, SqlTransaction transaction, LearnSite.Model.AIActivityPlanPublishRequest request, FullLessonDraftBlock block, ActivityPlanPublishLinkPayload existingLink, ref int nextMissionSort, ref int nextListSort)
        {
            if (block.ResourceStudy == null)
            {
                return null;
            }

            int missionId = ResolveOrCreateMission(
                connection,
                transaction,
                request.Cid,
                existingLink == null ? null : existingLink.MissionId,
                block.ResourceStudy.Mtitle,
                block.ResourceStudy.Mcontent,
                false,
                1,
                "html",
                string.Empty,
                request.PublishToStudents,
                ref nextMissionSort);
            int listMenuId = ResolveOrCreateListMenu(
                connection,
                transaction,
                request.Cid,
                existingLink == null ? null : existingLink.ListMenuId,
                missionId,
                block.ResourceStudy.Mtitle,
                6,
                request.PublishToStudents,
                ref nextListSort);

            return new LearnSite.Model.AIActivityPlanPublishedBlockResult
            {
                BlockKey = block.BlockKey,
                BlockType = block.BlockType,
                Title = block.ResourceStudy.Mtitle,
                MissionId = missionId,
                ListMenuId = listMenuId,
                ListMenuType = 6,
                RuntimeRouteType = "description"
            };
        }

        private static LearnSite.Model.AIActivityPlanPublishedBlockResult PublishWebCoursewareBlock(SqlConnection connection, SqlTransaction transaction, LearnSite.Model.AIActivityPlanPublishRequest request, FullLessonDraftBlock block, ActivityPlanPublishLinkPayload existingLink, ref int nextMissionSort, ref int nextListSort)
        {
            if (block.WebCourseware == null)
            {
                return null;
            }

            string baseBackUrl = block.WebCourseware.Mback;
            int missionId = ResolveOrCreateMission(
                connection,
                transaction,
                request.Cid,
                existingLink == null ? null : existingLink.MissionId,
                block.WebCourseware.Mtitle,
                string.Empty,
                true,
                38,
                "ware",
                baseBackUrl,
                request.PublishToStudents,
                ref nextMissionSort);
            int listMenuId = ResolveOrCreateListMenu(
                connection,
                transaction,
                request.Cid,
                existingLink == null ? null : existingLink.ListMenuId,
                missionId,
                block.WebCourseware.Mtitle,
                38,
                request.PublishToStudents,
                ref nextListSort);
            string publishedBackUrl = BuildPublishedWebCoursewareBackUrl(baseBackUrl, listMenuId, missionId);
            if (!string.Equals(publishedBackUrl, baseBackUrl, StringComparison.Ordinal))
            {
                UpdateMissionBackUrl(connection, transaction, missionId, publishedBackUrl);
            }

            return new LearnSite.Model.AIActivityPlanPublishedBlockResult
            {
                BlockKey = block.BlockKey,
                BlockType = block.BlockType,
                Title = block.WebCourseware.Mtitle,
                MissionId = missionId,
                ListMenuId = listMenuId,
                ListMenuType = 38,
                RuntimeRouteType = "ware"
            };
        }

        private static LearnSite.Model.AIActivityPlanPublishedBlockResult PublishQuizBlock(SqlConnection connection, SqlTransaction transaction, LearnSite.Model.AIActivityPlanPublishRequest request, FullLessonDraftBlock block, ActivityPlanPublishLinkPayload existingLink, ref int nextListSort, int teacherClassId)
        {
            if (block.Quiz == null)
            {
                return null;
            }

            int paperId = ResolveOrCreateExamPaper(connection, transaction, request.Hid, existingLink == null ? null : existingLink.PaperId, block.Quiz.PaperTitle, block.Quiz.QuestionSummary, block.Quiz.Duration);
            if (paperId <= 0)
            {
                return null;
            }

            int examId = ResolveOrCreateExam(connection, transaction, request, block, existingLink == null ? null : existingLink.ExamId, paperId, teacherClassId);
            if (examId <= 0)
            {
                return null;
            }

            int listMenuId = ResolveOrCreateListMenu(
                connection,
                transaction,
                request.Cid,
                existingLink == null ? null : existingLink.ListMenuId,
                examId,
                block.Quiz.ExamName,
                39,
                request.PublishToStudents,
                ref nextListSort);

            return new LearnSite.Model.AIActivityPlanPublishedBlockResult
            {
                BlockKey = block.BlockKey,
                BlockType = block.BlockType,
                Title = block.Quiz.ExamName,
                ExamId = examId,
                PaperId = paperId,
                ListMenuId = listMenuId,
                ListMenuType = 39,
                RuntimeRouteType = "preview"
            };
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

        private static string BuildBlockMissionTitle(FullLessonDraftBlock block)
        {
            string boundedTitle = AIActivityPlanPromptBuilder.BoundText(block == null ? string.Empty : block.Title, 40);
            if (string.IsNullOrEmpty(boundedTitle))
            {
                boundedTitle = "未命名环节";
            }

            return "AI整课-" + boundedTitle;
        }

        private static string BuildPublishedWebCoursewareBackUrl(string url, int listMenuId, int missionId)
        {
            string result = url ?? string.Empty;
            if (string.IsNullOrEmpty(result))
            {
                return result;
            }

            if (listMenuId > 0 && result.IndexOf("lid=", StringComparison.OrdinalIgnoreCase) < 0)
            {
                result += (result.IndexOf('?') >= 0 ? "&" : "?") + "lid=" + listMenuId.ToString();
            }

            if (missionId > 0 && result.IndexOf("mid=", StringComparison.OrdinalIgnoreCase) < 0)
            {
                result += (result.IndexOf('?') >= 0 ? "&" : "?") + "mid=" + missionId.ToString();
            }

            return result;
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

        private static string AppendOrReplacePublishedFullLessonContent(string existingContent, string publishContent)
        {
            string beginMarker = AIActivityPlanPublishContentBuilder.FullLessonPublishBeginMarker;
            string endMarker = AIActivityPlanPublishContentBuilder.FullLessonPublishEndMarker;
            string current = existingContent ?? string.Empty;
            int beginIndex = current.IndexOf(beginMarker, StringComparison.Ordinal);
            int endIndex = current.IndexOf(endMarker, StringComparison.Ordinal);
            if (beginIndex >= 0 && endIndex > beginIndex)
            {
                string prefix = current.Substring(0, beginIndex).TrimEnd();
                string suffix = current.Substring(endIndex + endMarker.Length).TrimStart();
                string rebuilt = string.IsNullOrEmpty(prefix) ? publishContent : prefix + Environment.NewLine + publishContent;
                if (!string.IsNullOrEmpty(suffix))
                {
                    rebuilt += Environment.NewLine + suffix;
                }

                return rebuilt;
            }

            return AppendCourseContent(current, publishContent);
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
            using (SqlCommand command = new SqlCommand("select top 1 Topic,Grade,Duration,TeachingGoalsInput,ExistingCourseContentSnapshot,DraftJson,LinkedMissionId,LinkedListMenuId from CourseActivityPlanDraft where Cid=@Cid and Hid=@Hid", connection, transaction))
            {
                command.Parameters.AddWithValue("@Cid", cid);
                command.Parameters.AddWithValue("@Hid", hid);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        return new DraftLinkSnapshot();
                    }

                    LearnSite.Model.CourseActivityPlanDraft record = new LearnSite.Model.CourseActivityPlanDraft
                    {
                        Cid = cid,
                        Hid = hid,
                        Topic = reader.IsDBNull(0) ? string.Empty : reader.GetString(0),
                        Grade = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                        Duration = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                        TeachingGoalsInput = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                        ExistingCourseContentSnapshot = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                        DraftJson = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                        LinkedMissionId = reader.IsDBNull(6) ? (int?)null : reader.GetInt32(6),
                        LinkedListMenuId = reader.IsDBNull(7) ? (int?)null : reader.GetInt32(7)
                    };
                    ActivityPlanSavedDraftPayload payload = AIActivityPlanSavedDraftHelper.ParseFullLessonRecord(record, cid, hid);
                    return new DraftLinkSnapshot
                    {
                        LinkedMissionId = record.LinkedMissionId,
                        LinkedListMenuId = record.LinkedListMenuId,
                        Topic = record.Topic,
                        Grade = record.Grade,
                        Duration = record.Duration,
                        TeachingGoalsInput = record.TeachingGoalsInput,
                        ExistingCourseContentSnapshot = record.ExistingCourseContentSnapshot,
                        FullLessonDraft = payload == null ? null : payload.FullLessonDraft,
                        PublishLinks = payload == null || payload.PublishLinks == null
                            ? new Dictionary<string, ActivityPlanPublishLinkPayload>(StringComparer.OrdinalIgnoreCase)
                            : payload.PublishLinks
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

            using (SqlCommand command = new SqlCommand(@"insert into Mission(Mcid,Mtitle,Msort,Mupload,Mcategory,Mpublish,Mcontent,Mfiletype,Mdate,Mhit,Mgroup,Mgid,Microworld,Mexample,Mback,Mhelp)
values(@Mcid,@Mtitle,@Msort,@Mupload,@Mcategory,@Mpublish,@Mcontent,@Mfiletype,@Mdate,@Mhit,@Mgroup,@Mgid,@Microworld,@Mexample,@Mback,@Mhelp);
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
                command.Parameters.AddWithValue("@Mback", string.Empty);
                command.Parameters.AddWithValue("@Mhelp", false);
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        private static void UpdateMissionBackUrl(SqlConnection connection, SqlTransaction transaction, int missionId, string backUrl)
        {
            if (missionId <= 0 || string.IsNullOrEmpty(backUrl))
            {
                return;
            }

            using (SqlCommand command = new SqlCommand("update Mission set Mback=@Mback where Mid=@Mid", connection, transaction))
            {
                command.Parameters.AddWithValue("@Mback", backUrl);
                command.Parameters.AddWithValue("@Mid", missionId);
                command.ExecuteNonQuery();
            }
        }

        private static int ResolveListMenuId(SqlConnection connection, SqlTransaction transaction, LearnSite.Model.AIActivityPlanPublishRequest request, DraftLinkSnapshot draftLink, int xid, string title)
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
                command.Parameters.AddWithValue("@Lxid", xid);
                command.Parameters.AddWithValue("@Lshow", request.PublishToStudents);
                command.Parameters.AddWithValue("@Ltitle", title);
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        private static int ResolveOrCreateMission(SqlConnection connection, SqlTransaction transaction, int cid, int? existingMissionId, string title, string content, bool upload, int category, string fileType, string backUrl, bool publishToStudents, ref int nextMissionSort)
        {
            int missionId = existingMissionId.HasValue && MissionBelongsToCourse(connection, transaction, existingMissionId.Value, cid)
                ? existingMissionId.Value
                : 0;
            if (missionId <= 0)
            {
                using (SqlCommand command = new SqlCommand(@"insert into Mission(Mtitle,Mcid,Mcontent,Mdate,Mhit,Mfiletype,Mupload,Msort,Mpublish,Mgroup,Mgid,Mexample,Mcategory,Microworld,Mback,Mhelp)
values(@Mtitle,@Mcid,@Mcontent,@Mdate,@Mhit,@Mfiletype,@Mupload,@Msort,@Mpublish,@Mgroup,@Mgid,@Mexample,@Mcategory,@Microworld,@Mback,@Mhelp);
select cast(scope_identity() as int);", connection, transaction))
                {
                    command.Parameters.AddWithValue("@Mtitle", title);
                    command.Parameters.AddWithValue("@Mcid", cid);
                    command.Parameters.AddWithValue("@Mcontent", content ?? string.Empty);
                    command.Parameters.AddWithValue("@Mdate", DateTime.Now);
                    command.Parameters.AddWithValue("@Mhit", 0);
                    command.Parameters.AddWithValue("@Mfiletype", fileType);
                    command.Parameters.AddWithValue("@Mupload", upload);
                    command.Parameters.AddWithValue("@Msort", nextMissionSort++);
                    command.Parameters.AddWithValue("@Mpublish", publishToStudents);
                    command.Parameters.AddWithValue("@Mgroup", false);
                    command.Parameters.AddWithValue("@Mgid", 0);
                    command.Parameters.AddWithValue("@Mexample", string.Empty);
                    command.Parameters.AddWithValue("@Mcategory", category);
                    command.Parameters.AddWithValue("@Microworld", false);
                    command.Parameters.AddWithValue("@Mback", backUrl ?? string.Empty);
                    command.Parameters.AddWithValue("@Mhelp", false);
                    missionId = Convert.ToInt32(command.ExecuteScalar());
                }
            }

            UpdateMission(connection, transaction, missionId, title, content, upload, category, fileType, backUrl, publishToStudents);
            return missionId;
        }

        private static void UpdateMission(SqlConnection connection, SqlTransaction transaction, int missionId, string title, string content, bool upload, int category, string fileType, string backUrl, bool publishToStudents)
        {
            using (SqlCommand command = new SqlCommand("update Mission set Mtitle=@Mtitle,Mpublish=@Mpublish,Mupload=@Mupload,Mcategory=@Mcategory,Mcontent=@Mcontent,Mfiletype=@Mfiletype,Mback=@Mback where Mid=@Mid", connection, transaction))
            {
                command.Parameters.AddWithValue("@Mid", missionId);
                command.Parameters.AddWithValue("@Mtitle", title);
                command.Parameters.AddWithValue("@Mpublish", publishToStudents);
                command.Parameters.AddWithValue("@Mupload", upload);
                command.Parameters.AddWithValue("@Mcategory", category);
                command.Parameters.AddWithValue("@Mcontent", content ?? string.Empty);
                command.Parameters.AddWithValue("@Mfiletype", fileType ?? string.Empty);
                command.Parameters.AddWithValue("@Mback", backUrl ?? string.Empty);
                command.ExecuteNonQuery();
            }
        }

        private static int ResolveOrCreateListMenu(SqlConnection connection, SqlTransaction transaction, int cid, int? existingListMenuId, int xid, string title, int ltype, bool publishToStudents, ref int nextListSort)
        {
            int listMenuId = existingListMenuId.HasValue && ListMenuBelongsToCourse(connection, transaction, existingListMenuId.Value, cid)
                ? existingListMenuId.Value
                : 0;
            if (listMenuId <= 0)
            {
                using (SqlCommand command = new SqlCommand(@"insert into ListMenu(Lcid,Lsort,Ltype,Lxid,Lshow,Ltitle)
values(@Lcid,@Lsort,@Ltype,@Lxid,@Lshow,@Ltitle);
select cast(scope_identity() as int);", connection, transaction))
                {
                    command.Parameters.AddWithValue("@Lcid", cid);
                    command.Parameters.AddWithValue("@Lsort", nextListSort++);
                    command.Parameters.AddWithValue("@Ltype", ltype);
                    command.Parameters.AddWithValue("@Lxid", xid);
                    command.Parameters.AddWithValue("@Lshow", publishToStudents);
                    command.Parameters.AddWithValue("@Ltitle", title);
                    listMenuId = Convert.ToInt32(command.ExecuteScalar());
                }
            }

            UpdateListMenu(connection, transaction, listMenuId, cid, xid, title, ltype, publishToStudents);
            return listMenuId;
        }

        private static int ResolveOrCreateExamPaper(SqlConnection connection, SqlTransaction transaction, int hid, int? existingPaperId, string paperTitle, string questionSummary, int duration)
        {
            int paperId = existingPaperId.HasValue && ExamPaperExists(connection, transaction, existingPaperId.Value)
                ? existingPaperId.Value
                : 0;
            if (paperId <= 0)
            {
                using (SqlCommand command = new SqlCommand(@"insert into ExamPaper(PaperCode,PaperName,PaperType,TotalScore,PassScore,QuestionCount,Duration,Description,Status,CreateBy,CreateTime)
values(@PaperCode,@PaperName,@PaperType,@TotalScore,@PassScore,@QuestionCount,@Duration,@Description,@Status,@CreateBy,@CreateTime);
select cast(scope_identity() as int);", connection, transaction))
                {
                    command.Parameters.AddWithValue("@PaperCode", Guid.NewGuid().ToString("N").Substring(0, 6).ToUpperInvariant());
                    command.Parameters.AddWithValue("@PaperName", paperTitle);
                    command.Parameters.AddWithValue("@PaperType", 1);
                    command.Parameters.AddWithValue("@TotalScore", 100m);
                    command.Parameters.AddWithValue("@PassScore", 60m);
                    command.Parameters.AddWithValue("@QuestionCount", 0);
                    command.Parameters.AddWithValue("@Duration", duration <= 0 ? 10 : duration);
                    command.Parameters.AddWithValue("@Description", questionSummary ?? string.Empty);
                    command.Parameters.AddWithValue("@Status", 0);
                    command.Parameters.AddWithValue("@CreateBy", hid.ToString());
                    command.Parameters.AddWithValue("@CreateTime", DateTime.Now);
                    paperId = Convert.ToInt32(command.ExecuteScalar());
                }
            }

            using (SqlCommand command = new SqlCommand("update ExamPaper set PaperName=@PaperName,Duration=@Duration,Description=@Description,UpdateTime=@UpdateTime where PaperId=@PaperId", connection, transaction))
            {
                command.Parameters.AddWithValue("@PaperId", paperId);
                command.Parameters.AddWithValue("@PaperName", paperTitle);
                command.Parameters.AddWithValue("@Duration", duration <= 0 ? 10 : duration);
                command.Parameters.AddWithValue("@Description", questionSummary ?? string.Empty);
                command.Parameters.AddWithValue("@UpdateTime", DateTime.Now);
                command.ExecuteNonQuery();
            }

            return paperId;
        }

        private static int ResolveOrCreateExam(SqlConnection connection, SqlTransaction transaction, LearnSite.Model.AIActivityPlanPublishRequest request, FullLessonDraftBlock block, int? existingExamId, int paperId, int teacherClassId)
        {
            int examId = existingExamId.HasValue && ExamBelongsToTeacher(connection, transaction, existingExamId.Value, request.Hid)
                ? existingExamId.Value
                : 0;
            if (examId <= 0)
            {
                using (SqlCommand command = new SqlCommand(@"insert into Exam(ExamCode,ExamName,PaperId,ExamType,TimeMode,StartTime,EndTime,ValidDays,Duration,LateMinutes,ShowAnswer,ShowScore,ShowRank,Password,ParticipantType,Participants,AntiCheat,Status,CreateBy,CreateTime)
values(@ExamCode,@ExamName,@PaperId,@ExamType,@TimeMode,@StartTime,@EndTime,@ValidDays,@Duration,@LateMinutes,@ShowAnswer,@ShowScore,@ShowRank,@Password,@ParticipantType,@Participants,@AntiCheat,@Status,@CreateBy,@CreateTime);
select cast(scope_identity() as int);", connection, transaction))
                {
                    DateTime startTime = DateTime.Now;
                    DateTime endTime = startTime.AddYears(1);
                    command.Parameters.AddWithValue("@ExamCode", Guid.NewGuid().ToString("N").Substring(0, 8).ToUpperInvariant());
                    command.Parameters.AddWithValue("@ExamName", block.Quiz.ExamName);
                    command.Parameters.AddWithValue("@PaperId", paperId);
                    command.Parameters.AddWithValue("@ExamType", 3);
                    command.Parameters.AddWithValue("@TimeMode", 2);
                    command.Parameters.AddWithValue("@StartTime", startTime);
                    command.Parameters.AddWithValue("@EndTime", endTime);
                    command.Parameters.AddWithValue("@ValidDays", 365);
                    command.Parameters.AddWithValue("@Duration", block.Quiz.Duration <= 0 ? 10 : block.Quiz.Duration);
                    command.Parameters.AddWithValue("@LateMinutes", 0);
                    command.Parameters.AddWithValue("@ShowAnswer", 0);
                    command.Parameters.AddWithValue("@ShowScore", 1);
                    command.Parameters.AddWithValue("@ShowRank", 0);
                    command.Parameters.AddWithValue("@Password", string.Empty);
                    command.Parameters.AddWithValue("@ParticipantType", teacherClassId > 0 ? 1 : 2);
                    command.Parameters.AddWithValue("@Participants", teacherClassId > 0 ? teacherClassId.ToString() : string.Empty);
                    command.Parameters.AddWithValue("@AntiCheat", string.Empty);
                    command.Parameters.AddWithValue("@Status", request.PublishToStudents ? 1 : 0);
                    command.Parameters.AddWithValue("@CreateBy", request.Hid.ToString());
                    command.Parameters.AddWithValue("@CreateTime", DateTime.Now);
                    examId = Convert.ToInt32(command.ExecuteScalar());
                }
            }

            using (SqlCommand command = new SqlCommand(@"update Exam set ExamName=@ExamName,PaperId=@PaperId,Duration=@Duration,ParticipantType=@ParticipantType,Participants=@Participants,Status=@Status,UpdateTime=@UpdateTime,UpdateBy=@UpdateBy where ExamId=@ExamId", connection, transaction))
            {
                command.Parameters.AddWithValue("@ExamId", examId);
                command.Parameters.AddWithValue("@ExamName", block.Quiz.ExamName);
                command.Parameters.AddWithValue("@PaperId", paperId);
                command.Parameters.AddWithValue("@Duration", block.Quiz.Duration <= 0 ? 10 : block.Quiz.Duration);
                command.Parameters.AddWithValue("@ParticipantType", teacherClassId > 0 ? 1 : 2);
                command.Parameters.AddWithValue("@Participants", teacherClassId > 0 ? teacherClassId.ToString() : string.Empty);
                command.Parameters.AddWithValue("@Status", request.PublishToStudents ? 1 : 0);
                command.Parameters.AddWithValue("@UpdateTime", DateTime.Now);
                command.Parameters.AddWithValue("@UpdateBy", request.Hid.ToString());
                command.ExecuteNonQuery();
            }

            return examId;
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
            UpdateMission(connection, transaction, missionId, missionTitle, missionContent, true, 0, "office", string.Empty, publishToStudents);
        }

        private static void UpdateListMenu(SqlConnection connection, SqlTransaction transaction, int listMenuId, int cid, int xid, string title, int ltype, bool publishToStudents)
        {
            using (SqlCommand command = new SqlCommand("update ListMenu set Lcid=@Lcid,Ltype=@Ltype,Lxid=@Lxid,Lshow=@Lshow,Ltitle=@Ltitle where Lid=@Lid", connection, transaction))
            {
                command.Parameters.AddWithValue("@Lid", listMenuId);
                command.Parameters.AddWithValue("@Lcid", cid);
                command.Parameters.AddWithValue("@Ltype", ltype);
                command.Parameters.AddWithValue("@Lxid", xid);
                command.Parameters.AddWithValue("@Lshow", publishToStudents);
                command.Parameters.AddWithValue("@Ltitle", title);
                command.ExecuteNonQuery();
            }
        }

        private static void SaveSingleDraftLinks(SqlConnection connection, SqlTransaction transaction, LearnSite.Model.AIActivityPlanPublishRequest request, string updatedCourseContent, int missionId, int listMenuId)
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

            UpsertDraftRecord(connection, transaction, draftRecord, missionId, listMenuId);
        }

        private static void SaveFullLessonDraftLinks(SqlConnection connection, SqlTransaction transaction, LearnSite.Model.AIActivityPlanPublishRequest request, string updatedCourseContent, IDictionary<string, ActivityPlanPublishLinkPayload> publishLinks, IList<LearnSite.Model.AIActivityPlanPublishedBlockResult> publishedBlocks)
        {
            int? linkedMissionId = null;
            int? linkedListMenuId = null;
            if (publishedBlocks != null)
            {
                for (int i = 0; i < publishedBlocks.Count; i++)
                {
                    if (!linkedMissionId.HasValue && publishedBlocks[i].MissionId.HasValue)
                    {
                        linkedMissionId = publishedBlocks[i].MissionId.Value;
                    }

                    if (!linkedListMenuId.HasValue && publishedBlocks[i].ListMenuId > 0)
                    {
                        linkedListMenuId = publishedBlocks[i].ListMenuId;
                    }
                }
            }

            LearnSite.Model.CourseActivityPlanDraft draftRecord = AIActivityPlanSavedDraftHelper.BuildFullLessonRecord(
                request.Cid,
                request.Hid,
                request.Topic,
                string.Empty,
                string.Empty,
                string.Empty,
                updatedCourseContent,
                request.FullLessonDraft,
                linkedMissionId,
                linkedListMenuId,
                publishLinks);
            if (draftRecord == null)
            {
                return;
            }

            UpsertDraftRecord(connection, transaction, draftRecord, linkedMissionId, linkedListMenuId);
        }

        private static void UpsertDraftRecord(SqlConnection connection, SqlTransaction transaction, LearnSite.Model.CourseActivityPlanDraft draftRecord, int? linkedMissionId, int? linkedListMenuId)
        {
            using (SqlCommand command = new SqlCommand(@"if exists(select 1 from CourseActivityPlanDraft where Cid=@Cid and Hid=@Hid)
update CourseActivityPlanDraft set Topic=@Topic,Grade=@Grade,Duration=@Duration,TeachingGoalsInput=@TeachingGoalsInput,ExistingCourseContentSnapshot=@ExistingCourseContentSnapshot,DraftJson=@DraftJson,LinkedMissionId=@LinkedMissionId,LinkedListMenuId=@LinkedListMenuId,UpdatedAt=@UpdatedAt where Cid=@Cid and Hid=@Hid
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
                command.Parameters.AddWithValue("@LinkedMissionId", linkedMissionId.HasValue ? (object)linkedMissionId.Value : DBNull.Value);
                command.Parameters.AddWithValue("@LinkedListMenuId", linkedListMenuId.HasValue ? (object)linkedListMenuId.Value : DBNull.Value);
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

        private static bool ExamBelongsToTeacher(SqlConnection connection, SqlTransaction transaction, int examId, int hid)
        {
            using (SqlCommand command = new SqlCommand("select count(1) from Exam where ExamId=@ExamId and CreateBy=@CreateBy", connection, transaction))
            {
                command.Parameters.AddWithValue("@ExamId", examId);
                command.Parameters.AddWithValue("@CreateBy", hid.ToString());
                return Convert.ToInt32(command.ExecuteScalar()) > 0;
            }
        }

        private static bool ExamPaperExists(SqlConnection connection, SqlTransaction transaction, int paperId)
        {
            using (SqlCommand command = new SqlCommand("select count(1) from ExamPaper where PaperId=@PaperId", connection, transaction))
            {
                command.Parameters.AddWithValue("@PaperId", paperId);
                return Convert.ToInt32(command.ExecuteScalar()) > 0;
            }
        }

        private static int LoadFirstTeacherClassId(SqlConnection connection, SqlTransaction transaction, int hid)
        {
            using (SqlCommand command = new SqlCommand("select top 1 Rid from Room where Rhid=@Hid order by Rid", connection, transaction))
            {
                command.Parameters.AddWithValue("@Hid", hid);
                object value = command.ExecuteScalar();
                if (value == null || value == DBNull.Value)
                {
                    return 0;
                }

                return Convert.ToInt32(value);
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

        private static bool IsSupportedPublishBlock(FullLessonDraftBlock block)
        {
            if (block == null || string.IsNullOrEmpty((block.BlockKey ?? string.Empty).Trim()))
            {
                return false;
            }

            string blockType = NormalizeBlockType(block.BlockType);
            if (blockType == "mission")
            {
                return true;
            }

            if (blockType == "guidedinquiry")
            {
                return block.GuidedInquiry != null && AIActivityPlanDraftHelper.IsValidGuidedInquiryPayload(block.GuidedInquiry);
            }

            if (blockType == "resource-study")
            {
                return block.ResourceStudy != null && AIActivityPlanDraftHelper.IsValidResourceStudyPayload(block.ResourceStudy);
            }

            if (blockType == "webcourseware")
            {
                return block.WebCourseware != null && AIActivityPlanDraftHelper.IsValidWebCoursewarePayload(block.WebCourseware);
            }

            if (blockType == "quiz")
            {
                return block.Quiz != null && AIActivityPlanDraftHelper.IsValidQuizPayload(block.Quiz);
            }

            return false;
        }

        private static string NormalizeBlockType(string blockType)
        {
            return (blockType ?? string.Empty).Trim().ToLowerInvariant();
        }

        private static ActivityPlanPublishLinkPayload GetPublishLink(IDictionary<string, ActivityPlanPublishLinkPayload> publishLinks, string blockKey)
        {
            if (publishLinks == null || string.IsNullOrEmpty(blockKey))
            {
                return null;
            }

            ActivityPlanPublishLinkPayload link;
            return publishLinks.TryGetValue(blockKey, out link) ? link : null;
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

            public string Topic { get; set; }

            public string Grade { get; set; }

            public string Duration { get; set; }

            public string TeachingGoalsInput { get; set; }

            public string ExistingCourseContentSnapshot { get; set; }

            public FullLessonDraft FullLessonDraft { get; set; }

            public Dictionary<string, ActivityPlanPublishLinkPayload> PublishLinks { get; set; }

            public DraftLinkSnapshot()
            {
                PublishLinks = new Dictionary<string, ActivityPlanPublishLinkPayload>(StringComparer.OrdinalIgnoreCase);
            }
        }
    }
}
