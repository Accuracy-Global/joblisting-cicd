CREATE TABLE [dbo].[Packages] (
    [PACKAGEID]        INT            IDENTITY (1, 1) NOT NULL,
    [PACKAGENAME]      NVARCHAR (150) NOT NULL,
    [UNITS]            INT            NOT NULL,
    [UNITPRICE]        MONEY          NOT NULL,
    [isActive]         BIT            NOT NULL,
    [createdby]        NVARCHAR (150) NOT NULL,
    [createddate]      DATETIME       CONSTRAINT [DF_packages_createdate] DEFAULT (getdate()) NULL,
    [lastmodifiedby]   NVARCHAR (150) NULL,
    [lastmodifieddate] DATETIME       CONSTRAINT [DF_packages_lastmodifydate] DEFAULT (getdate()) NULL,
    [RoleName]         NVARCHAR (50)  NULL,
    [totalAmount]      AS             ([UNITS]*[UNITPRICE]),
    CONSTRAINT [PK_Packages] PRIMARY KEY CLUSTERED ([PACKAGEID] ASC)
);

