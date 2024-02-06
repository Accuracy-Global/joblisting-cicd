CREATE TABLE [dbo].[Invoices] (
    [id]            INT        NOT NULL,
    [username]      NCHAR (10) NULL,
    [creation_date] DATETIME   NULL,
    CONSTRAINT [PK_invoice] PRIMARY KEY CLUSTERED ([id] ASC)
);

