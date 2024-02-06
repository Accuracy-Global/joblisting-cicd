CREATE TABLE [dbo].[Partners] (
    [id]             INT            IDENTITY (1, 1) NOT NULL,
    [name]           NVARCHAR (250) NOT NULL,
    [emailaddress]   NVARCHAR (250) NOT NULL,
    [registerdate]   DATETIME       CONSTRAINT [DF_partner_createdate] DEFAULT (getdate()) NOT NULL,
    [isdeleted]      BIT            NOT NULL,
    [createdby]      NVARCHAR (250) NOT NULL,
    [lastmodifydate] DATETIME       CONSTRAINT [DF_partner_lastmodifydate] DEFAULT (getdate()) NOT NULL,
    [lastmodifyby]   NVARCHAR (250) NOT NULL,
    CONSTRAINT [PK_partner] PRIMARY KEY CLUSTERED ([id] ASC)
);

