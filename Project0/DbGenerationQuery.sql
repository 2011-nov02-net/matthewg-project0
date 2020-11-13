-- Generate Database for Project 0

DROP TABLE IF EXISTS Store
CREATE TABLE Store (
	Id INT PRIMARY KEY IDENTITY,
	Name NVARCHAR(99)
)

DROP TABLE IF EXISTS Location
CREATE TABLE Location (
	Id INT PRIMARY KEY IDENTITY,
	Name NVARCHAR(99) NOT NULL,
	Address NVARCHAR(99) NOT NULL UNIQUE,
	City NVARCHAR(99) NOT NULL,
	State NCHAR(2),
	Country NVARCHAR(99) NOT NULL,
	PostalCode NVARCHAR(99),
	Phone NVARCHAR(99),
	StoreId INT NOT NULL FOREIGN KEY REFERENCES Store (Id)
)

DROP TABLE IF EXISTS Customer
CREATE TABLE Customer (
	Id INT PRIMARY KEY IDENTITY,
	FirstName NVARCHAR(99) NOT NULL,
	LastName NVARCHAR(99) NOT NULL,
	Email NVARCHAR(99) UNIQUE NOT NULL
)

DROP TABLE IF EXISTS Product
CREATE TABLE Product (
	Id INT PRIMARY KEY IDENTITY,
	Name NVARCHAR(99) NOT NULL
)

DROP TABLE IF EXISTS LocationInventory
CREATE TABLE LocationInventory (
	LocationId INT NOT NULL FOREIGN KEY REFERENCES Location (Id),
	ProductId INT NOT NULL FOREIGN KEY REFERENCES Product (Id),
	Price MONEY NOT NULL CHECK (Price > 0),
	Stock INT NOT NULL CHECK (Stock >= 0)
)

DROP TABLE IF EXISTS [Order]
CREATE TABLE [Order] (
	Id INT PRIMARY KEY IDENTITY,
	CustomerId INT NOT NULL FOREIGN KEY REFERENCES Customer (Id),
	LocationId INT NOT NULL FOREIGN KEY REFERENCES Location (Id),
	[Date] DATETIME NOT NULL
)

DROP TABLE IF EXISTS OrderContents
CREATE TABLE OrderContents (
	OrderId INT NOT NULL FOREIGN KEY REFERENCES [Order] (Id),
	ProductId INT NOT NULL FOREIGN KEY REFERENCES Product (Id),
	Quantity INT NOT NULL CHECK (Quantity > 0)
)

-- Generate Dummy Data

INSERT INTO Store (Name) VALUES ('Walmart'), ('McDonald''s');
INSERT INTO Location (Name, Address, City, State, Country, PostalCode, Phone, StoreId) VALUES
	('Walmart Neighborhood Market', '5175 Brookberry Park Ave', 'Winston-Salem', 'NC', 'United States', '27104', '(336)245-3007', (SELECT Id FROM Store WHERE Name='Walmart')),
	('Walmart Supercenter', '4550 Kester Mill Rd', 'Winston-Salem', 'NC', 'United States', '27103', '(336) 760-9868', (SELECT Id FROM Store WHERE Name='Walmart')),
	('McDonald''s', '2060 Village Link Rd', 'Winston-Salem', 'NC', 'United States', '27106', '(336)922-1030', (SELECT Id FROM Store WHERE Name='McDonald''s')),
	('McDonald''s', '3401 Robinhood Rd', 'Winston-Salem', 'NC', 'United States', '27106', '(336)774-1625', (SELECT Id FROM Store WHERE Name='McDonald''s'));
INSERT INTO Customer (FirstName, LastName, Email) VALUES
	('Matt', 'Goodman', 'matthew.goodman@revature.net'),
	('Nick', 'Escalona', 'nick.escalona@revature.net')
INSERT INTO Product (Name) VALUES
	('10 Chicken Nuggets'),
	('Fries(sm)'),
	('Milk(1 gal)'),
	('Frozen Pizza')
INSERT INTO LocationInventory (LocationId, ProductId, Price, Stock) VALUES
	((SELECT Id FROM Location WHERE Address='2060 Village Link Rd'), (SELECT Id FROM Product WHERE Name='10 Chicken Nuggets'), 5, 200),
	((SELECT Id FROM Location WHERE Address='2060 Village Link Rd'), (SELECT Id FROM Product WHERE Name='Fries(sm)'), 1, 400),
	((SELECT Id FROM Location WHERE Address='3401 Robinhood Rd'), (SELECT Id FROM Product WHERE Name='10 Chicken Nuggets'), 5, 200),
	((SELECT Id FROM Location WHERE Address='3401 Robinhood Rd'), (SELECT Id FROM Product WHERE Name='Fries(sm)'), 5, 200),
	((SELECT Id FROM Location WHERE Address='5175 Brookberry Park Ave'), (SELECT Id FROM Product WHERE Name='Milk(1 gal)'), 3, 40),
	((SELECT Id FROM Location WHERE Address='5175 Brookberry Park Ave'), (SELECT Id FROM Product WHERE Name='Frozen Pizza'), 6, 30),
	((SELECT Id FROM Location WHERE Address='4550 Kester Mill Rd'), (SELECT Id FROM Product WHERE Name='Milk(1 gal)'), 4, 60),
	((SELECT Id FROM Location WHERE Address='4550 Kester Mill Rd'), (SELECT Id FROM Product WHERE Name='Frozen Pizza'), 5, 20)
INSERT INTO [Order] (CustomerId, LocationId, [Date]) VALUES
	((SELECT Id FROM Customer WHERE Email='matthew.goodman@revature.net'), (SELECT Id FROM Location WHERE Address='2060 Village Link Rd'), SYSDATETIME())
INSERT INTO OrderContents (OrderId, ProductId, Quantity) VALUES
	((SELECT MAX([Id]) FROM [Order] WHERE CustomerId=(SELECT Id FROM Customer WHERE Email='matthew.goodman@revature.net')), (SELECT Id FROM Product WHERE Name='10 Chicken Nuggets'), 1)

