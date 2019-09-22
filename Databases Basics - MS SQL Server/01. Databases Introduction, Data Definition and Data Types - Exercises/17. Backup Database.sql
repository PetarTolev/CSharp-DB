BACKUP DATABASE SoftUni TO DISK='D:\Projects\C# DB - September 2019\MS SQL - September 2019\softuni-backup.bak'

   DROP DATABASE SoftUni

RESTORE DATABASE SoftUni FROM DISK='D:\Projects\C# DB - September 2019\MS SQL - September 2019\softuni-backup.bak'