CREATE TABLE [dbo].[Rights] (
    [RoleName]     NVARCHAR (50)  NOT NULL,
    [FunctionName] NVARCHAR (120) NOT NULL,
    [IsEnabled]    BIT            NOT NULL,
    CONSTRAINT [PK_Rights] PRIMARY KEY CLUSTERED ([RoleName] ASC, [FunctionName] ASC),
    CONSTRAINT [FK_Rights_Functions] FOREIGN KEY ([FunctionName]) REFERENCES [dbo].[Functions] ([FunctionName]),
    CONSTRAINT [FK_Rights_Roles] FOREIGN KEY ([RoleName]) REFERENCES [dbo].[Roles] ([RoleName])
);

