CREATE TABLE [dbo].[EmailTemplates] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [Name]           NVARCHAR (250) NOT NULL,
    [Subject]        NVARCHAR (250) NOT NULL,
    [EmailBody]      NVARCHAR (MAX) NOT NULL,
    [IsDeleted]      BIT            NOT NULL,
    [DateCreated]    DATETIME       CONSTRAINT [DF_emailtemplate_createdate] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]      NVARCHAR (250) NOT NULL,
    [DateUpdated]    DATETIME       CONSTRAINT [DF_emailtemplate_lastmodifydate] DEFAULT (getdate()) NOT NULL,
    [UpdatedBy]      NVARCHAR (250) NOT NULL,
    [EmailEventId]   INT            NOT NULL,
    [EmailSettingId] INT            NOT NULL,
    CONSTRAINT [PK_emailtemplate] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_emailtemplate_EmailEvent] FOREIGN KEY ([EmailEventId]) REFERENCES [dbo].[EmailEvent] ([Id]),
    CONSTRAINT [FK_emailtemplate_emailsetting] FOREIGN KEY ([EmailSettingId]) REFERENCES [dbo].[emailsetting] ([id])
);

