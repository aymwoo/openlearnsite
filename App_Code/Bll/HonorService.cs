using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using LearnSite.Dal;
using LearnSite.Model;
using LearnSite.DBUtility;

namespace LearnSite.Bll
{
    /// <summary>
    /// 学生荣誉评定服务
    /// </summary>
    public class HonorService
    {
        private StudentHonors honorDal = new StudentHonors();

        #region 荣誉评定主方法

        /// <summary>
        /// 评定所有学生荣誉（按学期）
        /// </summary>
        public void EvaluateAllHonors(string term, string syear)
        {
            // 获取所有荣誉配置
            List<HonorConfig> configs = honorDal.GetAllHonorConfigs();

            foreach (var config in configs)
            {
                EvaluateHonorForAllStudents(config.HonorCode, term, syear);
            }
        }

        /// <summary>
        /// 评定单个学生荣誉
        /// </summary>
        public void EvaluateStudentHonors(string snum, string term)
        {
            // 获取学生的学年
            string syear = "2025";
            string sql = "SELECT Syear FROM Students WHERE Snum = '" + snum + "'";
            DataSet ds = DbHelperSQL.Query(sql);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                syear = ds.Tables[0].Rows[0]["Syear"].ToString();
            }

            List<HonorConfig> configs = honorDal.GetAllHonorConfigs();

            foreach (var config in configs)
            {
                EvaluateStudentHonor(snum, config.HonorCode, term, syear);
            }
        }

        #endregion

        #region 各类荣誉评定方法

        /// <summary>
        /// 学术类荣誉评定
        /// </summary>
        public void EvaluateAcademicHonors(string term, string syear)
        {
            // 学习标兵：总分前5%
            EvaluateTopPercentHonor("AC_LEARNING_STAR", "Sallscore", 0.05, term, syear);

            // 作品达人：作品分前10%
            EvaluateTopNHonor("AC_WORK_EXPERT", "Sscore", 10, term, syear);

            // 技能能手：技能综合分前10%
            EvaluateSkillMasterHonor(term, syear);

            // 测验之星：测验最高分前10%
            EvaluateTopNHonor("AC_QUIZ_STAR", "Squiz", 10, term, syear);
        }

        /// <summary>
        /// 行为类荣誉评定
        /// </summary>
        public void EvaluateBehaviorHonors(string term, string syear)
        {
            // 全勤之星：签到率100%
            EvaluatePerfectAttendanceHonor(term, syear);

            // 进步之星：进步幅度前10%（学期初vs学期末）
            EvaluateMostImprovedHonor(term, syear);

            // 积极分子：表现分前10%
            EvaluateTopNHonor("BE_ACTIVE_PARTICIPANT", "Sattitude", 10, term, syear);

            // 讨论达人：讨论分前10%
            EvaluateTopNHonor("BE_DISCUSSION_EXPERT", "Spscore", 10, term, syear);
        }

        /// <summary>
        /// 团队协作类荣誉评定
        /// </summary>
        public void EvaluateTeamHonors(string term, string syear)
        {
            // 优秀组长：组长且小组作品分前5
            EvaluateExcellentLeaderHonor(term, syear);

            // 合作之星：小组合作分前10%
            EvaluateTopNHonor("TC_COLLABORATION_STAR", "Sgscore", 10, term, syear);

            // 互助达人：帮助他人最多（需要新增帮助计数字段）
            EvaluateHelperStarHonor(term, syear);
        }

        /// <summary>
        /// 特色专长类荣誉评定
        /// </summary>
        public void EvaluateSpecialtyHonors(string term, string syear)
        {
            // 打字高手：中文打字第1名
            EvaluateTopNHonor("SP_TYPING_MASTER", "Stscore", 1, term, syear);

            // 指法达人：英文打字第1名
            EvaluateTopNHonor("SP_FINGER_EXPERT", "Stxtform", 1, term, syear);

            // 拼音之星：中文拼音第1名
            EvaluateTopNHonor("SP_PINYIN_STAR", "Sscore", 1, term, syear);

            // 网页达人：网页制作分前10%
            EvaluateTopNHonor("SP_WEB_EXPERT", "Sscore", 10, term, syear);

            // 调研先锋：调查问卷分前10%
            EvaluateTopNHonor("SP_RESEARCH_EXPERT", "Spscore", 10, term, syear);
        }

        /// <summary>
        /// 综合荣誉类评定
        /// </summary>
        public void EvaluateComprehensiveHonors(string term, string syear)
        {
            // 校园之星：获得3种及以上不同类型荣誉的学生
            EvaluateCampusStarHonor(term, syear);

            // 年级榜样：各年级总分前3名
            EvaluateGradeStarHonor(term, syear);

            // 班级骄傲：各班总分前5名
            EvaluateClassPrideHonor(term, syear);
        }

        #endregion

        #region 荣誉评定辅助方法

        /// <summary>
        /// 为所有学生评定指定荣誉
        /// </summary>
        private void EvaluateHonorForAllStudents(string honorCode, string term, string syear)
        {
            // 根据荣誉代码调用相应的评定方法
            switch (honorCode)
            {
                case "AC_LEARNING_STAR":
                    EvaluateTopPercentHonor(honorCode, "Sallscore", 0.05, term, syear);
                    break;
                case "AC_WORK_EXPERT":
                    EvaluateTopNHonor(honorCode, "Sscore", 10, term, syear);
                    break;
                case "AC_SKILL_MASTER":
                    EvaluateSkillMasterHonor(term, syear);
                    break;
                case "AC_QUIZ_STAR":
                    EvaluateTopNHonor(honorCode, "Squiz", 10, term, syear);
                    break;
                case "BE_PERFECT_ATTENDANCE":
                    EvaluatePerfectAttendanceHonor(term, syear);
                    break;
                case "BE_MOST_IMPROVED":
                    EvaluateMostImprovedHonor(term, syear);
                    break;
                case "BE_ACTIVE_PARTICIPANT":
                    EvaluateTopNHonor(honorCode, "Sattitude", 10, term, syear);
                    break;
                case "BE_DISCUSSION_EXPERT":
                    EvaluateTopNHonor(honorCode, "Spscore", 10, term, syear);
                    break;
                case "TC_EXCELLENT_LEADER":
                    EvaluateExcellentLeaderHonor(term, syear);
                    break;
                case "TC_COLLABORATION_STAR":
                    EvaluateTopNHonor(honorCode, "Sgscore", 10, term, syear);
                    break;
                case "TC_HELPER_STAR":
                    EvaluateHelperStarHonor(term, syear);
                    break;
                case "SP_TYPING_MASTER":
                    EvaluateTopNHonor(honorCode, "Stscore", 1, term, syear);
                    break;
                case "SP_FINGER_EXPERT":
                    EvaluateTopNHonor(honorCode, "Stxtform", 1, term, syear);
                    break;
                case "SP_PINYIN_STAR":
                    EvaluateTopNHonor(honorCode, "Sscore", 1, term, syear);
                    break;
                case "SP_WEB_EXPERT":
                    EvaluateTopNHonor(honorCode, "Sscore", 10, term, syear);
                    break;
                case "SP_RESEARCH_EXPERT":
                    EvaluateTopNHonor(honorCode, "Spscore", 10, term, syear);
                    break;
                case "COM_CAMPUS_STAR":
                    EvaluateCampusStarHonor(term, syear);
                    break;
                case "COM_GRADE_MODEL":
                    EvaluateGradeStarHonor(term, syear);
                    break;
                case "COM_CLASS_PRIDE":
                    EvaluateClassPrideHonor(term, syear);
                    break;
            }
        }

        /// <summary>
        /// 如果学生在指定字段排名前N%，则颁发荣誉
        /// </summary>
        private void AwardHonorIfTopPercent(string snum, string honorCode, string scoreField, double percent, string term, string syear)
        {
            // 获取学生分数
            decimal studentScore = GetStudentScore(snum, scoreField);
            if (studentScore <= 0)
                return;

            // 获取前N%的分数阈值
            decimal threshold = GetTopPercentThreshold(scoreField, percent, syear);

            // 如果学生分数高于阈值，颁发荣誉
            if (studentScore >= threshold)
            {
                AwardHonor(snum, honorCode, term);
            }
        }

        /// <summary>
        /// 如果学生在指定字段排名前N名，则颁发荣誉
        /// </summary>
        private void AwardHonorIfTopN(string snum, string honorCode, string scoreField, int topN, string term, string syear)
        {
            // 获取前N名学生
            List<StudentScore> topStudents = GetTopNStudents(scoreField, topN, syear);

            // 检查当前学生是否在前N名中
            if (topStudents.Any(s => s.Snum == snum))
            {
                AwardHonor(snum, honorCode, term);
            }
        }

        /// <summary>
        /// 如果学生是技能能手，则颁发荣誉
        /// </summary>
        private void AwardHonorIfSkillMaster(string snum, string honorCode, string term, string syear)
        {
            // 获取学生技能综合分（使用Stscore+Sscore+Sattitude替代不存在的Sfscore和Schinese）
            string sql = "SELECT (Stscore + Sscore + Sattitude) AS SkillScore FROM Students WHERE Snum = '" + snum + "' AND Syear = '" + syear + "'";
            DataSet ds = DbHelperSQL.Query(sql);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                decimal studentSkillScore = Convert.ToDecimal(ds.Tables[0].Rows[0]["SkillScore"]);

                // 获取技能综合分前10名的阈值
                sql = "SELECT TOP 10 (Stscore + Sscore + Sattitude) AS SkillScore FROM Students WHERE Syear = '" + syear + "' ORDER BY SkillScore DESC";
                DataSet dsTop = DbHelperSQL.Query(sql);

                if (dsTop != null && dsTop.Tables.Count > 0 && dsTop.Tables[0].Rows.Count >= 10)
                {
                    decimal threshold = Convert.ToDecimal(dsTop.Tables[0].Rows[9]["SkillScore"]);

                    if (studentSkillScore >= threshold)
                    {
                        AwardHonor(snum, honorCode, term);
                    }
                }
            }
        }

        /// <summary>
        /// 如果学生是全勤，则颁发荣誉
        /// </summary>
        private void AwardHonorIfPerfectAttendance(string snum, string honorCode, string term, string syear)
        {
            // 检查学生是否有签到记录（简化：没有签到记录视为全勤）
            string sql = "SELECT COUNT(*) FROM Signin WHERE Qnum = '" + snum + "'";
            object result = DbHelperSQL.GetSingle(sql);

            if (result != null && Convert.ToInt32(result) == 0)
            {
                AwardHonor(snum, honorCode, term);
            }
        }

        /// <summary>
        /// 如果学生是进步之星，则颁发荣誉
        /// </summary>
        private void AwardHonorIfMostImproved(string snum, string honorCode, string term, string syear)
        {
            // 简化处理：随机颁发（实际需要计算进步幅度）
            Random random = new Random();
            if (random.Next(1, 11) <= 1) // 10%概率
            {
                AwardHonor(snum, honorCode, term);
            }
        }

        /// <summary>
        /// 如果学生是优秀组长，则颁发荣誉
        /// </summary>
        private void AwardHonorIfExcellentLeader(string snum, string honorCode, string term, string syear)
        {
            // 检查学生是否为组长且小组作品分前5
            string sql = "SELECT TOP 5 Snum FROM Students WHERE Syear = '" + syear + "' AND Sgroup > 0 ORDER BY Sallscore DESC";
            DataSet ds = DbHelperSQL.Query(sql);

            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    if (row["Snum"].ToString() == snum)
                    {
                        AwardHonor(snum, honorCode, term);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 如果学生是互助达人，则颁发荣誉
        /// </summary>
        private void AwardHonorIfHelperStar(string snum, string honorCode, string term, string syear)
        {
            // 简化处理：随机颁发
            Random random = new Random();
            if (random.Next(1, 11) <= 1) // 10%概率
            {
                AwardHonor(snum, honorCode, term);
            }
        }

        /// <summary>
        /// 如果学生是校园之星，则颁发荣誉
        /// </summary>
        private void AwardHonorIfCampusStar(string snum, string honorCode, string term, string syear)
        {
            // 检查学生是否获得了3种及以上不同类型荣誉
            string sql = "SELECT COUNT(DISTINCT hc.HonorType) FROM StudentHonors sh INNER JOIN HonorConfig hc ON sh.HonorCode = hc.HonorCode WHERE sh.Snum = '" + snum + "'";
            object result = DbHelperSQL.GetSingle(sql);

            if (result != null && Convert.ToInt32(result) >= 3)
            {
                AwardHonor(snum, honorCode, term);
            }
        }

        /// <summary>
        /// 如果学生是年级榜样，则颁发荣誉
        /// </summary>
        private void AwardHonorIfGradeStar(string snum, string honorCode, string term, string syear)
        {
            // 获取学生年级
            string sql = "SELECT Sgrade FROM Students WHERE Snum = '" + snum + "' AND Syear = '" + syear + "'";
            DataSet dsGrade = DbHelperSQL.Query(sql);

            if (dsGrade != null && dsGrade.Tables.Count > 0 && dsGrade.Tables[0].Rows.Count > 0)
            {
                string sgrade = dsGrade.Tables[0].Rows[0]["Sgrade"].ToString();

                // 获取年级前3名
                sql = "SELECT TOP 3 Snum FROM Students WHERE Syear = '" + syear + "' AND Sgrade = '" + sgrade + "' ORDER BY Sallscore DESC";
                DataSet dsTop = DbHelperSQL.Query(sql);

                if (dsTop != null && dsTop.Tables.Count > 0)
                {
                    foreach (DataRow row in dsTop.Tables[0].Rows)
                    {
                        if (row["Snum"].ToString() == snum)
                        {
                            AwardHonor(snum, honorCode, term);
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 如果学生是班级骄傲，则颁发荣誉
        /// </summary>
        private void AwardHonorIfClassPride(string snum, string honorCode, string term, string syear)
        {
            // 获取学生年级和班级
            string sql = "SELECT Sgrade, Sclass FROM Students WHERE Snum = '" + snum + "' AND Syear = '" + syear + "'";
            DataSet dsClass = DbHelperSQL.Query(sql);

            if (dsClass != null && dsClass.Tables.Count > 0 && dsClass.Tables[0].Rows.Count > 0)
            {
                string sgrade = dsClass.Tables[0].Rows[0]["Sgrade"].ToString();
                string sclass = dsClass.Tables[0].Rows[0]["Sclass"].ToString();

                // 获取班级前5名
                sql = "SELECT TOP 5 Snum FROM Students WHERE Syear = '" + syear + "' AND Sgrade = '" + sgrade + "' AND Sclass = '" + sclass + "' ORDER BY Sallscore DESC";
                DataSet dsTop = DbHelperSQL.Query(sql);

                if (dsTop != null && dsTop.Tables.Count > 0)
                {
                    foreach (DataRow row in dsTop.Tables[0].Rows)
                    {
                        if (row["Snum"].ToString() == snum)
                        {
                            AwardHonor(snum, honorCode, term);
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 为单个学生评定指定荣誉
        /// </summary>
        private void EvaluateStudentHonor(string snum, string honorCode, string term, string syear)
        {
            // 直接为指定学生评定荣誉
            switch (honorCode)
            {
                case "AC_LEARNING_STAR":
                    // 学习标兵：总分前5%
                    AwardHonorIfTopPercent(snum, honorCode, "Sallscore", 0.05, term, syear);
                    break;
                case "AC_WORK_EXPERT":
                    // 作品达人：作品分前10%
                    AwardHonorIfTopN(snum, honorCode, "Sscore", 10, term, syear);
                    break;
                case "AC_SKILL_MASTER":
                    // 技能能手：技能综合分前10%
                    AwardHonorIfSkillMaster(snum, honorCode, term, syear);
                    break;
                case "AC_QUIZ_STAR":
                    // 测验之星：测验最高分前10%
                    AwardHonorIfTopN(snum, honorCode, "Squiz", 10, term, syear);
                    break;
                case "BE_PERFECT_ATTENDANCE":
                    // 全勤之星：签到率100%
                    AwardHonorIfPerfectAttendance(snum, honorCode, term, syear);
                    break;
                case "BE_MOST_IMPROVED":
                    // 进步之星：进步幅度前10%
                    AwardHonorIfMostImproved(snum, honorCode, term, syear);
                    break;
                case "BE_ACTIVE_PARTICIPANT":
                    // 积极分子：表现分前10%
                    AwardHonorIfTopN(snum, honorCode, "Sattitude", 10, term, syear);
                    break;
                case "BE_DISCUSSION_EXPERT":
                    // 讨论达人：讨论分前10%
                    AwardHonorIfTopN(snum, honorCode, "Spscore", 10, term, syear);
                    break;
                case "TC_EXCELLENT_LEADER":
                    // 优秀组长：组长且小组优秀
                    AwardHonorIfExcellentLeader(snum, honorCode, term, syear);
                    break;
                case "TC_COLLABORATION_STAR":
                    // 合作之星：小组合作分前10%
                    AwardHonorIfTopN(snum, honorCode, "Sgscore", 10, term, syear);
                    break;
                case "TC_HELPER_STAR":
                    // 互助达人：帮助他人最多
                    AwardHonorIfHelperStar(snum, honorCode, term, syear);
                    break;
                case "SP_TYPING_MASTER":
                    // 打字高手：中文打字第1名
                    AwardHonorIfTopN(snum, honorCode, "Stscore", 1, term, syear);
                    break;
                case "SP_FINGER_EXPERT":
                    // 指法达人：英文打字第1名
                    AwardHonorIfTopN(snum, honorCode, "Stxtform", 1, term, syear);
                    break;
                case "SP_PINYIN_STAR":
                    // 拼音之星：中文拼音第1名
                    AwardHonorIfTopN(snum, honorCode, "Sscore", 1, term, syear);
                    break;
                case "SP_WEB_EXPERT":
                    // 网页达人：使用Sscore替代Sweb
                    AwardHonorIfTopN(snum, honorCode, "Sscore", 10, term, syear);
                    break;
                case "SP_RESEARCH_EXPERT":
                    // 调研先锋：调研分前10%
                    AwardHonorIfTopN(snum, honorCode, "Spscore", 10, term, syear);
                    break;
                case "COM_CAMPUS_STAR":
                    // 校园之星：获得3种及以上不同类型荣誉的学生
                    AwardHonorIfCampusStar(snum, honorCode, term, syear);
                    break;
                case "COM_GRADE_MODEL":
                    // 年级榜样：各年级总分前3名
                    AwardHonorIfGradeStar(snum, honorCode, term, syear);
                    break;
                case "COM_CLASS_PRIDE":
                    // 班级骄傲：各班总分前5名
                    AwardHonorIfClassPride(snum, honorCode, term, syear);
                    break;
            }
        }

        /// <summary>
        /// 按前N名评定荣誉
        /// </summary>
        private void EvaluateTopNHonor(string honorCode, string scoreField, int topN, string term, string syear)
        {
            List<StudentScore> topStudents = GetTopNStudents(scoreField, topN, syear);

            foreach (var student in topStudents)
            {
                AwardHonor(student.Snum, honorCode, term);
            }
        }

        /// <summary>
        /// 按前N%评定荣誉
        /// </summary>
        private void EvaluateTopPercentHonor(string honorCode, string scoreField, double percent, string term, string syear)
        {
            List<StudentScore> allStudents = GetAllStudentsWithScore(scoreField, syear);
            int count = (int)(allStudents.Count * percent);

            for (int i = 0; i < count && i < allStudents.Count; i++)
            {
                AwardHonor(allStudents[i].Snum, honorCode, term);
            }
        }

        /// <summary>
        /// 按第1名评定荣誉
        /// </summary>
        private void EvaluateRank1Honor(string honorCode, string scoreField, string term, string syear)
        {
            List<StudentScore> topStudents = GetTopNStudents(scoreField, 1, syear);
            if (topStudents.Count > 0)
            {
                AwardHonor(topStudents[0].Snum, honorCode, term);
            }
        }

        /// <summary>
        /// 评定技能能手（打字+指法+拼音综合分）
        /// </summary>
        private void EvaluateSkillMasterHonor(string term, string syear)
        {
            string sql = "SELECT TOP 10 Snum FROM Students WHERE Syear = '" + syear + "' ORDER BY (Stscore + Stxtform + Sscore) DESC";
            DataSet ds = DbHelperSQL.Query(sql);

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                AwardHonor(row["Snum"].ToString(), "AC_SKILL_MASTER", term);
            }
        }

        /// <summary>
        /// 评定全勤之星
        /// </summary>
        private void EvaluatePerfectAttendanceHonor(string term, string syear)
        {
            // 简化：假设签到次数为0的都是全勤（实际需要更复杂的逻辑）
            string sql = "SELECT DISTINCT Snum FROM Students WHERE Syear = '" + syear + "' AND Snum NOT IN (SELECT DISTINCT Qnum FROM Signin)";
            DataSet ds = DbHelperSQL.Query(sql);

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                AwardHonor(row["Snum"].ToString(), "BE_PERFECT_ATTENDANCE", term);
            }
        }

        /// <summary>
        /// 评定进步之星
        /// </summary>
        private void EvaluateMostImprovedHonor(string term, string syear)
        {
            // 简化：随机评定10%
            string sql = "SELECT TOP 10 PERCENT Snum FROM Students WHERE Syear = '" + syear + "' ORDER BY NEWID()";
            DataSet ds = DbHelperSQL.Query(sql);

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                AwardHonor(row["Snum"].ToString(), "BE_MOST_IMPROVED", term);
            }
        }

        /// <summary>
        /// 评定优秀组长
        /// </summary>
        private void EvaluateExcellentLeaderHonor(string term, string syear)
        {
            // 简化：获取分组前5名学生
            string sql = "SELECT TOP 5 Snum FROM Students WHERE Syear = '" + syear + "' AND Sgroup > 0 ORDER BY Sallscore DESC";
            DataSet ds = DbHelperSQL.Query(sql);

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                AwardHonor(row["Snum"].ToString(), "TC_EXCELLENT_LEADER", term);
            }
        }

        /// <summary>
        /// 评定互助达人
        /// </summary>
        private void EvaluateHelperStarHonor(string term, string syear)
        {
            // 简化：随机评定5名学生
            string sql = "SELECT TOP 5 Snum FROM Students WHERE Syear = '" + syear + "' ORDER BY NEWID()";
            DataSet ds = DbHelperSQL.Query(sql);

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                AwardHonor(row["Snum"].ToString(), "TC_HELPER_STAR", term);
            }
        }

        /// <summary>
        /// 评定校园之星
        /// </summary>
        private void EvaluateCampusStarHonor(string term, string syear)
        {
            // 简化：获取荣誉最多的前5名学生
            string sql = "SELECT TOP 5 sh.Snum FROM StudentHonors sh INNER JOIN Students s ON sh.Snum = s.Snum "
                       + "WHERE s.Syear = '" + syear + "' GROUP BY sh.Snum ORDER BY COUNT(*) DESC";
            DataSet ds = DbHelperSQL.Query(sql);

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                AwardHonor(row["Snum"].ToString(), "CO_CAMPUS_STAR", term);
            }
        }

        /// <summary>
        /// 评定年级榜样
        /// </summary>
        private void EvaluateGradeStarHonor(string term, string syear)
        {
            // 获取各年级前3名
            string sql = "SELECT Snum FROM (SELECT Snum, ROW_NUMBER() OVER(PARTITION BY Sgrade ORDER BY Sallscore DESC) AS Rank "
                       + "FROM Students WHERE Syear = '" + syear + "') t WHERE Rank <= 3";
            DataSet ds = DbHelperSQL.Query(sql);

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                AwardHonor(row["Snum"].ToString(), "CO_GRADE_STAR", term);
            }
        }

        /// <summary>
        /// 评定班级骄傲
        /// </summary>
        private void EvaluateClassPrideHonor(string term, string syear)
        {
            // 获取各班前5名
            string sql = "SELECT Snum FROM (SELECT Snum, ROW_NUMBER() OVER(PARTITION BY Sgrade, Sclass ORDER BY Sallscore DESC) AS Rank "
                       + "FROM Students WHERE Syear = '" + syear + "') t WHERE Rank <= 5";
            DataSet ds = DbHelperSQL.Query(sql);

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                AwardHonor(row["Snum"].ToString(), "CO_CLASS_PRIDE", term);
            }
        }

        #endregion

        #region 颁发荣誉

        /// <summary>
        /// 颁发荣誉给学生
        /// </summary>
        private void AwardHonor(string snum, string honorCode, string term)
        {
            // 检查是否已获得该荣誉
            StudentHonor existingHonor = honorDal.GetStudentHonor(snum, honorCode);

            if (existingHonor == null)
            {
                // 首次获得，评定等级
                int honorLevel = DetermineHonorLevel(snum, honorCode);
                
                StudentHonor honor = new StudentHonor
                {
                    Snum = snum,
                    HonorCode = honorCode,
                    HonorLevel = honorLevel,
                    EarnDate = DateTime.Now,
                    EarnCount = 1,
                    Continuous = 1,
                    Term = term
                };

                honorDal.AddStudentHonor(honor);
            }
            else
            {
                // 已获得，更新次数和等级
                int newLevel = DetermineHonorLevel(snum, honorCode);
                int newCount = existingHonor.EarnCount + 1;
                int newContinuous = existingHonor.Continuous + 1;

                honorDal.UpdateHonorLevel(snum, honorCode, newLevel, newCount, newContinuous);
            }
        }

        /// <summary>
        /// 确定荣誉等级
        /// </summary>
        private int DetermineHonorLevel(string snum, string honorCode)
        {
            // 简化：随机评定等级
            Random random = new Random();
            int level = random.Next(1, 4);
            return level;
        }

        #endregion

        #region 数据获取辅助方法

        /// <summary>
        /// 获取前N名学生
        /// </summary>
        private List<StudentScore> GetTopNStudents(string scoreField, int topN, string syear)
        {
            string sql = "SELECT TOP " + topN + " Snum, " + scoreField + " AS Score FROM Students "
                       + "WHERE Syear = '" + syear + "' AND " + scoreField + " > 0 ORDER BY " + scoreField + " DESC";
            DataSet ds = DbHelperSQL.Query(sql);

            List<StudentScore> students = new List<StudentScore>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                students.Add(new StudentScore
                {
                    Snum = row["Snum"].ToString(),
                    Score = Convert.ToDecimal(row["Score"])
                });
            }
            return students;
        }

        /// <summary>
        /// 获取所有学生及其分数
        /// </summary>
        private List<StudentScore> GetAllStudentsWithScore(string scoreField, string syear)
        {
            string sql = "SELECT Snum, " + scoreField + " AS Score FROM Students "
                       + "WHERE Syear = '" + syear + "' AND " + scoreField + " > 0 ORDER BY " + scoreField + " DESC";
            DataSet ds = DbHelperSQL.Query(sql);

            List<StudentScore> students = new List<StudentScore>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                students.Add(new StudentScore
                {
                    Snum = row["Snum"].ToString(),
                    Score = Convert.ToDecimal(row["Score"])
                });
            }
            return students;
        }

        /// <summary>
        /// 获取学生分数
        /// </summary>
        private decimal GetStudentScore(string snum, string scoreField)
        {
            string sql = "SELECT " + scoreField + " FROM Students WHERE Snum = '" + snum + "'";
            object result = DbHelperSQL.GetSingle(sql);

            if (result != null && result != DBNull.Value)
            {
                return Convert.ToDecimal(result);
            }
            return 0;
        }

        /// <summary>
        /// 获取前N%的分数阈值
        /// </summary>
        private decimal GetTopPercentThreshold(string scoreField, double percent, string syear)
        {
            // 获取所有学生分数
            List<StudentScore> allStudents = GetAllStudentsWithScore(scoreField, syear);
            if (allStudents.Count == 0)
                return 0;

            // 计算前N%的学生数量
            int topCount = (int)Math.Ceiling(allStudents.Count * percent);
            if (topCount <= 0)
                topCount = 1;

            // 获取第N%学生的分数作为阈值
            if (allStudents.Count >= topCount)
            {
                return allStudents[topCount - 1].Score;
            }
            return allStudents[0].Score;
        }

        #endregion

        #region 获取荣誉数据

        /// <summary>
        /// 获取学生所有荣誉
        /// </summary>
        public List<StudentHonor> GetStudentHonors(string snum)
        {
            return honorDal.GetStudentHonors(snum);
        }

        /// <summary>
        /// 获取荣誉排行榜
        /// </summary>
        public List<HonorRanking> GetHonorRanking(string honorCode, string syear, int topN)
        {
            return honorDal.GetHonorRanking(honorCode, syear, topN);
        }

        /// <summary>
        /// 获取指定年级的荣誉排行榜
        /// </summary>
        public List<HonorRanking> GetHonorRankingByGrade(string honorCode, string syear, string sgrade, int topN)
        {
            return honorDal.GetHonorRankingByGrade(honorCode, syear, sgrade, topN);
        }

        /// <summary>
        /// 获取指定年级和班级的荣誉排行榜
        /// </summary>
        public List<HonorRanking> GetHonorRankingByClass(string honorCode, string syear, string sgrade, string sclass, int topN)
        {
            return honorDal.GetHonorRankingByClass(honorCode, syear, sgrade, sclass, topN);
        }

        /// <summary>
        /// 获取所有荣誉配置
        /// </summary>
        public List<HonorConfig> GetAllHonorConfigs()
        {
            return honorDal.GetAllHonorConfigs();
        }

        /// <summary>
        /// 获取学生荣誉统计
        /// </summary>
        public StudentHonorStat GetStudentHonorStat(string snum)
        {
            return honorDal.GetStudentHonorStat(snum);
        }

        #endregion

        #region 辅助类

        /// <summary>
        /// 学生分数辅助类
        /// </summary>
        private class StudentScore
        {
            public string Snum { get; set; }
            public decimal Score { get; set; }
        }

        #endregion
    }
}
