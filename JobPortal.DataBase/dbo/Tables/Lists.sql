CREATE TABLE [dbo].[Lists] (
    [Id]         BIGINT         IDENTITY (1, 1) NOT NULL,
    [Name]       NVARCHAR (50)  NOT NULL,
    [ParentList] BIGINT         NULL,
    [Text]       NVARCHAR (MAX) NOT NULL,
    [Value]      NVARCHAR (MAX) NULL,
    [IsDefault]  BIT            CONSTRAINT [DF_Lists_IsDefault] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_List_Id] PRIMARY KEY CLUSTERED ([Id] ASC)
);

