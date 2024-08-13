
GO
/*
Table [dbo].[CommiteesDataEntry] is being dropped.  Deployment will halt if the table contains data.
*/

IF EXISTS (select top 1 1 from [dbo].[CommiteesDataEntry])
    RAISERROR (N'Rows were detected. The schema update is terminating because data loss might occur.', 16, 127) WITH NOWAIT

GO
/*
Table [dbo].[UsersDataEntry] is being dropped.  Deployment will halt if the table contains data.
*/

IF EXISTS (select top 1 1 from [dbo].[UsersDataEntry])
    RAISERROR (N'Rows were detected. The schema update is terminating because data loss might occur.', 16, 127) WITH NOWAIT

GO
PRINT N'Dropping Default Constraint unnamed constraint on [dbo].[MainUsers]...';


GO
ALTER TABLE [dbo].[MainUsers] DROP CONSTRAINT [DF__MainUsers__Passw__3C34F16F];


GO
PRINT N'Dropping Table [dbo].[CommiteesDataEntry]...';


GO
DROP TABLE [dbo].[CommiteesDataEntry];


GO
PRINT N'Dropping Table [dbo].[UsersDataEntry]...';


GO
DROP TABLE [dbo].[UsersDataEntry];


GO
PRINT N'Altering Table [dbo].[Attachment]...';


GO
ALTER TABLE [dbo].[Attachment]
    ADD [CreatedBy] INT           NULL,
        [CreatedOn] DATETIME2 (7) NULL,
        [DeletedBy] INT           NULL,
        [DeletedOn] DATETIME2 (7) NULL,
        [UpdatedBy] INT           NULL,
        [UpdatedOn] DATETIME2 (7) NULL;


GO
PRINT N'Altering Table [dbo].[BranchNegoiationUsers]...';


GO
ALTER TABLE [dbo].[BranchNegoiationUsers]
    ADD [CreatedBy] INT           NULL,
        [CreatedOn] DATETIME2 (7) NULL,
        [DeletedBy] INT           NULL,
        [DeletedOn] DATETIME2 (7) NULL,
        [UpdatedBy] INT           NULL,
        [UpdatedOn] DATETIME2 (7) NULL;


GO
PRINT N'Altering Table [dbo].[Committees]...';


GO
ALTER TABLE [dbo].[Committees]
    ADD [CreatedBy] INT           NULL,
        [CreatedOn] DATETIME2 (7) NULL,
        [DeletedBy] INT           NULL,
        [DeletedOn] DATETIME2 (7) NULL,
        [UpdatedBy] INT           NULL,
        [UpdatedOn] DATETIME2 (7) NULL;


GO
PRINT N'Altering Table [dbo].[ContactData]...';


GO
ALTER TABLE [dbo].[ContactData]
    ADD [CreatedBy] INT           NULL,
        [CreatedOn] DATETIME2 (7) NULL,
        [DeletedBy] INT           NULL,
        [DeletedOn] DATETIME2 (7) NULL,
        [UpdatedBy] INT           NULL,
        [UpdatedOn] DATETIME2 (7) NULL;


GO
PRINT N'Altering Table [dbo].[Countries]...';


GO
ALTER TABLE [dbo].[Countries]
    ADD [CreatedBy] INT           NULL,
        [CreatedOn] DATETIME2 (7) NULL,
        [DeletedBy] INT           NULL,
        [DeletedOn] DATETIME2 (7) NULL,
        [UpdatedBy] INT           NULL,
        [UpdatedOn] DATETIME2 (7) NULL;


GO
PRINT N'Altering Table [dbo].[CurrentResidence]...';


GO
ALTER TABLE [dbo].[CurrentResidence]
    ADD [CreatedBy] INT           NULL,
        [CreatedOn] DATETIME2 (7) NULL,
        [DeletedBy] INT           NULL,
        [DeletedOn] DATETIME2 (7) NULL,
        [UpdatedBy] INT           NULL,
        [UpdatedOn] DATETIME2 (7) NULL;


GO
PRINT N'Altering Table [dbo].[Department]...';


GO
ALTER TABLE [dbo].[Department]
    ADD [CreatedBy] INT           NULL,
        [CreatedOn] DATETIME2 (7) NULL,
        [DeletedBy] INT           NULL,
        [DeletedOn] DATETIME2 (7) NULL,
        [UpdatedBy] INT           NULL,
        [UpdatedOn] DATETIME2 (7) NULL;


GO
PRINT N'Altering Table [dbo].[DepartmentNegoiationUsers]...';


GO
ALTER TABLE [dbo].[DepartmentNegoiationUsers]
    ADD [CreatedBy] INT           NULL,
        [CreatedOn] DATETIME2 (7) NULL,
        [DeletedBy] INT           NULL,
        [DeletedOn] DATETIME2 (7) NULL,
        [UpdatedBy] INT           NULL,
        [UpdatedOn] DATETIME2 (7) NULL;


GO
PRINT N'Altering Table [dbo].[EducationalLevels]...';


GO
ALTER TABLE [dbo].[EducationalLevels]
    ADD [CreatedBy] INT           NULL,
        [CreatedOn] DATETIME2 (7) NULL,
        [DeletedBy] INT           NULL,
        [DeletedOn] DATETIME2 (7) NULL,
        [UpdatedBy] INT           NULL,
        [UpdatedOn] DATETIME2 (7) NULL;


GO
PRINT N'Altering Table [dbo].[FamilyInformation]...';


GO
ALTER TABLE [dbo].[FamilyInformation]
    ADD [CreatedBy] INT           NULL,
        [CreatedOn] DATETIME2 (7) NULL,
        [DeletedBy] INT           NULL,
        [DeletedOn] DATETIME2 (7) NULL,
        [UpdatedBy] INT           NULL,
        [UpdatedOn] DATETIME2 (7) NULL;


GO
PRINT N'Altering Table [dbo].[IsslamRecognition]...';


GO
ALTER TABLE [dbo].[IsslamRecognition]
    ADD [CreatedBy] INT           NULL,
        [CreatedOn] DATETIME2 (7) NULL,
        [DeletedBy] INT           NULL,
        [DeletedOn] DATETIME2 (7) NULL,
        [UpdatedBy] INT           NULL,
        [UpdatedOn] DATETIME2 (7) NULL;


GO
PRINT N'Altering Table [dbo].[Localizations]...';


GO
ALTER TABLE [dbo].[Localizations]
    ADD [CreatedBy] INT           NULL,
        [CreatedOn] DATETIME2 (7) NULL,
        [DeletedBy] INT           NULL,
        [DeletedOn] DATETIME2 (7) NULL,
        [UpdatedBy] INT           NULL,
        [UpdatedOn] DATETIME2 (7) NULL;


GO
PRINT N'Altering Table [dbo].[MainRoles]...';


GO
ALTER TABLE [dbo].[MainRoles]
    ADD [CreatedBy] INT           NULL,
        [CreatedOn] DATETIME2 (7) NULL,
        [DeletedBy] INT           NULL,
        [DeletedOn] DATETIME2 (7) NULL,
        [UpdatedBy] INT           NULL,
        [UpdatedOn] DATETIME2 (7) NULL;


GO
PRINT N'Altering Table [dbo].[MainUserRole]...';


GO
ALTER TABLE [dbo].[MainUserRole]
    ADD [CreatedBy] INT           NULL,
        [CreatedOn] DATETIME2 (7) NULL,
        [DeletedBy] INT           NULL,
        [DeletedOn] DATETIME2 (7) NULL,
        [UpdatedBy] INT           NULL,
        [UpdatedOn] DATETIME2 (7) NULL;


GO
PRINT N'Altering Table [dbo].[MainUsers]...';


GO
ALTER TABLE [dbo].[MainUsers]
    ADD [CreatedBy] INT           NULL,
        [CreatedOn] DATETIME2 (7) NULL,
        [DeletedBy] INT           NULL,
        [DeletedOn] DATETIME2 (7) NULL,
        [UpdatedBy] INT           NULL,
        [UpdatedOn] DATETIME2 (7) NULL;


GO
PRINT N'Altering Table [dbo].[MinistryBranshs]...';


GO
ALTER TABLE [dbo].[MinistryBranshs]
    ADD [CreatedBy] INT           NULL,
        [CreatedOn] DATETIME2 (7) NULL,
        [DeletedBy] INT           NULL,
        [DeletedOn] DATETIME2 (7) NULL,
        [UpdatedBy] INT           NULL,
        [UpdatedOn] DATETIME2 (7) NULL;


GO
PRINT N'Altering Table [dbo].[Muslimes]...';


GO
ALTER TABLE [dbo].[Muslimes]
    ADD [CreatedBy] INT           NULL,
        [CreatedOn] DATETIME2 (7) NULL,
        [DeletedBy] INT           NULL,
        [DeletedOn] DATETIME2 (7) NULL,
        [UpdatedBy] INT           NULL,
        [UpdatedOn] DATETIME2 (7) NULL;


GO
PRINT N'Altering Table [dbo].[OrderHistory]...';


GO
ALTER TABLE [dbo].[OrderHistory]
    ADD [CreatedBy] INT           NULL,
        [CreatedOn] DATETIME2 (7) NULL,
        [DeletedBy] INT           NULL,
        [DeletedOn] DATETIME2 (7) NULL,
        [UpdatedBy] INT           NULL,
        [UpdatedOn] DATETIME2 (7) NULL;


GO
PRINT N'Altering Table [dbo].[Orders]...';


GO
ALTER TABLE [dbo].[Orders]
    ADD [CreatedBy] INT           NULL,
        [CreatedOn] DATETIME2 (7) NULL,
        [DeletedBy] INT           NULL,
        [DeletedOn] DATETIME2 (7) NULL,
        [UpdatedBy] INT           NULL,
        [UpdatedOn] DATETIME2 (7) NULL;


GO
PRINT N'Altering Table [dbo].[OriginalCountry]...';


GO
ALTER TABLE [dbo].[OriginalCountry]
    ADD [CreatedBy] INT           NULL,
        [CreatedOn] DATETIME2 (7) NULL,
        [DeletedBy] INT           NULL,
        [DeletedOn] DATETIME2 (7) NULL,
        [UpdatedBy] INT           NULL,
        [UpdatedOn] DATETIME2 (7) NULL;


GO
PRINT N'Altering Table [dbo].[PersonalData]...';


GO
ALTER TABLE [dbo].[PersonalData]
    ADD [CreatedBy] INT           NULL,
        [CreatedOn] DATETIME2 (7) NULL,
        [DeletedBy] INT           NULL,
        [DeletedOn] DATETIME2 (7) NULL,
        [UpdatedBy] INT           NULL,
        [UpdatedOn] DATETIME2 (7) NULL;


GO
PRINT N'Altering Table [dbo].[PersonalInformation]...';


GO
ALTER TABLE [dbo].[PersonalInformation]
    ADD [CreatedBy] INT           NULL,
        [CreatedOn] DATETIME2 (7) NULL,
        [DeletedBy] INT           NULL,
        [DeletedOn] DATETIME2 (7) NULL,
        [UpdatedBy] INT           NULL,
        [UpdatedOn] DATETIME2 (7) NULL;


GO
PRINT N'Altering Table [dbo].[Preachers]...';


GO
ALTER TABLE [dbo].[Preachers]
    ADD [CreatedBy] INT           NULL,
        [CreatedOn] DATETIME2 (7) NULL,
        [DeletedBy] INT           NULL,
        [DeletedOn] DATETIME2 (7) NULL,
        [UpdatedBy] INT           NULL,
        [UpdatedOn] DATETIME2 (7) NULL;


GO
PRINT N'Altering Table [dbo].[Religions]...';


GO
ALTER TABLE [dbo].[Religions]
    ADD [CreatedBy] INT           NULL,
        [CreatedOn] DATETIME2 (7) NULL,
        [DeletedBy] INT           NULL,
        [DeletedOn] DATETIME2 (7) NULL,
        [UpdatedBy] INT           NULL,
        [UpdatedOn] DATETIME2 (7) NULL;


GO
PRINT N'Altering Table [dbo].[ResidenceIssuePlace]...';


GO
ALTER TABLE [dbo].[ResidenceIssuePlace]
    ADD [CreatedBy] INT           NULL,
        [CreatedOn] DATETIME2 (7) NULL,
        [DeletedBy] INT           NULL,
        [DeletedOn] DATETIME2 (7) NULL,
        [UpdatedBy] INT           NULL,
        [UpdatedOn] DATETIME2 (7) NULL;


GO
PRINT N'Altering Table [dbo].[Settings]...';


GO
ALTER TABLE [dbo].[Settings]
    ADD [CreatedBy] INT           NULL,
        [CreatedOn] DATETIME2 (7) NULL,
        [DeletedBy] INT           NULL,
        [DeletedOn] DATETIME2 (7) NULL,
        [UpdatedBy] INT           NULL,
        [UpdatedOn] DATETIME2 (7) NULL;


GO
PRINT N'Altering Table [dbo].[UserToken]...';


GO
ALTER TABLE [dbo].[UserToken]
    ADD [CreatedBy] INT           NULL,
        [CreatedOn] DATETIME2 (7) NULL,
        [DeletedBy] INT           NULL,
        [DeletedOn] DATETIME2 (7) NULL,
        [UpdatedBy] INT           NULL,
        [UpdatedOn] DATETIME2 (7) NULL;


GO
PRINT N'Altering Table [dbo].[Witness]...';


GO
ALTER TABLE [dbo].[Witness]
    ADD [CreatedBy] INT           NULL,
        [CreatedOn] DATETIME2 (7) NULL,
        [DeletedBy] INT           NULL,
        [DeletedOn] DATETIME2 (7) NULL,
        [UpdatedBy] INT           NULL,
        [UpdatedOn] DATETIME2 (7) NULL;


GO
PRINT N'Altering Table [dbo].[Work]...';


GO
ALTER TABLE [dbo].[Work]
    ADD [CreatedBy] INT           NULL,
        [CreatedOn] DATETIME2 (7) NULL,
        [DeletedBy] INT           NULL,
        [DeletedOn] DATETIME2 (7) NULL,
        [UpdatedBy] INT           NULL,
        [UpdatedOn] DATETIME2 (7) NULL;


GO
PRINT N'Creating Default Constraint unnamed constraint on [dbo].[MainUsers]...';


GO
ALTER TABLE [dbo].[MainUsers]
    ADD DEFAULT (CONVERT([bit],(0))) FOR [PasswordChanged];


GO
PRINT N'Update complete.';


GO
