using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using LearnSite.Bll;
using LearnSite.Model;

namespace LearnSite.Student
{
    public partial class honorboard : System.Web.UI.Page
    {
        private HonorService honorService = new HonorService();
        private string currentHonorType = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // 设置默认显示范围（根据教师设置或学生年级）
                SetDefaultDisplayScope();

                // 绑定荣誉榜（不自动评定荣誉，直接显示已有数据）
                BindHonorBoard();
            }
            else
            {
                // PostBack 时绑定荣誉榜
                BindHonorBoard();
            }
        }

        /// <summary>
        /// 设置默认显示范围（根据教师设置或学生年级）
        /// </summary>
        private void SetDefaultDisplayScope()
        {
            // 从数据库读取教师设置的显示范围
            HonorBoardSetting setting = GetHonorBoardSetting();

            if (setting == null)
            {
                // 如果没有设置，使用默认值(显示当前学生所在年级)
                SetDefaultStudentScope();
            }
            else
            {
                // 根据教师设置调整显示范围
                AddScopeLabelBySetting(setting);
            }
        }

        /// <summary>
        /// 设置学生默认显示范围(当没有教师设置时)
        /// </summary>
        private void SetDefaultStudentScope()
        {
            // 检查学生登录状态并获取年级
            if (LearnSite.Common.CookieHelp.IsStudentLogin())
            {
                LearnSite.Model.Cook cook = new LearnSite.Model.Cook();
                string snum = cook.Snum;

                if (!string.IsNullOrEmpty(snum))
                {
                    // 获取学生信息
                    LearnSite.BLL.Students studentBll = new LearnSite.BLL.Students();
                    LearnSite.Model.Students student = studentBll.SnumGetModel(snum);

                    if (student != null && student.Sgrade.HasValue)
                    {
                        string grade = student.Sgrade.Value.ToString();
                        // 在页面上添加当前年级提示
                        AddCurrentGradeLabel(grade);
                    }
                }
            }
        }

        /// <summary>
        /// 根据教师设置添加显示范围标签
        /// </summary>
        private void AddScopeLabelBySetting(HonorBoardSetting setting)
        {
            // 获取当前登录学生的信息
            string studentGrade = GetStudentGrade();
            string studentClass = GetStudentClass();

            if (setting.Scope == "school")
            {
                // 全校范围
                AddScopeLabel("全校");
            }
            else if (setting.Scope == "grade")
            {
                // 年级模式：显示学生所在年级
                if (!string.IsNullOrEmpty(studentGrade))
                {
                    AddScopeLabel(studentGrade + "年级");
                }
                else
                {
                    SetDefaultStudentScope();
                }
            }
            else if (setting.Scope == "class")
            {
                // 班级模式：显示学生所在班级
                if (!string.IsNullOrEmpty(studentGrade) && !string.IsNullOrEmpty(studentClass))
                {
                    AddScopeLabel(studentGrade + "年级 " + studentClass + "班");
                }
                else if (!string.IsNullOrEmpty(studentGrade))
                {
                    // 只有年级信息，显示年级
                    AddScopeLabel(studentGrade + "年级");
                }
                else
                {
                    SetDefaultStudentScope();
                }
            }
            else
            {
                // 设置无效，使用学生默认
                SetDefaultStudentScope();
            }
        }

        /// <summary>
        /// 获取当前登录学生的年级
        /// </summary>
        private string GetStudentGrade()
        {
            if (!LearnSite.Common.CookieHelp.IsStudentLogin())
            {
                return null;
            }

            LearnSite.Model.Cook cook = new LearnSite.Model.Cook();
            string snum = cook.Snum;

            if (string.IsNullOrEmpty(snum))
            {
                return null;
            }

            LearnSite.BLL.Students studentBll = new LearnSite.BLL.Students();
            LearnSite.Model.Students student = studentBll.SnumGetModel(snum);

            if (student != null && student.Sgrade.HasValue)
            {
                return student.Sgrade.Value.ToString();
            }

            return null;
        }

        /// <summary>
        /// 获取当前登录学生的班级
        /// </summary>
        private string GetStudentClass()
        {
            if (!LearnSite.Common.CookieHelp.IsStudentLogin())
            {
                return null;
            }

            LearnSite.Model.Cook cook = new LearnSite.Model.Cook();
            string snum = cook.Snum;

            if (string.IsNullOrEmpty(snum))
            {
                return null;
            }

            LearnSite.BLL.Students studentBll = new LearnSite.BLL.Students();
            LearnSite.Model.Students student = studentBll.SnumGetModel(snum);

            if (student != null && student.Sclass.HasValue)
            {
                return student.Sclass.Value.ToString();
            }

            return null;
        }

        /// <summary>
        /// 添加显示范围标签
        /// </summary>
        private void AddScopeLabel(string scopeText)
        {
            Label scopeLabel = new Label();
            scopeLabel.Text = "当前显示：" + scopeText + "荣誉";
            scopeLabel.CssClass = "current-scope-label";
            scopeLabel.Style.Add("font-weight", "bold");
            scopeLabel.Style.Add("color", "#667eea");
            scopeLabel.Style.Add("margin-right", "20px");

            FilterSection.Controls.AddAt(0, scopeLabel);
        }

        /// <summary>
        /// 获取荣誉榜设置
        /// </summary>
        private HonorBoardSetting GetHonorBoardSetting()
        {
            string sql = "SELECT TOP 1 * FROM HonorBoardSettings WHERE Syear = '2026'";
            var ds = LearnSite.DBUtility.DbHelperSQL.Query(sql);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                var row = ds.Tables[0].Rows[0];
                return new HonorBoardSetting
                {
                    ID = Convert.ToInt32(row["ID"]),
                    Syear = row["Syear"].ToString(),
                    Scope = row["Scope"].ToString(),
                    Sgrade = row["Sgrade"] != DBNull.Value ? (int?)Convert.ToInt32(row["Sgrade"]) : null,
                    Sclass = row["Sclass"] != DBNull.Value ? (int?)Convert.ToInt32(row["Sclass"]) : null
                };
            }

            return null;
        }
        
        /// <summary>
        /// 添加当前年级提示标签
        /// </summary>
        private void AddCurrentGradeLabel(string grade)
        {
            // 创建一个标签显示当前年级
            Label currentGradeLabel = new Label();
            currentGradeLabel.Text = "当前显示：" + grade + "年级荣誉";
            currentGradeLabel.CssClass = "current-grade-label";
            currentGradeLabel.Style.Add("font-weight", "bold");
            currentGradeLabel.Style.Add("color", "#667eea");
            currentGradeLabel.Style.Add("margin-right", "20px");
            
            // 将标签添加到筛选区域
            FilterSection.Controls.AddAt(0, currentGradeLabel);
        }

        #region 绑定荣誉榜

        /// <summary>
        /// 绑定荣誉榜
        /// </summary>
        private void BindHonorBoard()
        {
            List<HonorConfig> configs = GetFilteredHonorConfigs();

            if (configs.Count == 0)
            {
                PanelHonors.Visible = false;
                PanelEmpty.Visible = true;
                return;
            }

            PanelHonors.Visible = true;
            PanelEmpty.Visible = false;
            PanelHonors.Controls.Clear();

            string syear = GetSyear();
            string sgrade = "";
            string sclass = "";
            
            // 根据教师设置和学生信息获取显示范围
            GetDisplayScope(out sgrade, out sclass);

            // 调试输出
            System.Diagnostics.Debug.WriteLine("BindHonorBoard - sgrade: '" + sgrade + "', sclass: '" + sclass + "'");

            // 检查是否需要自动评定荣誉（如果表中没有数据）
            CheckAndEvaluateHonors(syear);

            foreach (var config in configs)
            {
                PanelHonors.Controls.Add(CreateHonorCard(config, syear, sgrade, sclass));
            }
        }

        /// <summary>
        /// 根据教师设置和学生信息获取显示范围
        /// </summary>
        private void GetDisplayScope(out string sgrade, out string sclass)
        {
            // 从数据库读取教师设置的显示范围
            HonorBoardSetting setting = GetHonorBoardSetting();
            
            string studentGrade = GetStudentGrade();
            string studentClass = GetStudentClass();

            if (setting == null)
            {
                // 没有设置，默认使用学生年级
                sgrade = studentGrade;
                sclass = "";
                return;
            }
            else
            {
                // 根据教师设置返回显示范围
                if (setting.Scope == "school")
                {
                    // 全校：不设置年级和班级，查询全校数据
                    sgrade = "";
                    sclass = "";
                }
                else if (setting.Scope == "grade")
                {
                    // 年级模式：学生登录后自动显示自己所在年级的荣誉榜
                    sgrade = studentGrade;
                    sclass = "";
                }
                else if (setting.Scope == "class")
                {
                    // 班级模式：学生登录后自动显示自己所在班级的荣誉榜
                    sgrade = studentGrade;
                    sclass = studentClass;
                }
                else
                {
                    // 设置无效，使用学生年级
                    sgrade = studentGrade;
                    sclass = "";
                }
            }
        }
        
        /// <summary>
        /// 检查并评定荣誉（如果表中没有数据）
        /// </summary>
        private void CheckAndEvaluateHonors(string syear)
        {
            // 检查StudentHonors表中是否有数据
            string sql = "SELECT COUNT(*) FROM StudentHonors";
            object count = LearnSite.DBUtility.DbHelperSQL.GetSingle(sql);
            int recordCount = count != null ? Convert.ToInt32(count) : 0;
            
            System.Diagnostics.Debug.WriteLine("CheckAndEvaluateHonors - recordCount: " + recordCount);
            
            // 如果没有数据，自动评定所有学生的荣誉
            if (recordCount == 0)
            {
                System.Diagnostics.Debug.WriteLine("CheckAndEvaluateHonors - 没有荣誉数据，开始评定所有学生荣誉");
                honorService.EvaluateAllHonors("TERM1", syear);
            }
        }

        /// <summary>
        /// 创建荣誉卡片
        /// </summary>
        private Panel CreateHonorCard(HonorConfig config, string syear, string sgrade, string sclass)
        {
            Panel card = new Panel { CssClass = "honor-card" };

            // 头部
            Panel header = new Panel { CssClass = "honor-card-header" };
            Label icon = new Label
            {
                Text = config.IconEmoji ?? "🏆",
                CssClass = "honor-icon"
            };

            Panel titlePanel = new Panel();
            Label title = new Label
            {
                Text = config.HonorName,
                CssClass = "honor-title"
            };
            Label type = new Label
            {
                Text = config.HonorType,
                CssClass = "honor-type"
            };

            titlePanel.Controls.Add(title);
            titlePanel.Controls.Add(new LiteralControl("<br />"));
            titlePanel.Controls.Add(type);

            header.Controls.Add(icon);
            header.Controls.Add(titlePanel);
            card.Controls.Add(header);

            // 学生列表
            Panel studentsPanel = new Panel { CssClass = "honor-students" };
            studentsPanel.Controls.Add(new LiteralControl("<h4 style='margin: 0 0 15px 0; color: #666;'>获得该荣誉的学生</h4>"));

            List<HonorRanking> rankings = GetHonorRanking(config.HonorCode, syear, sgrade, sclass);

            if (rankings.Count == 0)
            {
                Label noData = new Label
                {
                    Text = "暂无学生获得此荣誉",
                    CssClass = "text-muted"
                };
                studentsPanel.Controls.Add(noData);
            }
            else
            {
                foreach (var ranking in rankings)
                {
                    studentsPanel.Controls.Add(CreateStudentItem(ranking));
                }
            }

            card.Controls.Add(studentsPanel);

            // 描述
            if (!string.IsNullOrEmpty(config.Description))
            {
                Panel descPanel = new Panel();
                descPanel.Attributes.Add("style", "margin-top: 15px; padding-top: 15px; border-top: 1px solid #eee; color: #666; font-size: 13px;");
                descPanel.Controls.Add(new LiteralControl("📋 " + config.Description));
                card.Controls.Add(descPanel);
            }

            return card;
        }

        /// <summary>
        /// 创建学生项
        /// </summary>
        private Panel CreateStudentItem(HonorRanking ranking)
        {
            Panel item = new Panel { CssClass = "honor-student-item" };

            // 头像
            Panel avatar = new Panel
            {
                CssClass = "honor-student-avatar"
            };
            avatar.Controls.Add(new LiteralControl(ranking.Sname.Substring(0, 1)));

            // 信息
            Panel info = new Panel { CssClass = "honor-student-info" };
            Label name = new Label
            {
                Text = ranking.Sname,
                CssClass = "honor-student-name"
            };
            Label classInfo = new Label
            {
                Text = ranking.ClassInfo,
                CssClass = "honor-student-class"
            };

            info.Controls.Add(name);
            info.Controls.Add(new LiteralControl("<br />"));
            info.Controls.Add(classInfo);

            // 徽章
            Label badge = new Label
            {
                Text = GetBadgeText(ranking.HonorLevel),
                CssClass = "badge " + GetBadgeClass(ranking.HonorLevel)
            };

            item.Controls.Add(avatar);
            item.Controls.Add(info);
            item.Controls.Add(badge);

            return item;
        }

        #endregion

        #region 数据获取

        /// <summary>
        /// 获取过滤后的荣誉配置
        /// </summary>
        private List<HonorConfig> GetFilteredHonorConfigs()
        {
            List<HonorConfig> allConfigs = honorService.GetAllHonorConfigs();

            if (!string.IsNullOrEmpty(currentHonorType))
            {
                return allConfigs.Where(c => c.HonorType == currentHonorType).ToList();
            }

            return allConfigs;
        }

        /// <summary>
        /// 获取荣誉排行榜
        /// </summary>
        private List<HonorRanking> GetHonorRanking(string honorCode, string syear, string sgrade, string sclass)
        {
            // 根据年级和班级过滤获取数据
            List<HonorRanking> rankings = null;

            // 调试输出
            string queryType = "";
            if (!string.IsNullOrEmpty(sgrade) && !string.IsNullOrEmpty(sclass))
            {
                // 指定年级和班级
                queryType = "班级查询";
                rankings = honorService.GetHonorRankingByClass(honorCode, syear, sgrade, sclass, 20);
            }
            else if (!string.IsNullOrEmpty(sgrade))
            {
                // 指定年级
                queryType = "年级查询";
                rankings = honorService.GetHonorRankingByGrade(honorCode, syear, sgrade, 20);
            }
            else
            {
                // 所有数据
                queryType = "全校查询";
                rankings = honorService.GetHonorRanking(honorCode, syear, 20);
            }

            System.Diagnostics.Debug.WriteLine("GetHonorRanking - honorCode: " + honorCode + ", syear: '" + syear + "', sgrade: '" + sgrade + "', sclass: '" + sclass + "', queryType: " + queryType + ", resultCount: " + (rankings != null ? rankings.Count : 0));

            // 如果没有数据，尝试获取所有学年的数据
            if (rankings == null || rankings.Count == 0)
            {
                System.Diagnostics.Debug.WriteLine("GetHonorRanking - 第一次查询无数据，尝试查询所有学年");
                if (!string.IsNullOrEmpty(sgrade) && !string.IsNullOrEmpty(sclass))
                {
                    rankings = honorService.GetHonorRankingByClass(honorCode, "", sgrade, sclass, 20);
                }
                else if (!string.IsNullOrEmpty(sgrade))
                {
                    rankings = honorService.GetHonorRankingByGrade(honorCode, "", sgrade, 20);
                }
                else
                {
                    rankings = honorService.GetHonorRanking(honorCode, "", 20);
                }
                System.Diagnostics.Debug.WriteLine("GetHonorRanking - 第二次查询结果数量: " + (rankings != null ? rankings.Count : 0));
            }

            return rankings != null ? rankings.Take(5).ToList() : new List<HonorRanking>();
        }

        /// <summary>
        /// 获取学年
        /// </summary>
        private string GetSyear()
        {
            // 从Session或其他地方获取当前学年
            if (Session["Syear"] != null)
            {
                return Session["Syear"].ToString();
            }

            // 返回当前年份作为默认学年
            return DateTime.Now.Year.ToString();
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 获取徽章文本
        /// </summary>
        private string GetBadgeText(int level)
        {
            switch (level)
            {
                case 1: return "🥉 青铜";
                case 2: return "🥈 银白";
                case 3: return "🥇 黄金";
                default: return "🏅";
            }
        }

        /// <summary>
        /// 获取徽章CSS类
        /// </summary>
        private string GetBadgeClass(int level)
        {
            switch (level)
            {
                case 1: return "bronze";
                case 2: return "silver";
                case 3: return "gold";
                default: return "bronze";
            }
        }

        #endregion

        #region 事件处理

        protected void BtnAll_Click(object sender, EventArgs e)
        {
            currentHonorType = "";
            SetActiveTab("BtnAll");
            BindHonorBoard();
        }

        protected void BtnAcademic_Click(object sender, EventArgs e)
        {
            currentHonorType = "学术成就";
            SetActiveTab("BtnAcademic");
            BindHonorBoard();
        }

        protected void BtnBehavior_Click(object sender, EventArgs e)
        {
            currentHonorType = "行为表现";
            SetActiveTab("BtnBehavior");
            BindHonorBoard();
        }

        protected void BtnTeam_Click(object sender, EventArgs e)
        {
            currentHonorType = "团队协作";
            SetActiveTab("BtnTeam");
            BindHonorBoard();
        }

        protected void BtnSpecialty_Click(object sender, EventArgs e)
        {
            currentHonorType = "特色专长";
            SetActiveTab("BtnSpecialty");
            BindHonorBoard();
        }

        protected void BtnComprehensive_Click(object sender, EventArgs e)
        {
            currentHonorType = "综合荣誉";
            SetActiveTab("BtnComprehensive");
            BindHonorBoard();
        }

        protected void Filter_Changed(object sender, EventArgs e)
        {
            BindHonorBoard();
        }

        protected void BtnRefresh_Click(object sender, EventArgs e)
        {
            // 重新评定荣誉
            string syear = GetSyear();
            honorService.EvaluateAllHonors("TERM1", syear);
            BindHonorBoard();
        }

        protected void BtnMyHonors_Click(object sender, EventArgs e)
        {
            Response.Redirect("myhonors.aspx");
        }

        protected void BtnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("myinfo.aspx");
        }

        /// <summary>
        /// 设置激活标签
        /// </summary>
        private void SetActiveTab(string activeBtnId)
        {
            foreach (Control control in Page.FindControl("form1").Controls)
            {
                if (control is Button)
                {
                    Button btn = (Button)control;
                    if (btn.ID == activeBtnId)
                    {
                        btn.CssClass = "honor-tab active";
                    }
                    else if (btn.CssClass.Contains("honor-tab"))
                    {
                        btn.CssClass = "honor-tab";
                    }
                }
            }
        }

        #endregion
    }
}
