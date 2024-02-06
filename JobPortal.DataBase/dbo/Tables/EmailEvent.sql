CREATE TABLE [dbo].[EmailEvent] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [Name]           NVARCHAR (250) NOT NULL,
    [IsDefault]      BIT            CONSTRAINT [DF_EmailEvent_IsDefault] DEFAULT ((1)) NOT NULL,
    [IsDeleted]      BIT            CONSTRAINT [DF_EmailEvent_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreateDate]     DATETIME       CONSTRAINT [DF_EmailEvent_CreateDate] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]      NVARCHAR (250) NOT NULL,
    [LastModifyDate] DATETIME       CONSTRAINT [DF_EmailEvent_LastModifyDate] DEFAULT (getdate()) NOT NULL,
    [LastModifyBy]   NVARCHAR (250) NOT NULL,
    CONSTRAINT [PK_EmailEvent] PRIMARY KEY CLUSTERED ([Id] ASC)
);

