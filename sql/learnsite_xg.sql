/*
 Navicat Premium Data Transfer

 Source Server         : 245
 Source Server Type    : SQL Server
 Source Server Version : 16001000 (16.00.1000)
 Source Host           : 172.16.3.245:1433
 Source Catalog        : learnsite
 Source Schema         : dbo

 Target Server Type    : SQL Server
 Target Server Version : 16001000 (16.00.1000)
 File Encoding         : 65001

 Date: 06/04/2026 21:13:43
*/


-- ----------------------------
-- Table structure for Answers
-- ----------------------------
CREATE TABLE [dbo].[Answers] (
  [Aid] int  IDENTITY(1,1) NOT NULL,
  [Eid] int  NOT NULL,
  [Asid] int  NOT NULL,
  [Asnum] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Asname] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Asgrade] int  NULL,
  [Asclass] int  NULL,
  [Atime] datetime  NULL,
  [Ascore] int  NULL,
  [Aspent] int  NULL,
  [Adata] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL
)  
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Answers] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for Autonomic
-- ----------------------------
CREATE TABLE [dbo].[Autonomic] (
  [Aid] int  IDENTITY(1,1) NOT NULL,
  [Asid] int  NULL,
  [Anum] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Aname] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Ayid] int  NULL,
  [Afid] int  NULL,
  [Atype] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Afilename] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Aurl] nvarchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [Alength] int  NULL,
  [Ascore] int DEFAULT 0 NULL,
  [Adate] datetime  NULL,
  [Aip] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Avote] int DEFAULT 0 NULL,
  [Aegg] int DEFAULT 0 NULL,
  [Acheck] bit DEFAULT 0 NULL,
  [Aself] nvarchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [Agood] bit DEFAULT 0 NULL,
  [Ayear] int  NULL,
  [Agrade] int  NULL,
  [Aclass] int  NULL,
  [Aterm] int  NULL,
  [Ahit] int DEFAULT 0 NULL,
  [Aoffice] bit DEFAULT 0 NULL,
  [Aflash] bit DEFAULT 0 NULL,
  [Aerror] bit DEFAULT 0 NULL
)  
ON [PRIMARY]
GO

ALTER TABLE [dbo].[Autonomic] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for CheckRecords
-- ----------------------------
CREATE TABLE [dbo].[CheckRecords] (
  [Id] int  IDENTITY(1,1) NOT NULL,
  [PcName] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [IpAddress] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [ClassName] nvarchar(20) COLLATE Chinese_PRC_CI_AS  NULL,
  [HasRubbish] bit  NULL,
  [DrawerClean] bit  NULL,
  [EquipmentArranged] bit  NULL,
  [ChairAdjusted] bit  NULL,
  [KeyboardMouseDamaged] bit  NULL,
  [CableUnplugged] bit  NULL,
  [PeripheralUnplugged] bit  NULL,
  [ScreenMarked] bit  NULL,
  [SubmitTime] datetime DEFAULT getdate() NULL,
  [snid] int  NULL,
  [xuehao] nvarchar(20) COLLATE Chinese_PRC_CI_AS  NULL,
  [sname] nvarchar(20) COLLATE Chinese_PRC_CI_AS  NULL,
  [suser] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Comment] nvarchar(500) COLLATE Chinese_PRC_CI_AS  NULL
)  
ON [PRIMARY]
GO

ALTER TABLE [dbo].[CheckRecords] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for Chinese
-- ----------------------------
CREATE TABLE [dbo].[Chinese] (
  [Nid] int  IDENTITY(1,1) NOT NULL,
  [Ntitle] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Ncontent] ntext COLLATE Chinese_PRC_CI_AS  NULL
)  
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Chinese] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for ClassInfo
-- ----------------------------
CREATE TABLE [dbo].[ClassInfo] (
  [ClassID] int  IDENTITY(1,1) NOT NULL,
  [ClassName] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [Grade] int  NOT NULL,
  [Class] int  NOT NULL,
  [IsActive] bit DEFAULT 1 NOT NULL
)  
ON [PRIMARY]
GO

ALTER TABLE [dbo].[ClassInfo] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for Computers
-- ----------------------------
CREATE TABLE [dbo].[Computers] (
  [Pid] int  IDENTITY(1,1) NOT NULL,
  [Pip] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Pmachine] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Plock] bit DEFAULT 0 NULL,
  [Pdate] datetime  NULL,
  [Px] int DEFAULT 0 NULL,
  [Py] int DEFAULT 0 NULL,
  [Pm] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Pnum] nvarchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [Pon] bit DEFAULT 0 NULL
)  
ON [PRIMARY]
GO

ALTER TABLE [dbo].[Computers] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for Consoles
-- ----------------------------
CREATE TABLE [dbo].[Consoles] (
  [Nid] int  IDENTITY(1,1) NOT NULL,
  [Nhid] int  NULL,
  [Ncid] int  NULL,
  [Ntitle] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Ncontent] ntext COLLATE Chinese_PRC_CI_AS  NULL,
  [Npublish] bit DEFAULT 0 NULL,
  [Ndate] datetime  NULL,
  [Nbegin] bit DEFAULT 0 NULL
)  
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Consoles] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for Courses
-- ----------------------------
CREATE TABLE [dbo].[Courses] (
  [Cid] int  IDENTITY(1,1) NOT NULL,
  [Ctitle] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Cclass] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Ccontent] ntext COLLATE Chinese_PRC_CI_AS  NULL,
  [Cdate] datetime  NULL,
  [Chit] int DEFAULT 0 NULL,
  [Cobj] int  NULL,
  [Cterm] int  NULL,
  [Cks] int  NULL,
  [Cfiletype] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Cupload] bit DEFAULT 1 NULL,
  [Chid] int  NULL,
  [Cpublish] bit DEFAULT 1 NULL,
  [Cdelete] bit DEFAULT 0 NULL,
  [Cgood] bit DEFAULT 1 NULL,
  [Cold] bit DEFAULT 0 NULL,
  [Cbanner] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL
)  
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Courses] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for CourseSchedule
-- ----------------------------
CREATE TABLE [dbo].[CourseSchedule] (
  [ScheduleID] int  IDENTITY(1,1) NOT NULL,
  [SchoolYear] int  NOT NULL,
  [Term] int  NOT NULL,
  [WeekDay] int  NOT NULL,
  [TimeSlot] int  NOT NULL,
  [ClassName] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Subject] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [CreateTime] datetime  NULL,
  [TeacherID] int DEFAULT 0 NOT NULL
)  
ON [PRIMARY]
GO

ALTER TABLE [dbo].[CourseSchedule] SET (LOCK_ESCALATION = TABLE)
GO

EXEC sp_addextendedproperty
'MS_Description', N'教师ID，用于区分不同教师的课表',
'SCHEMA', N'dbo',
'TABLE', N'CourseSchedule',
'COLUMN', N'TeacherID'
GO


-- ----------------------------
-- Table structure for CourseSchedule_Backup
-- ----------------------------
CREATE TABLE [dbo].[CourseSchedule_Backup] (
  [ID] int  IDENTITY(1,1) NOT NULL,
  [SchoolYear] int  NOT NULL,
  [WeekDay] int  NOT NULL,
  [TimeSlot] int  NOT NULL,
  [ClassName] nvarchar(100) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [Subject] nvarchar(100) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [CreateTime] datetime  NOT NULL
)  
ON [PRIMARY]
GO

ALTER TABLE [dbo].[CourseSchedule_Backup] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for DelStudents
-- ----------------------------
CREATE TABLE [dbo].[DelStudents] (
  [Did] int  IDENTITY(1,1) NOT NULL,
  [Dnum] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Dyear] int  NULL,
  [Dgrade] int  NULL,
  [Dclass] int  NULL,
  [Dname] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Dsex] nvarchar(2) COLLATE Chinese_PRC_CI_AS  NULL,
  [Daddress] nvarchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [Dphone] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Dparents] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Dheadtheacher] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL
)  
ON [PRIMARY]
GO

ALTER TABLE [dbo].[DelStudents] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for English
-- ----------------------------
CREATE TABLE [dbo].[English] (
  [Eid] int  IDENTITY(1,1) NOT NULL,
  [Eword] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Emeaning] ntext COLLATE Chinese_PRC_CI_AS  NULL,
  [Elevel] int  NULL
)  
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[English] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for Exam
-- ----------------------------
CREATE TABLE [dbo].[Exam] (
  [ExamId] int  IDENTITY(1,1) NOT NULL,
  [ExamCode] nvarchar(20) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [ExamName] nvarchar(200) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [PaperId] int  NOT NULL,
  [ExamType] int DEFAULT 1 NULL,
  [StartTime] datetime  NOT NULL,
  [EndTime] datetime  NOT NULL,
  [Duration] int DEFAULT 60 NULL,
  [LateMinutes] int DEFAULT 0 NULL,
  [AllowRetake] int DEFAULT 0 NULL,
  [ShowAnswer] int DEFAULT 0 NULL,
  [ShowScore] int DEFAULT 1 NULL,
  [ShowRank] int DEFAULT 0 NULL,
  [AntiCheat] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [Password] nvarchar(100) COLLATE Chinese_PRC_CI_AS  NULL,
  [IpWhitelist] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [MaxParticipants] int DEFAULT 0 NULL,
  [ParticipantType] int DEFAULT 1 NULL,
  [Participants] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [Status] int DEFAULT 0 NULL,
  [PublishTime] datetime  NULL,
  [CreateBy] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [CreateTime] datetime DEFAULT getdate() NULL,
  [UpdateBy] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [UpdateTime] datetime  NULL,
  [TimeMode] int DEFAULT 1 NULL,
  [ValidDays] int DEFAULT 0 NULL
)  
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Exam] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for ExamAnswer
-- ----------------------------
CREATE TABLE [dbo].[ExamAnswer] (
  [AnswerId] bigint  IDENTITY(1,1) NOT NULL,
  [ExamId] int  NOT NULL,
  [PaperId] int  NOT NULL,
  [StudentId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [StudentName] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [ClassId] int  NULL,
  [Answers] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [TempAnswers] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [RandomQuestions] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [StartTime] datetime  NOT NULL,
  [SubmitTime] datetime  NULL,
  [Duration] int DEFAULT 0 NULL,
  [TotalScore] decimal(5,2) DEFAULT 0 NULL,
  [ObjectiveScore] decimal(5,2) DEFAULT 0 NULL,
  [SubjectiveScore] decimal(5,2) DEFAULT 0 NULL,
  [ScoreDetails] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [Status] int DEFAULT 0 NULL,
  [IpAddress] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [UserAgent] nvarchar(500) COLLATE Chinese_PRC_CI_AS  NULL,
  [MarkedBy] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [MarkTime] datetime  NULL,
  [Remark] nvarchar(500) COLLATE Chinese_PRC_CI_AS  NULL
)  
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[ExamAnswer] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for ExamAnswerLog
-- ----------------------------
CREATE TABLE [dbo].[ExamAnswerLog] (
  [LogId] bigint  IDENTITY(1,1) NOT NULL,
  [AnswerId] bigint  NOT NULL,
  [EventType] int  NOT NULL,
  [EventData] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [EventTime] datetime DEFAULT getdate() NULL
)  
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[ExamAnswerLog] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for ExamDictQuestionType
-- ----------------------------
CREATE TABLE [dbo].[ExamDictQuestionType] (
  [TypeId] int  NOT NULL,
  [TypeName] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [TypeCode] nvarchar(20) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [HasOptions] int DEFAULT 0 NULL,
  [AutoScore] int DEFAULT 1 NULL,
  [SortOrder] int DEFAULT 0 NULL,
  [Status] int DEFAULT 1 NULL
)  
ON [PRIMARY]
GO

ALTER TABLE [dbo].[ExamDictQuestionType] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for ExamPaper
-- ----------------------------
CREATE TABLE [dbo].[ExamPaper] (
  [PaperId] int  IDENTITY(1,1) NOT NULL,
  [PaperCode] nvarchar(20) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [PaperName] nvarchar(200) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [PaperType] int DEFAULT 1 NULL,
  [SubjectId] int  NULL,
  [GradeId] int  NULL,
  [TotalScore] decimal(5,2) DEFAULT 100.00 NULL,
  [PassScore] decimal(5,2) DEFAULT 60.00 NULL,
  [QuestionCount] int DEFAULT 0 NULL,
  [Duration] int DEFAULT 60 NULL,
  [Description] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [Sections] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [RandomConfig] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [Status] int DEFAULT 0 NULL,
  [CreateBy] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [CreateTime] datetime DEFAULT getdate() NULL,
  [UpdateBy] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [UpdateTime] datetime  NULL
)  
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[ExamPaper] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for ExamPaperQuestion
-- ----------------------------
CREATE TABLE [dbo].[ExamPaperQuestion] (
  [Id] bigint  IDENTITY(1,1) NOT NULL,
  [PaperId] int  NOT NULL,
  [QuestionId] bigint  NOT NULL,
  [SectionName] nvarchar(100) COLLATE Chinese_PRC_CI_AS  NULL,
  [Score] decimal(5,2)  NOT NULL,
  [SortOrder] int DEFAULT 0 NULL
)  
ON [PRIMARY]
GO

ALTER TABLE [dbo].[ExamPaperQuestion] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for ExamQuestion
-- ----------------------------
CREATE TABLE [dbo].[ExamQuestion] (
  [QuestionId] bigint  IDENTITY(1,1) NOT NULL,
  [BankId] int  NOT NULL,
  [QuestionType] int  NOT NULL,
  [QuestionContent] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [QuestionText] nvarchar(1000) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [Options] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [Answer] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [QuestionConfig] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [Analysis] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [Score] decimal(5,2) DEFAULT 2.00 NULL,
  [Difficulty] int DEFAULT 1 NULL,
  [KnowledgePoint] nvarchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [Tags] nvarchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [Image] nvarchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [Audio] nvarchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [Video] nvarchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [ParentId] bigint  NULL,
  [SortOrder] int DEFAULT 0 NULL,
  [Status] int DEFAULT 1 NULL,
  [UseCount] int DEFAULT 0 NULL,
  [CorrectRate] decimal(5,2) DEFAULT 0.00 NULL,
  [CreateBy] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [CreateTime] datetime DEFAULT getdate() NULL,
  [UpdateBy] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [UpdateTime] datetime  NULL,
  [GradeId] int  NULL,
  [CourseId] int  NULL
)  
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[ExamQuestion] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for ExamQuestionBank
-- ----------------------------
CREATE TABLE [dbo].[ExamQuestionBank] (
  [BankId] int  IDENTITY(1,1) NOT NULL,
  [BankCode] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [BankName] nvarchar(100) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [SubjectId] int  NULL,
  [GradeId] int  NULL,
  [Description] nvarchar(500) COLLATE Chinese_PRC_CI_AS  NULL,
  [QuestionCount] int DEFAULT 0 NULL,
  [Status] int DEFAULT 1 NULL,
  [CreateBy] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [CreateTime] datetime DEFAULT getdate() NULL,
  [UpdateBy] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [UpdateTime] datetime  NULL,
  [CourseId] int  NULL
)  
ON [PRIMARY]
GO

ALTER TABLE [dbo].[ExamQuestionBank] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for ExamResult
-- ----------------------------
CREATE TABLE [dbo].[ExamResult] (
  [ResultId] bigint  IDENTITY(1,1) NOT NULL,
  [ExamId] int  NOT NULL,
  [AnswerId] bigint  NOT NULL,
  [StudentId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [StudentName] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [ClassId] int  NULL,
  [TotalScore] decimal(5,2) DEFAULT 0 NULL,
  [ObjectiveScore] decimal(5,2) DEFAULT 0 NULL,
  [SubjectiveScore] decimal(5,2) DEFAULT 0 NULL,
  [RankInClass] int  NULL,
  [RankInGrade] int  NULL,
  [CorrectCount] int DEFAULT 0 NULL,
  [WrongCount] int DEFAULT 0 NULL,
  [PartialCount] int DEFAULT 0 NULL,
  [Duration] int DEFAULT 0 NULL,
  [SubmitTime] datetime  NULL,
  [Status] int DEFAULT 1 NULL
)  
ON [PRIMARY]
GO

ALTER TABLE [dbo].[ExamResult] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for Exams
-- ----------------------------
CREATE TABLE [dbo].[Exams] (
  [Eid] int  IDENTITY(1,1) NOT NULL,
  [Etitle] nvarchar(200) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [Edescription] nvarchar(500) COLLATE Chinese_PRC_CI_AS  NULL,
  [Cid] int  NOT NULL,
  [Hid] int  NOT NULL,
  [Etime] datetime  NULL,
  [Eclose] bit DEFAULT 1 NULL,
  [Escore] int DEFAULT 0 NULL,
  [Ecount] int DEFAULT 0 NULL,
  [Edata] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NOT NULL
)  
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Exams] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for ExamSurvey
-- ----------------------------
CREATE TABLE [dbo].[ExamSurvey] (
  [SurveyId] int  IDENTITY(1,1) NOT NULL,
  [SurveyCode] nvarchar(20) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [SurveyName] nvarchar(200) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [SurveyType] int DEFAULT 1 NULL,
  [Questions] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [Settings] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [StartTime] datetime  NULL,
  [EndTime] datetime  NULL,
  [Anonymous] int DEFAULT 0 NULL,
  [Status] int DEFAULT 0 NULL,
  [CreateBy] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [CreateTime] datetime DEFAULT getdate() NULL
)  
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[ExamSurvey] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for ExamSurveyAnswer
-- ----------------------------
CREATE TABLE [dbo].[ExamSurveyAnswer] (
  [AnswerId] bigint  IDENTITY(1,1) NOT NULL,
  [SurveyId] int  NOT NULL,
  [UserId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Answers] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [SubmitTime] datetime DEFAULT getdate() NULL,
  [IpAddress] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL
)  
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[ExamSurveyAnswer] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for Files
-- ----------------------------
CREATE TABLE [dbo].[Files] (
  [FileId] int  IDENTITY(1,1) NOT NULL,
  [FolderId] int  NOT NULL,
  [FileName] nvarchar(255) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [FileSize] bigint  NOT NULL,
  [CreateTime] datetime DEFAULT getdate() NULL,
  [UpdateTime] datetime DEFAULT getdate() NULL,
  [RelativePath] nvarchar(1000) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [UserSnum] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL
)  
ON [PRIMARY]
GO

ALTER TABLE [dbo].[Files] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for Flection
-- ----------------------------
CREATE TABLE [dbo].[Flection] (
  [Fid] int  IDENTITY(1,1) NOT NULL,
  [Fcid] int  NULL,
  [Fhid] int  NULL,
  [Fcontent] ntext COLLATE Chinese_PRC_CI_AS  NULL,
  [Fdate] datetime  NULL
)  
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Flection] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for Folders
-- ----------------------------
CREATE TABLE [dbo].[Folders] (
  [FolderId] int  IDENTITY(1,1) NOT NULL,
  [ParentFolderId] int  NULL,
  [FolderName] nvarchar(255) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [Path] nvarchar(1000) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [UserSnum] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [CreateTime] datetime DEFAULT getdate() NULL,
  [UpdateTime] datetime DEFAULT getdate() NULL
)  
ON [PRIMARY]
GO

ALTER TABLE [dbo].[Folders] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for Game
-- ----------------------------
CREATE TABLE [dbo].[Game] (
  [Gid] int  IDENTITY(1,1) NOT NULL,
  [Gsid] int  NULL,
  [Gsname] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Gnum] int  NULL,
  [Gtitle] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Gsave] int  NULL,
  [Gnote] ntext COLLATE Chinese_PRC_CI_AS  NULL,
  [Gscore] int  NULL,
  [Gdate] datetime  NULL
)  
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Game] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for Gauge
-- ----------------------------
CREATE TABLE [dbo].[Gauge] (
  [Gid] int  IDENTITY(1,1) NOT NULL,
  [Ghid] int  NULL,
  [Gtype] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Gtitle] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Gcount] int  NULL,
  [Gdate] datetime  NULL
)  
ON [PRIMARY]
GO

ALTER TABLE [dbo].[Gauge] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for GaugeFeedback
-- ----------------------------
CREATE TABLE [dbo].[GaugeFeedback] (
  [Fid] int  IDENTITY(1,1) NOT NULL,
  [Fnum] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Fgrade] int  NULL,
  [Fclass] int  NULL,
  [Fcid] int  NULL,
  [Fmid] int  NULL,
  [Fwid] int  NULL,
  [Fgid] int  NULL,
  [Fselect] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Fscore] int  NULL,
  [Fgood] bit DEFAULT 0 NULL,
  [Fdate] datetime  NULL,
  [Fsid] int DEFAULT 0 NULL
)  
ON [PRIMARY]
GO

ALTER TABLE [dbo].[GaugeFeedback] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for GaugeItem
-- ----------------------------
CREATE TABLE [dbo].[GaugeItem] (
  [Mid] int  IDENTITY(1,1) NOT NULL,
  [Mgid] int  NULL,
  [Mitem] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Mscore] int  NULL,
  [Msort] int  NULL
)  
ON [PRIMARY]
GO

ALTER TABLE [dbo].[GaugeItem] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for GroupWork
-- ----------------------------
CREATE TABLE [dbo].[GroupWork] (
  [Gid] int  IDENTITY(1,1) NOT NULL,
  [Gnum] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Gstudents] nvarchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [Gterm] int  NULL,
  [Ggrade] int  NULL,
  [Gclass] int  NULL,
  [Gcid] int  NULL,
  [Gmid] int  NULL,
  [Gfilename] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Gtype] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Gurl] nvarchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [Glengh] int  NULL,
  [Gscore] int  NULL,
  [Gtime] int  NULL,
  [Gvote] int  NULL,
  [Gcheck] bit DEFAULT 0 NULL,
  [Gnote] ntext COLLATE Chinese_PRC_CI_AS  NULL,
  [Grank] int  NULL,
  [Ghit] int DEFAULT 0 NULL,
  [Gip] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Gdate] datetime  NULL,
  [Ggroup] int  NULL
)  
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[GroupWork] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for HonorBoardSettings
-- ----------------------------
CREATE TABLE [dbo].[HonorBoardSettings] (
  [ID] int  IDENTITY(1,1) NOT NULL,
  [Syear] nvarchar(10) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [Scope] nvarchar(20) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [Sgrade] int  NULL,
  [Sclass] int  NULL,
  [CreateTime] datetime  NULL,
  [UpdateTime] datetime  NULL
)  
ON [PRIMARY]
GO

ALTER TABLE [dbo].[HonorBoardSettings] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for HonorConfig
-- ----------------------------
CREATE TABLE [dbo].[HonorConfig] (
  [HonorCode] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [HonorName] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [HonorType] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [IconClass] nvarchar(100) COLLATE Chinese_PRC_CI_AS  NULL,
  [IconEmoji] nvarchar(10) COLLATE Chinese_PRC_CI_AS  NULL,
  [Description] nvarchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [IsActive] bit DEFAULT 1 NULL,
  [SortOrder] int  NULL
)  
ON [PRIMARY]
GO

ALTER TABLE [dbo].[HonorConfig] SET (LOCK_ESCALATION = TABLE)
GO

EXEC sp_addextendedproperty
'MS_Description', N'荣誉配置表，定义可获得的荣誉类型和规则',
'SCHEMA', N'dbo',
'TABLE', N'HonorConfig'
GO


-- ----------------------------
-- Table structure for House
-- ----------------------------
CREATE TABLE [dbo].[House] (
  [Hid] int  IDENTITY(1,1) NOT NULL,
  [Hname] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Hseat] ntext COLLATE Chinese_PRC_CI_AS  NULL
)  
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[House] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for Ip
-- ----------------------------
CREATE TABLE [dbo].[Ip] (
  [Iid] int  IDENTITY(1,1) NOT NULL,
  [Ihid] int  NULL,
  [Inum] int  NULL,
  [Iip] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL
)  
ON [PRIMARY]
GO

ALTER TABLE [dbo].[Ip] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for JudgeArg
-- ----------------------------
CREATE TABLE [dbo].[JudgeArg] (
  [Jid] int  IDENTITY(1,1) NOT NULL,
  [Jhid] int  NULL,
  [Jmid] int  NULL,
  [Jsleep] int DEFAULT 1000 NULL,
  [Jinone] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Jintwo] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Jinthree] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Joutone] nvarchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [Joutwo] nvarchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [Jouthree] nvarchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [Jright] bit DEFAULT 0 NULL,
  [Jcode] ntext COLLATE Chinese_PRC_CI_AS  NULL,
  [Jcid] int  NULL,
  [Jimg] nvarchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [Jthumb] nvarchar(200) COLLATE Chinese_PRC_CI_AS  NULL
)  
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[JudgeArg] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for kechengbiao
-- ----------------------------
CREATE TABLE [dbo].[kechengbiao] (
  [Id] int  IDENTITY(1,1) NOT NULL,
  [Year] int  NOT NULL,
  [Day1] varchar(255) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [Day2] varchar(255) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [Term] int  NOT NULL,
  [TeacherId] int  NOT NULL,
  [SubjectId] int  NOT NULL,
  [Day3] varchar(255) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [Day4] varchar(255) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [Day5] varchar(255) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [SlotNumber] int  NULL,
  [StartTime] time(7)  NULL,
  [EndTime] time(7)  NULL
)  
ON [PRIMARY]
GO

ALTER TABLE [dbo].[kechengbiao] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for ListMenu
-- ----------------------------
CREATE TABLE [dbo].[ListMenu] (
  [Lid] int  IDENTITY(1,1) NOT NULL,
  [Lcid] int  NULL,
  [Lsort] int DEFAULT 0 NULL,
  [Ltype] int  NULL,
  [Lxid] int  NULL,
  [Lshow] bit DEFAULT 1 NULL,
  [Ltitle] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL
)  
ON [PRIMARY]
GO

ALTER TABLE [dbo].[ListMenu] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for MenuWorks
-- ----------------------------
CREATE TABLE [dbo].[MenuWorks] (
  [kid] int  IDENTITY(1,1) NOT NULL,
  [ksid] int  NULL,
  [klid] int  NULL,
  [ktime] int  NULL,
  [kcheck] bit DEFAULT 0 NULL,
  [kstar] int  NULL
)  
ON [PRIMARY]
GO

ALTER TABLE [dbo].[MenuWorks] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for Mission
-- ----------------------------
CREATE TABLE [dbo].[Mission] (
  [Mid] int  IDENTITY(1,1) NOT NULL,
  [Mtitle] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Mcid] int  NULL,
  [Mcontent] ntext COLLATE Chinese_PRC_CI_AS  NULL,
  [Mdate] datetime  NULL,
  [Mhit] int DEFAULT 0 NULL,
  [Mfiletype] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Mupload] bit DEFAULT 0 NULL,
  [Msort] int DEFAULT 0 NULL,
  [Mpublish] bit DEFAULT 1 NULL,
  [Mgroup] bit DEFAULT 0 NULL,
  [Mgid] int DEFAULT 0 NULL,
  [Mdelete] bit DEFAULT 0 NULL,
  [Mexample] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Mcategory] int DEFAULT 0 NULL,
  [Microworld] bit DEFAULT 0 NULL,
  [Mback] nvarchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [Mhelp] bit DEFAULT 0 NULL,
  [Mcase] ntext COLLATE Chinese_PRC_CI_AS  NULL
)  
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Mission] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for NotSign
-- ----------------------------
CREATE TABLE [dbo].[NotSign] (
  [Nid] int  IDENTITY(1,1) NOT NULL,
  [Nnum] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Ndate] datetime  NULL,
  [Nyear] int  NULL,
  [Nmonth] int  NULL,
  [Nday] int  NULL,
  [Nweek] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Nnote] ntext COLLATE Chinese_PRC_CI_AS  NULL,
  [Ngrade] int  NULL,
  [Nterm] int  NULL
)  
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[NotSign] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for Pchinese
-- ----------------------------
CREATE TABLE [dbo].[Pchinese] (
  [Pid] int  IDENTITY(1,1) NOT NULL,
  [Psid] int  NULL,
  [Psnum] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Papple] int DEFAULT 0 NULL,
  [Ptotal] int DEFAULT 0 NULL,
  [Pspeed] int DEFAULT 0 NULL,
  [Pdegree] int  NULL,
  [Pyear] int  NULL,
  [Pgrade] int  NULL,
  [Pclass] int  NULL,
  [Pterm] int  NULL,
  [Pdate] datetime  NULL
)  
ON [PRIMARY]
GO

ALTER TABLE [dbo].[Pchinese] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for performance_score
-- ----------------------------
CREATE TABLE [dbo].[performance_score] (
  [Id] int  IDENTITY(1,1) NOT NULL,
  [StudentId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [StudentName] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [Score] int  NOT NULL,
  [Reason] nvarchar(255) COLLATE Chinese_PRC_CI_AS  NULL,
  [CreateTime] datetime DEFAULT getdate() NULL
)  
ON [PRIMARY]
GO

ALTER TABLE [dbo].[performance_score] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for Pfinger
-- ----------------------------
CREATE TABLE [dbo].[Pfinger] (
  [Pid] int  IDENTITY(1,1) NOT NULL,
  [Psnum] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Pspd] decimal(18,2)  NULL,
  [Pyear] int  NULL,
  [Pmonth] int  NULL,
  [Pdate] datetime  NULL,
  [Pdegree] int  NULL,
  [Pgrade] int DEFAULT 0 NULL,
  [Pterm] int DEFAULT 0 NULL,
  [Psid] int DEFAULT 0 NULL
)  
ON [PRIMARY]
GO

ALTER TABLE [dbo].[Pfinger] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for Problems
-- ----------------------------
CREATE TABLE [dbo].[Problems] (
  [Pid] int  IDENTITY(1,1) NOT NULL,
  [Phid] int  NULL,
  [Pnid] int  NULL,
  [Ptitle] nvarchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [Pcode] nvarchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [Pouput] nvarchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [Pscore] int  NULL,
  [Pdate] datetime  NULL,
  [Psort] int  NULL,
  [Pcid] int  NULL
)  
ON [PRIMARY]
GO

ALTER TABLE [dbo].[Problems] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for Ptyper
-- ----------------------------
CREATE TABLE [dbo].[Ptyper] (
  [Pid] int  IDENTITY(1,1) NOT NULL,
  [Ptid] int  NULL,
  [Psnum] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Pscore] int  NULL,
  [Pdate] datetime  NULL,
  [Pip] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Ptype] int DEFAULT 1 NULL,
  [Pdegree] int DEFAULT 0 NULL,
  [Pgrade] int DEFAULT 0 NULL,
  [Pterm] int DEFAULT 0 NULL,
  [Psid] int DEFAULT 0 NULL
)  
ON [PRIMARY]
GO

ALTER TABLE [dbo].[Ptyper] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for Quiz
-- ----------------------------
CREATE TABLE [dbo].[Quiz] (
  [Qid] int  IDENTITY(1,1) NOT NULL,
  [Qtype] int  NULL,
  [Question] ntext COLLATE Chinese_PRC_CI_AS  NULL,
  [Qanswer] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Qanalyze] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Qscore] int  NULL,
  [Qclass] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Qselect] bit DEFAULT 0 NULL,
  [Qright] int DEFAULT 0 NULL,
  [Qwrong] int DEFAULT 0 NULL,
  [Qaccuracy] int DEFAULT 0 NULL,
  [BankId] int  NULL,
  [Difficulty] int DEFAULT 1 NULL,
  [Tags] nvarchar(500) COLLATE Chinese_PRC_CI_AS  NULL,
  [OptionsJson] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [QuestionConfig] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [KnowledgePoint] nvarchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [UseCount] int DEFAULT 0 NULL
)  
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Quiz] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for QuizGrade
-- ----------------------------
CREATE TABLE [dbo].[QuizGrade] (
  [Qid] int  IDENTITY(1,1) NOT NULL,
  [Qobj] int DEFAULT 0 NULL,
  [Qclass] ntext COLLATE Chinese_PRC_CI_AS  NULL,
  [Qhid] int  NULL,
  [Qonly] int  NULL,
  [Qmore] int  NULL,
  [Qjudge] int  NULL,
  [Qopen] bit DEFAULT 1 NULL,
  [Qanswer] bit DEFAULT 1 NULL,
  [Qfill] int DEFAULT 1 NULL,
  [Qmatch] int DEFAULT 1 NULL,
  [Qcategory] int DEFAULT 1 NULL,
  [Qessay] int DEFAULT 1 NULL,
  [Qnpfield] int DEFAULT 0 NULL,
  [Qselectfield] int DEFAULT 0 NULL,
  [Qscorefield] int DEFAULT 0 NULL,
  [Qmatrixfield] int DEFAULT 0 NULL,
  [Qmultiblankfield] int DEFAULT 0 NULL
)  
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[QuizGrade] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for Research
-- ----------------------------
CREATE TABLE [dbo].[Research] (
  [Rid] int  IDENTITY(1,1) NOT NULL,
  [Rsid] int  NULL,
  [Ryear] int  NULL,
  [Rgrade] int  NULL,
  [Rclass] int  NULL,
  [Rterm] int  NULL,
  [Rlearn] smallmoney  NULL,
  [Rplay] smallmoney  NULL,
  [Rsleep] smallmoney  NULL,
  [Rfree] smallmoney  NULL,
  [Rdate] datetime  NULL
)  
ON [PRIMARY]
GO

ALTER TABLE [dbo].[Research] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for Result
-- ----------------------------
CREATE TABLE [dbo].[Result] (
  [Rid] int  IDENTITY(1,1) NOT NULL,
  [Rnum] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Rscore] int  NULL,
  [Rdate] datetime  NULL,
  [Rhistory] ntext COLLATE Chinese_PRC_CI_AS  NULL,
  [Rwrong] ntext COLLATE Chinese_PRC_CI_AS  NULL,
  [Rgrade] int DEFAULT 0 NULL,
  [Rterm] int DEFAULT 0 NULL,
  [Rsid] int DEFAULT 0 NULL,
  [Ranswer] ntext COLLATE Chinese_PRC_CI_AS  NULL
)  
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Result] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for Room
-- ----------------------------
CREATE TABLE [dbo].[Room] (
  [Rid] int  IDENTITY(1,1) NOT NULL,
  [Rhid] int DEFAULT 0 NULL,
  [Rgrade] int  NULL,
  [Rclass] int  NULL,
  [Rset] bit DEFAULT 0 NULL,
  [Rpwd] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Rlock] bit DEFAULT 0 NULL,
  [Rip] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Rgauge] bit DEFAULT 0 NULL,
  [RgroupMax] int DEFAULT 0 NULL,
  [Rclassedit] bit DEFAULT 0 NULL,
  [Rphotoedit] bit DEFAULT 0 NULL,
  [Rsexedit] bit DEFAULT 0 NULL,
  [Rnameedit] bit DEFAULT 0 NULL,
  [Rcid] int  NULL,
  [Ropen] bit DEFAULT 0 NULL,
  [Rseat] int DEFAULT 0 NULL,
  [Rshare] bit DEFAULT 0 NULL,
  [Rpwdsee] bit DEFAULT 0 NULL,
  [Rgroupshare] bit DEFAULT 0 NULL,
  [Rtyper] nvarchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [Rreg] bit DEFAULT 0 NULL,
  [Rchinese] nvarchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [Rscratch] bit DEFAULT 0 NULL,
  [RLogin] bit DEFAULT 0 NULL,
  [Rpass] bit DEFAULT 0 NULL,
  [Rtitle] bit DEFAULT 0 NULL,
  [ClassName] nvarchar(100) COLLATE Chinese_PRC_CI_AS  NULL,
  [Campus] nvarchar(100) COLLATE Chinese_PRC_CI_AS DEFAULT '' NULL,
  [Rinternet] bit DEFAULT 0 NULL
)  
ON [PRIMARY]
GO

ALTER TABLE [dbo].[Room] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for ShareDisk
-- ----------------------------
CREATE TABLE [dbo].[ShareDisk] (
  [Kid] int  IDENTITY(1,1) NOT NULL,
  [Kown] bit DEFAULT 0 NULL,
  [Kyear] int  NULL,
  [Kgrade] int  NULL,
  [Kclass] int  NULL,
  [Kgroup] int  NULL,
  [Knum] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Kname] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Kfilename] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Kfsize] int  NULL,
  [Kfurl] nvarchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [Kftpe] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Kfdate] datetime  NULL
)  
ON [PRIMARY]
GO

ALTER TABLE [dbo].[ShareDisk] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for Signin
-- ----------------------------
CREATE TABLE [dbo].[Signin] (
  [Qid] int  IDENTITY(1,1) NOT NULL,
  [Qnum] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Qattitude] int DEFAULT 0 NULL,
  [Qdate] datetime  NULL,
  [Qyear] int  NULL,
  [Qmonth] int  NULL,
  [Qday] int  NULL,
  [Qweek] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Qip] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Qmachine] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Qnote] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Qwork] int DEFAULT 0 NULL,
  [Qgrade] int  NULL,
  [Qterm] int  NULL,
  [Qgroup] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Qgscore] int DEFAULT 0 NULL,
  [Qsid] int DEFAULT 0 NULL,
  [Qname] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Qclass] int DEFAULT 0 NULL,
  [Qsyear] int DEFAULT 0 NULL,
  [Qcid] int  NULL,
  [Qtitle] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [Qsession] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Qonline] bit DEFAULT 0 NULL
)  
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Signin] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for Skdj
-- ----------------------------
CREATE TABLE [dbo].[Skdj] (
  [Ssid] int  IDENTITY(1,1) NOT NULL,
  [Sstid] int  NULL,
  [Ssdate] datetime  NULL,
  [SSyear] int  NULL,
  [SSmonth] int  NULL,
  [SSday] int  NULL,
  [SSweek] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Ssession] nvarchar(20) COLLATE Chinese_PRC_CI_AS  NULL,
  [SSgrade] int  NULL,
  [SSclass] int  NULL,
  [Ssctitle] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [Sstname] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Ssnotes] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL
)  
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Skdj] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for Soft
-- ----------------------------
CREATE TABLE [dbo].[Soft] (
  [Fid] int  IDENTITY(1,1) NOT NULL,
  [Ftitle] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Fcontent] ntext COLLATE Chinese_PRC_CI_AS  NULL,
  [Furl] nvarchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [Fhit] int DEFAULT 0 NULL,
  [Fdate] datetime  NULL,
  [Ffiletype] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Fclass] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Fhide] bit DEFAULT 0 NULL,
  [Fopen] int DEFAULT 0 NULL,
  [Fhid] int DEFAULT 0 NULL,
  [Fyid] int DEFAULT 1 NULL,
  [Fup] bit DEFAULT 0 NULL
)  
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Soft] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for SoftCategory
-- ----------------------------
CREATE TABLE [dbo].[SoftCategory] (
  [Yid] int  IDENTITY(1,1) NOT NULL,
  [Ysort] int  NULL,
  [Ytitle] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Ycontent] nvarchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [Yopen] bit DEFAULT 0 NULL
)  
ON [PRIMARY]
GO

ALTER TABLE [dbo].[SoftCategory] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for Solves
-- ----------------------------
CREATE TABLE [dbo].[Solves] (
  [Vid] int  IDENTITY(1,1) NOT NULL,
  [Vpid] int  NULL,
  [Vsid] int  NULL,
  [Vanswer] nvarchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [Vright] bit DEFAULT 0 NULL,
  [Vscore] int  NULL,
  [Vdate] datetime  NULL,
  [Vgrade] int  NULL,
  [Vterm] int  NULL,
  [Vyear] nvarchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [Vnid] int  NULL,
  [Vcid] int  NULL,
  [Vclass] nvarchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [Vlid] int  NULL
)  
ON [PRIMARY]
GO

ALTER TABLE [dbo].[Solves] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for student_scores
-- ----------------------------
CREATE TABLE [dbo].[student_scores] (
  [ID] int  IDENTITY(1,1) NOT NULL,
  [StudentID] nvarchar(20) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [Qattitude] decimal(5,2) DEFAULT 0 NULL,
  [Wscore] decimal(5,2) DEFAULT 0 NULL,
  [CreateDate] date DEFAULT getdate() NULL,
  [CreatedAt] datetime DEFAULT getdate() NULL
)  
ON [PRIMARY]
GO

ALTER TABLE [dbo].[student_scores] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for StudentHonors
-- ----------------------------
CREATE TABLE [dbo].[StudentHonors] (
  [ID] int  IDENTITY(1,1) NOT NULL,
  [Snum] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [HonorCode] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [HonorLevel] int DEFAULT 1 NOT NULL,
  [EarnDate] datetime DEFAULT getdate() NOT NULL,
  [EarnCount] int DEFAULT 1 NOT NULL,
  [Continuous] int DEFAULT 1 NOT NULL,
  [Term] nvarchar(20) COLLATE Chinese_PRC_CI_AS  NULL,
  [Remarks] nvarchar(500) COLLATE Chinese_PRC_CI_AS  NULL
)  
ON [PRIMARY]
GO

ALTER TABLE [dbo].[StudentHonors] SET (LOCK_ESCALATION = TABLE)
GO

EXEC sp_addextendedproperty
'MS_Description', N'学生荣誉记录表，记录学生获得的各种荣誉',
'SCHEMA', N'dbo',
'TABLE', N'StudentHonors'
GO


-- ----------------------------
-- Table structure for Students
-- ----------------------------
CREATE TABLE [dbo].[Students] (
  [Sid] int  IDENTITY(1,1) NOT NULL,
  [Snum] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Syear] int  NULL,
  [Sgrade] int  NULL,
  [Sclass] int  NULL,
  [Sname] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Spwd] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Sex] nvarchar(2) COLLATE Chinese_PRC_CI_AS  NULL,
  [Saddress] nvarchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [Sphone] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Sparents] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Sheadtheacher] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Sscore] int DEFAULT 0 NULL,
  [Squiz] int DEFAULT 0 NULL,
  [Sattitude] int DEFAULT 0 NULL,
  [Sape] nvarchar(1) COLLATE Chinese_PRC_CI_AS  NULL,
  [Swscore] int DEFAULT 0 NULL,
  [Stscore] int DEFAULT 0 NULL,
  [Sallscore] int DEFAULT 0 NULL,
  [Spscore] int DEFAULT 0 NULL,
  [Sgroup] int DEFAULT 0 NULL,
  [Sleader] bit DEFAULT 0 NULL,
  [Svote] int DEFAULT 0 NULL,
  [Sgscore] int  NULL,
  [Sfscore] int DEFAULT 0 NULL,
  [Svscore] int DEFAULT 0 NULL,
  [Sgtitle] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Sascore] int DEFAULT 0 NULL,
  [Skaoxu] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Swdscore] int DEFAULT 0 NULL,
  [Stxtform] int DEFAULT 0 NULL,
  [Schinese] int DEFAULT 0 NULL,
  [Stat] bit DEFAULT 0 NULL,
  [Stenscore] int DEFAULT 0 NULL,
  [Sidle] int DEFAULT 0 NULL,
  [Sztype] int DEFAULT 0 NULL,
  [SigninCount] int DEFAULT 0 NULL,
  [Steam] int DEFAULT 0 NULL,
  [Sfixedip] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Sseat] nvarchar(20) COLLATE Chinese_PRC_CI_AS  NULL,
  [Theme] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL
)  
ON [PRIMARY]
GO

ALTER TABLE [dbo].[Students] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for StudentsExcel
-- ----------------------------
CREATE TABLE [dbo].[StudentsExcel] (
  [Sid] int  IDENTITY(1,1) NOT NULL,
  [Snum] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Syear] int  NULL,
  [Sgrade] int  NULL,
  [Sclass] int  NULL,
  [Sname] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Spwd] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Sex] nvarchar(2) COLLATE Chinese_PRC_CI_AS  NULL,
  [Saddress] nvarchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [Sphone] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [Sparents] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Sheadtheacher] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Sscore] int DEFAULT 0 NULL,
  [Squiz] int DEFAULT 0 NULL,
  [Sattitude] int DEFAULT 0 NULL,
  [Sape] nvarchar(1) COLLATE Chinese_PRC_CI_AS  NULL
)  
ON [PRIMARY]
GO

ALTER TABLE [dbo].[StudentsExcel] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for Summary
-- ----------------------------
CREATE TABLE [dbo].[Summary] (
  [Sid] int  IDENTITY(1,1) NOT NULL,
  [Scid] int  NULL,
  [Smid] int  NULL,
  [Shid] int  NULL,
  [Scontent] ntext COLLATE Chinese_PRC_CI_AS  NULL,
  [Sdate] datetime  NULL,
  [Sgrade] int  NULL,
  [Sclass] int  NULL,
  [Syear] int  NULL,
  [Sshow] bit DEFAULT 1 NULL
)  
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Summary] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for Survey
-- ----------------------------
CREATE TABLE [dbo].[Survey] (
  [Vid] int  IDENTITY(1,1) NOT NULL,
  [Vcid] int  NULL,
  [Vhid] int  NULL,
  [Vtitle] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Vcontent] ntext COLLATE Chinese_PRC_CI_AS  NULL,
  [Vtype] int DEFAULT 0 NULL,
  [Vtotal] int DEFAULT 0 NULL,
  [Vscore] int DEFAULT 0 NULL,
  [Vaverage] int  NULL,
  [Vclose] bit DEFAULT 0 NULL,
  [Vpoint] bit DEFAULT 0 NULL,
  [Vdate] datetime  NULL,
  [YunXuXueShengChuTi] bit DEFAULT 0 NULL,
  [Qscorefield] int DEFAULT 0 NULL,
  [Qnpfield] int DEFAULT 0 NULL,
  [Qmatrixfield] int DEFAULT 0 NULL,
  [Qselectfield] int DEFAULT 0 NULL
)  
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Survey] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for SurveyClass
-- ----------------------------
CREATE TABLE [dbo].[SurveyClass] (
  [Yid] int  IDENTITY(1,1) NOT NULL,
  [Yyear] int  NULL,
  [Ygrade] int  NULL,
  [Yclass] int  NULL,
  [Yterm] int  NULL,
  [Ycid] int  NULL,
  [Yvid] int  NULL,
  [Yselect] ntext COLLATE Chinese_PRC_CI_AS  NULL,
  [Ycount] ntext COLLATE Chinese_PRC_CI_AS  NULL,
  [Yscore] int  NULL,
  [Ydate] datetime  NULL
)  
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[SurveyClass] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for SurveyFeedback
-- ----------------------------
CREATE TABLE [dbo].[SurveyFeedback] (
  [Fid] int  IDENTITY(1,1) NOT NULL,
  [Fnum] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Fyear] int  NULL,
  [Fgrade] int  NULL,
  [Fclass] int  NULL,
  [Fterm] int  NULL,
  [Fcid] int  NULL,
  [Fvid] int  NULL,
  [Fvtype] int DEFAULT 0 NULL,
  [Fselect] ntext COLLATE Chinese_PRC_CI_AS  NULL,
  [Fscore] int DEFAULT 0 NULL,
  [Fdate] datetime  NULL,
  [Fsid] int DEFAULT 0 NULL,
  [Flid] int  NULL,
  [FBlanks] nvarchar(max) COLLATE Chinese_PRC_CI_AS DEFAULT 0 NULL,
  [FError] nvarchar(max) COLLATE Chinese_PRC_CI_AS DEFAULT 0 NULL,
  [FCiShu] int DEFAULT 0 NULL
)  
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[SurveyFeedback] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for SurveyItem
-- ----------------------------
CREATE TABLE [dbo].[SurveyItem] (
  [Mid] int  IDENTITY(1,1) NOT NULL,
  [Mqid] int  NULL,
  [Mvid] int  NULL,
  [Mitem] ntext COLLATE Chinese_PRC_CI_AS  NULL,
  [Mscore] int DEFAULT 0 NULL,
  [Mcount] int DEFAULT 0 NULL,
  [Mcid] int  NULL,
  [Mblack] bit DEFAULT 0 NULL,
  [Image] nvarchar(500) COLLATE Chinese_PRC_CI_AS  NULL,
  [SortOrder] int DEFAULT 0 NULL
)  
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[SurveyItem] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for SurveyQuestion
-- ----------------------------
CREATE TABLE [dbo].[SurveyQuestion] (
  [Qid] int  IDENTITY(1,1) NOT NULL,
  [Qvid] int  NULL,
  [Qcid] int  NULL,
  [Qtitle] ntext COLLATE Chinese_PRC_CI_AS  NULL,
  [Qcount] int DEFAULT 0 NULL,
  [Qblack] bit DEFAULT 0 NULL,
  [ChuTiRen] nvarchar(50) COLLATE Chinese_PRC_CI_AS DEFAULT 0 NULL,
  [ChuTiRenID] nvarchar(50) COLLATE Chinese_PRC_CI_AS DEFAULT 0 NULL,
  [ZhuangTai] int DEFAULT 1 NULL,
  [DianZan] nvarchar(max) COLLATE Chinese_PRC_CI_AS DEFAULT 0 NULL,
  [CanKaoYe] nvarchar(max) COLLATE Chinese_PRC_CI_AS DEFAULT 0 NULL,
  [Qtype] int DEFAULT 0 NULL,
  [QuestionConfig] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [MinLength] int  NULL,
  [MaxLength] int  NULL,
  [Required] bit DEFAULT 1 NULL,
  [SortOrder] int DEFAULT 0 NULL
)  
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[SurveyQuestion] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for Teacher
-- ----------------------------
CREATE TABLE [dbo].[Teacher] (
  [Hid] int  IDENTITY(1,1) NOT NULL,
  [Hname] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Hpwd] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Hpermiss] bit DEFAULT 0 NULL,
  [Hnote] ntext COLLATE Chinese_PRC_CI_AS  NULL,
  [Hpath] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Hdelete] bit DEFAULT 0 NULL,
  [Hcount] int DEFAULT 0 NULL,
  [Hnick] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Hroom] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL
)  
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Teacher] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for Teachers
-- ----------------------------
CREATE TABLE [dbo].[Teachers] (
  [Tid] int  IDENTITY(1,1) NOT NULL,
  [TeacherName] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [TeacherPwd] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Theme] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Tdate] datetime  NULL
)  
ON [PRIMARY]
GO

ALTER TABLE [dbo].[Teachers] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for TempSeat
-- ----------------------------
CREATE TABLE [dbo].[TempSeat] (
  [Tid] int  IDENTITY(1,1) NOT NULL,
  [Snum] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [TempIp] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [TempSeat] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [ExpireTime] datetime  NOT NULL,
  [CreateTime] datetime  NOT NULL
)  
ON [PRIMARY]
GO

ALTER TABLE [dbo].[TempSeat] SET (LOCK_ESCALATION = TABLE)
GO

EXEC sp_addextendedproperty
'MS_Description', N'主键ID',
'SCHEMA', N'dbo',
'TABLE', N'TempSeat',
'COLUMN', N'Tid'
GO

EXEC sp_addextendedproperty
'MS_Description', N'学号',
'SCHEMA', N'dbo',
'TABLE', N'TempSeat',
'COLUMN', N'Snum'
GO

EXEC sp_addextendedproperty
'MS_Description', N'临时IP地址',
'SCHEMA', N'dbo',
'TABLE', N'TempSeat',
'COLUMN', N'TempIp'
GO

EXEC sp_addextendedproperty
'MS_Description', N'临时座位号',
'SCHEMA', N'dbo',
'TABLE', N'TempSeat',
'COLUMN', N'TempSeat'
GO

EXEC sp_addextendedproperty
'MS_Description', N'过期时间（临时座位失效时间）',
'SCHEMA', N'dbo',
'TABLE', N'TempSeat',
'COLUMN', N'ExpireTime'
GO

EXEC sp_addextendedproperty
'MS_Description', N'创建时间',
'SCHEMA', N'dbo',
'TABLE', N'TempSeat',
'COLUMN', N'CreateTime'
GO

EXEC sp_addextendedproperty
'MS_Description', N'临时座位表，用于记录学生临时换座位信息，下节课自动恢复原座位',
'SCHEMA', N'dbo',
'TABLE', N'TempSeat'
GO


-- ----------------------------
-- Table structure for TermTotal
-- ----------------------------
CREATE TABLE [dbo].[TermTotal] (
  [Tid] int  IDENTITY(1,1) NOT NULL,
  [Tnum] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Tterm] int  NULL,
  [Tgrade] int  NULL,
  [Tscore] int  NULL,
  [Tgscore] int  NULL,
  [Tquiz] int  NULL,
  [Tattitude] int  NULL,
  [Twscore] int  NULL,
  [Ttscore] int  NULL,
  [Tpscore] int  NULL,
  [Tallscore] int  NULL,
  [Tape] varchar(10) COLLATE Chinese_PRC_CI_AS  NULL,
  [Tfscore] int DEFAULT 0 NULL,
  [Tvscore] int DEFAULT 0 NULL,
  [Tsid] int DEFAULT 0 NULL,
  [Tyear] int  NULL,
  [Tclass] int  NULL,
  [Tname] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Ttxtform] int DEFAULT 0 NULL,
  [Tchinese] int DEFAULT 0 NULL
)  
ON [PRIMARY]
GO

ALTER TABLE [dbo].[TermTotal] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for ThemeSettings
-- ----------------------------
CREATE TABLE [dbo].[ThemeSettings] (
  [SettingId] int  IDENTITY(1,1) NOT NULL,
  [SettingKey] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [SettingValue] nvarchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [SettingDesc] nvarchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [CreatedDate] datetime  NULL
)  
ON [PRIMARY]
GO

ALTER TABLE [dbo].[ThemeSettings] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for TimeSlots
-- ----------------------------
CREATE TABLE [dbo].[TimeSlots] (
  [SlotID] int  IDENTITY(1,1) NOT NULL,
  [SlotName] nvarchar(100) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [StartTime] time(7)  NOT NULL,
  [EndTime] time(7)  NOT NULL,
  [DisplayOrder] int  NOT NULL
)  
ON [PRIMARY]
GO

ALTER TABLE [dbo].[TimeSlots] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for TopicDiscuss
-- ----------------------------
CREATE TABLE [dbo].[TopicDiscuss] (
  [Tid] int  IDENTITY(1,1) NOT NULL,
  [Tcid] int  NULL,
  [Ttitle] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Tcontent] ntext COLLATE Chinese_PRC_CI_AS  NULL,
  [Tcount] int DEFAULT 0 NULL,
  [Tteacher] int  NULL,
  [Tdate] datetime  NULL,
  [Tclose] bit DEFAULT 0 NULL,
  [Tresult] ntext COLLATE Chinese_PRC_CI_AS  NULL
)  
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[TopicDiscuss] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for TopicReply
-- ----------------------------
CREATE TABLE [dbo].[TopicReply] (
  [Rid] int  IDENTITY(1,1) NOT NULL,
  [Rtid] int  NULL,
  [Rsnum] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Rwords] ntext COLLATE Chinese_PRC_CI_AS  NULL,
  [Rtime] datetime  NULL,
  [Rip] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Rscore] int  NULL,
  [Rban] bit DEFAULT 0 NULL,
  [Rgrade] int  NULL,
  [Rterm] int  NULL,
  [Rcid] int DEFAULT 0 NULL,
  [Rclass] int DEFAULT 0 NULL,
  [Rsid] int DEFAULT 0 NULL,
  [Ryear] int DEFAULT 0 NULL,
  [Redit] bit DEFAULT 0 NULL,
  [Ragree] int DEFAULT 0 NULL,
  [Rlid] int  NULL
)  
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[TopicReply] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for Turtle
-- ----------------------------
CREATE TABLE [dbo].[Turtle] (
  [Tid] int  IDENTITY(1,1) NOT NULL,
  [Thid] int  NULL,
  [Ttilte] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Tcontent] ntext COLLATE Chinese_PRC_CI_AS  NULL,
  [Tdegree] int  NULL,
  [Tsort] int  NULL,
  [Tcode] ntext COLLATE Chinese_PRC_CI_AS  NULL,
  [Timg] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Turl] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Tout] nvarchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [Tdate] datetime  NULL,
  [Tstudy] bit DEFAULT 0 NULL,
  [Tsid] int  NULL,
  [Tscore] int  NULL,
  [Tip] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL
)  
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Turtle] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for TurtleAnswer
-- ----------------------------
CREATE TABLE [dbo].[TurtleAnswer] (
  [Aid] int  IDENTITY(1,1) NOT NULL,
  [Amid] int  NULL,
  [Aqid] int  NULL,
  [Acode] ntext COLLATE Chinese_PRC_CI_AS  NULL,
  [Aimg] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Aurl] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Aout] nvarchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [Ascore] int  NULL,
  [Asid] int  NULL,
  [Asname] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Alock] bit DEFAULT 0 NULL,
  [Adate] datetime  NULL
)  
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[TurtleAnswer] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for TurtleMatch
-- ----------------------------
CREATE TABLE [dbo].[TurtleMatch] (
  [Mid] int  IDENTITY(1,1) NOT NULL,
  [Mhid] int  NULL,
  [Mtitle] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Mcontent] ntext COLLATE Chinese_PRC_CI_AS  NULL,
  [Mbegin] datetime  NULL,
  [Mend] datetime  NULL,
  [Mpublish] bit DEFAULT 0 NULL,
  [Mdate] datetime  NULL
)  
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[TurtleMatch] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for TurtleQuestion
-- ----------------------------
CREATE TABLE [dbo].[TurtleQuestion] (
  [Qid] int  IDENTITY(1,1) NOT NULL,
  [Qmid] int  NULL,
  [Qtitle] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Qcontent] ntext COLLATE Chinese_PRC_CI_AS  NULL,
  [Qdegree] int  NULL,
  [Qsort] int  NULL,
  [Qcode] ntext COLLATE Chinese_PRC_CI_AS  NULL,
  [Qimg] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Qurl] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Qout] nvarchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [Qscore] int  NULL,
  [Qdate] datetime  NULL
)  
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[TurtleQuestion] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for TxtForm
-- ----------------------------
CREATE TABLE [dbo].[TxtForm] (
  [Mid] int  IDENTITY(1,1) NOT NULL,
  [Mtitle] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Mcid] int  NULL,
  [Mcontent] ntext COLLATE Chinese_PRC_CI_AS  NULL,
  [Mdate] datetime  NULL,
  [Mhit] int  NULL,
  [Mpublish] bit DEFAULT 0 NULL,
  [Mdelete] bit DEFAULT 0 NULL,
  [Mcollabo] bit DEFAULT 0 NULL
)  
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[TxtForm] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for TxtFormBack
-- ----------------------------
CREATE TABLE [dbo].[TxtFormBack] (
  [Rid] int  IDENTITY(1,1) NOT NULL,
  [Rmid] int  NULL,
  [Rsnum] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Rsid] int  NULL,
  [Rwords] ntext COLLATE Chinese_PRC_CI_AS  NULL,
  [Rtime] datetime  NULL,
  [Rip] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Rscore] int DEFAULT 0 NULL,
  [Ryear] int  NULL,
  [Rterm] int  NULL,
  [Rgrade] int  NULL,
  [Rclass] int  NULL,
  [Ragree] int  NULL,
  [Rlid] int  NULL,
  [Rcontent] ntext COLLATE Chinese_PRC_CI_AS  NULL
)  
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[TxtFormBack] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for Typer
-- ----------------------------
CREATE TABLE [dbo].[Typer] (
  [Tid] int  IDENTITY(1,1) NOT NULL,
  [Ttype] smallint  NULL,
  [Tuse] int  NULL,
  [Ttitle] nvarchar(100) COLLATE Chinese_PRC_CI_AS  NULL,
  [Tcontent] ntext COLLATE Chinese_PRC_CI_AS  NULL
)  
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Typer] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for Works
-- ----------------------------
CREATE TABLE [dbo].[Works] (
  [Wid] int  IDENTITY(1,1) NOT NULL,
  [Wnum] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Wcid] int  NULL,
  [Wmid] int  NULL,
  [Wmsort] int  NULL,
  [Wfilename] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Wurl] nvarchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [Wlength] int  NULL,
  [Wscore] int DEFAULT 0 NULL,
  [Wdate] datetime  NULL,
  [Wip] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Wtime] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Wvote] int DEFAULT 0 NULL,
  [Wegg] smallint DEFAULT 1 NULL,
  [Wcheck] bit DEFAULT 0 NULL,
  [Wself] nvarchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [Wcan] bit DEFAULT 1 NULL,
  [Wgood] bit DEFAULT 0 NULL,
  [Wtype] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Wgrade] int  NULL,
  [Wterm] int  NULL,
  [Whit] int DEFAULT 0 NULL,
  [Wlscore] int DEFAULT 0 NULL,
  [Wlemotion] int DEFAULT 0 NULL,
  [Woffice] bit DEFAULT 0 NULL,
  [Wflash] bit DEFAULT 0 NULL,
  [Werror] bit DEFAULT 0 NULL,
  [Wfscore] int DEFAULT 0 NULL,
  [Wclass] int DEFAULT 0 NULL,
  [Wsid] int DEFAULT 0 NULL,
  [Wname] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Wyear] int DEFAULT 0 NULL,
  [Wdscore] int DEFAULT 0 NULL,
  [Wthumbnail] nvarchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [Wtitle] nvarchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [Weditday] int DEFAULT 0 NULL,
  [Wdict] ntext COLLATE Chinese_PRC_CI_AS  NULL,
  [Wcode] ntext COLLATE Chinese_PRC_CI_AS  NULL,
  [Wpass] bit DEFAULT 0 NULL,
  [Wlid] int  NULL
)  
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Works] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for WorksDiscuss
-- ----------------------------
CREATE TABLE [dbo].[WorksDiscuss] (
  [Did] int  IDENTITY(1,1) NOT NULL,
  [Dwid] int  NULL,
  [Dsnum] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Dwords] ntext COLLATE Chinese_PRC_CI_AS  NULL,
  [Dtime] datetime  NULL,
  [Dip] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Dsid] int DEFAULT 0 NULL
)  
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[WorksDiscuss] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Auto increment value for Answers
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[Answers]', RESEED, 1)
GO


-- ----------------------------
-- Primary Key structure for table Answers
-- ----------------------------
ALTER TABLE [dbo].[Answers] ADD CONSTRAINT [PK__Answers__C6970A10D00EB005] PRIMARY KEY CLUSTERED ([Aid])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for Autonomic
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[Autonomic]', RESEED, 1)
GO


-- ----------------------------
-- Primary Key structure for table Autonomic
-- ----------------------------
ALTER TABLE [dbo].[Autonomic] ADD CONSTRAINT [PK__Autonomi__C6970A108BBD3787] PRIMARY KEY CLUSTERED ([Aid])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for CheckRecords
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[CheckRecords]', RESEED, 22973)
GO


-- ----------------------------
-- Primary Key structure for table CheckRecords
-- ----------------------------
ALTER TABLE [dbo].[CheckRecords] ADD CONSTRAINT [PK__CheckRec__3214EC07E468F13F] PRIMARY KEY CLUSTERED ([Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for Chinese
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[Chinese]', RESEED, 1)
GO


-- ----------------------------
-- Primary Key structure for table Chinese
-- ----------------------------
ALTER TABLE [dbo].[Chinese] ADD CONSTRAINT [PK__Chinese__C7D1D6CB4C7DDDF8] PRIMARY KEY CLUSTERED ([Nid])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for ClassInfo
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[ClassInfo]', RESEED, 20)
GO


-- ----------------------------
-- Indexes structure for table ClassInfo
-- ----------------------------
CREATE NONCLUSTERED INDEX [IX_ClassInfo_Grade_Class]
ON [dbo].[ClassInfo] (
  [Grade] ASC,
  [Class] ASC
)
GO


-- ----------------------------
-- Primary Key structure for table ClassInfo
-- ----------------------------
ALTER TABLE [dbo].[ClassInfo] ADD CONSTRAINT [PK_ClassInfo] PRIMARY KEY CLUSTERED ([ClassID])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for Computers
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[Computers]', RESEED, 5210)
GO


-- ----------------------------
-- Primary Key structure for table Computers
-- ----------------------------
ALTER TABLE [dbo].[Computers] ADD CONSTRAINT [PK__Computer__C57059383710452A] PRIMARY KEY CLUSTERED ([Pid])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for Consoles
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[Consoles]', RESEED, 1)
GO


-- ----------------------------
-- Primary Key structure for table Consoles
-- ----------------------------
ALTER TABLE [dbo].[Consoles] ADD CONSTRAINT [PK__Consoles__C7D1D6CBACBF386E] PRIMARY KEY CLUSTERED ([Nid])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for Courses
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[Courses]', RESEED, 3091)
GO


-- ----------------------------
-- Primary Key structure for table Courses
-- ----------------------------
ALTER TABLE [dbo].[Courses] ADD CONSTRAINT [PK_Courses] PRIMARY KEY CLUSTERED ([Cid])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for CourseSchedule
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[CourseSchedule]', RESEED, 1042)
GO


-- ----------------------------
-- Indexes structure for table CourseSchedule
-- ----------------------------
CREATE NONCLUSTERED INDEX [IX_CourseSchedule_SchoolYear_Term_Subject]
ON [dbo].[CourseSchedule] (
  [SchoolYear] ASC,
  [Term] ASC,
  [Subject] ASC
)
GO

CREATE NONCLUSTERED INDEX [IX_CourseSchedule_SchoolYear_Term_WeekDay_TimeSlot]
ON [dbo].[CourseSchedule] (
  [SchoolYear] ASC,
  [Term] ASC,
  [WeekDay] ASC,
  [TimeSlot] ASC
)
GO

CREATE NONCLUSTERED INDEX [IX_CourseSchedule_TeacherID]
ON [dbo].[CourseSchedule] (
  [TeacherID] ASC
)
GO


-- ----------------------------
-- Primary Key structure for table CourseSchedule
-- ----------------------------
ALTER TABLE [dbo].[CourseSchedule] ADD CONSTRAINT [PK_CourseSchedule] PRIMARY KEY CLUSTERED ([ScheduleID])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for CourseSchedule_Backup
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[CourseSchedule_Backup]', RESEED, 46)
GO


-- ----------------------------
-- Auto increment value for DelStudents
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[DelStudents]', RESEED, 2049)
GO


-- ----------------------------
-- Primary Key structure for table DelStudents
-- ----------------------------
ALTER TABLE [dbo].[DelStudents] ADD CONSTRAINT [PK__DelStude__C0312218BDF27B03] PRIMARY KEY CLUSTERED ([Did])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for English
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[English]', RESEED, 10701)
GO


-- ----------------------------
-- Primary Key structure for table English
-- ----------------------------
ALTER TABLE [dbo].[English] ADD CONSTRAINT [PK__English__C1971B53A61E0FC8] PRIMARY KEY CLUSTERED ([Eid])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for Exam
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[Exam]', RESEED, 1)
GO


-- ----------------------------
-- Indexes structure for table Exam
-- ----------------------------
CREATE UNIQUE NONCLUSTERED INDEX [IX_Exam_Code]
ON [dbo].[Exam] (
  [ExamCode] ASC
)
GO

CREATE NONCLUSTERED INDEX [IX_Exam_Status]
ON [dbo].[Exam] (
  [Status] ASC
)
GO

CREATE NONCLUSTERED INDEX [IX_Exam_Time]
ON [dbo].[Exam] (
  [StartTime] ASC,
  [EndTime] ASC
)
GO


-- ----------------------------
-- Primary Key structure for table Exam
-- ----------------------------
ALTER TABLE [dbo].[Exam] ADD CONSTRAINT [PK_Exam] PRIMARY KEY CLUSTERED ([ExamId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for ExamAnswer
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[ExamAnswer]', RESEED, 2)
GO


-- ----------------------------
-- Indexes structure for table ExamAnswer
-- ----------------------------
CREATE NONCLUSTERED INDEX [IX_ExamAnswer_ExamId]
ON [dbo].[ExamAnswer] (
  [ExamId] ASC
)
GO

CREATE NONCLUSTERED INDEX [IX_ExamAnswer_StudentId]
ON [dbo].[ExamAnswer] (
  [StudentId] ASC
)
GO

CREATE NONCLUSTERED INDEX [IX_ExamAnswer_Status]
ON [dbo].[ExamAnswer] (
  [Status] ASC
)
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_ExamAnswer_Unique]
ON [dbo].[ExamAnswer] (
  [ExamId] ASC,
  [StudentId] ASC
)
GO


-- ----------------------------
-- Primary Key structure for table ExamAnswer
-- ----------------------------
ALTER TABLE [dbo].[ExamAnswer] ADD CONSTRAINT [PK_ExamAnswer] PRIMARY KEY CLUSTERED ([AnswerId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for ExamAnswerLog
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[ExamAnswerLog]', RESEED, 1)
GO


-- ----------------------------
-- Indexes structure for table ExamAnswerLog
-- ----------------------------
CREATE NONCLUSTERED INDEX [IX_ExamAnswerLog_AnswerId]
ON [dbo].[ExamAnswerLog] (
  [AnswerId] ASC
)
GO


-- ----------------------------
-- Primary Key structure for table ExamAnswerLog
-- ----------------------------
ALTER TABLE [dbo].[ExamAnswerLog] ADD CONSTRAINT [PK_ExamAnswerLog] PRIMARY KEY CLUSTERED ([LogId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for ExamPaper
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[ExamPaper]', RESEED, 1)
GO


-- ----------------------------
-- Indexes structure for table ExamPaper
-- ----------------------------
CREATE UNIQUE NONCLUSTERED INDEX [IX_ExamPaper_Code]
ON [dbo].[ExamPaper] (
  [PaperCode] ASC
)
GO


-- ----------------------------
-- Primary Key structure for table ExamPaper
-- ----------------------------
ALTER TABLE [dbo].[ExamPaper] ADD CONSTRAINT [PK_ExamPaper] PRIMARY KEY CLUSTERED ([PaperId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for ExamPaperQuestion
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[ExamPaperQuestion]', RESEED, 10001)
GO


-- ----------------------------
-- Indexes structure for table ExamPaperQuestion
-- ----------------------------
CREATE NONCLUSTERED INDEX [IX_ExamPaperQuestion_PaperId]
ON [dbo].[ExamPaperQuestion] (
  [PaperId] ASC
)
GO

CREATE NONCLUSTERED INDEX [IX_ExamPaperQuestion_QuestionId]
ON [dbo].[ExamPaperQuestion] (
  [QuestionId] ASC
)
GO


-- ----------------------------
-- Primary Key structure for table ExamPaperQuestion
-- ----------------------------
ALTER TABLE [dbo].[ExamPaperQuestion] ADD CONSTRAINT [PK_ExamPaperQuestion] PRIMARY KEY CLUSTERED ([Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for ExamQuestion
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[ExamQuestion]', RESEED, 30005)
GO


-- ----------------------------
-- Indexes structure for table ExamQuestion
-- ----------------------------
CREATE NONCLUSTERED INDEX [IX_ExamQuestion_BankId]
ON [dbo].[ExamQuestion] (
  [BankId] ASC
)
GO

CREATE NONCLUSTERED INDEX [IX_ExamQuestion_QuestionType]
ON [dbo].[ExamQuestion] (
  [QuestionType] ASC
)
GO

CREATE NONCLUSTERED INDEX [IX_ExamQuestion_Difficulty]
ON [dbo].[ExamQuestion] (
  [Difficulty] ASC
)
GO

CREATE NONCLUSTERED INDEX [IX_ExamQuestion_Status]
ON [dbo].[ExamQuestion] (
  [Status] ASC
)
GO

CREATE NONCLUSTERED INDEX [IX_ExamQuestion_ParentId]
ON [dbo].[ExamQuestion] (
  [ParentId] ASC
)
GO

CREATE NONCLUSTERED INDEX [IX_ExamQuestion_GradeId]
ON [dbo].[ExamQuestion] (
  [GradeId] ASC
)
GO

CREATE NONCLUSTERED INDEX [IX_ExamQuestion_CourseId]
ON [dbo].[ExamQuestion] (
  [CourseId] ASC
)
GO


-- ----------------------------
-- Primary Key structure for table ExamQuestion
-- ----------------------------
ALTER TABLE [dbo].[ExamQuestion] ADD CONSTRAINT [PK_ExamQuestion] PRIMARY KEY CLUSTERED ([QuestionId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for ExamQuestionBank
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[ExamQuestionBank]', RESEED, 3020)
GO


-- ----------------------------
-- Indexes structure for table ExamQuestionBank
-- ----------------------------
CREATE UNIQUE NONCLUSTERED INDEX [IX_ExamQuestionBank_BankCode]
ON [dbo].[ExamQuestionBank] (
  [BankCode] ASC
)
GO

CREATE NONCLUSTERED INDEX [IX_ExamQuestionBank_CreateBy]
ON [dbo].[ExamQuestionBank] (
  [CreateBy] ASC
)
GO

CREATE NONCLUSTERED INDEX [IX_ExamQuestionBank_CourseId]
ON [dbo].[ExamQuestionBank] (
  [CourseId] ASC
)
GO


-- ----------------------------
-- Primary Key structure for table ExamQuestionBank
-- ----------------------------
ALTER TABLE [dbo].[ExamQuestionBank] ADD CONSTRAINT [PK_ExamQuestionBank] PRIMARY KEY CLUSTERED ([BankId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for ExamResult
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[ExamResult]', RESEED, 1)
GO


-- ----------------------------
-- Indexes structure for table ExamResult
-- ----------------------------
CREATE NONCLUSTERED INDEX [IX_ExamResult_ExamId]
ON [dbo].[ExamResult] (
  [ExamId] ASC
)
GO

CREATE NONCLUSTERED INDEX [IX_ExamResult_StudentId]
ON [dbo].[ExamResult] (
  [StudentId] ASC
)
GO

CREATE NONCLUSTERED INDEX [IX_ExamResult_ClassId]
ON [dbo].[ExamResult] (
  [ClassId] ASC
)
GO


-- ----------------------------
-- Primary Key structure for table ExamResult
-- ----------------------------
ALTER TABLE [dbo].[ExamResult] ADD CONSTRAINT [PK_ExamResult] PRIMARY KEY CLUSTERED ([ResultId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for Exams
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[Exams]', RESEED, 1001)
GO


-- ----------------------------
-- Primary Key structure for table Exams
-- ----------------------------
ALTER TABLE [dbo].[Exams] ADD CONSTRAINT [PK__Exams__C1971B534455901F] PRIMARY KEY CLUSTERED ([Eid])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for ExamSurvey
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[ExamSurvey]', RESEED, 1)
GO


-- ----------------------------
-- Primary Key structure for table ExamSurvey
-- ----------------------------
ALTER TABLE [dbo].[ExamSurvey] ADD CONSTRAINT [PK_ExamSurvey] PRIMARY KEY CLUSTERED ([SurveyId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for ExamSurveyAnswer
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[ExamSurveyAnswer]', RESEED, 1)
GO


-- ----------------------------
-- Indexes structure for table ExamSurveyAnswer
-- ----------------------------
CREATE NONCLUSTERED INDEX [IX_ExamSurveyAnswer_SurveyId]
ON [dbo].[ExamSurveyAnswer] (
  [SurveyId] ASC
)
GO


-- ----------------------------
-- Primary Key structure for table ExamSurveyAnswer
-- ----------------------------
ALTER TABLE [dbo].[ExamSurveyAnswer] ADD CONSTRAINT [PK_ExamSurveyAnswer] PRIMARY KEY CLUSTERED ([AnswerId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for Files
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[Files]', RESEED, 1)
GO


-- ----------------------------
-- Primary Key structure for table Files
-- ----------------------------
ALTER TABLE [dbo].[Files] ADD CONSTRAINT [PK__Files__6F0F98BF08A39A03] PRIMARY KEY CLUSTERED ([FileId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for Flection
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[Flection]', RESEED, 1)
GO


-- ----------------------------
-- Primary Key structure for table Flection
-- ----------------------------
ALTER TABLE [dbo].[Flection] ADD CONSTRAINT [PK_Flection] PRIMARY KEY CLUSTERED ([Fid])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for Folders
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[Folders]', RESEED, 1)
GO


-- ----------------------------
-- Primary Key structure for table Folders
-- ----------------------------
ALTER TABLE [dbo].[Folders] ADD CONSTRAINT [PK__Folders__ACD7107F9349BE39] PRIMARY KEY CLUSTERED ([FolderId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for Game
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[Game]', RESEED, 5506)
GO


-- ----------------------------
-- Primary Key structure for table Game
-- ----------------------------
ALTER TABLE [dbo].[Game] ADD CONSTRAINT [PK__Game__C51E1336C0C0ACD7] PRIMARY KEY CLUSTERED ([Gid])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for Gauge
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[Gauge]', RESEED, 1)
GO


-- ----------------------------
-- Primary Key structure for table Gauge
-- ----------------------------
ALTER TABLE [dbo].[Gauge] ADD CONSTRAINT [PK__Gauge__C51E1336B7DDD26A] PRIMARY KEY CLUSTERED ([Gid])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for GaugeFeedback
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[GaugeFeedback]', RESEED, 1)
GO


-- ----------------------------
-- Primary Key structure for table GaugeFeedback
-- ----------------------------
ALTER TABLE [dbo].[GaugeFeedback] ADD CONSTRAINT [PK__GaugeFee__C1D1314A173B615F] PRIMARY KEY CLUSTERED ([Fid])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for GaugeItem
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[GaugeItem]', RESEED, 1)
GO


-- ----------------------------
-- Primary Key structure for table GaugeItem
-- ----------------------------
ALTER TABLE [dbo].[GaugeItem] ADD CONSTRAINT [PK__GaugeIte__C79638C212B5CE18] PRIMARY KEY CLUSTERED ([Mid])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for GroupWork
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[GroupWork]', RESEED, 1)
GO


-- ----------------------------
-- Primary Key structure for table GroupWork
-- ----------------------------
ALTER TABLE [dbo].[GroupWork] ADD CONSTRAINT [PK_GroupWork] PRIMARY KEY CLUSTERED ([Gid])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for HonorBoardSettings
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[HonorBoardSettings]', RESEED, 1)
GO


-- ----------------------------
-- Primary Key structure for table HonorBoardSettings
-- ----------------------------
ALTER TABLE [dbo].[HonorBoardSettings] ADD CONSTRAINT [PK_HonorBoardSettings] PRIMARY KEY CLUSTERED ([ID])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table HonorConfig
-- ----------------------------
ALTER TABLE [dbo].[HonorConfig] ADD CONSTRAINT [PK__HonorCon__A414B8D26ED3CB8B] PRIMARY KEY CLUSTERED ([HonorCode])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for House
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[House]', RESEED, 1)
GO


-- ----------------------------
-- Primary Key structure for table House
-- ----------------------------
ALTER TABLE [dbo].[House] ADD CONSTRAINT [PK__House__C750193F2E9BA0D4] PRIMARY KEY CLUSTERED ([Hid])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for Ip
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[Ip]', RESEED, 175)
GO


-- ----------------------------
-- Primary Key structure for table Ip
-- ----------------------------
ALTER TABLE [dbo].[Ip] ADD CONSTRAINT [PK__Ip__C4962F8465965489] PRIMARY KEY CLUSTERED ([Iid])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for JudgeArg
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[JudgeArg]', RESEED, 1)
GO


-- ----------------------------
-- Primary Key structure for table JudgeArg
-- ----------------------------
ALTER TABLE [dbo].[JudgeArg] ADD CONSTRAINT [PK__JudgeArg__C4D0354D5894D5DE] PRIMARY KEY CLUSTERED ([Jid])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for kechengbiao
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[kechengbiao]', RESEED, 1007)
GO


-- ----------------------------
-- Primary Key structure for table kechengbiao
-- ----------------------------
ALTER TABLE [dbo].[kechengbiao] ADD CONSTRAINT [PK__kechengbiao__3214EC27118145AF] PRIMARY KEY CLUSTERED ([Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for ListMenu
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[ListMenu]', RESEED, 12157)
GO


-- ----------------------------
-- Primary Key structure for table ListMenu
-- ----------------------------
ALTER TABLE [dbo].[ListMenu] ADD CONSTRAINT [PK__ListMenu__C6505B390612621B] PRIMARY KEY CLUSTERED ([Lid])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for MenuWorks
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[MenuWorks]', RESEED, 31096)
GO


-- ----------------------------
-- Primary Key structure for table MenuWorks
-- ----------------------------
ALTER TABLE [dbo].[MenuWorks] ADD CONSTRAINT [PK__MenuWork__DFDFDF3EB6EA5FC0] PRIMARY KEY CLUSTERED ([kid])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for Mission
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[Mission]', RESEED, 8150)
GO


-- ----------------------------
-- Primary Key structure for table Mission
-- ----------------------------
ALTER TABLE [dbo].[Mission] ADD CONSTRAINT [PK_Mission] PRIMARY KEY CLUSTERED ([Mid])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for NotSign
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[NotSign]', RESEED, 5094)
GO


-- ----------------------------
-- Primary Key structure for table NotSign
-- ----------------------------
ALTER TABLE [dbo].[NotSign] ADD CONSTRAINT [PK__NotSign__C7D1D6CB2E650AEE] PRIMARY KEY CLUSTERED ([Nid])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for Pchinese
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[Pchinese]', RESEED, 1)
GO


-- ----------------------------
-- Primary Key structure for table Pchinese
-- ----------------------------
ALTER TABLE [dbo].[Pchinese] ADD CONSTRAINT [PK__Pchinese__C570593867C47EF4] PRIMARY KEY CLUSTERED ([Pid])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for performance_score
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[performance_score]', RESEED, 1001)
GO


-- ----------------------------
-- Primary Key structure for table performance_score
-- ----------------------------
ALTER TABLE [dbo].[performance_score] ADD CONSTRAINT [PK__performa__3214EC072CBF9B5C] PRIMARY KEY CLUSTERED ([Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for Pfinger
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[Pfinger]', RESEED, 19530)
GO


-- ----------------------------
-- Primary Key structure for table Pfinger
-- ----------------------------
ALTER TABLE [dbo].[Pfinger] ADD CONSTRAINT [PK__Pfinger__C5705938D355F2D4] PRIMARY KEY CLUSTERED ([Pid])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for Problems
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[Problems]', RESEED, 1)
GO


-- ----------------------------
-- Primary Key structure for table Problems
-- ----------------------------
ALTER TABLE [dbo].[Problems] ADD CONSTRAINT [PK__Problems__C5705938950439F5] PRIMARY KEY CLUSTERED ([Pid])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for Ptyper
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[Ptyper]', RESEED, 3)
GO


-- ----------------------------
-- Primary Key structure for table Ptyper
-- ----------------------------
ALTER TABLE [dbo].[Ptyper] ADD CONSTRAINT [PK_Ptyper] PRIMARY KEY CLUSTERED ([Pid])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for Quiz
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[Quiz]', RESEED, 1)
GO


-- ----------------------------
-- Primary Key structure for table Quiz
-- ----------------------------
ALTER TABLE [dbo].[Quiz] ADD CONSTRAINT [PK_Quiz] PRIMARY KEY CLUSTERED ([Qid])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for QuizGrade
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[QuizGrade]', RESEED, 1)
GO


-- ----------------------------
-- Primary Key structure for table QuizGrade
-- ----------------------------
ALTER TABLE [dbo].[QuizGrade] ADD CONSTRAINT [PK__QuizGrad__CAB64A03ACE99E5E] PRIMARY KEY CLUSTERED ([Qid])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for Research
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[Research]', RESEED, 1)
GO


-- ----------------------------
-- Primary Key structure for table Research
-- ----------------------------
ALTER TABLE [dbo].[Research] ADD CONSTRAINT [PK__Research__CAF055CAD4A3A9A2] PRIMARY KEY CLUSTERED ([Rid])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for Result
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[Result]', RESEED, 1)
GO


-- ----------------------------
-- Primary Key structure for table Result
-- ----------------------------
ALTER TABLE [dbo].[Result] ADD CONSTRAINT [PK_Result] PRIMARY KEY CLUSTERED ([Rid])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for Room
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[Room]', RESEED, 2022)
GO


-- ----------------------------
-- Primary Key structure for table Room
-- ----------------------------
ALTER TABLE [dbo].[Room] ADD CONSTRAINT [PK_Room] PRIMARY KEY CLUSTERED ([Rid])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for ShareDisk
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[ShareDisk]', RESEED, 3)
GO


-- ----------------------------
-- Primary Key structure for table ShareDisk
-- ----------------------------
ALTER TABLE [dbo].[ShareDisk] ADD CONSTRAINT [PK__ShareDis__C41FDD701D46A062] PRIMARY KEY CLUSTERED ([Kid])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for Signin
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[Signin]', RESEED, 2027371)
GO


-- ----------------------------
-- Primary Key structure for table Signin
-- ----------------------------
ALTER TABLE [dbo].[Signin] ADD CONSTRAINT [PK_Signin] PRIMARY KEY CLUSTERED ([Qid])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for Skdj
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[Skdj]', RESEED, 34974)
GO


-- ----------------------------
-- Primary Key structure for table Skdj
-- ----------------------------
ALTER TABLE [dbo].[Skdj] ADD CONSTRAINT [PK_Skdj] PRIMARY KEY CLUSTERED ([Ssid])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for Soft
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[Soft]', RESEED, 1004)
GO


-- ----------------------------
-- Primary Key structure for table Soft
-- ----------------------------
ALTER TABLE [dbo].[Soft] ADD CONSTRAINT [PK_Soft] PRIMARY KEY CLUSTERED ([Fid])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for SoftCategory
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[SoftCategory]', RESEED, 1)
GO


-- ----------------------------
-- Primary Key structure for table SoftCategory
-- ----------------------------
ALTER TABLE [dbo].[SoftCategory] ADD CONSTRAINT [PK__SoftCate__D8B67DA3D2CF8432] PRIMARY KEY CLUSTERED ([Yid])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for Solves
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[Solves]', RESEED, 1)
GO


-- ----------------------------
-- Primary Key structure for table Solves
-- ----------------------------
ALTER TABLE [dbo].[Solves] ADD CONSTRAINT [PK__Solves__C5F0B44364F5B1A4] PRIMARY KEY CLUSTERED ([Vid])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for student_scores
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[student_scores]', RESEED, 10)
GO


-- ----------------------------
-- Primary Key structure for table student_scores
-- ----------------------------
ALTER TABLE [dbo].[student_scores] ADD CONSTRAINT [PK__student___3214EC276F2414DC] PRIMARY KEY CLUSTERED ([ID])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for StudentHonors
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[StudentHonors]', RESEED, 1)
GO


-- ----------------------------
-- Indexes structure for table StudentHonors
-- ----------------------------
CREATE NONCLUSTERED INDEX [IX_StudentHonors_EarnDate]
ON [dbo].[StudentHonors] (
  [EarnDate] ASC
)
GO

CREATE NONCLUSTERED INDEX [IX_StudentHonors_HonorCode]
ON [dbo].[StudentHonors] (
  [HonorCode] ASC
)
GO

CREATE NONCLUSTERED INDEX [IX_StudentHonors_Snum]
ON [dbo].[StudentHonors] (
  [Snum] ASC
)
GO


-- ----------------------------
-- Primary Key structure for table StudentHonors
-- ----------------------------
ALTER TABLE [dbo].[StudentHonors] ADD CONSTRAINT [PK__StudentH__3214EC2737440FCB] PRIMARY KEY CLUSTERED ([ID])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for Students
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[Students]', RESEED, 7431)
GO


-- ----------------------------
-- Indexes structure for table Students
-- ----------------------------
CREATE NONCLUSTERED INDEX [IX_Students_Sfixedip]
ON [dbo].[Students] (
  [Sfixedip] ASC
)
GO

CREATE NONCLUSTERED INDEX [IX_Students_Sseat]
ON [dbo].[Students] (
  [Sseat] ASC
)
GO

CREATE NONCLUSTERED INDEX [IX_Students_Grade_Class_Sfixedip]
ON [dbo].[Students] (
  [Sgrade] ASC,
  [Sclass] ASC,
  [Sfixedip] ASC
)
GO


-- ----------------------------
-- Primary Key structure for table Students
-- ----------------------------
ALTER TABLE [dbo].[Students] ADD CONSTRAINT [PK_Students] PRIMARY KEY CLUSTERED ([Sid])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for StudentsExcel
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[StudentsExcel]', RESEED, 3391)
GO


-- ----------------------------
-- Primary Key structure for table StudentsExcel
-- ----------------------------
ALTER TABLE [dbo].[StudentsExcel] ADD CONSTRAINT [PK_StudentsExcel] PRIMARY KEY CLUSTERED ([Sid])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for Summary
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[Summary]', RESEED, 1)
GO


-- ----------------------------
-- Primary Key structure for table Summary
-- ----------------------------
ALTER TABLE [dbo].[Summary] ADD CONSTRAINT [PK_Summary] PRIMARY KEY CLUSTERED ([Sid])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for Survey
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[Survey]', RESEED, 3011)
GO


-- ----------------------------
-- Primary Key structure for table Survey
-- ----------------------------
ALTER TABLE [dbo].[Survey] ADD CONSTRAINT [PK__Survey__C5F0B44329B610B4] PRIMARY KEY CLUSTERED ([Vid])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for SurveyClass
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[SurveyClass]', RESEED, 1008)
GO


-- ----------------------------
-- Primary Key structure for table SurveyClass
-- ----------------------------
ALTER TABLE [dbo].[SurveyClass] ADD CONSTRAINT [PK__SurveyCl__D8B67DA3DE486974] PRIMARY KEY CLUSTERED ([Yid])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for SurveyFeedback
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[SurveyFeedback]', RESEED, 5700)
GO


-- ----------------------------
-- Primary Key structure for table SurveyFeedback
-- ----------------------------
ALTER TABLE [dbo].[SurveyFeedback] ADD CONSTRAINT [PK__SurveyFe__C1D1314A11485904] PRIMARY KEY CLUSTERED ([Fid])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for SurveyItem
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[SurveyItem]', RESEED, 1192)
GO


-- ----------------------------
-- Primary Key structure for table SurveyItem
-- ----------------------------
ALTER TABLE [dbo].[SurveyItem] ADD CONSTRAINT [PK__SurveyIt__C79638C217C22007] PRIMARY KEY CLUSTERED ([Mid])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for SurveyQuestion
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[SurveyQuestion]', RESEED, 2063)
GO


-- ----------------------------
-- Primary Key structure for table SurveyQuestion
-- ----------------------------
ALTER TABLE [dbo].[SurveyQuestion] ADD CONSTRAINT [PK__SurveyQu__CAB64A0395BA7419] PRIMARY KEY CLUSTERED ([Qid])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for Teacher
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[Teacher]', RESEED, 3)
GO


-- ----------------------------
-- Primary Key structure for table Teacher
-- ----------------------------
ALTER TABLE [dbo].[Teacher] ADD CONSTRAINT [PK_Teacher] PRIMARY KEY CLUSTERED ([Hid])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for Teachers
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[Teachers]', RESEED, 1)
GO


-- ----------------------------
-- Primary Key structure for table Teachers
-- ----------------------------
ALTER TABLE [dbo].[Teachers] ADD CONSTRAINT [PK_Teachers] PRIMARY KEY CLUSTERED ([Tid])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for TempSeat
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[TempSeat]', RESEED, 1)
GO


-- ----------------------------
-- Indexes structure for table TempSeat
-- ----------------------------
CREATE NONCLUSTERED INDEX [IX_TempSeat_Snum]
ON [dbo].[TempSeat] (
  [Snum] ASC
)
GO

CREATE NONCLUSTERED INDEX [IX_TempSeat_ExpireTime]
ON [dbo].[TempSeat] (
  [ExpireTime] ASC
)
GO


-- ----------------------------
-- Primary Key structure for table TempSeat
-- ----------------------------
ALTER TABLE [dbo].[TempSeat] ADD CONSTRAINT [PK_TempSeat] PRIMARY KEY CLUSTERED ([Tid])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for TermTotal
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[TermTotal]', RESEED, 8455)
GO


-- ----------------------------
-- Primary Key structure for table TermTotal
-- ----------------------------
ALTER TABLE [dbo].[TermTotal] ADD CONSTRAINT [PK_TermTotal] PRIMARY KEY CLUSTERED ([Tid])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for ThemeSettings
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[ThemeSettings]', RESEED, 1001)
GO


-- ----------------------------
-- Primary Key structure for table ThemeSettings
-- ----------------------------
ALTER TABLE [dbo].[ThemeSettings] ADD CONSTRAINT [PK_ThemeSettings] PRIMARY KEY CLUSTERED ([SettingId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for TimeSlots
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[TimeSlots]', RESEED, 1001)
GO


-- ----------------------------
-- Primary Key structure for table TimeSlots
-- ----------------------------
ALTER TABLE [dbo].[TimeSlots] ADD CONSTRAINT [PK__TimeSlot__0A124A4FF980E907] PRIMARY KEY CLUSTERED ([SlotID])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for TopicDiscuss
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[TopicDiscuss]', RESEED, 1)
GO


-- ----------------------------
-- Primary Key structure for table TopicDiscuss
-- ----------------------------
ALTER TABLE [dbo].[TopicDiscuss] ADD CONSTRAINT [PK__TopicDis__C451DB31E719DE9B] PRIMARY KEY CLUSTERED ([Tid])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for TopicReply
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[TopicReply]', RESEED, 1)
GO


-- ----------------------------
-- Primary Key structure for table TopicReply
-- ----------------------------
ALTER TABLE [dbo].[TopicReply] ADD CONSTRAINT [PK__TopicRep__CAF055CADEE1F94B] PRIMARY KEY CLUSTERED ([Rid])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for Turtle
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[Turtle]', RESEED, 1)
GO


-- ----------------------------
-- Primary Key structure for table Turtle
-- ----------------------------
ALTER TABLE [dbo].[Turtle] ADD CONSTRAINT [PK__Turtle__C451DB3176DA7A51] PRIMARY KEY CLUSTERED ([Tid])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for TurtleAnswer
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[TurtleAnswer]', RESEED, 1)
GO


-- ----------------------------
-- Primary Key structure for table TurtleAnswer
-- ----------------------------
ALTER TABLE [dbo].[TurtleAnswer] ADD CONSTRAINT [PK__TurtleAn__C6970A10FD79058C] PRIMARY KEY CLUSTERED ([Aid])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for TurtleMatch
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[TurtleMatch]', RESEED, 1)
GO


-- ----------------------------
-- Primary Key structure for table TurtleMatch
-- ----------------------------
ALTER TABLE [dbo].[TurtleMatch] ADD CONSTRAINT [PK__TurtleMa__C79638C2D84E6B33] PRIMARY KEY CLUSTERED ([Mid])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for TurtleQuestion
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[TurtleQuestion]', RESEED, 1)
GO


-- ----------------------------
-- Primary Key structure for table TurtleQuestion
-- ----------------------------
ALTER TABLE [dbo].[TurtleQuestion] ADD CONSTRAINT [PK__TurtleQu__CAB64A03ECADD16B] PRIMARY KEY CLUSTERED ([Qid])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for TxtForm
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[TxtForm]', RESEED, 2001)
GO


-- ----------------------------
-- Primary Key structure for table TxtForm
-- ----------------------------
ALTER TABLE [dbo].[TxtForm] ADD CONSTRAINT [PK__TxtForm__C79638C242D5F7C0] PRIMARY KEY CLUSTERED ([Mid])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for TxtFormBack
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[TxtFormBack]', RESEED, 3018)
GO


-- ----------------------------
-- Primary Key structure for table TxtFormBack
-- ----------------------------
ALTER TABLE [dbo].[TxtFormBack] ADD CONSTRAINT [PK__TxtFormB__CAF055CADD92060A] PRIMARY KEY CLUSTERED ([Rid])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for Typer
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[Typer]', RESEED, 3)
GO


-- ----------------------------
-- Primary Key structure for table Typer
-- ----------------------------
ALTER TABLE [dbo].[Typer] ADD CONSTRAINT [PK_Typer] PRIMARY KEY CLUSTERED ([Tid])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for Works
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[Works]', RESEED, 8567)
GO


-- ----------------------------
-- Primary Key structure for table Works
-- ----------------------------
ALTER TABLE [dbo].[Works] ADD CONSTRAINT [PK_Works] PRIMARY KEY CLUSTERED ([Wid])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for WorksDiscuss
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[WorksDiscuss]', RESEED, 1)
GO


-- ----------------------------
-- Primary Key structure for table WorksDiscuss
-- ----------------------------
ALTER TABLE [dbo].[WorksDiscuss] ADD CONSTRAINT [PK_WorksDiscuss] PRIMARY KEY CLUSTERED ([Did])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Foreign Keys structure for table ExamQuestion
-- ----------------------------
ALTER TABLE [dbo].[ExamQuestion] ADD CONSTRAINT [FK_ExamQuestion_ExamQuestionBank] FOREIGN KEY ([BankId]) REFERENCES [dbo].[ExamQuestionBank] ([BankId]) ON DELETE CASCADE ON UPDATE NO ACTION
GO

