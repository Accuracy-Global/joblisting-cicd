CREATE TABLE [dbo].[BulkJobs] (
    [Id]          NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [RefNo]       NVARCHAR (50)  NULL,
    [Title]       NVARCHAR (150) NULL,
    [Summary]     NVARCHAR (MAX) NULL,
    [Keywords]    NVARCHAR (150) NULL,
    [CreatedBy]   NVARCHAR (50)  NULL,
    [DateCreated] DATETIME       CONSTRAINT [DF_jobs_bulk_createdate] DEFAULT (getdate()) NULL,
    [Transfered]  BIT            NULL,
    CONSTRAINT [PK_jobs_bulk] PRIMARY KEY CLUSTERED ([Id] ASC)
);

