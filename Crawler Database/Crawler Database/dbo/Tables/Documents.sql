CREATE TABLE [dbo].[Documents] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [url]            NVARCHAR (MAX) NULL,
    [html]           NVARCHAR (MAX) NULL,
    [mainDocumentId] INT            NULL,
    CONSTRAINT [PK_dbo.Documents] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.Documents_dbo.MainDocuments_mainDocumentId] FOREIGN KEY ([mainDocumentId]) REFERENCES [dbo].[MainDocuments] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_mainDocumentId]
    ON [dbo].[Documents]([mainDocumentId] ASC);

