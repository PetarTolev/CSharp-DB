SELECT s.FirstName,
	   s.LastName,
	   COUNT(t.Id) AS 'TeachersCount'
FROM Students s
JOIN StudentsTeachers st
ON st.StudentId = s.Id
JOIN Teachers t
ON t.Id = st.TeacherId
GROUP BY s.FirstName,
		 s.LastName