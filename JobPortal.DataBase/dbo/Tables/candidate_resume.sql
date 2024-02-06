CREATE TABLE [dbo].[candidate_resume] (
    [candidate_id] BIGINT NOT NULL,
    [resume_id]    BIGINT NOT NULL,
    CONSTRAINT [PK_candidate_resume] PRIMARY KEY CLUSTERED ([candidate_id] ASC, [resume_id] ASC),
    CONSTRAINT [FK_candidate_resume_candidate] FOREIGN KEY ([candidate_id]) REFERENCES [dbo].[Candidates] ([Id]),
    CONSTRAINT [FK_candidate_resume_resume] FOREIGN KEY ([resume_id]) REFERENCES [dbo].[Resumes] ([Id])
);

