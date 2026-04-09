/****** Object:  Table [dbo].[WorksDiscuss]    Script Date: 03/29/2022 19:53:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WorksDiscuss](
	[Did] [int] IDENTITY(1,1) NOT NULL,
	[Dwid] [int] NULL,
	[Dsnum] [nvarchar](50) NULL,
	[Dwords] [ntext] NULL,
	[Dtime] [datetime] NULL,
	[Dip] [nvarchar](50) NULL,
	[Dsid] [int] NULL,
 CONSTRAINT [PK_WorksDiscuss] PRIMARY KEY CLUSTERED 
(
	[Did] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Works]    Script Date: 03/29/2022 19:53:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Works](
	[Wid] [int] IDENTITY(1,1) NOT NULL,
	[Wnum] [nvarchar](50) NULL,
	[Wcid] [int] NULL,
	[Wmid] [int] NULL,
	[Wmsort] [int] NULL,
	[Wfilename] [nvarchar](50) NULL,
	[Wurl] [nvarchar](200) NULL,
	[Wlength] [int] NULL,
	[Wscore] [int] NULL,
	[Wdate] [datetime] NULL,
	[Wip] [nvarchar](50) NULL,
	[Wtime] [nvarchar](50) NULL,
	[Wvote] [int] NULL,
	[Wegg] [smallint] NULL,
	[Wcheck] [bit] NULL,
	[Wself] [nvarchar](200) NULL,
	[Wcan] [bit] NULL,
	[Wgood] [bit] NULL,
	[Wtype] [nvarchar](50) NULL,
	[Wgrade] [int] NULL,
	[Wterm] [int] NULL,
	[Whit] [int] NULL,
	[Wlscore] [int] NULL,
	[Wlemotion] [int] NULL,
	[Woffice] [bit] NULL,
	[Wflash] [bit] NULL,
	[Werror] [bit] NULL,
	[Wfscore] [int] NULL,
	[Wclass] [int] NULL,
	[Wsid] [int] NULL,
	[Wname] [nvarchar](50) NULL,
	[Wyear] [int] NULL,
	[Wdscore] [int] NULL,
	[Wthumbnail] [nvarchar](200) NULL,
	[Wtitle] [nvarchar](200) NULL,
	[Weditday] [int] NULL,
	[Wdict] [ntext] NULL,
	[Wcode] [ntext] NULL,
	[Wpass] [bit] NULL,
 CONSTRAINT [PK_Works] PRIMARY KEY CLUSTERED 
(
	[Wid] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Typer]    Script Date: 03/29/2022 19:53:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Typer](
	[Tid] [int] IDENTITY(1,1) NOT NULL,
	[Ttype] [smallint] NULL,
	[Tuse] [int] NULL,
	[Ttitle] [nvarchar](100) NULL,
	[Tcontent] [ntext] NULL,
 CONSTRAINT [PK_Typer] PRIMARY KEY CLUSTERED 
(
	[Tid] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TxtFormBack]    Script Date: 03/29/2022 19:53:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TxtFormBack](
	[Rid] [int] IDENTITY(1,1) NOT NULL,
	[Rmid] [int] NULL,
	[Rsnum] [nvarchar](50) NULL,
	[Rsid] [int] NULL,
	[Rwords] [ntext] NULL,
	[Rtime] [datetime] NULL,
	[Rip] [nvarchar](50) NULL,
	[Rscore] [int] NULL,
	[Ryear] [int] NULL,
	[Rterm] [int] NULL,
	[Rgrade] [int] NULL,
	[Rclass] [int] NULL,
	[Ragree] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Rid] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TxtForm]    Script Date: 03/29/2022 19:53:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TxtForm](
	[Mid] [int] IDENTITY(1,1) NOT NULL,
	[Mtitle] [nvarchar](50) NULL,
	[Mcid] [int] NULL,
	[Mcontent] [ntext] NULL,
	[Mdate] [datetime] NULL,
	[Mhit] [int] NULL,
	[Mpublish] [bit] NULL,
	[Mdelete] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Mid] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TurtleQuestion]    Script Date: 03/29/2022 19:53:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TurtleQuestion](
	[Qid] [int] IDENTITY(1,1) NOT NULL,
	[Qmid] [int] NULL,
	[Qtitle] [nvarchar](50) NULL,
	[Qcontent] [ntext] NULL,
	[Qdegree] [int] NULL,
	[Qsort] [int] NULL,
	[Qcode] [ntext] NULL,
	[Qimg] [nvarchar](50) NULL,
	[Qurl] [nvarchar](50) NULL,
	[Qout] [nvarchar](200) NULL,
	[Qscore] [int] NULL,
	[Qdate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Qid] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TurtleMatch]    Script Date: 03/29/2022 19:53:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TurtleMatch](
	[Mid] [int] IDENTITY(1,1) NOT NULL,
	[Mhid] [int] NULL,
	[Mtitle] [nvarchar](50) NULL,
	[Mcontent] [ntext] NULL,
	[Mbegin] [datetime] NULL,
	[Mend] [datetime] NULL,
	[Mpublish] [bit] NULL,
	[Mdate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Mid] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TurtleAnswer]    Script Date: 03/29/2022 19:53:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TurtleAnswer](
	[Aid] [int] IDENTITY(1,1) NOT NULL,
	[Amid] [int] NULL,
	[Aqid] [int] NULL,
	[Acode] [ntext] NULL,
	[Aimg] [nvarchar](50) NULL,
	[Aurl] [nvarchar](50) NULL,
	[Aout] [nvarchar](200) NULL,
	[Ascore] [int] NULL,
	[Asid] [int] NULL,
	[Asname] [nvarchar](50) NULL,
	[Alock] [bit] NULL,
	[Adate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Aid] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Turtle]    Script Date: 03/29/2022 19:53:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Turtle](
	[Tid] [int] IDENTITY(1,1) NOT NULL,
	[Thid] [int] NULL,
	[Ttilte] [nvarchar](50) NULL,
	[Tcontent] [ntext] NULL,
	[Tdegree] [int] NULL,
	[Tsort] [int] NULL,
	[Tcode] [ntext] NULL,
	[Timg] [nvarchar](50) NULL,
	[Turl] [nvarchar](50) NULL,
	[Tout] [nvarchar](200) NULL,
	[Tdate] [datetime] NULL,
	[Tstudy] [bit] NULL,
	[Tsid] [int] NULL,
	[Tscore] [int] NULL,
	[Tip] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[Tid] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TopicReply]    Script Date: 03/29/2022 19:53:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TopicReply](
	[Rid] [int] IDENTITY(1,1) NOT NULL,
	[Rtid] [int] NULL,
	[Rsnum] [nvarchar](50) NULL,
	[Rwords] [ntext] NULL,
	[Rtime] [datetime] NULL,
	[Rip] [nvarchar](50) NULL,
	[Rscore] [int] NULL,
	[Rban] [bit] NULL,
	[Rgrade] [int] NULL,
	[Rterm] [int] NULL,
	[Rcid] [int] NULL,
	[Rclass] [int] NULL,
	[Rsid] [int] NULL,
	[Ryear] [int] NULL,
	[Redit] [bit] NULL,
	[Ragree] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Rid] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TopicDiscuss]    Script Date: 03/29/2022 19:53:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TopicDiscuss](
	[Tid] [int] IDENTITY(1,1) NOT NULL,
	[Tcid] [int] NULL,
	[Ttitle] [nvarchar](50) NULL,
	[Tcontent] [ntext] NULL,
	[Tcount] [int] NULL,
	[Tteacher] [int] NULL,
	[Tdate] [datetime] NULL,
	[Tclose] [bit] NULL,
	[Tresult] [ntext] NULL,
PRIMARY KEY CLUSTERED 
(
	[Tid] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TermTotal]    Script Date: 03/29/2022 19:53:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TermTotal](
	[Tid] [int] IDENTITY(1,1) NOT NULL,
	[Tnum] [nvarchar](50) NULL,
	[Tterm] [int] NULL,
	[Tgrade] [int] NULL,
	[Tscore] [int] NULL,
	[Tgscore] [int] NULL,
	[Tquiz] [int] NULL,
	[Tattitude] [int] NULL,
	[Twscore] [int] NULL,
	[Ttscore] [int] NULL,
	[Tpscore] [int] NULL,
	[Tallscore] [int] NULL,
	[Tape] [nvarchar](1) NULL,
	[Tfscore] [int] NULL,
	[Tvscore] [int] NULL,
	[Tsid] [int] NULL,
	[Tyear] [int] NULL,
	[Tclass] [int] NULL,
	[Tname] [nvarchar](50) NULL,
	[Ttxtform] [int] NULL,
	[Tchinese] [int] NULL,
 CONSTRAINT [PK_TermTotal] PRIMARY KEY CLUSTERED 
(
	[Tid] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Teacher]    Script Date: 03/29/2022 19:53:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Teacher](
	[Hid] [int] IDENTITY(1,1) NOT NULL,
	[Hname] [nvarchar](50) NULL,
	[Hpwd] [nvarchar](50) NULL,
	[Hpermiss] [bit] NULL,
	[Hnote] [ntext] NULL,
	[Hpath] [nvarchar](50) NULL,
	[Hdelete] [bit] NULL,
	[Hcount] [int] NULL,
	[Hnick] [nvarchar](50) NULL,
	[Hroom] [nvarchar](50) NULL,
 CONSTRAINT [PK_Teacher] PRIMARY KEY CLUSTERED 
(
	[Hid] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SurveyQuestion]    Script Date: 03/29/2022 19:53:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SurveyQuestion](
	[Qid] [int] IDENTITY(1,1) NOT NULL,
	[Qvid] [int] NULL,
	[Qcid] [int] NULL,
	[Qtitle] [ntext] NULL,
	[Qcount] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Qid] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SurveyItem]    Script Date: 03/29/2022 19:53:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SurveyItem](
	[Mid] [int] IDENTITY(1,1) NOT NULL,
	[Mqid] [int] NULL,
	[Mvid] [int] NULL,
	[Mitem] [ntext] NULL,
	[Mscore] [int] NULL,
	[Mcount] [int] NULL,
	[Mcid] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Mid] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SurveyFeedback]    Script Date: 03/29/2022 19:53:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SurveyFeedback](
	[Fid] [int] IDENTITY(1,1) NOT NULL,
	[Fnum] [nvarchar](50) NULL,
	[Fyear] [int] NULL,
	[Fgrade] [int] NULL,
	[Fclass] [int] NULL,
	[Fterm] [int] NULL,
	[Fcid] [int] NULL,
	[Fvid] [int] NULL,
	[Fvtype] [int] NULL,
	[Fselect] [ntext] NULL,
	[Fscore] [int] NULL,
	[Fdate] [datetime] NULL,
	[Fsid] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Fid] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SurveyClass]    Script Date: 03/29/2022 19:53:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SurveyClass](
	[Yid] [int] IDENTITY(1,1) NOT NULL,
	[Yyear] [int] NULL,
	[Ygrade] [int] NULL,
	[Yclass] [int] NULL,
	[Yterm] [int] NULL,
	[Ycid] [int] NULL,
	[Yvid] [int] NULL,
	[Yselect] [ntext] NULL,
	[Ycount] [ntext] NULL,
	[Yscore] [int] NULL,
	[Ydate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Yid] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Survey]    Script Date: 03/29/2022 19:53:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Survey](
	[Vid] [int] IDENTITY(1,1) NOT NULL,
	[Vcid] [int] NULL,
	[Vhid] [int] NULL,
	[Vtitle] [nvarchar](50) NULL,
	[Vcontent] [ntext] NULL,
	[Vtype] [int] NULL,
	[Vtotal] [int] NULL,
	[Vscore] [int] NULL,
	[Vaverage] [int] NULL,
	[Vclose] [bit] NULL,
	[Vpoint] [bit] NULL,
	[Vdate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Vid] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Summary]    Script Date: 03/29/2022 19:53:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Summary](
	[Sid] [int] IDENTITY(1,1) NOT NULL,
	[Scid] [int] NULL,
	[Smid] [int] NULL,
	[Shid] [int] NULL,
	[Scontent] [ntext] NULL,
	[Sdate] [datetime] NULL,
	[Sgrade] [int] NULL,
	[Sclass] [int] NULL,
	[Syear] [int] NULL,
	[Sshow] [bit] NULL,
 CONSTRAINT [PK_Summary] PRIMARY KEY CLUSTERED 
(
	[Sid] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StudentsExcel]    Script Date: 03/29/2022 19:53:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StudentsExcel](
	[Sid] [int] IDENTITY(1,1) NOT NULL,
	[Snum] [nvarchar](50) NULL,
	[Syear] [int] NULL,
	[Sgrade] [int] NULL,
	[Sclass] [int] NULL,
	[Sname] [nvarchar](50) NULL,
	[Spwd] [nvarchar](50) NULL,
	[Sex] [nvarchar](2) NULL,
	[Saddress] [nvarchar](200) NULL,
	[Sphone] [nvarchar](50) NOT NULL,
	[Sparents] [nvarchar](50) NULL,
	[Sheadtheacher] [nvarchar](50) NULL,
	[Sscore] [int] NULL,
	[Squiz] [int] NULL,
	[Sattitude] [int] NULL,
	[Sape] [nvarchar](1) NULL,
 CONSTRAINT [PK_StudentsExcel] PRIMARY KEY CLUSTERED 
(
	[Sid] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Students]    Script Date: 03/29/2022 19:53:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Students](
	[Sid] [int] IDENTITY(1,1) NOT NULL,
	[Snum] [nvarchar](50) NULL,
	[Syear] [int] NULL,
	[Sgrade] [int] NULL,
	[Sclass] [int] NULL,
	[Sname] [nvarchar](50) NULL,
	[Spwd] [nvarchar](50) NULL,
	[Sex] [nvarchar](2) NULL,
	[Saddress] [nvarchar](200) NULL,
	[Sphone] [nvarchar](50) NULL,
	[Sparents] [nvarchar](50) NULL,
	[Sheadtheacher] [nvarchar](50) NULL,
	[Sscore] [int] NULL,
	[Squiz] [int] NULL,
	[Sattitude] [int] NULL,
	[Sape] [nvarchar](10) NULL,
	[Swscore] [int] NULL,
	[Stscore] [int] NULL,
	[Sallscore] [int] NULL,
	[Spscore] [int] NULL,
	[Sgroup] [int] NULL,
	[Sleader] [bit] NULL,
	[Svote] [int] NULL,
	[Sgscore] [int] NULL,
	[Sfscore] [int] NULL,
	[Svscore] [int] NULL,
	[Sgtitle] [nvarchar](50) NULL,
	[Sascore] [int] NULL,
	[Skaoxu] [nvarchar](50) NULL,
	[Swdscore] [int] NULL,
	[Stxtform] [int] NULL,
	[Schinese] [int] NULL,
	[Stat] [bit] NULL,
	[Stenscore] [int] NULL,
	[Sidle] [int] NULL,
	[Sztype] [int] NULL,
 CONSTRAINT [PK_Students] PRIMARY KEY CLUSTERED 
(
	[Sid] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Solves]    Script Date: 03/29/2022 19:53:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Solves](
	[Vid] [int] IDENTITY(1,1) NOT NULL,
	[Vpid] [int] NULL,
	[Vsid] [int] NULL,
	[Vanswer] [nvarchar](200) NULL,
	[Vright] [bit] NULL,
	[Vscore] [int] NULL,
	[Vdate] [datetime] NULL,
	[Vgrade] [int] NULL,
	[Vterm] [int] NULL,
	[Vyear] [nvarchar](200) NULL,
	[Vnid] [int] NULL,
	[Vcid] [int] NULL,
	[Vclass] [nvarchar](200) NULL,
PRIMARY KEY CLUSTERED 
(
	[Vid] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SoftCategory]    Script Date: 03/29/2022 19:53:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SoftCategory](
	[Yid] [int] IDENTITY(1,1) NOT NULL,
	[Ysort] [int] NULL,
	[Ytitle] [nvarchar](50) NULL,
	[Ycontent] [nvarchar](200) NULL,
	[Yopen] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Yid] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Soft]    Script Date: 03/29/2022 19:53:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Soft](
	[Fid] [int] IDENTITY(1,1) NOT NULL,
	[Ftitle] [nvarchar](50) NULL,
	[Fcontent] [ntext] NULL,
	[Furl] [nvarchar](200) NULL,
	[Fhit] [int] NULL,
	[Fdate] [datetime] NULL,
	[Ffiletype] [nvarchar](50) NULL,
	[Fclass] [nvarchar](50) NULL,
	[Fhide] [bit] NULL,
	[Fopen] [int] NULL,
	[Fhid] [int] NULL,
	[Fyid] [int] NULL,
	[Fup] [bit] NULL,
 CONSTRAINT [PK_Soft] PRIMARY KEY CLUSTERED 
(
	[Fid] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Signin]    Script Date: 03/29/2022 19:53:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Signin](
	[Qid] [int] IDENTITY(1,1) NOT NULL,
	[Qnum] [nvarchar](50) NULL,
	[Qattitude] [int] NULL,
	[Qdate] [datetime] NULL,
	[Qyear] [int] NULL,
	[Qmonth] [int] NULL,
	[Qday] [int] NULL,
	[Qweek] [nvarchar](50) NULL,
	[Qip] [nvarchar](50) NULL,
	[Qmachine] [nvarchar](50) NULL,
	[Qnote] [nvarchar](50) NULL,
	[Qwork] [int] NULL,
	[Qgrade] [int] NULL,
	[Qterm] [int] NULL,
	[Qgroup] [nvarchar](50) NULL,
	[Qgscore] [int] NULL,
	[Qsid] [int] NULL,
	[Qname] [nvarchar](50) NULL,
	[Qclass] [int] NULL,
	[Qsyear] [int] NULL,
	[Qcid] [int] NULL,
 CONSTRAINT [PK_Signin] PRIMARY KEY CLUSTERED 
(
	[Qid] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ShareDisk]    Script Date: 03/29/2022 19:53:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ShareDisk](
	[Kid] [int] IDENTITY(1,1) NOT NULL,
	[Kown] [bit] NULL,
	[Kyear] [int] NULL,
	[Kgrade] [int] NULL,
	[Kclass] [int] NULL,
	[Kgroup] [int] NULL,
	[Knum] [nvarchar](50) NULL,
	[Kname] [nvarchar](50) NULL,
	[Kfilename] [nvarchar](50) NULL,
	[Kfsize] [int] NULL,
	[Kfurl] [nvarchar](200) NULL,
	[Kftpe] [nvarchar](50) NULL,
	[Kfdate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Kid] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Room]    Script Date: 03/29/2022 19:53:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Room](
	[Rid] [int] IDENTITY(1,1) NOT NULL,
	[Rhid] [int] NULL,
	[Rgrade] [int] NULL,
	[Rclass] [int] NULL,
	[Rset] [bit] NULL,
	[Rpwd] [nvarchar](50) NULL,
	[Rlock] [bit] NULL,
	[Rip] [nvarchar](50) NULL,
	[Rgauge] [bit] NULL,
	[RgroupMax] [int] NULL,
	[Rclassedit] [bit] NULL,
	[Rphotoedit] [bit] NULL,
	[Rsexedit] [bit] NULL,
	[Rnameedit] [bit] NULL,
	[Rcid] [int] NULL,
	[Ropen] [bit] NULL,
	[Rseat] [int] NULL,
	[Rshare] [bit] NULL,
	[Rpwdsee] [bit] NULL,
	[Rgroupshare] [bit] NULL,
	[Rtyper] [nvarchar](200) NULL,
	[Rreg] [bit] NULL,
	[Rchinese] [nvarchar](200) NULL,
	[Rscratch] [bit] NULL,
	[RLogin] [bit] NULL,
 CONSTRAINT [PK_Room] PRIMARY KEY CLUSTERED 
(
	[Rid] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Result]    Script Date: 03/29/2022 19:53:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Result](
	[Rid] [int] IDENTITY(1,1) NOT NULL,
	[Rnum] [nvarchar](50) NULL,
	[Rscore] [int] NULL,
	[Rdate] [datetime] NULL,
	[Rhistory] [ntext] NULL,
	[Rwrong] [ntext] NULL,
	[Rgrade] [int] NULL,
	[Rterm] [int] NULL,
	[Rsid] [int] NULL,
 CONSTRAINT [PK_Result] PRIMARY KEY CLUSTERED 
(
	[Rid] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Research]    Script Date: 03/29/2022 19:53:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Research](
	[Rid] [int] IDENTITY(1,1) NOT NULL,
	[Rsid] [int] NULL,
	[Ryear] [int] NULL,
	[Rgrade] [int] NULL,
	[Rclass] [int] NULL,
	[Rterm] [int] NULL,
	[Rlearn] [smallmoney] NULL,
	[Rplay] [smallmoney] NULL,
	[Rsleep] [smallmoney] NULL,
	[Rfree] [smallmoney] NULL,
	[Rdate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Rid] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[QuizGrade]    Script Date: 03/29/2022 19:53:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[QuizGrade](
	[Qid] [int] IDENTITY(1,1) NOT NULL,
	[Qobj] [int] NULL,
	[Qclass] [ntext] NULL,
	[Qhid] [int] NULL,
	[Qonly] [int] NULL,
	[Qmore] [int] NULL,
	[Qjudge] [int] NULL,
	[Qopen] [bit] NULL,
	[Qanswer] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Qid] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Quiz]    Script Date: 03/29/2022 19:53:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Quiz](
	[Qid] [int] IDENTITY(1,1) NOT NULL,
	[Qtype] [int] NULL,
	[Question] [ntext] NULL,
	[Qanswer] [nvarchar](50) NULL,
	[Qanalyze] [nvarchar](50) NULL,
	[Qscore] [int] NULL,
	[Qclass] [nvarchar](50) NULL,
	[Qselect] [bit] NULL,
	[Qright] [int] NULL,
	[Qwrong] [int] NULL,
	[Qaccuracy] [int] NULL,
 CONSTRAINT [PK_Quiz] PRIMARY KEY CLUSTERED 
(
	[Qid] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Ptyper]    Script Date: 03/29/2022 19:53:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ptyper](
	[Pid] [int] IDENTITY(1,1) NOT NULL,
	[Ptid] [int] NULL,
	[Psnum] [nvarchar](50) NULL,
	[Pscore] [int] NULL,
	[Pdate] [datetime] NULL,
	[Pip] [nvarchar](50) NULL,
	[Ptype] [int] NULL,
	[Pdegree] [int] NULL,
	[Pgrade] [int] NULL,
	[Pterm] [int] NULL,
	[Psid] [int] NULL,
 CONSTRAINT [PK_Ptyper] PRIMARY KEY CLUSTERED 
(
	[Pid] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Problems]    Script Date: 03/29/2022 19:53:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Problems](
	[Pid] [int] IDENTITY(1,1) NOT NULL,
	[Phid] [int] NULL,
	[Pnid] [int] NULL,
	[Ptitle] [nvarchar](200) NULL,
	[Pcode] [nvarchar](200) NULL,
	[Pouput] [nvarchar](200) NULL,
	[Pscore] [int] NULL,
	[Pdate] [datetime] NULL,
	[Psort] [int] NULL,
	[Pcid] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Pid] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Pfinger]    Script Date: 03/29/2022 19:53:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pfinger](
	[Pid] [int] IDENTITY(1,1) NOT NULL,
	[Psnum] [nvarchar](50) NULL,
	[Pspd] [decimal](18, 2) NULL,
	[Pyear] [int] NULL,
	[Pmonth] [int] NULL,
	[Pdate] [datetime] NULL,
	[Pdegree] [int] NULL,
	[Pgrade] [int] NULL,
	[Pterm] [int] NULL,
	[Psid] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Pid] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Pchinese]    Script Date: 03/29/2022 19:53:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pchinese](
	[Pid] [int] IDENTITY(1,1) NOT NULL,
	[Psid] [int] NULL,
	[Psnum] [nvarchar](50) NULL,
	[Papple] [int] NULL,
	[Ptotal] [int] NULL,
	[Pspeed] [int] NULL,
	[Pdegree] [int] NULL,
	[Pyear] [int] NULL,
	[Pgrade] [int] NULL,
	[Pclass] [int] NULL,
	[Pterm] [int] NULL,
	[Pdate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Pid] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NotSign]    Script Date: 03/29/2022 19:53:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NotSign](
	[Nid] [int] IDENTITY(1,1) NOT NULL,
	[Nnum] [nvarchar](50) NULL,
	[Ndate] [datetime] NULL,
	[Nyear] [int] NULL,
	[Nmonth] [int] NULL,
	[Nday] [int] NULL,
	[Nweek] [nvarchar](50) NULL,
	[Nnote] [ntext] NULL,
	[Ngrade] [int] NULL,
	[Nterm] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Nid] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Mission]    Script Date: 03/29/2022 19:53:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Mission](
	[Mid] [int] IDENTITY(1,1) NOT NULL,
	[Mtitle] [nvarchar](50) NULL,
	[Mcid] [int] NULL,
	[Mcontent] [ntext] NULL,
	[Mdate] [datetime] NULL,
	[Mhit] [int] NULL,
	[Mfiletype] [nvarchar](50) NULL,
	[Mupload] [bit] NULL,
	[Msort] [int] NULL,
	[Mpublish] [bit] NULL,
	[Mgroup] [bit] NULL,
	[Mgid] [int] NULL,
	[Mdelete] [bit] NULL,
	[Mexample] [nvarchar](500) NULL,
	[Mcategory] [int] NULL,
	[Microworld] [bit] NULL,
	[Mback] [nvarchar](200) NULL,
 CONSTRAINT [PK_Mission] PRIMARY KEY CLUSTERED 
(
	[Mid] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ListMenu]    Script Date: 03/29/2022 19:53:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ListMenu](
	[Lid] [int] IDENTITY(1,1) NOT NULL,
	[Lcid] [int] NULL,
	[Lsort] [int] NULL,
	[Ltype] [int] NULL,
	[Lxid] [int] NULL,
	[Lshow] [bit] NULL,
	[Ltitle] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[Lid] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[JudgeArg]    Script Date: 03/29/2022 19:53:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JudgeArg](
	[Jid] [int] IDENTITY(1,1) NOT NULL,
	[Jhid] [int] NULL,
	[Jmid] [int] NULL,
	[Jsleep] [int] NULL,
	[Jinone] [nvarchar](50) NULL,
	[Jintwo] [nvarchar](50) NULL,
	[Jinthree] [nvarchar](50) NULL,
	[Joutone] [nvarchar](200) NULL,
	[Joutwo] [nvarchar](200) NULL,
	[Jouthree] [nvarchar](200) NULL,
	[Jright] [bit] NULL,
	[Jcode] [ntext] NULL,
	[Jcid] [int] NULL,
	[Jimg] [nvarchar](200) NULL,
PRIMARY KEY CLUSTERED 
(
	[Jid] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Ip]    Script Date: 03/29/2022 19:53:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ip](
	[Iid] [int] IDENTITY(1,1) NOT NULL,
	[Ihid] [int] NULL,
	[Inum] [int] NULL,
	[Iip] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[Iid] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[House]    Script Date: 03/29/2022 19:53:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[House](
	[Hid] [int] IDENTITY(1,1) NOT NULL,
	[Hname] [nvarchar](50) NULL,
	[Hseat] [ntext] NULL,
PRIMARY KEY CLUSTERED 
(
	[Hid] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GroupWork]    Script Date: 03/29/2022 19:53:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GroupWork](
	[Gid] [int] IDENTITY(1,1) NOT NULL,
	[Gnum] [nvarchar](50) NULL,
	[Gstudents] [nvarchar](200) NULL,
	[Gterm] [int] NULL,
	[Ggrade] [int] NULL,
	[Gclass] [int] NULL,
	[Gcid] [int] NULL,
	[Gmid] [int] NULL,
	[Gfilename] [nvarchar](50) NULL,
	[Gtype] [nvarchar](50) NULL,
	[Gurl] [nvarchar](200) NULL,
	[Glengh] [int] NULL,
	[Gscore] [int] NULL,
	[Gtime] [int] NULL,
	[Gvote] [int] NULL,
	[Gcheck] [bit] NULL,
	[Gnote] [ntext] NULL,
	[Grank] [int] NULL,
	[Ghit] [int] NULL,
	[Gip] [nvarchar](50) NULL,
	[Gdate] [datetime] NULL,
	[Ggroup] [int] NULL,
 CONSTRAINT [PK_GroupWork] PRIMARY KEY CLUSTERED 
(
	[Gid] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GaugeItem]    Script Date: 03/29/2022 19:53:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GaugeItem](
	[Mid] [int] IDENTITY(1,1) NOT NULL,
	[Mgid] [int] NULL,
	[Mitem] [nvarchar](50) NULL,
	[Mscore] [int] NULL,
	[Msort] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Mid] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GaugeFeedback]    Script Date: 03/29/2022 19:53:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GaugeFeedback](
	[Fid] [int] IDENTITY(1,1) NOT NULL,
	[Fnum] [nvarchar](50) NULL,
	[Fgrade] [int] NULL,
	[Fclass] [int] NULL,
	[Fcid] [int] NULL,
	[Fmid] [int] NULL,
	[Fwid] [int] NULL,
	[Fgid] [int] NULL,
	[Fselect] [nvarchar](50) NULL,
	[Fscore] [int] NULL,
	[Fgood] [bit] NULL,
	[Fdate] [datetime] NULL,
	[Fsid] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Fid] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Gauge]    Script Date: 03/29/2022 19:53:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Gauge](
	[Gid] [int] IDENTITY(1,1) NOT NULL,
	[Ghid] [int] NULL,
	[Gtype] [nvarchar](50) NULL,
	[Gtitle] [nvarchar](50) NULL,
	[Gcount] [int] NULL,
	[Gdate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Gid] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Game]    Script Date: 03/29/2022 19:53:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Game](
	[Gid] [int] IDENTITY(1,1) NOT NULL,
	[Gsid] [int] NULL,
	[Gsname] [nvarchar](50) NULL,
	[Gnum] [int] NULL,
	[Gtitle] [nvarchar](50) NULL,
	[Gsave] [int] NULL,
	[Gnote] [ntext] NULL,
	[Gscore] [int] NULL,
	[Gdate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Gid] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Flection]    Script Date: 03/29/2022 19:53:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Flection](
	[Fid] [int] IDENTITY(1,1) NOT NULL,
	[Fcid] [int] NULL,
	[Fhid] [int] NULL,
	[Fcontent] [ntext] NULL,
	[Fdate] [datetime] NULL,
 CONSTRAINT [PK_Flection] PRIMARY KEY CLUSTERED 
(
	[Fid] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[English]    Script Date: 03/29/2022 19:53:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[English](
	[Eid] [int] IDENTITY(1,1) NOT NULL,
	[Eword] [nvarchar](50) NULL,
	[Emeaning] [ntext] NULL,
	[Elevel] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Eid] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DelStudents]    Script Date: 03/29/2022 19:53:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DelStudents](
	[Did] [int] IDENTITY(1,1) NOT NULL,
	[Dnum] [nvarchar](50) NULL,
	[Dyear] [int] NULL,
	[Dgrade] [int] NULL,
	[Dclass] [int] NULL,
	[Dname] [nvarchar](50) NULL,
	[Dsex] [nvarchar](2) NULL,
	[Daddress] [nvarchar](200) NULL,
	[Dphone] [nvarchar](50) NULL,
	[Dparents] [nvarchar](50) NULL,
	[Dheadtheacher] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[Did] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Courses]    Script Date: 03/29/2022 19:53:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Courses](
	[Cid] [int] IDENTITY(1,1) NOT NULL,
	[Ctitle] [nvarchar](50) NULL,
	[Cclass] [nvarchar](50) NULL,
	[Ccontent] [ntext] NULL,
	[Cdate] [datetime] NULL,
	[Chit] [int] NULL,
	[Cobj] [int] NULL,
	[Cterm] [int] NULL,
	[Cks] [int] NULL,
	[Cfiletype] [nvarchar](50) NULL,
	[Cupload] [bit] NULL,
	[Chid] [int] NULL,
	[Cpublish] [bit] NULL,
	[Cdelete] [bit] NULL,
	[Cgood] [bit] NULL,
	[Cold] [bit] NULL,
 CONSTRAINT [PK_Courses] PRIMARY KEY CLUSTERED 
(
	[Cid] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Consoles]    Script Date: 03/29/2022 19:53:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Consoles](
	[Nid] [int] IDENTITY(1,1) NOT NULL,
	[Nhid] [int] NULL,
	[Ncid] [int] NULL,
	[Ntitle] [nvarchar](50) NULL,
	[Ncontent] [ntext] NULL,
	[Npublish] [bit] NULL,
	[Ndate] [datetime] NULL,
	[Nbegin] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Nid] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Computers]    Script Date: 03/29/2022 19:53:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Computers](
	[Pid] [int] IDENTITY(1,1) NOT NULL,
	[Pip] [nvarchar](50) NULL,
	[Pmachine] [nvarchar](50) NULL,
	[Plock] [bit] NULL,
	[Pdate] [datetime] NULL,
	[Px] [int] NULL,
	[Py] [int] NULL,
	[Pm] [nvarchar](50) NULL,
	[Pnum] [nvarchar](200) NULL,
	[Pon] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Pid] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Chinese]    Script Date: 03/29/2022 19:53:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Chinese](
	[Nid] [int] IDENTITY(1,1) NOT NULL,
	[Ntitle] [nvarchar](50) NULL,
	[Ncontent] [ntext] NULL,
PRIMARY KEY CLUSTERED 
(
	[Nid] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Autonomic]    Script Date: 03/29/2022 19:53:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Autonomic](
	[Aid] [int] IDENTITY(1,1) NOT NULL,
	[Asid] [int] NULL,
	[Anum] [nvarchar](50) NULL,
	[Aname] [nvarchar](50) NULL,
	[Ayid] [int] NULL,
	[Afid] [int] NULL,
	[Atype] [nvarchar](50) NULL,
	[Afilename] [nvarchar](50) NULL,
	[Aurl] [nvarchar](200) NULL,
	[Alength] [int] NULL,
	[Ascore] [int] NULL,
	[Adate] [datetime] NULL,
	[Aip] [nvarchar](50) NULL,
	[Avote] [int] NULL,
	[Aegg] [int] NULL,
	[Acheck] [bit] NULL,
	[Aself] [nvarchar](200) NULL,
	[Agood] [bit] NULL,
	[Ayear] [int] NULL,
	[Agrade] [int] NULL,
	[Aclass] [int] NULL,
	[Aterm] [int] NULL,
	[Ahit] [int] NULL,
	[Aoffice] [bit] NULL,
	[Aflash] [bit] NULL,
	[Aerror] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Aid] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Default [DF__WorksDiscu__Dsid__06CD04F7]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[WorksDiscuss] ADD  DEFAULT ((0)) FOR [Dsid]
GO
/****** Object:  Default [DF_Works_Wscore]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Works] ADD  CONSTRAINT [DF_Works_Wscore]  DEFAULT ((0)) FOR [Wscore]
GO
/****** Object:  Default [DF_Works_Wvote]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Works] ADD  CONSTRAINT [DF_Works_Wvote]  DEFAULT ((0)) FOR [Wvote]
GO
/****** Object:  Default [DF_Works_Wegg]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Works] ADD  CONSTRAINT [DF_Works_Wegg]  DEFAULT ((1)) FOR [Wegg]
GO
/****** Object:  Default [DF_Works_Wcheck]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Works] ADD  CONSTRAINT [DF_Works_Wcheck]  DEFAULT ((0)) FOR [Wcheck]
GO
/****** Object:  Default [DF_Works_Wcan]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Works] ADD  CONSTRAINT [DF_Works_Wcan]  DEFAULT ((1)) FOR [Wcan]
GO
/****** Object:  Default [DF_Works_Wgood]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Works] ADD  CONSTRAINT [DF_Works_Wgood]  DEFAULT ((0)) FOR [Wgood]
GO
/****** Object:  Default [DF__Works__Whit__0D7A0286]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Works] ADD  DEFAULT ((0)) FOR [Whit]
GO
/****** Object:  Default [DF__Works__Wlscore__0E6E26BF]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Works] ADD  DEFAULT ((0)) FOR [Wlscore]
GO
/****** Object:  Default [DF__Works__Wlemotion__0F624AF8]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Works] ADD  DEFAULT ((0)) FOR [Wlemotion]
GO
/****** Object:  Default [DF__Works__Woffice__10566F31]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Works] ADD  DEFAULT ((0)) FOR [Woffice]
GO
/****** Object:  Default [DF__Works__Wflash__114A936A]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Works] ADD  DEFAULT ((0)) FOR [Wflash]
GO
/****** Object:  Default [DF__Works__Werror__123EB7A3]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Works] ADD  DEFAULT ((0)) FOR [Werror]
GO
/****** Object:  Default [DF__Works__Wfscore__1332DBDC]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Works] ADD  DEFAULT ((0)) FOR [Wfscore]
GO
/****** Object:  Default [DF__Works__Wclass__14270015]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Works] ADD  DEFAULT ((0)) FOR [Wclass]
GO
/****** Object:  Default [DF__Works__Wsid__151B244E]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Works] ADD  DEFAULT ((0)) FOR [Wsid]
GO
/****** Object:  Default [DF__Works__Wyear__160F4887]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Works] ADD  DEFAULT ((0)) FOR [Wyear]
GO
/****** Object:  Default [DF__Works__Wdscore__17036CC0]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Works] ADD  DEFAULT ((0)) FOR [Wdscore]
GO
/****** Object:  Default [DF__Works__Weditday__19AACF41]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Works] ADD  DEFAULT ((0)) FOR [Weditday]
GO
/****** Object:  Default [DF__Works__Wpass__4A4E069C]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Works] ADD  DEFAULT ((0)) FOR [Wpass]
GO
/****** Object:  Default [DF__TxtFormBa__Rscor__17F790F9]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[TxtFormBack] ADD  DEFAULT ((0)) FOR [Rscore]
GO
/****** Object:  Default [DF__TxtForm__Mpublis__18EBB532]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[TxtForm] ADD  DEFAULT ((0)) FOR [Mpublish]
GO
/****** Object:  Default [DF__TxtForm__Mdelete__19DFD96B]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[TxtForm] ADD  DEFAULT ((0)) FOR [Mdelete]
GO
/****** Object:  Default [DF__TurtleMat__Mpubl__640DD89F]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[TurtleMatch] ADD  DEFAULT ((0)) FOR [Mpublish]
GO
/****** Object:  Default [DF__TurtleAns__Alock__5F492382]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[TurtleAnswer] ADD  DEFAULT ((0)) FOR [Alock]
GO
/****** Object:  Default [DF__Turtle__Tstudy__56B3DD81]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Turtle] ADD  DEFAULT ((0)) FOR [Tstudy]
GO
/****** Object:  Default [DF__TopicReply__Rban__1AD3FDA4]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[TopicReply] ADD  DEFAULT ((0)) FOR [Rban]
GO
/****** Object:  Default [DF__TopicReply__Rcid__1BC821DD]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[TopicReply] ADD  DEFAULT ((0)) FOR [Rcid]
GO
/****** Object:  Default [DF__TopicRepl__Rclas__1CBC4616]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[TopicReply] ADD  DEFAULT ((0)) FOR [Rclass]
GO
/****** Object:  Default [DF__TopicReply__Rsid__1DB06A4F]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[TopicReply] ADD  DEFAULT ((0)) FOR [Rsid]
GO
/****** Object:  Default [DF__TopicRepl__Ryear__1EA48E88]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[TopicReply] ADD  DEFAULT ((0)) FOR [Ryear]
GO
/****** Object:  Default [DF__TopicRepl__Redit__1F98B2C1]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[TopicReply] ADD  DEFAULT ((0)) FOR [Redit]
GO
/****** Object:  Default [DF__TopicRepl__Ragre__208CD6FA]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[TopicReply] ADD  DEFAULT ((0)) FOR [Ragree]
GO
/****** Object:  Default [DF__TopicDisc__Tcoun__2180FB33]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[TopicDiscuss] ADD  DEFAULT ((0)) FOR [Tcount]
GO
/****** Object:  Default [DF__TopicDisc__Tclos__22751F6C]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[TopicDiscuss] ADD  DEFAULT ((0)) FOR [Tclose]
GO
/****** Object:  Default [DF__TermTotal__Tfsco__236943A5]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[TermTotal] ADD  DEFAULT ((0)) FOR [Tfscore]
GO
/****** Object:  Default [DF__TermTotal__Tvsco__245D67DE]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[TermTotal] ADD  DEFAULT ((0)) FOR [Tvscore]
GO
/****** Object:  Default [DF__TermTotal__Tsid__25518C17]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[TermTotal] ADD  DEFAULT ((0)) FOR [Tsid]
GO
/****** Object:  Default [DF__TermTotal__Ttxtf__2645B050]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[TermTotal] ADD  DEFAULT ((0)) FOR [Ttxtform]
GO
/****** Object:  Default [DF__TermTotal__Tchin__2739D489]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[TermTotal] ADD  DEFAULT ((0)) FOR [Tchinese]
GO
/****** Object:  Default [DF_Teacher_Hpermiss]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Teacher] ADD  CONSTRAINT [DF_Teacher_Hpermiss]  DEFAULT ((0)) FOR [Hpermiss]
GO
/****** Object:  Default [DF__Teacher__Hdelete__29221CFB]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Teacher] ADD  DEFAULT ((0)) FOR [Hdelete]
GO
/****** Object:  Default [DF__Teacher__Hcount__2A164134]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Teacher] ADD  DEFAULT ((0)) FOR [Hcount]
GO
/****** Object:  Default [DF__SurveyQue__Qcoun__2B0A656D]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[SurveyQuestion] ADD  DEFAULT ((0)) FOR [Qcount]
GO
/****** Object:  Default [DF__SurveyIte__Mscor__2BFE89A6]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[SurveyItem] ADD  DEFAULT ((0)) FOR [Mscore]
GO
/****** Object:  Default [DF__SurveyIte__Mcoun__2CF2ADDF]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[SurveyItem] ADD  DEFAULT ((0)) FOR [Mcount]
GO
/****** Object:  Default [DF__SurveyFee__Fvtyp__2DE6D218]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[SurveyFeedback] ADD  DEFAULT ((0)) FOR [Fvtype]
GO
/****** Object:  Default [DF__SurveyFee__Fscor__2EDAF651]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[SurveyFeedback] ADD  DEFAULT ((0)) FOR [Fscore]
GO
/****** Object:  Default [DF__SurveyFeed__Fsid__2FCF1A8A]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[SurveyFeedback] ADD  DEFAULT ((0)) FOR [Fsid]
GO
/****** Object:  Default [DF__Survey__Vtype__30C33EC3]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Survey] ADD  DEFAULT ((0)) FOR [Vtype]
GO
/****** Object:  Default [DF__Survey__Vtotal__31B762FC]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Survey] ADD  DEFAULT ((0)) FOR [Vtotal]
GO
/****** Object:  Default [DF__Survey__Vscore__32AB8735]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Survey] ADD  DEFAULT ((0)) FOR [Vscore]
GO
/****** Object:  Default [DF__Survey__Vclose__339FAB6E]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Survey] ADD  DEFAULT ((0)) FOR [Vclose]
GO
/****** Object:  Default [DF__Survey__Vpoint__3493CFA7]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Survey] ADD  DEFAULT ((0)) FOR [Vpoint]
GO
/****** Object:  Default [DF_Summary_Sshow]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Summary] ADD  CONSTRAINT [DF_Summary_Sshow]  DEFAULT ((1)) FOR [Sshow]
GO
/****** Object:  Default [DF_StudentsExcel_Sscore]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[StudentsExcel] ADD  CONSTRAINT [DF_StudentsExcel_Sscore]  DEFAULT ((0)) FOR [Sscore]
GO
/****** Object:  Default [DF_StudentsExcel_Squiz]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[StudentsExcel] ADD  CONSTRAINT [DF_StudentsExcel_Squiz]  DEFAULT ((0)) FOR [Squiz]
GO
/****** Object:  Default [DF_StudentsExcel_Sattitude]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[StudentsExcel] ADD  CONSTRAINT [DF_StudentsExcel_Sattitude]  DEFAULT ((0)) FOR [Sattitude]
GO
/****** Object:  Default [DF_Students_Sscore]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Students] ADD  CONSTRAINT [DF_Students_Sscore]  DEFAULT ((0)) FOR [Sscore]
GO
/****** Object:  Default [DF_Students_Squiz]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Students] ADD  CONSTRAINT [DF_Students_Squiz]  DEFAULT ((0)) FOR [Squiz]
GO
/****** Object:  Default [DF_Students_Sattitude]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Students] ADD  CONSTRAINT [DF_Students_Sattitude]  DEFAULT ((0)) FOR [Sattitude]
GO
/****** Object:  Default [DF__Students__Swscor__0AD2A005]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Students] ADD  CONSTRAINT [DF__Students__Swscor__0AD2A005]  DEFAULT ((0)) FOR [Swscore]
GO
/****** Object:  Default [DF__Students__Stscor__0BC6C43E]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Students] ADD  CONSTRAINT [DF__Students__Stscor__0BC6C43E]  DEFAULT ((0)) FOR [Stscore]
GO
/****** Object:  Default [DF__Students__Sallsc__0CBAE877]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Students] ADD  CONSTRAINT [DF__Students__Sallsc__0CBAE877]  DEFAULT ((0)) FOR [Sallscore]
GO
/****** Object:  Default [DF__Students__Spscor__3F115E1A]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Students] ADD  DEFAULT ((0)) FOR [Spscore]
GO
/****** Object:  Default [DF__Students__Sgroup__40058253]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Students] ADD  DEFAULT ((0)) FOR [Sgroup]
GO
/****** Object:  Default [DF__Students__Sleade__40F9A68C]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Students] ADD  DEFAULT ((0)) FOR [Sleader]
GO
/****** Object:  Default [DF__Students__Svote__41EDCAC5]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Students] ADD  DEFAULT ((0)) FOR [Svote]
GO
/****** Object:  Default [DF__Students__Sfscor__42E1EEFE]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Students] ADD  DEFAULT ((0)) FOR [Sfscore]
GO
/****** Object:  Default [DF__Students__Svscor__43D61337]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Students] ADD  DEFAULT ((0)) FOR [Svscore]
GO
/****** Object:  Default [DF__Students__Sascor__44CA3770]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Students] ADD  DEFAULT ((0)) FOR [Sascore]
GO
/****** Object:  Default [DF__Students__Swdsco__45BE5BA9]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Students] ADD  DEFAULT ((0)) FOR [Swdscore]
GO
/****** Object:  Default [DF__Students__Stxtfo__46B27FE2]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Students] ADD  DEFAULT ((0)) FOR [Stxtform]
GO
/****** Object:  Default [DF__Students__Schine__47A6A41B]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Students] ADD  DEFAULT ((0)) FOR [Schinese]
GO
/****** Object:  Default [DF__Students__Stat__18B6AB08]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Students] ADD  DEFAULT ((0)) FOR [Stat]
GO
/****** Object:  Default [DF__Students__Stensc__1A9EF37A]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Students] ADD  DEFAULT ((0)) FOR [Stenscore]
GO
/****** Object:  Default [DF__Students__Sidle__4B422AD5]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Students] ADD  DEFAULT ((0)) FOR [Sidle]
GO
/****** Object:  Default [DF__Students__Sztype__4C364F0E]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Students] ADD  DEFAULT ((0)) FOR [Sztype]
GO
/****** Object:  Default [DF__Solves__Vright__3DE82FB7]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Solves] ADD  DEFAULT ((0)) FOR [Vright]
GO
/****** Object:  Default [DF__SoftCateg__Yopen__489AC854]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[SoftCategory] ADD  DEFAULT ((0)) FOR [Yopen]
GO
/****** Object:  Default [DF_Soft_Fhit]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Soft] ADD  CONSTRAINT [DF_Soft_Fhit]  DEFAULT ((0)) FOR [Fhit]
GO
/****** Object:  Default [DF__Soft__Fhide__4A8310C6]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Soft] ADD  DEFAULT ((0)) FOR [Fhide]
GO
/****** Object:  Default [DF__Soft__Fopen__4B7734FF]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Soft] ADD  DEFAULT ((0)) FOR [Fopen]
GO
/****** Object:  Default [DF__Soft__Fhid__4C6B5938]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Soft] ADD  DEFAULT ((0)) FOR [Fhid]
GO
/****** Object:  Default [DF__Soft__Fyid__4D5F7D71]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Soft] ADD  DEFAULT ((1)) FOR [Fyid]
GO
/****** Object:  Default [DF__Soft__Fup__4E53A1AA]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Soft] ADD  DEFAULT ((0)) FOR [Fup]
GO
/****** Object:  Default [DF_Signin_Qattitude]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Signin] ADD  CONSTRAINT [DF_Signin_Qattitude]  DEFAULT ((0)) FOR [Qattitude]
GO
/****** Object:  Default [DF_Signin_Qwork]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Signin] ADD  CONSTRAINT [DF_Signin_Qwork]  DEFAULT ((0)) FOR [Qwork]
GO
/****** Object:  Default [DF__Signin__Qgscore__51300E55]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Signin] ADD  DEFAULT ((0)) FOR [Qgscore]
GO
/****** Object:  Default [DF__Signin__Qsid__5224328E]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Signin] ADD  DEFAULT ((0)) FOR [Qsid]
GO
/****** Object:  Default [DF__Signin__Qclass__531856C7]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Signin] ADD  DEFAULT ((0)) FOR [Qclass]
GO
/****** Object:  Default [DF__Signin__Qsyear__540C7B00]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Signin] ADD  DEFAULT ((0)) FOR [Qsyear]
GO
/****** Object:  Default [DF__ShareDisk__Kown__55009F39]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[ShareDisk] ADD  DEFAULT ((0)) FOR [Kown]
GO
/****** Object:  Default [DF_Room_Rhid]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Room] ADD  CONSTRAINT [DF_Room_Rhid]  DEFAULT ((0)) FOR [Rhid]
GO
/****** Object:  Default [DF_Room_Rset]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Room] ADD  CONSTRAINT [DF_Room_Rset]  DEFAULT ((0)) FOR [Rset]
GO
/****** Object:  Default [DF__Room__Rlock__57DD0BE4]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Room] ADD  DEFAULT ((0)) FOR [Rlock]
GO
/****** Object:  Default [DF__Room__Rgauge__58D1301D]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Room] ADD  DEFAULT ((0)) FOR [Rgauge]
GO
/****** Object:  Default [DF__Room__RgroupMax__59C55456]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Room] ADD  DEFAULT ((0)) FOR [RgroupMax]
GO
/****** Object:  Default [DF__Room__Rclassedit__5AB9788F]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Room] ADD  DEFAULT ((0)) FOR [Rclassedit]
GO
/****** Object:  Default [DF__Room__Rphotoedit__5BAD9CC8]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Room] ADD  DEFAULT ((0)) FOR [Rphotoedit]
GO
/****** Object:  Default [DF__Room__Rsexedit__5CA1C101]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Room] ADD  DEFAULT ((0)) FOR [Rsexedit]
GO
/****** Object:  Default [DF__Room__Rnameedit__5D95E53A]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Room] ADD  DEFAULT ((0)) FOR [Rnameedit]
GO
/****** Object:  Default [DF__Room__Ropen__5E8A0973]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Room] ADD  DEFAULT ((0)) FOR [Ropen]
GO
/****** Object:  Default [DF__Room__Rseat__5F7E2DAC]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Room] ADD  DEFAULT ((0)) FOR [Rseat]
GO
/****** Object:  Default [DF__Room__Rshare__607251E5]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Room] ADD  DEFAULT ((0)) FOR [Rshare]
GO
/****** Object:  Default [DF__Room__Rpwdsee__6166761E]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Room] ADD  DEFAULT ((0)) FOR [Rpwdsee]
GO
/****** Object:  Default [DF__Room__Rgroupshar__625A9A57]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Room] ADD  DEFAULT ((0)) FOR [Rgroupshare]
GO
/****** Object:  Default [DF__Room__Rreg__634EBE90]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Room] ADD  DEFAULT ((0)) FOR [Rreg]
GO
/****** Object:  Default [DF__Room__Rscratch__6442E2C9]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Room] ADD  DEFAULT ((0)) FOR [Rscratch]
GO
/****** Object:  Default [DF__Room__RLogin__1B9317B3]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Room] ADD  DEFAULT ((0)) FOR [RLogin]
GO
/****** Object:  Default [DF__Result__Rgrade__65370702]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Result] ADD  DEFAULT ((0)) FOR [Rgrade]
GO
/****** Object:  Default [DF__Result__Rterm__662B2B3B]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Result] ADD  DEFAULT ((0)) FOR [Rterm]
GO
/****** Object:  Default [DF__Result__Rsid__671F4F74]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Result] ADD  DEFAULT ((0)) FOR [Rsid]
GO
/****** Object:  Default [DF__QuizGrade__Qobj__681373AD]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[QuizGrade] ADD  DEFAULT ((0)) FOR [Qobj]
GO
/****** Object:  Default [DF_QuizGrade_Qopen]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[QuizGrade] ADD  CONSTRAINT [DF_QuizGrade_Qopen]  DEFAULT ((1)) FOR [Qopen]
GO
/****** Object:  Default [DF__QuizGrade__Qansw__69FBBC1F]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[QuizGrade] ADD  DEFAULT ((1)) FOR [Qanswer]
GO
/****** Object:  Default [DF__Quiz__Qselect__0EA330E9]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Quiz] ADD  CONSTRAINT [DF__Quiz__Qselect__0EA330E9]  DEFAULT ((0)) FOR [Qselect]
GO
/****** Object:  Default [DF__Quiz__Qright__6BE40491]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Quiz] ADD  DEFAULT ((0)) FOR [Qright]
GO
/****** Object:  Default [DF__Quiz__Qwrong__6CD828CA]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Quiz] ADD  DEFAULT ((0)) FOR [Qwrong]
GO
/****** Object:  Default [DF__Quiz__Qaccuracy__6DCC4D03]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Quiz] ADD  DEFAULT ((0)) FOR [Qaccuracy]
GO
/****** Object:  Default [DF_Ptyper_Ptype]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Ptyper] ADD  CONSTRAINT [DF_Ptyper_Ptype]  DEFAULT ((1)) FOR [Ptype]
GO
/****** Object:  Default [DF__Ptyper__Pdegree__0DAF0CB0]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Ptyper] ADD  CONSTRAINT [DF__Ptyper__Pdegree__0DAF0CB0]  DEFAULT ((0)) FOR [Pdegree]
GO
/****** Object:  Default [DF__Ptyper__Pgrade__70A8B9AE]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Ptyper] ADD  DEFAULT ((0)) FOR [Pgrade]
GO
/****** Object:  Default [DF__Ptyper__Pterm__719CDDE7]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Ptyper] ADD  DEFAULT ((0)) FOR [Pterm]
GO
/****** Object:  Default [DF__Ptyper__Psid__72910220]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Ptyper] ADD  DEFAULT ((0)) FOR [Psid]
GO
/****** Object:  Default [DF__Pfinger__Pgrade__73852659]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Pfinger] ADD  DEFAULT ((0)) FOR [Pgrade]
GO
/****** Object:  Default [DF__Pfinger__Pterm__74794A92]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Pfinger] ADD  DEFAULT ((0)) FOR [Pterm]
GO
/****** Object:  Default [DF__Pfinger__Psid__756D6ECB]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Pfinger] ADD  DEFAULT ((0)) FOR [Psid]
GO
/****** Object:  Default [DF__Pchinese__Papple__76619304]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Pchinese] ADD  DEFAULT ((0)) FOR [Papple]
GO
/****** Object:  Default [DF__Pchinese__Ptotal__7755B73D]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Pchinese] ADD  DEFAULT ((0)) FOR [Ptotal]
GO
/****** Object:  Default [DF__Pchinese__Pspeed__7849DB76]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Pchinese] ADD  DEFAULT ((0)) FOR [Pspeed]
GO
/****** Object:  Default [DF_Mission_Mhit]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Mission] ADD  CONSTRAINT [DF_Mission_Mhit]  DEFAULT ((0)) FOR [Mhit]
GO
/****** Object:  Default [DF_Mission_Mupload]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Mission] ADD  CONSTRAINT [DF_Mission_Mupload]  DEFAULT ((0)) FOR [Mupload]
GO
/****** Object:  Default [DF_Mission_Msort]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Mission] ADD  CONSTRAINT [DF_Mission_Msort]  DEFAULT ((0)) FOR [Msort]
GO
/****** Object:  Default [DF_Mission_Mpublish]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Mission] ADD  CONSTRAINT [DF_Mission_Mpublish]  DEFAULT ((1)) FOR [Mpublish]
GO
/****** Object:  Default [DF__Mission__Mgroup__7D0E9093]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Mission] ADD  DEFAULT ((0)) FOR [Mgroup]
GO
/****** Object:  Default [DF__Mission__Mgid__7E02B4CC]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Mission] ADD  DEFAULT ((0)) FOR [Mgid]
GO
/****** Object:  Default [DF__Mission__Mdelete__7EF6D905]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Mission] ADD  DEFAULT ((0)) FOR [Mdelete]
GO
/****** Object:  Default [DF__Mission__Mcatego__7FEAFD3E]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Mission] ADD  DEFAULT ((0)) FOR [Mcategory]
GO
/****** Object:  Default [DF__Mission__Microwo__00DF2177]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Mission] ADD  DEFAULT ((0)) FOR [Microworld]
GO
/****** Object:  Default [DF__ListMenu__Lsort__01D345B0]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[ListMenu] ADD  DEFAULT ((0)) FOR [Lsort]
GO
/****** Object:  Default [DF__ListMenu__Lshow__02C769E9]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[ListMenu] ADD  DEFAULT ((1)) FOR [Lshow]
GO
/****** Object:  Default [DF__JudgeArg__Jsleep__4865BE2A]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[JudgeArg] ADD  DEFAULT ((1000)) FOR [Jsleep]
GO
/****** Object:  Default [DF__JudgeArg__Jright__4959E263]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[JudgeArg] ADD  DEFAULT ((0)) FOR [Jright]
GO
/****** Object:  Default [DF_GroupWork_Gcheck]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[GroupWork] ADD  CONSTRAINT [DF_GroupWork_Gcheck]  DEFAULT ((0)) FOR [Gcheck]
GO
/****** Object:  Default [DF_GroupWork_Ghit]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[GroupWork] ADD  CONSTRAINT [DF_GroupWork_Ghit]  DEFAULT ((0)) FOR [Ghit]
GO
/****** Object:  Default [DF__GaugeFeed__Fgood__05A3D694]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[GaugeFeedback] ADD  DEFAULT ((0)) FOR [Fgood]
GO
/****** Object:  Default [DF__GaugeFeedb__Fsid__0697FACD]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[GaugeFeedback] ADD  DEFAULT ((0)) FOR [Fsid]
GO
/****** Object:  Default [DF_Courses_Chit]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Courses] ADD  CONSTRAINT [DF_Courses_Chit]  DEFAULT ((0)) FOR [Chit]
GO
/****** Object:  Default [DF_Courses_Cupload]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Courses] ADD  CONSTRAINT [DF_Courses_Cupload]  DEFAULT ((1)) FOR [Cupload]
GO
/****** Object:  Default [DF_Courses_Cpublish]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Courses] ADD  CONSTRAINT [DF_Courses_Cpublish]  DEFAULT ((1)) FOR [Cpublish]
GO
/****** Object:  Default [DF__Courses__Cdelete__0A688BB1]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Courses] ADD  DEFAULT ((0)) FOR [Cdelete]
GO
/****** Object:  Default [DF__Courses__Cgood__0B5CAFEA]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Courses] ADD  DEFAULT ((1)) FOR [Cgood]
GO
/****** Object:  Default [DF__Courses__Cold__0C50D423]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Courses] ADD  DEFAULT ((0)) FOR [Cold]
GO
/****** Object:  Default [DF__Consoles__Npubli__3552E9B6]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Consoles] ADD  DEFAULT ((0)) FOR [Npublish]
GO
/****** Object:  Default [DF__Consoles__Nbegin__50FB042B]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Consoles] ADD  DEFAULT ((0)) FOR [Nbegin]
GO
/****** Object:  Default [DF__Computers__Plock__0D44F85C]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Computers] ADD  DEFAULT ((0)) FOR [Plock]
GO
/****** Object:  Default [DF__Computers__Px__0E391C95]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Computers] ADD  DEFAULT ((0)) FOR [Px]
GO
/****** Object:  Default [DF__Computers__Py__0F2D40CE]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Computers] ADD  DEFAULT ((0)) FOR [Py]
GO
/****** Object:  Default [DF__Computers__Pon__51EF2864]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Computers] ADD  DEFAULT ((0)) FOR [Pon]
GO
/****** Object:  Default [DF__Autonomic__Ascor__10216507]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Autonomic] ADD  DEFAULT ((0)) FOR [Ascore]
GO
/****** Object:  Default [DF__Autonomic__Avote__11158940]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Autonomic] ADD  DEFAULT ((0)) FOR [Avote]
GO
/****** Object:  Default [DF__Autonomic__Aegg__1209AD79]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Autonomic] ADD  DEFAULT ((0)) FOR [Aegg]
GO
/****** Object:  Default [DF__Autonomic__Achec__12FDD1B2]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Autonomic] ADD  DEFAULT ((0)) FOR [Acheck]
GO
/****** Object:  Default [DF__Autonomic__Agood__13F1F5EB]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Autonomic] ADD  DEFAULT ((0)) FOR [Agood]
GO
/****** Object:  Default [DF__Autonomic__Ahit__14E61A24]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Autonomic] ADD  DEFAULT ((0)) FOR [Ahit]
GO
/****** Object:  Default [DF__Autonomic__Aoffi__15DA3E5D]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Autonomic] ADD  DEFAULT ((0)) FOR [Aoffice]
GO
/****** Object:  Default [DF__Autonomic__Aflas__16CE6296]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Autonomic] ADD  DEFAULT ((0)) FOR [Aflash]
GO
/****** Object:  Default [DF__Autonomic__Aerro__17C286CF]    Script Date: 03/29/2022 19:53:35 ******/
ALTER TABLE [dbo].[Autonomic] ADD  DEFAULT ((0)) FOR [Aerror]
GO

BEGIN
insert into Teacher(Hname,Hpwd,Hpermiss,Hnote) values ('admin','12345','1','管理员')
END