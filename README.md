# Dotnet Web API Template Installation and Usage Guide

This guide provides instructions on how to clone, install, and use the Dotnet Web API Template, including how to run the application and its associated tests.

## Prerequisites

Before you begin, ensure you have the following installed:
- Git
- .NET 8.0 SDK or later

## Cloning the Template

1. Open a terminal.
2. Clone the repository using the following command:
   
    ```bash
    https://github.com/Willy-Kind/dotnet-webapi-template.git
    ```
   
# Dotnet Web API Template

[![.NET](https://github.com/Willy-Kind/dotnet-api-template/actions/workflows/dotnet.yml/badge.svg)](https://github.com/Willy-Kind/dotnet-api-template/actions/workflows/dotnet.yml)

This is a template for building a .NET Web API project.

# Dotnet Web API Template Installation and Usage Guide

This guide provides instructions on how to clone, install, and use the Dotnet Web API Template, including how to run the application and its associated tests.

## Prerequisites

Before you begin, ensure you have the following installed:
- Git
- .NET 8.0 SDK or later

## Cloning the Template

1. Open a terminal.
2. Clone the repository using the following command:
   
    ```bash
    https://github.com/Willy-Kind/dotnet-webapi-template.git
    ```
   
# Getting Started

## Installing the Template

After cloning the repository, you need to install the template into your .NET CLI. This allows you to create new projects based on this template.

1. Install the template:

    ```bash
    dotnet new install dotnet-webapi-template .
    ```

    This command tells the .NET CLI to install the template located in the current directory.

## Creating a New Project

Once the template is installed, you can create a new project based on it.

(Only WebApi)
1. Create a new project using the template :

    ```bash
    dotnet new webapi-template -n YourProjectName
    ```

    Replace `YourProjectName` with the desired name for your new project.

2. Navigate to your new project directory:

    ```bash
    cd YourProjectName
    ```

(With Tests and .files)
1. Create a new project using the template :

    ```bash
    dotnet new webapi-template -n YourProjectName --generateTests true
    ```

    Replace `YourProjectName` with the desired name for your new project.

2. Navigate to your new project directory:

    ```bash
    cd YourProjectName
    ```

## Running the Application

To run your newly created Web API application, follow these steps:

1. Restore the project dependencies:

    ```bash
    dotnet restore
    ```

2. Build the project:

    ```bash
    dotnet build
    ```

3. Run the application:

    ```bash
    dotnet run
    ```

4. Open your web browser and navigate to `http://localhost:5000/swagger` (or the URL provided in the console) to access the API's Swagger UI.

## Running the Tests

If your project includes tests, you can run them using the following command:

```bash
dotnet test
```
