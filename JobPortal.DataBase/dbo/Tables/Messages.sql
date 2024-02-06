CREATE TABLE [dbo].[Messages] (
    [Id]            INT            NOT NULL,
    [MessageTypeId] INT            NULL,
    [Template]      NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_message] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_message_message_type] FOREIGN KEY ([MessageTypeId]) REFERENCES [dbo].[message_type] ([id])
);

