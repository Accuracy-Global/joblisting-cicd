CREATE TABLE [dbo].[EmailEventToken] (
    [EmailEventId] INT NOT NULL,
    [EmailTokenId] INT NOT NULL,
    CONSTRAINT [PK_EmailEventToken] PRIMARY KEY CLUSTERED ([EmailEventId] ASC, [EmailTokenId] ASC),
    CONSTRAINT [FK_EmailEventToken_EmailEvent] FOREIGN KEY ([EmailEventId]) REFERENCES [dbo].[EmailEvent] ([Id]),
    CONSTRAINT [FK_EmailEventToken_EmailToken] FOREIGN KEY ([EmailTokenId]) REFERENCES [dbo].[EmailToken] ([Id])
);

