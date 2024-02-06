CREATE TABLE [dbo].[emailsetting] (
    [id]               INT            IDENTITY (1, 1) NOT NULL,
    [site_id]          INT            NOT NULL,
    [emailsettingtype] INT            NOT NULL,
    [smtp_username]    NVARCHAR (250) NOT NULL,
    [smtp_password]    NVARCHAR (50)  NOT NULL,
    [smtp_port]        INT            NOT NULL,
    [isSecured]        BIT            NOT NULL,
    [lastmodifyby]     NVARCHAR (250) NOT NULL,
    [lastmodifydate]   DATETIME       CONSTRAINT [DF_emailsetting_lastmodifydate] DEFAULT (getdate()) NOT NULL,
    [isdeleted]        BIT            NOT NULL,
    [createdate]       DATETIME       CONSTRAINT [DF_emailsetting_createdate] DEFAULT (getdate()) NOT NULL,
    [createdby]        NVARCHAR (250) NOT NULL,
    [smtp_server]      NVARCHAR (250) NOT NULL,
    CONSTRAINT [PK_emailsetting] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_emailsetting_emailsettingtype] FOREIGN KEY ([emailsettingtype]) REFERENCES [dbo].[emailsettingtype] ([id]),
    CONSTRAINT [FK_emailsetting_site] FOREIGN KEY ([site_id]) REFERENCES [dbo].[site] ([id])
);

