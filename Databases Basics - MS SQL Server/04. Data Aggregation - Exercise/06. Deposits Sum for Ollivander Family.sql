SELECT wd.DepositGroup,
	   SUM(wd.DepositAmount) AS 'TotalSum'
FROM WizzardDeposits AS wd
Where wd.MagicWandCreator = 'Ollivander family'
GROUP BY DepositGroup
