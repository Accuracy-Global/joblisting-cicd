CREATE TABLE [dbo].[EmployerTransactions] (
    [TRANSID]        INT      IDENTITY (1, 1) NOT NULL,
    [PACKAGEID]      INT      NULL,
    [UNITSPURCHASED] INT      NULL,
    [TOTALAMOUNT]    MONEY    NULL,
    [TRANSDATE]      DATETIME CONSTRAINT [df_employertransactions_transdate] DEFAULT (getdate()) NULL,
    [ISACTIVE]       BIT      NULL,
    [USED]           INT      NULL,
    [EMPLOYERID]     BIGINT   NULL,
    [balance]        AS       ([UNITSPURCHASED]-isnull([USED],(0))),
    [expirydate]     DATETIME CONSTRAINT [df_employertransactions_expirydate] DEFAULT (getdate()+(30)) NULL,
    CONSTRAINT [PK_EmployerTransactions] PRIMARY KEY CLUSTERED ([TRANSID] ASC),
    CONSTRAINT [FK_EmployerTransactions_employer] FOREIGN KEY ([EMPLOYERID]) REFERENCES [dbo].[Employers] ([Id]),
    CONSTRAINT [FK_EmployerTransactions_EmployerTransactions] FOREIGN KEY ([TRANSID]) REFERENCES [dbo].[EmployerTransactions] ([TRANSID]),
    CONSTRAINT [FK_EmployerTransactions_Packages] FOREIGN KEY ([PACKAGEID]) REFERENCES [dbo].[Packages] ([PACKAGEID])
);

