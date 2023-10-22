# Summary

- The aim of this task is to build a simple API (backed by any kind of database). 
- The application should be able to store weather forecast data in the database, based on provided latitude and longitude 
- you can use https://open-meteo.com/ to get a weather forecast. 
- The API should be able to add (new longitude and latitude), delete or provide weather forecast (get a forecast?). 
- It should also provide an endpoint to list previously used longitudes and latitudes from the database, and choose them to provide the newest weather forecast. 

Application specification

·  It should be a RESTful API

·  You can use https://open-meteo.com  for the weather forecast

·  The application can be built in .net framework 4.8 or .net 7

·  Usage of any free library which will help implement solution is acceptable (e.g. swagger)

·  It is preferable that the API operates using JSON (for both input and output)

·  The solution should also include base specs/tests coverage

# setup sqlite db
```cmd
dotnet tool install --global dotnet-ef
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet ef migrations add InitialCreate
dotnet ef database update
```

# Recreating sqlite db
- See ResetDatabase() function in SqliteWeatherForecastRepository