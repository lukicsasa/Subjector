CREATE TABLE [dbo].[Activity]
(
	[Id] INT NOT NULL IDENTITY (1,1) PRIMARY KEY, 
    [Name] NVARCHAR(250) NOT NULL, 
    [Description] NVARCHAR(MAX) NULL, 
    [TypeId] INT NOT NULL, 
    [CreatedBy] INT NOT NULL, 
    [DateCreated] DATETIME NOT NULL DEFAULT GETDATE(), 
    [Archived] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [FK_Activity_User] FOREIGN KEY ([CreatedBy]) REFERENCES [User]([Id]), 
    CONSTRAINT [FK_Activity_ActivityType] FOREIGN KEY ([TypeId]) REFERENCES [ActivityType]([Id])
)
