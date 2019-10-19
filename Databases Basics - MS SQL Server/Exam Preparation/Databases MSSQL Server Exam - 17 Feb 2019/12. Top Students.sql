SELECT TOP(10) s.FirstName,
	   s.LastName,
	   format(AVG(se.Grade), 'N2') AS 'Grade'
FROM Students s
JOIN StudentsExams se
ON se.StudentId = s.Id
GROUP BY s.FirstName,
		 s.LastName
ORDER BY Grade DESC,
		 s.FirstName,
		 s.LastName