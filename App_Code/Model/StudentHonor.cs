using System;
using System.Collections.Generic;

namespace LearnSite.Model
{
    /// <summary>
    /// 学生荣誉记录模型
    /// </summary>
    public class StudentHonor
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 学生学号
        /// </summary>
        public string Snum { get; set; }

        /// <summary>
        /// 荣誉代码
        /// </summary>
        public string HonorCode { get; set; }

        /// <summary>
        /// 荣誉等级（1=铜，2=银，3=金）
        /// </summary>
        public int HonorLevel { get; set; }

        /// <summary>
        /// 获得日期
        /// </summary>
        public DateTime EarnDate { get; set; }

        /// <summary>
        /// 获得次数
        /// </summary>
        public int EarnCount { get; set; }

        /// <summary>
        /// 连续次数
        /// </summary>
        public int Continuous { get; set; }

        /// <summary>
        /// 学期
        /// </summary>
        public string Term { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// 获取等级名称
        /// </summary>
        public string LevelName
        {
            get
            {
                switch (HonorLevel)
                {
                    case 1: return "青铜荣誉";
                    case 2: return "银白荣誉";
                    case 3: return "黄金荣誉";
                    default: return "未知等级";
                }
            }
        }

        /// <summary>
        /// 获取等级英文
        /// </summary>
        public string LevelEnglish
        {
            get
            {
                switch (HonorLevel)
                {
                    case 1: return "bronze";
                    case 2: return "silver";
                    case 3: return "gold";
                    default: return "unknown";
                }
            }
        }

        /// <summary>
        /// 获取等级Emoji
        /// </summary>
        public string LevelEmoji
        {
            get
            {
                switch (HonorLevel)
                {
                    case 1: return "🥉";
                    case 2: return "🥈";
                    case 3: return "🥇";
                    default: return "🏅";
                }
            }
        }
    }

    /// <summary>
    /// 荣誉配置模型
    /// </summary>
    public class HonorConfig
    {
        /// <summary>
        /// 荣誉代码（主键）
        /// </summary>
        public string HonorCode { get; set; }

        /// <summary>
        /// 荣誉名称
        /// </summary>
        public string HonorName { get; set; }

        /// <summary>
        /// 荣誉类型
        /// </summary>
        public string HonorType { get; set; }

        /// <summary>
        /// 图标样式（CSS类名）
        /// </summary>
        public string IconClass { get; set; }

        /// <summary>
        /// Emoji图标
        /// </summary>
        public string IconEmoji { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int SortOrder { get; set; }
    }

    /// <summary>
    /// 荣誉排行模型
    /// </summary>
    public class HonorRanking
    {
        /// <summary>
        /// 学生学号
        /// </summary>
        public string Snum { get; set; }

        /// <summary>
        /// 学生姓名
        /// </summary>
        public string Sname { get; set; }

        /// <summary>
        /// 年级
        /// </summary>
        public string Sgrade { get; set; }

        /// <summary>
        /// 班级
        /// </summary>
        public string Sclass { get; set; }

        /// <summary>
        /// 荣誉等级
        /// </summary>
        public int HonorLevel { get; set; }

        /// <summary>
        /// 获得次数
        /// </summary>
        public int EarnCount { get; set; }

        /// <summary>
        /// 获得日期
        /// </summary>
        public DateTime EarnDate { get; set; }

        /// <summary>
        /// 班级信息
        /// </summary>
        public string ClassInfo
        {
            get { return Sgrade + "年级" + Sclass + "班"; }
        }
    }

    /// <summary>
    /// 学生荣誉统计模型
    /// </summary>
    public class StudentHonorStat
    {
        /// <summary>
        /// 荣誉总数
        /// </summary>
        public int TotalHonors { get; set; }

        /// <summary>
        /// 金色荣誉数
        /// </summary>
        public int GoldCount { get; set; }

        /// <summary>
        /// 银白荣誉数
        /// </summary>
        public int SilverCount { get; set; }

        /// <summary>
        /// 青铜荣誉数
        /// </summary>
        public int BronzeCount { get; set; }
    }

    /// <summary>
    /// 班级荣誉统计模型
    /// </summary>
    public class ClassHonorStat
    {
        /// <summary>
        /// 荣誉类型
        /// </summary>
        public string HonorType { get; set; }

        /// <summary>
        /// 荣誉名称
        /// </summary>
        public string HonorName { get; set; }

        /// <summary>
        /// 获得该荣誉的学生数
        /// </summary>
        public int StudentCount { get; set; }
    }

    /// <summary>
    /// 荣誉榜设置模型
    /// </summary>
    public class HonorBoardSetting
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 学年
        /// </summary>
        public string Syear { get; set; }

        /// <summary>
        /// 显示范围(school=全校, grade=年级, class=班级)
        /// </summary>
        public string Scope { get; set; }

        /// <summary>
        /// 年级(当Scope为grade或class时使用)
        /// </summary>
        public int? Sgrade { get; set; }

        /// <summary>
        /// 班级(当Scope为class时使用)
        /// </summary>
        public int? Sclass { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }
    }
}
