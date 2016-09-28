USE [master]
GO

/* ------------------------------------------------------------------------ 
        DATABASE
   ------------------------------------------------------------------------ */

/****** Object:  Database [StockDB]    Script Date: 9/27/2016 6:55:04 PM ******/
CREATE DATABASE [StockDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'StockDB', FILENAME = N'C:\Program Files (x86)\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\StockDB.mdf' , SIZE = 4096KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'StockDB_log', FILENAME = N'C:\Program Files (x86)\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\StockDB_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO

ALTER DATABASE [StockDB] SET COMPATIBILITY_LEVEL = 100
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [StockDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [StockDB] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [StockDB] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [StockDB] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [StockDB] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [StockDB] SET ARITHABORT OFF 
GO

ALTER DATABASE [StockDB] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [StockDB] SET AUTO_CREATE_STATISTICS ON 
GO

ALTER DATABASE [StockDB] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [StockDB] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [StockDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [StockDB] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [StockDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [StockDB] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [StockDB] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [StockDB] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [StockDB] SET  DISABLE_BROKER 
GO

ALTER DATABASE [StockDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [StockDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [StockDB] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [StockDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [StockDB] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [StockDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [StockDB] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [StockDB] SET RECOVERY FULL 
GO

ALTER DATABASE [StockDB] SET  MULTI_USER 
GO

ALTER DATABASE [StockDB] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [StockDB] SET DB_CHAINING OFF 
GO

ALTER DATABASE [StockDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [StockDB] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO

ALTER DATABASE [StockDB] SET  READ_WRITE 
GO

/* ------------------------------------------------------------------------ 
        ELMAH
   ------------------------------------------------------------------------ */
USE [StockDB]
GO


DECLARE @DBCompatibilityLevel INT
DECLARE @DBCompatibilityLevelMajor INT
DECLARE @DBCompatibilityLevelMinor INT

SELECT 
    @DBCompatibilityLevel = cmptlevel 
FROM 
    master.dbo.sysdatabases 
WHERE 
    name = DB_NAME()

/* ------------------------------------------------------------------------ 
        TABLES
   ------------------------------------------------------------------------ */

CREATE TABLE [dbo].[ELMAH_Error]
(
    [ErrorId]     UNIQUEIDENTIFIER NOT NULL,
    [Application] NVARCHAR(60)  COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
    [Host]        NVARCHAR(50)  COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
    [Type]        NVARCHAR(100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
    [Source]      NVARCHAR(60)  COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
    [Message]     NVARCHAR(500) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
    [User]        NVARCHAR(50)  COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
    [StatusCode]  INT NOT NULL,
    [TimeUtc]     DATETIME NOT NULL,
    [Sequence]    INT IDENTITY (1, 1) NOT NULL,
    [AllXml]      NVARCHAR(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) 
ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[ELMAH_Error] WITH NOCHECK ADD 
    CONSTRAINT [PK_ELMAH_Error] PRIMARY KEY NONCLUSTERED ([ErrorId]) ON [PRIMARY] 
GO

ALTER TABLE [dbo].[ELMAH_Error] ADD 
    CONSTRAINT [DF_ELMAH_Error_ErrorId] DEFAULT (NEWID()) FOR [ErrorId]
GO

CREATE NONCLUSTERED INDEX [IX_ELMAH_Error_App_Time_Seq] ON [dbo].[ELMAH_Error] 
(
    [Application]   ASC,
    [TimeUtc]       DESC,
    [Sequence]      DESC
) 
ON [PRIMARY]
GO

/* ------------------------------------------------------------------------ 
        STORED PROCEDURES                                                      
   ------------------------------------------------------------------------ */

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE [dbo].[ELMAH_GetErrorXml]
(
    @Application NVARCHAR(60),
    @ErrorId UNIQUEIDENTIFIER
)
AS

    SET NOCOUNT ON

    SELECT 
        [AllXml]
    FROM 
        [ELMAH_Error]
    WHERE
        [ErrorId] = @ErrorId
    AND
        [Application] = @Application

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE [dbo].[ELMAH_GetErrorsXml]
(
    @Application NVARCHAR(60),
    @PageIndex INT = 0,
    @PageSize INT = 15,
    @TotalCount INT OUTPUT
)
AS 

    SET NOCOUNT ON

    DECLARE @FirstTimeUTC DATETIME
    DECLARE @FirstSequence INT
    DECLARE @StartRow INT
    DECLARE @StartRowIndex INT

    SELECT 
        @TotalCount = COUNT(1) 
    FROM 
        [ELMAH_Error]
    WHERE 
        [Application] = @Application

    -- Get the ID of the first error for the requested page

    SET @StartRowIndex = @PageIndex * @PageSize + 1

    IF @StartRowIndex <= @TotalCount
    BEGIN

        SET ROWCOUNT @StartRowIndex

        SELECT  
            @FirstTimeUTC = [TimeUtc],
            @FirstSequence = [Sequence]
        FROM 
            [ELMAH_Error]
        WHERE   
            [Application] = @Application
        ORDER BY 
            [TimeUtc] DESC, 
            [Sequence] DESC

    END
    ELSE
    BEGIN

        SET @PageSize = 0

    END

    -- Now set the row count to the requested page size and get
    -- all records below it for the pertaining application.

    SET ROWCOUNT @PageSize

    SELECT 
        errorId     = [ErrorId], 
        application = [Application],
        host        = [Host], 
        type        = [Type],
        source      = [Source],
        message     = [Message],
        [user]      = [User],
        statusCode  = [StatusCode], 
        time        = CONVERT(VARCHAR(50), [TimeUtc], 126) + 'Z'
    FROM 
        [ELMAH_Error] error
    WHERE
        [Application] = @Application
    AND
        [TimeUtc] <= @FirstTimeUTC
    AND 
        [Sequence] <= @FirstSequence
    ORDER BY
        [TimeUtc] DESC, 
        [Sequence] DESC
    FOR
        XML AUTO

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE [dbo].[ELMAH_LogError]
(
    @ErrorId UNIQUEIDENTIFIER,
    @Application NVARCHAR(60),
    @Host NVARCHAR(30),
    @Type NVARCHAR(100),
    @Source NVARCHAR(60),
    @Message NVARCHAR(500),
    @User NVARCHAR(50),
    @AllXml NVARCHAR(MAX),
    @StatusCode INT,
    @TimeUtc DATETIME
)
AS

    SET NOCOUNT ON

    INSERT
    INTO
        [ELMAH_Error]
        (
            [ErrorId],
            [Application],
            [Host],
            [Type],
            [Source],
            [Message],
            [User],
            [AllXml],
            [StatusCode],
            [TimeUtc]
        )
    VALUES
        (
            @ErrorId,
            @Application,
            @Host,
            @Type,
            @Source,
            @Message,
            @User,
            @AllXml,
            @StatusCode,
            @TimeUtc
        )

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


