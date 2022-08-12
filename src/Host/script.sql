BEGIN TRANSACTION;
GO

CREATE TABLE [FooterConfig] (
    [Id] uniqueidentifier NOT NULL,
    [SoftwareName] nvarchar(max) NULL,
    [CompanyName] nvarchar(max) NULL,
    [Address] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [Fax] nvarchar(max) NULL,
    [HotLine] nvarchar(max) NULL,
    [Email] nvarchar(max) NULL,
    [CreatedBy] nvarchar(max) NULL,
    [CreatedOn] datetime2 NULL,
    [LastModifiedBy] nvarchar(max) NULL,
    [LastModifiedOn] datetime2 NULL,
    [DeletedOn] datetime2 NULL,
    [DeletedBy] nvarchar(max) NULL,
    CONSTRAINT [PK_FooterConfig] PRIMARY KEY ([Id])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220616093239_Update_19', N'6.0.1');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [BannerConfig] (
    [Id] uniqueidentifier NOT NULL,
    [Line1] nvarchar(max) NULL,
    [Line2] nvarchar(max) NULL,
    [CreatedBy] nvarchar(max) NULL,
    [CreatedOn] datetime2 NULL,
    [LastModifiedBy] nvarchar(max) NULL,
    [LastModifiedOn] datetime2 NULL,
    [DeletedOn] datetime2 NULL,
    [DeletedBy] nvarchar(max) NULL,
    CONSTRAINT [PK_BannerConfig] PRIMARY KEY ([Id])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220617040600_Update_20', N'6.0.1');
GO

COMMIT;
GO

