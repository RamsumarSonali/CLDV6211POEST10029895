--Create Venue Table
CREATE TABLE Venue (
VenueId INT IDENTITY (1,1) PRIMARY KEY,
VenueName NVARCHAR(100) NOT NULL,
Location NVARCHAR (255) NOT NULL,
Capacity INT NOT NULL,
ImageUrl NVARCHAR (255)
);

-- Create Event Table
CREATE TABLE Event (
EventId INT IDENTITY (1,1) PRIMARY KEY,
EventName NVARCHAR(100) NOT NULL,
EventDate DATETIME NOT NULL,
Description NVARCHAR (500),
VenueID INT NOT NULL,
CONSTRAINT FK_Event_Venue FOREIGN KEY (VenueId) REFERENCES Venue(VenueId)
);

--Create Booking table
CREATE TABLE Booking(
BookingId INT IDENTITY (1,1) PRIMARY KEY,
EventId INT NOT NULL,
VenueId INT NOT NULL,
BookingDate DATETIME NOT NULL,
CONSTRAINT FK_Booking_Venue FOREIGN KEY (VenueId) REFERENCES Venue(VenueId),
CONSTRAINT FK_Booking_Event FOREIGN KEY (EventId) REFERENCES Event(EventId),
--no double bookings
CONSTRAINT UQ_Venue_Booking UNIQUE (VenueId, BookingDate)
);

SELECT * FROM Venue;
SELECT * FROM Event;
SELECT * FROM Booking;

