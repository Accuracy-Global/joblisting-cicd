CREATE TABLE [dbo].[ContactType] (
    [Id]              INT          IDENTITY (1, 1) NOT NULL,
    [ContactTypeName] VARCHAR (50) NOT NULL,
    [CreatedDate]     DATETIME     CONSTRAINT [DF_ContactType_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]       VARCHAR (50) NULL,
    [ModifiedDate]    DATETIME     NULL,
    [ModifiedBy]      VARCHAR (50) NULL,
    [IsDeleted]       BIT          NULL,
    CONSTRAINT [PK_ContactType] PRIMARY KEY CLUSTERED ([Id] ASC)
);

