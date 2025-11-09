# Readme

# Employee Management API

This project is a Blazor WebAssembly application with an API backend for managing employees. The backend uses Entity Framework Core for data access.

## Prerequisites

- .NET 6 SDK
- Visual Studio 2022
- SQL Server (or another supported database)

## Setting Up the Database

To set up the database, follow these steps:

### 1. Install Entity Framework Core Tools

Ensure that the Entity Framework Core tools are installed. You can install them using the following command in the Package Manager Console:

### 2. Add Migration

Open the Package Manager Console in Visual Studio (`Tools` > `NuGet Package Manager` > `Package Manager Console`) and run the following command to create a new migration:

### 3. Update Database

Apply the migration to the database using the following command:

Alternatively, you can use the .NET CLI to perform these steps:

1. **Install Entity Framework Core Tools:**

2. **Add Migration:**

Navigate to the project directory and run:

3. **Update Database:**

Apply the migration to the database:

## Running the Application

To run the application, use the following command:

This will start the application and apply any pending migrations to the database.

## Additional Information

- The `EmployeeDbContext` class is configured to use the `EmployeeConfiguration` class to set up the `Employee` entity.
- The `Program.cs` file includes code to apply database migrations at startup.

For more information, refer to the project documentation or the official Entity Framework Core documentation.


###

Run in package manager console: 

Add-Migration InitialCreate
   
Update-Database
   
dotnet tool install --global dotnet-ef

cd .\EmployeeManagement.API\
dotnet ef migrations add InitialCreate
   
dotnet ef database update
   
## API Endpoints

- GET /api/employees
- GET /api/employees/{id}
- POST /api/employees
- PUT /api/employees/{id}
- DELETE /api/employees/{id}

### New: Department statistics with grouping, filtering, and joining

GET /api/employees/stats

Query parameters:

- minSalary (decimal, optional): only include employees with Salary >= minSalary
- search (string, optional): case-insensitive term matched against first/last name, email, or department

Example requests:

- /api/employees/stats
- /api/employees/stats?minSalary=50000
- /api/employees/stats?search=it
- /api/employees/stats?minSalary=60000&search=sales

Response shape:

[
	{
		"department": "IT",
		"employeeCount": 3,
		"averageSalary": 73333.33,
		"totalSalary": 220000.00,
		"maxSalary": 90000.00,
		"topEarnerFirstName": "Alice",
		"topEarnerLastName": "Anderson",
		"topEarnerEmail": "alice@example.com"
	}
]

