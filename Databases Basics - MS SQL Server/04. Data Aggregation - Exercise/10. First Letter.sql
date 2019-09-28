SELECT DISTINCT SUBSTRING(FirstName, 1, 1) AS [FirstLetter]
FROM WizzardDeposits wd
WHERE wd.DepositGroup = 'Troll Chest'
ORDER BY FirstLetter