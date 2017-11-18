CREATE TABLE [dbo].[Result]
(
	[StudentId] INT NOT NULL , 
    [SubjectActivityId] UNIQUEIDENTIFIER NOT NULL, 
    [DateCreated] DATETIME NOT NULL DEFAULT GETDATE(), 
    [Points] INT NOT NULL, 
    [Archived] BIT NOT NULL DEFAULT 0, 
    [GradedBy] INT NOT NULL, 
    PRIMARY KEY ([StudentId], [SubjectActivityId]), 
    CONSTRAINT [FK_Result_User] FOREIGN KEY ([GradedBy]) REFERENCES [User]([Id]), 
    CONSTRAINT [FK_Result_Student] FOREIGN KEY ([StudentId]) REFERENCES [User]([Id]), 
    CONSTRAINT [FK_Result_SubjectActivity] FOREIGN KEY ([SubjectActivityId]) REFERENCES [SubjectActivity]([Id])
)
