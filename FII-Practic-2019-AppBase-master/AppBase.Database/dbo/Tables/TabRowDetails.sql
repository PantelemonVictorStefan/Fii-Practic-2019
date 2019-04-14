CREATE TABLE [dbo].[TabRowDetails] (
    [CodTab]    NVARCHAR (120) NOT NULL,
    [Cod]       NVARCHAR (120) NOT NULL,
    [Pos]       INT            NULL,
    [ExtraInfo] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_TabRowDetails] PRIMARY KEY CLUSTERED ([CodTab] ASC, [Cod] ASC),
    CONSTRAINT [FK_TabRowDetails_TabRows] FOREIGN KEY ([CodTab], [Cod]) REFERENCES [dbo].[TabRows] ([CodTab], [Cod])
);

