CREATE TABLE [dbo].[TrackJobHits] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [UserId]      INT            NOT NULL,
    [VisitorName] NVARCHAR (256) NOT NULL,
    [UserType]    NVARCHAR (10)  NOT NULL,
    [JobId]       BIGINT         NOT NULL,
    [VisitCount]  INT            NOT NULL,
    CONSTRAINT [PK_TrackJobView] PRIMARY KEY CLUSTERED ([Id] ASC)
);

