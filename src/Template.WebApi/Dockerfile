# Use the .NET SDK image as the base image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory inside the container
WORKDIR /app

# Copy the project file and restore dependencies
COPY src/Template.WebApi/*.csproj ./
RUN dotnet restore

# Copy the remaining source code
COPY src/Template.WebApi/ ./

# Build the application
RUN dotnet build 

# Publish the application
RUN dotnet publish -c Release --no-restore -o out

# Use the .NET runtime image as the base image for the final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Set the working directory inside the container
WORKDIR /app

# ENV DOTNET_ENVIRONMENT=Development

# Copy the published output from the build stage
COPY --from=build /app/out ./

# Expose the port that the application listens on
EXPOSE 5000

# Set the entry point for the container
ENTRYPOINT ["dotnet", "Template.WebApi.dll"]