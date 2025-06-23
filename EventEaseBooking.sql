--Create Venue Table
CREATE TABLE Venue (
VenueId INT IDENTITY (1,1) PRIMARY KEY,
VenueName NVARCHAR(100) NOT NULL,
Location NVARCHAR (255) NOT NULL,
Capacity INT NOT NULL,
ImageUrl NVARCHAR (255)
);

--p3 step1:
CREATE TABLE EventType(
EventTypeId INT IDENTITY(1,1) PRIMARY KEY,
Name NVARCHAR(100) NOT NULL
);

-- Create Event Table
CREATE TABLE Event (
EventId INT IDENTITY (1,1) PRIMARY KEY,
EventName NVARCHAR(100) NOT NULL,
EventDate DATETIME NOT NULL,
Description NVARCHAR (500),
VenueId INT NOT NULL,
EventTypeId INT NULL, --STEP 2
FOREIGN KEY (VenueId) REFERENCES Venue(VenueId) ON DELETE SET NULL,
FOREIGN KEY (EventTypeId) REFERENCES EventType(EventTypeId) ON DELETE SET NULL, --step 3
);

--Create Booking table
CREATE TABLE Booking(
BookingId INT IDENTITY (1,1) PRIMARY KEY,
EventId INT NOT NULL,
VenueId INT NOT NULL,
BookingDate DATETIME DEFAULT GETDATE(),
FOREIGN KEY (VenueId) REFERENCES Venue(VenueId) ON DELETE CASCADE,
FOREIGN KEY (EventId) REFERENCES Event(EventId) ON DELETE CASCADE,
--no double bookings of same venue for same event
CONSTRAINT UQ_Venue_Event UNIQUE (VenueId, EventId)
);

--ensure no 2 bookings overlap for th same venue
CREATE UNIQUE INDEX UQ_Venue_Booking ON Booking (VenueId, BookingDate);

INSERT INTO Venue (VenueName, Location, Capacity, ImageUrl)
VALUES
('Grand Hall', '123 Main Street, Cityville', 500, 'https://example.com/images/grand_hall.jpg'),
('Lakeside Pavilion', '456 Lakeshore road, Seaside', 200, 'https://example.com/images/lakeside_pavilion.jpg'),
('Riverside Conference', '789 River road, Rivertown', 150, 'https://example.com/images/riverside_conference.jog'),
('The Skyline Venue', '101 Skyline Boulevard, Hilltop', 350, 'https://example.com/images/skyline_venue.jpg'),
('The Green Garden', '202 Garden street, Greenfield', 100, 'https://example.com/images/green_garden.jpg');

-- step 4 seed eventtype table
INSERT INTO EventType (Name)
VALUES
('Conference'),
('Wedding'),
('Naming'),
('Birthday'),
('Concert');

INSERT INTO Event (EventName, EventDate, Description, VenueId, EventTypeId)
VALUES
('Tech Conference 2025', '2025-05-15 09:00:00', 'Annual conference on technology and innovation.', 1,1),
('Wedding Reception - Johnson', '2025-06-20 18:00:00', 'Celebration of the marriage between 2 people', 2,2),
('Business Seminar', '2025-07-10 14:00:00', 'Seminar on business management and strategy', 3,3),
('Music Concert', '2025-08-25 19:00:00', 'Live music concert featuring popular bands', 4,4),
('Garden Party', '2025-08-12 15:00:00', 'Outdoor garden party with refreshments and entertainment', 5,5);

INSERT INTO Booking (EventId, VenueId, BookingDate)
VALUES
(1, 1, '2025-05-01 10:00:00'),
(2, 2, '2025-06-01 12:00:00'),
(3, 3, '2025-07-01 14:00:00'),
(4, 4, '2025-08-01 11:00:00'),
(5, 5, '2025-09-01 09:00:00');

--insert data into event table
SELECT * FROM Venue;
SELECT * FROM Event;
SELECT * FROM Booking;
SELECT * FROM EventType;

