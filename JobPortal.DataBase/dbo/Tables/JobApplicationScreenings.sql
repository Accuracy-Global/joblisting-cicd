CREATE TABLE [dbo].[JobApplicationScreenings] (
    [id]                 INT            IDENTITY (1, 1) NOT NULL,
    [job_application_id] BIGINT         NOT NULL,
    [screening_id]       INT            NOT NULL,
    [status]             INT            NOT NULL,
    [screening_date]     DATETIME       CONSTRAINT [DF_job_application_screening_screening_date] DEFAULT (getdate()) NOT NULL,
    [createdby]          NVARCHAR (250) NOT NULL,
    [lastmodifyby]       NVARCHAR (250) NOT NULL,
    CONSTRAINT [PK_job_application_screening] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_job_application_screening_job_application] FOREIGN KEY ([job_application_id]) REFERENCES [dbo].[JobApplications] ([Id]),
    CONSTRAINT [FK_job_application_screening_screening] FOREIGN KEY ([screening_id]) REFERENCES [dbo].[Screenings] ([Id]),
    CONSTRAINT [FK_job_application_screening_screening_status] FOREIGN KEY ([status]) REFERENCES [dbo].[ScreeningStatuses] ([Id])
);

