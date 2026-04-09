using System;

namespace LearnSite.Model
{
    public class PerformanceScore
    {
        private int _id;
        private string _studentId;
        private string _studentName;
        private int _score;
        private string _reason;
        private DateTime _createTime;

        public PerformanceScore()
        {
            _studentId = "";
            _studentName = "";
            _reason = "";
        }

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string StudentId
        {
            get { return _studentId; }
            set { _studentId = value; }
        }

        public string StudentName
        {
            get { return _studentName; }
            set { _studentName = value; }
        }

        public int Score
        {
            get { return _score; }
            set { _score = value; }
        }

        public string Reason
        {
            get { return _reason; }
            set { _reason = value; }
        }

        public DateTime CreateTime
        {
            get { return _createTime; }
            set { _createTime = value; }
        }
    }
}