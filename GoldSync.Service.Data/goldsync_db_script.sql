USE [goldsync]
GO
DROP PROCEDURE IF EXISTS [goldsync].[AddLogProcessWithData]
GO
DROP PROCEDURE IF EXISTS [goldsync].[AddLogProcess]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[goldsync].[LogProcessData]') AND type in (N'U'))
ALTER TABLE [goldsync].[LogProcessData] DROP CONSTRAINT IF EXISTS [FK_GoldSync_LogProcessData_ProcessId__LogProcess]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[goldsync].[LogProcess]') AND type in (N'U'))
ALTER TABLE [goldsync].[LogProcess] DROP CONSTRAINT IF EXISTS [FK_GoldSync_LogProcess_SourceId__ListSources]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[goldsync].[LogProcess]') AND type in (N'U'))
ALTER TABLE [goldsync].[LogProcess] DROP CONSTRAINT IF EXISTS [FK_GoldSync_LogProcess_ProcessStatusId__ListProcessStatus]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[goldsync].[LogProcessData]') AND type in (N'U'))
ALTER TABLE [goldsync].[LogProcessData] DROP CONSTRAINT IF EXISTS [DF_GoldSync_LogProcessData_CreatedUtc]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[goldsync].[LogProcess]') AND type in (N'U'))
ALTER TABLE [goldsync].[LogProcess] DROP CONSTRAINT IF EXISTS [DF_GoldSync_LogProcess_Workstation]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[goldsync].[LogProcess]') AND type in (N'U'))
ALTER TABLE [goldsync].[LogProcess] DROP CONSTRAINT IF EXISTS [DF_GoldSync_LogProcess_CreatedBy]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[goldsync].[LogProcess]') AND type in (N'U'))
ALTER TABLE [goldsync].[LogProcess] DROP CONSTRAINT IF EXISTS [DF_GoldSync_LogProcess_CreatedUtc]
GO
DROP TABLE IF EXISTS [goldsync].[LogProcessData]
GO
DROP TABLE IF EXISTS [goldsync].[LogProcess]
GO
DROP TABLE IF EXISTS [goldsync].[ListSources]
GO
DROP TABLE IF EXISTS [goldsync].[ListProcessStatus]
GO
DROP SCHEMA IF EXISTS [goldsync]
GO
CREATE SCHEMA [goldsync]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [goldsync].[ListProcessStatus](
	[processStatusId] [int] NOT NULL,
	[processStatusName] [varchar](32) NOT NULL,
 CONSTRAINT [PK_GoldSync_ListProcessStatus_ProcessStatusId] PRIMARY KEY CLUSTERED 
(
	[processStatusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [goldsync].[ListSources](
	[sourceId] [int] NOT NULL,
	[sourceName] [varchar](32) NOT NULL,
	[sourceDescription] [varchar](255) NULL,
 CONSTRAINT [PK_GoldSync_ListSources_SourceId] PRIMARY KEY CLUSTERED 
(
	[sourceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [goldsync].[LogProcess](
	[processId] [int] IDENTITY(1,1) NOT NULL,
	[sourceId] [int] NOT NULL,
	[processStatusId] [int] NOT NULL,
	[createdUtc] [datetime2](3) NOT NULL,
	[createdBy] [nvarchar](255) NOT NULL,
	[workstation] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_GoldSync_LogProcess_ProcessId] PRIMARY KEY CLUSTERED 
(
	[processId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [goldsync].[LogProcessData]    Script Date: 2023-02-18 19:42:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [goldsync].[LogProcessData](
	[processId] [int] NOT NULL,
	[createdUtc] [datetime2](3) NOT NULL,
	[processData] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_GoldSync_LogProcessData_ProcessId] PRIMARY KEY CLUSTERED 
(
	[processId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [goldsync].[ListProcessStatus] ([processStatusId], [processStatusName]) VALUES (1, N'Started')
GO
INSERT [goldsync].[ListProcessStatus] ([processStatusId], [processStatusName]) VALUES (2, N'Stopped')
GO
INSERT [goldsync].[ListProcessStatus] ([processStatusId], [processStatusName]) VALUES (3, N'Executing')
GO
INSERT [goldsync].[ListProcessStatus] ([processStatusId], [processStatusName]) VALUES (4, N'Processed')
GO
INSERT [goldsync].[ListProcessStatus] ([processStatusId], [processStatusName]) VALUES (5, N'Skipped')
GO
INSERT [goldsync].[ListProcessStatus] ([processStatusId], [processStatusName]) VALUES (6, N'Canceled')
GO
INSERT [goldsync].[ListProcessStatus] ([processStatusId], [processStatusName]) VALUES (7, N'Failed')
GO
INSERT [goldsync].[ListProcessStatus] ([processStatusId], [processStatusName]) VALUES (8, N'Setup')
GO
INSERT [goldsync].[ListSources] ([sourceId], [sourceName], [sourceDescription]) VALUES (1, N'Gold Sync Central', N'Flow for Gold Central load in the Host with the background service for periodic data synchronization.')
GO
ALTER TABLE [goldsync].[LogProcess] ADD  CONSTRAINT [DF_GoldSync_LogProcess_CreatedUtc]  DEFAULT (getutcdate()) FOR [createdUtc]
GO
ALTER TABLE [goldsync].[LogProcess] ADD  CONSTRAINT [DF_GoldSync_LogProcess_CreatedBy]  DEFAULT (original_login()) FOR [createdBy]
GO
ALTER TABLE [goldsync].[LogProcess] ADD  CONSTRAINT [DF_GoldSync_LogProcess_Workstation]  DEFAULT (host_name()) FOR [workstation]
GO
ALTER TABLE [goldsync].[LogProcessData] ADD  CONSTRAINT [DF_GoldSync_LogProcessData_CreatedUtc]  DEFAULT (getutcdate()) FOR [createdUtc]
GO
ALTER TABLE [goldsync].[LogProcess]  WITH CHECK ADD  CONSTRAINT [FK_GoldSync_LogProcess_ProcessStatusId__ListProcessStatus] FOREIGN KEY([processStatusId])
REFERENCES [goldsync].[ListProcessStatus] ([processStatusId])
GO
ALTER TABLE [goldsync].[LogProcess] CHECK CONSTRAINT [FK_GoldSync_LogProcess_ProcessStatusId__ListProcessStatus]
GO
ALTER TABLE [goldsync].[LogProcess]  WITH CHECK ADD  CONSTRAINT [FK_GoldSync_LogProcess_SourceId__ListSources] FOREIGN KEY([sourceId])
REFERENCES [goldsync].[ListSources] ([sourceId])
GO
ALTER TABLE [goldsync].[LogProcess] CHECK CONSTRAINT [FK_GoldSync_LogProcess_SourceId__ListSources]
GO
ALTER TABLE [goldsync].[LogProcessData]  WITH CHECK ADD  CONSTRAINT [FK_GoldSync_LogProcessData_ProcessId__LogProcess] FOREIGN KEY([processId])
REFERENCES [goldsync].[LogProcess] ([processId])
GO
ALTER TABLE [goldsync].[LogProcessData] CHECK CONSTRAINT [FK_GoldSync_LogProcessData_ProcessId__LogProcess]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [goldsync].[AddLogProcess]
(
  @sourceId INT,
  @processStatusId INT,
  @processId INT OUTPUT
)
AS
BEGIN

  SET NOCOUNT ON;

  INSERT INTO goldsync.LogProcess
    (sourceId, processStatusId)
  VALUES 
    (@sourceId, @processStatusId);

  SET @processId = SCOPE_IDENTITY();

END;
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [goldsync].[AddLogProcessWithData]
(
  @processId INT,
  @processData NVARCHAR(MAX)
)
AS
BEGIN

  SET NOCOUNT ON;

  INSERT INTO goldsync.LogProcessData
    (processId, processData)
  VALUES 
    (@processId, @processData);

END;
GO
