CREATE TABLE Users
(Id             INT IDENTITY, 
 Username       VARCHAR(30) NOT NULL, 
 [Password]     VARCHAR(26) NOT NULL, 
 ProfilePicture VARBINARY, 
 LastLoginTime  DATE, 
 IsDeleted      BIT
 CONSTRAINT PK_Id PRIMARY KEY(Id)
);
INSERT INTO Users
(Username, 
 [Password], 
 ProfilePicture, 
 LastLoginTime, 
 IsDeleted
)
VALUES
('Ivan', 
 '123456', 
 1233, 
 '2019-09-20', 
 0
),
('Gosho', 
 '11223344', 
 900, 
 '2018-01-24', 
 0
),
('Petar', 
 '01928374', 
 550, 
 '2019-06-26', 
 1
),
('Simona', 
 '281764', 
 601, 
 '2019-11-08', 
 0
),
('Stoqn', 
 '142154', 
 200, 
 '2019-07-19', 
 0
);