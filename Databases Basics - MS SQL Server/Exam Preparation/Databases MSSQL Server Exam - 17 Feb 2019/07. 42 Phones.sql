SELECT s.FirstName, 
	   s.[Address], 
	   s.Phone
FROM Students s
WHERE LEFT(s.Phone, 2) = '42' AND
	  s.MiddleName IS NOT NULL
ORDER BY s.FirstName