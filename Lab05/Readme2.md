# Employee Management

## Description
A Blazor WebAssembly application with an API backend for managing employees. The backend uses Entity Framework Core for data access.

## Installation
1. **Prerequisites:**
    - .NET 6 SDK
    - Visual Studio 2022
    - SQL Server (or another supported database)

2. **Install Entity Framework Core tools:**
    ```sh
    dotnet tool install --global dotnet-ef
    ```

3. **Create initial database migration:**
    ```sh
    dotnet ef migrations add InitialCreate
    ```

4. **Apply migrations to the database:**
    ```sh
    dotnet ef database update
    ```

5. **Run in package manager console:**
    ```sh
    Add-Migration InitialCreate
    Update-Database
    dotnet tool install --global dotnet-ef
    dotnet ef migrations add InitialCreate
    dotnet ef database update
    ```

## Usage
Usage details: To be filled later.

## Debugging Instructions
To debug the Employee Management application, both the API and the Web projects need to be started. Follow these steps:

1. **Open the Solution:**
   Open the `EmployeeManagement.sln` solution in Visual Studio.

2. **Set Multiple Startup Projects:**
   - Right-click on the solution in the Solution Explorer and select `Properties`.
   - In the `Common Properties` section, select `Startup Project`.
   - Choose the `Multiple startup projects` option.
   - Set the `Action` for both `EmployeeManagement.API` and `EmployeeManagement.Web` to `Start`.

3. **Start Debugging:**
   - Press `F5` or click the `Start` button in Visual Studio to start debugging both projects.
   - The API will run on `https://localhost:53399` and the Web project will run on `https://localhost:53401`.

4. **Check Logs:**
   - The API project uses ASP.NET Core logging. Check the output in the Visual Studio Output window or configure logging in `appsettings.json` to write logs to a file.
   - The Web project can be debugged using browser developer tools.

5. **Apply Migrations:**
   Ensure that the database migrations are applied before running the application. You can do this by running the following commands in the Package Manager Console:
   ```sh
   Add-Migration InitialCreate
   Update-Database

## Infrastructure and Deployment
Infrastructure and deployment information: To be filled later.

## Results and Data Access
Results and data access: To be filled later.

## API Documentation
### Endpoints
- **GET /api/employees**
  - Description: Retrieves all employees.
  - Response: `200 OK` with a list of employees.

- **GET /api/employees/{id}**
  - Description: Retrieves an employee by ID.
  - Response: `200 OK` with the employee details or `404 Not Found` if the employee does not exist.

- **POST /api/employees**
  - Description: Creates a new employee.
  - Request Body: Employee object. Example: `{ "FirstName": "Jane", "LastName": "Smith", "Email": "jane.smith@example.com", "Department": "HR", "DateOfBirth": "1992-05-15T00:00:00", "Salary": 60000 }`
  - Response: `201 Created` with the created employee details or `400 Bad Request` if the request is invalid.

- **PUT /api/employees/{id}**
  - Description: Updates an existing employee.
  - Request Body: Employee object with updated details. Example: `{ "Id": 1, "FirstName": "John", "LastName": "Doe", "Email": "john.doe@example.com", "Department": "IT", "DateOfBirth": "1985-10-20T00:00:00", "Salary": 75000 }`
  - Response: `204 No Content` or `400 Bad Request` if the request is invalid.

- **DELETE /api/employees/{id}**
  - Description: Deletes an employee by ID.
  - Response: `204 No Content` or `404 Not Found` if the employee does not exist.

## Contributing
Contributing information: To be filled later.

## License
License: To be filled later.