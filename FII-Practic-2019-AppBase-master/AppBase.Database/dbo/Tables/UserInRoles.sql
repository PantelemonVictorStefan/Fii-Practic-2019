CREATE TABLE [dbo].[UserInRoles] (
    [UserName] NVARCHAR (250) NOT NULL,
    [RoleName] NVARCHAR (50)  NOT NULL,
    CONSTRAINT [PK_UserInRoles] PRIMARY KEY CLUSTERED ([UserName] ASC, [RoleName] ASC),
    CONSTRAINT [FK_UserInRoles_Roles] FOREIGN KEY ([RoleName]) REFERENCES [dbo].[Roles] ([RoleName]),
    CONSTRAINT [FK_UserInRoles_Users] FOREIGN KEY ([UserName]) REFERENCES [dbo].[Users] ([UserName])
);

