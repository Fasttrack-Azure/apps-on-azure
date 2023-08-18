CREATE TABLE [dbo].[OrderLines]
(
	[Id] INT IDENTITY NOT NULL PRIMARY KEY, 
    [Quantity] INT NULL, 
    [CookieId] INT NULL, 
    [OrderId] INT NULL,
    CONSTRAINT [CookieForeignKey] FOREIGN KEY (CookieId) REFERENCES [dbo].[Cookies](Id),
    CONSTRAINT [OrderForeignKey] FOREIGN KEY (OrderId) REFERENCES [dbo].[Orders](Id) ON DELETE CASCADE
)
