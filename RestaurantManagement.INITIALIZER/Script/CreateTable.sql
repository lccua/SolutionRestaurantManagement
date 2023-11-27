-- Create ContactInformation table
CREATE TABLE ContactInformation (
    ContactInformationId INT IDENTITY(1,1) PRIMARY KEY,
    Email VARCHAR(255),
    PhoneNumber VARCHAR(255)
);

-- Create Location table
CREATE TABLE Location (
    LocationId INT IDENTITY(1,1) PRIMARY KEY,
    PostalCode INT,
    MunicipalityName VARCHAR(255) NOT NULL,
    StreetName VARCHAR(255) NULL,
    HouseNumber VARCHAR(255) NULL
);

-- Create Cuisine table
CREATE TABLE Cuisine (
    CuisineId INT IDENTITY(1,1) PRIMARY KEY,
    CuisineType VARCHAR(255)
);

-- Create Admin table
CREATE TABLE Admin (
    AdminId INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(255),
    ContactInfoId INT,
    IsActive INT DEFAULT 1,

    FOREIGN KEY (ContactInfoId) REFERENCES ContactInformation(ContactInformationId)
);

-- Create Customer table
CREATE TABLE Customer (
    CustomerNumber INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(255),
    ContactInformationId INT,
    LocationId INT,
    IsActive INT DEFAULT 1,

    FOREIGN KEY (ContactInformationId) REFERENCES ContactInformation(ContactInformationId),
    FOREIGN KEY (LocationId) REFERENCES Location(LocationId)
);

-- Create Restaurant table
CREATE TABLE Restaurant (
    RestaurantId INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(255),
    IsActive INT DEFAULT 1,

    CuisineId INT,
    LocationId INT,
    ContactInformationId INT,
  
    FOREIGN KEY (CuisineId) REFERENCES Cuisine(CuisineId),
    FOREIGN KEY (LocationId) REFERENCES Location(LocationId),
    FOREIGN KEY (ContactInformationId) REFERENCES ContactInformation(ContactInformationId)
);

-- Create [Table] table
CREATE TABLE [Table] (
    TableNumber INT IDENTITY(1,1) PRIMARY KEY,
    Capacity INT,

    RestaurantId INT,

    FOREIGN KEY (RestaurantId) REFERENCES Restaurant(RestaurantId)
);

-- Create Reservation table
CREATE TABLE Reservation (
    ReservationNumber INT IDENTITY(1,1) PRIMARY KEY,

    ReservationDate DATE,
    StartHour TIME,
    EndHour TIME,
    AmountOfSeats INT,

    CustomerNumber INT,
    RestaurantId INT,
    TableNumber INT,

    FOREIGN KEY (CustomerNumber) REFERENCES Customer(CustomerNumber),
    FOREIGN KEY (RestaurantId) REFERENCES Restaurant(RestaurantId),
    FOREIGN KEY (TableNumber) REFERENCES [Table](TableNumber),

);



