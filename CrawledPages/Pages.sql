﻿CREATE TABLE [dbo].[Pages]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Uri] NVARCHAR(MAX) NOT NULL, 
    [Content] NVARCHAR(MAX) NOT NULL
)
