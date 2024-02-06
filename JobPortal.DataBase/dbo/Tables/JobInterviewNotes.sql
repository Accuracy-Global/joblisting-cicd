CREATE TABLE [dbo].[JobInterviewNotes] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [InterviewId] INT            NOT NULL,
    [Comments]    NVARCHAR (MAX) NULL,
    [DateCreated] DATETIME       NULL,
    [DateUpdated] DATETIME       NULL,
    [CreatedBy]   NVARCHAR (255) NULL,
    [UpdatedBy]   NVARCHAR (255) NULL,
    [IsDeleted]   BIT            CONSTRAINT [DF_JobInterviewNotes_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_JobInterviewNotes] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_JobInterviewNotes_JobInterview] FOREIGN KEY ([InterviewId]) REFERENCES [dbo].[JobInterviews] ([Id])
);

