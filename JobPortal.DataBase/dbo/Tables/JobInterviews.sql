CREATE TABLE [dbo].[JobInterviews] (
    [Id]               INT            IDENTITY (1, 1) NOT NULL,
    [JobApplicationId] BIGINT         NOT NULL,
    [InterviewDate]    DATETIME       NULL,
    [InterviewEmail]   NVARCHAR (MAX) NULL,
    [Agenda]           NVARCHAR (MAX) NULL,
    [InterviewType]    INT            NULL,
    [Interviewer]      NVARCHAR (255) NULL,
    [Status]           INT            CONSTRAINT [DF_JobInterview_Status] DEFAULT ((1)) NOT NULL,
    [DateCreated]      DATETIME       NULL,
    [DateUpdated]      DATETIME       NULL,
    [CreatedBy]        NVARCHAR (255) NULL,
    [UpdatedBy]        NVARCHAR (255) NULL,
    [IsDeleted]        BIT            CONSTRAINT [DF_JobInterview_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_JobInterview] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_JobInterview_job_application] FOREIGN KEY ([JobApplicationId]) REFERENCES [dbo].[JobApplications] ([Id])
);

