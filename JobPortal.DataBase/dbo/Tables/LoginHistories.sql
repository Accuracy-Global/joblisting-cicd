CREATE TABLE [dbo].[LoginHistories] (
    [Id]            BIGINT         IDENTITY (1, 1) NOT NULL,
    [Username]      NVARCHAR (256) NOT NULL,
    [IPAddress]     NVARCHAR (15)  NULL,
    [LoginDateTime] DATETIME       CONSTRAINT [DF_LoginHistory_LoginDateTime] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_LoginHistory] PRIMARY KEY CLUSTERED ([Id] ASC)
);

