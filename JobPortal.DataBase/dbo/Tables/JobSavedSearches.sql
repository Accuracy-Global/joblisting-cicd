CREATE TABLE [dbo].[JobSavedSearches] (
    [Id]          BIGINT         IDENTITY (1, 1) NOT NULL,
    [Url]         VARCHAR (255)  NULL,
    [Title]       VARCHAR (255)  NULL,
    [CandidateId] BIGINT         NOT NULL,
    [DateUpdated] DATETIME       CONSTRAINT [DF_job_savedsearch_lastmodifydate] DEFAULT (getdate()) NULL,
    [UpdatedBy]   NVARCHAR (255) NULL,
    CONSTRAINT [PK_SavedSearch] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_SavedSearch_candidate1] FOREIGN KEY ([CandidateId]) REFERENCES [dbo].[Candidates] ([Id])
);



