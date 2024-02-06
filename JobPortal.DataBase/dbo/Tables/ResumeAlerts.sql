CREATE TABLE [dbo].[ResumeAlerts] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [EmployerId]  BIGINT         NOT NULL,
    [Keywords]    NVARCHAR (255) NULL,
    [Location]    NVARCHAR (255) NULL,
    [Status]      INT            NOT NULL,
    [Frequency]   INT            NOT NULL,
    [DateCreated] DATETIME       CONSTRAINT [DF_ResumeAlerts_DateCreated] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]   NVARCHAR (255) NOT NULL,
    [DateUpdated] DATETIME       NULL,
    [UpdatedBy]   NVARCHAR (255) NULL,
    [IsDeleted]   BIT            CONSTRAINT [DF_ResumeAlerts_IsDelete] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ResumeAlert] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ResumeAlert_employer] FOREIGN KEY ([EmployerId]) REFERENCES [dbo].[Employers] ([Id])
);

