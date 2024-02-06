CREATE TABLE [dbo].[JobApplications] (
    [Id]               BIGINT          IDENTITY (1, 1) NOT NULL,
    [JobId]            BIGINT          NOT NULL,
    [EmployerId]       BIGINT          NULL,
    [CandidateId]      BIGINT          NOT NULL,
    [DateApply]        DATETIME        CONSTRAINT [DF_job_application_applydate] DEFAULT (getdate()) NOT NULL,
    [IsDeleted]        BIT             NOT NULL,
    [ResumeId]         BIGINT          NOT NULL,
    [DateUpdated]      DATETIME        CONSTRAINT [DF_job_application_LastModifyDate] DEFAULT (getdate()) NULL,
    [UpdatedBy]        NVARCHAR (255)  NULL,
    [CreatedBy]        NVARCHAR (255)  NULL,
    [EmployerResponse] INT             CONSTRAINT [DF_job_application_EmployerResponse] DEFAULT ((1)) NOT NULL,
    [SelectBy]         INT             CONSTRAINT [DF_job_application_SelectBy] DEFAULT ((1)) NOT NULL,
    [IsShortlisted]    BIT             CONSTRAINT [DF_job_application_IsShortList] DEFAULT ((0)) NOT NULL,
    [IsRejected]       BIT             CONSTRAINT [DF_job_application_IsRejected] DEFAULT ((0)) NOT NULL,
    [ResumeWeight]     DECIMAL (18, 2) NULL,
    [ShortlistedDate]  DATETIME        NULL,
    [NoticePeriod]     SMALLINT        NULL,
    [MatchPercentage]  FLOAT (53)      NULL,
    [LowerEducation]   INT             NULL,
    [SameEducation]    INT             NULL,
    [HigherEducation]  INT             NULL,
    [LowerSalary]      INT             NULL,
    [SameSalary]       INT             NULL,
    [HigherSalary]     INT             NULL,
    CONSTRAINT [PK_job_application] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_job_application_candidate] FOREIGN KEY ([CandidateId]) REFERENCES [dbo].[Candidates] ([Id]),
    CONSTRAINT [FK_JobApplications_Employers] FOREIGN KEY ([EmployerId]) REFERENCES [dbo].[Employers] ([Id]),
    CONSTRAINT [FK_JobApplications_Jobs] FOREIGN KEY ([JobId]) REFERENCES [dbo].[Jobs] ([Id]),
    CONSTRAINT [FK_JobApplications_Resumes] FOREIGN KEY ([ResumeId]) REFERENCES [dbo].[Resumes] ([Id])
);



