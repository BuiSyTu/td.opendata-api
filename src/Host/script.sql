BEGIN TRANSACTION;
GO

ALTER TABLE [Tags] ADD [View] int NOT NULL DEFAULT 0;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220407072707_Update_14', N'6.0.1');
GO

COMMIT;
GO

