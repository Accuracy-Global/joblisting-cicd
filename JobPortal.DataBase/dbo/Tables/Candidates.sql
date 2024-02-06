CREATE TABLE [dbo].[Candidates] (
    [Id]                    BIGINT           IDENTITY (1, 1) NOT NULL,
    [Username]              NVARCHAR (256)   NOT NULL,
    [FirstName]             NVARCHAR (50)    NULL,
    [MiddleName]            NVARCHAR (50)    NULL,
    [LastName]              NVARCHAR (50)    NULL,
    [DateOfBirth]           DATE             NULL,
    [EmailAddress]          NVARCHAR (250)   NOT NULL,
    [AlternateEmailAddress] NVARCHAR (256)   NULL,
    [Address]               NVARCHAR (250)   NULL,
    [CountryId]             BIGINT           NULL,
    [City]                  NVARCHAR (50)    NULL,
    [State]                 NVARCHAR (50)    NULL,
    [StateId]               BIGINT           NULL,
    [Zip]                   NVARCHAR (15)    NULL,
    [Telephone]             NVARCHAR (50)    NULL,
    [DateCreated]           DATETIME         CONSTRAINT [DF_candidate_createdate] DEFAULT (getdate()) NOT NULL,
    [DateUpdated]           DATETIME         CONSTRAINT [DF_candidate_lastmodifydate] DEFAULT (getdate()) NULL,
    [CreatedBy]             NVARCHAR (250)   NOT NULL,
    [UpdatedBy]             NVARCHAR (250)   NULL,
    [JobseekerCode]         NVARCHAR (15)    NULL,
    [IsDeleted]             BIT              CONSTRAINT [DF_candidate_isdeleted] DEFAULT ((0)) NULL,
    [Comments]              NVARCHAR (MAX)   NULL,
    [UserId]                UNIQUEIDENTIFIER NULL,
    [IsPause]               BIT              NULL,
    [Photo]                 VARBINARY (MAX)  NULL,
    CONSTRAINT [PK_candidate] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Candidates_CountryList] FOREIGN KEY ([CountryId]) REFERENCES [dbo].[Lists] ([Id]),
    CONSTRAINT [FK_Candidates_StateList] FOREIGN KEY ([StateId]) REFERENCES [dbo].[Lists] ([Id]),
    CONSTRAINT [IX_Candidates] UNIQUE NONCLUSTERED ([Username] ASC)
);



