SELECT CountryName AS 'Country Name',
IsoCode AS 'ISO Code'
FROM Countries
WHERE len(CountryName) - len(replace(CountryName,'A','')) >= 3
ORDER BY IsoCode