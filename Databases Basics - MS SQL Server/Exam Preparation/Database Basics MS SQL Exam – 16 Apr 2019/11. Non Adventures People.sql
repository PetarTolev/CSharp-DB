SELECT p.FirstName, 
	   p.LastName,
	   p.Age
FROM Passengers p
Left JOIN Tickets t
ON p.Id = t.PassengerId
WHERE PassengerId IS NULL
ORDER BY p.Age DESC,
		 p.FirstName,
		 p.LastName