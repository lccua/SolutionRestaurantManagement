-- Insert dummy data into ContactInformation table
INSERT INTO ContactInformation (Email, PhoneNumber)
VALUES
('john.doe@example.com', '123456789'),
('jane.smith@example.com', '987654321'),
('sam.jones@example.com', '555123456'),
('gert.doe@admin.com', '123456789'),
('tom.smith@admin.com', '987654321'),
('stijn.jones@admin.com', '555123456');

-- Insert dummy data into Location table
INSERT INTO Location (PostalCode, MunicipalityName, StreetName, HouseNumber)
VALUES
(1234, 'New York', 'Broadway', '123A'),
(5432, 'Los Angeles', 'Hollywood Blvd', '456B'),
(9876, 'Chicago', 'Michigan Ave', '789C');

-- Insert dummy data into Cuisine table
INSERT INTO Cuisine (CuisineType)
VALUES
('Italian'),
('Mexican'),
('Japanese');

-- Insert dummy data into Admin table
INSERT INTO Admin (Name, ContactInfoId)
VALUES
('gert', 4),
('tom', 5),
('stijn', 6);

-- Insert dummy data into Customer table
INSERT INTO Customer (Name, ContactId, LocationId)
VALUES
('john', 1, 1),
('jane', 2, 2),
('sam', 3, 3);

-- Insert dummy data into Restaurant table
INSERT INTO Restaurant (Name, IsActive, CuisineId, LocationId, ContactId)
VALUES
('La Trattoria', 1, 1, 1, 1),
('Taco Fiesta', 1, 2, 2, 2),
('Sakura Sushi', 1, 3, 3, 3);

-- Insert dummy data into [Table] table
INSERT INTO [Table] (Capacity, RestaurantId)
VALUES
(2, 1),
(4, 1),
(6, 1),
(8, 1),
(2, 2),
(4, 2),
(6, 2),
(8, 2),
(2, 3),
(4, 3),
(6, 3),
(8, 3);

-- Insert dummy data into Reservation table
INSERT INTO Reservation (ReservationDate, StartHour, EndHour, AmountOfSeats, CustomerNumber, RestaurantId, TableNumber)
VALUES
('2023-11-25', '18:00','19:30', 2, 1, 1, 1),
('2023-11-26', '19:00','20:30',4, 2, 2, 2),
('2023-11-27', '20:00','21:30', 6, 3, 3, 3);
