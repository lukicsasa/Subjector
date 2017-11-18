CREATE TABLE [dbo].[SubjectActivity]
(
	[SubjectId] INT NOT NULL , 
    [ActivityId] INT NOT NULL, 
    [Weight] DECIMAL(18, 2) NOT NULL, 
    [CreatedBy] INT NOT NULL, 
    [DateCreated] DATETIME NOT NULL DEFAULT GETDATE(), 
    [Archived] BIT NOT NULL DEFAULT 0, 
    [Id] UNIQUEIDENTIFIER UNIQUE NOT NULL DEFAULT NEWID(), 
    CONSTRAINT [FK_SubjectActivity_User] FOREIGN KEY ([CreatedBy]) REFERENCES [User]([Id]), 
    CONSTRAINT [PK_SubjectActivity] PRIMARY KEY ([SubjectId], [ActivityId]), 
    CONSTRAINT [FK_SubjectActivity_Subject] FOREIGN KEY ([SubjectId]) REFERENCES [Subject]([Id]), 
    CONSTRAINT [FK_SubjectActivity_Activity] FOREIGN KEY ([ActivityId]) REFERENCES [Activity]([Id])
)
