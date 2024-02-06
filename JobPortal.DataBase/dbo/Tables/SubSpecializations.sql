CREATE TABLE [dbo].[SubSpecializations] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (100) NOT NULL,
    [CategoryId]  INT            NOT NULL,
    [IsDeleted]   BIT            NOT NULL,
    [DateCreated] DATETIME       CONSTRAINT [DF_specialisation_sub_createdate] DEFAULT (getdate()) NOT NULL,
    [DateUpdated] DATETIME       CONSTRAINT [DF_specialisation_sub_lastmodifydate] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]   NVARCHAR (250) NOT NULL,
    [UpdatedBy]   NVARCHAR (250) NOT NULL,
    [Content]     NVARCHAR (500) NULL,
    [Title]       NVARCHAR (500) NULL,
    [Keywords]    NVARCHAR (500) NULL,
    [Description] NVARCHAR (500) NULL,
    CONSTRAINT [PK_specialisation_sub] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_SubSpecializations_Specializations] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[Specializations] ([Id])
);

