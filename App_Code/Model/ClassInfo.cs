using System;

namespace LearnSite.Model
{
    /// <summary>
    /// 实体类ClassInfo 班级信息
    /// </summary>
    [Serializable]
    public class ClassInfo
    {
        public ClassInfo()
        { }

        #region Model
        private int _classId;
        private string _className;
        private int? _grade;
        private int? _classNum;

        /// <summary>
        /// 班级ID (对应Room表的Rid)
        /// </summary>
        public int ClassId
        {
            set { _classId = value; }
            get { return _classId; }
        }

        /// <summary>
        /// 班级名称 (如"7年级1班")
        /// </summary>
        public string ClassName
        {
            set { _className = value; }
            get { return _className; }
        }

        /// <summary>
        /// 年级
        /// </summary>
        public int? Grade
        {
            set { _grade = value; }
            get { return _grade; }
        }

        /// <summary>
        /// 班级号
        /// </summary>
        public int? ClassNum
        {
            set { _classNum = value; }
            get { return _classNum; }
        }
        #endregion Model
    }
}
