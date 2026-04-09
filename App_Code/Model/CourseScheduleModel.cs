using System;

/// <summary>
/// 课程表实体类
/// </summary>
public class CourseScheduleModel
{
    private int _id;
    private int _schoolYear;
    private int _term;
    private int _weekDay;
    private int _timeSlot;
    private string _className;
    private string _subject;
    private int _teacherID;
    private DateTime _createTime;

    public CourseScheduleModel()
    {
        _createTime = DateTime.Now;
        _term = 1; // 默认1期
        _teacherID = 0; // 默认0表示未指定教师
    }

    public int ID
    {
        get { return _id; }
        set { _id = value; }
    }

    public int SchoolYear
    {
        get { return _schoolYear; }
        set { _schoolYear = value; }
    }

    public int Term
    {
        get { return _term; }
        set { _term = value; }
    }

    public int WeekDay
    {
        get { return _weekDay; }
        set { _weekDay = value; }
    }

    public int TimeSlot
    {
        get { return _timeSlot; }
        set { _timeSlot = value; }
    }

    public string ClassName
    {
        get { return _className; }
        set { _className = value; }
    }

    public string Subject
    {
        get { return _subject; }
        set { _subject = value; }
    }

    public int TeacherID
    {
        get { return _teacherID; }
        set { _teacherID = value; }
    }

    public DateTime CreateTime
    {
        get { return _createTime; }
        set { _createTime = value; }
    }
}

/// <summary>
/// 班级信息实体类
/// </summary>
public class ClassInfoModel
{
    private int _classID;
    private string _className;
    private int _grade;
    private int _class;
    private bool _isActive = true;

    public int ClassID
    {
        get { return _classID; }
        set { _classID = value; }
    }

    public string ClassName
    {
        get { return _className; }
        set { _className = value; }
    }

    public int Grade
    {
        get { return _grade; }
        set { _grade = value; }
    }

    public int Class
    {
        get { return _class; }
        set { _class = value; }
    }

    public bool IsActive
    {
        get { return _isActive; }
        set { _isActive = value; }
    }
}