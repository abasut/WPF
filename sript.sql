-- Скрипт создания базы данных и таблицы для WPF-приложения -- Выполните в SQL Server Management Studio или в Azure Data Studio / VS Code с расширением SQL -- Подключитесь к вашему SQLExpress серверу
USE master;
GO
-- Создаём базу данных (если уже существует — пропускаем) IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'WpfCrudDb') BEGIN CREATE DATABASE WpfCrudDb; END GO
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'WpfCrudDb')
BEGIN
    CREATE DATABASE WpfCrudDb;
END
GO

USE WpfCrudDb;
GO

IF OBJECT_ID(N'dbo.People', N'U') IS NOT NULL
    DROP TABLE dbo.People;
GO

-- Создаём таблицу People
CREATE TABLE dbo.People
(
    Id          INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    FullName    NVARCHAR(100)     NOT NULL,
    Age         INT               NULL,
    Email       NVARCHAR(150)     NULL,
    Phone       NVARCHAR(30)      NULL,
    CreatedAt   DATETIME2(0)      NOT NULL DEFAULT (SYSUTCDATETIME())
);
GO

-- Добавляем тестовые данные
INSERT INTO dbo.People (FullName, Age, Email, Phone) VALUES
(N'Иван Петров', 25, N'ivan.petrov@example.com', N'+7-900-111-22-33'),
(N'Мария Сидорова', 31, N'maria.sidorova@example.com', N'+7-900-222-33-44'),
(N'Алексей Козлов', 28, N'alex.kozlov@example.com', N'+7-900-333-44-55'),
(N'Елена Новикова', 22, N'elena.novikova@example.com', N'+7-900-444-55-66'),
(N'Дмитрий Смирнов', 35, N'dmitry.smirnov@example.com', N'+7-900-555-66-77');
GO

-- Проверка
SELECT * FROM dbo.People;
GO

PRINT N'База данных WpfCrudDb и таблица People успешно созданы.';
GO

