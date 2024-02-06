CREATE TABLE [dbo].[UserProfiles] (
    [UserId]   INT           IDENTITY (1, 1) NOT NULL,
    [UserName] NVARCHAR (56) NOT NULL,
    [Type]     INT           CONSTRAINT [DF_UserProfiles_Type] DEFAULT ((5)) NOT NULL,
    PRIMARY KEY CLUSTERED ([UserId] ASC),
    UNIQUE NONCLUSTERED ([UserName] ASC)
);

