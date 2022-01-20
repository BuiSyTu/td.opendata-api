IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF SCHEMA_ID(N'IDENTITY') IS NULL EXEC(N'CREATE SCHEMA [IDENTITY];');
GO

CREATE TABLE [Attachments] (
    [Id] uniqueidentifier NOT NULL,
    [Name] nvarchar(max) NOT NULL,
    [Type] nvarchar(max) NOT NULL,
    [Url] nvarchar(max) NOT NULL,
    [Tenant] nvarchar(max) NULL,
    [CreatedBy] nvarchar(max) NULL,
    [CreatedOn] datetime2 NULL,
    [LastModifiedBy] nvarchar(max) NULL,
    [LastModifiedOn] datetime2 NULL,
    [DeletedOn] datetime2 NULL,
    [DeletedBy] nvarchar(max) NULL,
    CONSTRAINT [PK_Attachments] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [AuditTrails] (
    [Id] uniqueidentifier NOT NULL,
    [UserName] nvarchar(max) NULL,
    [Type] nvarchar(max) NULL,
    [TableName] nvarchar(max) NULL,
    [DateTime] datetime2 NOT NULL,
    [OldValues] nvarchar(max) NULL,
    [NewValues] nvarchar(max) NULL,
    [AffectedColumns] nvarchar(max) NULL,
    [PrimaryKey] nvarchar(max) NULL,
    CONSTRAINT [PK_AuditTrails] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Brands] (
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
    CONSTRAINT [PK_Brands] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Categories] (
    [Id] uniqueidentifier NOT NULL,
    [Name] nvarchar(250) NULL,
    [Description] nvarchar(max) NULL,
    [Code] nvarchar(250) NULL,
    [IsActive] bit NULL,
    [ImageUrl] nvarchar(max) NULL,
    [Icon] nvarchar(max) NULL,
    [Order] int NULL,
    [ParentId] uniqueidentifier NULL,
    [Tenant] nvarchar(max) NULL,
    [CreatedBy] nvarchar(max) NULL,
    [CreatedOn] datetime2 NULL,
    [LastModifiedBy] nvarchar(max) NULL,
    [LastModifiedOn] datetime2 NULL,
    [DeletedOn] datetime2 NULL,
    [DeletedBy] nvarchar(max) NULL,
    CONSTRAINT [PK_Categories] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Categories_Categories_ParentId] FOREIGN KEY ([ParentId]) REFERENCES [Categories] ([Id])
);
GO

CREATE TABLE [DataTypes] (
    [Id] uniqueidentifier NOT NULL,
    [Name] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [Code] nvarchar(max) NULL,
    [IsActive] bit NULL,
    [Tenant] nvarchar(max) NULL,
    [CreatedBy] nvarchar(max) NULL,
    [CreatedOn] datetime2 NULL,
    [LastModifiedBy] nvarchar(max) NULL,
    [LastModifiedOn] datetime2 NULL,
    [DeletedOn] datetime2 NULL,
    [DeletedBy] nvarchar(max) NULL,
    CONSTRAINT [PK_DataTypes] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Licenses] (
    [Id] uniqueidentifier NOT NULL,
    [Name] nvarchar(250) NULL,
    [Description] nvarchar(max) NULL,
    [Code] nvarchar(250) NULL,
    [IsActive] bit NULL,
    [Tenant] nvarchar(max) NULL,
    [CreatedBy] nvarchar(max) NULL,
    [CreatedOn] datetime2 NULL,
    [LastModifiedBy] nvarchar(max) NULL,
    [LastModifiedOn] datetime2 NULL,
    [DeletedOn] datetime2 NULL,
    [DeletedBy] nvarchar(max) NULL,
    CONSTRAINT [PK_Licenses] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Organizations] (
    [Id] uniqueidentifier NOT NULL,
    [Name] nvarchar(250) NULL,
    [Description] nvarchar(max) NULL,
    [Code] nvarchar(250) NULL,
    [IsActive] bit NULL,
    [Tenant] nvarchar(max) NULL,
    [CreatedBy] nvarchar(max) NULL,
    [CreatedOn] datetime2 NULL,
    [LastModifiedBy] nvarchar(max) NULL,
    [LastModifiedOn] datetime2 NULL,
    [DeletedOn] datetime2 NULL,
    [DeletedBy] nvarchar(max) NULL,
    CONSTRAINT [PK_Organizations] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [ProviderTypes] (
    [Id] uniqueidentifier NOT NULL,
    [Name] nvarchar(250) NULL,
    [Description] nvarchar(max) NULL,
    [Code] nvarchar(250) NULL,
    [IsActive] bit NULL,
    [Tenant] nvarchar(max) NULL,
    [CreatedBy] nvarchar(max) NULL,
    [CreatedOn] datetime2 NULL,
    [LastModifiedBy] nvarchar(max) NULL,
    [LastModifiedOn] datetime2 NULL,
    [DeletedOn] datetime2 NULL,
    [DeletedBy] nvarchar(max) NULL,
    CONSTRAINT [PK_ProviderTypes] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [IDENTITY].[Roles] (
    [Id] nvarchar(450) NOT NULL,
    [Description] nvarchar(max) NULL,
    [Tenant] nvarchar(450) NULL,
    [Name] nvarchar(256) NULL,
    [NormalizedName] nvarchar(256) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    CONSTRAINT [PK_Roles] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Tags] (
    [Id] uniqueidentifier NOT NULL,
    [Name] nvarchar(250) NULL,
    [Tenant] nvarchar(max) NULL,
    [CreatedBy] nvarchar(max) NULL,
    [CreatedOn] datetime2 NULL,
    [LastModifiedBy] nvarchar(max) NULL,
    [LastModifiedOn] datetime2 NULL,
    [DeletedOn] datetime2 NULL,
    [DeletedBy] nvarchar(max) NULL,
    CONSTRAINT [PK_Tags] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [IDENTITY].[Users] (
    [Id] nvarchar(450) NOT NULL,
    [FirstName] nvarchar(max) NULL,
    [LastName] nvarchar(max) NULL,
    [ImageUrl] nvarchar(max) NULL,
    [IsActive] bit NOT NULL,
    [RefreshToken] nvarchar(max) NULL,
    [RefreshTokenExpiryTime] datetime2 NOT NULL,
    [Tenant] nvarchar(max) NULL,
    [ObjectId] nvarchar(256) NULL,
    [UserName] nvarchar(256) NULL,
    [NormalizedUserName] nvarchar(256) NULL,
    [Email] nvarchar(256) NULL,
    [NormalizedEmail] nvarchar(256) NULL,
    [EmailConfirmed] bit NOT NULL,
    [PasswordHash] nvarchar(max) NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [LockoutEnabled] bit NOT NULL,
    [AccessFailedCount] int NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Products] (
    [Id] uniqueidentifier NOT NULL,
    [Name] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [Rate] decimal(18,2) NOT NULL,
    [Tenant] nvarchar(max) NULL,
    [ImagePath] nvarchar(max) NULL,
    [BrandId] uniqueidentifier NOT NULL,
    [CreatedBy] nvarchar(max) NULL,
    [CreatedOn] datetime2 NULL,
    [LastModifiedBy] nvarchar(max) NULL,
    [LastModifiedOn] datetime2 NULL,
    [DeletedOn] datetime2 NULL,
    [DeletedBy] nvarchar(max) NULL,
    CONSTRAINT [PK_Products] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Products_Brands_BrandId] FOREIGN KEY ([BrandId]) REFERENCES [Brands] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [Datasets] (
    [Id] uniqueidentifier NOT NULL,
    [Name] nvarchar(250) NULL,
    [Title] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [Code] nvarchar(250) NULL,
    [Tags] nvarchar(max) NULL,
    [State] int NULL,
    [Visibility] bit NULL,
    [LicenseId] uniqueidentifier NOT NULL,
    [Author] nvarchar(max) NULL,
    [AuthorEmail] nvarchar(max) NULL,
    [Maintainer] nvarchar(max) NULL,
    [MaintainerEmail] nvarchar(max) NULL,
    [OrganizationId] uniqueidentifier NOT NULL,
    [Resource] nvarchar(max) NULL,
    [DataTypeId] uniqueidentifier NOT NULL,
    [CategoryId] uniqueidentifier NOT NULL,
    [ProviderTypeId] uniqueidentifier NOT NULL,
    [Tenant] nvarchar(max) NULL,
    [CreatedBy] nvarchar(max) NULL,
    [CreatedOn] datetime2 NULL,
    [LastModifiedBy] nvarchar(max) NULL,
    [LastModifiedOn] datetime2 NULL,
    [DeletedOn] datetime2 NULL,
    [DeletedBy] nvarchar(max) NULL,
    CONSTRAINT [PK_Datasets] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Datasets_Categories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [Categories] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Datasets_DataTypes_DataTypeId] FOREIGN KEY ([DataTypeId]) REFERENCES [DataTypes] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Datasets_Licenses_LicenseId] FOREIGN KEY ([LicenseId]) REFERENCES [Licenses] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Datasets_Organizations_OrganizationId] FOREIGN KEY ([OrganizationId]) REFERENCES [Organizations] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Datasets_ProviderTypes_ProviderTypeId] FOREIGN KEY ([ProviderTypeId]) REFERENCES [ProviderTypes] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [IDENTITY].[RoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [Description] nvarchar(max) NULL,
    [Tenant] nvarchar(max) NULL,
    [Group] nvarchar(max) NULL,
    [CreatedBy] nvarchar(max) NULL,
    [CreatedOn] datetime2 NOT NULL,
    [LastModifiedBy] nvarchar(max) NULL,
    [LastModifiedOn] datetime2 NULL,
    [RoleId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_RoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_RoleClaims_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [IDENTITY].[Roles] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [IDENTITY].[UserClaims] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_UserClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_UserClaims_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [IDENTITY].[Users] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [IDENTITY].[UserLogins] (
    [LoginProvider] nvarchar(450) NOT NULL,
    [ProviderKey] nvarchar(450) NOT NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_UserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_UserLogins_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [IDENTITY].[Users] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [IDENTITY].[UserRoles] (
    [UserId] nvarchar(450) NOT NULL,
    [RoleId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_UserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_UserRoles_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [IDENTITY].[Roles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_UserRoles_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [IDENTITY].[Users] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [IDENTITY].[UserTokens] (
    [UserId] nvarchar(450) NOT NULL,
    [LoginProvider] nvarchar(450) NOT NULL,
    [Name] nvarchar(450) NOT NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_UserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
    CONSTRAINT [FK_UserTokens_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [IDENTITY].[Users] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [CustomFields] (
    [Id] uniqueidentifier NOT NULL,
    [DatasetId] uniqueidentifier NOT NULL,
    [Key] nvarchar(max) NULL,
    [Value] nvarchar(max) NULL,
    [Tenant] nvarchar(max) NULL,
    [CreatedBy] nvarchar(max) NULL,
    [CreatedOn] datetime2 NULL,
    [LastModifiedBy] nvarchar(max) NULL,
    [LastModifiedOn] datetime2 NULL,
    [DeletedOn] datetime2 NULL,
    [DeletedBy] nvarchar(max) NULL,
    CONSTRAINT [PK_CustomFields] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_CustomFields_Datasets_DatasetId] FOREIGN KEY ([DatasetId]) REFERENCES [Datasets] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [DatasetAPIConfigs] (
    [Id] uniqueidentifier NOT NULL,
    [Method] nvarchar(max) NULL,
    [Url] nvarchar(max) NULL,
    [Headers] nvarchar(max) NULL,
    [Data] nvarchar(max) NULL,
    [DatasetId] uniqueidentifier NOT NULL,
    [Tenant] nvarchar(max) NULL,
    [CreatedBy] nvarchar(max) NULL,
    [CreatedOn] datetime2 NULL,
    [LastModifiedBy] nvarchar(max) NULL,
    [LastModifiedOn] datetime2 NULL,
    [DeletedOn] datetime2 NULL,
    [DeletedBy] nvarchar(max) NULL,
    CONSTRAINT [PK_DatasetAPIConfigs] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_DatasetAPIConfigs_Datasets_DatasetId] FOREIGN KEY ([DatasetId]) REFERENCES [Datasets] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [DatasetDBConfigs] (
    [Id] uniqueidentifier NOT NULL,
    [DBProvider] nvarchar(max) NULL,
    [ConnectionString] nvarchar(max) NULL,
    [DatabaseName] nvarchar(max) NULL,
    [DataTable] nvarchar(max) NULL,
    [DatasetId] uniqueidentifier NOT NULL,
    [Tenant] nvarchar(max) NULL,
    [CreatedBy] nvarchar(max) NULL,
    [CreatedOn] datetime2 NULL,
    [LastModifiedBy] nvarchar(max) NULL,
    [LastModifiedOn] datetime2 NULL,
    [DeletedOn] datetime2 NULL,
    [DeletedBy] nvarchar(max) NULL,
    CONSTRAINT [PK_DatasetDBConfigs] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_DatasetDBConfigs_Datasets_DatasetId] FOREIGN KEY ([DatasetId]) REFERENCES [Datasets] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [DatasetFileConfigs] (
    [Id] uniqueidentifier NOT NULL,
    [FileType] nvarchar(max) NULL,
    [FileName] nvarchar(max) NULL,
    [FileData] nvarchar(max) NULL,
    [Tenant] nvarchar(max) NULL,
    [DatasetId] uniqueidentifier NOT NULL,
    [CreatedBy] nvarchar(max) NULL,
    [CreatedOn] datetime2 NULL,
    [LastModifiedBy] nvarchar(max) NULL,
    [LastModifiedOn] datetime2 NULL,
    [DeletedOn] datetime2 NULL,
    [DeletedBy] nvarchar(max) NULL,
    CONSTRAINT [PK_DatasetFileConfigs] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_DatasetFileConfigs_Datasets_DatasetId] FOREIGN KEY ([DatasetId]) REFERENCES [Datasets] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [DatasetOffices] (
    [Id] uniqueidentifier NOT NULL,
    [DatasetId] uniqueidentifier NOT NULL,
    [OfficeCode] nvarchar(max) NULL,
    [Tenant] nvarchar(max) NULL,
    [CreatedBy] nvarchar(max) NULL,
    [CreatedOn] datetime2 NULL,
    [LastModifiedBy] nvarchar(max) NULL,
    [LastModifiedOn] datetime2 NULL,
    [DeletedOn] datetime2 NULL,
    [DeletedBy] nvarchar(max) NULL,
    CONSTRAINT [PK_DatasetOffices] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_DatasetOffices_Datasets_DatasetId] FOREIGN KEY ([DatasetId]) REFERENCES [Datasets] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [Metadatas] (
    [Id] uniqueidentifier NOT NULL,
    [DataType] nvarchar(max) NULL,
    [IsDisplay] bit NULL,
    [DataNameOld] nvarchar(max) NULL,
    [DataNameNew] nvarchar(max) NULL,
    [Title] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [DatasetId] uniqueidentifier NOT NULL,
    [Tenant] nvarchar(max) NULL,
    [CreatedBy] nvarchar(max) NULL,
    [CreatedOn] datetime2 NULL,
    [LastModifiedBy] nvarchar(max) NULL,
    [LastModifiedOn] datetime2 NULL,
    [DeletedOn] datetime2 NULL,
    [DeletedBy] nvarchar(max) NULL,
    CONSTRAINT [PK_Metadatas] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Metadatas_Datasets_DatasetId] FOREIGN KEY ([DatasetId]) REFERENCES [Datasets] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_Categories_ParentId] ON [Categories] ([ParentId]);
GO

CREATE INDEX [IX_CustomFields_DatasetId] ON [CustomFields] ([DatasetId]);
GO

CREATE UNIQUE INDEX [IX_DatasetAPIConfigs_DatasetId] ON [DatasetAPIConfigs] ([DatasetId]);
GO

CREATE UNIQUE INDEX [IX_DatasetDBConfigs_DatasetId] ON [DatasetDBConfigs] ([DatasetId]);
GO

CREATE UNIQUE INDEX [IX_DatasetFileConfigs_DatasetId] ON [DatasetFileConfigs] ([DatasetId]);
GO

CREATE INDEX [IX_DatasetOffices_DatasetId] ON [DatasetOffices] ([DatasetId]);
GO

CREATE INDEX [IX_Datasets_CategoryId] ON [Datasets] ([CategoryId]);
GO

CREATE INDEX [IX_Datasets_DataTypeId] ON [Datasets] ([DataTypeId]);
GO

CREATE INDEX [IX_Datasets_LicenseId] ON [Datasets] ([LicenseId]);
GO

CREATE INDEX [IX_Datasets_OrganizationId] ON [Datasets] ([OrganizationId]);
GO

CREATE INDEX [IX_Datasets_ProviderTypeId] ON [Datasets] ([ProviderTypeId]);
GO

CREATE INDEX [IX_Metadatas_DatasetId] ON [Metadatas] ([DatasetId]);
GO

CREATE INDEX [IX_Products_BrandId] ON [Products] ([BrandId]);
GO

CREATE INDEX [IX_RoleClaims_RoleId] ON [IDENTITY].[RoleClaims] ([RoleId]);
GO

CREATE UNIQUE INDEX [RoleNameIndex] ON [IDENTITY].[Roles] ([NormalizedName], [Tenant]) WHERE [NormalizedName] IS NOT NULL AND [Tenant] IS NOT NULL;
GO

CREATE INDEX [IX_UserClaims_UserId] ON [IDENTITY].[UserClaims] ([UserId]);
GO

CREATE INDEX [IX_UserLogins_UserId] ON [IDENTITY].[UserLogins] ([UserId]);
GO

CREATE INDEX [IX_UserRoles_RoleId] ON [IDENTITY].[UserRoles] ([RoleId]);
GO

CREATE INDEX [EmailIndex] ON [IDENTITY].[Users] ([NormalizedEmail]);
GO

CREATE UNIQUE INDEX [UserNameIndex] ON [IDENTITY].[Users] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220106032245_Init', N'6.0.1');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Metadatas]') AND [c].[name] = N'DataNameNew');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Metadatas] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Metadatas] DROP COLUMN [DataNameNew];
GO

EXEC sp_rename N'[Metadatas].[DataNameOld]', N'Data', N'COLUMN';
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220107023551_Update_1', N'6.0.1');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [DatasetOffices] ADD [OfficeName] nvarchar(max) NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220107024215_Update_2', N'6.0.1');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DROP TABLE [Metadatas];
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Datasets]') AND [c].[name] = N'Description');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Datasets] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [Datasets] ALTER COLUMN [Description] text NULL;
GO

ALTER TABLE [Datasets] ADD [Metadata] nvarchar(max) NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220107093708_Update_3', N'6.0.1');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220107094359_Update_4', N'6.0.1');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Datasets] DROP CONSTRAINT [FK_Datasets_Categories_CategoryId];
GO

ALTER TABLE [Datasets] DROP CONSTRAINT [FK_Datasets_DataTypes_DataTypeId];
GO

ALTER TABLE [Datasets] DROP CONSTRAINT [FK_Datasets_Licenses_LicenseId];
GO

ALTER TABLE [Datasets] DROP CONSTRAINT [FK_Datasets_Organizations_OrganizationId];
GO

ALTER TABLE [Datasets] DROP CONSTRAINT [FK_Datasets_ProviderTypes_ProviderTypeId];
GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Datasets]') AND [c].[name] = N'ProviderTypeId');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Datasets] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [Datasets] ALTER COLUMN [ProviderTypeId] uniqueidentifier NULL;
GO

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Datasets]') AND [c].[name] = N'OrganizationId');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [Datasets] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [Datasets] ALTER COLUMN [OrganizationId] uniqueidentifier NULL;
GO

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Datasets]') AND [c].[name] = N'LicenseId');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [Datasets] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [Datasets] ALTER COLUMN [LicenseId] uniqueidentifier NULL;
GO

DECLARE @var5 sysname;
SELECT @var5 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Datasets]') AND [c].[name] = N'DataTypeId');
IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [Datasets] DROP CONSTRAINT [' + @var5 + '];');
ALTER TABLE [Datasets] ALTER COLUMN [DataTypeId] uniqueidentifier NULL;
GO

DECLARE @var6 sysname;
SELECT @var6 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Datasets]') AND [c].[name] = N'CategoryId');
IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [Datasets] DROP CONSTRAINT [' + @var6 + '];');
ALTER TABLE [Datasets] ALTER COLUMN [CategoryId] uniqueidentifier NULL;
GO

ALTER TABLE [Datasets] ADD CONSTRAINT [FK_Datasets_Categories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [Categories] ([Id]);
GO

ALTER TABLE [Datasets] ADD CONSTRAINT [FK_Datasets_DataTypes_DataTypeId] FOREIGN KEY ([DataTypeId]) REFERENCES [DataTypes] ([Id]);
GO

ALTER TABLE [Datasets] ADD CONSTRAINT [FK_Datasets_Licenses_LicenseId] FOREIGN KEY ([LicenseId]) REFERENCES [Licenses] ([Id]);
GO

ALTER TABLE [Datasets] ADD CONSTRAINT [FK_Datasets_Organizations_OrganizationId] FOREIGN KEY ([OrganizationId]) REFERENCES [Organizations] ([Id]);
GO

ALTER TABLE [Datasets] ADD CONSTRAINT [FK_Datasets_ProviderTypes_ProviderTypeId] FOREIGN KEY ([ProviderTypeId]) REFERENCES [ProviderTypes] ([Id]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220107095510_Update_5', N'6.0.1');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Datasets] DROP CONSTRAINT [FK_Datasets_Categories_CategoryId];
GO

ALTER TABLE [Datasets] DROP CONSTRAINT [FK_Datasets_DataTypes_DataTypeId];
GO

ALTER TABLE [Datasets] DROP CONSTRAINT [FK_Datasets_Licenses_LicenseId];
GO

ALTER TABLE [Datasets] DROP CONSTRAINT [FK_Datasets_Organizations_OrganizationId];
GO

ALTER TABLE [Datasets] DROP CONSTRAINT [FK_Datasets_ProviderTypes_ProviderTypeId];
GO

ALTER TABLE [Datasets] ADD CONSTRAINT [FK_Datasets_Categories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [Categories] ([Id]) ON DELETE SET NULL;
GO

ALTER TABLE [Datasets] ADD CONSTRAINT [FK_Datasets_DataTypes_DataTypeId] FOREIGN KEY ([DataTypeId]) REFERENCES [DataTypes] ([Id]) ON DELETE SET NULL;
GO

ALTER TABLE [Datasets] ADD CONSTRAINT [FK_Datasets_Licenses_LicenseId] FOREIGN KEY ([LicenseId]) REFERENCES [Licenses] ([Id]) ON DELETE SET NULL;
GO

ALTER TABLE [Datasets] ADD CONSTRAINT [FK_Datasets_Organizations_OrganizationId] FOREIGN KEY ([OrganizationId]) REFERENCES [Organizations] ([Id]) ON DELETE SET NULL;
GO

ALTER TABLE [Datasets] ADD CONSTRAINT [FK_Datasets_ProviderTypes_ProviderTypeId] FOREIGN KEY ([ProviderTypeId]) REFERENCES [ProviderTypes] ([Id]) ON DELETE SET NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220107095819_Update_6', N'6.0.1');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [DatasetFileConfigs] ADD [TableName] nvarchar(max) NULL;
GO

ALTER TABLE [DatasetAPIConfigs] ADD [Key] nvarchar(max) NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220110024354_Update_7', N'6.0.1');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

EXEC sp_rename N'[DatasetAPIConfigs].[Key]', N'TableName', N'COLUMN';
GO

ALTER TABLE [DatasetAPIConfigs] ADD [DataKey] nvarchar(max) NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220110024521_Update_8', N'6.0.1');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [DatasetDBConfigs] ADD [TableName] nvarchar(max) NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220110045559_Update_9', N'6.0.1');
GO

COMMIT;
GO

