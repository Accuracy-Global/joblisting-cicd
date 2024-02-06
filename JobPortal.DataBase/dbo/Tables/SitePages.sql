CREATE TABLE [dbo].[SitePages] (
    [Id]              INT            IDENTITY (1, 1) NOT NULL,
    [PageUrl]         NVARCHAR (300) NOT NULL,
    [PageTitle]       NVARCHAR (500) NULL,
    [PageKeywords]    NVARCHAR (500) NULL,
    [PageDescription] NVARCHAR (500) NULL,
    [CreateBy]        NVARCHAR (256) NOT NULL,
    [CreateDate]      DATETIME       NOT NULL,
    [ModifyBy]        NVARCHAR (256) NOT NULL,
    [ModifyDate]      DATETIME       NOT NULL,
    [IsDeleted]       BIT            CONSTRAINT [DF_SitePages_IsDeleted] DEFAULT ((0)) NOT NULL,
    [PageContent]     TEXT           NULL,
    [PageText]        VARCHAR (25)   NULL,
    [PageId]          VARCHAR (25)   NULL,
    CONSTRAINT [PK_SitePages] PRIMARY KEY CLUSTERED ([Id] ASC)
);

