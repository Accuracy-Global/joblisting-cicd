CREATE TABLE [dbo].[site] (
    [id]                  INT            IDENTITY (1, 1) NOT NULL,
    [partner_id]          INT            NOT NULL,
    [sitename]            NVARCHAR (100) NOT NULL,
    [siteaddress]         NVARCHAR (100) NOT NULL,
    [sitelogoname]        NVARCHAR (250) NULL,
    [supportemailaddress] NVARCHAR (250) NULL,
    [domainname]          NVARCHAR (250) NULL,
    [notifyjobs]          BIT            NOT NULL,
    [isdeleted]           BIT            NOT NULL,
    [createdate]          DATETIME       CONSTRAINT [DF_site_createdate] DEFAULT (getdate()) NOT NULL,
    [createdby]           NVARCHAR (250) NOT NULL,
    [lastmodifydate]      DATETIME       CONSTRAINT [DF_site_lastmodifydate] DEFAULT (getdate()) NOT NULL,
    [lastmodifyby]        NVARCHAR (250) NOT NULL,
    CONSTRAINT [PK_site] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_site_partner] FOREIGN KEY ([partner_id]) REFERENCES [dbo].[Partners] ([id])
);

