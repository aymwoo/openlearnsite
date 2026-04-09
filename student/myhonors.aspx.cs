using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using LearnSite.Bll;
using LearnSite.Model;

namespace LearnSite.Student
{
    public partial class myhonors : System.Web.UI.Page
    {
        private HonorService honorService = new HonorService();
        private string snum = "";
        private LearnSite.BLL.Students studentBll = new LearnSite.BLL.Students();

    protected void Page_Load(object sender, EventArgs e)
        {
            // 检测学生登录状态
            if (!LearnSite.Common.CookieHelp.IsStudentLogin())
            {
                LearnSite.Common.CookieHelp.JudgeStudentCookies();
                return;
            }

            // 从 Cookie 获取当前登录学生信息
            LearnSite.Model.Cook cook = new LearnSite.Model.Cook();
            snum = cook.Snum;

            if (string.IsNullOrEmpty(snum))
            {
                LearnSite.Common.CookieHelp.JudgeStudentCookies();
                return;
            }

            if (!IsPostBack)
            {
                // 自动更新当前学生荣誉数据
                honorService.EvaluateStudentHonors(snum, "TERM1");
                BindStudentInfo();
                BindHonorStats();
                BindMyHonors();
                BindLockedHonors();
                BindTimeline();
            }
        }

        #region 绑定学生信息

        /// <summary>
        /// 绑定学生基本信息
        /// </summary>
        private void BindStudentInfo()
        {
            // 从数据库获取学生信息
            var student = GetStudentInfo(snum);

            if (student != null)
            {
                LblAvatar.Text = student.Sname.Substring(0, 1);
                LblName.Text = student.Sname;
                LblClass.Text = student.Sgrade + "年级" + student.Sclass + "班";
                LblSnum.Text = student.Snum;
                LblTotalScore.Text = student.Sallscore.ToString("F1");
            }
        }

        #endregion

        #region 绑定荣誉统计

        /// <summary>
        /// 绑定荣誉统计
        /// </summary>
        private void BindHonorStats()
        {
            StudentHonorStat stat = honorService.GetStudentHonorStat(snum);

            LblHonorCount.Text = stat.TotalHonors.ToString();
            LblGoldCount.Text = stat.GoldCount.ToString();
            LblSilverCount.Text = stat.SilverCount.ToString();
            LblBronzeCount.Text = stat.BronzeCount.ToString();

            StatTotal.Text = stat.TotalHonors.ToString();
            StatGold.Text = stat.GoldCount.ToString();
            StatSilver.Text = stat.SilverCount.ToString();
            StatBronze.Text = stat.BronzeCount.ToString();
        }

        #endregion

        #region 绑定我的荣誉

        /// <summary>
        /// 绑定已获得的荣誉
        /// </summary>
        private void BindMyHonors()
        {
            List<StudentHonor> honors = honorService.GetStudentHonors(snum);

            if (honors.Count == 0)
            {
                PanelMyHonors.Visible = false;
                PanelEmptyHonors.Visible = true;
                return;
            }

            PanelMyHonors.Visible = true;
            PanelEmptyHonors.Visible = false;
            PanelMyHonors.Controls.Clear();

            List<HonorConfig> allConfigs = honorService.GetAllHonorConfigs();

            foreach (var honor in honors)
            {
                var config = allConfigs.FirstOrDefault(c => c.HonorCode == honor.HonorCode);
                if (config != null)
                {
                    PanelMyHonors.Controls.Add(CreateHonorItem(honor, config));
                }
            }
        }

        /// <summary>
        /// 创建荣誉项
        /// </summary>
        private Panel CreateHonorItem(StudentHonor honor, HonorConfig config)
        {
            Panel item = new Panel();
            item.CssClass = "honor-item " + GetLevelClass(honor.HonorLevel);

            // 图标
            Label icon = new Label
            {
                Text = config.IconEmoji ?? "🏆",
                CssClass = "honor-emoji"
            };

            // 名称
            Label name = new Label
            {
                Text = config.HonorName,
                CssClass = "honor-name"
            };

            // 等级
            Label level = new Label
            {
                Text = honor.LevelEmoji + " " + honor.LevelName,
                CssClass = "honor-level"
            };

            // 次数
            Label count = new Label
            {
                Text = "已获得 " + honor.EarnCount.ToString() + " 次",
                CssClass = "honor-count"
            };

            item.Controls.Add(icon);
            item.Controls.Add(new LiteralControl("<br />"));
            item.Controls.Add(name);
            item.Controls.Add(new LiteralControl("<br />"));
            item.Controls.Add(level);
            item.Controls.Add(new LiteralControl("<br />"));
            item.Controls.Add(count);

            return item;
        }

        #endregion

        #region 绑定待解锁荣誉

        /// <summary>
        /// 绑定待解锁荣誉
        /// </summary>
        private void BindLockedHonors()
        {
            List<StudentHonor> myHonors = honorService.GetStudentHonors(snum);
            List<HonorConfig> allConfigs = honorService.GetAllHonorConfigs();

            // 获取已获得的荣誉代码
            HashSet<string> earnedHonorCodes = new HashSet<string>(myHonors.Select(h => h.HonorCode));

            // 筛选未获得的荣誉
            List<HonorConfig> lockedConfigs = allConfigs.Where(c => !earnedHonorCodes.Contains(c.HonorCode)).Take(8).ToList();

            PanelLockedHonors.Controls.Clear();

            foreach (var config in lockedConfigs)
            {
                PanelLockedHonors.Controls.Add(CreateLockedHonorItem(config));
            }
        }

        /// <summary>
        /// 创建待解锁荣誉项
        /// </summary>
        private Panel CreateLockedHonorItem(HonorConfig config)
        {
            Panel item = new Panel { CssClass = "honor-item locked-honor" };

            // 图标
            Label icon = new Label
            {
                Text = config.IconEmoji ?? "🏆",
                CssClass = "honor-emoji"
            };

            // 名称
            Label name = new Label
            {
                Text = config.HonorName,
                CssClass = "honor-name"
            };

            // 描述
            Label desc = new Label
            {
                Text = config.Description ?? "继续努力解锁",
                CssClass = "honor-level"
            };

            item.Controls.Add(icon);
            item.Controls.Add(new LiteralControl("<br />"));
            item.Controls.Add(name);
            item.Controls.Add(new LiteralControl("<br />"));
            item.Controls.Add(desc);

            return item;
        }

        #endregion

        #region 绑定时间线

        /// <summary>
        /// 绑定荣誉获得时间线
        /// </summary>
        private void BindTimeline()
        {
            List<StudentHonor> honors = honorService.GetStudentHonors(snum)
                .OrderByDescending(h => h.EarnDate)
                .Take(10)
                .ToList();

            PanelTimeline.Controls.Clear();

            List<HonorConfig> allConfigs = honorService.GetAllHonorConfigs();

            foreach (var honor in honors)
            {
                var config = allConfigs.FirstOrDefault(c => c.HonorCode == honor.HonorCode);
                if (config != null)
                {
                    PanelTimeline.Controls.Add(CreateTimelineItem(honor, config));
                }
            }
        }

        /// <summary>
        /// 创建时间线项
        /// </summary>
        private Panel CreateTimelineItem(StudentHonor honor, HonorConfig config)
        {
            Panel item = new Panel { CssClass = "timeline-item" };

            // 日期
            Label date = new Label
            {
                Text = honor.EarnDate.ToString("yyyy-MM-dd HH:mm"),
                CssClass = "timeline-date"
            };

            // 内容
            Panel content = new Panel { CssClass = "timeline-content" };

            Label honorName = new Label
            {
                Text = honor.LevelEmoji + " " + config.HonorName,
                CssClass = "timeline-honor-name"
            };

            Label honorLevel = new Label
            {
                Text = honor.LevelName + " - 第" + honor.EarnCount + "次获得",
                CssClass = "timeline-honor-level"
            };

            content.Controls.Add(honorName);
            content.Controls.Add(new LiteralControl("<br />"));
            content.Controls.Add(honorLevel);

            item.Controls.Add(date);
            item.Controls.Add(content);

            return item;
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 获取等级CSS类
        /// </summary>
        private string GetLevelClass(int level)
        {
            switch (level)
            {
                case 1: return "bronze";
                case 2: return "silver";
                case 3: return "gold";
                default: return "bronze";
            }
        }

        /// <summary>
        /// 获取学生信息
        /// </summary>
        private StudentInfo GetStudentInfo(string snum)
        {
            LearnSite.Model.Students student = studentBll.SnumGetModel(snum);
            if (student != null)
            {
                return new StudentInfo
                {
                    Snum = student.Snum,
                    Sname = student.Sname,
                    Sgrade = student.Sgrade.ToString(),
                    Sclass = student.Sclass.ToString(),
                    Sallscore = Convert.ToDecimal(student.Sscore + student.Sattitude)
                };
            }
            return null;
        }

        #endregion

        #region 辅助类

        private class StudentInfo
        {
            public string Snum { get; set; }
            public string Sname { get; set; }
            public string Sgrade { get; set; }
            public string Sclass { get; set; }
            public decimal Sallscore { get; set; }
        }

        #endregion
    }
}
