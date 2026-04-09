using System;

namespace LearnSite.Model
{
    /// <summary>
    /// AISkill:实体类
    /// </summary>
    [Serializable]
    public class AISkill
    {
        public AISkill()
        {}

        #region Model
        private int _id;
        private string _skillname;
        private string _promptcontent;
        private bool _isactive;

        public int Id
        {
            set { _id = value; }
            get { return _id; }
        }

        public string SkillName
        {
            set { _skillname = value; }
            get { return _skillname; }
        }

        public string PromptContent
        {
            set { _promptcontent = value; }
            get { return _promptcontent; }
        }

        public bool IsActive
        {
            set { _isactive = value; }
            get { return _isactive; }
        }
        #endregion Model
    }
}
