CREATE TABLE [dbo].[EmailToken] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [TokenName]      NVARCHAR (100) NOT NULL,
    [TokenValue]     NVARCHAR (100) NOT NULL,
    [IsDeleted]      BIT            CONSTRAINT [DF_EmailToken_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedBy]      NVARCHAR (250) NOT NULL,
    [CreateDate]     DATETIME       CONSTRAINT [DF_EmailToken_CreateDate] DEFAULT (getdate()) NOT NULL,
    [LastModifyBy]   NVARCHAR (250) NOT NULL,
    [LastModifyDate] DATETIME       CONSTRAINT [DF_EmailToken_LastModifyDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_EmailToken] PRIMARY KEY CLUSTERED ([Id] ASC)
);

