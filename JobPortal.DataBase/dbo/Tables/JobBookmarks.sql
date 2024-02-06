CREATE TABLE [dbo].[JobBookmarks] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [CandidateId] BIGINT         NOT NULL,
    [JobId]       BIGINT         NOT NULL,
    [DateCreated] DATETIME       NOT NULL,
    [CreatedBy]   NVARCHAR (255) NOT NULL,
    [UpdatedBy]   NVARCHAR (255) NULL,
    [DateUpdated] DATETIME       NULL,
    [IsDeleted]   BIT            NOT NULL,
    CONSTRAINT [PK_JobBookMark] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_JobBookMark_candidate] FOREIGN KEY ([CandidateId]) REFERENCES [dbo].[Candidates] ([Id]),
    CONSTRAINT [FK_JobBookmarks_Jobs] FOREIGN KEY ([JobId]) REFERENCES [dbo].[Jobs] ([Id])
);



