## BookStore API

## Description
BookStore API is a web application that provides functionality to manage books and authors, as well as the ability to perform CRUD (Create, Read, Update, and Delete) operations. The API is secured through JWT authentication and supports pagination, search, and caching to optimize performance.

## Project Structure
The solution is composed of several projects to maintain a clear separation between the different layers of the application:

- **BookStore.API**: Contains the API drivers and configuration.
- **BookStore.Business**: Implements the business logic, including the services for managing authors and books.
- **BookStore.Data**: Contains the data access, including repositories and interfaces that interact with the database.
- **BookStore.Tests**: Contains the unit tests for the services in the business layer.

Each layer communicates through interfaces, allowing separation of concerns and facilitating testing of each component in isolation.

## Instructions for Running the Application
1. **Clone the repository:**.
   ```bash
   git clone https://github.com/tu-usuario/bookstore-api.git

2. **Navigate to the project directory:**
	````bash
	cd bookstore-api

3. **Restore NuGet packages:**
	````bash
	dotnet restore
 
4. **Configure the database:**
  Make sure you have an instance of SQL Server running. Then, update the connection string in appsettings.json in the BookStore.API project according to your configuration.

5. **Configure JwtSettings:**
   In the appsettings.json file of the BookStore.API project, add the following settings for JWT:
   ````json
   "JwtSettings": {
    "Issuer": "BookStoreAPI",
    "Audience": "BookStoreClient",
    "Key": "!zDSFG9c8D7xSGc9789887AxG890!",
    "DurationInMinutes": 60
    }


7. **Apply the database migrations:**
	````bash
	dotnet ef database update --project BookStore.Data

8. **Compile and run the application:**
	````bash
	dotnet run --project BookStore.API

 ## Design Decisions
- **Separation of layers**: The application follows a multi-tier architecture to ensure that the business logic is separated from the data access and presentation layer (API).
- **Use of Entity Framework Core**: EF Core was chosen to handle database operations because of its seamless integration with .NET and support for migrations.
- **Pagination and filtering**: To optimize queries, pagination and search were implemented in the author and book endpoints, which improves performance and user experience.
- **JWT authentication**: API security is handled with JWT, ensuring that only authenticated users can access protected resources.
- **Caching**: To improve performance, caching was implemented on read endpoints.

## Challenges and Approach
Unit testing the services with Moq presented some technical challenges due to the asynchronous nature of operations in EF Core. Additionally, I faced difficulties in deploying to Azure as it was my first experience with this technology.
