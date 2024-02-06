﻿CREATE TABLE [dbo].[Jobs] (
    [Id]                BIGINT         IDENTITY (1, 1) NOT NULL,
    [EmployerId]        BIGINT         NOT NULL,
    [Title]             NVARCHAR (255) NOT NULL,
    [Summary]           NVARCHAR (MAX) NOT NULL,
    [PublishedDate]     DATETIME       NULL,
    [ClosingDate]       DATETIME       NULL,
    [IsActive]          BIT            CONSTRAINT [DF_Jobs_IsActive] DEFAULT ((1)) NOT NULL,
    [IsDeleted]         BIT            CONSTRAINT [DF_Jobs_IsDeleted] DEFAULT ((0)) NOT NULL,
    [IsExpired]         BIT            CONSTRAINT [DF_Jobs_IsExpired] DEFAULT ((0)) NOT NULL,
    [DateCreated]       DATETIME       NOT NULL,
    [DateUpdated]       DATETIME       NULL,
    [CreatedBy]         NVARCHAR (255) NOT NULL,
    [UpdatedBy]         NVARCHAR (255) NULL,
    [Keywords]          NVARCHAR (200) NULL,
    [IsFeaturedJob]     BIT            NULL,
    [Hits]              INT            NULL,
    [CountryId]         BIGINT         NULL,
    [Zip]               VARCHAR (15)   NULL,
    [StateId]           BIGINT         NULL,
    [IsSiteJob]         BIT            NOT NULL,
    [SalaryRange]       NVARCHAR (50)  NULL,
    [SenderReference]   BIGINT         NULL,
    [JobXml]            TEXT           NULL,
    [RedirectionUrl]    NVARCHAR (300) NULL,
    [CounterBy]         NVARCHAR (50)  NULL,
    [Published]         BIT            NULL,
    [PermaLink]         NVARCHAR (255) NULL,
    [MinimumAge]        TINYINT        NULL,
    [MaximumAge]        TINYINT        NULL,
    [MinimumExperience] TINYINT        NULL,
    [MaximumExperience] TINYINT        NULL,
    [Currency]          NVARCHAR (100) NULL,
    [MinimumSalary]     DECIMAL (18)   NULL,
    [MaximumSalary]     DECIMAL (18)   NULL,
    [CategoryId]        INT            NULL,
    [EmploymentTypeId]  INT            NULL,
    [SepecializationId] INT            NULL,
    [QualificationId]   INT            NULL,
    CONSTRAINT [PK__job__3214EC0725FB978D] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Jobs_Category] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[Specializations] ([Id]),
    CONSTRAINT [FK_Jobs_Country] FOREIGN KEY ([CountryId]) REFERENCES [dbo].[Lists] ([Id]),
    CONSTRAINT [FK_Jobs_Employers] FOREIGN KEY ([EmployerId]) REFERENCES [dbo].[Employers] ([Id]),
    CONSTRAINT [FK_Jobs_Specialization] FOREIGN KEY ([SepecializationId]) REFERENCES [dbo].[SubSpecializations] ([Id]),
    CONSTRAINT [FK_Jobs_State] FOREIGN KEY ([StateId]) REFERENCES [dbo].[Lists] ([Id])
);



