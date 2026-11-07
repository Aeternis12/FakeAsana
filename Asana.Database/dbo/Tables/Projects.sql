CREATE TABLE [dbo].[Projects]
(
	[Id] INT IDENTITY(1,1)      NOT NULL, 
    [Name] NVARCHAR(200)        NULL, 
    [Description] NVARCHAR(MAX) NULL, 
    [CompletePercent] FLOAT     NOT NULL DEFAULT 0,

    CONSTRAINT [PK_Projects] PRIMARY KEY CLUSTERED ([Id] ASC)
)
