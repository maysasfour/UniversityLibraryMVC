-- Drop existing tables if they exist (to start fresh)
IF OBJECT_ID('dbo.Loans', 'U') IS NOT NULL DROP TABLE dbo.Loans;
IF OBJECT_ID('dbo.Books', 'U') IS NOT NULL DROP TABLE dbo.Books;
IF OBJECT_ID('dbo.Members', 'U') IS NOT NULL DROP TABLE dbo.Members;
IF OBJECT_ID('dbo.Universities', 'U') IS NOT NULL DROP TABLE dbo.Universities;

-- Create Universities table first (referenced by other tables)
CREATE TABLE [dbo].[Universities] (
    [UniversityID] INT IDENTITY(1,1) NOT NULL,
    [Name] NVARCHAR(100) NOT NULL,
    [PrimaryColor] VARCHAR(7) DEFAULT '#003366',
    [SecondaryColor] VARCHAR(7) DEFAULT '#FFD700',
    [LogoPath] NVARCHAR(255),
    [MenuPosition] INT DEFAULT 1,
    CONSTRAINT [PK_Universities] PRIMARY KEY CLUSTERED ([UniversityID] ASC)
);

-- Create Books table
CREATE TABLE [dbo].[Books] (
    [BookID] INT IDENTITY(1,1) NOT NULL,
    [ISBN] VARCHAR(20) NOT NULL,
    [Title] NVARCHAR(255) NOT NULL,
    [Author] NVARCHAR(100) NOT NULL,
    [Publisher] NVARCHAR(100),
    [PublicationYear] INT,
    [Category] NVARCHAR(50),
    [AvailableCopies] INT DEFAULT 1,
    [TotalCopies] INT DEFAULT 1,
    [UniversityID] INT NOT NULL,
    CONSTRAINT [PK_Books] PRIMARY KEY CLUSTERED ([BookID] ASC),
    CONSTRAINT [FK_Books_Universities] FOREIGN KEY ([UniversityID]) REFERENCES [dbo].[Universities]([UniversityID])
);

-- Create Members table
CREATE TABLE [dbo].[Members] (
    [MemberID] INT IDENTITY(1,1) NOT NULL,
    [StudentID] VARCHAR(20) NOT NULL UNIQUE,
    [Name] NVARCHAR(100) NOT NULL,
    [Email] NVARCHAR(100) NOT NULL,
    [Phone] VARCHAR(20),
    [MembershipDate] DATETIME DEFAULT GETDATE(),
    [UniversityID] INT NOT NULL,
    [IsActive] BIT DEFAULT 1,
    CONSTRAINT [PK_Members] PRIMARY KEY CLUSTERED ([MemberID] ASC),
    CONSTRAINT [FK_Members_Universities] FOREIGN KEY ([UniversityID]) REFERENCES [dbo].[Universities]([UniversityID])
);

-- Create Loans table
CREATE TABLE [dbo].[Loans] (
    [LoanID] INT IDENTITY(1,1) NOT NULL,
    [BookID] INT NOT NULL,
    [MemberID] INT NOT NULL,
    [LoanDate] DATETIME DEFAULT GETDATE(),
    [DueDate] DATETIME NOT NULL,
    [ReturnDate] DATETIME NULL,
    [Status] NVARCHAR(50) DEFAULT 'Active',
    [FineAmount] DECIMAL(10,2) DEFAULT 0,
    CONSTRAINT [PK_Loans] PRIMARY KEY CLUSTERED ([LoanID] ASC),
    CONSTRAINT [FK_Loans_Books] FOREIGN KEY ([BookID]) REFERENCES [dbo].[Books]([BookID]),
    CONSTRAINT [FK_Loans_Members] FOREIGN KEY ([MemberID]) REFERENCES [dbo].[Members]([MemberID])
);

-- Insert Meu University
INSERT INTO [dbo].[Universities] ([Name], [PrimaryColor], [SecondaryColor], [LogoPath], [MenuPosition])
VALUES ('Meu University', '#003366', '#FFD700', '/images/meu-university-logo.png', 1);

-- Insert sample books
INSERT INTO [dbo].[Books] ([ISBN], [Title], [Author], [Publisher], [PublicationYear], [Category], [AvailableCopies], [TotalCopies], [UniversityID])
VALUES 
('978-3-16-148410-0', 'Introduction to Computer Science', 'John Smith', 'Tech Press', 2022, 'Computer Science', 5, 5, 1),
('978-1-56-619909-4', 'Database Systems', 'Jane Doe', 'Data Books', 2023, 'Databases', 3, 3, 1),
('978-0-26-203384-8', 'Web Development Fundamentals', 'Robert Brown', 'Web Publishing', 2021, 'Web Development', 4, 4, 1);

-- Insert sample members
INSERT INTO [dbo].[Members] ([StudentID], [Name], [Email], [Phone], [MembershipDate], [UniversityID], [IsActive])
VALUES 
('MU2024001', 'Ahmed Hassan', 'ahmed.hassan@meu.edu', '555-1234', '2024-01-15', 1, 1),
('MU2024002', 'Sara Mahmoud', 'sara.mahmoud@meu.edu', '555-5678', '2024-02-01', 1, 1),
('MU2024003', 'Mohammed Ali', 'mohammed.ali@meu.edu', '555-9012', '2024-01-20', 1, 1);