using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using LearnSite.BLL;
using LearnSite.Model;
using LearnSite.Common;

namespace LearnSite.Teacher
{
    public partial class honorboardmanage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CookieHelp.JudgeTeacherCookies();
            if (!IsPostBack)
            {
                Master.Page.Title = CookieHelp.SetMainPageTitle() + "荣誉榜设置";
                LoadSettings();
            }
        }

        /// <summary>
        /// 加载当前设置
        /// </summary>
        private void LoadSettings()
        {
            // 使用固定学年,后续可以从配置或其他地方获取
            string syear = "2026";

            // 查询当前设置
            string sql = "SELECT TOP 1 * FROM HonorBoardSettings WHERE Syear = '" + syear + "'";
            var ds = LearnSite.DBUtility.DbHelperSQL.Query(sql);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                var row = ds.Tables[0].Rows[0];
                string scope = row["Scope"].ToString();

                // 设置显示范围
                foreach (ListItem item in RblScope.Items)
                {
                    item.Selected = (item.Value == scope);
                }
            }
        }

        /// <summary>
        /// 显示范围改变时
        /// </summary>
        protected void RblScope_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 年级和班级模式下都不需要显示选择框，学生登录后自动根据自己所在的年级或班级显示
            // 此方法可以留空，因为选择范围后不需要做任何UI更新
        }

        /// <summary>
        /// 保存设置
        /// </summary>
        protected void BtnSave_Click(object sender, EventArgs e)
        {
            // 使用固定学年,后续可以从配置或其他地方获取
            string syear = "2026";
            string scope = RblScope.SelectedValue;

            // 检查是否已存在设置
            string checkSql = "SELECT COUNT(*) FROM HonorBoardSettings WHERE Syear = '" + syear + "'";
            var ds = LearnSite.DBUtility.DbHelperSQL.Query(checkSql);
            int count = Convert.ToInt32(ds.Tables[0].Rows[0][0]);

            if (count > 0)
            {
                // 更新设置，Sgrade和Sclass字段设为NULL，学生登录后自动根据自己所在的年级或班级显示
                string updateSql = "UPDATE HonorBoardSettings SET ";
                updateSql += "Scope = '" + scope + "', ";
                updateSql += "Sgrade = NULL, ";
                updateSql += "Sclass = NULL, ";
                updateSql += "UpdateTime = GETDATE() ";
                updateSql += "WHERE Syear = '" + syear + "'";

                LearnSite.DBUtility.DbHelperSQL.ExecuteSql(updateSql);
            }
            else
            {
                // 插入新设置
                string insertSql = "INSERT INTO HonorBoardSettings (Syear, Scope, Sgrade, Sclass, CreateTime) VALUES (";
                insertSql += "'" + syear + "', ";
                insertSql += "'" + scope + "', ";
                insertSql += "NULL, ";
                insertSql += "NULL, ";
                insertSql += "GETDATE())";

                LearnSite.DBUtility.DbHelperSQL.ExecuteSql(insertSql);
            }

            PanelSettings.Visible = false;
            PanelSuccess.Visible = true;
        }
    }
}
