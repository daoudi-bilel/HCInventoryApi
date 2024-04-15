# IT Inventory Management Backend

Welcome to the IT Inventory Management backend! This application is designed to support a small Angular web application that manages a company's IT inventory, including Employees and Devices. The backend API provides essential CRUD operations for the required entities and additional functionality to manage the linking of devices to employees.

## Table of Contents

- [Overview](#overview)
- [Tech Stack](#tech-stack)
- [Features](#features)
- [API Endpoints](#api-endpoints)
- [Setup and Installation](#setup-and-installation)
- [Database Configuration](#database-configuration)
- [Error Handling and Concurrency](#error-handling-and-concurrency)
- [License](#license)

## Overview

This backend application is a .NET Core API that leverages Entity Framework ORM to manage a SQL database for storing and manipulating data related to Employees and Devices. The API provides a comprehensive set of endpoints for creating, reading, updating, and deleting (CRUD) employees and devices, as well as linking devices to employees. 

## Tech Stack

- **Backend Framework**: .NET Core
- **Database**: SQL Server or any other compatible relational database
- **ORM**: Entity Framework Core
- **Web API**: ASP.NET Core

## Features

- **Employees**: Create, read, update, and delete employees with properties such as name and email.
- **Devices**: Create, read, update, and delete devices with properties such as type and description.
- **Linking**: Manage the linking of devices to employees.
- **Search**: Search for employees or devices based on keyword.
- **Sorting**: Get data Sorted in both directions.

## API Endpoints

Here are some of the main API endpoints provided by the backend:

- **Employees**:
    - `GET /api/v1/employees`: Retrieve a paginated list of employees.
    - `POST /api/v1/employees`: Create a new employee.
    - `GET /api/v1/employees/{id}`: Retrieve a specific employee by ID.
    - `PUT /api/v1/employees/{id}`: Update an existing employee by ID.
    - `DELETE /api/v1/employees/{id}`: Delete an employee by ID.

- **Devices**:
    - `GET /api/v1/devices`: Retrieve a paginated list of devices.
    - `POST /api/v1/devices`: Create a new device.
    - `GET /api/v1/devices/{id}`: Retrieve a specific device by ID.
    - `PUT /api/v1/devices/{id}`: Update an existing device by ID.
    - `DELETE /api/v1/devices/{id}`: Delete a device by ID.

- **Linking**:
    - `PUT /api/v1/devices/{deviceId}/employees/{employeeId}`: Link a device to an employee.

- **Search**:
    - `GET /api/v1/employees?keyword=string`: Search for employees based on name or email.
    - `GET /api/v1/devices?keyword=string`: Search for devices based on type or description.

## Setup and Installation

To set up and run the backend application:

1. **Clone the Repository**: Clone the repository to your local machine.
    ```bash
    git clone [<repository-url>](https://github.com/daoudi-bilel/HCInventoryApi.git)
    ```

2. **Install Dependencies**: Navigate to the project directory and install dependencies.
    ```bash
    cd ITInventoryManagementAPI
    dotnet restore
    ```

3. **Configure the Database**: Update the database connection string in the configuration file (`appsettings.json`) to point to your SQL database.

4. **Apply Migrations**: Apply database migrations to set up the schema.
    ```bash
    dotnet ef database update
    ```

5. **Run the Application**: Start the application.
    ```bash
    dotnet run
    ```

The API will be available at `http://localhost:5268` by default.

## Database Configuration

Ensure your SQL database is properly configured and the connection string in `appsettings.json` is updated accordingly. You can also specify different settings for development and production environments.

## Error Handling and Concurrency

The application uses optimistic concurrency control to handle data modifications and conflicts that may arise from simultaneous data changes. It is important to handle these exceptions appropriately to maintain data integrity.

- **DbUpdateConcurrencyException**: Handle this exception to manage data changes that occur concurrently.
