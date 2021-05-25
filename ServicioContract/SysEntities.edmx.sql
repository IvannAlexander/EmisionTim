
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 05/24/2021 19:31:08
-- Generated from EDMX file: D:\Alexander\Mega\Trabajo\Proyectos\Mvc\Emision\ServicioContract\SysEntities.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Db_Emision];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Sys_Certificado]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Sys_Certificado];
GO
IF OBJECT_ID(N'[dbo].[Sys_Cliente]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Sys_Cliente];
GO
IF OBJECT_ID(N'[dbo].[Sys_Dir_Empresa]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Sys_Dir_Empresa];
GO
IF OBJECT_ID(N'[dbo].[Sys_Empresa]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Sys_Empresa];
GO
IF OBJECT_ID(N'[dbo].[Sys_Menu]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Sys_Menu];
GO
IF OBJECT_ID(N'[dbo].[Sys_Menu_Usuario]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Sys_Menu_Usuario];
GO
IF OBJECT_ID(N'[dbo].[Sys_Perfiles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Sys_Perfiles];
GO
IF OBJECT_ID(N'[dbo].[Sys_Tipo_Sistema]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Sys_Tipo_Sistema];
GO
IF OBJECT_ID(N'[dbo].[Sys_Usuario]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Sys_Usuario];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Sys_Certificado'
CREATE TABLE [dbo].[Sys_Certificado] (
    [Sys_Id] int IDENTITY(1,1) NOT NULL,
    [Sys_Rfc] varchar(13)  NOT NULL,
    [Sys_Numero] varchar(50)  NOT NULL,
    [Sys_Nombre] varchar(250)  NOT NULL,
    [Sys_Certificado1] varchar(max)  NOT NULL,
    [Sys_Key] varchar(max)  NOT NULL,
    [Sys_Pwd] varchar(250)  NOT NULL,
    [Sys_Fec_Registro] datetime  NOT NULL,
    [Sys_Fec_Alta] datetime  NOT NULL,
    [Sys_Fec_Vencimiento] datetime  NOT NULL
);
GO

-- Creating table 'Sys_Dir_Empresa'
CREATE TABLE [dbo].[Sys_Dir_Empresa] (
    [Sys_Id] int IDENTITY(1,1) NOT NULL,
    [Sys_Rfc] varchar(13)  NOT NULL,
    [Sys_CodPos] varchar(5)  NOT NULL,
    [Sys_Calle] varchar(250)  NULL,
    [Sys_No_Exterior] varchar(10)  NULL,
    [Sys_No_Interior] varchar(10)  NULL,
    [Sys_Colonia] varchar(250)  NULL,
    [Sys_Municipio] varchar(250)  NULL,
    [Sys_Referencia] varchar(250)  NULL,
    [Sys_Estado] varchar(50)  NULL,
    [Sys_Pais] varchar(50)  NULL,
    [Sys_Usr_Modif] varchar(20)  NULL,
    [Sys_Fec_Registro] datetime  NOT NULL,
    [Sys_Fec_Modf] datetime  NULL
);
GO

-- Creating table 'Sys_Empresa'
CREATE TABLE [dbo].[Sys_Empresa] (
    [Sys_Id] int IDENTITY(1,1) NOT NULL,
    [Sys_Rfc] varchar(13)  NOT NULL,
    [Sys_Nombre] varchar(250)  NOT NULL,
    [Sys_Regimen_Fiscal] varchar(250)  NULL,
    [Sys_Curp] varchar(18)  NULL,
    [Sys_Pagina] varchar(100)  NULL,
    [Sys_Correo] varchar(100)  NULL,
    [Sys_Telefono] varchar(25)  NULL,
    [Sys_Usr_Modif] varchar(20)  NULL,
    [Sys_Fec_Registro] datetime  NOT NULL,
    [Sys_Fec_Modf] datetime  NULL,
    [Sys_Activo] bit  NULL
);
GO

-- Creating table 'Sys_Menu'
CREATE TABLE [dbo].[Sys_Menu] (
    [Sys_Id] int  NOT NULL,
    [Sys_Nombre] varchar(50)  NOT NULL,
    [Sys_Controller] varchar(100)  NULL,
    [Sys_Action] varchar(100)  NULL,
    [Sys_Parent] int  NULL
);
GO

-- Creating table 'Sys_Perfiles'
CREATE TABLE [dbo].[Sys_Perfiles] (
    [Sys_Id] int IDENTITY(1,1) NOT NULL,
    [Sys_Tipo] int  NOT NULL,
    [Sys_Rfc] varchar(13)  NOT NULL,
    [Sys_Perfiles1] varchar(50)  NULL
);
GO

-- Creating table 'Sys_Tipo_Sistema'
CREATE TABLE [dbo].[Sys_Tipo_Sistema] (
    [Sys_Id] int  NOT NULL,
    [Sys_Nombre] varchar(50)  NULL
);
GO

-- Creating table 'Sys_Usuario'
CREATE TABLE [dbo].[Sys_Usuario] (
    [Sys_Id] int IDENTITY(1,1) NOT NULL,
    [Sys_Rfc] varchar(13)  NOT NULL,
    [Sys_Usr] varchar(20)  NOT NULL,
    [Sys_Pass] varchar(100)  NOT NULL,
    [Sys_Correo] varchar(30)  NOT NULL,
    [Sys_Intentos] int  NULL,
    [Sys_Fec_Bloqueo] datetime  NULL,
    [Sys_Fec_Creacion] datetime  NOT NULL,
    [Sys_Ultimo_Login] datetime  NULL,
    [Sys_Estatus] int  NULL,
    [Sys_Sistema_Id] int  NULL,
    [Sys_Perfil_Id] int  NULL
);
GO

-- Creating table 'Sys_Menu_Usuario'
CREATE TABLE [dbo].[Sys_Menu_Usuario] (
    [Sys_Id] int IDENTITY(1,1) NOT NULL,
    [Sys_Menu_Id] int  NOT NULL,
    [Sys_Usuario_Id] int  NOT NULL
);
GO

-- Creating table 'Sys_Cliente'
CREATE TABLE [dbo].[Sys_Cliente] (
    [Sys_Id] int IDENTITY(1,1) NOT NULL,
    [Sys_RFC] varchar(13)  NOT NULL,
    [Sys_Razon_Social] varchar(255)  NULL,
    [Sys_Nombre_Comercial] varchar(255)  NULL,
    [Sys_Correo] varchar(255)  NULL,
    [Sys_Calle] varchar(255)  NULL,
    [Sys_CP] varchar(5)  NULL,
    [Sys_Municipio] varchar(255)  NULL,
    [Sys_Estado] varchar(255)  NULL,
    [Sys_Colonia] varchar(255)  NULL,
    [Sys_Pais] varchar(50)  NULL,
    [Sys_Fecha_Registro] datetime  NOT NULL,
    [Sys_Fecha_Modificacion] datetime  NOT NULL,
    [Sys_Usuario_Id] int  NOT NULL,
    [Sys_Empresa_Rfc] varchar(13)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Sys_Id] in table 'Sys_Certificado'
ALTER TABLE [dbo].[Sys_Certificado]
ADD CONSTRAINT [PK_Sys_Certificado]
    PRIMARY KEY CLUSTERED ([Sys_Id] ASC);
GO

-- Creating primary key on [Sys_Id] in table 'Sys_Dir_Empresa'
ALTER TABLE [dbo].[Sys_Dir_Empresa]
ADD CONSTRAINT [PK_Sys_Dir_Empresa]
    PRIMARY KEY CLUSTERED ([Sys_Id] ASC);
GO

-- Creating primary key on [Sys_Rfc] in table 'Sys_Empresa'
ALTER TABLE [dbo].[Sys_Empresa]
ADD CONSTRAINT [PK_Sys_Empresa]
    PRIMARY KEY CLUSTERED ([Sys_Rfc] ASC);
GO

-- Creating primary key on [Sys_Id] in table 'Sys_Menu'
ALTER TABLE [dbo].[Sys_Menu]
ADD CONSTRAINT [PK_Sys_Menu]
    PRIMARY KEY CLUSTERED ([Sys_Id] ASC);
GO

-- Creating primary key on [Sys_Id] in table 'Sys_Perfiles'
ALTER TABLE [dbo].[Sys_Perfiles]
ADD CONSTRAINT [PK_Sys_Perfiles]
    PRIMARY KEY CLUSTERED ([Sys_Id] ASC);
GO

-- Creating primary key on [Sys_Id] in table 'Sys_Tipo_Sistema'
ALTER TABLE [dbo].[Sys_Tipo_Sistema]
ADD CONSTRAINT [PK_Sys_Tipo_Sistema]
    PRIMARY KEY CLUSTERED ([Sys_Id] ASC);
GO

-- Creating primary key on [Sys_Id] in table 'Sys_Usuario'
ALTER TABLE [dbo].[Sys_Usuario]
ADD CONSTRAINT [PK_Sys_Usuario]
    PRIMARY KEY CLUSTERED ([Sys_Id] ASC);
GO

-- Creating primary key on [Sys_Id] in table 'Sys_Menu_Usuario'
ALTER TABLE [dbo].[Sys_Menu_Usuario]
ADD CONSTRAINT [PK_Sys_Menu_Usuario]
    PRIMARY KEY CLUSTERED ([Sys_Id] ASC);
GO

-- Creating primary key on [Sys_Id] in table 'Sys_Cliente'
ALTER TABLE [dbo].[Sys_Cliente]
ADD CONSTRAINT [PK_Sys_Cliente]
    PRIMARY KEY CLUSTERED ([Sys_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------