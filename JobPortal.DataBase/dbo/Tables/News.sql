CREATE TABLE [dbo].[News] (
    [Id]             INT            NOT NULL,
    [NewsCategoryId] INT            NULL,
    [Title]          NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_news] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_news_news_category] FOREIGN KEY ([NewsCategoryId]) REFERENCES [dbo].[news_category] ([id])
);

