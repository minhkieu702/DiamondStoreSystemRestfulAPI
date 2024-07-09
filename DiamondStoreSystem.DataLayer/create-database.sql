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

CREATE TABLE [Accessories] (
    [AccessoryID] nvarchar(450) NOT NULL,
    [AccessoryName] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [Material] int NOT NULL,
    [Style] int NOT NULL,
    [Brand] nvarchar(max) NOT NULL,
    [Block] bit NOT NULL,
    [Price] float NOT NULL,
    [UnitInStock] int NOT NULL,
    [SKU] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Accessories] PRIMARY KEY ([AccessoryID])
);
GO

CREATE TABLE [Accounts] (
    [AccountID] nvarchar(450) NOT NULL,
    [Email] nvarchar(max) NOT NULL,
    [Password] nvarchar(max) NOT NULL,
    [LastName] nvarchar(max) NOT NULL,
    [FirstName] nvarchar(max) NOT NULL,
    [Phone] decimal(18,2) NOT NULL,
    [Address] nvarchar(max) NOT NULL,
    [Gender] int NOT NULL,
    [DOB] datetime2 NOT NULL,
    [JoinDate] datetime2 NOT NULL,
    [LoyaltyPoint] int NULL,
    [Block] bit NOT NULL,
    [Role] int NOT NULL,
    [WorkingSchedule] int NULL,
    CONSTRAINT [PK_Accounts] PRIMARY KEY ([AccountID])
);
GO

CREATE TABLE [Diamond] (
    [DiamondID] nvarchar(450) NOT NULL,
    [Origin] nvarchar(max) NOT NULL,
    [LabCreated] int NOT NULL,
    [TablePercent] float NOT NULL,
    [DepthPercent] float NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [GIAReportNumber] int NOT NULL,
    [IssueDate] datetime2 NOT NULL,
    [Shape] int NOT NULL,
    [CaratWeight] float NOT NULL,
    [ColorGrade] int NOT NULL,
    [ClarityGrade] int NOT NULL,
    [CutGrade] int NOT NULL,
    [PolishGrade] int NOT NULL,
    [SymmetryGrade] int NOT NULL,
    [FluoresceneGrade] int NOT NULL,
    [Inscription] nvarchar(max) NOT NULL,
    [Height] float NOT NULL,
    [Width] float NOT NULL,
    [Length] float NOT NULL,
    [Price] float NOT NULL,
    [Block] bit NOT NULL,
    [SKU] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Diamond] PRIMARY KEY ([DiamondID])
);
GO

CREATE TABLE [Order] (
    [OrderID] nvarchar(450) NOT NULL,
    [OrderStatus] int NOT NULL,
    [DateOrdered] datetime2 NOT NULL,
    [DateReceived] datetime2 NULL,
    [TotalPrice] float NOT NULL,
    [CustomerID] nvarchar(450) NOT NULL,
    [EmployeeAssignID] nvarchar(450) NOT NULL,
    [PayMethod] int NOT NULL,
    [Block] bit NOT NULL,
    CONSTRAINT [PK_Order] PRIMARY KEY ([OrderID]),
    CONSTRAINT [FK_Order_Accounts_CustomerID] FOREIGN KEY ([CustomerID]) REFERENCES [Accounts] ([AccountID]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Order_Accounts_EmployeeAssignID] FOREIGN KEY ([EmployeeAssignID]) REFERENCES [Accounts] ([AccountID]) ON DELETE NO ACTION
);
GO

CREATE TABLE [Products] (
    [ProductID] nvarchar(450) NOT NULL,
    [Price] float NOT NULL,
    [Block] bit NOT NULL,
    [AccessoryID] nvarchar(450) NULL,
    [OrderID] nvarchar(450) NOT NULL,
    [MainDiamondID] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_Products] PRIMARY KEY ([ProductID]),
    CONSTRAINT [FK_Products_Accessories_AccessoryID] FOREIGN KEY ([AccessoryID]) REFERENCES [Accessories] ([AccessoryID]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Products_Diamond_MainDiamondID] FOREIGN KEY ([MainDiamondID]) REFERENCES [Diamond] ([DiamondID]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Products_Order_OrderID] FOREIGN KEY ([OrderID]) REFERENCES [Order] ([OrderID]) ON DELETE NO ACTION
);
GO

CREATE TABLE [VnPaymentResponses] (
    [VnpOrderId] nvarchar(450) NOT NULL,
    [Success] bit NOT NULL,
    [PaymentMethod] nvarchar(max) NOT NULL,
    [OrderDescription] nvarchar(max) NOT NULL,
    [TransactionId] nvarchar(max) NOT NULL,
    [Token] nvarchar(max) NOT NULL,
    [VnPayResponseCode] nvarchar(max) NOT NULL,
    [OrderId] nvarchar(450) NULL,
    CONSTRAINT [PK_VnPaymentResponses] PRIMARY KEY ([VnpOrderId]),
    CONSTRAINT [FK_VnPaymentResponses_Order_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Order] ([OrderID]) ON DELETE NO ACTION
);
GO

CREATE TABLE [SubDiamonds] (
    [SubDiamondID] nvarchar(450) NOT NULL,
    [ProductID] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_SubDiamonds] PRIMARY KEY ([SubDiamondID]),
    CONSTRAINT [FK_SubDiamonds_Diamond_SubDiamondID] FOREIGN KEY ([SubDiamondID]) REFERENCES [Diamond] ([DiamondID]) ON DELETE NO ACTION,
    CONSTRAINT [FK_SubDiamonds_Products_ProductID] FOREIGN KEY ([ProductID]) REFERENCES [Products] ([ProductID]) ON DELETE NO ACTION
);
GO

CREATE TABLE [Warranties] (
    [WarrantyID] nvarchar(450) NOT NULL,
    [IssueDate] datetime2 NOT NULL,
    [ExpiredDate] datetime2 NOT NULL,
    [Block] bit NOT NULL,
    [ProductID] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_Warranties] PRIMARY KEY ([WarrantyID]),
    CONSTRAINT [FK_Warranties_Products_ProductID] FOREIGN KEY ([ProductID]) REFERENCES [Products] ([ProductID]) ON DELETE NO ACTION
);
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'AccessoryID', N'AccessoryName', N'Block', N'Brand', N'Description', N'Material', N'Price', N'SKU', N'Style', N'UnitInStock') AND [object_id] = OBJECT_ID(N'[Accessories]'))
    SET IDENTITY_INSERT [Accessories] ON;
INSERT INTO [Accessories] ([AccessoryID], [AccessoryName], [Block], [Brand], [Description], [Material], [Price], [SKU], [Style], [UnitInStock])
VALUES (N'A001', N'Gold Chain', CAST(0 AS bit), N'LuxuryBrand', N'18k gold chain', 1, 500.0E0, N'GC001', 1, 10);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'AccessoryID', N'AccessoryName', N'Block', N'Brand', N'Description', N'Material', N'Price', N'SKU', N'Style', N'UnitInStock') AND [object_id] = OBJECT_ID(N'[Accessories]'))
    SET IDENTITY_INSERT [Accessories] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'AccountID', N'Address', N'Block', N'DOB', N'Email', N'FirstName', N'Gender', N'JoinDate', N'LastName', N'LoyaltyPoint', N'Password', N'Phone', N'Role', N'WorkingSchedule') AND [object_id] = OBJECT_ID(N'[Accounts]'))
    SET IDENTITY_INSERT [Accounts] ON;
INSERT INTO [Accounts] ([AccountID], [Address], [Block], [DOB], [Email], [FirstName], [Gender], [JoinDate], [LastName], [LoyaltyPoint], [Password], [Phone], [Role], [WorkingSchedule])
VALUES (N'C001', N'456 Customer Ave.', CAST(0 AS bit), '1990-01-01T00:00:00.0000000', N'customer@example.com', N'Regular', 0, '2024-07-01T15:45:21.7629888+07:00', N'Customer', 100, N'473287f8298dba7163a897908958f7c0eae733e25d2e027992ea2edc9bed2fa8', 9876543210.0, 0, 0),
(N'S001', N'123 Admin St.', CAST(0 AS bit), '1980-01-01T00:00:00.0000000', N'admin@example.com', N'Super', 1, '2024-07-01T15:45:21.7629829+07:00', N'Admin', 0, N'473287f8298dba7163a897908958f7c0eae733e25d2e027992ea2edc9bed2fa8', 1234567890.0, 3, 1),
(N'S002', N'123 Admin St.', CAST(0 AS bit), '1980-01-01T00:00:00.0000000', N'staff1@example.com', N'Super', 1, '2024-07-01T15:45:21.7629869+07:00', N'Admin', 0, N'473287f8298dba7163a897908958f7c0eae733e25d2e027992ea2edc9bed2fa8', 1234567890.0, 1, 1);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'AccountID', N'Address', N'Block', N'DOB', N'Email', N'FirstName', N'Gender', N'JoinDate', N'LastName', N'LoyaltyPoint', N'Password', N'Phone', N'Role', N'WorkingSchedule') AND [object_id] = OBJECT_ID(N'[Accounts]'))
    SET IDENTITY_INSERT [Accounts] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'DiamondID', N'Block', N'CaratWeight', N'ClarityGrade', N'ColorGrade', N'CutGrade', N'DepthPercent', N'Description', N'FluoresceneGrade', N'GIAReportNumber', N'Height', N'Inscription', N'IssueDate', N'LabCreated', N'Length', N'Origin', N'PolishGrade', N'Price', N'SKU', N'Shape', N'SymmetryGrade', N'TablePercent', N'Width') AND [object_id] = OBJECT_ID(N'[Diamond]'))
    SET IDENTITY_INSERT [Diamond] ON;
INSERT INTO [Diamond] ([DiamondID], [Block], [CaratWeight], [ClarityGrade], [ColorGrade], [CutGrade], [DepthPercent], [Description], [FluoresceneGrade], [GIAReportNumber], [Height], [Inscription], [IssueDate], [LabCreated], [Length], [Origin], [PolishGrade], [Price], [SKU], [Shape], [SymmetryGrade], [TablePercent], [Width])
VALUES (N'D001', CAST(0 AS bit), 1.0E0, 1, 1, 1, 62.0E0, N'Brilliant cut diamond', 1, 123456, 4.0E0, N'GIA12345446', '2020-01-01T00:00:00.0000000', 0, 6.0E0, N'Africa', 1, 500.0E0, N'DIA001', 1, 1, 57.5E0, 6.0E0),
(N'D002', CAST(0 AS bit), 1.0E0, 1, 1, 1, 62.0E0, N'Brilliant cut diamond', 1, 123456, 4.0E0, N'GIA12345446', '2020-01-01T00:00:00.0000000', 0, 6.0E0, N'Africa', 1, 500.0E0, N'DIA001', 1, 1, 57.5E0, 6.0E0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'DiamondID', N'Block', N'CaratWeight', N'ClarityGrade', N'ColorGrade', N'CutGrade', N'DepthPercent', N'Description', N'FluoresceneGrade', N'GIAReportNumber', N'Height', N'Inscription', N'IssueDate', N'LabCreated', N'Length', N'Origin', N'PolishGrade', N'Price', N'SKU', N'Shape', N'SymmetryGrade', N'TablePercent', N'Width') AND [object_id] = OBJECT_ID(N'[Diamond]'))
    SET IDENTITY_INSERT [Diamond] OFF;
GO

CREATE INDEX [IX_Order_CustomerID] ON [Order] ([CustomerID]);
GO

CREATE INDEX [IX_Order_EmployeeAssignID] ON [Order] ([EmployeeAssignID]);
GO

CREATE UNIQUE INDEX [IX_Products_AccessoryID] ON [Products] ([AccessoryID]) WHERE [AccessoryID] IS NOT NULL;
GO

CREATE UNIQUE INDEX [IX_Products_MainDiamondID] ON [Products] ([MainDiamondID]);
GO

CREATE INDEX [IX_Products_OrderID] ON [Products] ([OrderID]);
GO

CREATE INDEX [IX_SubDiamonds_ProductID] ON [SubDiamonds] ([ProductID]);
GO

CREATE UNIQUE INDEX [IX_VnPaymentResponses_OrderId] ON [VnPaymentResponses] ([OrderId]) WHERE [OrderId] IS NOT NULL;
GO

CREATE UNIQUE INDEX [IX_Warranties_ProductID] ON [Warranties] ([ProductID]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240701084522_InitialCreate', N'8.0.6');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Diamond] ADD [ImageURL] nvarchar(max) NULL;
GO

UPDATE [Accounts] SET [JoinDate] = '2024-07-01T09:39:24.3252167+07:00'
WHERE [AccountID] = N'C001';
SELECT @@ROWCOUNT;

GO

UPDATE [Accounts] SET [JoinDate] = '2024-07-01T09:39:24.3252108+07:00'
WHERE [AccountID] = N'S001';
SELECT @@ROWCOUNT;

GO

UPDATE [Accounts] SET [JoinDate] = '2024-07-01T09:39:24.3252148+07:00'
WHERE [AccountID] = N'S002';
SELECT @@ROWCOUNT;

GO

UPDATE [Diamond] SET [ImageURL] = N'https://th.bing.com/th/id/R.f570735ea23fb999beba0f0ab75776bf?rik=CmY%2bEAjezLt7Fw&riu=http%3a%2f%2fftwg.org%2fwp-content%2fuploads%2f2018%2f09%2fAdobeStock_103498617-1140x458.jpeg&ehk=j4i4O38TXR1VIDNSDUiMnV9aUME6dlkf0GM6BpnbKbk%3d&risl=&pid=ImgRaw&r=0'
WHERE [DiamondID] = N'D001';
SELECT @@ROWCOUNT;

GO

UPDATE [Diamond] SET [ImageURL] = N'https://assets.entrepreneur.com/content/3x2/2000/20160305000536-diamond.jpeg'
WHERE [DiamondID] = N'D002';
SELECT @@ROWCOUNT;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240701143924_InitialCreate1', N'8.0.6');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Products] DROP CONSTRAINT [FK_Products_Accessories_AccessoryID];
GO

DROP INDEX [IX_Products_AccessoryID] ON [Products];
GO

UPDATE [Accounts] SET [JoinDate] = '2024-07-02T03:19:38.1705533+07:00'
WHERE [AccountID] = N'C001';
SELECT @@ROWCOUNT;

GO

UPDATE [Accounts] SET [JoinDate] = '2024-07-02T03:19:38.1705465+07:00'
WHERE [AccountID] = N'S001';
SELECT @@ROWCOUNT;

GO

UPDATE [Accounts] SET [JoinDate] = '2024-07-02T03:19:38.1705512+07:00'
WHERE [AccountID] = N'S002';
SELECT @@ROWCOUNT;

GO

CREATE INDEX [IX_Products_AccessoryID] ON [Products] ([AccessoryID]);
GO

ALTER TABLE [Products] ADD CONSTRAINT [FK_Products_Accessories_AccessoryID] FOREIGN KEY ([AccessoryID]) REFERENCES [Accessories] ([AccessoryID]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240701201938_InitialCreate2', N'8.0.6');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Accessories] ADD [ImageUrl] nvarchar(max) NULL;
GO

UPDATE [Accessories] SET [ImageUrl] = NULL
WHERE [AccessoryID] = N'A001';
SELECT @@ROWCOUNT;

GO

UPDATE [Accounts] SET [JoinDate] = '2024-07-05T21:05:52.2375141+07:00'
WHERE [AccountID] = N'C001';
SELECT @@ROWCOUNT;

GO

UPDATE [Accounts] SET [JoinDate] = '2024-07-05T21:05:52.2375087+07:00'
WHERE [AccountID] = N'S001';
SELECT @@ROWCOUNT;

GO

UPDATE [Accounts] SET [JoinDate] = '2024-07-05T21:05:52.2375119+07:00'
WHERE [AccountID] = N'S002';
SELECT @@ROWCOUNT;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240705140552_InitialCreate4', N'8.0.6');
GO

COMMIT;
GO

