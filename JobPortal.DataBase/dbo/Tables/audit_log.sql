CREATE TABLE [dbo].[audit_log] (
    [id]                  INT        NOT NULL,
    [table_name]          NCHAR (10) NULL,
    [column]              NCHAR (10) NULL,
    [row]                 NCHAR (10) NULL,
    [modified_date]       DATETIME   NULL,
    [changed_by_username] NCHAR (10) NULL,
    [old_value]           NCHAR (10) NULL,
    [new_value]           NCHAR (10) NULL,
    CONSTRAINT [PK_audit_log] PRIMARY KEY CLUSTERED ([id] ASC)
);

