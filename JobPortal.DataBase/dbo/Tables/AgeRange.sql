CREATE TABLE [dbo].[AgeRange] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [name]           NVARCHAR (50)  NOT NULL,
    [isdeleted]      BIT            NOT NULL,
    [createdby]      NVARCHAR (255) NOT NULL,
    [createdate]     DATETIME       CONSTRAINT [DF_AgeRange_createdate] DEFAULT (getdate()) NOT NULL,
    [lastmodifyby]   NVARCHAR (255) NOT NULL,
    [lastmodifydate] DATETIME       CONSTRAINT [DF_AgeRange_lastmodifydate] DEFAULT (getdate()) NOT NULL,
    [sortorder]      INT            NULL,
    CONSTRAINT [PK_AgeRange] PRIMARY KEY CLUSTERED ([Id] ASC)
);

