CREATE TABLE [dbo].[ConfigParameters] (
    [Id]    INT            IDENTITY (1, 1) NOT NULL,
    [Name]  NVARCHAR (50)  NULL,
    [Value] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_ConfigParameters] PRIMARY KEY CLUSTERED ([Id] ASC)
);

