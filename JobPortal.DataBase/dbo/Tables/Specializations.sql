CREATE TABLE [dbo].[Specializations] (
    [Id]               INT            IDENTITY (1, 1) NOT NULL,
    [Name]             NVARCHAR (100) NOT NULL,
    [FullName]         NVARCHAR (200) NOT NULL,
    [IsDeleted]        BIT            NOT NULL,
    [DateCreated]      DATETIME       CONSTRAINT [DF_specialisation_createdate] DEFAULT (getdate()) NOT NULL,
    [DateUpdated]      DATETIME       CONSTRAINT [DF_specialisation_lastmodifydate] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]        NVARCHAR (250) NOT NULL,
    [UpdatedBy]        NVARCHAR (250) NOT NULL,
    [Title]            NVARCHAR (500) NULL,
    [Keywords]         NVARCHAR (500) NULL,
    [Description]      NVARCHAR (500) NULL,
    [ClassificationId] INT            NULL,
    [CountJobs]        INT            NULL,
    [RateJobs]         INT            NULL,
    [Content]          NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_specialisation] PRIMARY KEY CLUSTERED ([Id] ASC)
);

