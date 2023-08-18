CREATE TABLE [dbo].[Orders]
(
	[Id] INT IDENTITY NOT NULL PRIMARY KEY, 
    [Date] DATETIMEOFFSET NULL, 
    [Price] FLOAT NULL, 
    [Status] NVARCHAR(150) NULL, 
    [StoreId] INT NULL,
    CONSTRAINT [StoreForeignKey] FOREIGN KEY (StoreId) REFERENCES [dbo].[Stores](Id)
)