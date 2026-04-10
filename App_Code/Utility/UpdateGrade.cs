using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Text;
using System.Web.Security;
using System.Collections;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
namespace LearnSite.DBUtility
{
    /// <summary>
    ///UpdateGrade 的摘要说明
    /// </summary>
    public class UpdateGrade
    {
        public UpdateGrade()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }
        /// <summary>
        /// 版本更新检测，false为未更新
        /// 每次更新数据库时，请更改检测的表或字段
        /// </summary>
        /// <returns></returns>
        public static bool VersionCheck()
        {
            if (DbHelperSQL.TableCounts("English") < 10)
                return false;
            else
                return TableCheck();//检测数据库表是否完整并最新
        }

        public static string GetTargetVersion()
        {
            return "1913";
        }

        public static string GetCurrentVersion()
        {
            try
            {
                if (DbHelperSQL.TabExists("CourseActivityPlanDraft") && DbHelperSQL.ColumnExists("Survey", "Venableai") && HasCoreExamTables()) return "1913";
                if (DbHelperSQL.ColumnExists("Survey", "Venableai") && HasCoreExamTables()) return "1912";
                if (DbHelperSQL.TabExists("AIStudentExamAssessment") && HasCoreExamTables()) return "1911";
                if (DbHelperSQL.ColumnExists("MenuWorks", "Kseconds")) return "1910";
                if (DbHelperSQL.TabExists("AICustomSkill")) return "1900";
                if (DbHelperSQL.TabExists("AISkill")) return "1800";
                if (DbHelperSQL.TabExists("AIProvider")) return "1700";
                if (DbHelperSQL.TabExists("Answers")) return "1600";
                if (DbHelperSQL.TabExists("Folders")) return "1500";
                if (DbHelperSQL.ColumnExists("Mission", "Microworld")) return "1365";
                return "1.10以下";
            }
            catch
            {
                return "未知";
            }
        }
        public static bool TableExistCheck()
        {
            string CheckTabel = "Students";
            return DbHelperSQL.TabExists(CheckTabel);
        }
        /// <summary>
        /// 检测数据库表是否完整并最新
        /// </summary>
        /// <returns></returns>
        public static bool TableCheck()
        {
            //如果数据库不为空
            if (DBUtility.SqlHelper.CountTable() > 0)
            {
                string CheckTabel = "Answers";//Folders
                string CheckField = "Aid";//这里是每次字段增加时的判断入口FolderId
                if (DbHelperSQL.TabExists(CheckTabel))
                {
                    try
                    {
                        if (!DbHelperSQL.TabExists("AISkill")) return false;
                        if (!DbHelperSQL.TabExists("AICustomSkill")) return false;
                        if (!HasCoreExamTables()) return false;
                        if (!DbHelperSQL.ColumnExists("MenuWorks", "Kseconds")) return false;
                        if (!DbHelperSQL.ColumnExists("Survey", "Venableai")) return false;
                        if (!DbHelperSQL.TabExists("CourseActivityPlanDraft")) return false;
                        return DbHelperSQL.ColumnExists(CheckTabel, CheckField);
                    }
                    catch
                    {
                        return false;
                    }
                }
                else
                    return false;//如表不存在，则更新
            }
            else
                return false;
        }

        private static bool HasCoreExamTables()
        {
            return DbHelperSQL.TabExists("Exam")
                && DbHelperSQL.TabExists("ExamPaper")
                && DbHelperSQL.TabExists("ExamQuestion")
                && DbHelperSQL.TabExists("ExamPaperQuestion")
                && DbHelperSQL.TabExists("ExamAnswer")
                && DbHelperSQL.TabExists("ExamResult");
        }

        public static void updateDatabase()
        {
            string uptable = "Works";
            string upWself = "Wself";
            string upWcan = "Wcan";
            string upWgood = "Wgood";
            if (!DbHelperSQL.ColumnExists(uptable, upWself))
            {
                DbHelperSQL.AddColumn(uptable, upWself, "nvarchar(200)", -1);
            }
            if (!DbHelperSQL.ColumnExists(uptable, upWcan))
            {
                DbHelperSQL.AddColumn(uptable, upWcan, "bit", 1);
            }
            if (!DbHelperSQL.ColumnExists(uptable, upWgood))
            {
                DbHelperSQL.AddColumn(uptable, upWgood, "bit", 0);
                string mysql = "update Works set Wcan=1,Wgood=0";
                DbHelperSQL.ExecuteSql(mysql);
            }
        }

        public static void CreateNewTable()
        {
            if (!DbHelperSQL.TabExists("WorksDiscuss"))
            {
                StringBuilder astr = new StringBuilder();
                astr.Append(" create table WorksDiscuss (");
                astr.Append(" Did int  IDENTITY (1, 1)  primary key not null, ");
                astr.Append(" Dwid int,");
                astr.Append(" Dsnum nvarchar(50),");
                astr.Append(" Dwords NVARCHAR(MAX),");
                astr.Append(" Dtime datetime,");
                astr.Append(" Dip nvarchar(50)");
                astr.Append(" )");

                DbHelperSQL.ExecuteSql(astr.ToString());
            }
        }

        public static void UpdateTable105()
        {
            string studentstable = "Students";
            string Swscore = "Swscore";
            string Stscore = "Stscore";
            string Sallscore = "Sallscore";
            if (!DbHelperSQL.ColumnExists(studentstable, Swscore))
            {
                DbHelperSQL.AddColumn(studentstable, Swscore, "int", 0);
            }
            if (!DbHelperSQL.ColumnExists(studentstable, Stscore))
            {
                DbHelperSQL.AddColumn(studentstable, Stscore, "int", 0);
            }
            if (!DbHelperSQL.ColumnExists(studentstable, Sallscore))
            {
                DbHelperSQL.AddColumn(studentstable, Sallscore, "int", 0);
            }

            string Ptypertable = "Ptyper";
            string Pdegree = "Pdegree";
            if (!DbHelperSQL.ColumnExists(Ptypertable, Pdegree))
            {
                DbHelperSQL.AddColumn(Ptypertable, Pdegree, "int", 0);
            }

            string Quiztable = "Quiz";
            string Qclass = "Qclass";
            string Qselect = "Qselect";
            if (!DbHelperSQL.ColumnExists(Quiztable, Qclass))
            {
                DbHelperSQL.AddColumn(Quiztable, Qclass, "nvarchar(50)", -1);
            }
            if (!DbHelperSQL.ColumnExists(Quiztable, Qselect))
            {
                DbHelperSQL.AddColumn(Quiztable, Qselect, "bit", 0);
            }
        }
        public static void UpdateTable106()
        {
            if (!DbHelperSQL.TabExists("QuizGrade"))
            {
                StringBuilder astr = new StringBuilder();
                astr.Append(" create table QuizGrade (");
                astr.Append(" Qid int  IDENTITY (1, 1)  primary key not null, ");
                astr.Append(" Qobj int DEFAULT 0,");
                astr.Append(" Qclass NVARCHAR(MAX)");
                astr.Append(" )");

                DbHelperSQL.ExecuteSql(astr.ToString());
            }

            string WorksTable = "Works";
            string Wtypestr = "Wtype";
            if (!DbHelperSQL.ColumnExists(WorksTable, Wtypestr))
            {
                DbHelperSQL.AddColumn(WorksTable, Wtypestr, "nvarchar(50)", -1);
                string mysql = "update Works set Wtype=Mfiletype  from Works,Mission where Wmid=Mid";
                DbHelperSQL.ExecuteSql(mysql);//更新增加的字段Wtpye初始值
            }

        }

        public static void UpdateTable107()
        {
            if (!DbHelperSQL.TabExists("NotSign"))
            {
                StringBuilder astr = new StringBuilder();
                astr.Append(" create table NotSign (");
                astr.Append(" Nid int  IDENTITY (1, 1)  primary key not null, ");
                astr.Append(" Nnum nvarchar(50),");
                astr.Append(" Ndate datetime,");
                astr.Append(" Nyear int,");
                astr.Append(" Nmonth int,");
                astr.Append(" Nday int,");
                astr.Append(" Nweek nvarchar(50),");
                astr.Append(" Nnote NVARCHAR(MAX)");
                astr.Append(" )");

                DbHelperSQL.ExecuteSql(astr.ToString());
            }
        }
        /// <summary>
        /// 将原学案类型Html改为Htm，活动类型html改为htm
        /// </summary>
        public static void UpdateHtmlToHtm()
        {
            string mysql = "update Courses set Cclass='Htm' where Cclass='Html'";
            string sqlstr = "update Mission set Mfiletype='htm' where Mfiletype='html'";
            DbHelperSQL.ExecuteSql(mysql);
            DbHelperSQL.ExecuteSql(sqlstr);
        }
        public static void UpdateTable108()
        {
            if (!DbHelperSQL.TabExists("TopicDiscuss"))
            {
                StringBuilder astr = new StringBuilder();
                astr.Append(" create table TopicDiscuss (");
                astr.Append(" Tid int  IDENTITY (1, 1)  primary key not null, ");
                astr.Append(" Tcid int,");
                astr.Append(" Ttitle nvarchar(50),");
                astr.Append(" Tcontent NVARCHAR(MAX),");
                astr.Append(" Tcount int DEFAULT 0,");
                astr.Append(" Tteacher int,");
                astr.Append(" Tdate datetime,");
                astr.Append(" Tclose bit DEFAULT 0,");
                astr.Append(" Tresult NVARCHAR(MAX)");
                astr.Append(" )");

                DbHelperSQL.ExecuteSql(astr.ToString());
            }

            if (!DbHelperSQL.TabExists("TopicReply"))
            {
                StringBuilder bastr = new StringBuilder();
                bastr.Append(" create table TopicReply (");
                bastr.Append(" Rid int  IDENTITY (1, 1)  primary key not null, ");
                bastr.Append(" Rtid int,");
                bastr.Append(" Rsnum nvarchar(50),");
                bastr.Append(" Rwords NVARCHAR(MAX),");
                bastr.Append(" Rtime datetime,");
                bastr.Append(" Rip nvarchar(50),");
                bastr.Append(" Rscore int,");
                bastr.Append(" Rban bit DEFAULT 0,");
                bastr.Append(" Rgrade int,");
                bastr.Append(" Rterm int");
                bastr.Append(" )");

                DbHelperSQL.ExecuteSql(bastr.ToString());
            }
            string TopicReplyTable = "TopicReply";
            string Rgradestr = "Rgrade";
            string Rtermstr = "Rterm";
            if (!DbHelperSQL.ColumnExists(TopicReplyTable, Rgradestr))
            {
                DbHelperSQL.AddColumn(TopicReplyTable, Rgradestr, "int", -1);
            }

            if (!DbHelperSQL.ColumnExists(TopicReplyTable, Rtermstr))
            {
                DbHelperSQL.AddColumn(TopicReplyTable, Rtermstr, "int", -1);
            }

            string SignTable = "Signin";
            string Qgradestr = "Qgrade";
            string Qtermstr = "Qterm";
            if (!DbHelperSQL.ColumnExists(SignTable, Qgradestr))
            {
                DbHelperSQL.AddColumn(SignTable, Qgradestr, "int", -1);
            }

            if (!DbHelperSQL.ColumnExists(SignTable, Qtermstr))
            {
                DbHelperSQL.AddColumn(SignTable, Qtermstr, "int", -1);
            }

            if (!DbHelperSQL.TabExists("Computers"))
            {
                StringBuilder castr = new StringBuilder();
                castr.Append(" create table Computers (");
                castr.Append(" Pid int  IDENTITY (1, 1)  primary key not null, ");
                castr.Append(" Pip nvarchar(50),");
                castr.Append(" Pmachine nvarchar(50),");
                castr.Append(" Plock bit DEFAULT 0,");
                castr.Append(" Pdate datetime");
                castr.Append(" )");

                DbHelperSQL.ExecuteSql(castr.ToString());
            }
            string SoftTabel = "Soft";
            string Fhide = "Fhide";
            if (!DbHelperSQL.ColumnExists(SoftTabel, Fhide))
            {
                DbHelperSQL.AddColumn(SoftTabel, Fhide, "bit", 0);
                string mysql = "update Soft set Fhide=0 where Fhide is null";
                DbHelperSQL.ExecuteSql(mysql);
            }
        }

        public static void UpdateTable1081()
        {
            string WorksTable = "Works";
            string Wgradestr = "Wgrade";
            string Wtermstr = "Wterm";
            string Whitstr = "Whit";
            string Wlscorestr = "Wlscore";
            string Wlemotionstr = "Wlemotion";
            if (!DbHelperSQL.ColumnExists(WorksTable, Wgradestr))
            {
                DbHelperSQL.AddColumn(WorksTable, Wgradestr, "int", -1);
            }

            if (!DbHelperSQL.ColumnExists(WorksTable, Wtermstr))
            {
                DbHelperSQL.AddColumn(WorksTable, Wtermstr, "int", -1);
            }
            if (!DbHelperSQL.ColumnExists(WorksTable, Whitstr))
            {
                DbHelperSQL.AddColumn(WorksTable, Whitstr, "int", 0);
            }
            if (!DbHelperSQL.ColumnExists(WorksTable, Wlscorestr))
            {
                DbHelperSQL.AddColumn(WorksTable, Wlscorestr, "int", 0);
            }
            if (!DbHelperSQL.ColumnExists(WorksTable, Wlemotionstr))
            {
                DbHelperSQL.AddColumn(WorksTable, Wlemotionstr, "int", 0);
            }

            string RoomTable = "Room";
            string Rlockstr = "Rlock";
            if (!DbHelperSQL.ColumnExists(RoomTable, Rlockstr))
            {
                DbHelperSQL.AddColumn(RoomTable, Rlockstr, "bit", 0);
                LearnSite.BLL.Room rbll = new BLL.Room();
                rbll.InitLock();//字段为null时更新为false
            }

            string MissionTable = "Mission";
            string Mgroupstr = "Mgroup";
            if (!DbHelperSQL.ColumnExists(MissionTable, Mgroupstr))
            {
                DbHelperSQL.AddColumn(MissionTable, Mgroupstr, "bit", 0);
                LearnSite.BLL.Mission mbll = new BLL.Mission();
                mbll.InitMgroup();//初始化Mgroup字段为null时更新为false
            }

            string StudentsTable = "Students";
            string Spscorestr = "Spscore";
            string Sgroupstr = "Sgroup";
            string Sleaderstr = "Sleader";
            string Svotestr = "Svote";

            if (!DbHelperSQL.ColumnExists(StudentsTable, Spscorestr))
            {
                DbHelperSQL.AddColumn(StudentsTable, Spscorestr, "int", 0);
            }

            if (!DbHelperSQL.ColumnExists(StudentsTable, Sgroupstr))
            {
                DbHelperSQL.AddColumn(StudentsTable, Sgroupstr, "int", -1);
            }
            if (!DbHelperSQL.ColumnExists(StudentsTable, Sleaderstr))
            {
                DbHelperSQL.AddColumn(StudentsTable, Sleaderstr, "bit", 0);

                LearnSite.BLL.Students sbll = new BLL.Students();
                sbll.InitSleader();//初始化Sleader字段原null为false
            }
            if (!DbHelperSQL.ColumnExists(StudentsTable, Svotestr))
            {
                DbHelperSQL.AddColumn(StudentsTable, Svotestr, "int", 0);
            }
        }

        public static void UpdateTable1082()
        {
            string SoftTabel = "Soft";
            string Fopen = "Fopen";
            if (!DbHelperSQL.ColumnExists(SoftTabel, Fopen))
            {
                DbHelperSQL.AddColumn(SoftTabel, Fopen, "int", 0);
                string mysql = "update Soft set Fopen=0 where Fopen is null";
                DbHelperSQL.ExecuteSql(mysql);
            }

            if (!DbHelperSQL.TabExists("TermTotal"))
            {
                StringBuilder tastr = new StringBuilder();
                tastr.Append(" create table TermTotal (");
                tastr.Append(" Tid int  IDENTITY (1, 1)  primary key not null, ");
                tastr.Append(" Tnum nvarchar(50),");
                tastr.Append(" Tterm int,");
                tastr.Append(" Tgrade int,");
                tastr.Append(" Tscore int,");
                tastr.Append(" Tgscore int,");
                tastr.Append(" Tquiz int,");
                tastr.Append(" Tattitude int,");
                tastr.Append(" Twscore int,");
                tastr.Append(" Ttscore int,");
                tastr.Append(" Tpscore int,");
                tastr.Append(" Tallscore int,");
                tastr.Append(" Tape nvarchar(1)");
                tastr.Append(" )");

                DbHelperSQL.ExecuteSql(tastr.ToString());
            }
            string stustable = "Students";
            string Sgscore = "Sgscore";
            if (!DbHelperSQL.ColumnExists(stustable, Sgscore))
            {
                DbHelperSQL.AddColumn(stustable, Sgscore, "int", 0);
            }

            if (!DbHelperSQL.TabExists("GroupWork"))
            {
                StringBuilder gastr = new StringBuilder();
                gastr.Append(" create table GroupWork (");
                gastr.Append(" Gid int  IDENTITY (1, 1)  primary key not null, ");
                gastr.Append(" Gnum nvarchar(50),");
                gastr.Append(" Gstudents nvarchar(200),");
                gastr.Append(" Gterm int,");
                gastr.Append(" Ggrade int,");
                gastr.Append(" Gclass int,");
                gastr.Append(" Gcid int,");
                gastr.Append(" Gmid int,");
                gastr.Append(" Gfilename nvarchar(50),");
                gastr.Append(" Gtype nvarchar(50),");
                gastr.Append(" Gurl nvarchar(50),");
                gastr.Append(" Glengh int,");
                gastr.Append(" Gscore int,");
                gastr.Append(" Gtime int,");
                gastr.Append(" Gvote int,");
                gastr.Append(" Gcheck bit DEFAULT 0,");
                gastr.Append(" Gnote NVARCHAR(MAX),");
                gastr.Append(" Grank int,");
                gastr.Append(" Ghit int DEFAULT 0,");
                gastr.Append(" Gip nvarchar(50),");
                gastr.Append(" Gdate datetime");
                gastr.Append(" )");

                DbHelperSQL.ExecuteSql(gastr.ToString());
            }
        }

        public static string UpdateTableEnglish()
        {
            string msg = "已经更新过了！";
            if (!DbHelperSQL.TabExists("English"))
            {
                StringBuilder estr = new StringBuilder();
                estr.Append(" create table English (");
                estr.Append(" Eid int  IDENTITY (1, 1)  primary key not null, ");
                estr.Append(" Eword nvarchar(50),");
                estr.Append(" Emeaning NVARCHAR(MAX),");
                estr.Append(" Elevel int");
                estr.Append(" )");

                DbHelperSQL.ExecuteSql(estr.ToString());
            }
            if (DbHelperSQL.TableCounts("English") < 10)
            {
                msg = LearnSite.Common.DataExcel.DataSettoEnglish();//从Excel中导入到English表中
            }
            return msg;
        }

        public static void UpdateTable1092()
        {
            if (!DbHelperSQL.TabExists("Pfinger"))
            {
                StringBuilder Pstr = new StringBuilder();
                Pstr.Append(" create table Pfinger (");
                Pstr.Append(" Pid int  IDENTITY (1, 1)  primary key not null, ");
                Pstr.Append(" Psnum nvarchar(50),");
                Pstr.Append(" Pspd decimal(18, 2),");
                Pstr.Append(" Pyear int,");
                Pstr.Append(" Pmonth int,");
                Pstr.Append(" Pdate datetime,");
                Pstr.Append(" Pdegree int");
                Pstr.Append(" )");

                DbHelperSQL.ExecuteSql(Pstr.ToString());
            }
        }

        public static void UpdateTable1093()
        {
            string RoomTable = "Room";
            string Rlockstr = "Rip";
            if (!DbHelperSQL.ColumnExists(RoomTable, Rlockstr))
            {
                DbHelperSQL.AddColumn(RoomTable, Rlockstr, "nvarchar(50)", -1);
            }
        }

        public static void UpdateTable1094()
        {
            string SigninTable = "Signin";
            string Qmachinestr = "Qmachine";
            if (!DbHelperSQL.ColumnExists(SigninTable, Qmachinestr))
            {
                DbHelperSQL.AddColumn(SigninTable, Qmachinestr, "nvarchar(50)", -1);
            }
        }

        public static void UpdateTable1095()
        {
            string QuizGradeTable = "QuizGrade";
            string Qhidstr = "Qhid";
            string Qonlystr = "Qonly";
            string Qmorestr = "Qmore";
            string Qjudgestr = "Qjudge";
            string Qopenstr = "Qopen";
            string Qanswerstr = "Qanswer";
            if (!DbHelperSQL.ColumnExists(QuizGradeTable, Qhidstr))
            {
                DbHelperSQL.AddColumn(QuizGradeTable, Qhidstr, "int", -1);
            }
            if (!DbHelperSQL.ColumnExists(QuizGradeTable, Qonlystr))
            {
                DbHelperSQL.AddColumn(QuizGradeTable, Qonlystr, "int", -1);
            }
            if (!DbHelperSQL.ColumnExists(QuizGradeTable, Qmorestr))
            {
                DbHelperSQL.AddColumn(QuizGradeTable, Qmorestr, "int", -1);
            }
            if (!DbHelperSQL.ColumnExists(QuizGradeTable, Qjudgestr))
            {
                DbHelperSQL.AddColumn(QuizGradeTable, Qjudgestr, "int", -1);
            }
            if (!DbHelperSQL.ColumnExists(QuizGradeTable, Qopenstr))
            {
                DbHelperSQL.AddColumn(QuizGradeTable, Qopenstr, "bit", 0);
            }
            if (!DbHelperSQL.ColumnExists(QuizGradeTable, Qanswerstr))
            {
                DbHelperSQL.AddColumn(QuizGradeTable, Qanswerstr, "bit", 1);
            }
            if (!DbHelperSQL.TabExists("Summary"))
            {
                StringBuilder Pstr = new StringBuilder();
                Pstr.Append(" create table Summary (");
                Pstr.Append(" Sid int  IDENTITY (1, 1)  primary key not null, ");
                Pstr.Append(" Scid int,");
                Pstr.Append(" Smid int,");
                Pstr.Append(" Shid int,");
                Pstr.Append(" Scontent NVARCHAR(MAX),");
                Pstr.Append(" Sdate datetime,");
                Pstr.Append(" Sgrade int,");
                Pstr.Append(" Sclass int,");
                Pstr.Append(" Syear int,");
                Pstr.Append(" Sshow bit DEFAULT 0");
                Pstr.Append(" )");

                DbHelperSQL.ExecuteSql(Pstr.ToString());
            }
            string SummaryTable = "Summary";
            string Sshowstr = "Sshow";
            if (!DbHelperSQL.ColumnExists(SummaryTable, Sshowstr))
            {
                DbHelperSQL.AddColumn(SummaryTable, Sshowstr, "bit", 0);
            }
        }

        public static void UpdateTable1096()
        {
            string QuizTable = "Quiz";
            string Qrightstr = "Qright";
            string Qwrongstr = "Qwrong";
            string Qaccuracystr = "Qaccuracy";

            if (!DbHelperSQL.ColumnExists(QuizTable, Qrightstr))
            {
                LearnSite.BLL.QuizGrade qgbll = new BLL.QuizGrade();
                qgbll.DeleteNull();//删除更新前空字段记录

                DbHelperSQL.AddColumn(QuizTable, Qrightstr, "int", 0);
            }
            if (!DbHelperSQL.ColumnExists(QuizTable, Qwrongstr))
            {
                DbHelperSQL.AddColumn(QuizTable, Qwrongstr, "int", 0);
            }
            if (!DbHelperSQL.ColumnExists(QuizTable, Qaccuracystr))
            {
                DbHelperSQL.AddColumn(QuizTable, Qaccuracystr, "int", 0);

                LearnSite.BLL.Quiz qbll = new BLL.Quiz();
                qbll.initQuizRW();
            }

            string ResultTable = "Result";
            string Rhistorystr = "Rhistory";
            string Rwrongstr = "Rwrong";
            if (!DbHelperSQL.ColumnExists(ResultTable, Rhistorystr))
            {
                DbHelperSQL.AddColumn(ResultTable, Rhistorystr, "NVARCHAR(MAX)", -1);
            }
            if (!DbHelperSQL.ColumnExists(ResultTable, Rwrongstr))
            {
                DbHelperSQL.AddColumn(ResultTable, Rwrongstr, "NVARCHAR(MAX)", -1);
            }
        }

        public static void UpdateTable1098()
        {
            string WorksTable = "Works";
            string Wofficestr = "Woffice";
            string Wflashstr = "Wflash";
            string Werrorstr = "Werror";
            if (!DbHelperSQL.ColumnExists(WorksTable, Wofficestr))
            {
                DbHelperSQL.AddColumn(WorksTable, Wofficestr, "bit", 0);
            }
            if (!DbHelperSQL.ColumnExists(WorksTable, Wflashstr))
            {
                DbHelperSQL.AddColumn(WorksTable, Wflashstr, "bit", 0);
            }
            if (!DbHelperSQL.ColumnExists(WorksTable, Werrorstr))
            {
                DbHelperSQL.AddColumn(WorksTable, Werrorstr, "bit", 0);
                LearnSite.BLL.Works wbll = new BLL.Works();
                wbll.UpdateWoffice();
            }
        }

        public static void UpdateTable1100()
        {
            string ResultTable = "Result";
            string Rgradestr = "Rgrade";
            string Rtermstr = "Rterm";
            if (!DbHelperSQL.ColumnExists(ResultTable, Rgradestr))
            {
                DbHelperSQL.AddColumn(ResultTable, Rgradestr, "int", 0);
            }
            if (!DbHelperSQL.ColumnExists(ResultTable, Rtermstr))
            {
                DbHelperSQL.AddColumn(ResultTable, Rtermstr, "int", 0);
            }

            string PfingerTable = "Pfinger";
            string PtyperTable = "Ptyper";
            string Pgradestr = "Pgrade";
            string Ptermstr = "Pterm";
            if (!DbHelperSQL.ColumnExists(PfingerTable, Pgradestr))
            {
                DbHelperSQL.AddColumn(PfingerTable, Pgradestr, "int", 0);
            }
            if (!DbHelperSQL.ColumnExists(PfingerTable, Ptermstr))
            {
                DbHelperSQL.AddColumn(PfingerTable, Ptermstr, "int", 0);
            }
            //两张表，相同的字段
            if (!DbHelperSQL.ColumnExists(PtyperTable, Pgradestr))
            {
                DbHelperSQL.AddColumn(PtyperTable, Pgradestr, "int", 0);
            }
            if (!DbHelperSQL.ColumnExists(PtyperTable, Ptermstr))
            {
                DbHelperSQL.AddColumn(PtyperTable, Ptermstr, "int", 0);
            }
        }

        public static void UpdateTable1101()
        {
            string NotSignTable = "NotSign";
            string Ngradestr = "Ngrade";
            string Ntermstr = "Nterm";
            if (!DbHelperSQL.ColumnExists(NotSignTable, Ngradestr))
            {
                DbHelperSQL.AddColumn(NotSignTable, Ngradestr, "int", -1);
            }
            if (!DbHelperSQL.ColumnExists(NotSignTable, Ntermstr))
            {
                DbHelperSQL.AddColumn(NotSignTable, Ntermstr, "int", -1);

                LearnSite.BLL.NotSign nbll = new BLL.NotSign();
                nbll.upgradefix();//从2011年9月到10月
            }
        }

        public static void UpdateTable1102()
        {
            string stustable = "Students";
            string Sfscore = "Sfscore";//英文指法输入成绩统计字段
            if (!DbHelperSQL.ColumnExists(stustable, Sfscore))
            {
                DbHelperSQL.AddColumn(stustable, Sfscore, "int", 0);
                LearnSite.BLL.Students sbll = new BLL.Students();
                sbll.InitSfscore();
            }

            string TermTotaltable = "TermTotal";
            string Tfscore = "Tfscore";//英文指法输入成绩统计字段
            if (!DbHelperSQL.ColumnExists(TermTotaltable, Tfscore))
            {
                DbHelperSQL.AddColumn(TermTotaltable, Tfscore, "int", 0);
            }
        }

        public static void UpdateTable1103()
        {
            if (!DbHelperSQL.TabExists("DelStudents"))
            {
                StringBuilder Dstr = new StringBuilder();
                Dstr.Append(" create table DelStudents (");
                Dstr.Append(" Did int  IDENTITY (1, 1)  primary key not null, ");
                Dstr.Append(" Dnum nvarchar(50),");
                Dstr.Append(" Dyear int,");
                Dstr.Append(" Dgrade int,");
                Dstr.Append(" Dclass int,");
                Dstr.Append(" Dname nvarchar(50),");
                Dstr.Append(" Dsex nvarchar(2),");
                Dstr.Append(" Daddress nvarchar(200),");
                Dstr.Append(" Dphone nvarchar(50),");
                Dstr.Append(" Dparents nvarchar(50),");
                Dstr.Append(" Dheadtheacher nvarchar(50)");
                Dstr.Append(" )");

                DbHelperSQL.ExecuteSql(Dstr.ToString());
            }
        }

        public static void UpdateTable1105()
        {
            if (!DbHelperSQL.TabExists("Gauge"))
            {
                StringBuilder Gstr = new StringBuilder();
                Gstr.Append(" create table Gauge (");
                Gstr.Append(" Gid int  IDENTITY (1, 1)  primary key not null, ");
                Gstr.Append(" Ghid int,");
                Gstr.Append(" Gtype nvarchar(50),");
                Gstr.Append(" Gtitle nvarchar(50),");
                Gstr.Append(" Gcount int,");
                Gstr.Append(" Gdate datetime");
                Gstr.Append(" )");

                DbHelperSQL.ExecuteSql(Gstr.ToString());
            }

            if (!DbHelperSQL.TabExists("GaugeItem"))
            {
                StringBuilder Mstr = new StringBuilder();
                Mstr.Append(" create table GaugeItem (");
                Mstr.Append(" Mid int  IDENTITY (1, 1)  primary key not null, ");
                Mstr.Append(" Mgid int,");
                Mstr.Append(" Mitem nvarchar(50),");
                Mstr.Append(" Mscore int,");
                Mstr.Append(" Msort int");
                Mstr.Append(" )");

                DbHelperSQL.ExecuteSql(Mstr.ToString());
            }

            if (!DbHelperSQL.TabExists("GaugeFeedback"))
            {
                StringBuilder Fstr = new StringBuilder();
                Fstr.Append(" create table GaugeFeedback (");
                Fstr.Append(" Fid int  IDENTITY (1, 1)  primary key not null, ");
                Fstr.Append(" Fnum nvarchar(50),");
                Fstr.Append(" Fgrade int,");
                Fstr.Append(" Fclass int,");
                Fstr.Append(" Fcid int,");
                Fstr.Append(" Fmid int,");
                Fstr.Append(" Fwid int,");
                Fstr.Append(" Fgid int,");
                Fstr.Append(" Fselect nvarchar(50),");
                Fstr.Append(" Fscore int,");
                Fstr.Append(" Fgood bit DEFAULT 0,");
                Fstr.Append(" Fdate datetime");
                Fstr.Append(" )");

                DbHelperSQL.ExecuteSql(Fstr.ToString());
            }

            string MissionTable = "Mission";
            string Mgidstr = "Mgid";
            if (!DbHelperSQL.ColumnExists(MissionTable, Mgidstr))
            {
                DbHelperSQL.AddColumn(MissionTable, Mgidstr, "int", 0);
                LearnSite.BLL.Mission mbll = new BLL.Mission();
                mbll.UpdateMgid();
            }

            string RoomTable = "Room";
            string Rgaugestr = "Rgauge";
            if (!DbHelperSQL.ColumnExists(RoomTable, Rgaugestr))
            {
                DbHelperSQL.AddColumn(RoomTable, Rgaugestr, "bit", 0);
                LearnSite.BLL.Room rbll = new BLL.Room();
                rbll.UpdateRgauge();
            }

            string WorksTable = "Works";
            string Wfscorestr = "Wfscore";
            if (!DbHelperSQL.ColumnExists(WorksTable, Wfscorestr))
            {
                DbHelperSQL.AddColumn(WorksTable, Wfscorestr, "int", 0);
                LearnSite.BLL.Works wbll = new BLL.Works();
                wbll.InitWfscore();
            }
        }

        public static void UpdateTable1106()
        {
            string Coursestable = "Courses";
            string Cdelete = "Cdelete";//学案删除标志字段
            if (!DbHelperSQL.ColumnExists(Coursestable, Cdelete))
            {
                DbHelperSQL.AddColumn(Coursestable, Cdelete, "bit", 0);
                BLL.Courses cbll = new BLL.Courses();
                cbll.InitCdelete();
            }
            string Missiontable = "Mission";
            string Mdelete = "Mdelete";//学案删除标志字段
            if (!DbHelperSQL.ColumnExists(Missiontable, Mdelete))
            {
                DbHelperSQL.AddColumn(Missiontable, Mdelete, "bit", 0);
                BLL.Mission mbll = new BLL.Mission();
                mbll.InitMdelete();
            }
        }
        public static void UpdateTable1107()
        {
            string SoftTabel = "Soft";
            string Fhid = "Fhid";
            if (!DbHelperSQL.ColumnExists(SoftTabel, Fhid))
            {
                DbHelperSQL.AddColumn(SoftTabel, Fhid, "int", 0);
            }

            string SigninTabel = "Signin";
            string Qgroup = "Qgroup";
            string Qgscore = "Qgscore";
            if (!DbHelperSQL.ColumnExists(SigninTabel, Qgroup))
            {
                DbHelperSQL.AddColumn(SigninTabel, Qgroup, "nvarchar(50)", -1);
            }
            if (!DbHelperSQL.ColumnExists(SigninTabel, Qgscore))
            {
                DbHelperSQL.AddColumn(SigninTabel, Qgscore, "int", 0);
            }
        }

        public static void UpdateTable1108()
        {
            string TopicReplyTabel = "TopicReply";
            string Rcid = "Rcid";
            string Rclass = "Rclass";
            LearnSite.BLL.TopicReply rbll = new BLL.TopicReply();
            if (!DbHelperSQL.ColumnExists(TopicReplyTabel, Rcid))
            {
                DbHelperSQL.AddColumn(TopicReplyTabel, Rcid, "int", 0);
                rbll.InitRcid();
            }
            if (!DbHelperSQL.ColumnExists(TopicReplyTabel, Rclass))
            {
                DbHelperSQL.AddColumn(TopicReplyTabel, Rclass, "int", 0);
                rbll.InitRclass();
            }
        }
        public static void UpdateTable1109()
        {
            if (!DbHelperSQL.TabExists("Survey"))
            {
                StringBuilder Vstr = new StringBuilder();
                Vstr.Append(" create table Survey (");
                Vstr.Append(" Vid int  IDENTITY (1, 1)  primary key not null, ");
                Vstr.Append(" Vcid int,");
                Vstr.Append(" Vhid int,");
                Vstr.Append(" Vtitle nvarchar(50),");
                Vstr.Append(" Vcontent NVARCHAR(MAX),");
                Vstr.Append(" Vtype int DEFAULT 0,");
                Vstr.Append(" Vtotal int DEFAULT 0,");
                Vstr.Append(" Vscore int DEFAULT 0,");
                Vstr.Append(" Vaverage int,");
                Vstr.Append(" Vclose bit DEFAULT 0,");
                Vstr.Append(" Vpoint bit DEFAULT 0,");
                Vstr.Append(" Venableai bit DEFAULT 0,");
                Vstr.Append(" Vdate datetime");
                Vstr.Append(" )");

                DbHelperSQL.ExecuteSql(Vstr.ToString());
            }

            if (!DbHelperSQL.TabExists("SurveyClass"))
            {
                StringBuilder Ystr = new StringBuilder();
                Ystr.Append(" create table SurveyClass (");
                Ystr.Append(" Yid int  IDENTITY (1, 1)  primary key not null, ");
                Ystr.Append(" Yyear int,");
                Ystr.Append(" Ygrade int,");
                Ystr.Append(" Yclass int,");
                Ystr.Append(" Yterm int,");
                Ystr.Append(" Ycid int,");
                Ystr.Append(" Yvid int,");
                Ystr.Append(" Yselect NVARCHAR(MAX),");
                Ystr.Append(" Ycount NVARCHAR(MAX),");
                Ystr.Append(" Yscore int,");
                Ystr.Append(" Ydate datetime");
                Ystr.Append(" )");

                DbHelperSQL.ExecuteSql(Ystr.ToString());
            }

            if (!DbHelperSQL.TabExists("SurveyFeedback"))
            {
                StringBuilder Fstr = new StringBuilder();
                Fstr.Append(" create table SurveyFeedback (");
                Fstr.Append(" Fid int  IDENTITY (1, 1)  primary key not null, ");
                Fstr.Append(" Fnum nvarchar(50),");
                Fstr.Append(" Fyear int,");
                Fstr.Append(" Fgrade int,");
                Fstr.Append(" Fclass int,");
                Fstr.Append(" Fterm int,");
                Fstr.Append(" Fcid int,");
                Fstr.Append(" Fvid int,");
                Fstr.Append(" Fvtype int DEFAULT 0,");
                Fstr.Append(" Fselect NVARCHAR(MAX),");
                Fstr.Append(" Fscore int DEFAULT 0,");
                Fstr.Append(" Fdate datetime");
                Fstr.Append(" )");

                DbHelperSQL.ExecuteSql(Fstr.ToString());
            }

            if (!DbHelperSQL.TabExists("SurveyItem"))
            {
                StringBuilder Mstr = new StringBuilder();
                Mstr.Append(" create table SurveyItem (");
                Mstr.Append(" Mid int  IDENTITY (1, 1)  primary key not null, ");
                Mstr.Append(" Mqid int,");
                Mstr.Append(" Mvid int,");
                Mstr.Append(" Mitem NVARCHAR(MAX),");
                Mstr.Append(" Mscore int DEFAULT 0,");
                Mstr.Append(" Mcount int DEFAULT 0");
                Mstr.Append(" )");

                DbHelperSQL.ExecuteSql(Mstr.ToString());
            }

            if (!DbHelperSQL.TabExists("SurveyQuestion"))
            {
                StringBuilder Qstr = new StringBuilder();
                Qstr.Append(" create table SurveyQuestion (");
                Qstr.Append(" Qid int  IDENTITY (1, 1)  primary key not null, ");
                Qstr.Append(" Qvid int,");
                Qstr.Append(" Qcid int,");
                Qstr.Append(" Qtitle NVARCHAR(MAX),");
                Qstr.Append(" Qcount int DEFAULT 0");
                Qstr.Append(" )");

                DbHelperSQL.ExecuteSql(Qstr.ToString());
            }
        }
        private static void UpdateOldThing()
        {
            string mysql = "update Works set Wterm=Cterm from Works,Courses where Wcid=cid";
            DbHelperSQL.ExecuteSql(mysql);
            string mysql2 = "update Works set Wgrade=Cobj from Works,Courses where Wcid=cid";
            DbHelperSQL.ExecuteSql(mysql2);
        }
        /// <summary>
        /// 建立学号索引
        /// </summary>
        public static void CreatIndex()
        {
            DateTime today = DateTime.Now;
            string dd = today.Year.ToString() + today.Month.ToString() + today.Day.ToString();
            string iwnum = "create index Wnum+" + dd + " on Works (Wnum asc)";
            DbHelperSQL.CreatIndex(iwnum);

            string iFnum = "create index Fnum+" + dd + "  on SurveyFeedback (Fnum asc)";
            DbHelperSQL.CreatIndex(iFnum);

            string iRsnum = "create index Rsnum+" + dd + "  on TopicReply (Rsnum asc)";
            DbHelperSQL.CreatIndex(iRsnum);

            string iSnum = "create index Snum+" + dd + "  on Students (Snum asc)";
            DbHelperSQL.CreatIndex(iSnum);

            string iQnum = "create index Qnum+" + dd + "  on Signin (Qnum asc)";
            DbHelperSQL.CreatIndex(iQnum);

            string ilnum = "create index Rnum+" + dd + "  on Result (Rnum asc)";
            DbHelperSQL.CreatIndex(ilnum);

            string iPsnum = "create index Psnum+" + dd + "  on Ptyper (Psnum asc)";
            DbHelperSQL.CreatIndex(iPsnum);

            string iGsnum = "create index Psnum+" + dd + "  on Pfinger (Psnum asc)";
            DbHelperSQL.CreatIndex(iGsnum);

            string iFgnum = "create index Fnum+" + dd + "  on GaugeFeedback (Fnum asc)";
            DbHelperSQL.CreatIndex(iFgnum);

        }


        public static void UpdateTable1110()
        {
            string stustable = "Students";
            string Svscore = "Svscore";//课堂调查测验成绩统计字段
            if (!DbHelperSQL.ColumnExists(stustable, Svscore))
            {
                DbHelperSQL.AddColumn(stustable, Svscore, "int", 0);
                LearnSite.BLL.Students sbll = new BLL.Students();
                sbll.InitSvscore();
            }

            string RoomTable = "Room";
            string RgroupMaxstr = "RgroupMax";//班级小组人数上限
            if (!DbHelperSQL.ColumnExists(RoomTable, RgroupMaxstr))
            {
                DbHelperSQL.AddColumn(RoomTable, RgroupMaxstr, "int", 0);
                LearnSite.BLL.Room rbll = new BLL.Room();
                rbll.UpdateRgroupMax();
            }

            string TermTotalTable = "TermTotal";
            string Tvscorestr = "Tvscore";//学期存档中的课堂调查测验成绩统计字段
            string Tsidstr = "Tsid";
            if (!DbHelperSQL.ColumnExists(TermTotalTable, Tvscorestr))
            {
                DbHelperSQL.AddColumn(TermTotalTable, Tvscorestr, "int", 0);
                CreatIndex();//放这一次性使用
            }
            if (!DbHelperSQL.ColumnExists(TermTotalTable, Tsidstr))
            {
                DbHelperSQL.AddColumn(TermTotalTable, Tsidstr, "int", 0);
                UpdateOldThing();
                string mysql = "update TermTotal set Tsid=Sid from TermTotal,Students where Tnum=Snum";
                DbHelperSQL.ExecuteSql(mysql);
            }
        }
        /// <summary>
        /// 所有有关成绩的表全部增加学生ID编号字段，方便快速查询
        /// </summary>
        public static void UpdateTable1200()
        {
            string WorksTable = "Works";
            string Wclassstr = "Wclass";
            string Wsidstr = "Wsid";
            string Wnamestr = "Wname";
            string Wyearstr = "Wyear";
            if (!DbHelperSQL.ColumnExists(WorksTable, Wclassstr))
            {
                DbHelperSQL.AddColumn(WorksTable, Wclassstr, "int", 0);
            }
            if (!DbHelperSQL.ColumnExists(WorksTable, Wsidstr))
            {
                DbHelperSQL.AddColumn(WorksTable, Wsidstr, "int", 0);
            }
            if (!DbHelperSQL.ColumnExists(WorksTable, Wnamestr))
            {
                DbHelperSQL.AddColumn(WorksTable, Wnamestr, "nvarchar(50)", -1);
                string mysqla = "update Works set Wsid=Sid,Wclass=Sclass,Wname=Sname from Works,Students where Wnum=Snum";
                DbHelperSQL.ExecuteSql(mysqla);
            }
            if (!DbHelperSQL.ColumnExists(WorksTable, Wyearstr))
            {
                DbHelperSQL.AddColumn(WorksTable, Wyearstr, "int", 0);
                string mysqla = "update Works set Wyear=Syear from Works,Students where Wnum=Snum";
                DbHelperSQL.ExecuteSql(mysqla);
            }

            string SurveyFeedbackTable = "SurveyFeedback";
            string Fsidstr = "Fsid";
            if (!DbHelperSQL.ColumnExists(SurveyFeedbackTable, Fsidstr))
            {
                DbHelperSQL.AddColumn(SurveyFeedbackTable, Fsidstr, "int", 0);
                string mysqlb = "update SurveyFeedback set Fsid=Sid from SurveyFeedback,Students where Fnum=Snum";
                DbHelperSQL.ExecuteSql(mysqlb);
            }

            string TopicReplyTable = "TopicReply";
            string Rsidstr = "Rsid";
            string Ryearstr = "Ryear";
            if (!DbHelperSQL.ColumnExists(TopicReplyTable, Rsidstr))
            {
                DbHelperSQL.AddColumn(TopicReplyTable, Rsidstr, "int", 0);
                string mysqlc = "update TopicReply set Rsid=Sid from TopicReply,Students where Rsnum=Snum";
                DbHelperSQL.ExecuteSql(mysqlc);
            }
            if (!DbHelperSQL.ColumnExists(TopicReplyTable, Ryearstr))
            {
                DbHelperSQL.AddColumn(TopicReplyTable, Ryearstr, "int", 0);
                string mysqlc = "update TopicReply set Ryear=Syear from TopicReply,Students where Rsnum=Snum";
                DbHelperSQL.ExecuteSql(mysqlc);
            }

            string SigninTable = "Signin";
            string Qsidstr = "Qsid";
            string Qnamestr = "Qname";
            string Qclassstr = "Qclass";
            string Qsyearstr = "Qsyear";
            if (!DbHelperSQL.ColumnExists(SigninTable, Qsidstr))
            {
                DbHelperSQL.AddColumn(SigninTable, Qsidstr, "int", 0);
            }
            if (!DbHelperSQL.ColumnExists(SigninTable, Qnamestr))
            {
                DbHelperSQL.AddColumn(SigninTable, Qnamestr, "nvarchar(50)", -1);
            }
            if (!DbHelperSQL.ColumnExists(SigninTable, Qclassstr))
            {
                DbHelperSQL.AddColumn(SigninTable, Qclassstr, "int", 0);
                string mysql = "update Signin set Qsid=Sid,Qclass=Sclass,Qname=Sname from Signin,Students where Qnum=Snum";
                DbHelperSQL.ExecuteSql(mysql);
            }
            if (!DbHelperSQL.ColumnExists(SigninTable, Qsyearstr))
            {
                DbHelperSQL.AddColumn(SigninTable, Qsyearstr, "int", 0);
                string mysql = "update Signin set Qsyear=Syear from Signin,Students where Qnum=Snum";
                DbHelperSQL.ExecuteSql(mysql);
            }

            string ResultTable = "Result";
            string Rtsidstr = "Rsid";
            if (!DbHelperSQL.ColumnExists(ResultTable, Rtsidstr))
            {
                DbHelperSQL.AddColumn(ResultTable, Rtsidstr, "int", 0);
                string mysqle = "update Result set Rsid=Sid from Result,Students where Rnum=Snum";
                DbHelperSQL.ExecuteSql(mysqle);
            }

            string PtyperTable = "Ptyper";
            string Psidstr = "Psid";
            if (!DbHelperSQL.ColumnExists(PtyperTable, Psidstr))
            {
                DbHelperSQL.AddColumn(PtyperTable, Psidstr, "int", 0);
                string mysqlf = "update Ptyper set Psid=Sid from Ptyper,Students where Psnum=Snum";
                DbHelperSQL.ExecuteSql(mysqlf);
            }

            string PfingerTable = "Pfinger";
            string Pfsidstr = "Psid";
            if (!DbHelperSQL.ColumnExists(PfingerTable, Pfsidstr))
            {
                DbHelperSQL.AddColumn(PfingerTable, Pfsidstr, "int", 0);
                string mysqlg = "update Pfinger set Psid=Sid from Pfinger,Students where Psnum=Snum";
                DbHelperSQL.ExecuteSql(mysqlg);
            }

            string GaugeFeedbackTable = "GaugeFeedback";
            string Fgsidstr = "Fsid";
            if (!DbHelperSQL.ColumnExists(GaugeFeedbackTable, Fgsidstr))
            {
                DbHelperSQL.AddColumn(GaugeFeedbackTable, Fgsidstr, "int", 0);
                string mysqlh = "update GaugeFeedback set Fsid=Sid from GaugeFeedback,Students where Fnum=Snum";
                DbHelperSQL.ExecuteSql(mysqlh);
            }

            string WorksDiscussTable = "WorksDiscuss";
            string Dsidstr = "Dsid";
            if (!DbHelperSQL.ColumnExists(WorksDiscussTable, Dsidstr))
            {
                DbHelperSQL.AddColumn(WorksDiscussTable, Dsidstr, "int", 0);
                string mysqlj = "update WorksDiscuss set Dsid=Sid from WorksDiscuss,Students where Dsnum=Snum";
                DbHelperSQL.ExecuteSql(mysqlj);
            }
        }

        public static void UpdateTable1201()
        {
            string RoomTable = "Room";
            string Rclasseditstr = "Rclassedit";//班级修改
            string Rphotoeditstr = "Rphotoedit";//班级修改
            string Rsexeditstr = "Rsexedit";//班级修改
            string Rnameeditstr = "Rnameedit";//班级修改

            if (!DbHelperSQL.ColumnExists(RoomTable, Rclasseditstr))
            {
                DbHelperSQL.AddColumn(RoomTable, Rclasseditstr, "bit", 0);
            }

            if (!DbHelperSQL.ColumnExists(RoomTable, Rphotoeditstr))
            {
                DbHelperSQL.AddColumn(RoomTable, Rphotoeditstr, "bit", 0);
            }

            if (!DbHelperSQL.ColumnExists(RoomTable, Rsexeditstr))
            {
                DbHelperSQL.AddColumn(RoomTable, Rsexeditstr, "bit", 0);
            }

            if (!DbHelperSQL.ColumnExists(RoomTable, Rnameeditstr))
            {
                DbHelperSQL.AddColumn(RoomTable, Rnameeditstr, "bit", 0);
                string mysql = "update Room set Rclassedit=0,Rphotoedit=0,Rsexedit=0,Rnameedit=0 where Rclassedit is null";
                DbHelperSQL.ExecuteSql(mysql);
            }
        }

        public static void UpdateTable1202()
        {
            string CoursesTable = "Courses";
            string Cgoodstr = "Cgood";
            if (!DbHelperSQL.ColumnExists(CoursesTable, Cgoodstr))
            {
                DbHelperSQL.AddColumn(CoursesTable, Cgoodstr, "bit", 1);
                string mysqlb = "update Courses set Cgood=1 where Cgood is null";
                DbHelperSQL.ExecuteSql(mysqlb);
            }
        }

        public static void UpdateTable1203()
        {
            if (!DbHelperSQL.TabExists("ListMenu"))
            {
                StringBuilder Lstr = new StringBuilder();
                Lstr.Append(" create table ListMenu (");
                Lstr.Append(" Lid int  IDENTITY (1, 1)  primary key not null, ");
                Lstr.Append(" Lcid int,");
                Lstr.Append(" Lsort int  DEFAULT 0,");
                Lstr.Append(" Ltype int,");
                Lstr.Append(" Lxid int,");
                Lstr.Append(" Lshow bit DEFAULT 1,");
                Lstr.Append(" Ltitle nvarchar(50)");
                Lstr.Append(" )");

                DbHelperSQL.ExecuteSql(Lstr.ToString());
                LearnSite.BLL.ListMenu lbll = new BLL.ListMenu();
                lbll.initbuildmenu();//将旧学案更新到导航表
            }
            string SurveyItemTable = "SurveyItem";
            string Mcidstr = "Mcid";
            if (!DbHelperSQL.ColumnExists(SurveyItemTable, Mcidstr))
            {
                DbHelperSQL.AddColumn(SurveyItemTable, Mcidstr, "int", -1);
                string mysqlb = "update SurveyItem set Mcid=Qcid from SurveyItem,SurveyQuestion where Mqid=Qid ";
                DbHelperSQL.ExecuteSql(mysqlb);
            }
        }
        public static void UpdateTable1205()
        {
            string RoomTable = "Room";
            string RcidStr = "Rcid";//班级学案选择标志
            string RopenStr = "Ropen";//公开课模式，为真则，学生登录后直接进学案导航界面

            if (!DbHelperSQL.ColumnExists(RoomTable, RcidStr))
            {
                DbHelperSQL.AddColumn(RoomTable, RcidStr, "int", -1);
            }
            if (!DbHelperSQL.ColumnExists(RoomTable, RopenStr))
            {
                DbHelperSQL.AddColumn(RoomTable, RopenStr, "bit", 0);
            }

            string TeacherTable = "Teacher";
            string HdeleteStr = "Hdelete";//教师账号删除标志

            if (!DbHelperSQL.ColumnExists(TeacherTable, HdeleteStr))
            {
                DbHelperSQL.AddColumn(TeacherTable, HdeleteStr, "bit", 0);
            }
        }
        /// <summary>
        /// 无条件修补
        /// 将导航活动中不存在的活动标志为删除状态
        /// 根据学号同步一下作品表中的年级与班级跟学生表一致
        /// </summary>
        public static void UpdateTable1205a()
        {
            string mysql = "update Mission set Mdelete=1 where Mid not in (select Lxid from ListMenu where Ltype=1)";
            DbHelperSQL.ExecuteSql(mysql);//将导航活动中不存在的活动标志为删除状态
            string tsql = "update Teacher set Hdelete=0 where Hdelete is null";
            DbHelperSQL.ExecuteSql(tsql);//将原老师账号删除标志初始化
        }

        public static void UpdateTable1206()
        {
            string TeacherTable = "Teacher";
            string HcountStr = "Hcount";//教师学案数

            if (!DbHelperSQL.ColumnExists(TeacherTable, HcountStr))
            {
                DbHelperSQL.AddColumn(TeacherTable, HcountStr, "int", 0);

                LearnSite.DBUtility.UpdateGrade.UpdateTable1205a();//再修复一次旧的；
            }
        }


        public static void UpdateTable1207()
        {
            if (!DbHelperSQL.TabExists("House"))
            {
                StringBuilder Hstr = new StringBuilder();
                Hstr.Append(" create table House (");
                Hstr.Append(" Hid int  IDENTITY (1, 1)  primary key not null, ");
                Hstr.Append(" Hname nvarchar(50),");
                Hstr.Append(" Hseat NVARCHAR(MAX)");
                Hstr.Append(" )");

                DbHelperSQL.ExecuteSql(Hstr.ToString());//创建机房表
            }

            if (!DbHelperSQL.TabExists("Ip"))
            {
                StringBuilder istr = new StringBuilder();
                istr.Append(" create table Ip (");
                istr.Append(" Iid int  IDENTITY (1, 1)  primary key not null, ");
                istr.Append(" Ihid int,");
                istr.Append(" Inum int,");
                istr.Append(" Iip nvarchar(50)");
                istr.Append(" )");

                DbHelperSQL.ExecuteSql(istr.ToString());
            }
        }

        public static void UpdateTable1208()
        {
            string RoomTable = "Room";
            string RseatStr = "Rseat";//班级学案选择标志

            if (!DbHelperSQL.ColumnExists(RoomTable, RseatStr))
            {
                DbHelperSQL.AddColumn(RoomTable, RseatStr, "int", 0);
            }
        }
        public static void Updatefingertyper1208()
        {
            //2012-11-21发现打字和指法成绩更新时未更新当前所在年级及学期
            int term = Int32.Parse(LearnSite.Common.XmlHelp.GetTerm());
            string mysql = "update Pfinger set Pgrade=Sgrade,Pterm=" + term + " from Pfinger,Students where Psnum=Snum and Pdate between '2012-08-30'and '2013-02-01' ";
            DbHelperSQL.ExecuteSql(mysql);
            string sqlstr = "update Ptyper set Pgrade=Sgrade,Pterm=" + term + " from Ptyper,Students where Psnum=Snum and Pdate between '2012-08-30'and '2013-02-01' ";
            DbHelperSQL.ExecuteSql(sqlstr);
        }
        public static void UpdateTable1209()
        {
            string stustable = "Students";
            string Sgtitle = "Sgtitle";//小组名称
            if (!DbHelperSQL.ColumnExists(stustable, Sgtitle))
            {
                DbHelperSQL.AddColumn(stustable, Sgtitle, "nvarchar(50)", -1);
                LearnSite.BLL.Students sbll = new BLL.Students();
                sbll.InitSgtitle();
            }
        }
        public static void UpdateTable1210()
        {
            string termtable = "TermTotal";
            string Tyear = "Tyear";//入学年度
            string Tclass = "Tclass";//班级
            string Tname = "Tname";//姓名 因为历史存档所以增加
            if (!DbHelperSQL.ColumnExists(termtable, Tyear))
            {
                DbHelperSQL.AddColumn(termtable, Tyear, "int", -1);
            }
            if (!DbHelperSQL.ColumnExists(termtable, Tclass))
            {
                DbHelperSQL.AddColumn(termtable, Tclass, "int", -1);
            }
            if (!DbHelperSQL.ColumnExists(termtable, Tname))
            {
                DbHelperSQL.AddColumn(termtable, Tname, "nvarchar(50)", -1);

                LearnSite.BLL.TermTotal tbll = new BLL.TermTotal();
                tbll.initTyearTclassTname();
            }
            string SignTable = "Signin";
            string Qcid = "Qcid";
            if (!DbHelperSQL.ColumnExists(SignTable, Qcid))
            {
                DbHelperSQL.AddColumn(SignTable, Qcid, "int", -1);
            }
            string GroupWorkTable = "GroupWork";
            string Ggroup = "Ggroup";
            if (!DbHelperSQL.ColumnExists(GroupWorkTable, Ggroup))
            {
                DbHelperSQL.AddColumn(GroupWorkTable, Ggroup, "int", -1);
                LearnSite.BLL.GroupWork gbll = new BLL.GroupWork();
                gbll.initGgroup();
            }
            string RoomTable = "Room";
            string Rshare = "Rshare";
            string Rpwdsee = "Rpwdsee";
            string Rgroupshare = "Rgroupshare";
            if (!DbHelperSQL.ColumnExists(RoomTable, Rshare))
            {
                DbHelperSQL.AddColumn(RoomTable, Rshare, "bit", 0);
            }
            if (!DbHelperSQL.ColumnExists(RoomTable, Rpwdsee))
            {
                DbHelperSQL.AddColumn(RoomTable, Rpwdsee, "bit", 0);
                LearnSite.BLL.Room rbll = new BLL.Room();
                rbll.initRshare();
                rbll.initRpwdsee();//初始化
            }
            if (!DbHelperSQL.ColumnExists(RoomTable, Rgroupshare))
            {
                DbHelperSQL.AddColumn(RoomTable, Rgroupshare, "bit", 0);
                LearnSite.BLL.Room rbll = new BLL.Room();
                rbll.initRgroupshare();
            }

            if (!DbHelperSQL.TabExists("ShareDisk"))
            {
                StringBuilder Lstr = new StringBuilder();
                Lstr.Append(" create table ShareDisk (");
                Lstr.Append(" Kid int  IDENTITY (1, 1)  primary key not null, ");
                Lstr.Append(" Kown bit DEFAULT 0,");
                Lstr.Append(" Kyear int,");
                Lstr.Append(" Kgrade int,");
                Lstr.Append(" Kclass int,");
                Lstr.Append(" Kgroup int,");
                Lstr.Append(" Knum nvarchar(50),");
                Lstr.Append(" Kname nvarchar(50),");
                Lstr.Append(" Kfilename nvarchar(50),");
                Lstr.Append(" Kfsize int,");
                Lstr.Append(" Kfurl  nvarchar(200),");
                Lstr.Append(" Kftpe nvarchar(50),");
                Lstr.Append(" Kfdate datetime");
                Lstr.Append(" )");

                DbHelperSQL.ExecuteSql(Lstr.ToString());
            }
        }

        public static void UpdateTable1211()
        {
            string RoomTable = "Room";
            string Rtyper = "Rtyper";
            if (!DbHelperSQL.ColumnExists(RoomTable, Rtyper))
            {
                DbHelperSQL.AddColumn(RoomTable, Rtyper, "nvarchar(200)", -1);
            }
        }

        public static void UpdateTable1212()
        {
            string Coursestable = "Courses";
            string Cold = "Cold";//学案删除标志字段
            if (!DbHelperSQL.ColumnExists(Coursestable, Cold))
            {
                DbHelperSQL.AddColumn(Coursestable, Cold, "bit", 0);
                BLL.Courses cbll = new BLL.Courses();
                cbll.InitCold();
            }
        }

        public static void UpdateTable1213()
        {
            string RoomTable = "Room";
            string Rreg = "Rreg";
            if (!DbHelperSQL.ColumnExists(RoomTable, Rreg))
            {
                DbHelperSQL.AddColumn(RoomTable, Rreg, "bit", 0);
                BLL.Room rbll = new BLL.Room();
                rbll.initRreg();//初始化此字段
            }
        }

        public static void Updateworkswfupload20131111()
        {
            string mysql = "update Works set Wyear=Syear from Works,Students where Wsid=Sid and Wdate >'2013-11-10' and Wdate <'2013-11-19'";
            DbHelperSQL.ExecuteSql(mysql);
        }

        public static void UpdatetopicReply20131119()
        {
            string mysql = "update TopicReply set Rcid=Tcid from TopicReply,TopicDiscuss where Rtid=Tid and Rcid=0";
            DbHelperSQL.ExecuteSql(mysql);
        }

        public static void UpdateTable1214()
        {
            if (!DbHelperSQL.TabExists("SoftCategory"))
            {
                //创建资源分类表
                StringBuilder Ystr = new StringBuilder();
                Ystr.Append(" create table SoftCategory (");
                Ystr.Append(" Yid int  IDENTITY (1, 1)  primary key not null, ");
                Ystr.Append(" Ysort int,");
                Ystr.Append(" Ytitle nvarchar(50),");
                Ystr.Append(" Ycontent nvarchar(200),");
                Ystr.Append(" Yopen bit DEFAULT 0");
                Ystr.Append(" )");
                DbHelperSQL.ExecuteSql(Ystr.ToString());
                Model.SoftCategory ymodel = new Model.SoftCategory();
                ymodel.Yopen = false;
                ymodel.Ysort = 0;
                ymodel.Ytitle = "默认分类";
                ymodel.Ycontent = "升级前的资源归档";
                BLL.SoftCategory ybll = new BLL.SoftCategory();
                ybll.Add(ymodel);
            }
            //资源表增加字段Fyid，并初始化
            string SoftTabel = "Soft";
            string Fyid = "Fyid";
            if (!DbHelperSQL.ColumnExists(SoftTabel, Fyid))
            {
                DbHelperSQL.AddColumn(SoftTabel, Fyid, "int", 1);
                string mysql = "update Soft set Fyid=1 where Fyid is null";
                DbHelperSQL.ExecuteSql(mysql);
            }

        }

        public static void UpdateTable1215()
        {
            if (!DbHelperSQL.TabExists("Autonomic"))
            {
                //创建资源自主学习作品表
                StringBuilder Astr = new StringBuilder();
                Astr.Append(" create table Autonomic (");
                Astr.Append(" Aid int  IDENTITY (1, 1)  primary key not null, ");
                Astr.Append(" Asid  int,");
                Astr.Append(" Anum  nvarchar(50) ,");
                Astr.Append(" Aname nvarchar(50) ,");
                Astr.Append(" Ayid  int,");
                Astr.Append(" Afid  int,");
                Astr.Append(" Atype  nvarchar(50),");
                Astr.Append(" Afilename  nvarchar(50),");
                Astr.Append(" Aurl  nvarchar(200),");
                Astr.Append(" Alength  int,");
                Astr.Append(" Ascore  int DEFAULT 0,");
                Astr.Append(" Adate  datetime,");
                Astr.Append(" Aip  nvarchar(50),");
                Astr.Append(" Avote int DEFAULT 0,");
                Astr.Append(" Aegg  int DEFAULT 0,");
                Astr.Append(" Acheck  bit DEFAULT 0,");
                Astr.Append(" Aself  nvarchar(200),");
                Astr.Append(" Agood  bit DEFAULT 0,");
                Astr.Append(" Ayear  int,");
                Astr.Append(" Agrade  int,");
                Astr.Append(" Aclass  int,");
                Astr.Append(" Aterm  int,");
                Astr.Append(" Ahit  int DEFAULT 0,");
                Astr.Append(" Aoffice  bit DEFAULT 0,");
                Astr.Append(" Aflash  bit DEFAULT 0,");
                Astr.Append(" Aerror  bit DEFAULT 0");
                Astr.Append(" )");
                DbHelperSQL.ExecuteSql(Astr.ToString());
            }

            string studentstable = "Students";
            string Sascore = "Sascore";
            if (!DbHelperSQL.ColumnExists(studentstable, Sascore))
            {
                //增加学生表资源自主学习作品分
                DbHelperSQL.AddColumn(studentstable, Sascore, "int", 0);
            }

            //资源表增加字段Fyid，并初始化
            string SoftTabel = "Soft";
            string Fup = "Fup";
            if (!DbHelperSQL.ColumnExists(SoftTabel, Fup))
            {
                DbHelperSQL.AddColumn(SoftTabel, Fup, "bit", 0);
                string mysql = "update Soft set Fup=0 where Fup is null";
                DbHelperSQL.ExecuteSql(mysql);
            }
        }

        public static void UpdateTable1216()
        {
            string TopicReplyTable = "TopicReply";
            string Reditstr = "Redit";//允许修改
            string Ragreestr = "Ragree";//点赞数
            if (!DbHelperSQL.ColumnExists(TopicReplyTable, Reditstr))
            {
                DbHelperSQL.AddColumn(TopicReplyTable, Reditstr, "bit", 0);
                string mysql = "update TopicReply set Redit=0 where Redit is null";
                DbHelperSQL.ExecuteSql(mysql);
            }

            if (!DbHelperSQL.ColumnExists(TopicReplyTable, Ragreestr))
            {
                DbHelperSQL.AddColumn(TopicReplyTable, Ragreestr, "int", 0);
                string mysql = "update TopicReply set Ragree=0 where Ragree is null";
                DbHelperSQL.ExecuteSql(mysql);
            }
        }

        public static void UpdateTable1217()
        {
            string studentstable = "Students";
            string Skaoxu = "Skaoxu";
            if (!DbHelperSQL.ColumnExists(studentstable, Skaoxu))
            {
                DbHelperSQL.AddColumn(studentstable, Skaoxu, "nvarchar(50)", -1);
            }
        }


        public static void UpdateTable1218()
        {
            string computertable = "Computers";
            string Px = "Px";
            string Py = "Py";
            string Pm = "Pm";//电脑室名称
            if (!DbHelperSQL.ColumnExists(computertable, Px))
            {
                DbHelperSQL.AddColumn(computertable, Px, "int", 0);
            }
            if (!DbHelperSQL.ColumnExists(computertable, Py))
            {
                DbHelperSQL.AddColumn(computertable, Py, "int", 0);
            }
            if (!DbHelperSQL.ColumnExists(computertable, Pm))
            {
                DbHelperSQL.AddColumn(computertable, Pm, "nvarchar(50)", -1);
                LearnSite.BLL.Computers cbll = new BLL.Computers();
                cbll.initPxy();
            }

            string TeacherTable = "Teacher";
            string HnickStr = "Hnick";//教师昵称
            string Hroom = "Hroom";//电脑室

            if (!DbHelperSQL.ColumnExists(TeacherTable, HnickStr))
            {
                DbHelperSQL.AddColumn(TeacherTable, HnickStr, "nvarchar(50)", -1);
            }
            if (!DbHelperSQL.ColumnExists(TeacherTable, Hroom))
            {
                DbHelperSQL.AddColumn(TeacherTable, Hroom, "nvarchar(50)", -1);
                LearnSite.BLL.Teacher tbll = new BLL.Teacher();
                tbll.initHnick();
                string cfix = LearnSite.Common.WordProcess.GetMD5_8bit(DateTime.Now.ToString());//取时间加密码为前缀
                LearnSite.Common.XmlHelp.SetCookiesFix(cfix);
            }

            string studentstable = "Students";
            string Swdscore = "Swdscore";
            if (!DbHelperSQL.ColumnExists(studentstable, Swdscore))
            {
                //增加学生表作品加分
                DbHelperSQL.AddColumn(studentstable, Swdscore, "int", 0);
                LearnSite.BLL.Students sbll = new BLL.Students();
                sbll.initSwdscore();
            }
            string WorksTable = "Works";
            string Wdscore = "Wdscore";
            if (!DbHelperSQL.ColumnExists(WorksTable, Wdscore))
            {
                //增加作业表作品加分
                DbHelperSQL.AddColumn(WorksTable, Wdscore, "int", 0);
                LearnSite.BLL.Works wbll = new BLL.Works();
                wbll.initWdscore();
            }
        }

        public static void UpdateTable1220()
        {
            if (!DbHelperSQL.TabExists("TxtForm"))
            {
                //文本表单表
                StringBuilder Mstr = new StringBuilder();
                Mstr.Append(" create table TxtForm (");
                Mstr.Append(" Mid int  IDENTITY (1, 1)  primary key not null, ");
                Mstr.Append(" Mtitle nvarchar(50),");
                Mstr.Append(" Mcid int,");
                Mstr.Append(" Mcontent NVARCHAR(MAX),");
                Mstr.Append(" Mdate datetime,");
                Mstr.Append(" Mhit int,");
                Mstr.Append(" Mpublish bit DEFAULT 0,");
                Mstr.Append(" Mdelete bit DEFAULT 0");
                Mstr.Append(" )");
                DbHelperSQL.ExecuteSql(Mstr.ToString());
            }

            if (!DbHelperSQL.TabExists("TxtFormBack"))
            {
                //文本表单表
                StringBuilder Tstr = new StringBuilder();
                Tstr.Append(" create table TxtFormBack (");
                Tstr.Append(" Rid int  IDENTITY (1, 1)  primary key not null, ");
                Tstr.Append(" Rmid int,");
                Tstr.Append(" Rsnum nvarchar(50),");
                Tstr.Append(" Rsid int,");
                Tstr.Append(" Rwords NVARCHAR(MAX),");
                Tstr.Append(" Rtime datetime,");
                Tstr.Append(" Rip nvarchar(50),");
                Tstr.Append(" Rscore int DEFAULT 0,");
                Tstr.Append(" Ryear int,");
                Tstr.Append(" Rterm int,");
                Tstr.Append(" Rgrade int,");
                Tstr.Append(" Rclass int,");
                Tstr.Append(" Ragree int,");
                Tstr.Append(" )");
                DbHelperSQL.ExecuteSql(Tstr.ToString());
            }
        }


        public static void UpdateTable1222()
        {
            if (!DbHelperSQL.TabExists("Chinese"))
            {
                //拼音词语表
                StringBuilder Cstr = new StringBuilder();
                Cstr.Append(" create table Chinese (");
                Cstr.Append(" Nid int  IDENTITY (1, 1)  primary key not null, ");
                Cstr.Append(" Ntitle nvarchar(50),");
                Cstr.Append(" Ncontent NVARCHAR(MAX)");
                Cstr.Append(" )");
                DbHelperSQL.ExecuteSql(Cstr.ToString());
            }

            if (!DbHelperSQL.TabExists("Pchinese"))
            {
                StringBuilder Pstr = new StringBuilder();
                Pstr.Append(" create table Pchinese (");
                Pstr.Append(" Pid int  IDENTITY (1, 1)  primary key not null, ");
                Pstr.Append(" Psid int,");
                Pstr.Append(" Psnum nvarchar(50),");
                Pstr.Append(" Papple int DEFAULT 0,");
                Pstr.Append(" Ptotal int DEFAULT 0,");
                Pstr.Append(" Pspeed int DEFAULT 0,");
                Pstr.Append(" Pdegree int,");
                Pstr.Append(" Pyear int,");
                Pstr.Append(" Pgrade int,");
                Pstr.Append(" Pclass int,");
                Pstr.Append(" Pterm int,");
                Pstr.Append(" Pdate datetime");
                Pstr.Append(" )");

                DbHelperSQL.ExecuteSql(Pstr.ToString());
            }
            string RoomTable = "Room";
            string Rchinese = "Rchinese";
            if (!DbHelperSQL.ColumnExists(RoomTable, Rchinese))
            {
                DbHelperSQL.AddColumn(RoomTable, Rchinese, "nvarchar(200)", -1);
            }
        }


        public static void UpdateTable1226()
        {
            string studentstable = "Students";
            string Stxtform = "Stxtform";
            string Schinese = "Schinese";
            if (!DbHelperSQL.ColumnExists(studentstable, Stxtform))
            {
                //增加学生表表单分数
                DbHelperSQL.AddColumn(studentstable, Stxtform, "int", 0);
                LearnSite.BLL.Students sbll = new BLL.Students();
                sbll.initStxtform();
            }

            if (!DbHelperSQL.ColumnExists(studentstable, Schinese))
            {
                //增加学生表中文拼音分数
                DbHelperSQL.AddColumn(studentstable, Schinese, "int", 0);
                LearnSite.BLL.Students sbll = new BLL.Students();
                sbll.initSchinese();
            }

            string TermTotaltable = "TermTotal";
            string Ttxtform = "Ttxtform";
            string Tchinese = "Tchinese";
            if (!DbHelperSQL.ColumnExists(TermTotaltable, Ttxtform))
            {
                //增加统计表表单分数
                DbHelperSQL.AddColumn(TermTotaltable, Ttxtform, "int", 0);
            }

            if (!DbHelperSQL.ColumnExists(TermTotaltable, Tchinese))
            {
                //增加统计表中文拼音分数
                DbHelperSQL.AddColumn(TermTotaltable, Tchinese, "int", 0);
            }
        }
        /// <summary>
        /// 任务表 Mission增加字段Mexample实例（编程实例）
        ///        Mcategory分类（用于区分 提交页面0、描述页面1、编程页面2）
        /// 
        /// 作业表Works增加字段Wthumbnail缩略图（是/否）
        /// </summary>
        public static void UpdateTable1228()
        {
            string missiontalble = "Mission";
            string workstable = "Works";
            string Mexample = "Mexample";
            string Mcategory = "Mcategory";
            string Microworld = "Microworld";
            string Wthumbnail = "Wthumbnail";
            if (!DbHelperSQL.ColumnExists(missiontalble, Mexample))
            {
                DbHelperSQL.AddColumn(missiontalble, Mexample, "nvarchar(50)", -1);
            }
            if (!DbHelperSQL.ColumnExists(missiontalble, Mcategory))
            {
                DbHelperSQL.AddColumn(missiontalble, Mcategory, "int", 0);
                LearnSite.BLL.Mission mbll = new BLL.Mission();
                mbll.InitMcategory();//初始化字段值为0；
            }
            if (!DbHelperSQL.ColumnExists(workstable, Wthumbnail))
            {
                DbHelperSQL.AddColumn(workstable, Wthumbnail, "nvarchar(200)", -1);
            }
            if (!DbHelperSQL.ColumnExists(missiontalble, Microworld))
            {
                DbHelperSQL.AddColumn(missiontalble, Microworld, "bit", 0);
            }
        }

        public static void UpdateTable1229()
        {
            string RoomTable = "Room";
            string Rscratch = "Rscratch";
            if (!DbHelperSQL.ColumnExists(RoomTable, Rscratch))
            {
                DbHelperSQL.AddColumn(RoomTable, Rscratch, "bit", 0);
            }

        }
        public static void FixColumnSize1229()
        {
            string mysql = "alter table Works alter column Wthumbnail nvarchar(200)";
            DbHelperSQL.ExecuteSql(mysql);
        }

        public static void UpdateTable1230()
        {
            string workstable = "Works";
            string Wtitle = "Wtitle";
            if (!DbHelperSQL.ColumnExists(workstable, Wtitle))
            {
                DbHelperSQL.AddColumn(workstable, Wtitle, "nvarchar(200)", -1);
            }

        }

        public static void UpdateTable1232()
        {
            if (!DbHelperSQL.TabExists("Research"))
            {
                StringBuilder Pstr = new StringBuilder();
                Pstr.Append(" create table Research (");
                Pstr.Append(" Rid int  IDENTITY (1, 1)  primary key not null, ");
                Pstr.Append(" Rsid int,");
                Pstr.Append(" Ryear int,");
                Pstr.Append(" Rgrade int,");
                Pstr.Append(" Rclass int,");
                Pstr.Append(" Rterm int,");
                Pstr.Append(" Rlearn SmallMoney,");
                Pstr.Append(" Rplay SmallMoney,");
                Pstr.Append(" Rsleep SmallMoney,");
                Pstr.Append(" Rfree SmallMoney,");
                Pstr.Append(" Rdate datetime");
                Pstr.Append(" )");

                DbHelperSQL.ExecuteSql(Pstr.ToString());
            }

        }

        public static void UpdateTable1251()
        {
            string studentstable = "Students";
            string Stat = "Stat";
            if (!DbHelperSQL.ColumnExists(studentstable, Stat))
            {
                DbHelperSQL.AddColumn(studentstable, Stat, "bit", 0);
            }
        }

        public static void UpdateTable1252()
        {
            string workstable = "Works";
            string Weditday = "Weditday";
            if (!DbHelperSQL.ColumnExists(workstable, Weditday))
            {
                DbHelperSQL.AddColumn(workstable, Weditday, "int", 0);
            }

        }
        public static void UpdateTable1253()
        {
            string studentstable = "Students";
            string Stat = "Stenscore";
            if (!DbHelperSQL.ColumnExists(studentstable, Stat))
            {
                DbHelperSQL.AddColumn(studentstable, Stat, "int", 0);
            }

        }
        public static void UpdateTable1260()
        {
            string workstable = "Works";
            string Wdict = "Wdict";
            if (!DbHelperSQL.ColumnExists(workstable, Wdict))
            {
                DbHelperSQL.AddColumn(workstable, Wdict, "NVARCHAR(MAX)", -1);
            }
            string Wcode = "Wcode";
            if (!DbHelperSQL.ColumnExists(workstable, Wcode))
            {
                DbHelperSQL.AddColumn(workstable, Wcode, "NVARCHAR(MAX)", -1);
            }

            string RoomTable = "Room";
            string Rlogin = "Rlogin";
            if (!DbHelperSQL.ColumnExists(RoomTable, Rlogin))
            {
                DbHelperSQL.AddColumn(RoomTable, Rlogin, "bit", 0);
            }

        }

        public static void UpdateTable1280()
        {
            string missiontalble = "Mission";
            string Mback = "Mback";//海龟画图背景550x400

            if (!DbHelperSQL.ColumnExists(missiontalble, Mback))
            {
                DbHelperSQL.AddColumn(missiontalble, Mback, "nvarchar(200)", -1);
            }

        }

        public static void UpdateTable1300()
        {
            if (!DbHelperSQL.TabExists("Consoles"))
            {

                StringBuilder Gstr = new StringBuilder();
                Gstr.Append(" create table Consoles (");
                Gstr.Append(" Nid int  IDENTITY (1, 1)  primary key not null, ");
                Gstr.Append(" Nhid int,");
                Gstr.Append(" Ncid int,");
                Gstr.Append(" Ntitle nvarchar(50),");
                Gstr.Append(" Ncontent NVARCHAR(MAX),");
                Gstr.Append(" Npublish bit DEFAULT 0,");
                Gstr.Append(" Ndate datetime");
                Gstr.Append(" )");

                DbHelperSQL.ExecuteSql(Gstr.ToString());
            }
            if (!DbHelperSQL.TabExists("Problems"))
            {
                StringBuilder castr = new StringBuilder();
                castr.Append(" create table Problems (");
                castr.Append(" Pid int  IDENTITY (1, 1)  primary key not null, ");
                castr.Append(" Phid int,");
                castr.Append(" Pnid int,");
                castr.Append(" Ptitle nvarchar(200),");
                castr.Append(" Pcode nvarchar(200),");
                castr.Append(" Pouput nvarchar(200),");
                castr.Append(" Pscore int,");
                castr.Append(" Pdate datetime");
                castr.Append(" )");

                DbHelperSQL.ExecuteSql(castr.ToString());
            }

            if (!DbHelperSQL.TabExists("Solves"))
            {
                StringBuilder castr = new StringBuilder();
                castr.Append(" create table Solves (");
                castr.Append(" Vid int  IDENTITY (1, 1)  primary key not null, ");
                castr.Append(" Vpid int,");
                castr.Append(" Vsid int,");
                castr.Append(" Vanswer nvarchar(200),");
                castr.Append(" Vright bit DEFAULT 0,");
                castr.Append(" Vscore int,");
                castr.Append(" Vdate datetime");
                castr.Append(" )");

                DbHelperSQL.ExecuteSql(castr.ToString());
            }

            string SolvesTable = "Solves";
            string Vgradestr = "Vgrade";
            string Vtermstr = "Vterm";

            if (!DbHelperSQL.ColumnExists(SolvesTable, Vgradestr))
            {
                DbHelperSQL.AddColumn(SolvesTable, Vgradestr, "int", -1);
            }
            if (!DbHelperSQL.ColumnExists(SolvesTable, Vtermstr))
            {
                DbHelperSQL.AddColumn(SolvesTable, Vtermstr, "int", -1);
            }
            //1.3.1.0更新
            if (!DbHelperSQL.TabExists("JudgeArg"))
            {
                StringBuilder castr = new StringBuilder();
                castr.Append(" create table JudgeArg (");
                castr.Append(" Jid int  IDENTITY (1, 1)  primary key not null, ");
                castr.Append(" Jhid int,");
                castr.Append(" Jmid int,");
                castr.Append(" Jsleep int DEFAULT 1000,");
                castr.Append(" Jinone nvarchar(50),");
                castr.Append(" Jintwo nvarchar(50),");
                castr.Append(" Jinthree nvarchar(50),");
                castr.Append(" Joutone nvarchar(200),");
                castr.Append(" Joutwo nvarchar(200),");
                castr.Append(" Jouthree nvarchar(200),");
                castr.Append(" Jright bit DEFAULT 0,");
                castr.Append(" Jcode NVARCHAR(MAX) ");
                castr.Append(" )");

                DbHelperSQL.ExecuteSql(castr.ToString());
            }

        }

        public static void UpdateTable1320()
        {
            string workstable = "Works";
            string Wpass = "Wpass";//python自动批改通过标志
            if (!DbHelperSQL.ColumnExists(workstable, Wpass))
            {
                DbHelperSQL.AddColumn(workstable, Wpass, "bit", 0);
            }

            string studentstable = "Students";
            string Sidle = "Sidle";//测评总分
            if (!DbHelperSQL.ColumnExists(studentstable, Sidle))
            {
                DbHelperSQL.AddColumn(studentstable, Sidle, "int", 0);
            }

            LearnSite.BLL.Solves vbll = new BLL.Solves();
            vbll.UpdateVgradeVterm();//修改字段未加前的记录

            string Sztype = "Sztype";//测评总分
            if (!DbHelperSQL.ColumnExists(studentstable, Sztype))
            {
                DbHelperSQL.AddColumn(studentstable, Sztype, "int", 0);
            }

            LearnSite.BLL.Students sbll = new BLL.Students();
            sbll.initSztype();
        }

        public static void UpdateTable1330()
        {
            if (!DbHelperSQL.TabExists("Game"))
            {
                StringBuilder Gstr = new StringBuilder();
                Gstr.Append(" create table Game (");
                Gstr.Append(" Gid int  IDENTITY (1, 1)  primary key not null, ");
                Gstr.Append(" Gsid int,");
                Gstr.Append(" Gsname nvarchar(50),");
                Gstr.Append(" Gnum int,");
                Gstr.Append(" Gtitle nvarchar(50),");
                Gstr.Append(" Gsave int,");
                Gstr.Append(" Gnote NVARCHAR(MAX),");
                Gstr.Append(" Gscore int,");
                Gstr.Append(" Gdate datetime");
                Gstr.Append(" )");

                DbHelperSQL.ExecuteSql(Gstr.ToString());
            }

            string Consolestable = "Consoles";
            string Nbegin = "Nbegin";//python自动批改通过标志
            if (!DbHelperSQL.ColumnExists(Consolestable, Nbegin))
            {
                DbHelperSQL.AddColumn(Consolestable, Nbegin, "bit", 0);
                LearnSite.BLL.Consoles bll = new BLL.Consoles();
                bll.InitNbegin();
            }

        }

        public static void UpdateTable1332()
        {
            string SolvesTable = "Solves";
            string Vyearstr = "Vyear";
            string Vnidstr = "Vnid";
            string Vcidstr = "Vcid";
            string Vclassstr = "Vclass";

            if (!DbHelperSQL.ColumnExists(SolvesTable, Vyearstr))
            {
                DbHelperSQL.AddColumn(SolvesTable, Vyearstr, "int", -1);
            }
            if (!DbHelperSQL.ColumnExists(SolvesTable, Vnidstr))
            {
                DbHelperSQL.AddColumn(SolvesTable, Vnidstr, "int", -1);
            }
            if (!DbHelperSQL.ColumnExists(SolvesTable, Vcidstr))
            {
                DbHelperSQL.AddColumn(SolvesTable, Vcidstr, "int", -1);
            }
            if (!DbHelperSQL.ColumnExists(SolvesTable, Vclassstr))
            {
                DbHelperSQL.AddColumn(SolvesTable, Vclassstr, "int", -1);
            }
            LearnSite.BLL.Solves vbll = new BLL.Solves();
            vbll.init();


            string ProblemsTable = "Problems";
            string Psortstr = "Psort";
            if (!DbHelperSQL.ColumnExists(ProblemsTable, Psortstr))
            {
                DbHelperSQL.AddColumn(ProblemsTable, Psortstr, "int", -1);
            }
            LearnSite.BLL.Problems pbll = new BLL.Problems();
            pbll.init();
        }

        public static void UpdateEnglishLevel3() {
            LearnSite.BLL.English ebll = new BLL.English();
            int level=3;
            int count = ebll.CountLevel(level);//如果没有3的英语单词，则重新导入3
            if (count == 0) {
                LearnSite.Common.DataExcel.DataSetaddEnglish(level);
            }       

        }

        public static void UpdateIdleClass() {
            LearnSite.BLL.Solves bll = new BLL.Solves();
            bll.updateclass();
            bll.updatescore();
        }

        public static void UpdateTable1333()
        {
            string ProblemsTable = "Problems";
            string Pcidstr = "Pcid";
            if (!DbHelperSQL.ColumnExists(ProblemsTable, Pcidstr))
            {
                DbHelperSQL.AddColumn(ProblemsTable, Pcidstr, "int", -1);
            }
            LearnSite.BLL.Problems pbll = new BLL.Problems();
            pbll.initCid();

            string JudgeArgTable = "JudgeArg";
            string Jcidstr = "Jcid";
            if (!DbHelperSQL.ColumnExists(JudgeArgTable, Jcidstr))
            {
                DbHelperSQL.AddColumn(JudgeArgTable, Jcidstr, "int", -1);
                LearnSite.BLL.JudgeArg jbll = new BLL.JudgeArg();
                jbll.initCid();
            }


            string ComputersTable = "Computers";
            string Pnumstr = "Pnum";//自动分配学号
            string Ponstr = "Pon";//登录状态
            if (!DbHelperSQL.ColumnExists(ComputersTable, Pnumstr))
            {
                DbHelperSQL.AddColumn(ComputersTable, Pnumstr, "nvarchar(200)", -1);
            }
            if (!DbHelperSQL.ColumnExists(ComputersTable, Ponstr))
            {
                DbHelperSQL.AddColumn(ComputersTable, Ponstr, "bit", 0);
            }
        }


        public static void UpdateTable1335()
        {
            string JudgeArgTable = "JudgeArg";
            string Jimgstr = "Jimg";//海龟绘图尺寸特征
            if (!DbHelperSQL.ColumnExists(JudgeArgTable, Jimgstr))
            {
                DbHelperSQL.AddColumn(JudgeArgTable, Jimgstr, "nvarchar(200)", -1);
            }

            if (!DbHelperSQL.TabExists("Turtle"))
            {
                StringBuilder Gstr = new StringBuilder();
                Gstr.Append(" create table Turtle (");
                Gstr.Append(" Tid int  IDENTITY (1, 1)  primary key not null, ");
                Gstr.Append(" Thid int,");
                Gstr.Append(" Ttilte nvarchar(50),");
                Gstr.Append(" Tcontent NVARCHAR(MAX),");
                Gstr.Append(" Tdegree int,");//星级难度
                Gstr.Append(" Tsort int,");//题目序号
                Gstr.Append(" Tcode NVARCHAR(MAX),");//代码
                Gstr.Append(" Timg nvarchar(50),");//绘图尺寸特征
                Gstr.Append(" Turl nvarchar(50),");//绘图链接
                Gstr.Append(" Tout nvarchar(200),");//输出结果
                Gstr.Append(" Tdate datetime");
                Gstr.Append(" )");

                DbHelperSQL.ExecuteSql(Gstr.ToString());
            }


            string TurtleTable = "Turtle";
            string Tstudystr = "Tstudy";//作品类型：创作0 探究1
            if (!DbHelperSQL.ColumnExists(TurtleTable, Tstudystr))
            {
                DbHelperSQL.AddColumn(TurtleTable, Tstudystr, "bit", 0);
            }

            string Tsidstr = "Tsid";//学生ID
            string Tscore = "Tscore";//学生ID
            string Tip = "Tip";//ip地址

            if (!DbHelperSQL.ColumnExists(TurtleTable, Tsidstr))
            {
                DbHelperSQL.AddColumn(TurtleTable, Tsidstr, "int", -1);
            }
            if (!DbHelperSQL.ColumnExists(TurtleTable, Tscore))
            {
                DbHelperSQL.AddColumn(TurtleTable, Tscore, "int", -1);
            }
            if (!DbHelperSQL.ColumnExists(TurtleTable, Tip))
            {
                DbHelperSQL.AddColumn(TurtleTable, Tip, "nvarchar(50)", -1);
            }

        }


        public static void UpdateTable1336()
        {
            if (!DbHelperSQL.TabExists("TurtleMatch"))
            {
                StringBuilder Gstr = new StringBuilder();
                Gstr.Append(" create table TurtleMatch (");
                Gstr.Append(" Mid int  IDENTITY (1, 1)  primary key not null, ");
                Gstr.Append(" Mhid int,");
                Gstr.Append(" Mtitle nvarchar(50),");
                Gstr.Append(" Mcontent NVARCHAR(MAX),");
                Gstr.Append(" Mbegin datetime,");
                Gstr.Append(" Mend datetime,");
                Gstr.Append(" Mpublish bit DEFAULT 0,");
                Gstr.Append(" Mdate datetime");
                Gstr.Append(" )");

                DbHelperSQL.ExecuteSql(Gstr.ToString());
            }

            if (!DbHelperSQL.TabExists("TurtleQuestion"))
            {
                StringBuilder Gstr = new StringBuilder();
                Gstr.Append(" create table TurtleQuestion (");
                Gstr.Append(" Qid int  IDENTITY (1, 1)  primary key not null, ");
                Gstr.Append(" Qmid int,");
                Gstr.Append(" Qtitle nvarchar(50),");
                Gstr.Append(" Qcontent NVARCHAR(MAX),");
                Gstr.Append(" Qdegree int,");//星级难度
                Gstr.Append(" Qsort int,");//题目序号
                Gstr.Append(" Qcode NVARCHAR(MAX),");//代码
                Gstr.Append(" Qimg nvarchar(50),");//绘图尺寸特征
                Gstr.Append(" Qurl nvarchar(50),");//绘图链接
                Gstr.Append(" Qout nvarchar(200),");//输出结果
                Gstr.Append(" Qscore int,");//题目分值
                Gstr.Append(" Qdate datetime");
                Gstr.Append(" )");

                DbHelperSQL.ExecuteSql(Gstr.ToString());
            }

            if (!DbHelperSQL.TabExists("TurtleAnswer"))
            {
                StringBuilder Gstr = new StringBuilder();
                Gstr.Append(" create table TurtleAnswer (");
                Gstr.Append(" Aid int  IDENTITY (1, 1)  primary key not null, ");
                Gstr.Append(" Amid int,");
                Gstr.Append(" Aqid int,");
                Gstr.Append(" Acode NVARCHAR(MAX),");
                Gstr.Append(" Aimg nvarchar(50),");
                Gstr.Append(" Aurl nvarchar(50),");
                Gstr.Append(" Aout nvarchar(200),");
                Gstr.Append(" Ascore int,");
                Gstr.Append(" Asid int,");
                Gstr.Append(" Asname nvarchar(50),");
                Gstr.Append(" Alock bit DEFAULT 0,");
                Gstr.Append(" Adate datetime");
                Gstr.Append(" )");

                DbHelperSQL.ExecuteSql(Gstr.ToString());
            }
        }

        public static void UpdateTable1337()
        {
            string JudgeArgTable = "JudgeArg";
            string Jthumbstr = "Jthumb";//海龟绘图尺寸特征
            if (!DbHelperSQL.ColumnExists(JudgeArgTable, Jthumbstr))
            {
                DbHelperSQL.AddColumn(JudgeArgTable, Jthumbstr, "nvarchar(200)", -1);
            }

        }

        public static void UpdateTable1338()
        {
            string missiontalble = "Mission";
            string Mhelp = "Mhelp";//编程帮助

            if (!DbHelperSQL.ColumnExists(missiontalble, Mhelp))
            {
                DbHelperSQL.AddColumn(missiontalble, Mhelp, "bit", 0);
            }

        }

        public static void UpdateTable1339()
        {
            string Coursestable = "Courses";
            string Cbanner = "Cbanner";//学案横幅字段
            if (!DbHelperSQL.ColumnExists(Coursestable, Cbanner))
            {
                DbHelperSQL.AddColumn(Coursestable, Cbanner, "nvarchar(50)", -1);
            }
        }


        public static void UpdateTable1350()
        {
            string TopicReplytable = "TopicReply";
            string Rlid = "Rlid";//讨论3
            if (!DbHelperSQL.ColumnExists(TopicReplytable, Rlid))
            {
                DbHelperSQL.AddColumn(TopicReplytable, Rlid, "int", -1);
            }

            string TxtFormBacktable = "TxtFormBack";
            string aRlid = "Rlid";//填表4
            if (!DbHelperSQL.ColumnExists(TxtFormBacktable, aRlid))
            {
                DbHelperSQL.AddColumn(TxtFormBacktable, aRlid, "int", -1);
            }

            string Solvestable = "Solves";
            string Vlid = "Vlid";//python测评9
            if (!DbHelperSQL.ColumnExists(Solvestable, Vlid))
            {
                DbHelperSQL.AddColumn(Solvestable, Vlid, "int", -1);
            }

            string Workstable = "Works";
            string Wlid = "Wlid";
            //任务：活动、积木编程、python编程、流程图、像素画、网页设计
            //编号：1、5、8、10、11、12、13、14
            if (!DbHelperSQL.ColumnExists(Workstable, Wlid))
            {
                DbHelperSQL.AddColumn(Workstable, Wlid, "int", -1);
            }

            string SurveyFeedbacktable = "SurveyFeedback";
            string Flid = "Flid";//调查2
            if (!DbHelperSQL.ColumnExists(SurveyFeedbacktable, Flid))
            {
                DbHelperSQL.AddColumn(SurveyFeedbacktable, Flid, "int", -1);
            }

            if (!DbHelperSQL.TabExists("MenuWorks"))
            {
                //创建菜单导航任务活动记录表
                StringBuilder Gstr = new StringBuilder();
                Gstr.Append(" create table MenuWorks (");
                Gstr.Append(" kid int  IDENTITY (1, 1)  primary key not null, ");
                Gstr.Append(" ksid int,");
                Gstr.Append(" klid int,");
                Gstr.Append(" ktime int,");
                Gstr.Append(" kcheck bit DEFAULT 0 ");
                Gstr.Append(" )");

                DbHelperSQL.ExecuteSql(Gstr.ToString());
            }

            string RoomTable = "Room";
            string RPass = "Rpass";
            if (!DbHelperSQL.ColumnExists(RoomTable, RPass))
            {
                DbHelperSQL.AddColumn(RoomTable, RPass, "bit", 0);
            }
            string Rtitle = "Rtitle";
            if (!DbHelperSQL.ColumnExists(RoomTable, Rtitle))
            {
                DbHelperSQL.AddColumn(RoomTable, Rtitle, "bit", 0);
            }


            //update Works set Wsid=-Rid from Room where Wsid<0 and  Wyear>=2020 and Wgrade=Rgrade and Wclass=Rclass 
        }


        public static void UpdateTable1352()
        {
            string SurveyItemtable = "SurveyItem";
            string Mblack = "Mblack";//讨论3
            if (!DbHelperSQL.ColumnExists(SurveyItemtable, Mblack))
            {
                DbHelperSQL.AddColumn(SurveyItemtable, Mblack, "bit", 0);
            }
            string SurveyQuestiontable = "SurveyQuestion";
            string Qblack = "Qblack";//讨论3
            if (!DbHelperSQL.ColumnExists(SurveyQuestiontable, Qblack))
            {
                DbHelperSQL.AddColumn(SurveyQuestiontable, Qblack, "bit", 0);
            }
        }


        public static void UpdateTable1360()
        {
            string TxtFormBacktable = "TxtFormBack";
            string rcontent = "Rcontent";//填表内容
            if (!DbHelperSQL.ColumnExists(TxtFormBacktable, rcontent))
            {
                DbHelperSQL.AddColumn(TxtFormBacktable, rcontent, "NVARCHAR(MAX)", -1);
            }
            string MissionTable = "Mission";
            string mcase = "Mcase";//任务实例内容
            if (!DbHelperSQL.ColumnExists(MissionTable, mcase))
            {
                DbHelperSQL.AddColumn(MissionTable, mcase, "NVARCHAR(MAX)", -1);
            }

            string Studentstable = "Students";
            string Steam = "Steam";//推荐组长候选人
            if (!DbHelperSQL.ColumnExists(Studentstable, Steam))
            {
                DbHelperSQL.AddColumn(Studentstable, Steam, "int", 0);
            }

            string TxtFormtable = "TxtForm";
            string Mcollabo = "Mcollabo";//填表内容
            if (!DbHelperSQL.ColumnExists(TxtFormtable, Mcollabo))
            {
                DbHelperSQL.AddColumn(TxtFormtable, Mcollabo, "bit", 0);
            }

        }


        public static void UpdateTable1365()
        {
            string MenuWorkstable = "MenuWorks";
            string kstar = "kstar";//填表内容
            if (!DbHelperSQL.ColumnExists(MenuWorkstable, kstar))
            {
                DbHelperSQL.AddColumn(MenuWorkstable, kstar, "int", -1);
            }
        }

        public static void UpdateTable1500() {

            if (!DbHelperSQL.TabExists("Folders"))
            {
                //创建菜单导航任务活动记录表
                StringBuilder Gstr = new StringBuilder();
                Gstr.Append(" create table Folders (");
                Gstr.Append(" FolderId INT IDENTITY(1,1) PRIMARY KEY, ");
                Gstr.Append(" ParentFolderId int NULL,");
                Gstr.Append(" FolderName NVARCHAR(255) NOT NULL,");
                Gstr.Append(" Path nvarchar(1000) NOT NULL,");
                Gstr.Append(" UserSnum NVARCHAR(50) NOT NULL,");
                Gstr.Append(" CreateTime DATETIME DEFAULT GETDATE(),");
                Gstr.Append(" UpdateTime DATETIME DEFAULT GETDATE() ");
                Gstr.Append(" )");

                DbHelperSQL.ExecuteSql(Gstr.ToString());
            }
            if (!DbHelperSQL.TabExists("Files"))
            {
                //创建菜单导航任务活动记录表
                StringBuilder Gstr = new StringBuilder();
                Gstr.Append(" create table Files (");
                Gstr.Append(" FileId INT IDENTITY(1,1) PRIMARY KEY, ");
                Gstr.Append(" FolderId INT NOT NULL,");
                Gstr.Append(" FileName NVARCHAR(255) NOT NULL,");
                Gstr.Append(" FileSize BIGINT NOT NULL,");
                Gstr.Append(" CreateTime DATETIME DEFAULT GETDATE(),");
                Gstr.Append(" UpdateTime DATETIME DEFAULT GETDATE(),");
                Gstr.Append(" RelativePath nvarchar(1000) NOT NULL, ");
                Gstr.Append(" UserSnum NVARCHAR(50) NOT NULL");
                Gstr.Append(" )");

                DbHelperSQL.ExecuteSql(Gstr.ToString());
            }        
        }
        public static void UpdateTable1600()
        {
            // 创建试卷表 (Exams)
            if (!DbHelperSQL.TabExists("Exams"))
            {
                StringBuilder examStr = new StringBuilder();
                examStr.Append(" CREATE TABLE [dbo].[Exams] (");
                examStr.Append(" [Eid] INT IDENTITY(1,1) PRIMARY KEY, ");
                examStr.Append(" [Etitle] NVARCHAR(200) NOT NULL, ");
                examStr.Append(" [Edescription] NVARCHAR(500) NULL, ");
                examStr.Append(" [Cid] INT NOT NULL, ");
                examStr.Append(" [Hid] INT NOT NULL, ");
                examStr.Append(" [Etime] DATETIME NULL, ");
                examStr.Append(" [Eclose] BIT DEFAULT 1, ");
                examStr.Append(" [Escore] INT DEFAULT 0, ");
                examStr.Append(" [Ecount] INT DEFAULT 0, ");
                examStr.Append(" [Edata] NVARCHAR(MAX) NOT NULL ");
                examStr.Append(" )");

                DbHelperSQL.ExecuteSql(examStr.ToString());
            }

            // 创建答题表 (Answers)
            if (!DbHelperSQL.TabExists("Answers"))
            {
                StringBuilder answerStr = new StringBuilder();
                answerStr.Append(" CREATE TABLE [dbo].[Answers] (");
                answerStr.Append(" [Aid] INT IDENTITY(1,1) PRIMARY KEY, ");
                answerStr.Append(" [Eid] INT NOT NULL, ");
                answerStr.Append(" [Asid] INT NOT NULL, ");
                answerStr.Append(" [Asnum] NVARCHAR(50)  NULL, ");
                answerStr.Append(" [Asname] NVARCHAR(50) NULL, ");
                answerStr.Append(" [Asgrade] INT NULL, ");
                answerStr.Append(" [Asclass] INT NULL, ");
                answerStr.Append(" [Atime] DATETIME NULL, ");
                answerStr.Append(" [Ascore] INT NULL, ");
                answerStr.Append(" [Aspent] INT NULL, ");
                answerStr.Append(" [Adata] NVARCHAR(MAX) NULL ");
                answerStr.Append(" )");

                DbHelperSQL.ExecuteSql(answerStr.ToString());
            }
        }

        public static void UpdateTable1700()
        {
            if (!DbHelperSQL.TabExists("AIProvider"))
            {
                StringBuilder aiStr = new StringBuilder();
                aiStr.Append(" CREATE TABLE [dbo].[AIProvider] (");
                aiStr.Append(" [Id] INT IDENTITY(1,1) PRIMARY KEY, ");
                aiStr.Append(" [DisplayName] NVARCHAR(50) NULL, ");
                aiStr.Append(" [ProviderName] NVARCHAR(50) NULL, ");
                aiStr.Append(" [ModelName] NVARCHAR(50) NULL, ");
                aiStr.Append(" [ApiKey] NVARCHAR(200) NULL, ");
                aiStr.Append(" [BaseUrl] NVARCHAR(200) NULL, ");
                aiStr.Append(" [IsDefault] BIT DEFAULT 0 ");
                aiStr.Append(" )");

                DbHelperSQL.ExecuteSql(aiStr.ToString());

                // Insert default models
                StringBuilder defaultStr = new StringBuilder();
                defaultStr.Append(" INSERT INTO [dbo].[AIProvider] (DisplayName, ProviderName, ModelName, ApiKey, BaseUrl, IsDefault) VALUES ");
                defaultStr.Append(" ('通义千问', 'Aliyun', 'qwen-max', '', 'https://dashscope.aliyuncs.com/compatible-mode/v1', 0),");
                defaultStr.Append(" ('DeepSeek', 'DeepSeek', 'deepseek-chat', '', 'https://api.deepseek.com/v1', 1),");
                defaultStr.Append(" ('智谱GLM', 'ZhipuAI', 'glm-4', '', 'https://open.bigmodel.cn/api/paas/v4', 0);");
                DbHelperSQL.ExecuteSql(defaultStr.ToString());
            }

            if (!DbHelperSQL.TabExists("IpNet"))
            {
                StringBuilder netStr = new StringBuilder();
                netStr.Append(" CREATE TABLE [dbo].[IpNet] (");
                netStr.Append(" [Nid] INT IDENTITY(1,1) PRIMARY KEY, ");
                netStr.Append(" [Nnet] NVARCHAR(50) NOT NULL, ");
                netStr.Append(" [Nhid] INT NULL, ");
                netStr.Append(" [Nname] NVARCHAR(100) NULL, ");
                netStr.Append(" [Nremark] NVARCHAR(200) NULL ");
                netStr.Append(" )");

                DbHelperSQL.ExecuteSql(netStr.ToString());
            }
        }

        public static void UpdateTable1800()
        {
            if (!DbHelperSQL.TabExists("AISkill"))
            {
                StringBuilder aiStr = new StringBuilder();
                aiStr.Append(" CREATE TABLE [dbo].[AISkill] (");
                aiStr.Append(" [Id] INT IDENTITY(1,1) PRIMARY KEY, ");
                aiStr.Append(" [SkillName] NVARCHAR(100) NULL, ");
                aiStr.Append(" [PromptContent] NVARCHAR(MAX) NULL, ");
                aiStr.Append(" [IsActive] BIT DEFAULT 1 ");
                aiStr.Append(" )");
                DbHelperSQL.ExecuteSql(aiStr.ToString());
            }
        }

        public static void UpdateTable1900()
        {
            if (!DbHelperSQL.TabExists("AICustomSkill"))
            {
                StringBuilder aiStr = new StringBuilder();
                aiStr.Append(" CREATE TABLE [dbo].[AICustomSkill] (");
                aiStr.Append(" [Id] INT IDENTITY(1,1) PRIMARY KEY, ");
                aiStr.Append(" [SkillName] NVARCHAR(100) NOT NULL, ");
                aiStr.Append(" [PromptContent] NVARCHAR(MAX) NOT NULL, ");
                aiStr.Append(" [SkillScope] NVARCHAR(200) NOT NULL DEFAULT '', ");
                aiStr.Append(" [IsActive] BIT NOT NULL DEFAULT 1 ");
                aiStr.Append(" )");
                DbHelperSQL.ExecuteSql(aiStr.ToString());

                string defaultPrompt = LearnSite.Common.AIGaugeSkillHelper.GetDefaultGaugeSkillPrompt().Replace("'", "''");
                string seedSql = "INSERT INTO [dbo].[AICustomSkill] (SkillName,PromptContent,SkillScope,IsActive) VALUES (N'" + LearnSite.Common.AIGaugeSkillHelper.GetDefaultGaugeSkillName().Replace("'", "''") + "',N'" + defaultPrompt + "',N'gauge',1)";
                DbHelperSQL.ExecuteSql(seedSql);

                string examPrompt = LearnSite.Common.AIStudentExamSkillHelper.GetDefaultSkillPrompt().Replace("'", "''");
                string examSeedSql = "INSERT INTO [dbo].[AICustomSkill] (SkillName,PromptContent,SkillScope,IsActive) VALUES (N'" + LearnSite.Common.AIStudentExamSkillHelper.GetDefaultSkillName().Replace("'", "''") + "',N'" + examPrompt + "',N'student_exam',1)";
                DbHelperSQL.ExecuteSql(examSeedSql);
            }
        }

        public static void UpdateTable1911()
        {
            if (!DbHelperSQL.TabExists("AIStudentExamAssessment"))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(" CREATE TABLE [dbo].[AIStudentExamAssessment] (");
                sb.Append(" [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,");
                sb.Append(" [Fid] INT NULL,");
                sb.Append(" [Sid] INT NULL,");
                sb.Append(" [Snum] NVARCHAR(50) NULL,");
                sb.Append(" [Sname] NVARCHAR(50) NULL,");
                sb.Append(" [Cid] INT NULL,");
                sb.Append(" [Lid] INT NULL,");
                sb.Append(" [Vid] INT NULL,");
                sb.Append(" [ProviderName] NVARCHAR(100) NULL,");
                sb.Append(" [SkillName] NVARCHAR(100) NULL,");
                sb.Append(" [Summary] NVARCHAR(500) NULL,");
                sb.Append(" [AssessmentContent] NVARCHAR(MAX) NULL,");
                sb.Append(" [LearningLog] NVARCHAR(MAX) NULL,");
                sb.Append(" [AnswerLog] NVARCHAR(MAX) NULL,");
                sb.Append(" [Score] INT NULL,");
                sb.Append(" [QuestionCount] INT NULL,");
                sb.Append(" [IsFallback] BIT NOT NULL DEFAULT 0,");
                sb.Append(" [CreatedAt] DATETIME NOT NULL DEFAULT GETDATE()");
                sb.Append(" )");
                DbHelperSQL.ExecuteSql(sb.ToString());
            }
        }

        public static void UpdateTable1912()
        {
            string surveyTable = "Survey";
            string enableAi = "Venableai";
            if (!DbHelperSQL.ColumnExists(surveyTable, enableAi))
            {
                DbHelperSQL.AddColumn(surveyTable, enableAi, "bit", 0);
                DbHelperSQL.ExecuteSql("update Survey set Venableai = 0 where Venableai is null");
            }
        }

        public static void UpdateTable1913()
        {
            if (!DbHelperSQL.TabExists("CourseActivityPlanDraft"))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(" CREATE TABLE [dbo].[CourseActivityPlanDraft] (");
                sb.Append(" [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,");
                sb.Append(" [Cid] INT NOT NULL,");
                sb.Append(" [Hid] INT NOT NULL,");
                sb.Append(" [Topic] NVARCHAR(200) NOT NULL,");
                sb.Append(" [Grade] NVARCHAR(50) NULL,");
                sb.Append(" [Duration] NVARCHAR(50) NULL,");
                sb.Append(" [TeachingGoalsInput] NVARCHAR(500) NULL,");
                sb.Append(" [ExistingCourseContentSnapshot] NVARCHAR(MAX) NULL,");
                sb.Append(" [DraftJson] NVARCHAR(MAX) NOT NULL,");
                sb.Append(" [CreatedAt] DATETIME NOT NULL DEFAULT GETDATE(),");
                sb.Append(" [UpdatedAt] DATETIME NOT NULL DEFAULT GETDATE()");
                sb.Append(" )");
                DbHelperSQL.ExecuteSql(sb.ToString());
            }

            string checkIndexSql = "select count(1) from sys.indexes where name='UX_CourseActivityPlanDraft_Cid' and object_id = object_id('CourseActivityPlanDraft')";
            if (DbHelperSQL.FindNum(checkIndexSql) == 0)
            {
                DbHelperSQL.ExecuteSql("create unique nonclustered index UX_CourseActivityPlanDraft_Cid on CourseActivityPlanDraft (Cid asc) include (Hid, UpdatedAt)");
            }
        }

        public static void UpdateTable1910()
        {
            string menuWorksTable = "MenuWorks";
            string kseconds = "Kseconds";
            if (!DbHelperSQL.ColumnExists(menuWorksTable, kseconds))
            {
                DbHelperSQL.AddColumn(menuWorksTable, kseconds, "int", 0);
                DbHelperSQL.ExecuteSql("update MenuWorks set Kseconds = isnull(Ktime, 0) * 60 where Kseconds is null or Kseconds = 0");
            }

            string checkIndexSql = "select count(1) from sys.indexes where name='IX_MenuWorks_Klid_Ksid' and object_id = object_id('MenuWorks')";
            if (DbHelperSQL.FindNum(checkIndexSql) == 0)
            {
                string createIndexSql = "create nonclustered index IX_MenuWorks_Klid_Ksid on MenuWorks (Klid asc, Ksid asc) include (Ktime, Kseconds, Kcheck, Kstar)";
                DbHelperSQL.ExecuteSql(createIndexSql);
            }

            if (!DbHelperSQL.TabExists("CheckRecords"))
            {
                StringBuilder str = new StringBuilder();
                str.Append(" CREATE TABLE [dbo].[CheckRecords] (");
                str.Append(" [Id] INT IDENTITY(1,1) PRIMARY KEY, ");
                str.Append(" [PcName] NVARCHAR(50) NULL, ");
                str.Append(" [IpAddress] NVARCHAR(50) NULL, ");
                str.Append(" [ClassName] NVARCHAR(20) NULL, ");
                str.Append(" [HasRubbish] BIT NULL, ");
                str.Append(" [DrawerClean] BIT NULL, ");
                str.Append(" [EquipmentArranged] BIT NULL, ");
                str.Append(" [ChairAdjusted] BIT NULL, ");
                str.Append(" [KeyboardMouseDamaged] BIT NULL, ");
                str.Append(" [CableUnplugged] BIT NULL, ");
                str.Append(" [PeripheralUnplugged] BIT NULL, ");
                str.Append(" [ScreenMarked] BIT NULL, ");
                str.Append(" [SubmitTime] DATETIME DEFAULT GETDATE(), ");
                str.Append(" [snid] INT NULL, ");
                str.Append(" [xuehao] NVARCHAR(20) NULL, ");
                str.Append(" [sname] NVARCHAR(20) NULL, ");
                str.Append(" [suser] NVARCHAR(50) NULL, ");
                str.Append(" [Comment] NVARCHAR(500) NULL ");
                str.Append(" )");
                DbHelperSQL.ExecuteSql(str.ToString());
            }
        }

        public static void UpdateTable1801()
        {
            if (!DbHelperSQL.TabExists("ClassInfo"))
            {
                StringBuilder str = new StringBuilder();
                str.Append(" CREATE TABLE [dbo].[ClassInfo] (");
                str.Append(" [ClassID] INT IDENTITY(1,1) PRIMARY KEY, ");
                str.Append(" [ClassName] NVARCHAR(50) NOT NULL, ");
                str.Append(" [Grade] INT NOT NULL, ");
                str.Append(" [Class] INT NOT NULL, ");
                str.Append(" [IsActive] BIT DEFAULT 1 NOT NULL ");
                str.Append(" )");
                DbHelperSQL.ExecuteSql(str.ToString());
            }
        }

        public static void UpdateTable1802()
        {
            if (!DbHelperSQL.TabExists("CourseSchedule"))
            {
                StringBuilder str = new StringBuilder();
                str.Append(" CREATE TABLE [dbo].[CourseSchedule] (");
                str.Append(" [ScheduleID] INT IDENTITY(1,1) PRIMARY KEY, ");
                str.Append(" [SchoolYear] INT NOT NULL, ");
                str.Append(" [Term] INT NOT NULL, ");
                str.Append(" [WeekDay] INT NOT NULL, ");
                str.Append(" [TimeSlot] INT NOT NULL, ");
                str.Append(" [ClassName] NVARCHAR(50) NULL, ");
                str.Append(" [Subject] NVARCHAR(50) NULL, ");
                str.Append(" [CreateTime] DATETIME NULL, ");
                str.Append(" [TeacherID] INT DEFAULT 0 NOT NULL ");
                str.Append(" )");
                DbHelperSQL.ExecuteSql(str.ToString());
            }
        }

        public static void UpdateTable1803()
        {
            if (!DbHelperSQL.TabExists("kechengbiao"))
            {
                StringBuilder str = new StringBuilder();
                str.Append(" CREATE TABLE [dbo].[kechengbiao] (");
                str.Append(" [Id] INT IDENTITY(1,1) PRIMARY KEY, ");
                str.Append(" [Year] INT NOT NULL, ");
                str.Append(" [Day1] NVARCHAR(255) NOT NULL, ");
                str.Append(" [Day2] NVARCHAR(255) NOT NULL, ");
                str.Append(" [Term] INT NOT NULL, ");
                str.Append(" [TeacherId] INT NOT NULL, ");
                str.Append(" [SubjectId] INT NOT NULL, ");
                str.Append(" [Day3] NVARCHAR(255) NOT NULL, ");
                str.Append(" [Day4] NVARCHAR(255) NOT NULL, ");
                str.Append(" [Day5] NVARCHAR(255) NOT NULL, ");
                str.Append(" [SlotNumber] INT NULL, ");
                str.Append(" [StartTime] TIME NULL, ");
                str.Append(" [EndTime] TIME NULL ");
                str.Append(" )");
                DbHelperSQL.ExecuteSql(str.ToString());
            }
        }

        public static void UpdateTable1804()
        {
            if (!DbHelperSQL.TabExists("Exam"))
            {
                StringBuilder str = new StringBuilder();
                str.Append(" CREATE TABLE [dbo].[Exam] (");
                str.Append(" [ExamId] INT IDENTITY(1,1) PRIMARY KEY, ");
                str.Append(" [ExamCode] NVARCHAR(20) NOT NULL, ");
                str.Append(" [ExamName] NVARCHAR(200) NOT NULL, ");
                str.Append(" [PaperId] INT NOT NULL, ");
                str.Append(" [ExamType] INT DEFAULT 1, ");
                str.Append(" [StartTime] DATETIME NOT NULL, ");
                str.Append(" [EndTime] DATETIME NOT NULL, ");
                str.Append(" [Duration] INT DEFAULT 60, ");
                str.Append(" [LateMinutes] INT DEFAULT 0, ");
                str.Append(" [AllowRetake] INT DEFAULT 0, ");
                str.Append(" [ShowAnswer] INT DEFAULT 0, ");
                str.Append(" [ShowScore] INT DEFAULT 1, ");
                str.Append(" [ShowRank] INT DEFAULT 0, ");
                str.Append(" [AntiCheat] NVARCHAR(MAX) NULL, ");
                str.Append(" [Password] NVARCHAR(100) NULL, ");
                str.Append(" [IpWhitelist] NVARCHAR(MAX) NULL, ");
                str.Append(" [MaxParticipants] INT DEFAULT 0, ");
                str.Append(" [ParticipantType] INT DEFAULT 1, ");
                str.Append(" [Participants] NVARCHAR(MAX) NULL, ");
                str.Append(" [Status] INT DEFAULT 0, ");
                str.Append(" [PublishTime] DATETIME NULL, ");
                str.Append(" [CreateBy] NVARCHAR(50) NULL, ");
                str.Append(" [CreateTime] DATETIME DEFAULT GETDATE(), ");
                str.Append(" [UpdateBy] NVARCHAR(50) NULL, ");
                str.Append(" [UpdateTime] DATETIME NULL, ");
                str.Append(" [TimeMode] INT DEFAULT 1, ");
                str.Append(" [ValidDays] INT DEFAULT 0 ");
                str.Append(" )");
                DbHelperSQL.ExecuteSql(str.ToString());
            }
        }

        public static void UpdateTable1805()
        {
            if (!DbHelperSQL.TabExists("ExamQuestionBank"))
            {
                StringBuilder str = new StringBuilder();
                str.Append(" CREATE TABLE [dbo].[ExamQuestionBank] (");
                str.Append(" [BankId] INT IDENTITY(1,1) PRIMARY KEY, ");
                str.Append(" [BankCode] NVARCHAR(50) NOT NULL, ");
                str.Append(" [BankName] NVARCHAR(100) NOT NULL, ");
                str.Append(" [SubjectId] INT NULL, ");
                str.Append(" [GradeId] INT NULL, ");
                str.Append(" [Description] NVARCHAR(500) NULL, ");
                str.Append(" [QuestionCount] INT DEFAULT 0, ");
                str.Append(" [Status] INT DEFAULT 1, ");
                str.Append(" [CreateBy] NVARCHAR(50) NULL, ");
                str.Append(" [CreateTime] DATETIME DEFAULT GETDATE(), ");
                str.Append(" [UpdateBy] NVARCHAR(50) NULL, ");
                str.Append(" [UpdateTime] DATETIME NULL, ");
                str.Append(" [CourseId] INT NULL ");
                str.Append(" )");
                DbHelperSQL.ExecuteSql(str.ToString());
            }
        }

        public static void UpdateTable1806()
        {
            if (!DbHelperSQL.TabExists("ExamQuestion"))
            {
                StringBuilder str = new StringBuilder();
                str.Append(" CREATE TABLE [dbo].[ExamQuestion] (");
                str.Append(" [QuestionId] BIGINT IDENTITY(1,1) PRIMARY KEY, ");
                str.Append(" [BankId] INT NOT NULL, ");
                str.Append(" [QuestionType] INT NOT NULL, ");
                str.Append(" [QuestionContent] NVARCHAR(MAX) NOT NULL, ");
                str.Append(" [QuestionText] NVARCHAR(1000) NOT NULL, ");
                str.Append(" [Options] NVARCHAR(MAX) NULL, ");
                str.Append(" [Answer] NVARCHAR(MAX) NULL, ");
                str.Append(" [QuestionConfig] NVARCHAR(MAX) NULL, ");
                str.Append(" [Analysis] NVARCHAR(MAX) NULL, ");
                str.Append(" [Score] DECIMAL(5,2) DEFAULT 2.00, ");
                str.Append(" [Difficulty] INT DEFAULT 1, ");
                str.Append(" [KnowledgePoint] NVARCHAR(200) NULL, ");
                str.Append(" [Tags] NVARCHAR(200) NULL, ");
                str.Append(" [Image] NVARCHAR(200) NULL, ");
                str.Append(" [Audio] NVARCHAR(200) NULL, ");
                str.Append(" [Video] NVARCHAR(200) NULL, ");
                str.Append(" [ParentId] BIGINT NULL, ");
                str.Append(" [SortOrder] INT DEFAULT 0, ");
                str.Append(" [Status] INT DEFAULT 1, ");
                str.Append(" [UseCount] INT DEFAULT 0, ");
                str.Append(" [CorrectRate] DECIMAL(5,2) DEFAULT 0.00, ");
                str.Append(" [CreateBy] NVARCHAR(50) NULL, ");
                str.Append(" [CreateTime] DATETIME DEFAULT GETDATE(), ");
                str.Append(" [UpdateBy] NVARCHAR(50) NULL, ");
                str.Append(" [UpdateTime] DATETIME NULL, ");
                str.Append(" [GradeId] INT NULL, ");
                str.Append(" [CourseId] INT NULL ");
                str.Append(" )");
                DbHelperSQL.ExecuteSql(str.ToString());
            }
        }

        public static void UpdateTable1807()
        {
            if (!DbHelperSQL.TabExists("ExamPaper"))
            {
                StringBuilder str = new StringBuilder();
                str.Append(" CREATE TABLE [dbo].[ExamPaper] (");
                str.Append(" [PaperId] INT IDENTITY(1,1) PRIMARY KEY, ");
                str.Append(" [PaperCode] NVARCHAR(20) NOT NULL, ");
                str.Append(" [PaperName] NVARCHAR(200) NOT NULL, ");
                str.Append(" [PaperType] INT DEFAULT 1, ");
                str.Append(" [SubjectId] INT NULL, ");
                str.Append(" [GradeId] INT NULL, ");
                str.Append(" [TotalScore] DECIMAL(5,2) DEFAULT 100.00, ");
                str.Append(" [PassScore] DECIMAL(5,2) DEFAULT 60.00, ");
                str.Append(" [QuestionCount] INT DEFAULT 0, ");
                str.Append(" [Duration] INT DEFAULT 60, ");
                str.Append(" [Description] NVARCHAR(MAX) NULL, ");
                str.Append(" [Sections] NVARCHAR(MAX) NULL, ");
                str.Append(" [RandomConfig] NVARCHAR(MAX) NULL, ");
                str.Append(" [Status] INT DEFAULT 0, ");
                str.Append(" [CreateBy] NVARCHAR(50) NULL, ");
                str.Append(" [CreateTime] DATETIME DEFAULT GETDATE(), ");
                str.Append(" [UpdateBy] NVARCHAR(50) NULL, ");
                str.Append(" [UpdateTime] DATETIME NULL ");
                str.Append(" )");
                DbHelperSQL.ExecuteSql(str.ToString());
            }
        }

        public static void UpdateTable1808()
        {
            if (!DbHelperSQL.TabExists("ExamPaperQuestion"))
            {
                StringBuilder str = new StringBuilder();
                str.Append(" CREATE TABLE [dbo].[ExamPaperQuestion] (");
                str.Append(" [Id] BIGINT IDENTITY(1,1) PRIMARY KEY, ");
                str.Append(" [PaperId] INT NOT NULL, ");
                str.Append(" [QuestionId] BIGINT NOT NULL, ");
                str.Append(" [SectionName] NVARCHAR(100) NULL, ");
                str.Append(" [Score] DECIMAL(5,2) NOT NULL, ");
                str.Append(" [SortOrder] INT DEFAULT 0 ");
                str.Append(" )");
                DbHelperSQL.ExecuteSql(str.ToString());
            }
        }

        public static void UpdateTable1809()
        {
            if (!DbHelperSQL.TabExists("ExamAnswer"))
            {
                StringBuilder str = new StringBuilder();
                str.Append(" CREATE TABLE [dbo].[ExamAnswer] (");
                str.Append(" [AnswerId] BIGINT IDENTITY(1,1) PRIMARY KEY, ");
                str.Append(" [ExamId] INT NOT NULL, ");
                str.Append(" [PaperId] INT NOT NULL, ");
                str.Append(" [StudentId] NVARCHAR(50) NOT NULL, ");
                str.Append(" [StudentName] NVARCHAR(50) NULL, ");
                str.Append(" [ClassId] INT NULL, ");
                str.Append(" [Answers] NVARCHAR(MAX) NULL, ");
                str.Append(" [TempAnswers] NVARCHAR(MAX) NULL, ");
                str.Append(" [RandomQuestions] NVARCHAR(MAX) NULL, ");
                str.Append(" [StartTime] DATETIME NOT NULL, ");
                str.Append(" [SubmitTime] DATETIME NULL, ");
                str.Append(" [Duration] INT DEFAULT 0, ");
                str.Append(" [TotalScore] DECIMAL(5,2) DEFAULT 0, ");
                str.Append(" [ObjectiveScore] DECIMAL(5,2) DEFAULT 0, ");
                str.Append(" [SubjectiveScore] DECIMAL(5,2) DEFAULT 0, ");
                str.Append(" [ScoreDetails] NVARCHAR(MAX) NULL, ");
                str.Append(" [Status] INT DEFAULT 0, ");
                str.Append(" [IpAddress] NVARCHAR(50) NULL, ");
                str.Append(" [UserAgent] NVARCHAR(500) NULL, ");
                str.Append(" [MarkedBy] NVARCHAR(50) NULL, ");
                str.Append(" [MarkTime] DATETIME NULL, ");
                str.Append(" [Remark] NVARCHAR(500) NULL ");
                str.Append(" )");
                DbHelperSQL.ExecuteSql(str.ToString());
            }
        }

        public static void UpdateTable1810()
        {
            if (!DbHelperSQL.TabExists("ExamAnswerLog"))
            {
                StringBuilder str = new StringBuilder();
                str.Append(" CREATE TABLE [dbo].[ExamAnswerLog] (");
                str.Append(" [LogId] BIGINT IDENTITY(1,1) PRIMARY KEY, ");
                str.Append(" [AnswerId] BIGINT NOT NULL, ");
                str.Append(" [EventType] INT NOT NULL, ");
                str.Append(" [EventData] NVARCHAR(MAX) NULL, ");
                str.Append(" [EventTime] DATETIME DEFAULT GETDATE() ");
                str.Append(" )");
                DbHelperSQL.ExecuteSql(str.ToString());
            }
        }

        public static void UpdateTable1811()
        {
            if (!DbHelperSQL.TabExists("ExamResult"))
            {
                StringBuilder str = new StringBuilder();
                str.Append(" CREATE TABLE [dbo].[ExamResult] (");
                str.Append(" [ResultId] BIGINT IDENTITY(1,1) PRIMARY KEY, ");
                str.Append(" [ExamId] INT NOT NULL, ");
                str.Append(" [AnswerId] BIGINT NOT NULL, ");
                str.Append(" [StudentId] NVARCHAR(50) NOT NULL, ");
                str.Append(" [StudentName] NVARCHAR(50) NULL, ");
                str.Append(" [ClassId] INT NULL, ");
                str.Append(" [TotalScore] DECIMAL(5,2) DEFAULT 0, ");
                str.Append(" [ObjectiveScore] DECIMAL(5,2) DEFAULT 0, ");
                str.Append(" [SubjectiveScore] DECIMAL(5,2) DEFAULT 0, ");
                str.Append(" [RankInClass] INT NULL, ");
                str.Append(" [RankInGrade] INT NULL, ");
                str.Append(" [CorrectCount] INT DEFAULT 0, ");
                str.Append(" [WrongCount] INT DEFAULT 0, ");
                str.Append(" [PartialCount] INT DEFAULT 0, ");
                str.Append(" [Duration] INT DEFAULT 0, ");
                str.Append(" [SubmitTime] DATETIME NULL, ");
                str.Append(" [Status] INT DEFAULT 1 ");
                str.Append(" )");
                DbHelperSQL.ExecuteSql(str.ToString());
            }
        }

        public static void UpdateTable1812()
        {
            if (!DbHelperSQL.TabExists("ExamDictQuestionType"))
            {
                StringBuilder str = new StringBuilder();
                str.Append(" CREATE TABLE [dbo].[ExamDictQuestionType] (");
                str.Append(" [TypeId] INT NOT NULL PRIMARY KEY, ");
                str.Append(" [TypeName] NVARCHAR(50) NOT NULL, ");
                str.Append(" [TypeCode] NVARCHAR(20) NOT NULL, ");
                str.Append(" [HasOptions] INT DEFAULT 0, ");
                str.Append(" [AutoScore] INT DEFAULT 1, ");
                str.Append(" [SortOrder] INT DEFAULT 0, ");
                str.Append(" [Status] INT DEFAULT 1 ");
                str.Append(" )");
                DbHelperSQL.ExecuteSql(str.ToString());
            }
        }

        public static void UpdateTable1813()
        {
            if (!DbHelperSQL.TabExists("ExamSurvey"))
            {
                StringBuilder str = new StringBuilder();
                str.Append(" CREATE TABLE [dbo].[ExamSurvey] (");
                str.Append(" [SurveyId] INT IDENTITY(1,1) PRIMARY KEY, ");
                str.Append(" [SurveyCode] NVARCHAR(20) NOT NULL, ");
                str.Append(" [SurveyName] NVARCHAR(200) NOT NULL, ");
                str.Append(" [SurveyType] INT DEFAULT 1, ");
                str.Append(" [Questions] NVARCHAR(MAX) NOT NULL, ");
                str.Append(" [Settings] NVARCHAR(MAX) NULL, ");
                str.Append(" [StartTime] DATETIME NULL, ");
                str.Append(" [EndTime] DATETIME NULL, ");
                str.Append(" [Anonymous] INT DEFAULT 0, ");
                str.Append(" [Status] INT DEFAULT 0, ");
                str.Append(" [CreateBy] NVARCHAR(50) NULL, ");
                str.Append(" [CreateTime] DATETIME DEFAULT GETDATE() ");
                str.Append(" )");
                DbHelperSQL.ExecuteSql(str.ToString());
            }
        }

        public static void UpdateTable1814()
        {
            if (!DbHelperSQL.TabExists("ExamSurveyAnswer"))
            {
                StringBuilder str = new StringBuilder();
                str.Append(" CREATE TABLE [dbo].[ExamSurveyAnswer] (");
                str.Append(" [AnswerId] BIGINT IDENTITY(1,1) PRIMARY KEY, ");
                str.Append(" [SurveyId] INT NOT NULL, ");
                str.Append(" [UserId] NVARCHAR(50) NULL, ");
                str.Append(" [Answers] NVARCHAR(MAX) NOT NULL, ");
                str.Append(" [SubmitTime] DATETIME DEFAULT GETDATE(), ");
                str.Append(" [IpAddress] NVARCHAR(50) NULL ");
                str.Append(" )");
                DbHelperSQL.ExecuteSql(str.ToString());
            }
        }

        public static void UpdateTable1815()
        {
            if (!DbHelperSQL.TabExists("HonorBoardSettings"))
            {
                StringBuilder str = new StringBuilder();
                str.Append(" CREATE TABLE [dbo].[HonorBoardSettings] (");
                str.Append(" [ID] INT IDENTITY(1,1) PRIMARY KEY, ");
                str.Append(" [Syear] NVARCHAR(10) NOT NULL, ");
                str.Append(" [Scope] NVARCHAR(20) NOT NULL, ");
                str.Append(" [Sgrade] INT NULL, ");
                str.Append(" [Sclass] INT NULL, ");
                str.Append(" [CreateTime] DATETIME NULL, ");
                str.Append(" [UpdateTime] DATETIME NULL ");
                str.Append(" )");
                DbHelperSQL.ExecuteSql(str.ToString());
            }
        }

        public static void UpdateTable1816()
        {
            if (!DbHelperSQL.TabExists("HonorConfig"))
            {
                StringBuilder str = new StringBuilder();
                str.Append(" CREATE TABLE [dbo].[HonorConfig] (");
                str.Append(" [HonorCode] NVARCHAR(50) NOT NULL PRIMARY KEY, ");
                str.Append(" [HonorName] NVARCHAR(50) NOT NULL, ");
                str.Append(" [HonorType] NVARCHAR(50) NOT NULL, ");
                str.Append(" [IconClass] NVARCHAR(100) NULL, ");
                str.Append(" [IconEmoji] NVARCHAR(10) NULL, ");
                str.Append(" [Description] NVARCHAR(200) NULL, ");
                str.Append(" [IsActive] BIT DEFAULT 1, ");
                str.Append(" [SortOrder] INT NULL ");
                str.Append(" )");
                DbHelperSQL.ExecuteSql(str.ToString());
            }
        }

        public static void UpdateTable1817()
        {
            if (!DbHelperSQL.TabExists("StudentHonors"))
            {
                StringBuilder str = new StringBuilder();
                str.Append(" CREATE TABLE [dbo].[StudentHonors] (");
                str.Append(" [ID] INT IDENTITY(1,1) PRIMARY KEY, ");
                str.Append(" [Snum] NVARCHAR(50) NOT NULL, ");
                str.Append(" [HonorCode] NVARCHAR(50) NOT NULL, ");
                str.Append(" [HonorLevel] INT DEFAULT 1 NOT NULL, ");
                str.Append(" [EarnDate] DATETIME DEFAULT GETDATE() NOT NULL, ");
                str.Append(" [EarnCount] INT DEFAULT 1 NOT NULL, ");
                str.Append(" [Continuous] INT DEFAULT 1 NOT NULL, ");
                str.Append(" [Term] NVARCHAR(20) NULL, ");
                str.Append(" [Remarks] NVARCHAR(500) NULL ");
                str.Append(" )");
                DbHelperSQL.ExecuteSql(str.ToString());
            }
        }

        public static void UpdateTable1818()
        {
            if (!DbHelperSQL.TabExists("performance_score"))
            {
                StringBuilder str = new StringBuilder();
                str.Append(" CREATE TABLE [dbo].[performance_score] (");
                str.Append(" [Id] INT IDENTITY(1,1) PRIMARY KEY, ");
                str.Append(" [StudentId] NVARCHAR(50) NOT NULL, ");
                str.Append(" [StudentName] NVARCHAR(50) NOT NULL, ");
                str.Append(" [Score] INT NOT NULL, ");
                str.Append(" [Reason] NVARCHAR(255) NULL, ");
                str.Append(" [CreateTime] DATETIME DEFAULT GETDATE() ");
                str.Append(" )");
                DbHelperSQL.ExecuteSql(str.ToString());
            }
        }

        public static void UpdateTable1819()
        {
            if (!DbHelperSQL.TabExists("Skdj"))
            {
                StringBuilder str = new StringBuilder();
                str.Append(" CREATE TABLE [dbo].[Skdj] (");
                str.Append(" [Ssid] INT IDENTITY(1,1) PRIMARY KEY, ");
                str.Append(" [Sstid] INT NULL, ");
                str.Append(" [Ssdate] DATETIME NULL, ");
                str.Append(" [SSyear] INT NULL, ");
                str.Append(" [SSmonth] INT NULL, ");
                str.Append(" [SSday] INT NULL, ");
                str.Append(" [SSweek] NVARCHAR(50) NULL, ");
                str.Append(" [Ssession] NVARCHAR(20) NULL, ");
                str.Append(" [SSgrade] INT NULL, ");
                str.Append(" [SSclass] INT NULL, ");
                str.Append(" [Ssctitle] NVARCHAR(MAX) NULL, ");
                str.Append(" [Sstname] NVARCHAR(50) NULL, ");
                str.Append(" [Ssnotes] NVARCHAR(MAX) NULL ");
                str.Append(" )");
                DbHelperSQL.ExecuteSql(str.ToString());
            }
        }

        public static void UpdateTable1820()
        {
            if (!DbHelperSQL.TabExists("student_scores"))
            {
                StringBuilder str = new StringBuilder();
                str.Append(" CREATE TABLE [dbo].[student_scores] (");
                str.Append(" [ID] INT IDENTITY(1,1) PRIMARY KEY, ");
                str.Append(" [StudentID] NVARCHAR(20) NOT NULL, ");
                str.Append(" [Qattitude] DECIMAL(5,2) DEFAULT 0, ");
                str.Append(" [Wscore] DECIMAL(5,2) DEFAULT 0, ");
                str.Append(" [CreateDate] DATE DEFAULT GETDATE(), ");
                str.Append(" [CreatedAt] DATETIME DEFAULT GETDATE() ");
                str.Append(" )");
                DbHelperSQL.ExecuteSql(str.ToString());
            }
        }

        public static void UpdateTable1821()
        {
            if (!DbHelperSQL.TabExists("Teachers"))
            {
                StringBuilder str = new StringBuilder();
                str.Append(" CREATE TABLE [dbo].[Teachers] (");
                str.Append(" [Tid] INT IDENTITY(1,1) PRIMARY KEY, ");
                str.Append(" [TeacherName] NVARCHAR(50) NULL, ");
                str.Append(" [TeacherPwd] NVARCHAR(50) NULL, ");
                str.Append(" [Theme] NVARCHAR(50) NULL, ");
                str.Append(" [Tdate] DATETIME NULL ");
                str.Append(" )");
                DbHelperSQL.ExecuteSql(str.ToString());
            }
        }

        public static void UpdateTable1822()
        {
            if (!DbHelperSQL.TabExists("TempSeat"))
            {
                StringBuilder str = new StringBuilder();
                str.Append(" CREATE TABLE [dbo].[TempSeat] (");
                str.Append(" [Tid] INT IDENTITY(1,1) PRIMARY KEY, ");
                str.Append(" [Snum] NVARCHAR(50) NOT NULL, ");
                str.Append(" [TempIp] NVARCHAR(50) NULL, ");
                str.Append(" [TempSeat] NVARCHAR(50) NULL, ");
                str.Append(" [ExpireTime] DATETIME NOT NULL, ");
                str.Append(" [CreateTime] DATETIME NOT NULL ");
                str.Append(" )");
                DbHelperSQL.ExecuteSql(str.ToString());
            }
        }

        public static void UpdateTable1823()
        {
            if (!DbHelperSQL.TabExists("ThemeSettings"))
            {
                StringBuilder str = new StringBuilder();
                str.Append(" CREATE TABLE [dbo].[ThemeSettings] (");
                str.Append(" [SettingId] INT IDENTITY(1,1) PRIMARY KEY, ");
                str.Append(" [SettingKey] NVARCHAR(50) NULL, ");
                str.Append(" [SettingValue] NVARCHAR(200) NULL, ");
                str.Append(" [SettingDesc] NVARCHAR(200) NULL, ");
                str.Append(" [CreatedDate] DATETIME NULL ");
                str.Append(" )");
                DbHelperSQL.ExecuteSql(str.ToString());
            }
        }

        public static void UpdateTable1824()
        {
            if (!DbHelperSQL.TabExists("TimeSlots"))
            {
                StringBuilder str = new StringBuilder();
                str.Append(" CREATE TABLE [dbo].[TimeSlots] (");
                str.Append(" [SlotID] INT IDENTITY(1,1) PRIMARY KEY, ");
                str.Append(" [SlotName] NVARCHAR(100) NOT NULL, ");
                str.Append(" [StartTime] TIME NOT NULL, ");
                str.Append(" [EndTime] TIME NOT NULL, ");
                str.Append(" [DisplayOrder] INT NOT NULL ");
                str.Append(" )");
                DbHelperSQL.ExecuteSql(str.ToString());
            }
        }

        public static void UpdateTable1825()
        {
            string QuizTable = "Quiz";
            string[] fields = { "BankId", "Difficulty", "Tags", "OptionsJson", "QuestionConfig", "KnowledgePoint", "UseCount" };
            string[] types = { "int", "int", "nvarchar(500)", "nvarchar(MAX)", "nvarchar(MAX)", "nvarchar(200)", "int" };
            object[] defaults = { null, 1, null, null, null, null, 0 };
            
            for (int i = 0; i < fields.Length; i++)
            {
                if (!DbHelperSQL.ColumnExists(QuizTable, fields[i]))
                {
                    DbHelperSQL.AddColumn(QuizTable, fields[i], types[i], defaults[i]);
                }
            }
        }

        public static void UpdateTable1826()
        {
            string QuizGradeTable = "QuizGrade";
            string[] fields = { "Qfill", "Qmatch", "Qcategory", "Qessay", "Qnpfield", "Qselectfield", "Qscorefield", "Qmatrixfield", "Qmultiblankfield" };
            string type = "int";
            object defaultVal = 1;
            
            for (int i = 0; i < fields.Length; i++)
            {
                if (!DbHelperSQL.ColumnExists(QuizGradeTable, fields[i]))
                {
                    if (fields[i] == "Qnpfield" || fields[i] == "Qselectfield" || fields[i] == "Qscorefield" || fields[i] == "Qmatrixfield")
                        defaultVal = 0;
                    else
                        defaultVal = 1;
                    DbHelperSQL.AddColumn(QuizGradeTable, fields[i], type, defaultVal);
                }
            }
        }

        public static void UpdateTable1827()
        {
            string ResultTable = "Result";
            string Ranswer = "Ranswer";
            if (!DbHelperSQL.ColumnExists(ResultTable, Ranswer))
            {
                DbHelperSQL.AddColumn(ResultTable, Ranswer, "NVARCHAR(MAX)", null);
            }
        }

        public static void UpdateTable1828()
        {
            string RoomTable = "Room";
            string[] fields = { "ClassName", "Campus", "Rinternet" };
            string[] types = { "nvarchar(100)", "nvarchar(100)", "bit" };
            object[] defaults = { null, "", 0 };
            
            for (int i = 0; i < fields.Length; i++)
            {
                if (!DbHelperSQL.ColumnExists(RoomTable, fields[i]))
                {
                    DbHelperSQL.AddColumn(RoomTable, fields[i], types[i], defaults[i]);
                }
            }
        }

        public static void UpdateTable1829()
        {
            string SigninTable = "Signin";
            string[] fields = { "Qtitle", "Qsession", "Qonline" };
            string[] types = { "nvarchar(MAX)", "nvarchar(50)", "bit" };
            object[] defaults = { null, null, 0 };
            
            for (int i = 0; i < fields.Length; i++)
            {
                if (!DbHelperSQL.ColumnExists(SigninTable, fields[i]))
                {
                    DbHelperSQL.AddColumn(SigninTable, fields[i], types[i], defaults[i]);
                }
            }
        }

        public static void UpdateTable1830()
        {
            string StudentsTable = "Students";
            string[] fields = { "SigninCount", "Sfixedip", "Sseat", "Theme" };
            string[] types = { "int", "nvarchar(50)", "nvarchar(20)", "nvarchar(50)" };
            object[] defaults = { 0, null, null, null };
            
            for (int i = 0; i < fields.Length; i++)
            {
                if (!DbHelperSQL.ColumnExists(StudentsTable, fields[i]))
                {
                    DbHelperSQL.AddColumn(StudentsTable, fields[i], types[i], defaults[i]);
                }
            }
        }

        public static void UpdateTable1831()
        {
            string SurveyTable = "Survey";
            string[] fields = { "YunXuXueShengChuTi", "Qscorefield", "Qnpfield", "Qmatrixfield", "Qselectfield" };
            string[] types = { "bit", "int", "int", "int", "int" };
            object[] defaults = { 0, 0, 0, 0, 0 };
            
            for (int i = 0; i < fields.Length; i++)
            {
                if (!DbHelperSQL.ColumnExists(SurveyTable, fields[i]))
                {
                    DbHelperSQL.AddColumn(SurveyTable, fields[i], types[i], defaults[i]);
                }
            }
        }

        public static void UpdateTable1832()
        {
            string SurveyFeedbackTable = "SurveyFeedback";
            string[] fields = { "FBlanks", "FError", "FCiShu" };
            string[] types = { "nvarchar(MAX)", "nvarchar(MAX)", "int" };
            object[] defaults = { null, null, 0 };
            
            for (int i = 0; i < fields.Length; i++)
            {
                if (!DbHelperSQL.ColumnExists(SurveyFeedbackTable, fields[i]))
                {
                    DbHelperSQL.AddColumn(SurveyFeedbackTable, fields[i], types[i], defaults[i]);
                }
            }
        }

        public static void UpdateTable1833()
        {
            string SurveyItemTable = "SurveyItem";
            string[] fields = { "Image", "SortOrder" };
            string[] types = { "nvarchar(500)", "int" };
            object[] defaults = { null, 0 };
            
            for (int i = 0; i < fields.Length; i++)
            {
                if (!DbHelperSQL.ColumnExists(SurveyItemTable, fields[i]))
                {
                    DbHelperSQL.AddColumn(SurveyItemTable, fields[i], types[i], defaults[i]);
                }
            }
        }

        public static void UpdateTable1834()
        {
        }

        /// <summary>
        /// 更新Survey表，添加Venableai列
        /// </summary>
        public static void UpdateTableSurveyVenableai()
        {
            string SurveyTable = "Survey";
            string Venableai = "Venableai";
            if (!DbHelperSQL.ColumnExists(SurveyTable, Venableai))
            {
                DbHelperSQL.AddColumn(SurveyTable, Venableai, "bit", 0);
            }
        }

        public static void UpdateTableSurveyQuestionFields()
        {
            string SurveyQuestionTable = "SurveyQuestion";
            string[] fields = { "ChuTiRen", "ChuTiRenID", "ZhuangTai", "DianZan", "CanKaoYe", "Qtype", "QuestionConfig", "MinLength", "MaxLength", "Required", "SortOrder" };
            string[] types = { "nvarchar(50)", "nvarchar(50)", "int", "nvarchar(MAX)", "nvarchar(MAX)", "int", "nvarchar(MAX)", "int", "int", "bit", "int" };
            object[] defaults = { null, null, 1, null, null, 0, null, null, null, 1, 0 };
            
            for (int i = 0; i < fields.Length; i++)
            {
                if (!DbHelperSQL.ColumnExists(SurveyQuestionTable, fields[i]))
                {
                    DbHelperSQL.AddColumn(SurveyQuestionTable, fields[i], types[i], defaults[i]);
                }
            }
        }
    }
}
