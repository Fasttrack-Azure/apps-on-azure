﻿CREATE TABLE [dbo].[Cookies]
(
	[Id] INT IDENTITY NOT NULL PRIMARY KEY, 
    [Name] VARCHAR(50) NULL, 
    [ImageUrl] NVARCHAR(150) NULL, 
    [Price] FLOAT NULL
)