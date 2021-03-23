

USE [seu-banco-de-dados]
GO
/****** Object:  Table [dbo].[TbBuilds]    Script Date: 23/03/2021 15:20:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbBuilds](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProjectId] [varchar](50) NOT NULL,
	[Definition_Id] [int] NOT NULL,
	[BuildId] [int] NOT NULL,
	[BuildNumber] [varchar](max) NULL,
	[BuildNumberRevision] [int] NULL,
	[QueueTime] [datetime] NULL,
	[StartTime] [datetime] NULL,
	[FinishTime] [datetime] NULL,
	[BuildUri] [varchar](max) NULL,
	[BuildUrl] [varchar](max) NULL,
	[Definition_Drafts] [varchar](max) NULL,
	[Definition_Name] [varchar](max) NULL,
	[Definition_Url] [varchar](max) NULL,
	[KeepForever] [bit] NULL,
	[LastChangeDate] [datetime] NULL,
	[Links_Badge] [varchar](max) NULL,
	[Links_Self] [varchar](max) NULL,
	[Links_SourceVersionDisplayUri] [varchar](max) NULL,
	[Links_Timeline] [varchar](max) NULL,
	[Links_Web] [varchar](max) NULL,
	[LogUrl] [varchar](max) NULL,
	[OrchestrationPlan] [varchar](max) NULL,
	[Plans] [varchar](max) NULL,
	[PoolId] [varchar](max) NULL,
	[Priority] [varchar](max) NULL,
	[ProjectUrl] [varchar](max) NULL,
	[Properties] [varchar](max) NULL,
	[QueueId] [varchar](max) NULL,
	[QueueName] [varchar](max) NULL,
	[Reason] [varchar](max) NULL,
	[RepositoryId] [varchar](50) NULL,
	[RepositoryUrl] [varchar](max) NULL,
	[RequestedForAadDescriptor] [varchar](max) NULL,
	[RequestedForId] [varchar](max) NULL,
	[RequestedForIdentitieLink] [varchar](max) NULL,
	[RequestedForName] [varchar](max) NULL,
	[RequestedForUniqueName] [varchar](max) NULL,
	[Result] [varchar](max) NULL,
	[RetainedByRelease] [bit] NULL,
	[SourceBranch] [varchar](max) NULL,
	[SourceVersion] [varchar](max) NULL,
	[Status] [varchar](max) NULL,
	[Tags] [varchar](max) NULL,
	[TriggeredByBuild] [varchar](max) NULL,
	[TriggerInfo] [varchar](max) NULL,
	[ValidationResults] [varchar](max) NULL,
	[LoadDate] [datetime] NULL,
	[IsActive] [bit] NULL,
 CONSTRAINT [PK_TbBuilds] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbCommits]    Script Date: 23/03/2021 15:20:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbCommits](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CommitId] [varchar](50) NOT NULL,
	[RepositoryId] [varchar](50) NOT NULL,
	[Comment] [nvarchar](max) NULL,
	[Url] [nvarchar](max) NULL,
	[RemoteUrl] [nvarchar](max) NULL,
	[CommentTruncated] [bit] NULL,
	[AuthorName] [nvarchar](max) NULL,
	[AuthorEmail] [nvarchar](max) NULL,
	[AuthorDate] [datetime] NOT NULL,
	[CommitterName] [nvarchar](max) NULL,
	[CommitterEmail] [nvarchar](max) NULL,
	[CommitterDate] [datetime] NOT NULL,
	[ChangeCountsAdd] [int] NOT NULL,
	[ChangeCountsEdit] [int] NOT NULL,
	[ChangeCountsDelete] [int] NOT NULL,
	[LoadDate] [datetime] NOT NULL,
 CONSTRAINT [PK_TbCommits] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbGroups]    Script Date: 23/03/2021 15:20:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbGroups](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProjectId] [varchar](50) NOT NULL,
	[OurGroupCode] [int] NOT NULL,
	[Domain] [nvarchar](max) NULL,
	[PrincipalName] [nvarchar](max) NULL,
	[MailAddress] [nvarchar](max) NULL,
	[Origin] [nvarchar](max) NULL,
	[OriginId] [nvarchar](50) NOT NULL,
	[DisplayName] [nvarchar](max) NULL,
	[Descriptor] [nvarchar](max) NULL,
	[Url] [nvarchar](max) NULL,
	[LoadDate] [datetime] NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_TbGroups] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbLoadHistory]    Script Date: 23/03/2021 15:20:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbLoadHistory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Method] [int] NOT NULL,
	[FromDate] [datetime] NULL,
	[ToDate] [datetime] NULL,
	[LoadDate] [datetime] NOT NULL,
	[ExecutionDateTime] [datetime] NOT NULL,
	[Message] [varchar](max) NOT NULL,
	[IsSuccessful] [bit] NOT NULL,
 CONSTRAINT [PK_UpdateControl] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbLoadRestorePoint]    Script Date: 23/03/2021 15:20:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbLoadRestorePoint](
	[Id] [int] NOT NULL,
	[Method] [int] NOT NULL,
	[FromDate] [datetime] NULL,
	[ToDate] [datetime] NULL,
	[LastId] [varchar](max) NULL,
	[LoadDate] [datetime] NOT NULL,
	[ExecutionDateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_TbLoadRestorePoint] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbPipelineRuns]    Script Date: 23/03/2021 15:20:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbPipelineRuns](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PipelineRunId] [int] NOT NULL,
	[PipelineId] [int] NOT NULL,
	[ProjectId] [varchar](50) NULL,
	[State] [nvarchar](max) NULL,
	[Result] [nvarchar](max) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[FinishedDate] [datetime] NULL,
	[Url] [nvarchar](max) NULL,
	[Name] [nvarchar](max) NULL,
	[SelfLink] [nvarchar](max) NULL,
	[WebLink] [nvarchar](max) NULL,
	[LoadDate] [datetime] NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_TbPipelineRuns] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbPipelines]    Script Date: 23/03/2021 15:20:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbPipelines](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PipelineId] [int] NOT NULL,
	[ProjectId] [varchar](50) NOT NULL,
	[Url] [nvarchar](max) NULL,
	[SelfLink] [nvarchar](max) NULL,
	[WebLink] [nvarchar](max) NULL,
	[Revision] [int] NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Folder] [nvarchar](max) NULL,
	[LoadDate] [datetime] NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_TbPipelines] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbProjects]    Script Date: 23/03/2021 15:20:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbProjects](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProjectId] [varchar](50) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[Url] [nvarchar](max) NULL,
	[State] [nvarchar](max) NULL,
	[Revision] [int] NOT NULL,
	[Visibility] [nvarchar](max) NULL,
	[LastUpdateTime] [datetime2](7) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[LoadDate] [datetime] NOT NULL,
 CONSTRAINT [PK_TbProjects] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbReleaseApprovals]    Script Date: 23/03/2021 15:20:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbReleaseApprovals](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ApprovalId] [int] NOT NULL,
	[ReleaseId] [int] NOT NULL,
	[DefinitionId] [int] NOT NULL,
	[EnvironmentId] [int] NOT NULL,
	[ProjectId] [varchar](50) NOT NULL,
	[Revision] [int] NULL,
	[Approver] [nvarchar](max) NULL,
	[ApproverUrl] [nvarchar](max) NULL,
	[ApproverId] [nvarchar](max) NULL,
	[ApproverUniqueName] [nvarchar](max) NULL,
	[ApproverImageUrl] [nvarchar](max) NULL,
	[ApproverDescriptor] [nvarchar](max) NULL,
	[ApprovalType] [nvarchar](max) NULL,
	[ApprovedBy] [nvarchar](max) NULL,
	[ApprovedById] [nvarchar](max) NULL,
	[ApprovedByImageUrl] [nvarchar](max) NULL,
	[ApprovedByUniqueName] [nvarchar](max) NULL,
	[ApprovedByUrl] [nvarchar](max) NULL,
	[ApprovedByDescriptor] [nvarchar](max) NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedOn] [datetime] NULL,
	[Status] [nvarchar](max) NULL,
	[Comments] [nvarchar](max) NULL,
	[IsAutomated] [bit] NULL,
	[IsNotificationOn] [bit] NULL,
	[TrialNumber] [int] NULL,
	[Attempt] [int] NULL,
	[Rank] [int] NULL,
	[ReleaseName] [nvarchar](max) NULL,
	[ReleaseUrl] [nvarchar](max) NULL,
	[DefinitionName] [nvarchar](max) NULL,
	[DefinitionPath] [nvarchar](max) NULL,
	[DefinitionUrl] [nvarchar](max) NULL,
	[EnvironmentName] [nvarchar](max) NULL,
	[EnvironmentUrl] [nvarchar](max) NULL,
	[Url] [nvarchar](max) NULL,
	[LoadDate] [datetime] NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_TbReleaseApprovals] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbReleaseDefinitions]    Script Date: 23/03/2021 15:20:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbReleaseDefinitions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DefinitionId] [int] NOT NULL,
	[ProjectId] [varchar](50) NOT NULL,
	[Source] [nvarchar](max) NULL,
	[Revision] [int] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedByUrl] [nvarchar](max) NULL,
	[CreatedById] [nvarchar](max) NULL,
	[CreatedByUniqueName] [nvarchar](max) NULL,
	[CreatedByImageUrl] [nvarchar](max) NULL,
	[CreatedByDescriptor] [nvarchar](max) NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[ModifiedByUrl] [nvarchar](max) NULL,
	[ModifiedById] [nvarchar](max) NULL,
	[ModifiedByUniqueName] [nvarchar](max) NULL,
	[ModifiedByImageUrl] [nvarchar](max) NULL,
	[ModifiedByDescriptor] [nvarchar](max) NULL,
	[ModifiedOn] [datetime] NULL,
	[IsDeleted] [bit] NULL,
	[ReleaseNameFormat] [nvarchar](max) NULL,
	[Name] [nvarchar](max) NULL,
	[Path] [nvarchar](max) NULL,
	[Url] [nvarchar](max) NULL,
	[SelfLink] [nvarchar](max) NULL,
	[WebLink] [nvarchar](max) NULL,
	[LoadDate] [datetime] NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_TbReleaseDefinitions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbReleases]    Script Date: 23/03/2021 15:20:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbReleases](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ReleaseId] [int] NOT NULL,
	[DefinitionId] [int] NOT NULL,
	[ProjectId] [varchar](50) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Status] [nvarchar](max) NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[ModifiedByUrl] [nvarchar](max) NULL,
	[ModifiedById] [nvarchar](max) NULL,
	[ModifiedByUniqueName] [nvarchar](max) NULL,
	[ModifiedByImageUrl] [nvarchar](max) NULL,
	[ModifiedByDescriptor] [nvarchar](max) NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedByUrl] [nvarchar](max) NULL,
	[CreatedById] [nvarchar](max) NULL,
	[CreatedByUniqueName] [nvarchar](max) NULL,
	[CreatedByImageUrl] [nvarchar](max) NULL,
	[CreatedByDescriptor] [nvarchar](max) NULL,
	[CreatedFor] [nvarchar](max) NULL,
	[CreatedForUrl] [nvarchar](max) NULL,
	[CreatedForId] [nvarchar](max) NULL,
	[CreatedForUniqueName] [nvarchar](max) NULL,
	[CreatedForImageUrl] [nvarchar](max) NULL,
	[CreatedForDescriptor] [nvarchar](max) NULL,
	[Variables] [nvarchar](max) NULL,
	[VariableGroups] [nvarchar](max) NULL,
	[DefinitionName] [nvarchar](max) NULL,
	[DefinitionPath] [nvarchar](max) NULL,
	[DefinitionUrl] [nvarchar](max) NULL,
	[DefinitionSelfLink] [nvarchar](max) NULL,
	[DefinitionWebLink] [nvarchar](max) NULL,
	[ReleaseDefinitionRevision] [int] NULL,
	[Description] [nvarchar](max) NULL,
	[Reason] [nvarchar](max) NULL,
	[ReleaseNameFormat] [nvarchar](max) NULL,
	[KeepForever] [bit] NULL,
	[DefinitionSnapshotRevision] [int] NULL,
	[LogsContainerUrl] [nvarchar](max) NULL,
	[Url] [nvarchar](max) NULL,
	[SelfLink] [nvarchar](max) NULL,
	[WebLink] [nvarchar](max) NULL,
	[Tags] [nvarchar](max) NULL,
	[TriggeringArtifactAlias] [nvarchar](max) NULL,
	[ProjectName] [nvarchar](max) NULL,
	[LoadDate] [datetime] NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_TbReleases] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbRepositories]    Script Date: 23/03/2021 15:20:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbRepositories](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RepositoryId] [varchar](50) NOT NULL,
	[ProjectId] [varchar](50) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Url] [nvarchar](max) NULL,
	[DefaultBranch] [nvarchar](max) NULL,
	[Size] [int] NOT NULL,
	[RemoteUrl] [nvarchar](max) NULL,
	[SshUrl] [nvarchar](max) NULL,
	[WebUrl] [nvarchar](max) NULL,
	[IsActive] [bit] NOT NULL,
	[LoadDate] [datetime] NOT NULL,
 CONSTRAINT [PK_TbRepositories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


-- insert na tabela TbLoadRestorePoint
INSERT INTO [dbo].[TbLoadRestorePoint] VALUES (1,1,null,null,null,getdate(),getdate())
INSERT INTO [dbo].[TbLoadRestorePoint] VALUES (2,1,null,null,null,getdate(),getdate())
INSERT INTO [dbo].[TbLoadRestorePoint] VALUES (3,1,null,null,null,getdate(),getdate())
INSERT INTO [dbo].[TbLoadRestorePoint] VALUES (4,1,null,null,null,getdate(),getdate())
INSERT INTO [dbo].[TbLoadRestorePoint] VALUES (5,1,null,null,null,getdate(),getdate())
INSERT INTO [dbo].[TbLoadRestorePoint] VALUES (6,1,null,null,null,getdate(),getdate())
INSERT INTO [dbo].[TbLoadRestorePoint] VALUES (7,1,null,null,null,getdate(),getdate())
INSERT INTO [dbo].[TbLoadRestorePoint] VALUES (8,1,null,null,null,getdate(),getdate())
INSERT INTO [dbo].[TbLoadRestorePoint] VALUES (9,1,null,null,null,getdate(),getdate())
INSERT INTO [dbo].[TbLoadRestorePoint] VALUES (10,10,null,null,null,getdate(),getdate())
INSERT INTO [dbo].[TbLoadRestorePoint] VALUES (11,11,null,null,null,getdate(),getdate())
INSERT INTO [dbo].[TbLoadRestorePoint] VALUES (12,12,null,null,null,getdate(),getdate())
INSERT INTO [dbo].[TbLoadRestorePoint] VALUES (13,13,null,null,null,getdate(),getdate())
INSERT INTO [dbo].[TbLoadRestorePoint] VALUES (14,14,null,null,null,getdate(),getdate())
INSERT INTO [dbo].[TbLoadRestorePoint] VALUES (15,15,null,null,null,getdate(),getdate())
INSERT INTO [dbo].[TbLoadRestorePoint] VALUES (16,16,null,null,null,getdate(),getdate())

