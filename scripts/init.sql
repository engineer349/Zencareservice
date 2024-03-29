USE [zencareservice]
GO
/****** Object:  DatabaseRole [Admin]    Script Date: 16-03-2024 15:30:28 ******/
CREATE ROLE [Admin]
GO
/****** Object:  DatabaseRole [Doctor]    Script Date: 16-03-2024 15:30:28 ******/
CREATE ROLE [Doctor]
GO
/****** Object:  DatabaseRole [Patient]    Script Date: 16-03-2024 15:30:28 ******/
CREATE ROLE [Patient]
GO
/****** Object:  DatabaseRole [Staff]    Script Date: 16-03-2024 15:30:28 ******/
CREATE ROLE [Staff]
GO
/****** Object:  Table [dbo].[DocumentcodeTbl]    Script Date: 16-03-2024 15:30:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DocumentcodeTbl](
	[Sid] [int] IDENTITY(1,1) NOT NULL,
	[Seriesname] [nvarchar](100) NULL,
	[Keyword] [nvarchar](100) NULL,
	[CurrentNo] [int] NULL,
	[PadNo] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[Documentcode_vw]    Script Date: 16-03-2024 15:30:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create View [dbo].[Documentcode_vw]
as
select Seriesname,Keyword+RIGHT(REPLICATE('0', PadNo) + cast(CurrentNo+1 as varchar(100)), PadNo) as docno 
from DocumentcodeTbl
GO
/****** Object:  Table [dbo].[Appointment]    Script Date: 16-03-2024 15:30:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Appointment](
	[AptCategory] [varchar](255) NOT NULL,
	[AptId] [int] IDENTITY(1,1) NOT NULL,
	[AptCode] [varchar](255) NULL,
	[RCode] [varchar](255) NULL,
	[AptBookingdate] [date] NULL,
	[AptTime] [time](7) NULL,
	[Aptbookingconfirm] [int] NULL,
	[Aptbookingstatusconfirm] [int] NULL,
	[Aptrescheduleconfirm] [int] NULL,
	[Aptrescheduledate] [date] NULL,
	[Aptrescheduletime] [time](7) NULL,
	[Reasontype] [varchar](255) NULL,
	[PatientEmail] [varchar](255) NULL,
	[PFname] [varchar](255) NULL,
	[PLname] [varchar](255) NULL,
	[DFname] [varchar](255) NULL,
	[Patage] [varchar](255) NULL,
	[Patgender] [varchar](255) NULL,
	[Patphone] [varchar](255) NULL,
	[UsrId] [int] NULL,
	[DId] [varchar](20) NULL,
	[PId] [int] NULL,
	[Aptcreatedate] [datetime] NULL,
	[DContactno] [nvarchar](255) NULL,
	[DEmail] [nvarchar](255) NULL,
	[Pataddress1] [varchar](255) NULL,
	[Pataddress2] [varchar](255) NULL,
	[PState] [varchar](255) NULL,
	[PCity] [varchar](255) NULL,
 CONSTRAINT [AptCode] UNIQUE NONCLUSTERED 
(
	[AptCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[Appointment_vw]    Script Date: 16-03-2024 15:30:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE view [dbo].[Appointment_vw] as  select UsrId,PId,DId, AptId,AptCategory,RCode,AptBookingdate,PFname,DFname from Appointment 
GO
/****** Object:  Table [dbo].[Admin]    Script Date: 16-03-2024 15:30:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Admin](
	[AId] [int] IDENTITY(1,1) NOT NULL,
	[AFname] [varchar](255) NULL,
	[ALname] [varchar](255) NULL,
	[APhone] [varchar](14) NULL,
	[AEmail] [varchar](255) NULL,
	[ADob] [date] NULL,
	[Percode] [varchar](255) NULL,
	[RCode] [varchar](255) NULL,
	[Aage] [int] NULL,
	[UsrId] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Appointmentstatus]    Script Date: 16-03-2024 15:30:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Appointmentstatus](
	[ASId] [int] IDENTITY(1,1) NOT NULL,
	[AptCode] [varchar](255) NULL,
	[RCode] [varchar](255) NULL,
	[AptBookingdate] [date] NULL,
	[AptBooking] [time](7) NULL,
	[Reasontype] [varchar](255) NULL,
	[Aptbookingconfirm] [int] NULL,
	[Aptbookingstatusconfirm] [int] NULL,
	[Aptreschedule] [date] NULL,
	[Aptrescheduletime] [time](7) NULL,
	[Aptrecreatedate] [datetime] NULL,
	[PFname] [varchar](255) NULL,
	[PLname] [varchar](255) NULL,
	[DFname] [varchar](255) NULL,
	[DLname] [varchar](255) NULL,
	[Patage] [varchar](25) NULL,
	[Patgender] [varchar](25) NULL,
	[PatPhone] [varchar](14) NULL,
	[PatientEmail] [varchar](255) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Appointmentstatus_tbl]    Script Date: 16-03-2024 15:30:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Appointmentstatus_tbl](
	[Id] [int] NOT NULL,
	[regno] [uniqueidentifier] NULL,
	[username] [varchar](250) NULL,
	[email] [varchar](250) NULL,
	[patname] [varchar](250) NOT NULL,
	[patid] [nvarchar](max) NOT NULL,
	[patphoneno] [varchar](250) NOT NULL,
	[pataddress] [nvarchar](max) NULL,
	[docname] [varchar](250) NOT NULL,
	[docid] [varchar](250) NOT NULL,
	[apptstatusid] [nvarchar](max) NULL,
	[apptconfirmstatusno] [varchar](250) NOT NULL,
	[apptbookingconfirmdate] [datetime] NOT NULL,
	[apptbookingconfirmtime] [varchar](250) NOT NULL,
	[apptbookingstatus] [bit] NULL,
	[apptrescheduledate] [datetime] NULL,
	[apptrescheduletime] [varchar](250) NULL,
	[apptrescheduleconfirm] [bit] NULL,
 CONSTRAINT [PK_Appointmentstatus_tbl] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Aptstatus]    Script Date: 16-03-2024 15:30:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Aptstatus](
	[AptId] [int] NULL,
	[AptCode] [varchar](200) NULL,
	[RCode] [varchar](255) NULL,
	[UsrId] [int] NULL,
	[PFname] [varchar](200) NULL,
	[Patphone] [varchar](20) NULL,
	[Aptbookingconfirm] [varchar](20) NULL,
	[Aptrescheduledate] [date] NULL,
	[Aptrescheduletime] [time](7) NULL,
	[DFname] [varchar](255) NULL,
	[PatientEmail] [varchar](200) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Bloodgroup_tbl]    Script Date: 16-03-2024 15:30:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Bloodgroup_tbl](
	[SlNo] [int] IDENTITY(1,1) NOT NULL,
	[Bloodgroup] [varchar](255) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[City_tbl]    Script Date: 16-03-2024 15:30:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[City_tbl](
	[CId] [int] IDENTITY(1,1) NOT NULL,
	[CCode] [varchar](255) NULL,
	[City] [varchar](255) NULL,
	[SId] [int] NULL,
	[State] [nvarchar](255) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Courses]    Script Date: 16-03-2024 15:30:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Courses](
	[CourseID] [int] NOT NULL,
	[CourseName] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[CourseID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Dashboardvalues_tbl]    Script Date: 16-03-2024 15:30:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dashboardvalues_tbl](
	[SNo] [int] IDENTITY(1,1) NOT NULL,
	[AptCount] [int] NULL,
	[AptstatusCount] [int] NULL,
	[RegisterCount] [int] NULL,
	[PrescriptionCount] [int] NULL,
	[Role] [varchar](20) NULL,
	[UsrId] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Doctor]    Script Date: 16-03-2024 15:30:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Doctor](
	[DId] [int] IDENTITY(1,1) NOT NULL,
	[PerCode] [varchar](255) NULL,
	[RCode] [varchar](255) NULL,
	[Email] [varchar](255) NULL,
	[DFname] [varchar](255) NULL,
	[DLname] [varchar](255) NULL,
	[Dage] [varchar](3) NULL,
	[Dgender] [varchar](255) NULL,
	[Dphone] [varchar](14) NULL,
	[ADphone] [varchar](14) NULL,
	[Practionerproof] [varbinary](max) NULL,
	[Dphoto] [varbinary](max) NULL,
	[Dexp] [varchar](12) NULL,
	[Dqualification] [varchar](255) NULL,
	[Dspecialist] [varchar](255) NULL,
	[DDob] [date] NULL,
	[UsrId] [int] NULL,
	[Daddress1] [varchar](255) NULL,
	[Daddress2] [varchar](255) NULL,
	[DCity] [varchar](255) NULL,
	[Duniqueid] [varchar](255) NULL,
	[Dspecialistid] [varchar](100) NULL,
	[DProof] [varbinary](max) NULL,
	[DState] [varchar](255) NULL,
 CONSTRAINT [UQ_Doctor_Email] UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Doctorspecialist_tbl]    Script Date: 16-03-2024 15:30:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Doctorspecialist_tbl](
	[Id] [int] NOT NULL,
	[sno] [int] NOT NULL,
	[specialistid] [varchar](250) NOT NULL,
	[specialistname] [varchar](250) NOT NULL,
 CONSTRAINT [PK_Doctorspecialist_tbl] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Enrollments]    Script Date: 16-03-2024 15:30:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Enrollments](
	[EnrollmentID] [int] NOT NULL,
	[StudentID] [int] NULL,
	[CourseID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[EnrollmentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Gender_tbl]    Script Date: 16-03-2024 15:30:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Gender_tbl](
	[SlNo] [int] IDENTITY(1,1) NOT NULL,
	[Gender] [varchar](255) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Login]    Script Date: 16-03-2024 15:30:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Login](
	[LId] [int] IDENTITY(1,1) NOT NULL,
	[Username] [varchar](255) NULL,
	[Email] [varchar](255) NULL,
	[logindate] [datetime] NULL,
	[lastlogin] [datetime] NULL,
	[Fname] [varchar](255) NULL,
	[Lname] [varchar](255) NULL,
	[RCode] [varchar](255) NULL,
	[Dob] [datetime] NULL,
	[Role] [nvarchar](255) NULL,
	[Password] [varchar](255) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Module_tbl]    Script Date: 16-03-2024 15:30:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Module_tbl](
	[SNo] [int] NULL,
	[Modulename] [varchar](255) NULL,
	[Controller] [varchar](255) NULL,
	[Action] [varchar](255) NULL,
	[MRole] [nvarchar](100) NULL,
	[icon] [varchar](255) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Modules]    Script Date: 16-03-2024 15:30:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Modules](
	[ModuleId] [int] NOT NULL,
	[ModuleName] [varchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ModuleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Patdiagnosis]    Script Date: 16-03-2024 15:30:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Patdiagnosis](
	[PDId] [int] IDENTITY(1,1) NOT NULL,
	[RCode] [varchar](max) NULL,
	[PDCode] [varchar](255) NULL,
	[PFname] [varchar](255) NULL,
	[PLname] [varchar](255) NULL,
	[Patphoneno] [varchar](14) NULL,
	[Patage] [varchar](14) NULL,
	[Patgender] [varchar](14) NULL,
	[DFname] [varchar](255) NULL,
	[DLname] [varchar](255) NULL,
	[PatSymptoms] [varchar](255) NULL,
	[Patdiagnosisdetails] [nvarchar](max) NULL,
	[Patdiagnosisproof] [varbinary](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Patient]    Script Date: 16-03-2024 15:30:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Patient](
	[PId] [int] IDENTITY(1,1) NOT NULL,
	[RCode] [varchar](255) NULL,
	[Percode] [varchar](255) NULL,
	[UsrId] [int] NULL,
	[PEmail] [varchar](255) NULL,
	[PFname] [varchar](255) NULL,
	[PLname] [varchar](255) NULL,
	[PDob] [datetime] NULL,
	[Patage] [varchar](25) NULL,
	[Patgender] [varchar](25) NULL,
	[Pataddress1] [nvarchar](max) NULL,
	[Pataddress2] [nvarchar](max) NULL,
	[Patphoneno] [varchar](255) NULL,
	[Pataltphone] [varchar](255) NULL,
	[PCity] [varchar](255) NULL,
	[Puniqueid] [varchar](255) NULL,
	[PState] [varchar](255) NULL,
	[PatPhoto] [varbinary](max) NULL,
	[Pat_History] [varchar](255) NULL,
	[Pat_Diagnosis] [varchar](255) NULL,
	[Otherdetails] [varchar](255) NULL,
	[PProof] [varbinary](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Patientdetails]    Script Date: 16-03-2024 15:30:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Patientdetails](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PId] [int] NULL,
	[RCode] [varchar](255) NULL,
	[Rname] [varchar](255) NULL,
	[Patname] [varchar](255) NULL,
	[Patage] [int] NULL,
	[Patgender] [varchar](255) NULL,
	[Pataddress1] [varchar](255) NULL,
	[Pataddress2] [varchar](255) NULL,
	[City] [varchar](255) NULL,
	[Pincode] [varchar](255) NULL,
	[Attendeephoneno] [varchar](255) NULL,
	[Patphoto] [varbinary](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Permissions]    Script Date: 16-03-2024 15:30:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Permissions](
	[PermissionId] [int] NOT NULL,
	[PermissionName] [varchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PermissionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Personaldetails]    Script Date: 16-03-2024 15:30:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Personaldetails](
	[PerId] [int] IDENTITY(1,1) NOT NULL,
	[RCode] [varchar](255) NULL,
	[PerCode] [nvarchar](100) NULL,
	[Fname] [varchar](100) NULL,
	[Lname] [varchar](100) NULL,
	[Age] [varchar](3) NULL,
	[Gender] [varchar](100) NULL,
	[Email] [varchar](255) NULL,
	[Phoneno] [varchar](14) NULL,
	[Aphoneno] [varchar](14) NULL,
	[UniqueId] [varchar](255) NULL,
	[Addressline2] [nvarchar](max) NULL,
	[AltAddress] [nvarchar](max) NULL,
	[State] [varchar](255) NULL,
	[City] [varchar](255) NULL,
	[Country] [varchar](255) NULL,
	[Photo] [varbinary](max) NULL,
	[UsrId] [int] NULL,
	[Dob] [datetime] NULL,
	[Role] [varchar](255) NULL,
	[Addressline1] [nvarchar](100) NULL,
	[Zipcode] [varchar](6) NULL,
	[Updatedate] [date] NULL,
	[SId] [int] NULL,
	[CId] [int] NULL,
	[Perproof] [varbinary](max) NULL,
 CONSTRAINT [UQ_Personaldetails_Email] UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Prescription_Tbl]    Script Date: 16-03-2024 15:30:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Prescription_Tbl](
	[PrsCode] [varchar](255) NULL,
	[AptCode] [varchar](255) NULL,
	[Pfname] [varchar](255) NULL,
	[Dfname] [varchar](255) NULL,
	[Slno] [int] NULL,
	[Prscitem] [varchar](255) NULL,
	[PrscDosage] [varchar](12) NULL,
	[Prscdays] [int] NULL,
	[PrscId] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Prsc_head_tbl]    Script Date: 16-03-2024 15:30:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Prsc_head_tbl](
	[PrscHId] [int] IDENTITY(1,1) NOT NULL,
	[Aptcode] [varchar](255) NULL,
	[Dfname] [varchar](255) NULL,
	[Dphoneno] [varchar](14) NULL,
	[Pfname] [varchar](255) NULL,
	[Pphoneno] [varchar](14) NULL,
	[Pemail] [varchar](255) NULL,
	[PBloodgroup] [nvarchar](100) NULL,
	[Page] [varchar](3) NULL,
	[Pweight] [varchar](3) NULL,
	[Pgender] [varchar](10) NULL,
	[PUId] [int] NULL,
	[DUId] [int] NULL,
	[Prscode] [varchar](255) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Prsc_item_tbl]    Script Date: 16-03-2024 15:30:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Prsc_item_tbl](
	[Slno] [int] NULL,
	[Prscitem] [varchar](255) NULL,
	[PrscDosage] [varchar](12) NULL,
	[Prscdays] [int] NULL,
	[PUId] [int] NULL,
	[DUId] [int] NULL,
	[Aptcode] [varchar](255) NULL,
	[Prscode] [varchar](255) NULL,
	[PrscItemId] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Register]    Script Date: 16-03-2024 15:30:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Register](
	[RId] [int] IDENTITY(1,1) NOT NULL,
	[RCode] [varchar](255) NULL,
	[Fname] [varchar](255) NULL,
	[Lname] [varchar](255) NULL,
	[Email] [varchar](255) NULL,
	[Password] [varchar](255) NULL,
	[CPassword] [varchar](255) NULL,
	[Username] [varchar](255) NULL,
	[Dob] [datetime] NULL,
	[Status] [int] NULL,
	[Role] [varchar](255) NULL,
	[Phoneno] [varchar](16) NULL,
	[Regdate] [datetime] NULL,
	[agreeterm] [int] NULL,
 CONSTRAINT [unique_emailid] UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Rolerights]    Script Date: 16-03-2024 15:30:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Rolerights](
	[SNo] [int] IDENTITY(1,1) NOT NULL,
	[UsrId] [int] NULL,
	[UsrName] [varchar](20) NULL,
	[RoleName] [varchar](20) NULL,
	[ModuleName] [varchar](20) NULL,
	[Rights] [varchar](20) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Rolerights_tbl]    Script Date: 16-03-2024 15:30:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Rolerights_tbl](
	[SNo] [int] NULL,
	[UsrId] [int] NULL,
	[UsrName] [varchar](100) NULL,
	[RoleName] [varchar](255) NULL,
	[ModuleName] [varchar](255) NULL,
	[Rights] [int] NOT NULL,
	[icon] [varchar](255) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 16-03-2024 15:30:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[RoleId] [int] NOT NULL,
	[RoleName] [varchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Staff]    Script Date: 16-03-2024 15:30:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Staff](
	[SId] [int] IDENTITY(1,1) NOT NULL,
	[PerCode] [varchar](255) NULL,
	[RCode] [varchar](255) NULL,
	[UsrId] [int] NULL,
	[SEmail] [varchar](255) NULL,
	[SFname] [varchar](255) NULL,
	[SLname] [varchar](255) NULL,
	[Sage] [varchar](22) NULL,
	[Sgender] [varchar](22) NULL,
	[SAddress] [varchar](255) NULL,
	[SPhone] [varchar](14) NULL,
	[SDob] [datetime] NULL,
	[Saphoneno] [varchar](20) NULL,
	[Saddress1] [varchar](255) NULL,
	[Saddress2] [varchar](255) NULL,
	[SCity] [varchar](255) NULL,
	[Sphoto] [varbinary](max) NULL,
	[Suniqueid] [varchar](25) NULL,
	[SProof] [varbinary](max) NULL,
	[Empexp] [int] NULL,
	[Empqualify] [varchar](200) NULL,
	[Emphistory] [varchar](200) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[State_tbl]    Script Date: 16-03-2024 15:30:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[State_tbl](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SCode] [varchar](255) NULL,
	[State] [varchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Students]    Script Date: 16-03-2024 15:30:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Students](
	[StudentID] [int] NOT NULL,
	[FirstName] [varchar](50) NULL,
	[LastName] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[StudentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Treatment]    Script Date: 16-03-2024 15:30:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Treatment](
	[TId] [int] IDENTITY(1,1) NOT NULL,
	[RCode] [varchar](255) NULL,
	[PDCode] [varchar](255) NULL,
	[TCode] [varchar](max) NULL,
	[PFname] [varchar](255) NULL,
	[PLname] [varchar](255) NULL,
	[DFname] [varchar](255) NULL,
	[DLname] [varchar](255) NULL,
	[Patage] [varchar](14) NULL,
	[Patgender] [varchar](14) NULL,
	[Patphoneno] [varchar](14) NULL,
	[PatSymptoms] [varchar](255) NULL,
	[PatTreatmentdetails] [nvarchar](max) NULL,
	[PatTreatmentproof] [varbinary](max) NULL,
	[Patdiagnosisdetails] [nvarchar](max) NULL,
	[Patdiagnosisproof] [varbinary](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Enrollments]  WITH CHECK ADD FOREIGN KEY([CourseID])
REFERENCES [dbo].[Courses] ([CourseID])
GO
ALTER TABLE [dbo].[Enrollments]  WITH CHECK ADD FOREIGN KEY([StudentID])
REFERENCES [dbo].[Students] ([StudentID])
GO
/****** Object:  StoredProcedure [dbo].[AppointmentBooking_SP]    Script Date: 16-03-2024 15:30:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AppointmentBooking_SP] @PatientFirstname nvarchar(255),@PatientLastname varchar(255),@PatientEmail varchar(255),@DoctorFirstname varchar(255),                                                              
@AptBookdate date,@AptTime time,@ReasonType  varchar(255), @RCode varchar(255),@AptCategory varchar(255),@Patgender varchar(255),  
@PatientContact varchar(14),@Patage varchar(3),@UsrId int  

AS        
BEGIN 

 DECLARE  @AptCode nvarchar(100)                                                   
 DECLARE @count int                                                              
 DECLARE @tblid Table(AptId int)             
 DECLARE @DName varchar(255)        
 
 SELECT @DName=DFname from Doctor where UsrId=@DoctorFirstname                    
                      
if (@AptCategory = 'self')                        
BEGIN           
 select @AptCode=Docno from Documentcode_vw Where Seriesname='Appointment'        
 INSERT INTO Appointment(AptCategory,RCode,Reasontype,PatientEmail,PFname,PLname,Patgender,DId,AptBookingdate,AptTime,Aptcreatedate,Patphone,Aptbookingconfirm,Aptbookingstatusconfirm,UsrId,DFname,AptCode,Aptrescheduleconfirm)                                                               
 OUTPUT inserted.AptId into @tblid                                                                          
 VALUES(@AptCategory,@RCode,@ReasonType,@PatientEmail,@PatientFirstname,@PatientLastname,@Patgender,@DoctorFirstname,@AptBookdate,@Apttime,getdate(),@PatientContact,1,0,@UsrId,@DName,@AptCode,0)                                                             
  
 UPDATE DocumentcodeTbl set CurrentNo = CurrentNo +1 where Seriesname='Appointment'       
 --SELECT @AptCode='APT-100'+cast(AptId as varchar(100)) from Appointment where AptId in (select AptId from @tblid)                                      
 --UPDATE Appointment SET AptCode=@AptCode where AptId in (select AptId from @tblid)                    
 --UPDATE Appointment SET DFname =@doctorfirstname Where AptId in (select AptId from @tblid)                    
 UPDATE Appointment                         
 SET                 
  DContactno = (SELECT phoneno  from Register WHERE Register.RId = Appointment.DId),                      
  DEmail     = (SELECT Email from Register WHERE Register.RId = Appointment.DId),                      
  PId = (SELECT PId from Patient WHERE Appointment.RCode = Patient.RCode),                            
  Patgender = (SELECT Patgender from Patient  WHERE Appointment.RCode= Patient.RCode),                                  
  Pataddress1 = (SELECT Pataddress1 FROM Patient WHERE Appointment.RCode= Patient.RCode),                                  
  Pataddress2 = (SELECT Pataddress2 FROM Patient WHERE Appointment.RCode= Patient.RCode),                                  
  PState = (SELECT PState FROM Patient WHERE Appointment.RCode= Patient.RCode),                                  
  PCity = (SELECT PCity FROM Patient WHERE Appointment.RCode= Patient.RCode),                                  
  PatPhone = (SELECT Patphone FROM Patient WHERE Appointment.RCode= Patient.RCode),                                  
  Patage = (SELECT Patage FROM Patient WHERE Appointment.RCode= Patient.RCode)                              
  WHERE AptId in (select AptId from @tblid);           
       
  END        
        
 ELSE                        
 BEGIN        
 SELECT @AptCode=Docno from Documentcode_vw Where Seriesname='Appointment'        
 INSERT INTO Appointment(AptCategory,RCode,Reasontype,PatientEmail,PFname,PLname,Patage,Patgender,DId,AptBookingdate,AptTime,Aptcreatedate,Patphone,Aptbookingconfirm,Aptbookingstatusconfirm,UsrId,DFname,AptCode,Aptrescheduleconfirm)                       
                                       
  OUTPUT inserted.AptId into @tblid                                                                          
 VALUES(@AptCategory,@RCode,@ReasonType,@PatientEmail,@PatientFirstname,@PatientLastname,@Patage,@Patgender,@DoctorFirstname,@AptBookdate,@Apttime,getdate(),@PatientContact,1,0,@UsrId,@DName,@AptCode,0)                                             
   UPDATE DocumentcodeTbl set CurrentNo = CurrentNo +1 where Seriesname='Appointment'       
 --SELECT @AptCode='APT-100'+cast(AptId as varchar(100)) from Appointment where AptId in (select AptId from @tblid)                                      
 --UPDATE Appointment set AptCode=@AptCode where AptId in (select AptId from @tblid)                   
-- select @doctorfirstname=DFname from Doctor where DId=@DoctorFirstname                    
--UPDATE Appointment SET DFname =@doctorfirstname Where AptId in (select AptId from @tblid)                    
 UPDATE Appointment                         
SET               
 DContactno = (SELECT Phoneno  from Register WHERE Register.RId = Appointment.DId),                      
 DEmail     = (SELECT Email from Register WHERE Register.RId = Appointment.DId),                      
 PId =  (SELECT RId from Register WHERE Appointment.RCode = Register.RCode)               
END                        
END 
GO
/****** Object:  StoredProcedure [dbo].[AuthenticateUser]    Script Date: 16-03-2024 15:30:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AuthenticateUser]
    @Username VARCHAR(50),
    @Password VARCHAR(50)
AS
BEGIN
    SELECT UserId, Username, RoleId, Rolename
    FROM Users
    WHERE Username = @Username AND Password = @Password;
END;
GO
/****** Object:  StoredProcedure [dbo].[CalculateAge]    Script Date: 16-03-2024 15:30:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CalculateAge]
    @DateOfBirth DATE
AS
BEGIN
    -- Get the current date
    DECLARE @CurrentDate DATE
    SET @CurrentDate = GETDATE();

    -- Calculate the age using DATEDIFF
    DECLARE @Age INT
    SET @Age = DATEDIFF(YEAR, @DateOfBirth, @CurrentDate) -
               CASE
                   WHEN @DateOfBirth > DATEADD(YEAR, DATEDIFF(YEAR, @DateOfBirth, @CurrentDate), @DateOfBirth) THEN 1
                   ELSE 0
               END

    -- Display the result
    SELECT
        @DateOfBirth AS DateOfBirth,
        @CurrentDate AS CurrentDate,
        @Age AS Age
END;
GO
/****** Object:  StoredProcedure [dbo].[CheckLogin_SP]    Script Date: 16-03-2024 15:30:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[CheckLogin_SP] @Uname nvarchar(100),@Pass nvarchar(100) as              
begin                      
declare @count int                      
declare @LL datetime                      
declare @UsrId int      
declare @Role nvarchar(100)      
select @count=count(*) from Register where Username=@Uname or Email=@Uname and Password=@Pass                 
if @count>0                      
begin                      
select @LL=Logindate from login where lid in  (select max(LId) from login where  Username=@Uname and Password=@Pass)                      
insert into login(Fname,Lname,Username, Password,Email,logindate,lastlogin,RCode,Dob,Role)                        
select Fname,Lname, Username,Password,Email,getdate(),@LL,RCode,Dob,Role from Register where Username=@Uname or Email=@Uname and Password=@Pass                     
                  
select RId,RCode,Fname,Lname,Email,Password,CPassword,Username,Dob,Phoneno,Status,Role,1 as LStatus from Register                  
where Username=@Uname  or Email=@Uname and Password=@Pass                   
                  
                  
end                    
else                  
select 0 as LStatus         
select @UsrId =RId,@Role=Role from Register where Username=@Uname  or Email=@Uname and Password=@Pass                   
      
if @Role='Staff'      
--select 'Dashboard' as Modulename,'Reports' as Controller,'DashboardV1' as Action,1 as Rights, 'fa-solid fa-chart-line' as icon    
--union all      
select a.Modulename,a.Controller,a.Action,b.Rights,a.icon from Module_tbl a,Rolerights b where a.Modulename=b.ModuleName       
and UsrId=@UsrId       
      
else      
--select 'Dashboard' as Modulename,'Reports' as Controller,'DashboardV2' as Action,1 as Rights, 'fa-solid fa-chart-line' as icon    
--union all      
select a.Modulename,a.Controller,a.Action,b.Rights,a.icon from Module_tbl a,Rolerights b where a.Modulename=b.ModuleName       
and UsrId=@UsrId       
      
                  
end 
GO
/****** Object:  StoredProcedure [dbo].[ConfirmAppointment_SP]    Script Date: 16-03-2024 15:30:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 Create Procedure [dbo].[ConfirmAppointment_SP] @AptId int
 AS BEGIN
 
 UPDATE Appointment set Aptbookingstatusconfirm=1, Aptrescheduleconfirm=0 where AptId=@AptId

END
GO
/****** Object:  StoredProcedure [dbo].[DashboardValues_SP]    Script Date: 16-03-2024 15:30:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DashboardValues_SP] @UsrId int,@Role varchar(255)      
AS       
BEGIN      
IF @Role='Admin'      
 BEGIN      
  DECLARE @AptC int,@AptStC int,@RegC int,@PrsC int      
    SELECT @AptC = count(*) from Appointment      
  SELECT @AptStC = count(*) from Appointmentstatus_tbl      
  SELECT @RegC = count(*) from Register      
  SELECT @PrsC = count(*) from Prescription_Tbl      
  SET @AptC = @AptC       
  SET @AptStC = @AptStC       
  SET @RegC = @RegC      
      
UPDATE Dashboardvalues_tbl SET AptCount=@AptC , AptstatusCount =@AptStc, RegisterCount=@RegC, PrescriptionCount=@PrsC  WHERE Role=@Role      
      
SELECT AptCount,AptstatusCount,RegisterCount,PrescriptionCount FROM DashboardValues_tbl WHERE @Role='Admin'and UsrId=@UsrId      
      
END    
  
ELSE  
  
Select AptCount=0,AptstatusCount=0,RegisterCount=0,PrescriptionCount=0  
END    
GO
/****** Object:  StoredProcedure [dbo].[GetAllAppoinments]    Script Date: 16-03-2024 15:30:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAllAppoinments] @UsrId int                          
                          
AS                          
                          
BEGIN                          
                          
      SET NOCOUNT ON;                          
                          
      SELECT * FROM Appointment                    
                     
   select RCode,Fname,Lname,Email,Phoneno,Gender,Addressline1,Age FROM Personaldetails  where UsrId=@UsrId              
                        
   select  UsrId as DId, DFname,Dphone,Email from Doctor                  
                  
   Select Id, State from State_tbl                      
             
   select PFname,PLname,DFname,ReasonType,AptBookingdate,AptTime from Appointment where DId=@UsrId    
   
   select SlNo,Gender from Gender_tbl
                          
END 
GO
/****** Object:  StoredProcedure [dbo].[GetAllAppointmentdetails]    Script Date: 16-03-2024 15:30:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAllAppointmentdetails] @AptBookingdate datetime, @AptBokingtime time, @PFname varchar(255),@DFname varchar(255)        
          
AS          
          
BEGIN          
          
      SET NOCOUNT ON;          
          
      SELECT * from Appointment where AptBookingdate =@AptBookingdate and AptTime=@AptBokingtime and PFname=@PFname and DId=@DFname   and Aptrescheduleconfirm=0      
       
          
END  
GO
/****** Object:  StoredProcedure [dbo].[GetAllCitites]    Script Date: 16-03-2024 15:30:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAllCitites] @StateId int  
  
AS  
  
BEGIN  
  
      SET NOCOUNT ON;  
  
      SELECT CId, City 
  
      FROM City_tbl where SId=@StateId 

END
GO
/****** Object:  StoredProcedure [dbo].[GetAllEmaildetails]    Script Date: 16-03-2024 15:30:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAllEmaildetails] @Email varchar(255)  
    
AS    
    
BEGIN    
    
      SET NOCOUNT ON;    
    
      SELECT * from Register where Email =@Email  
	  SELECT * from Personaldetails where Email =@Email
    
END
GO
/****** Object:  StoredProcedure [dbo].[GetAllPersonalDetails]    Script Date: 16-03-2024 15:30:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAllPersonalDetails] @Id int                
                
AS                
                
BEGIN                
                
      SET NOCOUNT ON;                
                
   SELECT Fname,Lname,Phoneno,Email,Gender,Addressline1,Addressline2,AltAddress,Aphoneno,UniqueId,Zipcode,State,City,Country,Role,isnull(SId,0) as SId,isnull(CId,0) as CId,Photo,Perproof FROM Personaldetails where UsrId=@Id                
              
   Select Id, State from State_tbl              
          
   Select specialistid,specialistname from Doctorspecialist_tbl       
    
   Select SlNo, Gender from Gender_tbl  
          
                
END
GO
/****** Object:  StoredProcedure [dbo].[GetAllPrescriptions]    Script Date: 16-03-2024 15:30:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAllPrescriptions] @UsrId int                                      
                                      
AS                                      
                                      
BEGIN                                      
                                      
      SET NOCOUNT ON;                                      
                                   
      --SELECT distinct A.AptId, A.Aptcode FROM Appointment A INNER JOIN Prescription_tbl P ON A.AptCode = P.AptCode          
      --WHERE P.AptCode IS NOT NULL and A.DId=@UsrId          
    
     Select Aptcode from Appointment where AptCode is NOT NULL and  DId =@UsrId and  Aptbookingstatusconfirm =1 and  AptCode  NOT IN (SELECT AptCode from Prescription_Tbl)   
                         
      Select PFname,DFname,PatientEmail,Patphone,DContactno,PId,DId from Appointment where DId=@UsrId              
            
      Select  Slno,Bloodgroup from Bloodgroup_Tbl           
	   
	  Select Aptcode from Appointment Where PId=@UsrId and Aptbookingstatusconfirm=1
                                      
END 
GO
/****** Object:  StoredProcedure [dbo].[GetAllStates]    Script Date: 16-03-2024 15:30:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAllStates]   
      
AS      
      
BEGIN      
      
      SET NOCOUNT ON;      
      
      SELECT * from State_tbl  

      
END
GO
/****** Object:  StoredProcedure [dbo].[GetAllUserList]    Script Date: 16-03-2024 15:30:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAllUserList] @Role varchar(20)                
                  
AS                  
                  
BEGIN                  
                  
      SET NOCOUNT ON;                  
        
   if (@Role='Admin')  
     
      SELECT RId,RCode,Fname,Lname,Email,Phoneno FROM Register           
                 
END
GO
/****** Object:  StoredProcedure [dbo].[GetAppointmentList]    Script Date: 16-03-2024 15:30:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAppointmentList] @UsrId int, @Role varchar(255)                                   
                                    
AS                                    
                                    
BEGIN                                    
                                    
   SET NOCOUNT ON;                                    
                 
   IF(@Role ='Admin')              
     BEGIN              
  SELECT * from Appointment                
  END              
   ELSE IF(@Role ='Patient')              
    BEGIN              
  SELECT RCode, DFname,AptBookingdate,Reasontype,AptBookingdate,AptTime,DContactno,DEmail,Aptbookingconfirm ,Aptbookingstatusconfirm,Aptrescheduleconfirm FROM Appointment where UsrId= @UsrId                     
    END              
  ELSE IF(@Role ='Doctor')              
   BEGIN              
 SELECT AptId,RCode,PFname,PLname,Patphone,PatientEmail,AptBookingdate,AptTime,Reasontype,Aptrescheduleconfirm,Aptrescheduledate,Aptrescheduletime,Aptbookingconfirm ,Aptbookingstatusconfirm FROM Appointment  Where DId = @UsrId                       
   END               
                                   
END 
GO
/****** Object:  StoredProcedure [dbo].[GetDetails]    Script Date: 16-03-2024 15:30:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetDetails] @Aptcode varchar(255)               
                  
AS                  
                  
BEGIN                  
                  
   SET NOCOUNT ON;                  
            
   --SELECT AptCode,PFname,PLname,DFname,Patphone,PatientEmail,PId,DId,Patage,Patgender FROM Appointment where AptCode=@Aptcode            
           
   --SELECT AptCode,Pfname,Dfname,Slno,Prscitem,PrscDosage,Prscdays FROM Prescription_Tbl where AptCode = @Aptcode      
         
   Select Appointment.AptCode as AptCode,Appointment.PFname as PFirstname,Appointment.PLname as PLastname,Appointment.DFname as DFirstname,Appointment.Patphone as PContactno,Appointment.PatientEmail as PEmail,    
  Appointment.PId as PId,Appointment.DId as DId,Appointment.Patage as Page,Appointment.Patgender as PGender,Prsc_item_tbl.PrsCode as PrsCode,Prsc_item_tbl.Slno as Slno,Prsc_item_tbl.Prscitem as Drug,Prsc_item_tbl.Prscode as Prscode,    
 Appointment.DContactno as DContact,Appointment.DEmail as DEmail, Appointment.RCode as RCode, Appointment.Patphone as PContact, Prsc_item_tbl.PrscDosage as Dosage,Prsc_item_tbl.Prscdays as Noofdays From Appointment RIGHT JOIN Prsc_item_tbl ON Appointment.AptCode = Prsc_item_tbl.AptCode  where Appointment.AptCode =@Aptcode  
    
      
     
END
GO
/****** Object:  StoredProcedure [dbo].[GetEditAppointments]    Script Date: 16-03-2024 15:30:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetEditAppointments] @UsrId int,@Id int                            
                            
AS                            
                            
BEGIN                            
                            
      SET NOCOUNT ON;                            
                            
   SELECT * FROM Appointment                      
          
   select AptId,PFname,PLname,Patphone,PatientEmail,DFname,ReasonType,AptBookingdate,AptTime from Appointment where DId=@UsrId and AptId=@Id     
                             
END 
GO
/****** Object:  StoredProcedure [dbo].[GetModulesByRole]    Script Date: 16-03-2024 15:30:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetModulesByRole]
    @RoleId INT
AS
BEGIN
    SELECT Modules.ModuleId, ModuleName
    FROM Modules
    JOIN RoleModuleMapping ON Modules.ModuleId = RoleModuleMapping.ModuleId
    WHERE RoleModuleMapping.RoleId = @RoleId;
END;
GO
/****** Object:  StoredProcedure [dbo].[GetPrescription]    Script Date: 16-03-2024 15:30:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetPrescription] @Aptcode varchar(255)       
          
AS          
          
BEGIN          
          
   SET NOCOUNT ON;          
    
   SELECT AptCode,PFname,PLname,DFname,Patphone,PatientEmail,PId,DId,Patage,Patgender FROM Appointment where AptCode=@Aptcode    
             
END 
GO
/****** Object:  StoredProcedure [dbo].[GetPrescriptionList]    Script Date: 16-03-2024 15:30:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetPrescriptionList] @UsrId int, @Role varchar(255)                                     
                                      
AS                                      
                                      
BEGIN                                      
                                      
   SET NOCOUNT ON;                                      
                   
   IF(@Role ='Admin')                
     BEGIN                
  SELECT Prsc_head_tbl.PUId as PId,Prsc_head_tbl.DUId as DId,Prsc_item_tbl.PrsCode as Prscode,Prsc_item_tbl.AptCode as AptCode,Prsc_head_tbl.Pfname as Pfname,Prsc_head_tbl.Dfname as Dfname,Prsc_item_tbl.Slno as Slno,Prsc_item_tbl.Prscitem as Prscitem,Prsc_item_tbl.PrscDosage as PrscDosage,Prsc_item_tbl.Prscdays as Prscdays FROM Prsc_head_tbl INNER JOIN Prsc_item_tbl on Prsc_head_tbl.Aptcode= Prsc_item_tbl.Aptcode where Prsc_head_tbl.PUId =@UsrId                 
  END                
   ELSE IF(@Role ='Patient')                
    BEGIN                
  SELECT Prsc_head_tbl.PUId as PId,Prsc_head_tbl.DUId as DId,Prsc_item_tbl.PrsCode as Prscode,Prsc_item_tbl.AptCode as AptCode,Prsc_head_tbl.Pfname as Pfname,Prsc_head_tbl.Dfname as Dfname,Prsc_item_tbl.Slno as Slno,Prsc_item_tbl.Prscitem as Prscitem,Prsc_item_tbl.PrscDosage as PrscDosage,Prsc_item_tbl.Prscdays as Prscdays FROM Prsc_head_tbl INNER JOIN Prsc_item_tbl on Prsc_head_tbl.Aptcode= Prsc_item_tbl.Aptcode where Prsc_head_tbl.PUId =@UsrId                 
    END                
  ELSE IF(@Role ='Doctor')                
   BEGIN                
  SELECT Prsc_head_tbl.PUId as PId,Prsc_head_tbl.DUId as DId,Prsc_item_tbl.PrsCode as Prscode,Prsc_item_tbl.AptCode as AptCode,Prsc_head_tbl.Pfname as Pfname,Prsc_head_tbl.Dfname as Dfname,Prsc_item_tbl.Slno as Slno,Prsc_item_tbl.Prscitem as Prscitem,Prsc_item_tbl.PrscDosage as PrscDosage,Prsc_item_tbl.Prscdays as Prscdays FROM Prsc_head_tbl INNER JOIN Prsc_item_tbl on Prsc_head_tbl.Aptcode= Prsc_item_tbl.Aptcode where Prsc_head_tbl.DUId =@UsrId                 
   END                 
                                     
END 
GO
/****** Object:  StoredProcedure [dbo].[GetSelfAppointment]    Script Date: 16-03-2024 15:30:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetSelfAppointment] @UsrId int   
      
AS      
      
BEGIN      
      
   SET NOCOUNT ON;      
      
   SELECT Fname,Lname,Email,Phoneno,Age FROM Personaldetails where PerId=@UsrId
   
   SELECT PFname,PLname,AptBookingdate,AptTime FROM Appointment where DId=@UsrId 
         
END 
GO
/****** Object:  StoredProcedure [dbo].[Prescription_SP]    Script Date: 16-03-2024 15:30:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[Prescription_SP] @PatientFirstname nvarchar(255),@PatientLastname varchar(255),@DoctorFirstname varchar(255),@DoctorLastname varchar(255),                                                  
@Patgender varchar(255),@Patientage varchar(255),@PatientPhoneno varchar(25)
as                                
begin                             
declare  @PrsCode nvarchar(100)                 
declare @count int                            
declare @tblid Table(PrsId int)                
                              
insert into Prescription(PFname,PLname,DFname,DLname,Patphoneno,Patage,Patgender)                             
Output inserted.PrsId into @tblid                                        
values(@PatientFirstname,@PatientLastname,@DoctorFirstname,@DoctorLastname,@Patientphoneno,@Patientage,@Patgender)                               
select @PrsCode='PRSC-100'+cast(PrsId as varchar(100)) from Prescription where PrsId in (select PrsId from @tblid)    
Update Prescription set PrsCode=@PrsCode where PrsId in (select PrsId from @tblid)                     
      
end
GO
/****** Object:  StoredProcedure [dbo].[RescheduleAppointment_SP]    Script Date: 16-03-2024 15:30:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 CREATE PROCEDURE [dbo].[RescheduleAppointment_SP]  @AptId int, @Rescheduledate date, @Rescheduletime time  
  AS  
  BEGIN  
  Update Appointment set Aptbookingstatusconfirm=1, Aptrescheduleconfirm=1,Aptrescheduledate=@Rescheduledate, Aptrescheduletime=@Rescheduletime where AptId=@AptId  
  END
GO
/****** Object:  StoredProcedure [dbo].[SaveHeadPrescription_SP]    Script Date: 16-03-2024 15:30:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveHeadPrescription_SP]  @AptCode varchar(255),@PatFirstName varchar(255),  
@PatLastName varchar(255),@PatPhoneno varchar(14),@PatEmail varchar(255),@PatAge varchar(3),        
@PatBloodgroup varchar(10),@PatWeight varchar(14),@Dfname varchar(255)        
                                                                   
AS                                                        
BEGIN       
 DECLARE @Prscode nvarchar(255)
 DECLARE @tblid Table(PrscHId int) 
 select @Prscode=Docno from Documentcode_vw where Seriesname='Prescription' 
 insert into Prsc_head_tbl(Aptcode,Pfname,Pphoneno,Pemail,Page,PBloodgroup,Pweight,Dfname,Prscode)  
 Output inserted.PrscHId into @tblid
 values(@AptCode,@PatFirstName,@PatPhoneno,@PatEmail,@PatAge,@PatBloodgroup,@PatWeight,@Dfname,@Prscode)         
UPDATE Prsc_head_tbl  
SET pgender = Appointment.Patgender,  
    PUId = Appointment.PId,  
    DUId = Appointment.DId,  
 Dphoneno = Appointment.DContactno  
FROM Appointment  

WHERE Prsc_head_tbl.Aptcode=@AptCode  

END 
GO
/****** Object:  StoredProcedure [dbo].[SaveItemPrescription_SP]    Script Date: 16-03-2024 15:30:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveItemPrescription_SP] @Prscitem varchar(255),@Prscdosage varchar(255), @Prscdays varchar(255),@Slno int, @AptCode varchar(255)       
AS                                                          
BEGIN           
 DECLARE @Prscode nvarchar(255)    
 DECLARE @tblid Table(PrscItemId int)     
 select @Prscode=Docno from Documentcode_vw where Seriesname='Prescription'     
 insert into Prsc_item_tbl(Prscitem,PrscDosage,Prscdays,Slno,Aptcode)      
 Output inserted.PrscItemId into @tblid    
 values(@Prscitem,@Prscdosage,@Prscdays,@Slno,@AptCode)      
 UPDATE Prsc_item_tbl      
SET       
    PUId = Appointment.PId,      
    DUId = Appointment.DId	
FROM Appointment      
WHERE Prsc_item_tbl.Aptcode=@AptCode  
Update Prsc_item_tbl set PrscItemId = Prsc_head_tbl.PrscHId, Prscode=Prsc_head_tbl.Prscode from Prsc_head_tbl where Prsc_item_tbl.Aptcode=Prsc_head_tbl.Aptcode

   INSERT INTO Prescription_Tbl (Prscode,Aptcode, Pfname, Dfname, Slno, Prscitem, PrscDosage, Prscdays,PrscId)  
SELECT   
    ph.Prscode,
    ph.Aptcode,   
    ph.Pfname,   
    ph.Dfname,   
    pi.Slno,   
    pi.Prscitem,   
    pi.PrscDosage,  
    pi.Prscdays,
	pi.PrscItemId
FROM   
    Prsc_head_tbl ph  
INNER JOIN   
    Prsc_item_Tbl pi ON ph.Aptcode = pi.Aptcode   
WHERE   
    pi.Aptcode = @AptCode;  
  update DocumentcodeTbl set CurrentNo = CurrentNo +1 where Seriesname='Prescription'                 
END 
GO
/****** Object:  StoredProcedure [dbo].[SavePrescription_SP]    Script Date: 16-03-2024 15:30:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SavePrescription_SP]  @AptCode varchar(255),@PatFirstName varchar(255),@PatLastName varchar(255),@PatPhoneno varchar(14),@PatEmail varchar(255),@PatAge varchar(3),
@PatBloodgroup varchar(10),@PatWeight varchar(14),@Dfname varchar(255)
                                                           
AS                                                
BEGIN                                             
 insert into Prsc_head_tbl(Aptcode,Pfname,Pphoneno,Pemail,Page,PBloodgroup,Pweight,Dfname)                                             
 values(@AptCode,@PatFirstName,@PatPhoneno,@PatEmail,@PatAge,@PatBloodgroup,@PatWeight,@Dfname)        
END                   
GO
/****** Object:  StoredProcedure [dbo].[SaveRegister_SP]    Script Date: 16-03-2024 15:30:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveRegister_SP] @Firstname nvarchar(255),@Lastname varchar(255),@Email varchar(255),                                            
@Password varchar(255),@ConfirmPassword varchar(255),@Dob datetime,@Phone varchar(255),@Status int,                                            
@Role varchar(255),@agreeterm int, @Age int,@RCategory nvarchar(100),@Username varchar(255)        
                                                           
AS                                                
BEGIN                                             
 DECLARE  @RCode nvarchar(100)         
 DECLARE @PerCode nvarchar(100)                           
 DECLARE @count int                                            
 DECLARE @tblid Table(RId int)                           
 DECLARE @Pertblid Table (PerId int)                          
 DECLARE @Ptblid Table(PId int)                              
 DECLARE @Dtblid Table(DId int)                       
 DECLARE @Stblid Table(SId int)
 DECLARE @Atblid Table(AId int)
                  
IF( @Role='Patient') and ( @RCategory ='Patient')                 
BEGIN  
 select @RCode=Docno from Documentcode_vw where Seriesname='Patient'           
 insert into Register(Fname,Lname,Email,Password,CPassword,Username,Dob,Phoneno,Role,Status,Regdate,agreeterm,RCode)                                             
 Output inserted.RId into @tblid                                                        
 values(@Firstname,@Lastname,@Email,@Password,@ConfirmPassword,@Username,@Dob,@Phone,@Role,@Status,getdate(),@agreeterm,@RCode)        
END                   
if (@Role='Doctor') and (@RCategory = 'Employee' )    
BEGIN  
 select @RCode=Docno from Documentcode_vw where Seriesname='Doctor'                    
 select @Username=Docno from Documentcode_vw Where Seriesname='Employee'        
 insert into Register(Fname,Lname,Email,Password,CPassword,Username,Dob,Phoneno,Role,Status,Regdate,agreeterm,RCode)                                             
 Output inserted.RId into @tblid                                                        
 values(@Firstname,@Lastname,@Email,@Password,@ConfirmPassword,@Username,@Dob,@Phone,@Role,@Status,getdate(),@agreeterm,@RCode)    
END   
  
if (@Role='Staff') and ( @RCategory = 'Employee' )  
BEGIN  
  select @RCode=Docno from Documentcode_vw where Seriesname='Staff'                    
  select @Username=Docno from Documentcode_vw Where Seriesname = 'Employee'         
  insert into Register(Fname,Lname,Email,Password,CPassword,Username,Dob,Phoneno,Role,Status,Regdate,agreeterm,RCode)                                             
  Output inserted.RId into @tblid                                                        
  values(@Firstname,@Lastname,@Email,@Password,@ConfirmPassword,@Username,@Dob,@Phone,@Role,@Status,getdate(),@agreeterm,@RCode)        
END       
IF (@Role='Admin')        
BEGIN  
 select @RCode=Docno from Documentcode_vw Where Seriesname='Admin'                                      
 insert into Register(Fname,Lname,Email,Password,CPassword,Username,Dob,Phoneno,Role,Status,Regdate,agreeterm,RCode)                                             
 Output inserted.RId into @tblid                                                        
 values(@Firstname,@Lastname,@Email,@Password,@ConfirmPassword,@Username,@Dob,@Phone,@Role,@Status,getdate(),@agreeterm,@RCode)                      
END                    
                    
                    
select @PerCode=Docno from Documentcode_vw where Seriesname='Personaldetail'                                     
declare @UId int                          
select @UId=rid from @tblid                 
insert into Personaldetails(Fname,Lname,Email,Phoneno,Dob,UsrId,Role,PerCode,RCode,Age)                            
Output inserted.PerId into @Pertblid                          
values(@Firstname,@Lastname,@Email,@Phone,@Dob,@UId,@Role,@PerCode,@RCode,@Age)                      
update DocumentcodeTbl set CurrentNo = CurrentNo +1 where Seriesname='Personaldetail'                    
                    
IF (@Role='Patient')  and (@RCategory='Patient')         
BEGIN            
 declare @PUId int                          
 select @PUId=rid from @tblid                   
 insert into Patient(PFname,PLname,PEmail,Patphoneno,PDob,Percode,RCode,Patage,UsrId)                  
 Output inserted.PId into @Ptblid                   
 values(@Firstname,@Lastname,@Email,@Phone,@Dob,@PerCode,@RCode,@Age,@PUId)                     
 update DocumentcodeTbl set CurrentNo = CurrentNo +1 where Seriesname='Patient'            
END               
                    
ELSE IF (@Role ='Doctor')and (@RCategory='Employee')           
BEGIN           
 declare @DUId int                          
 select @DUId=rid from @tblid                
 insert into Doctor(DFname,DLname,Email,Dphone,DDob,Percode,RCode,Dage,UsrId)                  
 Output inserted.DId into @Dtblid                   
 values(@Firstname,@Lastname,@Email,@Phone,@Dob,@PerCode,@RCode,@Age,@DUId)                   
 update DocumentcodeTbl set CurrentNo = CurrentNo +1 where Seriesname='Doctor'             
 update DocumentcodeTbl set CurrentNo = CurrentNo +1 where Seriesname = 'Employee'        
END               
ELSE IF (@Role='Staff') and (@RCategory='Employee')             
BEGIN            
 declare @SUId int                          
 select @SUId=rid from @tblid                
 insert into Staff(SFname,SLname,SEmail,SPhone,SDob,Percode,RCode,Sage,UsrId)                  
 Output inserted.SId into @Stblid                   
 values(@Firstname,@Lastname,@Email,@Phone,@Dob,@PerCode,@RCode,@Age,@SUId)                   
 update DocumentcodeTbl set CurrentNo = CurrentNo +1 where Seriesname='Staff'             
 update DocumentcodeTbl set CurrentNo = CurrentNo +1 where Seriesname='Employee'        
END 
ELSE IF (@Role='Admin') and (@RCategory='Employee')             
BEGIN            
 declare @AUId int                          
 select @AUId=rid from @tblid                
 insert into Admin(AFname,ALname,AEmail,APhone,ADob,Percode,RCode,Aage,UsrId)                  
 Output inserted.AId into @Atblid                   
 values(@Firstname,@Lastname,@Email,@Phone,@Dob,@PerCode,@RCode,@Age,@SUId)                   
 update DocumentcodeTbl set CurrentNo = CurrentNo +1 where Seriesname='Admin'             
 update DocumentcodeTbl set CurrentNo = CurrentNo +1 where Seriesname='Employee'        
END 
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateAppointment_SP]    Script Date: 16-03-2024 15:30:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[UpdateAppointment_SP] @Aptrescheduledate date,@Aptrescheduletime time,@RCode varchar(25),@DId int,@AptId int
as begin
Update Appointment set Aptrescheduleconfirm ='1', Aptrescheduledate=@Aptrescheduledate, Aptrescheduletime = @Aptrescheduletime where DId=@DId or RCode=@RCode or AptId=@AptId
end
GO
/****** Object:  StoredProcedure [dbo].[UpdatePassword_SP]    Script Date: 16-03-2024 15:30:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[UpdatePassword_SP] @RPassword nvarchar(100), @CRPassword nvarchar(100),@Email nvarchar(100)          
as          
begin          
declare @count int      
select @count=count(*) from Register where Email=@Email              
if @count>0          
begin             
select  RId,RCode,Fname,Lname,Email,Password,CPassword,Username,Dob,Phoneno,Status,Role,1 as LStatus from Register      
where Email=@Email       
update Register set Password=@RPassword, CPassword=@CRPassword  Where  Email=@Email      
end        
else      
select 0 as LStatus       
end
GO
/****** Object:  StoredProcedure [dbo].[UpdatePersonaldetails]    Script Date: 16-03-2024 15:30:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[UpdatePersonaldetails]
@Gender nvarchar(100),@APhoneno nvarchar(100),@Addressline1 nvarchar(100),@Addressline2 nvarchar(100),@AltAddress nvarchar(100),                
@Country nvarchar(100),@Email nvarchar(100),@UniqueId  nvarchar(100), @Zipcode varchar(6),@Role varchar(20),@Photo varbinary(max),@Proof varbinary(max), @UsrId int, @statename nvarchar(100),@cityname nvarchar(100),@State nvarchar(100),@City nvarchar(100)  
AS                            
BEGIN    
DECLARE @doctorspecialist nvarchar(100), @specialistname varchar(100),@Dqualify varchar(100),@Dexp int
select @doctorspecialist =specialistname from Doctorspecialist_tbl where specialistid=@specialistname
SELECT City=@cityname FROM City_tbl WHERE CId =@City          
SELECT State=@statename FROM State_tbl WHERE Id=@State 
 Update Personaldetails set Gender=@Gender, Aphoneno=@APhoneno,Addressline1=@Addressline1,Addressline2=@Addressline2,AltAddress=@AltAddress,Country=@Country,Zipcode=@Zipcode,Photo=@Photo,Perproof=@Proof,State=@statename,City=@cityname where UsrId=@UsrId or Email=@Email

 Update Doctor set Dgender=@Gender,ADphone=@APhoneno,Daddress1=@Addressline1,Daddress2=@Addressline2,Duniqueid=@Uniqueid,DCity=@cityname,Dexp=@Dexp,Dqualification=@Dqualify,Dspecialist=@doctorspecialist,Dspecialistid=@specialistname,DProof=@Proof,DState =@statename,Email=@Email where UsrId=@UsrId  or @Role='Doctor'      
END
GO
/****** Object:  StoredProcedure [dbo].[UpdatePersonaldetails_SP]    Script Date: 16-03-2024 15:30:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdatePersonaldetails_SP] @Gender nvarchar(100),@APhoneno nvarchar(100),@Addressline1 nvarchar(100),@Addressline2 nvarchar(100),@AltAddress nvarchar(100),              
@Country nvarchar(100),@Email nvarchar(100),@UniqueId  nvarchar(100),@UsrId int,@Zipcode varchar(6),@Role varchar(20),@StateId nvarchar(100),@CityId nvarchar(100),@Photo varbinary(max),@Proof varbinary(max),  
@Empqualify varchar(255),    
@Emphistory varchar(255),    
@Empexp varchar(255),  
@pathistory nvarchar(100),     
@patdiagnosis varchar(100),    
@other varchar(100),        
@specialistid nvarchar(100),     
@Dqualify varchar(100),    
@Dexp int  
  
AS                                  
BEGIN          
DECLARE @statename varchar(255),  
@cityname varchar(255),  
@doctorspecialist varchar(100)  
SELECT * from Personaldetails where UsrId=@UsrId  
SELECT @cityname=City FROM City_tbl WHERE CId =@CityId               
SELECT @statename=State FROM State_tbl WHERE Id=@StateId  
select @doctorspecialist =specialistname from Doctorspecialist_tbl where specialistid=@specialistid   
                    
if (@Role='Patient')        
BEGIN           
  UPDATE Personaldetails set Gender=@Gender, Aphoneno=@Aphoneno, SId=@StateId,State=@statename, CId=@CityId,City=@cityname,Country=@Country, Addressline1=@Addressline1,UniqueId=@UniqueId,                      
  Addressline2=@Addressline2, AltAddress=@AltAddress,Zipcode=@Zipcode,Photo=@Photo,Perproof=@Proof,Updatedate = getdate() Where  UsrId=@UsrId                     
 update Patient set PatGender=@Gender,Pataltphone=@Aphoneno,PEmail=@Email, Pataddress1=@Addressline1,PUniqueId=@UniqueId,                      
 Pataddress2=@Addressline2,Pat_History=@pathistory,Pat_Diagnosis=@patdiagnosis,Otherdetails=@Other,PProof=@Proof,PatPhoto=@Photo  Where  UsrId=@UsrId                               
END              
IF (@Role='Doctor')         
BEGIN      
                 
 UPDATE Personaldetails set Gender=@Gender, Aphoneno=@Aphoneno, SId=@StateId,State=@statename, CId=@CityId,City=@cityname,Country=@Country, Addressline1=@Addressline1,UniqueId=@UniqueId,                      
 Addressline2=@Addressline2, AltAddress=@AltAddress,Zipcode=@Zipcode,Photo=@Photo,PerProof=@Proof,Updatedate = getdate() Where  UsrId=@UsrId                     
 UPDATE Doctor set Dgender=@Gender,ADphone=@APhoneno,Daddress1=@Addressline1,Daddress2=@Addressline2,Duniqueid=@Uniqueid,Dexp=@Dexp,Dqualification=@Dqualify,Dspecialist=@doctorspecialist,Dspecialistid=@specialistid,DState=@statename,Email=@Email,Dphoto
=@Photo,DProof=@Proof where UsrId=@UsrId             
END     
IF (@Role='Admin')   
BEGIN    
  UPDATE Personaldetails set Gender=@Gender, Aphoneno=@Aphoneno, SId=@StateId,State=@statename, CId=@CityId,City=@cityname,Country=@Country, Addressline1=@Addressline1,UniqueId=@UniqueId,                      
  Addressline2=@Addressline2, AltAddress=@AltAddress,Zipcode=@Zipcode,Photo=@Photo,Perproof=@Proof,Updatedate = getdate() Where  UsrId=@UsrId                     
 UPDATE Staff set Sgender=@Gender,Empqualify=@Empqualify,Emphistory=@Emphistory,Empexp=@Empexp,Sphoto=@Photo,SProof=@Proof    
END    
END
GO
