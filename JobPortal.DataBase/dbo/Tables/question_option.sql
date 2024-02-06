CREATE TABLE [dbo].[question_option] (
    [question_id]        INT NOT NULL,
    [question_option_id] INT NOT NULL,
    CONSTRAINT [PK_question_option] PRIMARY KEY CLUSTERED ([question_id] ASC, [question_option_id] ASC),
    CONSTRAINT [FK_question_option_question] FOREIGN KEY ([question_id]) REFERENCES [dbo].[Questions] ([Id])
);

