INSERT INTO Clients (FirstName, LastName) VALUES
('Gosho', 'Ivanov'),
('Pesho', 'Petrov'),
('Ivan', 'Iliev'),
('Merry', 'Ivanova')

INSERT INTO AccountType (Name) VALUES
('Checking'),
('Saving')

INSERT INTO Accounts (ClientId, AccountTypeId, Balance) VALUES
(1, 1, 175),
(2, 1, 275.56),
(3, 1, 40.30),
(4, 1, 40.30),
(4, 2, 375.50)
