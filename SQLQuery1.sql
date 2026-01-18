-- Drop and recreate Users table with proper structure
IF OBJECT_ID('dbo.Users', 'U') IS NOT NULL DROP TABLE dbo.Users;

CREATE TABLE [dbo].[Users] (
    [UserID] INT IDENTITY(1,1) NOT NULL,
    [Username] NVARCHAR(50) NOT NULL,
    [Password] NVARCHAR(100) NOT NULL,
    [Email] NVARCHAR(100) NOT NULL,  -- This is the missing column
    [Role] NVARCHAR(20) DEFAULT 'User',
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([UserID] ASC)
);

-- Insert admin user
INSERT INTO [dbo].[Users] ([Username], [Password], [Email], [Role])
VALUES ('admin', 'Admin123!', 'admin@meu.edu', 'Admin');


SELECT * FROM Users WHERE Email = 'admin@meu.edu';


    -- Check if user exists
SELECT * FROM AspNetUsers WHERE Email = 'admin@meu.edu';




-- Reset password for admin user
UPDATE AspNetUsers 
SET PasswordHash = 'AQAAAAEAACcQAAAAELV5G4oWJ6h7Xk8vzZt9sFjYKgJmLrE0UO+MlPdRfCwJqS8aHbAe7pNp5Q==' -- This is "Admin123!" hashed
WHERE Email = 'admin@meu.edu';



-- Insert new admin user
INSERT INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount)
VALUES (
    NEWID(), -- Id
    'admin@meu.edu', -- UserName
    'ADMIN@MEU.EDU', -- NormalizedUserName
    'admin@meu.edu', -- Email
    'ADMIN@MEU.EDU', -- NormalizedEmail
    1, -- EmailConfirmed
    'AQAAAAEAACcQAAAAELV5G4oWJ6h7Xk8vzZt9sFjYKgJmLrE0UO+MlPdRfCwJqS8aHbAe7pNp5Q==', -- PasswordHash (Admin123!)
    NEWID(), -- SecurityStamp
    NEWID(), -- ConcurrencyStamp
    NULL, -- PhoneNumber
    0, -- PhoneNumberConfirmed
    0, -- TwoFactorEnabled
    NULL, -- LockoutEnd
    1, -- LockoutEnabled
    0  -- AccessFailedCount
);

-- Add admin role
INSERT INTO AspNetUserRoles (UserId, RoleId)
SELECT u.Id, r.Id 
FROM AspNetUsers u, AspNetRoles r 
WHERE u.Email = 'admin@meu.edu' AND r.Name = 'Admin';




-- Create roles table if it doesn't exist
IF OBJECT_ID('dbo.AspNetRoles', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[AspNetRoles] (
        [Id] NVARCHAR(450) NOT NULL,
        [Name] NVARCHAR(256) NULL,
        [NormalizedName] NVARCHAR(256) NULL,
        [ConcurrencyStamp] NVARCHAR(MAX) NULL,
        CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
    
    -- Insert Admin role
    INSERT INTO AspNetRoles (Id, Name, NormalizedName) 
    VALUES (NEWID(), 'Admin', 'ADMIN');
END