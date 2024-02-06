CREATE TABLE [dbo].[emailsettingtype] (
    [id]             INT            IDENTITY (1, 1) NOT NULL,
    [EmailTypeName]  NVARCHAR (250) NOT NULL,
    [IsDefault]      BIT            CONSTRAINT [DF_emailsettingtype_IsDefault] DEFAULT ((1)) NOT NULL,
    [IsDelete]       BIT            CONSTRAINT [DF_emailsettingtype_IsDelete] DEFAULT ((0)) NOT NULL,
    [CreateDate]     DATETIME       CONSTRAINT [DF_emailsettingtype_CreateDate] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]      NVARCHAR (250) NOT NULL,
    [LastModifyDate] DATETIME       CONSTRAINT [DF_emailsettingtype_LastModifyDate] DEFAULT (getdate()) NOT NULL,
    [LastModifyBy]   NVARCHAR (250) NOT NULL,
    CONSTRAINT [PK_emailsettingtype] PRIMARY KEY CLUSTERED ([id] ASC)
);

