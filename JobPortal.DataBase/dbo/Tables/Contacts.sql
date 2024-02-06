CREATE TABLE [dbo].[Contacts] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [EmailAddress]  NVARCHAR (150) NOT NULL,
    [ContactTypeId] INT            NULL,
    [Username]      VARCHAR (50)   NOT NULL,
    [DateCreated]   DATETIME       CONSTRAINT [DF_Contact_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]     VARCHAR (50)   NULL,
    [DateUpdated]   DATETIME       NULL,
    [UpdatedBy]     VARCHAR (50)   NULL,
    [IsDeleted]     BIT            CONSTRAINT [DF_Contacts_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Contacts] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Contact_ContactType] FOREIGN KEY ([ContactTypeId]) REFERENCES [dbo].[ContactType] ([Id])
);

