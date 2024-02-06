CREATE TABLE [dbo].[Screenings] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [Name]      NVARCHAR (100) NOT NULL,
    [IsDeleted] BIT            NOT NULL,
    CONSTRAINT [PK_screening] PRIMARY KEY CLUSTERED ([Id] ASC)
);

