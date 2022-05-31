BEGIN TRANSACTION;
GO

CREATE TABLE [DataSources] (
    [Id] uniqueidentifier NOT NULL,
    [Name] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [Tenant] nvarchar(max) NULL,
    [CreatedBy] nvarchar(max) NULL,
    [CreatedOn] datetime2 NULL,
    [LastModifiedBy] nvarchar(max) NULL,
    [LastModifiedOn] datetime2 NULL,
    [DeletedOn] datetime2 NULL,
    [DeletedBy] nvarchar(max) NULL,
    CONSTRAINT [PK_DataSources] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [DocumentTypes] (
    [Id] uniqueidentifier NOT NULL,
    [Name] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [Tenant] nvarchar(max) NULL,
    [CreatedBy] nvarchar(max) NULL,
    [CreatedOn] datetime2 NULL,
    [LastModifiedBy] nvarchar(max) NULL,
    [LastModifiedOn] datetime2 NULL,
    [DeletedOn] datetime2 NULL,
    [DeletedBy] nvarchar(max) NULL,
    CONSTRAINT [PK_DocumentTypes] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [MiningSources] (
    [Id] uniqueidentifier NOT NULL,
    [Name] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [Tenant] nvarchar(max) NULL,
    [CreatedBy] nvarchar(max) NULL,
    [CreatedOn] datetime2 NULL,
    [LastModifiedBy] nvarchar(max) NULL,
    [LastModifiedOn] datetime2 NULL,
    [DeletedOn] datetime2 NULL,
    [DeletedBy] nvarchar(max) NULL,
    CONSTRAINT [PK_MiningSources] PRIMARY KEY ([Id])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220420084347_Update_15', N'6.0.1');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Datasets] ADD [OfficeCode] nvarchar(max) NULL;
GO

ALTER TABLE [Datasets] ADD [OfficeName] nvarchar(max) NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220427080929_Update_16', N'6.0.1');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Datasets] ADD [View] int NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220519030219_Update_17', N'6.0.1');
GO

COMMIT;
GO

