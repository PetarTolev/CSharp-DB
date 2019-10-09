CREATE TABLE DeletedPlanes
(
	Id INT,
	Name VARCHAR(30),
	Seats INT,
	Range INT
)

CREATE TRIGGER tr_DeletedPlanes ON Planes 
AFTER DELETE AS
  INSERT INTO DeletedPlanes (Id, Name, Seats, Range) 
      (SELECT Id, Name, Seats, Range FROM deleted)
