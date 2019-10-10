# Code Camps
ASP.NET Core 2.2 Application with Visual Studio 2017

# What does this application do?
This application was created for learning purposes. It is a platform for people to create Coding Camps and set its talks and speakers

# To run the application, follow the steps

1. Rebuild your project.
2. Navigate to Tools -> NuGet Package Manager -> Package Manager Console.
3. Go to your Solution Explorer, and search for a folder named **Migration**
4. If you did not find the folder from step #3, run the commands:
```
      PM> Add-Migration Initial_Migration
      PM> update-database
```
5. If you found the folder from step #3, run the command:\
```
      PM> update-database
```
6. Rebuild your project again and run it.

# To make sure that the database was created

1. Go to View, click *SQL Server Object Explorer*
2. Under *SQL Server*, find your local database **PSCodeCamp**
3. Check if the tables are created

# Testing

Please, use Postmen to test the API requests. I have created a JSON file for all of the API requests are inside **Postmen Requests** folder. Simply, import the JSON file into your Postmen application and start testing :)
