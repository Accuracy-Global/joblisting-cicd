CREATE TABLE [dbo].[Newsletters] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Frequency]   INT            NOT NULL,
    [Type]        INT            NOT NULL,
    [CountryId]   INT            NOT NULL,
    [StartDate]   DATETIME       NOT NULL,
    [Content]     NVARCHAR (MAX) NOT NULL,
    [IsDeleted]   BIT            NOT NULL,
    [DateCreated] DATETIME       NOT NULL,
    [CreatedBy]   NVARCHAR (250) NOT NULL,
    [DateUpdated] DATETIME       NOT NULL,
    [UpdatedBy]   NVARCHAR (250) NOT NULL,
    [IsSent]      BIT            CONSTRAINT [DF_Newsletters_IsSent] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_newsletter] PRIMARY KEY CLUSTERED ([Id] ASC)
);

