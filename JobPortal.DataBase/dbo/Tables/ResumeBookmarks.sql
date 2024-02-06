CREATE TABLE [dbo].[ResumeBookmarks] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [EmployerId]  BIGINT         NOT NULL,
    [ResumeId]    BIGINT         NOT NULL,
    [CreatedBy]   NVARCHAR (255) NOT NULL,
    [DateCreated] DATETIME       NOT NULL,
    [UpdatedBy]   NVARCHAR (255) NOT NULL,
    [DateUpdated] DATETIME       NOT NULL,
    [IsDeleted]   BIT            NOT NULL,
    CONSTRAINT [PK_ResumeBookMark] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ResumeBookMark_employer] FOREIGN KEY ([EmployerId]) REFERENCES [dbo].[Employers] ([Id]),
    CONSTRAINT [FK_ResumeBookMark_resume] FOREIGN KEY ([ResumeId]) REFERENCES [dbo].[Resumes] ([Id])
);

