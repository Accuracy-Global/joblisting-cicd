CREATE TABLE [dbo].[Questions] (
    [Id]              INT            NOT NULL,
    [QuestionTypeId]  INT            NULL,
    [Content]         NVARCHAR (MAX) NULL,
    [CorrectOptionId] INT            NULL,
    CONSTRAINT [PK_question] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_question_question_type] FOREIGN KEY ([QuestionTypeId]) REFERENCES [dbo].[question_type] ([id])
);

