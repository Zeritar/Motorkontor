USE master
GO

DROP DATABASE Motorkontor
GO

CREATE DATABASE Motorkontor
GO

USE Motorkontor
GO

CREATE TABLE Login(
	Id int IDENTITY PRIMARY KEY,
	UserName nvarchar(50),
	Passwd nvarchar(100)
)

CREATE TABLE Session(
	Id int IDENTITY PRIMARY KEY,
	SessionGuid nvarchar(100) UNIQUE,
	FK_LoginId int,
	StartTime datetime
)

CREATE TABLE Zipcode(
	ZipcodeId int IDENTITY PRIMARY KEY,
	ZipcodeName nvarchar(8),
	CityName nvarchar(50)
)

CREATE TABLE Address(
	AddressId int IDENTITY PRIMARY KEY,
	StreetAndNo nvarchar(100),
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
SELECT c.CustomerId, c.FirstName, c.LastName, a.StreetAndNo, z.ZipcodeName, z.CityName, c.CreateDate FROM Customer AS c
INNER JOIN Address AS a ON c.FK_AddressId = a.AddressId
INNER JOIN Zipcode AS z ON a.FK_ZipcodeId = z.ZipcodeId
WHERE c.CustomerId = @customerId
END
GO

CREATE PROCEDURE usp_readCustomers
AS
BEGIN
SELECT c.CustomerId, c.FirstName, c.LastName, a.StreetAndNo, z.ZipcodeName, z.CityName, c.CreateDate FROM Customer AS c
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
	@passwd nvarchar(100)
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
	@passwd nvarchar(100)
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
	@passwd nvarchar(100)
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

/* Session SP */
CREATE PROCEDURE usp_readSessionByGuid
(
	@sessionGuid nvarchar(100)
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
	@sessionGuid nvarchar(100),
	@userId int
)
AS
BEGIN
INSERT INTO Session(SessionGuid, FK_LoginId, StartTime) values (@sessionGuid, @userId, GETDATE())
END
GO

CREATE PROCEDURE usp_refreshSession
(
	@sessionGuid nvarchar(100)
)
AS
BEGIN
UPDATE Session SET StartTime = GETDATE()
WHERE SessionGuid = @sessionGuid
END
GO

CREATE PROCEDURE usp_dropSession
(
	@sessionGuid nvarchar(100)
)
AS
BEGIN
DELETE FROM Session
WHERE SessionGuid = @sessionGuid
END
GO

/* Zipcode SP */
CREATE PROCEDURE usp_readZipcodeById
(
	@zipcodeId int
)
AS
BEGIN
SELECT ZipcodeId, ZipcodeName, CityName FROM Zipcode
WHERE ZipcodeId = @zipcodeId
END
GO

CREATE PROCEDURE usp_readZipcodes
AS
BEGIN
SELECT ZipcodeId, ZipcodeName, CityName FROM Zipcode
END
GO

CREATE PROCEDURE usp_postZipcode
(
	@zipcodeName nvarchar(8),
	@cityName nvarchar(50)
)
AS
BEGIN
INSERT INTO Zipcode(ZipcodeName, CityName) values (@zipcodeName, @cityName)
END
GO

CREATE PROCEDURE usp_updateZipcode
(
	@zipcodeId int,
	@zipcodeName nvarchar(8),
	@cityName nvarchar(50)
)
AS
BEGIN
UPDATE Zipcode SET ZipcodeName = @zipcodeName, CityName = @cityName
WHERE ZipcodeId = @zipcodeId
END
GO

CREATE PROCEDURE usp_dropZipcode
(
	@zipcodeId int
)
AS
BEGIN
DELETE FROM Zipcode
WHERE ZipcodeId = @zipcodeId
END
GO

/* Address SP */
CREATE PROCEDURE usp_readAddressById
(
	@addressId int
)
AS
BEGIN
SELECT a.AddressId, a.StreetAndNo, z.ZipcodeName, z.CityName, a.CreateDate FROM Address AS a
INNER JOIN Zipcode as z ON a.FK_ZipcodeId = z.ZipcodeId
WHERE AddressId = @addressId
END
GO

CREATE PROCEDURE usp_readAddresses
AS
BEGIN
SELECT a.AddressId, a.StreetAndNo, z.ZipcodeName, z.CityName, a.CreateDate FROM Address AS a
INNER JOIN Zipcode as z ON a.FK_ZipcodeId = z.ZipcodeId
END
GO

CREATE PROCEDURE usp_postAddress
(
	@streetAndNo nvarchar(100),
	@zipcodeId int
)
AS
BEGIN
INSERT INTO Address(StreetAndNo, FK_ZipcodeId, CreateDate) values (@streetAndNo, @zipcodeId, GETDATE())
END
GO

CREATE PROCEDURE usp_updateAddress
(
	@addressId int,
	@streetAndNo nvarchar(100),
	@zipcodeId int
)
AS
BEGIN
UPDATE Address SET StreetAndNo = @streetAndNo, FK_ZipcodeId = @zipcodeId
WHERE AddressId = @addressId
END
GO

CREATE PROCEDURE usp_dropAddress
(
	@addressId int
)
AS
BEGIN
DELETE FROM Address
WHERE AddressId = @addressId
END
GO

/* Category SP */
CREATE PROCEDURE usp_readCategoryById
(
	@categoryId int
)
AS
BEGIN
SELECT CategoryId, CategoryName FROM Category
WHERE CategoryId = @categoryId
END
GO

CREATE PROCEDURE usp_readCategories
AS
BEGIN
SELECT CategoryId, CategoryName FROM Category
END
GO

CREATE PROCEDURE usp_postCategory
(
	@categoryName nvarchar(100)
)
AS
BEGIN
INSERT INTO Category(CategoryName) values (@categoryName)
END
GO

CREATE PROCEDURE usp_updateCategory
(
	@categoryId int,
	@categoryName nvarchar(100)
)
AS
BEGIN
UPDATE Category SET CategoryName = @categoryName
WHERE CategoryId = @categoryId
END
GO

CREATE PROCEDURE usp_dropCategory
(
	@categoryId int
)
AS
BEGIN
DELETE FROM Category
WHERE CategoryId = @categoryId
END
GO

/* Fuel SP */
CREATE PROCEDURE usp_readFuelById
(
	@fuelId int
)
AS
BEGIN
SELECT FuelId, FuelName FROM Fuel
WHERE FuelId = @fuelId
END
GO

CREATE PROCEDURE usp_readFuels
AS
BEGIN
SELECT FuelId, FuelName FROM Fuel
END
GO

CREATE PROCEDURE usp_postFuel
(
	@fuelName nvarchar(50)
)
AS
BEGIN
INSERT INTO Fuel(FuelName) values (@fuelName)
END
GO

CREATE PROCEDURE usp_updateFuel
(
	@fuelId int,
	@fuelName nvarchar(50)
)
AS
BEGIN
UPDATE Fuel SET FuelName = @fuelName
WHERE FuelId = @fuelId
END
GO

CREATE PROCEDURE usp_dropFuel
(
	@fuelId int
)
AS
BEGIN
DELETE FROM Fuel
WHERE FuelId = @fuelId
END
GO

/* Vehicle SP */
CREATE PROCEDURE usp_readVehicleById
(
	@vehicleId int
)
AS
BEGIN
SELECT v.VehicleId, v.Make, v.Model, c.CategoryName, f.FuelName, v.CreateDate FROM Vehicle AS v
INNER JOIN Category AS c ON v.FK_CategoryId = c.CategoryId
INNER JOIN Fuel AS f ON v.FK_FuelId = f.FuelId
WHERE VehicleId = @vehicleId
END
GO

CREATE PROCEDURE usp_readVehicles
AS
BEGIN
SELECT v.VehicleId, v.Make, v.Model, c.CategoryName, f.FuelName, v.CreateDate FROM Vehicle AS v
INNER JOIN Category AS c ON v.FK_CategoryId = c.CategoryId
INNER JOIN Fuel AS f ON v.FK_FuelId = f.FuelId
END
GO

CREATE PROCEDURE usp_postVehicle
(
	@make nvarchar(50),
	@model nvarchar(50),
	@categoryId int,
	@fuelId int
)
AS
BEGIN
INSERT INTO Vehicle(Make, Model, FK_CategoryId, FK_FuelId, CreateDate) values (@make, @model, @categoryId, @fuelId, GETDATE())
END
GO

CREATE PROCEDURE usp_updateVehicle
(
	@vehicleId int,
	@make nvarchar(50),
	@model nvarchar(50),
	@categoryId int,
	@fuelId int
)
AS
BEGIN
UPDATE Vehicle SET Make = @make, Model = @model, FK_CategoryId = @categoryId, FK_FuelId = @fuelId
WHERE VehicleId = @vehicleId
END
GO

CREATE PROCEDURE usp_dropVehicle
(
	@vehicleId int
)
AS
BEGIN
DELETE FROM Vehicle
WHERE VehicleId = @vehicleId
END
GO

/* Registration SP */
CREATE PROCEDURE usp_readRegistrationById
(
	@registrationId int
)
AS
BEGIN
SELECT r.RegistrationId, c.FirstName, c.LastName, v.Make, v.Model, r.FirstRegistrationDate FROM Registration AS r
INNER JOIN Customer AS c ON r.FK_CustomerId = c.CustomerId
INNER JOIN Vehicle AS v ON r.FK_VehicleId = v.VehicleId
WHERE RegistrationId = @registrationId
END
GO

CREATE PROCEDURE usp_readRegistrations
AS
BEGIN
SELECT r.RegistrationId, c.FirstName, c.LastName, v.Make, v.Model, r.FirstRegistrationDate FROM Registration AS r
INNER JOIN Customer AS c ON r.FK_CustomerId = c.CustomerId
INNER JOIN Vehicle AS v ON r.FK_VehicleId = v.VehicleId
END
GO

CREATE PROCEDURE usp_postRegistration
(
	@customerId int,
	@vehicleId int,
	@firstRegistrationDate date
)
AS
BEGIN
INSERT INTO Registration(FK_CustomerId, FK_VehicleId, FirstRegistrationDate) values (@customerId, @vehicleId, @firstRegistrationDate)
END
GO

CREATE PROCEDURE usp_updateRegistration
(
	@registrationId int,
	@customerId int,
	@vehicleId int,
	@firstRegistrationDate date
)
AS
BEGIN
UPDATE Registration SET FK_CustomerId = @customerId, FK_VehicleId = @vehicleId, @firstRegistrationDate = @firstRegistrationDate
WHERE RegistrationId = @registrationId
END
GO

CREATE PROCEDURE usp_dropRegistration
(
	@registrationId int
)
AS
BEGIN
DELETE FROM Registration
WHERE RegistrationId = @registrationId
END
GO

/* Seed Data */
exec usp_postZipcode @zipcodeName = '2650', @cityName = 'Hvidovre'
GO

exec usp_postZipcode @zipcodeName = '4930', @cityName = 'Maribo'
GO

exec usp_postZipcode @zipcodeName = '8000', @cityName = 'Aarhus C'
GO

exec usp_postAddress @streetAndNo = 'Gadevej 1', @zipcodeId = 1
GO

exec usp_postAddress @streetAndNo = 'Vejstræde 5', @zipcodeId = 2
GO

exec usp_postAddress @streetAndNo = 'Langgade 7', @zipcodeId = 3
GO

exec usp_postCustomer @firstName = 'Palle', @lastName = 'Westermann', @addressId = 1
GO

exec usp_postCustomer @firstName = 'Jan', @lastName = 'Stern', @addressId = 2
GO

exec usp_postCustomer @firstName = 'Ulla', @lastName = 'Hansen', @addressId = 3
GO

exec usp_postCategory @categoryName = 'Van'
GO

exec usp_postCategory @categoryName = 'Hatchback'
GO

exec usp_postCategory @categoryName = 'Sedan'
GO

exec usp_postFuel @fuelName = 'Gasoline'
GO

exec usp_postFuel @fuelName = 'Diesel'
GO

exec usp_postFuel @fuelName = 'Electrical'
GO

exec usp_postVehicle @make = 'Volkswagen', @model = 'Vento', @categoryId = 2, @fuelId = 1
GO

exec usp_postVehicle @make = 'Mercedes', @model = 'Transporter', @categoryId = 1, @fuelId = 2
GO

exec usp_postVehicle @make = 'Tesla', @model = 'Model S', @categoryId = 3, @fuelId = 3
GO

exec usp_postRegistration @customerId = 1, @vehicleId = 1, @firstRegistrationDate = '2007-03-15'
GO

exec usp_postRegistration @customerId = 2, @vehicleId = 2, @firstRegistrationDate = '2014-07-24'
GO

exec usp_postRegistration @customerId = 3, @vehicleId = 3, @firstRegistrationDate = '2020-01-07'
GO

exec usp_postLogin @username = 'username', @passwd = 'password'
GO


/* Pull seed data*/
exec usp_readCustomers
GO

exec usp_readAddresses
GO

exec usp_readZipcodes
GO

exec usp_readCategories
GO

exec usp_readFuels
GO

exec usp_readVehicles
GO

exec usp_readRegistrations
GO

exec usp_readLoginById @id = 1
GO