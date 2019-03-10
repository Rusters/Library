
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 11/19/2018 21:00:19
-- Generated from EDMX file: C:\Users\Ruster\Desktop\Interface\Interface\Models\Diagram.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Library];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_AuthorsBooks]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Books] DROP CONSTRAINT [FK_AuthorsBooks];
GO
IF OBJECT_ID(N'[dbo].[FK_BookExtrasBooks]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BookExtras] DROP CONSTRAINT [FK_BookExtrasBooks];
GO
IF OBJECT_ID(N'[dbo].[FK_BookExtrasReaders]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BookExtras] DROP CONSTRAINT [FK_BookExtrasReaders];
GO
IF OBJECT_ID(N'[dbo].[FK_BooksArticulBooks]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ArticulBooks] DROP CONSTRAINT [FK_BooksArticulBooks];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Authors]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Authors];
GO
IF OBJECT_ID(N'[dbo].[BookExtras]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BookExtras];
GO
IF OBJECT_ID(N'[dbo].[Books]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Books];
GO
IF OBJECT_ID(N'[dbo].[Readers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Readers];
GO
IF OBJECT_ID(N'[dbo].[ArticulBooks]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ArticulBooks];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Authors'
CREATE TABLE [dbo].[Authors] (
    [id] int IDENTITY(1,1) NOT NULL,
    [fio] nvarchar(50)  NOT NULL,
    [year_of_life] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'BookExtras'
CREATE TABLE [dbo].[BookExtras] (
    [id] int IDENTITY(1,1) NOT NULL,
    [date_out] datetime  NOT NULL,
    [date_get] datetime  NOT NULL,
    [cost] int  NOT NULL,
    [Readers_id] int  NOT NULL,
    [ArticulBooks_Id] int  NOT NULL
);
GO

-- Creating table 'Books'
CREATE TABLE [dbo].[Books] (
    [id] int IDENTITY(1,1) NOT NULL,
    [name] nvarchar(50)  NOT NULL,
    [date_public] nvarchar(50)  NOT NULL,
    [cost] int  NOT NULL,
    [Authors_id] int  NOT NULL
);
GO

-- Creating table 'Readers'
CREATE TABLE [dbo].[Readers] (
    [id] int IDENTITY(1,1) NOT NULL,
    [fio] nvarchar(50)  NOT NULL,
    [adress] nvarchar(50)  NOT NULL,
    [phone] nvarchar(50)  NOT NULL,
    [kolvo_books] int  NOT NULL,
    [date_reg] datetime  NOT NULL
);
GO

-- Creating table 'ArticulBooks'
CREATE TABLE [dbo].[ArticulBooks] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [articul] int  NOT NULL,
    [Books_id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [id] in table 'Authors'
ALTER TABLE [dbo].[Authors]
ADD CONSTRAINT [PK_Authors]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'BookExtras'
ALTER TABLE [dbo].[BookExtras]
ADD CONSTRAINT [PK_BookExtras]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'Books'
ALTER TABLE [dbo].[Books]
ADD CONSTRAINT [PK_Books]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'Readers'
ALTER TABLE [dbo].[Readers]
ADD CONSTRAINT [PK_Readers]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [Id] in table 'ArticulBooks'
ALTER TABLE [dbo].[ArticulBooks]
ADD CONSTRAINT [PK_ArticulBooks]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Authors_id] in table 'Books'
ALTER TABLE [dbo].[Books]
ADD CONSTRAINT [FK_AuthorsBooks]
    FOREIGN KEY ([Authors_id])
    REFERENCES [dbo].[Authors]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AuthorsBooks'
CREATE INDEX [IX_FK_AuthorsBooks]
ON [dbo].[Books]
    ([Authors_id]);
GO

-- Creating foreign key on [Readers_id] in table 'BookExtras'
ALTER TABLE [dbo].[BookExtras]
ADD CONSTRAINT [FK_BookExtrasReaders]
    FOREIGN KEY ([Readers_id])
    REFERENCES [dbo].[Readers]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BookExtrasReaders'
CREATE INDEX [IX_FK_BookExtrasReaders]
ON [dbo].[BookExtras]
    ([Readers_id]);
GO

-- Creating foreign key on [Books_id] in table 'ArticulBooks'
ALTER TABLE [dbo].[ArticulBooks]
ADD CONSTRAINT [FK_BooksArticulBooks]
    FOREIGN KEY ([Books_id])
    REFERENCES [dbo].[Books]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BooksArticulBooks'
CREATE INDEX [IX_FK_BooksArticulBooks]
ON [dbo].[ArticulBooks]
    ([Books_id]);
GO

-- Creating foreign key on [ArticulBooks_Id] in table 'BookExtras'
ALTER TABLE [dbo].[BookExtras]
ADD CONSTRAINT [FK_BookExtrasArticulBooks]
    FOREIGN KEY ([ArticulBooks_Id])
    REFERENCES [dbo].[ArticulBooks]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BookExtrasArticulBooks'
CREATE INDEX [IX_FK_BookExtrasArticulBooks]
ON [dbo].[BookExtras]
    ([ArticulBooks_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------