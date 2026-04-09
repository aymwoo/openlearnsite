using System;
using System.Collections.Generic;
using System.Data;

namespace LearnSite.BLL
{
    public static class BllDataTableMappers
    {
        public static List<LearnSite.Model.Typer> MapTyperList(DataTable dt)
        {
            List<LearnSite.Model.Typer> modelList = new List<LearnSite.Model.Typer>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LearnSite.Model.Typer model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LearnSite.Model.Typer();
                    if (dt.Rows[n]["Tid"].ToString() != "")
                    {
                        model.Tid = int.Parse(dt.Rows[n]["Tid"].ToString());
                    }
                    if (dt.Rows[n]["Ttype"].ToString() != "")
                    {
                        model.Ttype = int.Parse(dt.Rows[n]["Ttype"].ToString());
                    }
                    if (dt.Rows[n]["Tuse"].ToString() != "")
                    {
                        model.Tuse = int.Parse(dt.Rows[n]["Tuse"].ToString());
                    }
                    model.Ttitle = dt.Rows[n]["Ttitle"].ToString();
                    model.Tcontent = dt.Rows[n]["Tcontent"].ToString();
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        public static List<LearnSite.Model.QuizGrade> MapQuizGradeList(DataTable dt)
        {
            List<LearnSite.Model.QuizGrade> modelList = new List<LearnSite.Model.QuizGrade>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LearnSite.Model.QuizGrade model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LearnSite.Model.QuizGrade();
                    if (dt.Rows[n]["Qid"].ToString() != "")
                    {
                        model.Qid = int.Parse(dt.Rows[n]["Qid"].ToString());
                    }
                    if (dt.Rows[n]["Qobj"].ToString() != "")
                    {
                        model.Qobj = int.Parse(dt.Rows[n]["Qobj"].ToString());
                    }
                    model.Qclass = dt.Rows[n]["Qclass"].ToString();
                    if (dt.Rows[n]["Qhid"].ToString() != "")
                    {
                        model.Qhid = int.Parse(dt.Rows[n]["Qhid"].ToString());
                    }
                    if (dt.Rows[n]["Qonly"].ToString() != "")
                    {
                        model.Qonly = int.Parse(dt.Rows[n]["Qonly"].ToString());
                    }
                    if (dt.Rows[n]["Qmore"].ToString() != "")
                    {
                        model.Qmore = int.Parse(dt.Rows[n]["Qmore"].ToString());
                    }
                    if (dt.Rows[n]["Qjudge"].ToString() != "")
                    {
                        model.Qjudge = int.Parse(dt.Rows[n]["Qjudge"].ToString());
                    }
                    if (dt.Rows[n]["Qopen"].ToString() != "")
                    {
                        model.Qopen = dt.Rows[n]["Qopen"].ToString() == "1" || dt.Rows[n]["Qopen"].ToString().ToLower() == "true";
                    }
                    if (dt.Rows[n]["Qanswer"].ToString() != "")
                    {
                        model.Qanswer = dt.Rows[n]["Qanswer"].ToString() == "1" || dt.Rows[n]["Qanswer"].ToString().ToLower() == "true";
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        public static List<LearnSite.Model.TermTotal> MapTermTotalList(DataTable dt)
        {
            List<LearnSite.Model.TermTotal> modelList = new List<LearnSite.Model.TermTotal>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LearnSite.Model.TermTotal model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LearnSite.Model.TermTotal();
                    if (dt.Rows[n]["Tid"].ToString() != "")
                    {
                        model.Tid = int.Parse(dt.Rows[n]["Tid"].ToString());
                    }
                    model.Tnum = dt.Rows[n]["Tnum"].ToString();
                    if (dt.Rows[n]["Tterm"].ToString() != "")
                    {
                        model.Tterm = int.Parse(dt.Rows[n]["Tterm"].ToString());
                    }
                    if (dt.Rows[n]["Tgrade"].ToString() != "")
                    {
                        model.Tgrade = int.Parse(dt.Rows[n]["Tgrade"].ToString());
                    }
                    if (dt.Rows[n]["Tscore"].ToString() != "")
                    {
                        model.Tscore = int.Parse(dt.Rows[n]["Tscore"].ToString());
                    }
                    if (dt.Rows[n]["Tgscore"].ToString() != "")
                    {
                        model.Tgscore = int.Parse(dt.Rows[n]["Tgscore"].ToString());
                    }
                    if (dt.Rows[n]["Tquiz"].ToString() != "")
                    {
                        model.Tquiz = int.Parse(dt.Rows[n]["Tquiz"].ToString());
                    }
                    if (dt.Rows[n]["Tattitude"].ToString() != "")
                    {
                        model.Tattitude = int.Parse(dt.Rows[n]["Tattitude"].ToString());
                    }
                    if (dt.Rows[n]["Twscore"].ToString() != "")
                    {
                        model.Twscore = int.Parse(dt.Rows[n]["Twscore"].ToString());
                    }
                    if (dt.Rows[n]["Ttscore"].ToString() != "")
                    {
                        model.Ttscore = int.Parse(dt.Rows[n]["Ttscore"].ToString());
                    }
                    if (dt.Rows[n]["Tpscore"].ToString() != "")
                    {
                        model.Tpscore = int.Parse(dt.Rows[n]["Tpscore"].ToString());
                    }
                    if (dt.Rows[n]["Tallscore"].ToString() != "")
                    {
                        model.Tallscore = int.Parse(dt.Rows[n]["Tallscore"].ToString());
                    }
                    model.Tape = dt.Rows[n]["Tape"].ToString();
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        public static List<LearnSite.Model.AIProvider> MapAIProviderList(DataTable dt)
        {
            List<LearnSite.Model.AIProvider> modelList = new List<LearnSite.Model.AIProvider>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LearnSite.Model.AIProvider model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LearnSite.Model.AIProvider();
                    if (dt.Rows[n]["Id"].ToString() != "")
                    {
                        model.Id = int.Parse(dt.Rows[n]["Id"].ToString());
                    }
                    model.DisplayName = dt.Rows[n]["DisplayName"].ToString();
                    model.ProviderName = dt.Rows[n]["ProviderName"].ToString();
                    model.ModelName = dt.Rows[n]["ModelName"].ToString();
                    model.ApiKey = dt.Rows[n]["ApiKey"].ToString();
                    model.BaseUrl = dt.Rows[n]["BaseUrl"].ToString();
                    if (dt.Rows[n]["IsDefault"].ToString() != "")
                    {
                        model.IsDefault = dt.Rows[n]["IsDefault"].ToString() == "1" || dt.Rows[n]["IsDefault"].ToString().ToLower() == "true";
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        public static List<LearnSite.Model.AISkill> MapAISkillList(DataTable dt)
        {
            List<LearnSite.Model.AISkill> modelList = new List<LearnSite.Model.AISkill>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LearnSite.Model.AISkill model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LearnSite.Model.AISkill();
                    if (dt.Rows[n]["Id"].ToString() != "")
                    {
                        model.Id = int.Parse(dt.Rows[n]["Id"].ToString());
                    }
                    model.SkillName = dt.Rows[n]["SkillName"].ToString();
                    model.PromptContent = dt.Rows[n]["PromptContent"].ToString();
                    if (dt.Rows[n]["IsActive"].ToString() != "")
                    {
                        model.IsActive = dt.Rows[n]["IsActive"].ToString() == "1" || dt.Rows[n]["IsActive"].ToString().ToLower() == "true";
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        public static List<LearnSite.Model.TurtleMatch> MapTurtleMatchList(DataTable dt)
        {
            List<LearnSite.Model.TurtleMatch> modelList = new List<LearnSite.Model.TurtleMatch>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LearnSite.Model.TurtleMatch model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LearnSite.Model.TurtleMatch();
                    if (dt.Rows[n]["Mid"].ToString() != "")
                    {
                        model.Mid = int.Parse(dt.Rows[n]["Mid"].ToString());
                    }
                    if (dt.Rows[n]["Mhid"].ToString() != "")
                    {
                        model.Mhid = int.Parse(dt.Rows[n]["Mhid"].ToString());
                    }
                    model.Mtitle = dt.Rows[n]["Mtitle"].ToString();
                    model.Mcontent = dt.Rows[n]["Mcontent"].ToString();
                    if (dt.Rows[n]["Mbegin"].ToString() != "")
                    {
                        model.Mbegin = DateTime.Parse(dt.Rows[n]["Mbegin"].ToString());
                    }
                    if (dt.Rows[n]["Mend"].ToString() != "")
                    {
                        model.Mend = DateTime.Parse(dt.Rows[n]["Mend"].ToString());
                    }
                    if (dt.Rows[n]["Mpublish"].ToString() != "")
                    {
                        model.Mpublish = dt.Rows[n]["Mpublish"].ToString() == "1" || dt.Rows[n]["Mpublish"].ToString().ToLower() == "true";
                    }
                    if (dt.Rows[n]["Mdate"].ToString() != "")
                    {
                        model.Mdate = DateTime.Parse(dt.Rows[n]["Mdate"].ToString());
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        public static List<LearnSite.Model.Courses> MapCoursesList(DataTable dt)
        {
            List<LearnSite.Model.Courses> modelList = new List<LearnSite.Model.Courses>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LearnSite.Model.Courses model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LearnSite.Model.Courses();
                    if (dt.Rows[n]["Cid"].ToString() != "")
                    {
                        model.Cid = int.Parse(dt.Rows[n]["Cid"].ToString());
                    }
                    model.Ctitle = dt.Rows[n]["Ctitle"].ToString();
                    model.Cclass = dt.Rows[n]["Cclass"].ToString();
                    model.Ccontent = dt.Rows[n]["Ccontent"].ToString();
                    if (dt.Rows[n]["Cdate"].ToString() != "")
                    {
                        model.Cdate = DateTime.Parse(dt.Rows[n]["Cdate"].ToString());
                    }
                    if (dt.Rows[n]["Chit"].ToString() != "")
                    {
                        model.Chit = int.Parse(dt.Rows[n]["Chit"].ToString());
                    }
                    if (dt.Rows[n]["Cobj"].ToString() != "")
                    {
                        model.Cobj = int.Parse(dt.Rows[n]["Cobj"].ToString());
                    }
                    if (dt.Rows[n]["Cterm"].ToString() != "")
                    {
                        model.Cterm = int.Parse(dt.Rows[n]["Cterm"].ToString());
                    }
                    if (dt.Rows[n]["Cks"].ToString() != "")
                    {
                        model.Cks = int.Parse(dt.Rows[n]["Cks"].ToString());
                    }
                    model.Cfiletype = dt.Rows[n]["Cfiletype"].ToString();
                    if (dt.Rows[n]["Cupload"].ToString() != "")
                    {
                        model.Cupload = dt.Rows[n]["Cupload"].ToString() == "1" || dt.Rows[n]["Cupload"].ToString().ToLower() == "true";
                    }
                    if (dt.Rows[n]["Chid"].ToString() != "")
                    {
                        model.Chid = int.Parse(dt.Rows[n]["Chid"].ToString());
                    }
                    if (dt.Rows[n]["Cpublish"].ToString() != "")
                    {
                        model.Cpublish = dt.Rows[n]["Cpublish"].ToString() == "1" || dt.Rows[n]["Cpublish"].ToString().ToLower() == "true";
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        public static List<LearnSite.Model.Quiz> MapQuizList(DataTable dt)
        {
            List<LearnSite.Model.Quiz> modelList = new List<LearnSite.Model.Quiz>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LearnSite.Model.Quiz model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LearnSite.Model.Quiz();
                    if (dt.Rows[n]["Qid"].ToString() != "")
                    {
                        model.Qid = int.Parse(dt.Rows[n]["Qid"].ToString());
                    }
                    if (dt.Rows[n]["Qtype"].ToString() != "")
                    {
                        model.Qtype = int.Parse(dt.Rows[n]["Qtype"].ToString());
                    }
                    model.Question = dt.Rows[n]["Question"].ToString();
                    model.Qanswer = dt.Rows[n]["Qanswer"].ToString();
                    model.Qanalyze = dt.Rows[n]["Qanalyze"].ToString();
                    if (dt.Rows[n]["Qscore"].ToString() != "")
                    {
                        model.Qscore = int.Parse(dt.Rows[n]["Qscore"].ToString());
                    }
                    model.Qclass = dt.Rows[n]["Qclass"].ToString();
                    if (dt.Rows[n]["Qselect"].ToString() != "")
                    {
                        model.Qselect = dt.Rows[n]["Qselect"].ToString() == "1" || dt.Rows[n]["Qselect"].ToString().ToLower() == "true";
                    }
                    if (dt.Rows[n]["Qright"].ToString() != "")
                    {
                        model.Qright = int.Parse(dt.Rows[n]["Qright"].ToString());
                    }
                    if (dt.Rows[n]["Qwrong"].ToString() != "")
                    {
                        model.Qwrong = int.Parse(dt.Rows[n]["Qwrong"].ToString());
                    }
                    if (dt.Rows[n]["Qaccuracy"].ToString() != "")
                    {
                        model.Qaccuracy = int.Parse(dt.Rows[n]["Qaccuracy"].ToString());
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        public static List<LearnSite.Model.Signin> MapSigninList(DataTable dt)
        {
            List<LearnSite.Model.Signin> modelList = new List<LearnSite.Model.Signin>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LearnSite.Model.Signin model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LearnSite.Model.Signin();
                    if (dt.Rows[n]["Qid"].ToString() != "")
                    {
                        model.Qid = int.Parse(dt.Rows[n]["Qid"].ToString());
                    }
                    model.Qnum = dt.Rows[n]["Qnum"].ToString();
                    if (dt.Rows[n]["Qattitude"].ToString() != "")
                    {
                        model.Qattitude = int.Parse(dt.Rows[n]["Qattitude"].ToString());
                    }
                    if (dt.Rows[n]["Qdate"].ToString() != "")
                    {
                        model.Qdate = DateTime.Parse(dt.Rows[n]["Qdate"].ToString());
                    }
                    if (dt.Rows[n]["Qyear"].ToString() != "")
                    {
                        model.Qyear = int.Parse(dt.Rows[n]["Qyear"].ToString());
                    }
                    if (dt.Rows[n]["Qmonth"].ToString() != "")
                    {
                        model.Qmonth = int.Parse(dt.Rows[n]["Qmonth"].ToString());
                    }
                    if (dt.Rows[n]["Qday"].ToString() != "")
                    {
                        model.Qday = int.Parse(dt.Rows[n]["Qday"].ToString());
                    }
                    model.Qweek = dt.Rows[n]["Qweek"].ToString();
                    model.Qip = dt.Rows[n]["Qip"].ToString();
                    model.Qmachine = dt.Rows[n]["Qmachine"].ToString();
                    model.Qnote = dt.Rows[n]["Qnote"].ToString();
                    if (dt.Rows[n]["Qwork"].ToString() != "")
                    {
                        model.Qwork = int.Parse(dt.Rows[n]["Qwork"].ToString());
                    }
                    if (dt.Rows[n]["Qgrade"].ToString() != "")
                    {
                        model.Qgrade = int.Parse(dt.Rows[n]["Qgrade"].ToString());
                    }
                    if (dt.Rows[n]["Qterm"].ToString() != "")
                    {
                        model.Qterm = int.Parse(dt.Rows[n]["Qterm"].ToString());
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        public static List<LearnSite.Model.AICustomSkill> MapAICustomSkillList(DataTable dt)
        {
            List<LearnSite.Model.AICustomSkill> modelList = new List<LearnSite.Model.AICustomSkill>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LearnSite.Model.AICustomSkill model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LearnSite.Model.AICustomSkill();
                    if (dt.Rows[n]["Id"].ToString() != "")
                    {
                        model.Id = int.Parse(dt.Rows[n]["Id"].ToString());
                    }
                    model.SkillName = dt.Rows[n]["SkillName"].ToString();
                    model.PromptContent = dt.Rows[n]["PromptContent"].ToString();
                    model.SkillScope = dt.Rows[n]["SkillScope"].ToString();
                    if (dt.Rows[n]["IsActive"].ToString() != "")
                    {
                        model.IsActive = dt.Rows[n]["IsActive"].ToString() == "1" || dt.Rows[n]["IsActive"].ToString().ToLower() == "true";
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        public static List<LearnSite.Model.SoftCategory> MapSoftCategoryList(DataTable dt)
        {
            List<LearnSite.Model.SoftCategory> modelList = new List<LearnSite.Model.SoftCategory>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LearnSite.Model.SoftCategory model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LearnSite.Model.SoftCategory();
                    if (dt.Rows[n]["Yid"].ToString() != "")
                    {
                        model.Yid = int.Parse(dt.Rows[n]["Yid"].ToString());
                    }
                    if (dt.Rows[n]["Ysort"].ToString() != "")
                    {
                        model.Ysort = int.Parse(dt.Rows[n]["Ysort"].ToString());
                    }
                    model.Ytitle = dt.Rows[n]["Ytitle"].ToString();
                    model.Ycontent = dt.Rows[n]["Ycontent"].ToString();
                    if (dt.Rows[n]["Yopen"].ToString() != "")
                    {
                        model.Yopen = dt.Rows[n]["Yopen"].ToString() == "1" || dt.Rows[n]["Yopen"].ToString().ToLower() == "true";
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        public static List<LearnSite.Model.Works> MapWorksList(DataTable dt)
        {
            List<LearnSite.Model.Works> modelList = new List<LearnSite.Model.Works>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LearnSite.Model.Works model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LearnSite.Model.Works();
                    if (dt.Rows[n]["Wid"].ToString() != "")
                    {
                        model.Wid = int.Parse(dt.Rows[n]["Wid"].ToString());
                    }
                    model.Wnum = dt.Rows[n]["Wnum"].ToString();
                    if (dt.Rows[n]["Wcid"].ToString() != "")
                    {
                        model.Wcid = int.Parse(dt.Rows[n]["Wcid"].ToString());
                    }
                    if (dt.Rows[n]["Wmid"].ToString() != "")
                    {
                        model.Wmid = int.Parse(dt.Rows[n]["Wmid"].ToString());
                    }
                    if (dt.Rows[n]["Wmsort"].ToString() != "")
                    {
                        model.Wmsort = int.Parse(dt.Rows[n]["Wmsort"].ToString());
                    }
                    model.Wfilename = dt.Rows[n]["Wfilename"].ToString();
                    model.Wurl = dt.Rows[n]["Wurl"].ToString();
                    if (dt.Rows[n]["Wlength"].ToString() != "")
                    {
                        model.Wlength = int.Parse(dt.Rows[n]["Wlength"].ToString());
                    }
                    if (dt.Rows[n]["Wscore"].ToString() != "")
                    {
                        model.Wscore = int.Parse(dt.Rows[n]["Wscore"].ToString());
                    }
                    if (dt.Rows[n]["Wdate"].ToString() != "")
                    {
                        model.Wdate = DateTime.Parse(dt.Rows[n]["Wdate"].ToString());
                    }
                    model.Wip = dt.Rows[n]["Wip"].ToString();
                    model.Wtime = dt.Rows[n]["Wtime"].ToString();
                    if (dt.Rows[n]["Wvote"].ToString() != "")
                    {
                        model.Wvote = int.Parse(dt.Rows[n]["Wvote"].ToString());
                    }
                    if (dt.Rows[n]["Wegg"].ToString() != "")
                    {
                        model.Wegg = int.Parse(dt.Rows[n]["Wegg"].ToString());
                    }
                    if (dt.Rows[n]["Wcheck"].ToString() != "")
                    {
                        model.Wcheck = dt.Rows[n]["Wcheck"].ToString() == "1" || dt.Rows[n]["Wcheck"].ToString().ToLower() == "true";
                    }
                    model.Wself = dt.Rows[n]["Wself"].ToString();
                    if (dt.Rows[n]["Wcan"].ToString() != "")
                    {
                        model.Wcan = dt.Rows[n]["Wcan"].ToString() == "1" || dt.Rows[n]["Wcan"].ToString().ToLower() == "true";
                    }
                    if (dt.Rows[n]["Wgood"].ToString() != "")
                    {
                        model.Wgood = dt.Rows[n]["Wgood"].ToString() == "1" || dt.Rows[n]["Wgood"].ToString().ToLower() == "true";
                    }
                    model.Wtype = dt.Rows[n]["Wtype"].ToString();
                    if (dt.Rows[n]["Wgrade"].ToString() != "")
                    {
                        model.Wgrade = int.Parse(dt.Rows[n]["Wgrade"].ToString());
                    }
                    if (dt.Rows[n]["Wterm"].ToString() != "")
                    {
                        model.Wterm = int.Parse(dt.Rows[n]["Wterm"].ToString());
                    }
                    if (dt.Rows[n]["Whit"].ToString() != "")
                    {
                        model.Whit = int.Parse(dt.Rows[n]["Whit"].ToString());
                    }
                    if (dt.Rows[n]["Wlscore"].ToString() != "")
                    {
                        model.Wlscore = int.Parse(dt.Rows[n]["Wlscore"].ToString());
                    }
                    if (dt.Rows[n]["Wlemotion"].ToString() != "")
                    {
                        model.Wlemotion = int.Parse(dt.Rows[n]["Wlemotion"].ToString());
                    }
                    if (dt.Rows[n]["Woffice"].ToString() != "")
                    {
                        model.Woffice = dt.Rows[n]["Woffice"].ToString() == "1" || dt.Rows[n]["Woffice"].ToString().ToLower() == "true";
                    }
                    if (dt.Rows[n]["Wflash"].ToString() != "")
                    {
                        model.Wflash = dt.Rows[n]["Wflash"].ToString() == "1" || dt.Rows[n]["Wflash"].ToString().ToLower() == "true";
                    }
                    if (dt.Rows[n]["Werror"].ToString() != "")
                    {
                        model.Wflash = dt.Rows[n]["Werror"].ToString() == "1" || dt.Rows[n]["Werror"].ToString().ToLower() == "true";
                    }
                    if (dt.Rows[n]["Wfscore"].ToString() != "")
                    {
                        model.Wfscore = int.Parse(dt.Rows[n]["Wfscore"].ToString());
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        public static List<LearnSite.Model.Solves> MapSolvesList(DataTable dt)
        {
            List<LearnSite.Model.Solves> modelList = new List<LearnSite.Model.Solves>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LearnSite.Model.Solves model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LearnSite.Model.Solves();
                    if (dt.Rows[n]["Vid"] != null && dt.Rows[n]["Vid"].ToString() != "")
                    {
                        model.Vid = int.Parse(dt.Rows[n]["Vid"].ToString());
                    }
                    if (dt.Rows[n]["Vpid"] != null && dt.Rows[n]["Vpid"].ToString() != "")
                    {
                        model.Vpid = int.Parse(dt.Rows[n]["Vpid"].ToString());
                    }
                    if (dt.Rows[n]["Vsid"] != null && dt.Rows[n]["Vsid"].ToString() != "")
                    {
                        model.Vsid = int.Parse(dt.Rows[n]["Vsid"].ToString());
                    }
                    if (dt.Rows[n]["Vanswer"] != null && dt.Rows[n]["Vanswer"].ToString() != "")
                    {
                        model.Vanswer = dt.Rows[n]["Vanswer"].ToString();
                    }
                    if (dt.Rows[n]["Vright"] != null && dt.Rows[n]["Vright"].ToString() != "")
                    {
                        model.Vright = dt.Rows[n]["Vright"].ToString() == "1" || dt.Rows[n]["Vright"].ToString().ToLower() == "true";
                    }
                    if (dt.Rows[n]["Vscore"] != null && dt.Rows[n]["Vscore"].ToString() != "")
                    {
                        model.Vscore = int.Parse(dt.Rows[n]["Vscore"].ToString());
                    }
                    if (dt.Rows[n]["Vdate"] != null && dt.Rows[n]["Vdate"].ToString() != "")
                    {
                        model.Vdate = DateTime.Parse(dt.Rows[n]["Vdate"].ToString());
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        public static List<LearnSite.Model.TxtForm> MapTxtFormList(DataTable dt)
        {
            List<LearnSite.Model.TxtForm> modelList = new List<LearnSite.Model.TxtForm>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LearnSite.Model.TxtForm model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LearnSite.Model.TxtForm();
                    if (dt.Rows[n]["Mid"] != null && dt.Rows[n]["Mid"].ToString() != "")
                    {
                        model.Mid = int.Parse(dt.Rows[n]["Mid"].ToString());
                    }
                    if (dt.Rows[n]["Mtitle"] != null)
                    {
                        model.Mtitle = dt.Rows[n]["Mtitle"].ToString();
                    }
                    if (dt.Rows[n]["Mcid"] != null && dt.Rows[n]["Mcid"].ToString() != "")
                    {
                        model.Mcid = int.Parse(dt.Rows[n]["Mcid"].ToString());
                    }
                    if (dt.Rows[n]["Mcontent"] != null)
                    {
                        model.Mcontent = dt.Rows[n]["Mcontent"].ToString();
                    }
                    if (dt.Rows[n]["Mdate"] != null && dt.Rows[n]["Mdate"].ToString() != "")
                    {
                        model.Mdate = DateTime.Parse(dt.Rows[n]["Mdate"].ToString());
                    }
                    if (dt.Rows[n]["Mhit"] != null && dt.Rows[n]["Mhit"].ToString() != "")
                    {
                        model.Mhit = int.Parse(dt.Rows[n]["Mhit"].ToString());
                    }
                    if (dt.Rows[n]["Mpublish"] != null && dt.Rows[n]["Mpublish"].ToString() != "")
                    {
                        model.Mpublish = dt.Rows[n]["Mpublish"].ToString() == "1" || dt.Rows[n]["Mpublish"].ToString().ToLower() == "true";
                    }
                    if (dt.Rows[n]["Mdelete"] != null && dt.Rows[n]["Mdelete"].ToString() != "")
                    {
                        model.Mdelete = dt.Rows[n]["Mdelete"].ToString() == "1" || dt.Rows[n]["Mdelete"].ToString().ToLower() == "true";
                    }
                    if (dt.Rows[n]["Mcollabo"] != null && dt.Rows[n]["Mcollabo"].ToString() != "")
                    {
                        model.Mcollabo = dt.Rows[n]["Mcollabo"].ToString() == "1" || dt.Rows[n]["Mcollabo"].ToString().ToLower() == "true";
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        public static List<LearnSite.Model.MenuWorks> MapMenuWorksList(DataTable dt)
        {
            List<LearnSite.Model.MenuWorks> modelList = new List<LearnSite.Model.MenuWorks>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LearnSite.Model.MenuWorks model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LearnSite.Model.MenuWorks();
                    if (dt.Rows[n]["Kid"] != null && dt.Rows[n]["Kid"].ToString() != "")
                    {
                        model.Kid = int.Parse(dt.Rows[n]["Kid"].ToString());
                    }
                    if (dt.Rows[n]["Ksid"] != null && dt.Rows[n]["Ksid"].ToString() != "")
                    {
                        model.Ksid = int.Parse(dt.Rows[n]["Ksid"].ToString());
                    }
                    if (dt.Rows[n]["Klid"] != null && dt.Rows[n]["Klid"].ToString() != "")
                    {
                        model.Klid = int.Parse(dt.Rows[n]["Klid"].ToString());
                    }
                    if (dt.Rows[n]["Ktime"] != null && dt.Rows[n]["Ktime"].ToString() != "")
                    {
                        model.Ktime = int.Parse(dt.Rows[n]["Ktime"].ToString());
                    }
                    if (dt.Columns.Contains("Kseconds") && dt.Rows[n]["Kseconds"] != null && dt.Rows[n]["Kseconds"].ToString() != "")
                    {
                        model.Kseconds = int.Parse(dt.Rows[n]["Kseconds"].ToString());
                    }
                    if (dt.Rows[n]["Kcheck"] != null && dt.Rows[n]["Kcheck"].ToString() != "")
                    {
                        model.Kcheck = dt.Rows[n]["Kcheck"].ToString() == "1" || dt.Rows[n]["Kcheck"].ToString().ToLower() == "true";
                    }
                    if (dt.Rows[n]["Kstar"] != null && dt.Rows[n]["Kstar"].ToString() != "")
                    {
                        model.Kstar = int.Parse(dt.Rows[n]["Kstar"].ToString());
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        public static List<LearnSite.Model.TxtFormBack> MapTxtFormBackList(DataTable dt)
        {
            List<LearnSite.Model.TxtFormBack> modelList = new List<LearnSite.Model.TxtFormBack>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LearnSite.Model.TxtFormBack model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LearnSite.Model.TxtFormBack();
                    if (dt.Rows[n]["Rid"] != null && dt.Rows[n]["Rid"].ToString() != "")
                    {
                        model.Rid = int.Parse(dt.Rows[n]["Rid"].ToString());
                    }
                    if (dt.Rows[n]["Rmid"] != null && dt.Rows[n]["Rmid"].ToString() != "")
                    {
                        model.Rmid = int.Parse(dt.Rows[n]["Rmid"].ToString());
                    }
                    if (dt.Rows[n]["Rsnum"] != null)
                    {
                        model.Rsnum = dt.Rows[n]["Rsnum"].ToString();
                    }
                    if (dt.Rows[n]["Rsid"] != null && dt.Rows[n]["Rsid"].ToString() != "")
                    {
                        model.Rsid = int.Parse(dt.Rows[n]["Rsid"].ToString());
                    }
                    if (dt.Rows[n]["Rwords"] != null)
                    {
                        model.Rwords = dt.Rows[n]["Rwords"].ToString();
                    }
                    if (dt.Rows[n]["Rtime"] != null && dt.Rows[n]["Rtime"].ToString() != "")
                    {
                        model.Rtime = DateTime.Parse(dt.Rows[n]["Rtime"].ToString());
                    }
                    if (dt.Rows[n]["Rip"] != null)
                    {
                        model.Rip = dt.Rows[n]["Rip"].ToString();
                    }
                    if (dt.Rows[n]["Rscore"] != null && dt.Rows[n]["Rscore"].ToString() != "")
                    {
                        model.Rscore = int.Parse(dt.Rows[n]["Rscore"].ToString());
                    }
                    if (dt.Rows[n]["Ryear"] != null && dt.Rows[n]["Ryear"].ToString() != "")
                    {
                        model.Ryear = int.Parse(dt.Rows[n]["Ryear"].ToString());
                    }
                    if (dt.Rows[n]["Rterm"] != null && dt.Rows[n]["Rterm"].ToString() != "")
                    {
                        model.Rterm = int.Parse(dt.Rows[n]["Rterm"].ToString());
                    }
                    if (dt.Rows[n]["Rgrade"] != null && dt.Rows[n]["Rgrade"].ToString() != "")
                    {
                        model.Rgrade = int.Parse(dt.Rows[n]["Rgrade"].ToString());
                    }
                    if (dt.Rows[n]["Rclass"] != null && dt.Rows[n]["Rclass"].ToString() != "")
                    {
                        model.Rclass = int.Parse(dt.Rows[n]["Rclass"].ToString());
                    }
                    if (dt.Rows[n]["Ragree"] != null && dt.Rows[n]["Ragree"].ToString() != "")
                    {
                        model.Ragree = int.Parse(dt.Rows[n]["Ragree"].ToString());
                    }
                    if (dt.Columns.Contains("Rlid") && dt.Rows[n]["Rlid"] != null && dt.Rows[n]["Rlid"].ToString() != "")
                    {
                        model.Rlid = int.Parse(dt.Rows[n]["Rlid"].ToString());
                    }
                    if (dt.Columns.Contains("Rcontent") && dt.Rows[n]["Rcontent"] != null)
                    {
                        model.Rcontent = dt.Rows[n]["Rcontent"].ToString();
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        public static List<LearnSite.Model.Survey> MapSurveyList(DataTable dt)
        {
            List<LearnSite.Model.Survey> modelList = new List<LearnSite.Model.Survey>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LearnSite.Model.Survey model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LearnSite.Model.Survey();
                    if (dt.Rows[n]["Vid"] != null && dt.Rows[n]["Vid"].ToString() != "")
                    {
                        model.Vid = int.Parse(dt.Rows[n]["Vid"].ToString());
                    }
                    if (dt.Rows[n]["Vcid"] != null && dt.Rows[n]["Vcid"].ToString() != "")
                    {
                        model.Vcid = int.Parse(dt.Rows[n]["Vcid"].ToString());
                    }
                    if (dt.Rows[n]["Vhid"] != null && dt.Rows[n]["Vhid"].ToString() != "")
                    {
                        model.Vhid = int.Parse(dt.Rows[n]["Vhid"].ToString());
                    }
                    if (dt.Rows[n]["Vtitle"] != null && dt.Rows[n]["Vtitle"].ToString() != "")
                    {
                        model.Vtitle = dt.Rows[n]["Vtitle"].ToString();
                    }
                    if (dt.Rows[n]["Vcontent"] != null && dt.Rows[n]["Vcontent"].ToString() != "")
                    {
                        model.Vcontent = dt.Rows[n]["Vcontent"].ToString();
                    }
                    if (dt.Rows[n]["Vtype"] != null && dt.Rows[n]["Vtype"].ToString() != "")
                    {
                        model.Vtype = int.Parse(dt.Rows[n]["Vtype"].ToString());
                    }
                    if (dt.Rows[n]["Vtotal"] != null && dt.Rows[n]["Vtotal"].ToString() != "")
                    {
                        model.Vtotal = int.Parse(dt.Rows[n]["Vtotal"].ToString());
                    }
                    if (dt.Rows[n]["Vscore"] != null && dt.Rows[n]["Vscore"].ToString() != "")
                    {
                        model.Vscore = int.Parse(dt.Rows[n]["Vscore"].ToString());
                    }
                    if (dt.Rows[n]["Vaverage"] != null && dt.Rows[n]["Vaverage"].ToString() != "")
                    {
                        model.Vaverage = int.Parse(dt.Rows[n]["Vaverage"].ToString());
                    }
                    if (dt.Rows[n]["Vclose"] != null && dt.Rows[n]["Vclose"].ToString() != "")
                    {
                        model.Vclose = dt.Rows[n]["Vclose"].ToString() == "1" || dt.Rows[n]["Vclose"].ToString().ToLower() == "true";
                    }
                    if (dt.Rows[n]["Vpoint"] != null && dt.Rows[n]["Vpoint"].ToString() != "")
                    {
                        model.Vpoint = dt.Rows[n]["Vpoint"].ToString() == "1" || dt.Rows[n]["Vpoint"].ToString().ToLower() == "true";
                    }
                    if (dt.Columns.Contains("Venableai") && dt.Rows[n]["Venableai"] != null && dt.Rows[n]["Venableai"].ToString() != "")
                    {
                        model.Venableai = dt.Rows[n]["Venableai"].ToString() == "1" || dt.Rows[n]["Venableai"].ToString().ToLower() == "true";
                    }
                    if (dt.Rows[n]["Vdate"] != null && dt.Rows[n]["Vdate"].ToString() != "")
                    {
                        model.Vdate = DateTime.Parse(dt.Rows[n]["Vdate"].ToString());
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        public static List<LearnSite.Model.SurveyItem> MapSurveyItemList(DataTable dt)
        {
            List<LearnSite.Model.SurveyItem> modelList = new List<LearnSite.Model.SurveyItem>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LearnSite.Model.SurveyItem model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LearnSite.Model.SurveyItem();
                    if (dt.Rows[n]["Mid"] != null && dt.Rows[n]["Mid"].ToString() != "")
                    {
                        model.Mid = int.Parse(dt.Rows[n]["Mid"].ToString());
                    }
                    if (dt.Rows[n]["Mqid"] != null && dt.Rows[n]["Mqid"].ToString() != "")
                    {
                        model.Mqid = int.Parse(dt.Rows[n]["Mqid"].ToString());
                    }
                    if (dt.Rows[n]["Mvid"] != null && dt.Rows[n]["Mvid"].ToString() != "")
                    {
                        model.Mvid = int.Parse(dt.Rows[n]["Mvid"].ToString());
                    }
                    if (dt.Rows[n]["Mitem"] != null && dt.Rows[n]["Mitem"].ToString() != "")
                    {
                        model.Mitem = dt.Rows[n]["Mitem"].ToString();
                    }
                    if (dt.Rows[n]["Mscore"] != null && dt.Rows[n]["Mscore"].ToString() != "")
                    {
                        model.Mscore = int.Parse(dt.Rows[n]["Mscore"].ToString());
                    }
                    if (dt.Rows[n]["Mcount"] != null && dt.Rows[n]["Mcount"].ToString() != "")
                    {
                        model.Mcount = int.Parse(dt.Rows[n]["Mcount"].ToString());
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        public static List<LearnSite.Model.SurveyQuestion> MapSurveyQuestionList(DataTable dt)
        {
            List<LearnSite.Model.SurveyQuestion> modelList = new List<LearnSite.Model.SurveyQuestion>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LearnSite.Model.SurveyQuestion model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LearnSite.Model.SurveyQuestion();
                    if (dt.Rows[n]["Qid"] != null && dt.Rows[n]["Qid"].ToString() != "")
                    {
                        model.Qid = int.Parse(dt.Rows[n]["Qid"].ToString());
                    }
                    if (dt.Rows[n]["Qvid"] != null && dt.Rows[n]["Qvid"].ToString() != "")
                    {
                        model.Qvid = int.Parse(dt.Rows[n]["Qvid"].ToString());
                    }
                    if (dt.Rows[n]["Qcid"] != null && dt.Rows[n]["Qcid"].ToString() != "")
                    {
                        model.Qcid = int.Parse(dt.Rows[n]["Qcid"].ToString());
                    }
                    if (dt.Rows[n]["Qtitle"] != null && dt.Rows[n]["Qtitle"].ToString() != "")
                    {
                        model.Qtitle = dt.Rows[n]["Qtitle"].ToString();
                    }
                    if (dt.Rows[n]["Qcount"] != null && dt.Rows[n]["Qcount"].ToString() != "")
                    {
                        model.Qcount = int.Parse(dt.Rows[n]["Qcount"].ToString());
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        public static List<LearnSite.Model.Soft> MapSoftList(DataTable dt)
        {
            List<LearnSite.Model.Soft> modelList = new List<LearnSite.Model.Soft>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LearnSite.Model.Soft model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LearnSite.Model.Soft();
                    if (dt.Rows[n]["Fid"].ToString() != "")
                    {
                        model.Fid = int.Parse(dt.Rows[n]["Fid"].ToString());
                    }
                    model.Ftitle = dt.Rows[n]["Ftitle"].ToString();
                    model.Fcontent = dt.Rows[n]["Fcontent"].ToString();
                    model.Furl = dt.Rows[n]["Furl"].ToString();
                    if (dt.Rows[n]["Fhit"].ToString() != "")
                    {
                        model.Fhit = int.Parse(dt.Rows[n]["Fhit"].ToString());
                    }
                    if (dt.Rows[n]["Fdate"].ToString() != "")
                    {
                        model.Fdate = DateTime.Parse(dt.Rows[n]["Fdate"].ToString());
                    }
                    model.Ffiletype = dt.Rows[n]["Ffiletype"].ToString();
                    model.Fclass = dt.Rows[n]["Fclass"].ToString();
                    if (dt.Rows[n]["Fhide"].ToString() != "")
                    {
                        model.Fhide = dt.Rows[n]["Fhide"].ToString() == "1" || dt.Rows[n]["Fhide"].ToString().ToLower() == "true";
                    }
                    if (dt.Rows[n]["Fopen"].ToString() != "")
                    {
                        model.Fopen = int.Parse(dt.Rows[n]["Fopen"].ToString());
                    }
                    if (dt.Rows[n]["Fhid"].ToString() != "")
                    {
                        model.Fhid = int.Parse(dt.Rows[n]["Fhid"].ToString());
                    }
                    if (dt.Rows[n]["Fyid"].ToString() != "")
                    {
                        model.Fyid = int.Parse(dt.Rows[n]["Fyid"].ToString());
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        public static List<LearnSite.Model.Research> MapResearchList(DataTable dt)
        {
            List<LearnSite.Model.Research> modelList = new List<LearnSite.Model.Research>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LearnSite.Model.Research model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LearnSite.Model.Research();
                    if (dt.Rows[n]["Rid"] != null && dt.Rows[n]["Rid"].ToString() != "")
                    {
                        model.Rid = int.Parse(dt.Rows[n]["Rid"].ToString());
                    }
                    if (dt.Rows[n]["Rsid"] != null && dt.Rows[n]["Rsid"].ToString() != "")
                    {
                        model.Rsid = int.Parse(dt.Rows[n]["Rsid"].ToString());
                    }
                    if (dt.Rows[n]["Ryear"] != null && dt.Rows[n]["Ryear"].ToString() != "")
                    {
                        model.Ryear = int.Parse(dt.Rows[n]["Ryear"].ToString());
                    }
                    if (dt.Rows[n]["Rgrade"] != null && dt.Rows[n]["Rgrade"].ToString() != "")
                    {
                        model.Rgrade = int.Parse(dt.Rows[n]["Rgrade"].ToString());
                    }
                    if (dt.Rows[n]["Rclass"] != null && dt.Rows[n]["Rclass"].ToString() != "")
                    {
                        model.Rclass = int.Parse(dt.Rows[n]["Rclass"].ToString());
                    }
                    if (dt.Rows[n]["Rterm"] != null && dt.Rows[n]["Rterm"].ToString() != "")
                    {
                        model.Rterm = int.Parse(dt.Rows[n]["Rterm"].ToString());
                    }
                    if (dt.Rows[n]["Rlearn"] != null && dt.Rows[n]["Rlearn"].ToString() != "")
                    {
                        model.Rlearn = decimal.Parse(dt.Rows[n]["Rlearn"].ToString());
                    }
                    if (dt.Rows[n]["Rplay"] != null && dt.Rows[n]["Rplay"].ToString() != "")
                    {
                        model.Rplay = decimal.Parse(dt.Rows[n]["Rplay"].ToString());
                    }
                    if (dt.Rows[n]["Rsleep"] != null && dt.Rows[n]["Rsleep"].ToString() != "")
                    {
                        model.Rsleep = decimal.Parse(dt.Rows[n]["Rsleep"].ToString());
                    }
                    if (dt.Rows[n]["Rfree"] != null && dt.Rows[n]["Rfree"].ToString() != "")
                    {
                        model.Rfree = decimal.Parse(dt.Rows[n]["Rfree"].ToString());
                    }
                    if (dt.Rows[n]["Rdate"] != null && dt.Rows[n]["Rdate"].ToString() != "")
                    {
                        model.Rdate = DateTime.Parse(dt.Rows[n]["Rdate"].ToString());
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        public static List<LearnSite.Model.Result> MapResultList(DataTable dt)
        {
            List<LearnSite.Model.Result> modelList = new List<LearnSite.Model.Result>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LearnSite.Model.Result model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LearnSite.Model.Result();
                    if (dt.Rows[n]["Rid"].ToString() != "")
                    {
                        model.Rid = int.Parse(dt.Rows[n]["Rid"].ToString());
                    }
                    model.Rnum = dt.Rows[n]["Rnum"].ToString();
                    if (dt.Rows[n]["Rscore"].ToString() != "")
                    {
                        model.Rscore = int.Parse(dt.Rows[n]["Rscore"].ToString());
                    }
                    if (dt.Rows[n]["Rdate"].ToString() != "")
                    {
                        model.Rdate = DateTime.Parse(dt.Rows[n]["Rdate"].ToString());
                    }
                    model.Rhistory = dt.Rows[n]["Rhistory"].ToString();
                    model.Rwrong = dt.Rows[n]["Rwrong"].ToString();
                    if (dt.Rows[n]["Rgrade"].ToString() != "")
                    {
                        model.Rgrade = int.Parse(dt.Rows[n]["Rgrade"].ToString());
                    }
                    if (dt.Rows[n]["Rterm"].ToString() != "")
                    {
                        model.Rterm = int.Parse(dt.Rows[n]["Rterm"].ToString());
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        public static List<LearnSite.Model.Mission> MapMissionList(DataTable dt)
        {
            List<LearnSite.Model.Mission> modelList = new List<LearnSite.Model.Mission>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LearnSite.Model.Mission model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LearnSite.Model.Mission();
                    if (dt.Rows[n]["Mid"].ToString() != "")
                    {
                        model.Mid = int.Parse(dt.Rows[n]["Mid"].ToString());
                    }
                    model.Mtitle = dt.Rows[n]["Mtitle"].ToString();
                    if (dt.Rows[n]["Mcid"].ToString() != "")
                    {
                        model.Mcid = int.Parse(dt.Rows[n]["Mcid"].ToString());
                    }
                    model.Mcontent = dt.Rows[n]["Mcontent"].ToString();
                    if (dt.Rows[n]["Mdate"].ToString() != "")
                    {
                        model.Mdate = DateTime.Parse(dt.Rows[n]["Mdate"].ToString());
                    }
                    if (dt.Rows[n]["Mhit"].ToString() != "")
                    {
                        model.Mhit = int.Parse(dt.Rows[n]["Mhit"].ToString());
                    }
                    model.Mfiletype = dt.Rows[n]["Mfiletype"].ToString();
                    if (dt.Rows[n]["Mupload"].ToString() != "")
                    {
                        model.Mupload = dt.Rows[n]["Mupload"].ToString() == "1" || dt.Rows[n]["Mupload"].ToString().ToLower() == "true";
                    }
                    if (dt.Rows[n]["Msort"].ToString() != "")
                    {
                        model.Msort = int.Parse(dt.Rows[n]["Msort"].ToString());
                    }
                    if (dt.Rows[n]["Mpublish"].ToString() != "")
                    {
                        model.Mpublish = dt.Rows[n]["Mpublish"].ToString() == "1" || dt.Rows[n]["Mpublish"].ToString().ToLower() == "true";
                    }
                    if (dt.Rows[n]["Mgroup"].ToString() != "")
                    {
                        model.Mgroup = dt.Rows[n]["Mgroup"].ToString() == "1" || dt.Rows[n]["Mgroup"].ToString().ToLower() == "true";
                    }
                    if (dt.Rows[n]["Mgid"] != null && dt.Rows[n]["Mgid"].ToString() != "")
                    {
                        model.Mgid = int.Parse(dt.Rows[n]["Mgid"].ToString());
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        public static List<LearnSite.Model.ListMenu> MapListMenuList(DataTable dt)
        {
            List<LearnSite.Model.ListMenu> modelList = new List<LearnSite.Model.ListMenu>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LearnSite.Model.ListMenu model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LearnSite.Model.ListMenu();
                    if (dt.Rows[n]["Lid"] != null && dt.Rows[n]["Lid"].ToString() != "")
                    {
                        model.Lid = int.Parse(dt.Rows[n]["Lid"].ToString());
                    }
                    if (dt.Rows[n]["Lcid"] != null && dt.Rows[n]["Lcid"].ToString() != "")
                    {
                        model.Lcid = int.Parse(dt.Rows[n]["Lcid"].ToString());
                    }
                    if (dt.Rows[n]["Lsort"] != null && dt.Rows[n]["Lsort"].ToString() != "")
                    {
                        model.Lsort = int.Parse(dt.Rows[n]["Lsort"].ToString());
                    }
                    if (dt.Rows[n]["Ltype"] != null && dt.Rows[n]["Ltype"].ToString() != "")
                    {
                        model.Ltype = int.Parse(dt.Rows[n]["Ltype"].ToString());
                    }
                    if (dt.Rows[n]["Lxid"] != null && dt.Rows[n]["Lxid"].ToString() != "")
                    {
                        model.Lxid = int.Parse(dt.Rows[n]["Lxid"].ToString());
                    }
                    if (dt.Rows[n]["Lshow"] != null && dt.Rows[n]["Lshow"].ToString() != "")
                    {
                        model.Lshow = dt.Rows[n]["Lshow"].ToString() == "1" || dt.Rows[n]["Lshow"].ToString().ToLower() == "true";
                    }
                    if (dt.Rows[n]["Ltitle"] != null && dt.Rows[n]["Ltitle"].ToString() != "")
                    {
                        model.Ltitle = dt.Rows[n]["Ltitle"].ToString();
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        public static List<LearnSite.Model.Exams> MapExamsList(DataTable dt)
        {
            List<LearnSite.Model.Exams> modelList = new List<LearnSite.Model.Exams>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LearnSite.Model.Exams model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LearnSite.Model.Exams();
                    if (dt.Rows[n]["Eid"] != null && dt.Rows[n]["Eid"].ToString() != "")
                    {
                        model.Eid = int.Parse(dt.Rows[n]["Eid"].ToString());
                    }
                    if (dt.Rows[n]["Etitle"] != null)
                    {
                        model.Etitle = dt.Rows[n]["Etitle"].ToString();
                    }
                    if (dt.Rows[n]["Edescription"] != null)
                    {
                        model.Edescription = dt.Rows[n]["Edescription"].ToString();
                    }
                    if (dt.Rows[n]["Cid"] != null && dt.Rows[n]["Cid"].ToString() != "")
                    {
                        model.Cid = int.Parse(dt.Rows[n]["Cid"].ToString());
                    }
                    if (dt.Rows[n]["Hid"] != null && dt.Rows[n]["Hid"].ToString() != "")
                    {
                        model.Hid = int.Parse(dt.Rows[n]["Hid"].ToString());
                    }
                    if (dt.Rows[n]["Etime"] != null && dt.Rows[n]["Etime"].ToString() != "")
                    {
                        model.Etime = DateTime.Parse(dt.Rows[n]["Etime"].ToString());
                    }
                    if (dt.Rows[n]["Eclose"] != null && dt.Rows[n]["Eclose"].ToString() != "")
                    {
                        model.Eclose = dt.Rows[n]["Eclose"].ToString() == "1" || dt.Rows[n]["Eclose"].ToString().ToLower() == "true";
                    }
                    if (dt.Rows[n]["Escore"] != null && dt.Rows[n]["Escore"].ToString() != "")
                    {
                        model.Escore = int.Parse(dt.Rows[n]["Escore"].ToString());
                    }
                    if (dt.Rows[n]["Ecount"] != null && dt.Rows[n]["Ecount"].ToString() != "")
                    {
                        model.Ecount = int.Parse(dt.Rows[n]["Ecount"].ToString());
                    }
                    if (dt.Rows[n]["Edata"] != null)
                    {
                        model.Edata = dt.Rows[n]["Edata"].ToString();
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        public static List<LearnSite.Model.Teacher> MapTeacherList(DataTable dt)
        {
            List<LearnSite.Model.Teacher> modelList = new List<LearnSite.Model.Teacher>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LearnSite.Model.Teacher model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LearnSite.Model.Teacher();
                    if (dt.Rows[n]["Hid"].ToString() != "")
                    {
                        model.Hid = int.Parse(dt.Rows[n]["Hid"].ToString());
                    }
                    model.Hname = dt.Rows[n]["Hname"].ToString();
                    model.Hpwd = dt.Rows[n]["Hpwd"].ToString();
                    if (dt.Rows[n]["Hpermiss"].ToString() != "")
                    {
                        model.Hpermiss = dt.Rows[n]["Hpermiss"].ToString() == "1" || dt.Rows[n]["Hpermiss"].ToString().ToLower() == "true";
                    }
                    model.Hnote = dt.Rows[n]["Hnote"].ToString();
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        public static List<LearnSite.Model.StudentsExcel> MapStudentsExcelList(DataTable dt)
        {
            List<LearnSite.Model.StudentsExcel> modelList = new List<LearnSite.Model.StudentsExcel>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LearnSite.Model.StudentsExcel model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LearnSite.Model.StudentsExcel();
                    if (dt.Rows[n]["Sid"].ToString() != "")
                    {
                        model.Sid = int.Parse(dt.Rows[n]["Sid"].ToString());
                    }
                    model.Snum = dt.Rows[n]["Snum"].ToString();
                    if (dt.Rows[n]["Syear"].ToString() != "")
                    {
                        model.Syear = int.Parse(dt.Rows[n]["Syear"].ToString());
                    }
                    if (dt.Rows[n]["Sgrade"].ToString() != "")
                    {
                        model.Sgrade = int.Parse(dt.Rows[n]["Sgrade"].ToString());
                    }
                    if (dt.Rows[n]["Sclass"].ToString() != "")
                    {
                        model.Sclass = int.Parse(dt.Rows[n]["Sclass"].ToString());
                    }
                    model.Sname = dt.Rows[n]["Sname"].ToString();
                    model.Spwd = dt.Rows[n]["Spwd"].ToString();
                    model.Sex = dt.Rows[n]["Sex"].ToString();
                    model.Saddress = dt.Rows[n]["Saddress"].ToString();
                    model.Sphone = dt.Rows[n]["Sphone"].ToString();
                    model.Sparents = dt.Rows[n]["Sparents"].ToString();
                    model.Sheadtheacher = dt.Rows[n]["Sheadtheacher"].ToString();
                    if (dt.Rows[n]["Sscore"].ToString() != "")
                    {
                        model.Sscore = int.Parse(dt.Rows[n]["Sscore"].ToString());
                    }
                    if (dt.Rows[n]["Squiz"].ToString() != "")
                    {
                        model.Squiz = int.Parse(dt.Rows[n]["Squiz"].ToString());
                    }
                    if (dt.Rows[n]["Sattitude"].ToString() != "")
                    {
                        model.Sattitude = int.Parse(dt.Rows[n]["Sattitude"].ToString());
                    }
                    model.Sape = dt.Rows[n]["Sape"].ToString();
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        public static List<LearnSite.Model.ShareDisk> MapShareDiskList(DataTable dt)
        {
            List<LearnSite.Model.ShareDisk> modelList = new List<LearnSite.Model.ShareDisk>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LearnSite.Model.ShareDisk model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LearnSite.Model.ShareDisk();
                    if (dt.Rows[n]["Kid"] != null && dt.Rows[n]["Kid"].ToString() != "")
                    {
                        model.Kid = int.Parse(dt.Rows[n]["Kid"].ToString());
                    }
                    if (dt.Rows[n]["Kown"] != null && dt.Rows[n]["Kown"].ToString() != "")
                    {
                        model.Kown = dt.Rows[n]["Kown"].ToString() == "1" || dt.Rows[n]["Kown"].ToString().ToLower() == "true";
                    }
                    if (dt.Rows[n]["Kyear"] != null && dt.Rows[n]["Kyear"].ToString() != "")
                    {
                        model.Kyear = int.Parse(dt.Rows[n]["Kyear"].ToString());
                    }
                    if (dt.Rows[n]["Kgrade"] != null && dt.Rows[n]["Kgrade"].ToString() != "")
                    {
                        model.Kgrade = int.Parse(dt.Rows[n]["Kgrade"].ToString());
                    }
                    if (dt.Rows[n]["Kclass"] != null && dt.Rows[n]["Kclass"].ToString() != "")
                    {
                        model.Kclass = int.Parse(dt.Rows[n]["Kclass"].ToString());
                    }
                    if (dt.Rows[n]["Kgroup"] != null && dt.Rows[n]["Kgroup"].ToString() != "")
                    {
                        model.Kgroup = int.Parse(dt.Rows[n]["Kgroup"].ToString());
                    }
                    if (dt.Rows[n]["Knum"] != null)
                    {
                        model.Knum = dt.Rows[n]["Knum"].ToString();
                    }
                    if (dt.Rows[n]["Kname"] != null)
                    {
                        model.Kname = dt.Rows[n]["Kname"].ToString();
                    }
                    if (dt.Rows[n]["Kfilename"] != null)
                    {
                        model.Kfilename = dt.Rows[n]["Kfilename"].ToString();
                    }
                    if (dt.Rows[n]["Kfsize"] != null && dt.Rows[n]["Kfsize"].ToString() != "")
                    {
                        model.Kfsize = int.Parse(dt.Rows[n]["Kfsize"].ToString());
                    }
                    if (dt.Rows[n]["Kfurl"] != null)
                    {
                        model.Kfurl = dt.Rows[n]["Kfurl"].ToString();
                    }
                    if (dt.Rows[n]["Kftpe"] != null)
                    {
                        model.Kftpe = dt.Rows[n]["Kftpe"].ToString();
                    }
                    if (dt.Rows[n]["Kfdate"] != null && dt.Rows[n]["Kfdate"].ToString() != "")
                    {
                        model.Kfdate = DateTime.Parse(dt.Rows[n]["Kfdate"].ToString());
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        public static List<LearnSite.Model.TopicReply> MapTopicReplyList(DataTable dt)
        {
            List<LearnSite.Model.TopicReply> modelList = new List<LearnSite.Model.TopicReply>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LearnSite.Model.TopicReply model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LearnSite.Model.TopicReply();
                    if (dt.Rows[n]["Rid"].ToString() != "")
                    {
                        model.Rid = int.Parse(dt.Rows[n]["Rid"].ToString());
                    }
                    if (dt.Rows[n]["Rtid"].ToString() != "")
                    {
                        model.Rtid = int.Parse(dt.Rows[n]["Rtid"].ToString());
                    }
                    model.Rsnum = dt.Rows[n]["Rsnum"].ToString();
                    model.Rwords = dt.Rows[n]["Rwords"].ToString();
                    if (dt.Rows[n]["Rtime"].ToString() != "")
                    {
                        model.Rtime = DateTime.Parse(dt.Rows[n]["Rtime"].ToString());
                    }
                    model.Rip = dt.Rows[n]["Rip"].ToString();
                    if (dt.Rows[n]["Rscore"].ToString() != "")
                    {
                        model.Rscore = int.Parse(dt.Rows[n]["Rscore"].ToString());
                    }
                    if (dt.Rows[n]["Rban"].ToString() != "")
                    {
                        model.Rban = dt.Rows[n]["Rban"].ToString() == "1" || dt.Rows[n]["Rban"].ToString().ToLower() == "true";
                    }
                    if (dt.Rows[n]["Rgrade"].ToString() != "")
                    {
                        model.Rgrade = int.Parse(dt.Rows[n]["Rgrade"].ToString());
                    }
                    if (dt.Rows[n]["Rterm"].ToString() != "")
                    {
                        model.Rterm = int.Parse(dt.Rows[n]["Rterm"].ToString());
                    }
                    if (dt.Rows[n]["Rcid"].ToString() != "")
                    {
                        model.Rcid = int.Parse(dt.Rows[n]["Rcid"].ToString());
                    }
                    if (dt.Rows[n]["Rclass"].ToString() != "")
                    {
                        model.Rclass = int.Parse(dt.Rows[n]["Rclass"].ToString());
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        public static List<LearnSite.Model.WorksDiscuss> MapWorksDiscussList(DataTable dt)
        {
            List<LearnSite.Model.WorksDiscuss> modelList = new List<LearnSite.Model.WorksDiscuss>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LearnSite.Model.WorksDiscuss model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LearnSite.Model.WorksDiscuss();
                    if (dt.Rows[n]["Did"].ToString() != "")
                    {
                        model.Did = int.Parse(dt.Rows[n]["Did"].ToString());
                    }
                    if (dt.Rows[n]["Dwid"].ToString() != "")
                    {
                        model.Dwid = int.Parse(dt.Rows[n]["Dwid"].ToString());
                    }
                    model.Dsnum = dt.Rows[n]["Dsnum"].ToString();
                    model.Dwords = dt.Rows[n]["Dwords"].ToString();
                    if (dt.Rows[n]["Dtime"].ToString() != "")
                    {
                        model.Dtime = DateTime.Parse(dt.Rows[n]["Dtime"].ToString());
                    }
                    model.Dip = dt.Rows[n]["Dip"].ToString();
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        public static List<LearnSite.Model.TopicDiscuss> MapTopicDiscussList(DataTable dt)
        {
            List<LearnSite.Model.TopicDiscuss> modelList = new List<LearnSite.Model.TopicDiscuss>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LearnSite.Model.TopicDiscuss model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LearnSite.Model.TopicDiscuss();
                    if (dt.Rows[n]["Tid"].ToString() != "")
                    {
                        model.Tid = int.Parse(dt.Rows[n]["Tid"].ToString());
                    }
                    if (dt.Rows[n]["Tcid"].ToString() != "")
                    {
                        model.Tcid = int.Parse(dt.Rows[n]["Tcid"].ToString());
                    }
                    model.Ttitle = dt.Rows[n]["Ttitle"].ToString();
                    model.Tcontent = dt.Rows[n]["Tcontent"].ToString();
                    if (dt.Rows[n]["Tcount"].ToString() != "")
                    {
                        model.Tcount = int.Parse(dt.Rows[n]["Tcount"].ToString());
                    }
                    if (dt.Rows[n]["Tteacher"].ToString() != "")
                    {
                        model.Tteacher = int.Parse(dt.Rows[n]["Tteacher"].ToString());
                    }
                    if (dt.Rows[n]["Tdate"].ToString() != "")
                    {
                        model.Tdate = DateTime.Parse(dt.Rows[n]["Tdate"].ToString());
                    }
                    if (dt.Rows[n]["Tclose"].ToString() != "")
                    {
                        model.Tclose = dt.Rows[n]["Tclose"].ToString() == "1" || dt.Rows[n]["Tclose"].ToString().ToLower() == "true";
                    }
                    model.Tresult = dt.Rows[n]["Tresult"].ToString();
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        public static List<LearnSite.Model.TurtleAnswer> MapTurtleAnswerList(DataTable dt)
        {
            List<LearnSite.Model.TurtleAnswer> modelList = new List<LearnSite.Model.TurtleAnswer>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LearnSite.Model.TurtleAnswer model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LearnSite.Model.TurtleAnswer();
                    if (dt.Rows[n]["Aid"] != null && dt.Rows[n]["Aid"].ToString() != "")
                    {
                        model.Aid = int.Parse(dt.Rows[n]["Aid"].ToString());
                    }
                    if (dt.Rows[n]["Amid"] != null && dt.Rows[n]["Amid"].ToString() != "")
                    {
                        model.Amid = int.Parse(dt.Rows[n]["Amid"].ToString());
                    }
                    if (dt.Rows[n]["Aqid"] != null && dt.Rows[n]["Aqid"].ToString() != "")
                    {
                        model.Aqid = int.Parse(dt.Rows[n]["Aqid"].ToString());
                    }
                    if (dt.Rows[n]["Acode"] != null)
                    {
                        model.Acode = dt.Rows[n]["Acode"].ToString();
                    }
                    if (dt.Rows[n]["Aimg"] != null)
                    {
                        model.Aimg = dt.Rows[n]["Aimg"].ToString();
                    }
                    if (dt.Rows[n]["Aurl"] != null)
                    {
                        model.Aurl = dt.Rows[n]["Aurl"].ToString();
                    }
                    if (dt.Rows[n]["Aout"] != null)
                    {
                        model.Aout = dt.Rows[n]["Aout"].ToString();
                    }
                    if (dt.Rows[n]["Ascore"] != null && dt.Rows[n]["Ascore"].ToString() != "")
                    {
                        model.Ascore = int.Parse(dt.Rows[n]["Ascore"].ToString());
                    }
                    if (dt.Rows[n]["Asid"] != null && dt.Rows[n]["Asid"].ToString() != "")
                    {
                        model.Asid = int.Parse(dt.Rows[n]["Asid"].ToString());
                    }
                    if (dt.Rows[n]["Asname"] != null)
                    {
                        model.Asname = dt.Rows[n]["Asname"].ToString();
                    }
                    if (dt.Rows[n]["Alock"] != null && dt.Rows[n]["Alock"].ToString() != "")
                    {
                        model.Alock = dt.Rows[n]["Alock"].ToString() == "1" || dt.Rows[n]["Alock"].ToString().ToLower() == "true";
                    }
                    if (dt.Rows[n]["Adate"] != null && dt.Rows[n]["Adate"].ToString() != "")
                    {
                        model.Adate = DateTime.Parse(dt.Rows[n]["Adate"].ToString());
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        public static List<LearnSite.Model.TurtleQuestion> MapTurtleQuestionList(DataTable dt)
        {
            List<LearnSite.Model.TurtleQuestion> modelList = new List<LearnSite.Model.TurtleQuestion>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LearnSite.Model.TurtleQuestion model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LearnSite.Model.TurtleQuestion();
                    if (dt.Rows[n]["Qid"] != null && dt.Rows[n]["Qid"].ToString() != "")
                    {
                        model.Qid = int.Parse(dt.Rows[n]["Qid"].ToString());
                    }
                    if (dt.Rows[n]["Qmid"] != null && dt.Rows[n]["Qmid"].ToString() != "")
                    {
                        model.Qmid = int.Parse(dt.Rows[n]["Qmid"].ToString());
                    }
                    if (dt.Rows[n]["Qtitle"] != null)
                    {
                        model.Qtitle = dt.Rows[n]["Qtitle"].ToString();
                    }
                    if (dt.Rows[n]["Qcontent"] != null)
                    {
                        model.Qcontent = dt.Rows[n]["Qcontent"].ToString();
                    }
                    if (dt.Rows[n]["Qdegree"] != null && dt.Rows[n]["Qdegree"].ToString() != "")
                    {
                        model.Qdegree = int.Parse(dt.Rows[n]["Qdegree"].ToString());
                    }
                    if (dt.Rows[n]["Qsort"] != null && dt.Rows[n]["Qsort"].ToString() != "")
                    {
                        model.Qsort = int.Parse(dt.Rows[n]["Qsort"].ToString());
                    }
                    if (dt.Rows[n]["Qcode"] != null)
                    {
                        model.Qcode = dt.Rows[n]["Qcode"].ToString();
                    }
                    if (dt.Rows[n]["Qimg"] != null)
                    {
                        model.Qimg = dt.Rows[n]["Qimg"].ToString();
                    }
                    if (dt.Rows[n]["Qurl"] != null)
                    {
                        model.Qurl = dt.Rows[n]["Qurl"].ToString();
                    }
                    if (dt.Rows[n]["Qout"] != null)
                    {
                        model.Qout = dt.Rows[n]["Qout"].ToString();
                    }
                    if (dt.Rows[n]["Qscore"] != null && dt.Rows[n]["Qscore"].ToString() != "")
                    {
                        model.Qscore = int.Parse(dt.Rows[n]["Qscore"].ToString());
                    }
                    if (dt.Rows[n]["Qdate"] != null && dt.Rows[n]["Qdate"].ToString() != "")
                    {
                        model.Qdate = DateTime.Parse(dt.Rows[n]["Qdate"].ToString());
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        public static List<LearnSite.Model.Turtle> MapTurtleList(DataTable dt)
        {
            List<LearnSite.Model.Turtle> modelList = new List<LearnSite.Model.Turtle>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LearnSite.Model.Turtle model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LearnSite.Model.Turtle();
                    if (dt.Rows[n]["Tid"] != null && dt.Rows[n]["Tid"].ToString() != "")
                    {
                        model.Tid = int.Parse(dt.Rows[n]["Tid"].ToString());
                    }
                    if (dt.Rows[n]["Thid"] != null && dt.Rows[n]["Thid"].ToString() != "")
                    {
                        model.Thid = int.Parse(dt.Rows[n]["Thid"].ToString());
                    }
                    if (dt.Rows[n]["Ttilte"] != null)
                    {
                        model.Ttilte = dt.Rows[n]["Ttilte"].ToString();
                    }
                    if (dt.Rows[n]["Tcontent"] != null)
                    {
                        model.Tcontent = dt.Rows[n]["Tcontent"].ToString();
                    }
                    if (dt.Rows[n]["Tdegree"] != null && dt.Rows[n]["Tdegree"].ToString() != "")
                    {
                        model.Tdegree = int.Parse(dt.Rows[n]["Tdegree"].ToString());
                    }
                    if (dt.Rows[n]["Tsort"] != null && dt.Rows[n]["Tsort"].ToString() != "")
                    {
                        model.Tsort = int.Parse(dt.Rows[n]["Tsort"].ToString());
                    }
                    if (dt.Rows[n]["Tcode"] != null)
                    {
                        model.Tcode = dt.Rows[n]["Tcode"].ToString();
                    }
                    if (dt.Rows[n]["Timg"] != null)
                    {
                        model.Timg = dt.Rows[n]["Timg"].ToString();
                    }
                    if (dt.Rows[n]["Turl"] != null)
                    {
                        model.Turl = dt.Rows[n]["Turl"].ToString();
                    }
                    if (dt.Rows[n]["Tout"] != null)
                    {
                        model.Tout = dt.Rows[n]["Tout"].ToString();
                    }
                    if (dt.Rows[n]["Tdate"] != null && dt.Rows[n]["Tdate"].ToString() != "")
                    {
                        model.Tdate = DateTime.Parse(dt.Rows[n]["Tdate"].ToString());
                    }
                    if (dt.Rows[n]["Tstudy"] != null && dt.Rows[n]["Tstudy"].ToString() != "")
                    {
                        model.Tstudy = dt.Rows[n]["Tstudy"].ToString() == "1" || dt.Rows[n]["Tstudy"].ToString().ToLower() == "true";
                    }
                    if (dt.Rows[n]["Tsid"] != null && dt.Rows[n]["Tsid"].ToString() != "")
                    {
                        model.Tsid = int.Parse(dt.Rows[n]["Tsid"].ToString());
                    }
                    if (dt.Rows[n]["Tscore"] != null && dt.Rows[n]["Tscore"].ToString() != "")
                    {
                        model.Tscore = int.Parse(dt.Rows[n]["Tscore"].ToString());
                    }
                    if (dt.Rows[n]["Tip"] != null)
                    {
                        model.Tip = dt.Rows[n]["Tip"].ToString();
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        public static List<LearnSite.Model.Answers> MapAnswersList(DataTable dt)
        {
            List<LearnSite.Model.Answers> modelList = new List<LearnSite.Model.Answers>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LearnSite.Model.Answers model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LearnSite.Model.Answers();
                    if (dt.Rows[n]["Aid"] != null && dt.Rows[n]["Aid"].ToString() != "")
                    {
                        model.Aid = int.Parse(dt.Rows[n]["Aid"].ToString());
                    }
                    if (dt.Rows[n]["Eid"] != null && dt.Rows[n]["Eid"].ToString() != "")
                    {
                        model.Eid = int.Parse(dt.Rows[n]["Eid"].ToString());
                    }
                    if (dt.Rows[n]["Asid"] != null && dt.Rows[n]["Asid"].ToString() != "")
                    {
                        model.Asid = int.Parse(dt.Rows[n]["Asid"].ToString());
                    }
                    if (dt.Rows[n]["Asnum"] != null && dt.Rows[n]["Asnum"].ToString() != "")
                    {
                        model.Asnum = dt.Rows[n]["Asnum"].ToString();
                    }
                    if (dt.Rows[n]["Asname"] != null)
                    {
                        model.Asname = dt.Rows[n]["Asname"].ToString();
                    }
                    if (dt.Rows[n]["Asgrade"] != null && dt.Rows[n]["Asgrade"].ToString() != "")
                    {
                        model.Asgrade = int.Parse(dt.Rows[n]["Asgrade"].ToString());
                    }
                    if (dt.Rows[n]["Asclass"] != null && dt.Rows[n]["Asclass"].ToString() != "")
                    {
                        model.Asclass = int.Parse(dt.Rows[n]["Asclass"].ToString());
                    }
                    if (dt.Rows[n]["Atime"] != null && dt.Rows[n]["Atime"].ToString() != "")
                    {
                        model.Atime = DateTime.Parse(dt.Rows[n]["Atime"].ToString());
                    }
                    if (dt.Rows[n]["Ascore"] != null && dt.Rows[n]["Ascore"].ToString() != "")
                    {
                        model.Ascore = int.Parse(dt.Rows[n]["Ascore"].ToString());
                    }
                    if (dt.Rows[n]["Aspent"] != null && dt.Rows[n]["Aspent"].ToString() != "")
                    {
                        model.Aspent = int.Parse(dt.Rows[n]["Aspent"].ToString());
                    }
                    if (dt.Rows[n]["Adata"] != null)
                    {
                        model.Adata = dt.Rows[n]["Adata"].ToString();
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        public static List<LearnSite.Model.Game> MapGameList(DataTable dt)
        {
            List<LearnSite.Model.Game> modelList = new List<LearnSite.Model.Game>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LearnSite.Model.Game model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LearnSite.Model.Game();
                    if (dt.Rows[n]["Gid"] != null && dt.Rows[n]["Gid"].ToString() != "")
                    {
                        model.Gid = int.Parse(dt.Rows[n]["Gid"].ToString());
                    }
                    if (dt.Rows[n]["Gsid"] != null && dt.Rows[n]["Gsid"].ToString() != "")
                    {
                        model.Gsid = int.Parse(dt.Rows[n]["Gsid"].ToString());
                    }
                    if (dt.Rows[n]["Gsname"] != null && dt.Rows[n]["Gsname"].ToString() != "")
                    {
                        model.Gsname = dt.Rows[n]["Gsname"].ToString();
                    }
                    if (dt.Rows[n]["Gnum"] != null && dt.Rows[n]["Gnum"].ToString() != "")
                    {
                        model.Gnum = int.Parse(dt.Rows[n]["Gnum"].ToString());
                    }
                    if (dt.Rows[n]["Gtitle"] != null)
                    {
                        model.Gtitle = dt.Rows[n]["Gtitle"].ToString();
                    }
                    if (dt.Rows[n]["Gsave"] != null && dt.Rows[n]["Gsave"].ToString() != "")
                    {
                        model.Gsave = int.Parse(dt.Rows[n]["Gsave"].ToString());
                    }
                    if (dt.Rows[n]["Gnote"] != null)
                    {
                        model.Gnote = dt.Rows[n]["Gnote"].ToString();
                    }
                    if (dt.Rows[n]["Gscore"] != null && dt.Rows[n]["Gscore"].ToString() != "")
                    {
                        model.Gscore = int.Parse(dt.Rows[n]["Gscore"].ToString());
                    }
                    if (dt.Rows[n]["Gdate"] != null && dt.Rows[n]["Gdate"].ToString() != "")
                    {
                        model.Gdate = DateTime.Parse(dt.Rows[n]["Gdate"].ToString());
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        public static List<LearnSite.Model.Chinese> MapChineseList(DataTable dt)
        {
            List<LearnSite.Model.Chinese> modelList = new List<LearnSite.Model.Chinese>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LearnSite.Model.Chinese model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LearnSite.Model.Chinese();
                    if (dt.Rows[n]["Nid"] != null && dt.Rows[n]["Nid"].ToString() != "")
                    {
                        model.Nid = int.Parse(dt.Rows[n]["Nid"].ToString());
                    }
                    if (dt.Rows[n]["Ntitle"] != null)
                    {
                        model.Ntitle = dt.Rows[n]["Ntitle"].ToString();
                    }
                    if (dt.Rows[n]["Ncontent"] != null)
                    {
                        model.Ncontent = dt.Rows[n]["Ncontent"].ToString();
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        public static List<LearnSite.Model.Gauge> MapGaugeList(DataTable dt)
        {
            List<LearnSite.Model.Gauge> modelList = new List<LearnSite.Model.Gauge>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LearnSite.Model.Gauge model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LearnSite.Model.Gauge();
                    if (dt.Rows[n]["Gid"] != null && dt.Rows[n]["Gid"].ToString() != "")
                    {
                        model.Gid = int.Parse(dt.Rows[n]["Gid"].ToString());
                    }
                    if (dt.Rows[n]["Ghid"] != null && dt.Rows[n]["Ghid"].ToString() != "")
                    {
                        model.Ghid = int.Parse(dt.Rows[n]["Ghid"].ToString());
                    }
                    if (dt.Rows[n]["Gtype"] != null && dt.Rows[n]["Gtype"].ToString() != "")
                    {
                        model.Gtype = dt.Rows[n]["Gtype"].ToString();
                    }
                    if (dt.Rows[n]["Gtitle"] != null && dt.Rows[n]["Gtitle"].ToString() != "")
                    {
                        model.Gtitle = dt.Rows[n]["Gtitle"].ToString();
                    }
                    if (dt.Rows[n]["Gcount"] != null && dt.Rows[n]["Gcount"].ToString() != "")
                    {
                        model.Gcount = int.Parse(dt.Rows[n]["Gcount"].ToString());
                    }
                    if (dt.Rows[n]["Gdate"] != null && dt.Rows[n]["Gdate"].ToString() != "")
                    {
                        model.Gdate = DateTime.Parse(dt.Rows[n]["Gdate"].ToString());
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        public static List<LearnSite.Model.GaugeItem> MapGaugeItemList(DataTable dt)
        {
            List<LearnSite.Model.GaugeItem> modelList = new List<LearnSite.Model.GaugeItem>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LearnSite.Model.GaugeItem model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LearnSite.Model.GaugeItem();
                    if (dt.Rows[n]["Mid"] != null && dt.Rows[n]["Mid"].ToString() != "")
                    {
                        model.Mid = int.Parse(dt.Rows[n]["Mid"].ToString());
                    }
                    if (dt.Rows[n]["Mgid"] != null && dt.Rows[n]["Mgid"].ToString() != "")
                    {
                        model.Mgid = int.Parse(dt.Rows[n]["Mgid"].ToString());
                    }
                    if (dt.Rows[n]["Mitem"] != null && dt.Rows[n]["Mitem"].ToString() != "")
                    {
                        model.Mitem = dt.Rows[n]["Mitem"].ToString();
                    }
                    if (dt.Rows[n]["Mscore"] != null && dt.Rows[n]["Mscore"].ToString() != "")
                    {
                        model.Mscore = int.Parse(dt.Rows[n]["Mscore"].ToString());
                    }
                    if (dt.Rows[n]["Msort"] != null && dt.Rows[n]["Msort"].ToString() != "")
                    {
                        model.Msort = int.Parse(dt.Rows[n]["Msort"].ToString());
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        public static List<LearnSite.Model.GaugeFeedback> MapGaugeFeedbackList(DataTable dt)
        {
            List<LearnSite.Model.GaugeFeedback> modelList = new List<LearnSite.Model.GaugeFeedback>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LearnSite.Model.GaugeFeedback model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LearnSite.Model.GaugeFeedback();
                    if (dt.Rows[n]["Fid"] != null && dt.Rows[n]["Fid"].ToString() != "")
                    {
                        model.Fid = int.Parse(dt.Rows[n]["Fid"].ToString());
                    }
                    if (dt.Rows[n]["Fnum"] != null && dt.Rows[n]["Fnum"].ToString() != "")
                    {
                        model.Fnum = dt.Rows[n]["Fnum"].ToString();
                    }
                    if (dt.Rows[n]["Fgrade"] != null && dt.Rows[n]["Fgrade"].ToString() != "")
                    {
                        model.Fgrade = int.Parse(dt.Rows[n]["Fgrade"].ToString());
                    }
                    if (dt.Rows[n]["Fclass"] != null && dt.Rows[n]["Fclass"].ToString() != "")
                    {
                        model.Fclass = int.Parse(dt.Rows[n]["Fclass"].ToString());
                    }
                    if (dt.Rows[n]["Fcid"] != null && dt.Rows[n]["Fcid"].ToString() != "")
                    {
                        model.Fcid = int.Parse(dt.Rows[n]["Fcid"].ToString());
                    }
                    if (dt.Rows[n]["Fmid"] != null && dt.Rows[n]["Fmid"].ToString() != "")
                    {
                        model.Fmid = int.Parse(dt.Rows[n]["Fmid"].ToString());
                    }
                    if (dt.Rows[n]["Fwid"] != null && dt.Rows[n]["Fwid"].ToString() != "")
                    {
                        model.Fwid = int.Parse(dt.Rows[n]["Fwid"].ToString());
                    }
                    if (dt.Rows[n]["Fgid"] != null && dt.Rows[n]["Fgid"].ToString() != "")
                    {
                        model.Fgid = int.Parse(dt.Rows[n]["Fgid"].ToString());
                    }
                    if (dt.Rows[n]["Fselect"] != null && dt.Rows[n]["Fselect"].ToString() != "")
                    {
                        model.Fselect = dt.Rows[n]["Fselect"].ToString();
                    }
                    if (dt.Rows[n]["Fscore"] != null && dt.Rows[n]["Fscore"].ToString() != "")
                    {
                        model.Fscore = int.Parse(dt.Rows[n]["Fscore"].ToString());
                    }
                    if (dt.Rows[n]["Fgood"] != null && dt.Rows[n]["Fgood"].ToString() != "")
                    {
                        model.Fgood = dt.Rows[n]["Fgood"].ToString() == "1" || dt.Rows[n]["Fgood"].ToString().ToLower() == "true";
                    }
                    if (dt.Rows[n]["Fdate"] != null && dt.Rows[n]["Fdate"].ToString() != "")
                    {
                        model.Fdate = DateTime.Parse(dt.Rows[n]["Fdate"].ToString());
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        public static List<LearnSite.Model.English> MapEnglishList(DataTable dt)
        {
            List<LearnSite.Model.English> modelList = new List<LearnSite.Model.English>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LearnSite.Model.English model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LearnSite.Model.English();
                    if (dt.Rows[n]["Eid"].ToString() != "")
                    {
                        model.Eid = int.Parse(dt.Rows[n]["Eid"].ToString());
                    }
                    model.Eword = dt.Rows[n]["Eword"].ToString();
                    model.Emeaning = dt.Rows[n]["Emeaning"].ToString();
                    if (dt.Rows[n]["Elevel"].ToString() != "")
                    {
                        model.Elevel = int.Parse(dt.Rows[n]["Elevel"].ToString());
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        public static List<LearnSite.Model.Summary> MapSummaryList(DataTable dt)
        {
            List<LearnSite.Model.Summary> modelList = new List<LearnSite.Model.Summary>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LearnSite.Model.Summary model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LearnSite.Model.Summary();
                    if (dt.Rows[n]["Sid"].ToString() != "")
                    {
                        model.Sid = int.Parse(dt.Rows[n]["Sid"].ToString());
                    }
                    if (dt.Rows[n]["Scid"].ToString() != "")
                    {
                        model.Scid = int.Parse(dt.Rows[n]["Scid"].ToString());
                    }
                    if (dt.Rows[n]["Shid"].ToString() != "")
                    {
                        model.Shid = int.Parse(dt.Rows[n]["Shid"].ToString());
                    }
                    model.Scontent = dt.Rows[n]["Scontent"].ToString();
                    if (dt.Rows[n]["Sdate"].ToString() != "")
                    {
                        model.Sdate = DateTime.Parse(dt.Rows[n]["Sdate"].ToString());
                    }
                    if (dt.Rows[n]["Sgrade"].ToString() != "")
                    {
                        model.Sgrade = int.Parse(dt.Rows[n]["Sgrade"].ToString());
                    }
                    if (dt.Rows[n]["Sclass"].ToString() != "")
                    {
                        model.Sclass = int.Parse(dt.Rows[n]["Sclass"].ToString());
                    }
                    if (dt.Rows[n]["Syear"].ToString() != "")
                    {
                        model.Syear = int.Parse(dt.Rows[n]["Syear"].ToString());
                    }
                    if (dt.Rows[n]["Sshow"].ToString() != "")
                    {
                        if ((dt.Rows[n]["Sshow"].ToString() == "1") || (dt.Rows[n]["Sshow"].ToString().ToLower() == "true"))
                        {
                            model.Sshow = true;
                        }
                        else
                        {
                            model.Sshow = false;
                        }
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        public static List<LearnSite.Model.Pchinese> MapPchineseList(DataTable dt)
        {
            List<LearnSite.Model.Pchinese> modelList = new List<LearnSite.Model.Pchinese>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LearnSite.Model.Pchinese model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LearnSite.Model.Pchinese();
                    if (dt.Rows[n]["Pid"] != null && dt.Rows[n]["Pid"].ToString() != "")
                    {
                        model.Pid = int.Parse(dt.Rows[n]["Pid"].ToString());
                    }
                    if (dt.Rows[n]["Psid"] != null && dt.Rows[n]["Psid"].ToString() != "")
                    {
                        model.Psid = int.Parse(dt.Rows[n]["Psid"].ToString());
                    }
                    if (dt.Rows[n]["Psnum"] != null)
                    {
                        model.Psnum = dt.Rows[n]["Psnum"].ToString();
                    }
                    if (dt.Rows[n]["Papple"] != null && dt.Rows[n]["Papple"].ToString() != "")
                    {
                        model.Papple = int.Parse(dt.Rows[n]["Papple"].ToString());
                    }
                    if (dt.Rows[n]["Ptotal"] != null && dt.Rows[n]["Ptotal"].ToString() != "")
                    {
                        model.Ptotal = int.Parse(dt.Rows[n]["Ptotal"].ToString());
                    }
                    if (dt.Rows[n]["Pspeed"] != null && dt.Rows[n]["Pspeed"].ToString() != "")
                    {
                        model.Pspeed = int.Parse(dt.Rows[n]["Pspeed"].ToString());
                    }
                    if (dt.Rows[n]["Pdegree"] != null && dt.Rows[n]["Pdegree"].ToString() != "")
                    {
                        model.Pdegree = int.Parse(dt.Rows[n]["Pdegree"].ToString());
                    }
                    if (dt.Rows[n]["Pyear"] != null && dt.Rows[n]["Pyear"].ToString() != "")
                    {
                        model.Pyear = int.Parse(dt.Rows[n]["Pyear"].ToString());
                    }
                    if (dt.Rows[n]["Pgrade"] != null && dt.Rows[n]["Pgrade"].ToString() != "")
                    {
                        model.Pgrade = int.Parse(dt.Rows[n]["Pgrade"].ToString());
                    }
                    if (dt.Rows[n]["Pclass"] != null && dt.Rows[n]["Pclass"].ToString() != "")
                    {
                        model.Pclass = int.Parse(dt.Rows[n]["Pclass"].ToString());
                    }
                    if (dt.Rows[n]["Pterm"] != null && dt.Rows[n]["Pterm"].ToString() != "")
                    {
                        model.Pterm = int.Parse(dt.Rows[n]["Pterm"].ToString());
                    }
                    if (dt.Rows[n]["Pdate"] != null && dt.Rows[n]["Pdate"].ToString() != "")
                    {
                        model.Pdate = DateTime.Parse(dt.Rows[n]["Pdate"].ToString());
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        public static List<LearnSite.Model.SurveyClass> MapSurveyClassList(DataTable dt)
        {
            List<LearnSite.Model.SurveyClass> modelList = new List<LearnSite.Model.SurveyClass>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LearnSite.Model.SurveyClass model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LearnSite.Model.SurveyClass();
                    if (dt.Rows[n]["Yid"] != null && dt.Rows[n]["Yid"].ToString() != "")
                    {
                        model.Yid = int.Parse(dt.Rows[n]["Yid"].ToString());
                    }
                    if (dt.Rows[n]["Yyear"] != null && dt.Rows[n]["Yyear"].ToString() != "")
                    {
                        model.Yyear = int.Parse(dt.Rows[n]["Yyear"].ToString());
                    }
                    if (dt.Rows[n]["Ygrade"] != null && dt.Rows[n]["Ygrade"].ToString() != "")
                    {
                        model.Ygrade = int.Parse(dt.Rows[n]["Ygrade"].ToString());
                    }
                    if (dt.Rows[n]["Yclass"] != null && dt.Rows[n]["Yclass"].ToString() != "")
                    {
                        model.Yclass = int.Parse(dt.Rows[n]["Yclass"].ToString());
                    }
                    if (dt.Rows[n]["Yterm"] != null && dt.Rows[n]["Yterm"].ToString() != "")
                    {
                        model.Yterm = int.Parse(dt.Rows[n]["Yterm"].ToString());
                    }
                    if (dt.Rows[n]["Ycid"] != null && dt.Rows[n]["Ycid"].ToString() != "")
                    {
                        model.Ycid = int.Parse(dt.Rows[n]["Ycid"].ToString());
                    }
                    if (dt.Rows[n]["Yvid"] != null && dt.Rows[n]["Yvid"].ToString() != "")
                    {
                        model.Yvid = int.Parse(dt.Rows[n]["Yvid"].ToString());
                    }
                    if (dt.Rows[n]["Yselect"] != null && dt.Rows[n]["Yselect"].ToString() != "")
                    {
                        model.Yselect = dt.Rows[n]["Yselect"].ToString();
                    }
                    if (dt.Rows[n]["Ycount"] != null && dt.Rows[n]["Ycount"].ToString() != "")
                    {
                        model.Ycount = dt.Rows[n]["Ycount"].ToString();
                    }
                    if (dt.Rows[n]["Yscore"] != null && dt.Rows[n]["Yscore"].ToString() != "")
                    {
                        model.Yscore = int.Parse(dt.Rows[n]["Yscore"].ToString());
                    }
                    if (dt.Rows[n]["Ydate"] != null && dt.Rows[n]["Ydate"].ToString() != "")
                    {
                        model.Ydate = DateTime.Parse(dt.Rows[n]["Ydate"].ToString());
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        public static List<LearnSite.Model.SurveyFeedback> MapSurveyFeedbackList(DataTable dt)
        {
            List<LearnSite.Model.SurveyFeedback> modelList = new List<LearnSite.Model.SurveyFeedback>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LearnSite.Model.SurveyFeedback model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LearnSite.Model.SurveyFeedback();
                    if (dt.Rows[n]["Fid"] != null && dt.Rows[n]["Fid"].ToString() != "")
                    {
                        model.Fid = int.Parse(dt.Rows[n]["Fid"].ToString());
                    }
                    if (dt.Rows[n]["Fnum"] != null && dt.Rows[n]["Fnum"].ToString() != "")
                    {
                        model.Fnum = dt.Rows[n]["Fnum"].ToString();
                    }
                    if (dt.Rows[n]["Fyear"] != null && dt.Rows[n]["Fyear"].ToString() != "")
                    {
                        model.Fyear = int.Parse(dt.Rows[n]["Fyear"].ToString());
                    }
                    if (dt.Rows[n]["Fgrade"] != null && dt.Rows[n]["Fgrade"].ToString() != "")
                    {
                        model.Fgrade = int.Parse(dt.Rows[n]["Fgrade"].ToString());
                    }
                    if (dt.Rows[n]["Fclass"] != null && dt.Rows[n]["Fclass"].ToString() != "")
                    {
                        model.Fclass = int.Parse(dt.Rows[n]["Fclass"].ToString());
                    }
                    if (dt.Rows[n]["Fterm"] != null && dt.Rows[n]["Fterm"].ToString() != "")
                    {
                        model.Fterm = int.Parse(dt.Rows[n]["Fterm"].ToString());
                    }
                    if (dt.Rows[n]["Fcid"] != null && dt.Rows[n]["Fcid"].ToString() != "")
                    {
                        model.Fcid = int.Parse(dt.Rows[n]["Fcid"].ToString());
                    }
                    if (dt.Rows[n]["Fvid"] != null && dt.Rows[n]["Fvid"].ToString() != "")
                    {
                        model.Fvid = int.Parse(dt.Rows[n]["Fvid"].ToString());
                    }
                    if (dt.Rows[n]["Fvtype"] != null && dt.Rows[n]["Fvtype"].ToString() != "")
                    {
                        model.Fvtype = int.Parse(dt.Rows[n]["Fvtype"].ToString());
                    }
                    if (dt.Rows[n]["Fselect"] != null && dt.Rows[n]["Fselect"].ToString() != "")
                    {
                        model.Fselect = dt.Rows[n]["Fselect"].ToString();
                    }
                    if (dt.Rows[n]["Fscore"] != null && dt.Rows[n]["Fscore"].ToString() != "")
                    {
                        model.Fscore = int.Parse(dt.Rows[n]["Fscore"].ToString());
                    }
                    if (dt.Rows[n]["Fdate"] != null && dt.Rows[n]["Fdate"].ToString() != "")
                    {
                        model.Fdate = DateTime.Parse(dt.Rows[n]["Fdate"].ToString());
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        public static List<LearnSite.Model.Room> MapRoomList(DataTable dt)
        {
            List<LearnSite.Model.Room> modelList = new List<LearnSite.Model.Room>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LearnSite.Model.Room model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LearnSite.Model.Room();
                    if (dt.Rows[n]["Rid"].ToString() != "")
                    {
                        model.Rid = int.Parse(dt.Rows[n]["Rid"].ToString());
                    }
                    if (dt.Rows[n]["Rhid"].ToString() != "")
                    {
                        model.Rhid = int.Parse(dt.Rows[n]["Rhid"].ToString());
                    }
                    if (dt.Rows[n]["Rgrade"].ToString() != "")
                    {
                        model.Rgrade = int.Parse(dt.Rows[n]["Rgrade"].ToString());
                    }
                    if (dt.Rows[n]["Rclass"].ToString() != "")
                    {
                        model.Rclass = int.Parse(dt.Rows[n]["Rclass"].ToString());
                    }
                    if (dt.Rows[n]["Rset"].ToString() != "")
                    {
                        if ((dt.Rows[n]["Rset"].ToString() == "1") || (dt.Rows[n]["Rset"].ToString().ToLower() == "true"))
                        {
                            model.Rset = true;
                        }
                        else
                        {
                            model.Rset = false;
                        }
                    }
                    model.Rpwd = dt.Rows[n]["Rpwd"].ToString();
                    if (dt.Rows[n]["Rlock"].ToString() != "")
                    {
                        if ((dt.Rows[n]["Rlock"].ToString() == "1") || (dt.Rows[n]["Rlock"].ToString().ToLower() == "true"))
                        {
                            model.Rlock = true;
                        }
                        else
                        {
                            model.Rlock = false;
                        }
                    }
                    model.Rip = dt.Rows[n]["Rip"].ToString();
                    if (dt.Rows[n]["Rgauge"] != null && dt.Rows[n]["Rgauge"].ToString() != "")
                    {
                        if ((dt.Rows[n]["Rgauge"].ToString() == "1") || (dt.Rows[n]["Rgauge"].ToString().ToLower() == "true"))
                        {
                            model.Rgauge = true;
                        }
                        else
                        {
                            model.Rgauge = false;
                        }
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        public static List<LearnSite.Model.Students> MapStudentsList(DataTable dt)
        {
            List<LearnSite.Model.Students> modelList = new List<LearnSite.Model.Students>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LearnSite.Model.Students model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LearnSite.Model.Students();
                    if (dt.Rows[n]["Sid"].ToString() != "")
                    {
                        model.Sid = int.Parse(dt.Rows[n]["Sid"].ToString());
                    }
                    model.Snum = dt.Rows[n]["Snum"].ToString();
                    if (dt.Rows[n]["Syear"].ToString() != "")
                    {
                        model.Syear = int.Parse(dt.Rows[n]["Syear"].ToString());
                    }
                    if (dt.Rows[n]["Sgrade"].ToString() != "")
                    {
                        model.Sgrade = int.Parse(dt.Rows[n]["Sgrade"].ToString());
                    }
                    if (dt.Rows[n]["Sclass"].ToString() != "")
                    {
                        model.Sclass = int.Parse(dt.Rows[n]["Sclass"].ToString());
                    }
                    model.Sname = dt.Rows[n]["Sname"].ToString();
                    model.Spwd = dt.Rows[n]["Spwd"].ToString();
                    model.Sex = dt.Rows[n]["Sex"].ToString();
                    model.Saddress = dt.Rows[n]["Saddress"].ToString();
                    model.Sphone = dt.Rows[n]["Sphone"].ToString();
                    model.Sparents = dt.Rows[n]["Sparents"].ToString();
                    model.Sheadtheacher = dt.Rows[n]["Sheadtheacher"].ToString();
                    if (dt.Rows[n]["Sscore"].ToString() != "")
                    {
                        model.Sscore = int.Parse(dt.Rows[n]["Sscore"].ToString());
                    }
                    if (dt.Rows[n]["Squiz"].ToString() != "")
                    {
                        model.Squiz = int.Parse(dt.Rows[n]["Squiz"].ToString());
                    }
                    if (dt.Rows[n]["Sattitude"].ToString() != "")
                    {
                        model.Sattitude = int.Parse(dt.Rows[n]["Sattitude"].ToString());
                    }
                    model.Sape = dt.Rows[n]["Sape"].ToString();
                    if (dt.Rows[n]["Swscore"].ToString() != "")
                    {
                        model.Swscore = int.Parse(dt.Rows[n]["Swscore"].ToString());
                    }
                    if (dt.Rows[n]["Stscore"].ToString() != "")
                    {
                        model.Stscore = int.Parse(dt.Rows[n]["Stscore"].ToString());
                    }
                    if (dt.Rows[n]["Sallscore"].ToString() != "")
                    {
                        model.Sallscore = int.Parse(dt.Rows[n]["Sallscore"].ToString());
                    }
                    if (dt.Rows[n]["Spscore"].ToString() != "")
                    {
                        model.Spscore = int.Parse(dt.Rows[n]["Spscore"].ToString());
                    }
                    if (dt.Rows[n]["Sgroup"].ToString() != "")
                    {
                        model.Sgroup = int.Parse(dt.Rows[n]["Sgroup"].ToString());
                    }
                    if (dt.Rows[n]["Sleader"].ToString() != "")
                    {
                        if ((dt.Rows[n]["Sleader"].ToString() == "1") || (dt.Rows[n]["Sleader"].ToString().ToLower() == "true"))
                        {
                            model.Sleader = true;
                        }
                        else
                        {
                            model.Sleader = false;
                        }
                    }
                    if (dt.Rows[n]["Svote"].ToString() != "")
                    {
                        model.Svote = int.Parse(dt.Rows[n]["Svote"].ToString());
                    }
                    if (dt.Rows[n]["Sgscore"].ToString() != "")
                    {
                        model.Sgscore = int.Parse(dt.Rows[n]["Sgscore"].ToString());
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        public static List<LearnSite.Model.Ip> MapIpList(DataTable dt)
        {
            List<LearnSite.Model.Ip> modelList = new List<LearnSite.Model.Ip>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LearnSite.Model.Ip model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LearnSite.Model.Ip();
                    if (dt.Rows[n]["Iid"].ToString() != "")
                    {
                        model.Iid = int.Parse(dt.Rows[n]["Iid"].ToString());
                    }
                    if (dt.Rows[n]["Ihid"].ToString() != "")
                    {
                        model.Ihid = int.Parse(dt.Rows[n]["Ihid"].ToString());
                    }
                    if (dt.Rows[n]["Inum"].ToString() != "")
                    {
                        model.Inum = int.Parse(dt.Rows[n]["Inum"].ToString());
                    }
                    model.Iip = dt.Rows[n]["Iip"].ToString();
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        public static List<LearnSite.Model.House> MapHouseList(DataTable dt)
        {
            List<LearnSite.Model.House> modelList = new List<LearnSite.Model.House>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LearnSite.Model.House model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LearnSite.Model.House();
                    if (dt.Rows[n]["Hid"].ToString() != "")
                    {
                        model.Hid = int.Parse(dt.Rows[n]["Hid"].ToString());
                    }
                    model.Hname = dt.Rows[n]["Hname"].ToString();
                    model.Hseat = dt.Rows[n]["Hseat"].ToString();
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        public static List<LearnSite.Model.Consoles> MapConsolesList(DataTable dt)
        {
            List<LearnSite.Model.Consoles> modelList = new List<LearnSite.Model.Consoles>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LearnSite.Model.Consoles model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LearnSite.Model.Consoles();
                    if (dt.Rows[n]["Nid"] != null && dt.Rows[n]["Nid"].ToString() != "")
                    {
                        model.Nid = int.Parse(dt.Rows[n]["Nid"].ToString());
                    }
                    if (dt.Rows[n]["Nhid"] != null && dt.Rows[n]["Nhid"].ToString() != "")
                    {
                        model.Nhid = int.Parse(dt.Rows[n]["Nhid"].ToString());
                    }
                    if (dt.Rows[n]["Ncid"] != null && dt.Rows[n]["Ncid"].ToString() != "")
                    {
                        model.Ncid = int.Parse(dt.Rows[n]["Ncid"].ToString());
                    }
                    if (dt.Rows[n]["Ntitle"] != null && dt.Rows[n]["Ntitle"].ToString() != "")
                    {
                        model.Ntitle = dt.Rows[n]["Ntitle"].ToString();
                    }
                    if (dt.Rows[n]["Ncontent"] != null && dt.Rows[n]["Ncontent"].ToString() != "")
                    {
                        model.Ncontent = dt.Rows[n]["Ncontent"].ToString();
                    }
                    if (dt.Rows[n]["Npublish"] != null && dt.Rows[n]["Npublish"].ToString() != "")
                    {
                        if ((dt.Rows[n]["Npublish"].ToString() == "1") || (dt.Rows[n]["Npublish"].ToString().ToLower() == "true"))
                        {
                            model.Npublish = true;
                        }
                        else
                        {
                            model.Npublish = false;
                        }
                    }
                    if (dt.Rows[n]["Ndate"] != null && dt.Rows[n]["Ndate"].ToString() != "")
                    {
                        model.Ndate = DateTime.Parse(dt.Rows[n]["Ndate"].ToString());
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        public static List<LearnSite.Model.Autonomic> MapAutonomicList(DataTable dt)
        {
            List<LearnSite.Model.Autonomic> modelList = new List<LearnSite.Model.Autonomic>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LearnSite.Model.Autonomic model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LearnSite.Model.Autonomic();
                    if (dt.Rows[n]["Aid"] != null && dt.Rows[n]["Aid"].ToString() != "")
                    {
                        model.Aid = int.Parse(dt.Rows[n]["Aid"].ToString());
                    }
                    if (dt.Rows[n]["Asid"] != null && dt.Rows[n]["Asid"].ToString() != "")
                    {
                        model.Asid = int.Parse(dt.Rows[n]["Asid"].ToString());
                    }
                    if (dt.Rows[n]["Anum"] != null)
                    {
                        model.Anum = dt.Rows[n]["Anum"].ToString();
                    }
                    if (dt.Rows[n]["Aname"] != null)
                    {
                        model.Aname = dt.Rows[n]["Aname"].ToString();
                    }
                    if (dt.Rows[n]["Ayid"] != null && dt.Rows[n]["Ayid"].ToString() != "")
                    {
                        model.Ayid = int.Parse(dt.Rows[n]["Ayid"].ToString());
                    }
                    if (dt.Rows[n]["Afid"] != null && dt.Rows[n]["Afid"].ToString() != "")
                    {
                        model.Afid = int.Parse(dt.Rows[n]["Afid"].ToString());
                    }
                    if (dt.Rows[n]["Atype"] != null)
                    {
                        model.Atype = dt.Rows[n]["Atype"].ToString();
                    }
                    if (dt.Rows[n]["Afilename"] != null)
                    {
                        model.Afilename = dt.Rows[n]["Afilename"].ToString();
                    }
                    if (dt.Rows[n]["Aurl"] != null)
                    {
                        model.Aurl = dt.Rows[n]["Aurl"].ToString();
                    }
                    if (dt.Rows[n]["Alength"] != null && dt.Rows[n]["Alength"].ToString() != "")
                    {
                        model.Alength = int.Parse(dt.Rows[n]["Alength"].ToString());
                    }
                    if (dt.Rows[n]["Ascore"] != null && dt.Rows[n]["Ascore"].ToString() != "")
                    {
                        model.Ascore = int.Parse(dt.Rows[n]["Ascore"].ToString());
                    }
                    if (dt.Rows[n]["Adate"] != null && dt.Rows[n]["Adate"].ToString() != "")
                    {
                        model.Adate = DateTime.Parse(dt.Rows[n]["Adate"].ToString());
                    }
                    if (dt.Rows[n]["Aip"] != null)
                    {
                        model.Aip = dt.Rows[n]["Aip"].ToString();
                    }
                    if (dt.Rows[n]["Avote"] != null && dt.Rows[n]["Avote"].ToString() != "")
                    {
                        model.Avote = int.Parse(dt.Rows[n]["Avote"].ToString());
                    }
                    if (dt.Rows[n]["Aegg"] != null && dt.Rows[n]["Aegg"].ToString() != "")
                    {
                        model.Aegg = int.Parse(dt.Rows[n]["Aegg"].ToString());
                    }
                    if (dt.Rows[n]["Acheck"] != null && dt.Rows[n]["Acheck"].ToString() != "")
                    {
                        if ((dt.Rows[n]["Acheck"].ToString() == "1") || (dt.Rows[n]["Acheck"].ToString().ToLower() == "true"))
                        {
                            model.Acheck = true;
                        }
                        else
                        {
                            model.Acheck = false;
                        }
                    }
                    if (dt.Rows[n]["Aself"] != null)
                    {
                        model.Aself = dt.Rows[n]["Aself"].ToString();
                    }
                    if (dt.Rows[n]["Agood"] != null && dt.Rows[n]["Agood"].ToString() != "")
                    {
                        if ((dt.Rows[n]["Agood"].ToString() == "1") || (dt.Rows[n]["Agood"].ToString().ToLower() == "true"))
                        {
                            model.Agood = true;
                        }
                        else
                        {
                            model.Agood = false;
                        }
                    }
                    if (dt.Rows[n]["Ayear"] != null && dt.Rows[n]["Ayear"].ToString() != "")
                    {
                        model.Ayear = int.Parse(dt.Rows[n]["Ayear"].ToString());
                    }
                    if (dt.Rows[n]["Agrade"] != null && dt.Rows[n]["Agrade"].ToString() != "")
                    {
                        model.Agrade = int.Parse(dt.Rows[n]["Agrade"].ToString());
                    }
                    if (dt.Rows[n]["Aclass"] != null && dt.Rows[n]["Aclass"].ToString() != "")
                    {
                        model.Aclass = int.Parse(dt.Rows[n]["Aclass"].ToString());
                    }
                    if (dt.Rows[n]["Aterm"] != null && dt.Rows[n]["Aterm"].ToString() != "")
                    {
                        model.Aterm = int.Parse(dt.Rows[n]["Aterm"].ToString());
                    }
                    if (dt.Rows[n]["Ahit"] != null && dt.Rows[n]["Ahit"].ToString() != "")
                    {
                        model.Ahit = int.Parse(dt.Rows[n]["Ahit"].ToString());
                    }
                    if (dt.Rows[n]["Aoffice"] != null && dt.Rows[n]["Aoffice"].ToString() != "")
                    {
                        if ((dt.Rows[n]["Aoffice"].ToString() == "1") || (dt.Rows[n]["Aoffice"].ToString().ToLower() == "true"))
                        {
                            model.Aoffice = true;
                        }
                        else
                        {
                            model.Aoffice = false;
                        }
                    }
                    if (dt.Rows[n]["Aflash"] != null && dt.Rows[n]["Aflash"].ToString() != "")
                    {
                        if ((dt.Rows[n]["Aflash"].ToString() == "1") || (dt.Rows[n]["Aflash"].ToString().ToLower() == "true"))
                        {
                            model.Aflash = true;
                        }
                        else
                        {
                            model.Aflash = false;
                        }
                    }
                    if (dt.Rows[n]["Aerror"] != null && dt.Rows[n]["Aerror"].ToString() != "")
                    {
                        if ((dt.Rows[n]["Aerror"].ToString() == "1") || (dt.Rows[n]["Aerror"].ToString().ToLower() == "true"))
                        {
                            model.Aerror = true;
                        }
                        else
                        {
                            model.Aerror = false;
                        }
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        public static List<LearnSite.Model.GroupWork> MapGroupWorkList(DataTable dt)
        {
            List<LearnSite.Model.GroupWork> modelList = new List<LearnSite.Model.GroupWork>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LearnSite.Model.GroupWork model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LearnSite.Model.GroupWork();
                    if (dt.Rows[n]["Gid"].ToString() != "")
                    {
                        model.Gid = int.Parse(dt.Rows[n]["Gid"].ToString());
                    }
                    model.Gnum = dt.Rows[n]["Gnum"].ToString();
                    model.Gstudents = dt.Rows[n]["Gstudents"].ToString();
                    if (dt.Rows[n]["Gterm"].ToString() != "")
                    {
                        model.Gterm = int.Parse(dt.Rows[n]["Gterm"].ToString());
                    }
                    if (dt.Rows[n]["Ggrade"].ToString() != "")
                    {
                        model.Ggrade = int.Parse(dt.Rows[n]["Ggrade"].ToString());
                    }
                    if (dt.Rows[n]["Gclass"].ToString() != "")
                    {
                        model.Gclass = int.Parse(dt.Rows[n]["Gclass"].ToString());
                    }
                    if (dt.Rows[n]["Gcid"].ToString() != "")
                    {
                        model.Gcid = int.Parse(dt.Rows[n]["Gcid"].ToString());
                    }
                    if (dt.Rows[n]["Gmid"].ToString() != "")
                    {
                        model.Gmid = int.Parse(dt.Rows[n]["Gmid"].ToString());
                    }
                    model.Gfilename = dt.Rows[n]["Gfilename"].ToString();
                    model.Gtype = dt.Rows[n]["Gtype"].ToString();
                    model.Gurl = dt.Rows[n]["Gurl"].ToString();
                    if (dt.Rows[n]["Glengh"].ToString() != "")
                    {
                        model.Glengh = int.Parse(dt.Rows[n]["Glengh"].ToString());
                    }
                    if (dt.Rows[n]["Gscore"].ToString() != "")
                    {
                        model.Gscore = int.Parse(dt.Rows[n]["Gscore"].ToString());
                    }
                    if (dt.Rows[n]["Gtime"].ToString() != "")
                    {
                        model.Gtime = int.Parse(dt.Rows[n]["Gtime"].ToString());
                    }
                    if (dt.Rows[n]["Gvote"].ToString() != "")
                    {
                        model.Gvote = int.Parse(dt.Rows[n]["Gvote"].ToString());
                    }
                    if (dt.Rows[n]["Gcheck"].ToString() != "")
                    {
                        if ((dt.Rows[n]["Gcheck"].ToString() == "1") || (dt.Rows[n]["Gcheck"].ToString().ToLower() == "true"))
                        {
                            model.Gcheck = true;
                        }
                        else
                        {
                            model.Gcheck = false;
                        }
                    }
                    model.Gnote = dt.Rows[n]["Gnote"].ToString();
                    if (dt.Rows[n]["Grank"].ToString() != "")
                    {
                        model.Grank = int.Parse(dt.Rows[n]["Grank"].ToString());
                    }
                    if (dt.Rows[n]["Ghit"].ToString() != "")
                    {
                        model.Ghit = int.Parse(dt.Rows[n]["Ghit"].ToString());
                    }
                    model.Gip = dt.Rows[n]["Gip"].ToString();
                    if (dt.Rows[n]["Gdate"].ToString() != "")
                    {
                        model.Gdate = DateTime.Parse(dt.Rows[n]["Gdate"].ToString());
                    }
                    if (dt.Rows[n]["Ggroup"].ToString() != "")
                    {
                        model.Ggroup = int.Parse(dt.Rows[n]["Ggroup"].ToString());
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        public static List<LearnSite.Model.JudgeArg> MapJudgeArgList(DataTable dt)
        {
            List<LearnSite.Model.JudgeArg> modelList = new List<LearnSite.Model.JudgeArg>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LearnSite.Model.JudgeArg model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LearnSite.Model.JudgeArg();
                    if (dt.Rows[n]["Jid"] != null && dt.Rows[n]["Jid"].ToString() != "")
                    {
                        model.Jid = int.Parse(dt.Rows[n]["Jid"].ToString());
                    }
                    if (dt.Rows[n]["Jhid"] != null && dt.Rows[n]["Jhid"].ToString() != "")
                    {
                        model.Jhid = int.Parse(dt.Rows[n]["Jhid"].ToString());
                    }
                    if (dt.Rows[n]["Jmid"] != null && dt.Rows[n]["Jmid"].ToString() != "")
                    {
                        model.Jmid = int.Parse(dt.Rows[n]["Jmid"].ToString());
                    }
                    if (dt.Rows[n]["Jsleep"] != null && dt.Rows[n]["Jsleep"].ToString() != "")
                    {
                        model.Jsleep = int.Parse(dt.Rows[n]["Jsleep"].ToString());
                    }
                    if (dt.Rows[n]["Jinone"] != null)
                    {
                        model.Jinone = dt.Rows[n]["Jinone"].ToString();
                    }
                    if (dt.Rows[n]["Jintwo"] != null)
                    {
                        model.Jintwo = dt.Rows[n]["Jintwo"].ToString();
                    }
                    if (dt.Rows[n]["Jinthree"] != null)
                    {
                        model.Jinthree = dt.Rows[n]["Jinthree"].ToString();
                    }
                    if (dt.Rows[n]["Joutone"] != null)
                    {
                        model.Joutone = dt.Rows[n]["Joutone"].ToString();
                    }
                    if (dt.Rows[n]["Joutwo"] != null)
                    {
                        model.Joutwo = dt.Rows[n]["Joutwo"].ToString();
                    }
                    if (dt.Rows[n]["Jouthree"] != null)
                    {
                        model.Jouthree = dt.Rows[n]["Jouthree"].ToString();
                    }
                    if (dt.Rows[n]["Jright"] != null && dt.Rows[n]["Jright"].ToString() != "")
                    {
                        if ((dt.Rows[n]["Jright"].ToString() == "1") || (dt.Rows[n]["Jright"].ToString().ToLower() == "true"))
                        {
                            model.Jright = true;
                        }
                        else
                        {
                            model.Jright = false;
                        }
                    }
                    if (dt.Rows[n]["Jcode"] != null)
                    {
                        model.Jcode = dt.Rows[n]["Jcode"].ToString();
                    }
                    if (dt.Rows[n]["Jcid"] != null && dt.Rows[n]["Jcid"].ToString() != "")
                    {
                        model.Jcid = int.Parse(dt.Rows[n]["Jcid"].ToString());
                    }
                    if (dt.Rows[n]["Jimg"] != null)
                    {
                        model.Jimg = dt.Rows[n]["Jimg"].ToString();
                    }
                    if (dt.Columns.Contains("Jthumb"))
                    {
                        if (dt.Rows[n]["Jthumb"] != null)
                        {
                            model.Jthumb = dt.Rows[n]["Jthumb"].ToString();
                        }
                    }
                    else
                    {
                        model.Jthumb = "";
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        public static List<LearnSite.Model.Computers> MapComputersList(DataTable dt)
        {
            List<LearnSite.Model.Computers> modelList = new List<LearnSite.Model.Computers>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LearnSite.Model.Computers model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LearnSite.Model.Computers();
                    if (dt.Rows[n]["Pid"].ToString() != "")
                    {
                        model.Pid = int.Parse(dt.Rows[n]["Pid"].ToString());
                    }
                    model.Pip = dt.Rows[n]["Pip"].ToString();
                    model.Pmachine = dt.Rows[n]["Pmachine"].ToString();
                    if (dt.Rows[n]["Plock"].ToString() != "")
                    {
                        if ((dt.Rows[n]["Plock"].ToString() == "1") || (dt.Rows[n]["Plock"].ToString().ToLower() == "true"))
                        {
                            model.Plock = true;
                        }
                        else
                        {
                            model.Plock = false;
                        }
                    }
                    if (dt.Rows[n]["Pdate"].ToString() != "")
                    {
                        model.Pdate = DateTime.Parse(dt.Rows[n]["Pdate"].ToString());
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        public static List<LearnSite.Model.Problems> MapProblemsList(DataTable dt)
        {
            List<LearnSite.Model.Problems> modelList = new List<LearnSite.Model.Problems>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LearnSite.Model.Problems model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LearnSite.Model.Problems();
                    if (dt.Rows[n]["Pid"] != null && dt.Rows[n]["Pid"].ToString() != "")
                    {
                        model.Pid = int.Parse(dt.Rows[n]["Pid"].ToString());
                    }
                    if (dt.Rows[n]["Phid"] != null && dt.Rows[n]["Phid"].ToString() != "")
                    {
                        model.Phid = int.Parse(dt.Rows[n]["Phid"].ToString());
                    }
                    if (dt.Rows[n]["Pnid"] != null && dt.Rows[n]["Pnid"].ToString() != "")
                    {
                        model.Pnid = int.Parse(dt.Rows[n]["Pnid"].ToString());
                    }
                    if (dt.Rows[n]["Ptitle"] != null && dt.Rows[n]["Ptitle"].ToString() != "")
                    {
                        model.Ptitle = dt.Rows[n]["Ptitle"].ToString();
                    }
                    if (dt.Rows[n]["Pcode"] != null && dt.Rows[n]["Pcode"].ToString() != "")
                    {
                        model.Pcode = dt.Rows[n]["Pcode"].ToString();
                    }
                    if (dt.Rows[n]["Pouput"] != null && dt.Rows[n]["Pouput"].ToString() != "")
                    {
                        model.Pouput = dt.Rows[n]["Pouput"].ToString();
                    }
                    if (dt.Rows[n]["Pscore"] != null && dt.Rows[n]["Pscore"].ToString() != "")
                    {
                        model.Pscore = int.Parse(dt.Rows[n]["Pscore"].ToString());
                    }
                    if (dt.Rows[n]["Pdate"] != null && dt.Rows[n]["Pdate"].ToString() != "")
                    {
                        model.Pdate = DateTime.Parse(dt.Rows[n]["Pdate"].ToString());
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        public static List<LearnSite.Model.Flection> MapFlectionList(DataTable dt)
        {
            List<LearnSite.Model.Flection> modelList = new List<LearnSite.Model.Flection>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LearnSite.Model.Flection model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LearnSite.Model.Flection();
                    if (dt.Rows[n]["Fid"].ToString() != "")
                    {
                        model.Fid = int.Parse(dt.Rows[n]["Fid"].ToString());
                    }
                    if (dt.Rows[n]["Fcid"].ToString() != "")
                    {
                        model.Fcid = int.Parse(dt.Rows[n]["Fcid"].ToString());
                    }
                    if (dt.Rows[n]["Fhid"].ToString() != "")
                    {
                        model.Fhid = int.Parse(dt.Rows[n]["Fhid"].ToString());
                    }
                    model.Fcontent = dt.Rows[n]["Fcontent"].ToString();
                    if (dt.Rows[n]["Fdate"].ToString() != "")
                    {
                        model.Fdate = DateTime.Parse(dt.Rows[n]["Fdate"].ToString());
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        public static List<LearnSite.Model.Pfinger> MapPfingerList(DataTable dt)
        {
            List<LearnSite.Model.Pfinger> modelList = new List<LearnSite.Model.Pfinger>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LearnSite.Model.Pfinger model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LearnSite.Model.Pfinger();
                    if (dt.Rows[n]["Pid"].ToString() != "")
                    {
                        model.Pid = int.Parse(dt.Rows[n]["Pid"].ToString());
                    }
                    model.Psnum = dt.Rows[n]["Psnum"].ToString();
                    if (dt.Rows[n]["Pspd"].ToString() != "")
                    {
                        model.Pspd = decimal.Parse(dt.Rows[n]["Pspd"].ToString());
                    }
                    if (dt.Rows[n]["Pyear"].ToString() != "")
                    {
                        model.Pyear = int.Parse(dt.Rows[n]["Pyear"].ToString());
                    }
                    if (dt.Rows[n]["Pmonth"].ToString() != "")
                    {
                        model.Pmonth = int.Parse(dt.Rows[n]["Pmonth"].ToString());
                    }
                    if (dt.Rows[n]["Pdate"].ToString() != "")
                    {
                        model.Pdate = DateTime.Parse(dt.Rows[n]["Pdate"].ToString());
                    }
                    if (dt.Rows[n]["Pdegree"].ToString() != "")
                    {
                        model.Pdegree = int.Parse(dt.Rows[n]["Pdegree"].ToString());
                    }
                    if (dt.Rows[n]["Pgrade"].ToString() != "")
                    {
                        model.Pgrade = int.Parse(dt.Rows[n]["Pgrade"].ToString());
                    }
                    if (dt.Rows[n]["Pterm"].ToString() != "")
                    {
                        model.Pterm = int.Parse(dt.Rows[n]["Pterm"].ToString());
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        public static List<LearnSite.Model.DelStudents> MapDelStudentsList(DataTable dt)
        {
            List<LearnSite.Model.DelStudents> modelList = new List<LearnSite.Model.DelStudents>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LearnSite.Model.DelStudents model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LearnSite.Model.DelStudents();
                    if (dt.Rows[n]["Did"] != null && dt.Rows[n]["Did"].ToString() != "")
                    {
                        model.Did = int.Parse(dt.Rows[n]["Did"].ToString());
                    }
                    if (dt.Rows[n]["Dnum"] != null && dt.Rows[n]["Dnum"].ToString() != "")
                    {
                        model.Dnum = dt.Rows[n]["Dnum"].ToString();
                    }
                    if (dt.Rows[n]["Dyear"] != null && dt.Rows[n]["Dyear"].ToString() != "")
                    {
                        model.Dyear = int.Parse(dt.Rows[n]["Dyear"].ToString());
                    }
                    if (dt.Rows[n]["Dgrade"] != null && dt.Rows[n]["Dgrade"].ToString() != "")
                    {
                        model.Dgrade = int.Parse(dt.Rows[n]["Dgrade"].ToString());
                    }
                    if (dt.Rows[n]["Dclass"] != null && dt.Rows[n]["Dclass"].ToString() != "")
                    {
                        model.Dclass = int.Parse(dt.Rows[n]["Dclass"].ToString());
                    }
                    if (dt.Rows[n]["Dname"] != null && dt.Rows[n]["Dname"].ToString() != "")
                    {
                        model.Dname = dt.Rows[n]["Dname"].ToString();
                    }
                    if (dt.Rows[n]["Dsex"] != null && dt.Rows[n]["Dsex"].ToString() != "")
                    {
                        model.Dsex = dt.Rows[n]["Dsex"].ToString();
                    }
                    if (dt.Rows[n]["Daddress"] != null && dt.Rows[n]["Daddress"].ToString() != "")
                    {
                        model.Daddress = dt.Rows[n]["Daddress"].ToString();
                    }
                    if (dt.Rows[n]["Dphone"] != null && dt.Rows[n]["Dphone"].ToString() != "")
                    {
                        model.Dphone = dt.Rows[n]["Dphone"].ToString();
                    }
                    if (dt.Rows[n]["Dparents"] != null && dt.Rows[n]["Dparents"].ToString() != "")
                    {
                        model.Dparents = dt.Rows[n]["Dparents"].ToString();
                    }
                    if (dt.Rows[n]["Dheadtheacher"] != null && dt.Rows[n]["Dheadtheacher"].ToString() != "")
                    {
                        model.Dheadtheacher = dt.Rows[n]["Dheadtheacher"].ToString();
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        public static List<LearnSite.Model.NotSign> MapNotSignList(DataTable dt)
        {
            List<LearnSite.Model.NotSign> modelList = new List<LearnSite.Model.NotSign>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LearnSite.Model.NotSign model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LearnSite.Model.NotSign();
                    if (dt.Rows[n]["Nid"].ToString() != "")
                    {
                        model.Nid = int.Parse(dt.Rows[n]["Nid"].ToString());
                    }
                    model.Nnum = dt.Rows[n]["Nnum"].ToString();
                    if (dt.Rows[n]["Ndate"].ToString() != "")
                    {
                        model.Ndate = DateTime.Parse(dt.Rows[n]["Ndate"].ToString());
                    }
                    if (dt.Rows[n]["Nyear"].ToString() != "")
                    {
                        model.Nyear = int.Parse(dt.Rows[n]["Nyear"].ToString());
                    }
                    if (dt.Rows[n]["Nmonth"].ToString() != "")
                    {
                        model.Nmonth = int.Parse(dt.Rows[n]["Nmonth"].ToString());
                    }
                    if (dt.Rows[n]["Nday"].ToString() != "")
                    {
                        model.Nday = int.Parse(dt.Rows[n]["Nday"].ToString());
                    }
                    model.Nweek = dt.Rows[n]["Nweek"].ToString();
                    model.Nnote = dt.Rows[n]["Nnote"].ToString();
                    if (dt.Rows[n]["Ngrade"].ToString() != "")
                    {
                        model.Ngrade = int.Parse(dt.Rows[n]["Ngrade"].ToString());
                    }
                    if (dt.Rows[n]["Nterm"].ToString() != "")
                    {
                        model.Nterm = int.Parse(dt.Rows[n]["Nterm"].ToString());
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        public static List<LearnSite.Model.Ptyper> MapPtyperList(DataTable dt)
        {
            List<LearnSite.Model.Ptyper> modelList = new List<LearnSite.Model.Ptyper>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LearnSite.Model.Ptyper model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LearnSite.Model.Ptyper();
                    if (dt.Rows[n]["Pid"].ToString() != "")
                    {
                        model.Pid = int.Parse(dt.Rows[n]["Pid"].ToString());
                    }
                    if (dt.Rows[n]["Ptid"].ToString() != "")
                    {
                        model.Ptid = int.Parse(dt.Rows[n]["Ptid"].ToString());
                    }
                    model.Psnum = dt.Rows[n]["Psnum"].ToString();
                    if (dt.Rows[n]["Pscore"].ToString() != "")
                    {
                        model.Pscore = int.Parse(dt.Rows[n]["Pscore"].ToString());
                    }
                    if (dt.Rows[n]["Pdate"].ToString() != "")
                    {
                        model.Pdate = DateTime.Parse(dt.Rows[n]["Pdate"].ToString());
                    }
                    model.Pip = dt.Rows[n]["Pip"].ToString();
                    if (dt.Rows[n]["Ptype"].ToString() != "")
                    {
                        model.Ptype = int.Parse(dt.Rows[n]["Ptype"].ToString());
                    }
                    if (dt.Rows[n]["Pdegree"].ToString() != "")
                    {
                        model.Ptype = int.Parse(dt.Rows[n]["Pdegree"].ToString());
                    }
                    if (dt.Rows[n]["Pgrade"].ToString() != "")
                    {
                        model.Pgrade = int.Parse(dt.Rows[n]["Pgrade"].ToString());
                    }
                    if (dt.Rows[n]["Pterm"].ToString() != "")
                    {
                        model.Pterm = int.Parse(dt.Rows[n]["Pterm"].ToString());
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }
    }
}
