USE [master]
GO
/****** Object:  Database [iMultiplanSecure]    Script Date: 9/20/2017 10:26:55 AM ******/
CREATE DATABASE [iMultiplanSecure]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'iMultiplanSecure', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SERVER2012\MSSQL\DATA\iMultiplanSecure.mdf' , SIZE = 4096KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'iMultiplanSecure_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SERVER2012\MSSQL\DATA\iMultiplanSecure_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [iMultiplanSecure] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [iMultiplanSecure].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [iMultiplanSecure] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [iMultiplanSecure] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [iMultiplanSecure] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [iMultiplanSecure] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [iMultiplanSecure] SET ARITHABORT OFF 
GO
ALTER DATABASE [iMultiplanSecure] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [iMultiplanSecure] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [iMultiplanSecure] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [iMultiplanSecure] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [iMultiplanSecure] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [iMultiplanSecure] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [iMultiplanSecure] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [iMultiplanSecure] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [iMultiplanSecure] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [iMultiplanSecure] SET  DISABLE_BROKER 
GO
ALTER DATABASE [iMultiplanSecure] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [iMultiplanSecure] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [iMultiplanSecure] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [iMultiplanSecure] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [iMultiplanSecure] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [iMultiplanSecure] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [iMultiplanSecure] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [iMultiplanSecure] SET RECOVERY FULL 
GO
ALTER DATABASE [iMultiplanSecure] SET  MULTI_USER 
GO
ALTER DATABASE [iMultiplanSecure] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [iMultiplanSecure] SET DB_CHAINING OFF 
GO
ALTER DATABASE [iMultiplanSecure] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [iMultiplanSecure] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
EXEC sys.sp_db_vardecimal_storage_format N'iMultiplanSecure', N'ON'
GO
USE [iMultiplanSecure]
GO
/****** Object:  Table [dbo].[Group]    Script Date: 9/20/2017 10:26:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Group](
	[ID_Group] [int] IDENTITY(1,1) NOT NULL,
	[GroupName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Group] PRIMARY KEY CLUSTERED 
(
	[ID_Group] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[GroupMenu]    Script Date: 9/20/2017 10:26:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GroupMenu](
	[ID_GroupMenu] [int] IDENTITY(1,1) NOT NULL,
	[ID_Group] [int] NOT NULL,
	[ID_Menu] [int] NOT NULL,
 CONSTRAINT [PK_GroupMenu] PRIMARY KEY CLUSTERED 
(
	[ID_GroupMenu] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Menu]    Script Date: 9/20/2017 10:26:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Menu](
	[ID_Menu] [int] IDENTITY(1,1) NOT NULL,
	[Controller] [nvarchar](150) NULL,
	[Action] [nvarchar](50) NULL,
	[Area] [nvarchar](50) NULL,
	[ParentId] [int] NULL,
	[DisplayOrder] [int] NULL,
	[Icon] [nvarchar](50) NULL,
	[MenuText] [nvarchar](50) NOT NULL,
	[HasChildren] [bit] NOT NULL CONSTRAINT [DF_Menu_HasChildres]  DEFAULT ((0)),
 CONSTRAINT [PK_Menu] PRIMARY KEY CLUSTERED 
(
	[ID_Menu] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[User]    Script Date: 9/20/2017 10:26:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[ID_User] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](1500) NOT NULL,
	[Branch] [nvarchar](50) NOT NULL,
	[Status] [bit] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateLastUpdated] [datetime] NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[ID_User] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserGroup]    Script Date: 9/20/2017 10:26:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserGroup](
	[ID_UserGroup] [int] IDENTITY(1,1) NOT NULL,
	[ID_User] [int] NOT NULL,
	[ID_Group] [int] NOT NULL,
 CONSTRAINT [PK_UserGroup] PRIMARY KEY CLUSTERED 
(
	[ID_UserGroup] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[Group] ON 

GO
INSERT [dbo].[Group] ([ID_Group], [GroupName]) VALUES (1, N'Group A')
GO
INSERT [dbo].[Group] ([ID_Group], [GroupName]) VALUES (2, N'Group B')
GO
INSERT [dbo].[Group] ([ID_Group], [GroupName]) VALUES (3, N'Group C')
GO
INSERT [dbo].[Group] ([ID_Group], [GroupName]) VALUES (4, N'Group D')
GO
SET IDENTITY_INSERT [dbo].[Group] OFF
GO
SET IDENTITY_INSERT [dbo].[Menu] ON 

GO
INSERT [dbo].[Menu] ([ID_Menu], [Controller], [Action], [Area], [ParentId], [DisplayOrder], [Icon], [MenuText], [HasChildren]) VALUES (1, N'Admin', N'IndexAdmin', N'Admin', NULL, 1, N'fa-dashboard', N'Dashboard', 0)
GO
INSERT [dbo].[Menu] ([ID_Menu], [Controller], [Action], [Area], [ParentId], [DisplayOrder], [Icon], [MenuText], [HasChildren]) VALUES (2, N'', N'', N'', NULL, 2, N'fa-briefcase', N'Underwriting', 1)
GO
INSERT [dbo].[Menu] ([ID_Menu], [Controller], [Action], [Area], [ParentId], [DisplayOrder], [Icon], [MenuText], [HasChildren]) VALUES (3, N'Customers', N'Customers_AssuredRegister', N'Admin', 2, 3, NULL, N'Customer', 0)
GO
INSERT [dbo].[Menu] ([ID_Menu], [Controller], [Action], [Area], [ParentId], [DisplayOrder], [Icon], [MenuText], [HasChildren]) VALUES (4, N'Proposals', N'Proposals', N'Admin', 2, 4, NULL, N'Proposals', 0)
GO
INSERT [dbo].[Menu] ([ID_Menu], [Controller], [Action], [Area], [ParentId], [DisplayOrder], [Icon], [MenuText], [HasChildren]) VALUES (5, N'UserAdmin', N'index', N' ', NULL, 2, N'fa-users', N'User Management', 0)
GO
SET IDENTITY_INSERT [dbo].[Menu] OFF
GO
SET IDENTITY_INSERT [dbo].[User] ON 

GO
INSERT [dbo].[User] ([ID_User], [FirstName], [LastName], [Username], [Password], [Branch], [Status], [DateCreated], [DateLastUpdated]) VALUES (1, N'Simplex', N'Buz Solutions', N'simplex', N'40CEC5314C3C4BF833DE2C897A72BD9D5676D19A967043323DC55584102BD18D8BE52E2FC1416590735DBB72299F2909EFCB4CCFA01F070055CAE23311AC9F9B', N'HQR', 1, CAST(N'2017-09-19 15:27:46.920' AS DateTime), CAST(N'2017-09-19 15:27:46.920' AS DateTime))
GO
INSERT [dbo].[User] ([ID_User], [FirstName], [LastName], [Username], [Password], [Branch], [Status], [DateCreated], [DateLastUpdated]) VALUES (3, N'Adesina', N'Mark Omoniyi', N'mark2k', N'6ABECA58D1968532D5E0045C51100AF9E85B75BA525137C913CD3C8CBEB9ECC3AF3B4D9B8A7B70C8093B222B3317A69DBE85A15F0FD5DC7D8675A4EB3223AF88', N'HQR', 1, CAST(N'2017-09-20 06:14:32.827' AS DateTime), CAST(N'2017-09-20 06:14:32.827' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[User] OFF
GO
SET IDENTITY_INSERT [dbo].[UserGroup] ON 

GO
INSERT [dbo].[UserGroup] ([ID_UserGroup], [ID_User], [ID_Group]) VALUES (4, 3, 1)
GO
INSERT [dbo].[UserGroup] ([ID_UserGroup], [ID_User], [ID_Group]) VALUES (5, 3, 2)
GO
INSERT [dbo].[UserGroup] ([ID_UserGroup], [ID_User], [ID_Group]) VALUES (6, 3, 3)
GO
INSERT [dbo].[UserGroup] ([ID_UserGroup], [ID_User], [ID_Group]) VALUES (7, 3, 4)
GO
SET IDENTITY_INSERT [dbo].[UserGroup] OFF
GO
ALTER TABLE [dbo].[GroupMenu]  WITH CHECK ADD  CONSTRAINT [FK_GroupMenu_Group] FOREIGN KEY([ID_Group])
REFERENCES [dbo].[Group] ([ID_Group])
GO
ALTER TABLE [dbo].[GroupMenu] CHECK CONSTRAINT [FK_GroupMenu_Group]
GO
ALTER TABLE [dbo].[GroupMenu]  WITH CHECK ADD  CONSTRAINT [FK_GroupMenu_Menu] FOREIGN KEY([ID_Menu])
REFERENCES [dbo].[Menu] ([ID_Menu])
GO
ALTER TABLE [dbo].[GroupMenu] CHECK CONSTRAINT [FK_GroupMenu_Menu]
GO
ALTER TABLE [dbo].[Menu]  WITH CHECK ADD  CONSTRAINT [FK_Menu_Menu] FOREIGN KEY([ParentId])
REFERENCES [dbo].[Menu] ([ID_Menu])
GO
ALTER TABLE [dbo].[Menu] CHECK CONSTRAINT [FK_Menu_Menu]
GO
ALTER TABLE [dbo].[UserGroup]  WITH CHECK ADD  CONSTRAINT [FK_UserGroup_Group] FOREIGN KEY([ID_Group])
REFERENCES [dbo].[Group] ([ID_Group])
GO
ALTER TABLE [dbo].[UserGroup] CHECK CONSTRAINT [FK_UserGroup_Group]
GO
ALTER TABLE [dbo].[UserGroup]  WITH CHECK ADD  CONSTRAINT [FK_UserGroup_User] FOREIGN KEY([ID_User])
REFERENCES [dbo].[User] ([ID_User])
GO
ALTER TABLE [dbo].[UserGroup] CHECK CONSTRAINT [FK_UserGroup_User]
GO
USE [master]
GO
ALTER DATABASE [iMultiplanSecure] SET  READ_WRITE 
GO
