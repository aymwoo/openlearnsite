using System;
using System.Data;
using System.Collections.Generic;
using LearnSite.Model;

namespace LearnSite.BLL
{
    /// <summary>
    /// 业务逻辑类Classes 的摘要说明。
    /// </summary>
    public class Classes
    {
        private readonly LearnSite.BLL.Room roomBll = new LearnSite.BLL.Room();

        public Classes()
        { }

        #region 成员方法

        /// <summary>
        /// 根据教师ID获取所教班级列表
        /// </summary>
        /// <param name="teacherId">教师ID</param>
        /// <returns>班级列表</returns>
        public List<ClassInfo> GetClassList(string teacherId)
        {
            List<ClassInfo> classList = new List<ClassInfo>();

            if (string.IsNullOrEmpty(teacherId))
            {
                return classList;
            }

            int rhid;
            if (!int.TryParse(teacherId, out rhid))
            {
                return classList;
            }

            DataSet ds = roomBll.GetMyClassList(rhid);

            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    ClassInfo info = new ClassInfo();

                    if (row["Rid"] != null && !string.IsNullOrEmpty(row["Rid"].ToString()))
                    {
                        info.ClassId = int.Parse(row["Rid"].ToString());
                    }

                    // 格式化班级名称
                    string gradeclass = row["Rgradeclass"] != null ? row["Rgradeclass"].ToString() : "";
                    if (!string.IsNullOrEmpty(gradeclass))
                    {
                        // gradeclass 格式为 "7.1"，转换为 "7年级1班"
                        string[] parts = gradeclass.Split('.');
                        if (parts.Length == 2)
                        {
                            int grade, classNum;
                            if (int.TryParse(parts[0].Trim(), out grade))
                            {
                                info.Grade = grade;
                            }
                            if (int.TryParse(parts[1].Trim(), out classNum))
                            {
                                info.ClassNum = classNum;
                            }
                            info.ClassName = grade + "年级" + classNum + "班";
                        }
                        else
                        {
                            info.ClassName = gradeclass;
                        }
                    }

                    classList.Add(info);
                }
            }

            return classList;
        }

        /// <summary>
        /// 获取所有班级列表
        /// </summary>
        /// <returns>班级列表</returns>
        public List<ClassInfo> GetAllClassList()
        {
            List<ClassInfo> classList = new List<ClassInfo>();

            DataSet ds = roomBll.GetAllList();

            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    ClassInfo info = new ClassInfo();

                    if (row["Rid"] != null && !string.IsNullOrEmpty(row["Rid"].ToString()))
                    {
                        info.ClassId = int.Parse(row["Rid"].ToString());
                    }

                    if (row["Rgrade"] != null && !string.IsNullOrEmpty(row["Rgrade"].ToString()))
                    {
                        info.Grade = int.Parse(row["Rgrade"].ToString());
                    }

                    if (row["Rclass"] != null && !string.IsNullOrEmpty(row["Rclass"].ToString()))
                    {
                        info.ClassNum = int.Parse(row["Rclass"].ToString());
                    }

                    if (info.Grade.HasValue && info.ClassNum.HasValue)
                    {
                        info.ClassName = info.Grade.Value + "年级" + info.ClassNum.Value + "班";
                    }

                    classList.Add(info);
                }
            }

            return classList;
        }

        #endregion
    }
}
