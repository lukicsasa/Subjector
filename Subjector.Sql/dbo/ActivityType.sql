CREATE TABLE [dbo].[ActivityType]
(
	[Id] INT NOT NULL IDENTITY (1,1) PRIMARY KEY, 
    [Name] NVARCHAR(250) NOT NULL, 
    [Image] NVARCHAR(250) NULL, 
    [CreatedBy] INT NOT NULL, 
    [DateCreated] DATETIME NOT NULL DEFAULT GETDATE(), 
    [Archived] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [FK_ActivityType_User] FOREIGN KEY ([CreatedBy]) REFERENCES [User]([Id])
)
