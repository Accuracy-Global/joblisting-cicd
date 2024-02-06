CREATE TABLE [dbo].[Categories] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (255) NOT NULL,
    [ParentId]    INT            NULL,
    [Title]       NVARCHAR (MAX) NULL,
    [Description] NVARCHAR (MAX) NULL,
    [Keywords]    NVARCHAR (MAX) NULL,
    [DateCreated] DATETIME       CONSTRAINT [DF__Categorie__DateC__1352D76D] DEFAULT (getdate()) NULL,
    [DateUpdated] DATETIME       NULL,
    [CreatedBy]   NVARCHAR (50)  NOT NULL,
    [UpdatedBy]   NVARCHAR (50)  NULL,
    [IsDeleted]   BIT            CONSTRAINT [DF__Categorie__IsDel__1446FBA6] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Categories_Categories] FOREIGN KEY ([ParentId]) REFERENCES [dbo].[Categories] ([Id])
);

