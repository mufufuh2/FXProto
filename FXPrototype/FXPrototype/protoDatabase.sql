-- DECIMAL(n,m) is more efficient than FLOAT
-- Run this on MySQL for dummy data

DROP TABLE IF EXISTS `Account`;

CREATE TABLE IF NOT EXISTS `Account` (
	`AccountID` INT				NOT NULL	AUTO_INCREMENT,
	`Username`	NVARCHAR(40)	NOT NULL,
	-- Password Hashing to be implemented later, consider adding salting too --
	`PwdHash`	BINARY(64)		NULL,
	-- Plaintext Password to be deleted when hashing is implemented --
	`Password`	NVARCHAR(40)	NOT NULL,
	`FirstName`	NVARCHAR(40)	NULL,
	`LastName`	NVARCHAR(40)	NULL,
	`CompanyID`	INT				NULL,
	PRIMARY KEY (`AccountID` ASC)
);

-- --------------------------------------------------------------------------------------------------------------

DROP TABLE IF EXISTS `Company`;

CREATE TABLE IF NOT EXISTS `Company` (
	`CompanyID`		INT				NOT NULL	AUTO_INCREMENT,
	`CompanyName`	NVARCHAR(40)	NOT NULL,
	PRIMARY KEY (`CompanyID` ASC)
);

-- --------------------------------------------------------------------------------------------------------------

DROP TABLE IF EXISTS `Currency`;

CREATE TABLE IF NOT EXISTS `Currency` (
	`CurrencyID`	INT				NOT NULL	AUTO_INCREMENT,
	`CurrencyCode`	VARCHAR(4)		NOT NULL,
	`CurrencyName`	NVARCHAR(40)	NOT NULL,
    `Symbol`		VARCHAR(4)		NULL,
	PRIMARY KEY (`CurrencyID` ASC)
);

-- -----------------------------------------------------------------------------------------------------------

DROP TABLE IF EXISTS `BuyRates`;

CREATE TABLE IF NOT EXISTS `BuyRates` (
	`CompanyID` 	INT		NOT NULL,
    `CurrencyID`	INT		NOT NULL,
    `Rate`			FLOAT	NOT NULL,
--  `BaseRate`		INT		NOT NULL,
	PRIMARY KEY ( `CompanyID`, `CurrencyID`),
		FOREIGN KEY (`CompanyID`)
		REFERENCES `Company` (`CompanyID`)
		ON DELETE NO ACTION
		ON UPDATE CASCADE,
		FOREIGN KEY (`CurrencyID`)
		REFERENCES `Currency` (`CurrencyID`)
		ON DELETE NO ACTION
		ON UPDATE CASCADE
);

DROP TABLE IF EXISTS `BuyRateHist`;
-- Intermediary Table for associating a set of rates with a company, stores historical data --
CREATE TABLE IF NOT EXISTS `BuyRateHist`(
	`CompanyID`		INT			NOT NULL,
	`CurrencyID`	INT			NOT NULL,
    `Rate`			FLOAT		NOT NULL,
--  `BaseRate`		INT			NOT NULL,
-- List of exchange rates will often rotate so it is important to have a Validity field -- 
	`ValidFrom`		DATETIME	NOT NULL,
-- Optional ValidTo field for easier histogram creation/data collection -- 
	`ValidTo`		DATETIME	NULL,
	PRIMARY KEY ( `CompanyID`, `CurrencyID`, `ValidFrom`)
);

-- --------------------------------------------------------------------------------------------------------------

DROP TABLE IF EXISTS `SellRates`;

CREATE TABLE IF NOT EXISTS `SellRates` (
	`CompanyID` 	INT			NOT NULL,
    `CurrencyID`	INT			NOT NULL,
    `Rate`			FLOAT		NOT NULL,
--  `BaseRate`		INT			NOT NULL,
	PRIMARY KEY ( `CompanyID`, `CurrencyID`),
		FOREIGN KEY (`CompanyID`)
		REFERENCES `Company` (`CompanyID`)
		ON DELETE NO ACTION
		ON UPDATE CASCADE,
		FOREIGN KEY (`CurrencyID`)
		REFERENCES `Currency` (`CurrencyID`)
		ON DELETE NO ACTION
		ON UPDATE CASCADE
);

DROP TABLE IF EXISTS `SellRateHist`;
-- Intermediary Table for associating a set of rates with a company, stores historical data --
CREATE TABLE IF NOT EXISTS `SellRateHist`(
	`CompanyID`		INT			NOT NULL,
	`CurrencyID`	INT			NOT NULL,
    `Rate`			FLOAT		NOT NULL,
--  `BaseRate`		INT		NOT NULL,
-- List of exchange rates will often rotate so it is important to have a Validity field -- 
	`ValidFrom`		DATETIME	NOT NULL,
-- Optional ValidTo field for easier histogram creation/data collection -- 
	`ValidTo`		DATETIME	NULL,
	PRIMARY KEY ( `CompanyID`, `CurrencyID`, `ValidFrom`)
);

-- --------------------------------------------------------------------------------------------------------------

DROP TABLE IF EXISTS `Address`;

CREATE TABLE IF NOT EXISTS `Address`(
	`AddressID`		INT				NOT NULL	AUTO_INCREMENT,
	`AddressName`	NVARCHAR(40)	NULL,
	`Country`		NVARCHAR(30)	NOT NULL,
	`State`			NVARCHAR(30)	NOT NULL,
	`City`			NVARCHAR(30)	NOT NULL,
	`Street1`		NVARCHAR(50)	NOT NULL,
	`Street2`		NVARCHAR(40)	NULL,
	`PostCode`		VARCHAR(20)		NOT NULL,
-- Lat and Lon to be used for positional checking which is used to better assign 
	`Latitude`		FLOAT			NULL,
	`Longitude`		FLOAT			NULL,
	PRIMARY KEY (`AddressID` ASC)
);

DROP TABLE IF EXISTS `UserAddress`;
-- All Kept in one table so that we can quickly query the historical data of a user`s addresses --
CREATE TABLE IF NOT EXISTS `UserAddress` (
	`AccountID`		INT			NOT NULL,
	`AddressID`		INT			NOT NULL,
	`IsValid`		BOOLEAN		NOT NULL,
	PRIMARY KEY (`AccountID`, `AddressID`),
		FOREIGN KEY (`AccountID`)
		REFERENCES `Account` (`AccountID`)
		ON DELETE NO ACTION
		ON UPDATE CASCADE,
		FOREIGN KEY (`AddressID`)
		REFERENCES `Address` (`AddressID`)
		ON DELETE NO ACTION
		ON UPDATE CASCADE
);

-- -----------------------------------------------------------------------------------------------------------

DROP TABLE IF EXISTS `Quantity`;
-- Quantities of Bank Notes available to purchase at a vendor --
CREATE TABLE IF NOT EXISTS `Quantity` (
	`CompanyID`		INT			NOT NULL,
    `CurrencyID`	INT			NOT NULL,
    `Quantity`		FLOAT		NOT NULL,
	PRIMARY KEY (`CompanyID`,`CurrencyID`),
		FOREIGN KEY (`CompanyID`)
		REFERENCES `Company` (`CompanyID`)
		ON DELETE NO ACTION
		ON UPDATE CASCADE,
		FOREIGN KEY (`CurrencyID`)
		REFERENCES `Currency` (`CurrencyID`)
		ON DELETE NO ACTION
		ON UPDATE CASCADE
);

-- -----------------------------------------------------------------------------------------------------------

DROP TABLE IF EXISTS `Purchase`;
-- Currently, Base Currency and Trade Currency are only in terms of IDR against something else
CREATE TABLE IF NOT EXISTS `Purchase`(
	`PurchaseID`	INT				NOT NULL	AUTO_INCREMENT,
	`BuyerID`		INT				NOT NULL,
	`BaseCurrency`	INT				NOT NULL,
	`Rate`			FLOAT			NOT NULL,
	`TradeCurrency`	INT				NOT NULL,
	`TotalAmount`	FLOAT			NOT NULL,
    `PurchasedOn`	DATETIME		NOT NULL,
	PRIMARY KEY (`PurchaseID` ASC, `BuyerID`),
		FOREIGN KEY (`BuyerID`)
		REFERENCES `Account` (`AccountID`)
		ON DELETE NO ACTION
		ON UPDATE CASCADE
/*      
		FOREIGN KEY (`BaseCurrency`)
        REFERENCES `Currency` (`CurrencyID`),
        FOREIGN KEY (`TradeCurrency`)
        REFERENCES `Currency` (`CurrencyID`)
*/
);

DROP TABLE IF EXISTS `Sale`;
-- Currently, Base Currency and Trade Currency are only in terms of IDR against something else
CREATE TABLE IF NOT EXISTS `Sale` (
	`SellerID`		INT			NOT NULL,
	`PurchaseID`	INT			NOT NULL,
    `BaseCurrency`	INT			NOT NULL,
    `Rate`			FLOAT		NOT NULL,
    `TradeCurrency`	INT			NOT NULL,
	`Amount`		FLOAT		NOT NULL,
    `PurchasedOn`	DATETIME	NOT NULL,
	PRIMARY KEY (`SellerID`, `PurchaseID`),
		FOREIGN KEY (`SellerID`)
		REFERENCES `Account` (`AccountID`)
		ON DELETE NO ACTION
		ON UPDATE CASCADE,
		FOREIGN KEY (`PurchaseID`)
		REFERENCES `Purchase` (`PurchaseID`)
		ON DELETE NO ACTION
		ON UPDATE CASCADE
/*      
		FOREIGN KEY (`BaseCurrency`)
        REFERENCES `Currency` (`CurrencyID`),
        FOREIGN KEY (`TradeCurrency`)
        REFERENCES `Currency` (`CurrencyID`)
*/
);

/*========================================= CREATE DUMMY DATA ============================================ */


INSERT INTO `Account`(Username,Password,FirstName,LastName,CompanyID)
VALUES
('somebody','once','told','me',1),
('enterprise','enter','Enter','Pryce',2),
('guy1','guyspassword','Guy','Falke',NULL);

INSERT INTO `Company`(CompanyName)
VALUES
('SoundFX'),
('FXU'),
('STFX'),
('NOP');

INSERT INTO `Address` (AddressName,Country,State,City,Street1,Street2,PostCode,Latitude,Longitude)
VALUES
('Test1', 'Indonesia', 'Jakarta', 'Jakarta Barat', 'Jl. Kebon Jeruk 2', NULL, 11530, -6.19257205, 106.769725489693),
('Test2', 'Indonesia', 'Jakarta', 'Jakarta Utara', 'Jl. Melati VIII', NULL, 12122, -6.136197,106.900690224284),
('Test3', 'Indonesia', 'Jakarta', 'Jakarta Barat', 'Jl. Kebon Jeruk RW 07', NULL, 11480, -6.1900119,106.764386),
('Test4', 'Indonesia', 'Jakarta', 'Jakarta Barat', 'Jl. Selatan', NULL, 13340, -6.28381815, 106.804863491948);

INSERT INTO `UserAddress` (AccountID, AddressID, IsValid)
VALUES
(1,1,1),
(2,2,1),
(3,3,1);

INSERT INTO `Currency` (CurrencyCode, CurrencyName)
VALUES
('SGD', 'Singapore Dollars'),
('HKD', 'Hong Kong Dollars'),
('JPY', 'Japanese Yen'),
('CNY', 'Chinese Yuan'),
('USD', 'US Dollars'),
('GBP', 'British Pounds'),
('EUR', 'Euros'),
('KRW', 'South Korean Won'),
('AUD', 'Australian Dollars'),
('THB', 'Thai Baht');

-- values are against 10,000 IDR
INSERT INTO `BuyRates` (CompanyID, CurrencyID, Rate)
VALUES
-- SoundFX
(1,1,0.997856),
(1,2,5.75981),
(1,3,83.7253),
(1,4,4.88469),
(1,5,0.738015),
(1,6,0.551388),
(1,7,0.626896),
(1,8,805.908),
(1,9,0.982310),
(1,10,24.0776),
-- FXU
(2,1,0.999999),
(2,2,5.777777),
(2,3,84.0000),
(2,4,4.88888),
(2,5,0.777777),
(2,6,0.5555555),
(2,7,0.666666),
(2,8,855.908),
(2,9,0.99999),
(2,10,25),
-- STFX
(3,1,0.8),
(3,2,5),
(3,3,80),
(3,4,4),
(3,5,0.5),
(3,6,0.5),
(3,7,0.6),
(3,8,800),
(3,9,0.9),
(3,10,24),
-- NOP
(4,1,0.997856);

-- values are converted to IDR
INSERT INTO `SellRates` (CompanyID, CurrencyID, Rate)
VALUES
-- SoundFX
(1,1,9920.96),
(1,2,1736.17),
(1,3,119.438),
(1,4,2047.21),
(1,5,13549.87),
(1,6,18136.04),
(1,7,15951.62),
(1,8,12.4084),
(1,9,10180.09),
(1,10,415.324),
-- FXU
(2,1,9999.99),
(2,2,1777.77),
(2,3,199.999),
(2,4,2100),
(2,5,13600),
(2,6,18200),
(2,7,16000),
(2,8,13),
(2,9,10200),
(2,10,420),
-- STFX
(3,1,9000),
(3,2,1500),
(3,3,100),
(3,4,2000),
(3,5,12000),
(3,6,17000),
(3,7,15000),
(3,8,11),
(3,9,10000),
(3,10,400),
-- NOP
(4,1,9920.96);

INSERT INTO `Quantity` (CompanyID, CurrencyID, Quantity)
VALUES
-- SoundFX
(1,1,1000),
(1,2,1000),
(1,3,1000),
(1,4,1000),
(1,5,1000),
(1,6,1000),
(1,7,1000),
(1,8,1000),
(1,9,1000),
(1,10,1000),
-- FXU
(2,1,5000),
(2,2,5000),
(2,3,5000),
(2,4,5000),
(2,5,5000),
(2,6,5000),
(2,7,5000),
(2,8,5000),
(2,9,5000),
(2,10,5000),
-- STFX
(3,1,500),
(3,2,500),
(3,3,100),
(3,4,200),
(3,5,300),
(3,6,500),
(3,7,600),
(3,8,100),
(3,9,1000),
(3,10,200),
-- NOP
(4,1,10000);


SELECT * FROM `Account`;
SELECT * FROM `Company`;
SELECT * FROM `Address`;
SELECT * FROM `UserAddress`;

-- WHERE GROUP BY HAVING ORDER BY
-- Find all the addresses associated with an account
SELECT ac.AccountID, Username, FirstName, LastName, CompanyID, AddressName, Street1, PostCode
	FROM Account ac	LEFT JOIN UserAddress ud ON ac.AccountID = ud.AccountID
					LEFT JOIN Address ad ON ad.AddressID = ud.AddressID;

-- Get all sell rates of a specific company
SELECT CompanyName, cc.CurrencyCode, Rate
	FROM Company LEFT JOIN SellRates sr ON Company.CompanyID = sr.CompanyID
				 LEFT JOIN Currency cc ON sr.CurrencyID = cc.CurrencyID;

-- Get all sell rates of a specific currency
SELECT CompanyName, cc.CurrencyCode, Rate
	FROM Company LEFT JOIN SellRates sr ON Company.CompanyID = sr.CompanyID
				 LEFT JOIN Currency cc ON sr.CurrencyID = cc.CurrencyID
                 WHERE cc.CurrencyCode = 'SGD'
                 ORDER BY cc.CurrencyID ASC, Company.CompanyID ASC;

-- Steps for Making a new Purchase
-- 0. User selects a Currency to purchase (and a base currency if needed, set base as IDR for now)
-- 1. User inputs desired quantity to purchase
-- 2. Check Rates from each company (within a radius) and check available quantity against desired quantity
-- 3. Repeat 2. if desired quantity is not met
-- 4. Create the array of purchases with the following parameters (BuyerID, BaseCurrency, Rate, TradeCurrency, TotalAmount)
-- 5. Attempt purchase with paypal (sandbox)
-- 6. If purchase succeeds, update quantities table and record purchase
-- 7. Don't forget to give Sale receipts to the companies/vendors as well
INSERT INTO `Purchase` (BuyerID, BaseCurrency, Rate, TradeCurrency, TotalAmount, PurchasedOn)
VALUES
(3,11,9960.475,1,1000,NOW());
-- The value of Rate here should be AVG(Rate) from the Sale/SellRates table of the purchase

-- Seller Receipt: One for each seller
INSERT INTO `Sale`(SellerID, PurchaseID, BaseCurrency, Rate, TradeCurrency, Amount, PurchasedOn)
VALUES
(1, 1, 11, 9920.9, 1, 500, NOW()),
(2, 1, 11, 9999.99, 1, 500, NOW());

SELECT * FROM `BuyRates`;