# Use the official ASP.NET runtime as a base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Use the official .NET SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the solution file and the entire project structure
COPY . .

# Restore dependencies for all projects within the solution
RUN dotnet restore "ARLeF.CoretorOrtografic.sln"

# Build and publish the solution in Release mode
RUN dotnet publish "ARLeF.CoretorOrtografic.sln" -c Release -o /app/publish

# Final stage: use the ASP.NET runtime image to run the app
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ARLeF.CoretorOrtografic.API.dll"]  # Update this with the actual main project DLL name