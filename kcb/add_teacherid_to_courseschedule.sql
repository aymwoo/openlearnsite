-- 为CourseSchedule表添加TeacherID字段
-- 用于支持多教师使用平台，每个教师保存自己的课表

-- 检查并添加TeacherID字段
IF NOT EXISTS (
    SELECT * FROM sys.columns 
    WHERE object_id = OBJECT_ID('CourseSchedule') 
    AND name = 'TeacherID'
)
BEGIN
    ALTER TABLE CourseSchedule ADD TeacherID INT NOT NULL DEFAULT 0;
    
    PRINT '已成功添加TeacherID字段到CourseSchedule表';
END
ELSE
BEGIN
    PRINT 'TeacherID字段已存在，无需添加';
END

-- 为TeacherID字段添加索引以提高查询性能
IF NOT EXISTS (
    SELECT * FROM sys.indexes 
    WHERE name = 'IX_CourseSchedule_TeacherID' 
    AND object_id = OBJECT_ID('CourseSchedule')
)
BEGIN
    CREATE INDEX IX_CourseSchedule_TeacherID ON CourseSchedule(TeacherID);
    PRINT '已成功创建TeacherID索引';
END

-- 添加字段说明
EXEC sp_addextendedproperty 
    @name = N'MS_Description', 
    @value = N'教师ID，用于区分不同教师的课表',
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'CourseSchedule',
    @level2type = N'COLUMN', @level2name = N'TeacherID';
