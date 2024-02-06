CREATE TABLE [dbo].[UserSocialSiteInfo] (
    [Id]              BIGINT         IDENTITY (1, 1) NOT NULL,
    [Username]        NVARCHAR (256) NOT NULL,
    [SocialNetworkId] NVARCHAR (256) NOT NULL,
    [SocialSiteName]  NVARCHAR (256) NOT NULL,
    [DateCreated]     DATETIME       NOT NULL,
    CONSTRAINT [PK_UserSocialSiteInfo] PRIMARY KEY CLUSTERED ([Id] ASC)
);

