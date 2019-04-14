CREATE TABLE [dbo].[TabRows] (
    [CodTab]      NVARCHAR (120) NOT NULL,
    [Cod]         NVARCHAR (120) NOT NULL,
    [Description] NVARCHAR (120) NOT NULL,
    CONSTRAINT [PK_TabRows] PRIMARY KEY CLUSTERED ([CodTab] ASC, [Cod] ASC),
    CONSTRAINT [FK_TabRows_Tab] FOREIGN KEY ([CodTab]) REFERENCES [dbo].[Tabs] ([Cod]),
    CONSTRAINT [FK_TabRows_TabRowDetails] FOREIGN KEY ([CodTab], [Cod]) REFERENCES [dbo].[TabRowDetails] ([CodTab], [Cod])
);

