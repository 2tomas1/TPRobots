-- Crea la base real del torneo si no existe.
-- Ejecutar en SSMS conectado a localhost\SQLEXPRESS.

IF DB_ID('TorneoRobots') IS NULL
    CREATE DATABASE TorneoRobots;
GO

USE TorneoRobots;
GO

IF OBJECT_ID('dbo.Robots') IS NULL
CREATE TABLE dbo.Robots (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(MAX) NOT NULL,
    Tipo NVARCHAR(MAX) NOT NULL,
    Energia INT NOT NULL,
    NivelHabilidad INT NOT NULL,
    Estado NVARCHAR(MAX) NOT NULL
);
GO

IF OBJECT_ID('dbo.ResultadosDuelos') IS NULL
CREATE TABLE dbo.ResultadosDuelos (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Ganador NVARCHAR(MAX) NOT NULL,
    Perdedor NVARCHAR(MAX) NOT NULL,
    Fecha DATETIME2 NOT NULL
);
GO
