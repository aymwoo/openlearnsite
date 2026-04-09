using System;

namespace LearnSite.Model
{
    /// <summary>
    /// AICustomSkill:自定义技能实体类
    /// </summary>
    [Serializable]
    public class AICustomSkill
    {
        public AICustomSkill()
        {}

        #region Model
        private int _id;
        private string _skillname;
        private string _promptcontent;
        private string _skillscope;
        private bool _isactive;

        public int Id
        {
            set { _id = value; }
            get { return _id; }
        }

        /// <summary>
        /// 技能名称
        /// </summary>
        public string SkillName
        {
            set { _skillname = value; }
            get { return _skillname; }
        }

        /// <summary>
        /// 提示词内容
        /// </summary>
        public string PromptContent
        {
            set { _promptcontent = value; }
            get { return _promptcontent; }
        }

        /// <summary>
        /// 应用场景（逗号分隔的场景标识，如 chat,console,mission）
        /// </summary>
        public string SkillScope
        {
            set { _skillscope = value; }
            get { return _skillscope; }
        }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsActive
        {
            set { _isactive = value; }
            get { return _isactive; }
        }
        #endregion Model
    }
}
