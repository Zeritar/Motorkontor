USE master
go

CREATE DATABASE Motorkontor
GO

USE Motorkontor
GO

CREATE TABLE Login(
	Id int IDENTITY PRIMARY KEY,
	UserName nvarchar(50),
	Passwd nvarchar(50)
)

CREATE TABLE Session(
	Id int IDENTITY PRIMARY KEY,
	SessionGuid nvarchar(255) UNIQUE,
	FK_LoginId int,
	StartTime datetime
)

CREATE TABLE Zipcode(
	ZipcodeId int IDENTITY PRIMARY KEY,
	ZipcodeName nvarchar(50)
)

CREATE TABLE Address(
	AddressId int IDENTITY PRIMARY KEY,
	StreetAndNo nvarchar(256),
	FK_ZipcodeId int FOREIGN KEY REFERENCES Zipcode(ZipcodeId),
	CreateDate date
)

CREATE TABLE Category(
	CategoryId int IDENTITY PRIMARY KEY,
	CategoryName nvarchar(50)
)

CREATE TABLE Customer(
	CustomerId int IDENTITY PRIMARY KEY,
	FirstName nvarchar(50),
	LastName nvarchar(50),
	FK_AddressId int FOREIGN KEY REFERENCES Address(AddressId),
	CreateDate date
)

CREATE TABLE Fuel(
	FuelId int IDENTITY PRIMARY KEY,
	FuelName nvarchar(50)
)

CREATE TABLE Vehicle(
	VehicleId int IDENTITY PRIMARY KEY,
	Make nvarchar(50),
	Model nvarchar(50),
	FK_CategoryId int FOREIGN KEY REFERENCES Category(CategoryId),
	FK_FuelId int FOREIGN KEY REFERENCES Fuel(FuelId),
	CreateDate date
)

CREATE TABLE Registration(
	RegistrationId int IDENTITY PRIMARY KEY,
	FK_CustomerId int FOREIGN KEY REFERENCES Customer(CustomerId),
	FK_VehicleId int FOREIGN KEY REFERENCES Vehicle(VehicleId),
	FirstRegistrationDate date
)
GO

/* Customer SP */
CREATE PROCEDURE usp_readCustomerById
(
	@customerId int
)
AS
BEGIN
SELECT c.CustomerId, c.FirstName, c.LastName, a.StreetAndNo, z.ZipcodeName, c.CreateDate FROM Customer AS c
INNER JOIN Address AS a ON c.FK_AddressId = a.AddressId
INNER JOIN Zipcode AS z ON a.FK_ZipcodeId = z.ZipcodeId
WHERE c.CustomerId = @customerId
END
GO

CREATE PROCEDURE usp_readCustomers
AS
BEGIN
SELECT c.CustomerId, c.FirstName, c.LastName, a.StreetAndNo, z.ZipcodeName, c.CreateDate FROM Customer AS c
INNER JOIN Address AS a ON c.FK_AddressId = a.AddressId
INNER JOIN Zipcode AS z ON a.FK_ZipcodeId = z.ZipcodeId
END
GO

CREATE PROCEDURE usp_postCustomer
(
	@firstName nvarchar(50),
	@lastName nvarchar(50),
	@addressId int
)
AS
BEGIN
INSERT INTO Customer(FirstName, LastName, FK_AddressId, CreateDate) values (@firstName, @lastName, @addressId, GETDATE())
END
GO

CREATE PROCEDURE usp_updateCustomer
(
	@customerId int,
	@firstName nvarchar(50),
	@lastName nvarchar(50),
	@addressId int
)
AS
BEGIN
UPDATE Customer SET FirstName = @firstName, LastName = @lastName, FK_AddressId = @addressId, CreateDate = GETDATE()
WHERE CustomerId = @customerId
END
GO

CREATE PROCEDURE usp_dropCustomer
(
	@customerId int
)
AS
BEGIN
DELETE FROM Customer
WHERE CustomerId = @customerId
END
GO

/* Login SP */
CREATE PROCEDURE usp_readLoginById
(
	@id int
)
AS
BEGIN
SELECT Id, UserName FROM Login
WHERE Id = @id
END
GO

CREATE PROCEDURE usp_readLoginByUserPass
(
	@username nvarchar(50),
	@passwd nvarchar(50)
)
AS
BEGIN
SELECT Id, UserName FROM Login
WHERE UserName = @username AND Passwd = @passwd
END
GO

CREATE PROCEDURE usp_postLogin
(
	@username nvarchar(50),
	@passwd nvarchar(50)
)
AS
BEGIN
INSERT INTO Login(UserName, Passwd) values (@username, @passwd)
END
GO

CREATE PROCEDURE usp_updateLogin
(
	@id int,
	@username nvarchar(50),
	@passwd nvarchar(50)
)
AS
BEGIN
UPDATE Login SET UserName = @username, Passwd = @passwd
WHERE Id = @id
END
GO

CREATE PROCEDURE usp_dropLogin
(
	@id int
)
AS
BEGIN
DELETE FROM Login
WHERE Id = @id
END
GO

/* Session */
CREATE PROCEDURE usp_readSessionByGuid
(
	@sessionGuid nvarchar(255)
)
AS
BEGIN
SELECT s.SessionGuid, l.UserName FROM Session AS s
INNER JOIN Login AS L ON FK_LoginId = l.Id
WHERE SessionGuid = @sessionGuid
END
GO

CREATE PROCEDURE usp_postSession
(
	@sessionGuid nvarchar(255),
	@userId int
)
AS
BEGIN
INSERT INTO Session(SessionGuid, FK_LoginId, StartTime) values (@sessionGuid, @userId, GETDATE())
END
GO

CREATE PROCEDURE usp_refreshSession
(
	@sessionGuid nvarchar(255)
)
AS
BEGIN
UPDATE Session SET StartTime = GETDATE()
WHERE SessionGuid = @sessionGuid
END
GO

CREATE PROCEDURE usp_dropSession
(
	@sessionGuid nvarchar(255)
)
AS
BEGIN
DELETE FROM Session
WHERE SessionGuid = @sessionGuid
END
GO

insert into Zipcode values
('2650'),
('4930'),
('8000')

insert into Address values
('Gadevej 1', 1, GETDATE()),
('Vejstræde 5', 2, GETDATE()),
('Langgade 7', 3, GETDATE())

exec usp_postCustomer @firstName = 'Palle', @lastName = 'Westermann', @addressId = 1
GO

exec usp_postCustomer @firstName = 'Jan', @lastName = 'Stern', @addressId = 2
GO

exec usp_postCustomer @firstName = 'Ulla', @lastName = 'Hansen', @addressId = 3
GO

exec usp_readCustomers
GO

exec usp_updateCustomer @customerId = 2, @firstName = 'John', @lastName = 'Stern', @addressId = 2
GO

exec usp_readCustomers
GO

exec usp_dropCustomer @customerId = 3
GO

exec usp_readCustomers
GO