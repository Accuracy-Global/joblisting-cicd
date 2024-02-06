CREATE TABLE [dbo].[JobAlerts] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [CandidateId] BIGINT         NOT NULL,
    [Keywords]    NVARCHAR (100) NULL,
    [Location]    NVARCHAR (100) NULL,
    [Status]      INT            NOT NULL,
    [Frequency]   INT            NOT NULL,
    [DateCreated] DATETIME       NOT NULL,
    [DateUpdated] DATETIME       NULL,
    [CreatedBy]   NVARCHAR (255) NOT NULL,
    [UpdatedBy]   NVARCHAR (255) NULL,
    [IsDeleted]   BIT            CONSTRAINT [DF_JobAlerts_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_JobAlert] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_JobAlert_candidate] FOREIGN KEY ([CandidateId]) REFERENCES [dbo].[Candidates] ([Id])
);



