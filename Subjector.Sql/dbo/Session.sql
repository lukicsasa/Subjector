CREATE TABLE [dbo].[Session]
(
	[UserId] INT NOT NULL PRIMARY KEY, 
    [LastAction] DATETIME NOT NULL, 
    [Token] NVARCHAR(250) NULL, 
    [Active] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [FK_Session_User] FOREIGN KEY ([UserId]) REFERENCES [User]([Id])
)
