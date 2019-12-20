CREATE TABLE [dbo].[MainDocuments] (
    [Id]   INT            IDENTITY (1, 1) NOT NULL,
    [url]  NVARCHAR (MAX) NULL,
    [page] INT            NOT NULL,
    CONSTRAINT [PK_dbo.MainDocuments] PRIMARY KEY CLUSTERED ([Id] ASC)
);

