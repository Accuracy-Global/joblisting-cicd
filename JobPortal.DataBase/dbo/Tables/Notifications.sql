CREATE TABLE [dbo].[Notifications] (
    [Id]                 INT            NOT NULL,
    [NotificationTypeId] INT            NULL,
    [DateSent]           DATETIME       NULL,
    [Title]              NVARCHAR (MAX) NULL,
    [Content]            NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_notification] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_notification_notifiction_type] FOREIGN KEY ([NotificationTypeId]) REFERENCES [dbo].[notifiction_type] ([id])
);

