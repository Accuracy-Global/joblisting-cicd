CREATE TABLE [dbo].[Qualifications] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (50)  NOT NULL,
    [IsDeleted]   BIT            NOT NULL,
    [DateCreated] DATETIME       CONSTRAINT [DF_qualification_createdate] DEFAULT (getdate()) NOT NULL,
    [DateUpdated] DATETIME       CONSTRAINT [DF_qualification_lastmodifydate] DEFAULT (getdate()) NULL,
    [CreatedBy]   NVARCHAR (250) NOT NULL,
    [UpdatedBy]   NVARCHAR (250) NULL,
    [SortOrder]   INT            CONSTRAINT [DF_Qualifications_SortOrder] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_qualification] PRIMARY KEY CLUSTERED ([Id] ASC)
);

