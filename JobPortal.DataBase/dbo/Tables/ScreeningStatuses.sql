CREATE TABLE [dbo].[ScreeningStatuses] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [ScreeningId] INT           NOT NULL,
    [Status]      NVARCHAR (50) NOT NULL,
    [IsDeleted]   BIT           NOT NULL,
    CONSTRAINT [PK_screening_status] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_screening_status_screening] FOREIGN KEY ([ScreeningId]) REFERENCES [dbo].[Screenings] ([Id])
);

