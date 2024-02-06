CREATE TABLE [dbo].[BlockedEmployers] (
    [Id]          BIGINT        IDENTITY (1, 1) NOT NULL,
    [EmployerId]  BIGINT        NOT NULL,
    [JobseekerId] BIGINT        NOT NULL,
    [DateCreated] DATETIME      CONSTRAINT [DF_BlockedEmployers_DateCreated] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]   NVARCHAR (50) NOT NULL,
    [DateUpdated] DATETIME      NULL,
    [UpdatedBy]   NVARCHAR (50) NULL,
    CONSTRAINT [PK_BlockedEmployers] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_BlockedEmployers_Candidates] FOREIGN KEY ([JobseekerId]) REFERENCES [dbo].[Candidates] ([Id]),
    CONSTRAINT [FK_BlockedEmployers_Employers] FOREIGN KEY ([EmployerId]) REFERENCES [dbo].[Employers] ([Id])
);

