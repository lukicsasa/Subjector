CREATE TABLE [dbo].[Cert]
( 
    [Id] INT NOT NULL IDENTITY (1,1), 
	[UserId] INT NOT NULL,
    [CertNumber] NVARCHAR(250) NOT NULL, 
    [Active] BIT NOT NULL DEFAULT 1, 
    CONSTRAINT [FK_Cert_User] FOREIGN KEY ([UserId]) REFERENCES [User]([Id]), 
    PRIMARY KEY ([Id])
)
