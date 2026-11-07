CREATE TABLE [dbo].[ToDos]
(
	[Id] INT IDENTITY(1,1)      NOT NULL, 
    [ProjectId] INT      NOT NULL, 
    [Name] NVARCHAR(200)        NULL, 
    [Description] NVARCHAR(MAX) NULL, 
    [Priority] INT              NULL, 
    [IsCompleted] BIT            NULL DEFAULT 0, 
    [DueDate] DATETIME2         NULL,

    CONSTRAINT [PK_ToDos] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ToDos_Projects] FOREIGN KEY ([ProjectId]) REFERENCES [dbo].[Projects]([Id]) ON DELETE CASCADE
)
