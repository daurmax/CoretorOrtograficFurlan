# Use the official .NET SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy only the solution file and API project files required for building
COPY ["ARLeF.CoretorOrtografic.sln", "./"]
COPY ["Libraries/ARLeF.CoretorOrtografic.Core/ARLeF.CoretorOrtografic.Core.csproj", "Libraries/ARLeF.CoretorOrtografic.Core/"]
COPY ["Libraries/ARLeF.CoretorOrtografic.Dictionaries/ARLeF.CoretorOrtografic.Dictionaries.csproj", "Libraries/ARLeF.CoretorOrtografic.Dictionaries/"]
COPY ["Libraries/ARLeF.CoretorOrtografic.Infrastructure/ARLeF.CoretorOrtografic.Infrastructure.csproj", "Libraries/ARLeF.CoretorOrtografic.Infrastructure/"]
COPY ["API/ARLeF.CoretorOrtografic.API/ARLeF.CoretorOrtografic.API.csproj", "API/ARLeF.CoretorOrtografic.API/"]

# Restore dependencies for the specified projects
RUN dotnet restore "API/ARLeF.CoretorOrtografic.API/ARLeF.CoretorOrtografic.API.csproj"

# Copy the entire solution to ensure all source files are available
COPY . .

# Build and publish the API service only
RUN dotnet publish "API/ARLeF.CoretorOrtografic.API/ARLeF.CoretorOrtografic.API.csproj" -c Release -o /app/publish

# Final stage: use the ASP.NET runtime image to run the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Copy the published files from the build stage
COPY --from=build /app/publish .

# Run the application
ENTRYPOINT ["dotnet", "ARLeF.CoretorOrtografic.API.dll"]
