CREATE TABLE [dbo].[Subject]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY, 
    [Name] NVARCHAR(250) NOT NULL, 
    [Description] NVARCHAR(MAX) NULL, 
    [Year] INT NOT NULL, 
    [CreatedBy] INT NOT NULL, 
    [DateCreated] DATETIME NOT NULL DEFAULT GETDATE(), 
    [Archived] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [FK_Subject_User] FOREIGN KEY ([CreatedBy]) REFERENCES [User]([Id])
)
