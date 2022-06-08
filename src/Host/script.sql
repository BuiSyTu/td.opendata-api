BEGIN TRANSACTION;
GO

CREATE TABLE [SyncHistories] (
    [Id] uniqueidentifier NOT NULL,
    [SyncTime] datetime2 NULL,
    [DatasetId] uniqueidentifier NOT NULL,
    [CreatedBy] nvarchar(max) NULL,
    [CreatedOn] datetime2 NULL,
    [LastModifiedBy] nvarchar(max) NULL,
    [LastModifiedOn] datetime2 NULL,
    [DeletedOn] datetime2 NULL,
    [DeletedBy] nvarchar(max) NULL,
    CONSTRAINT [PK_SyncHistories] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_SyncHistories_Datasets_DatasetId] FOREIGN KEY ([DatasetId]) REFERENCES [Datasets] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_SyncHistories_DatasetId] ON [SyncHistories] ([DatasetId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220607014030_Update_18', N'6.0.1');
GO

COMMIT;
GO

