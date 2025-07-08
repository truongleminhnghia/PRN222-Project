# Stage 1: Restore and build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy solution and restore
COPY *.sln .
COPY DrinkToDoor.Data/DrinkToDoor.Data.csproj DrinkToDoor.Data/
COPY WEBAPI/DrinkToDoor.BLL/DrinkToDoor.BLL.csproj DrinkToDoor.BLL/
COPY WEBAPI/DrinToDoor.WebAPI/DrinToDoor.WebAPI.csproj DrinToDoor.WebAPI/

RUN dotnet restore

# Copy the rest of the source
COPY . .

# Build the WebAPI project
RUN dotnet publish DrinToDoor.WebAPI/DrinToDoor.WebAPI.csproj -c Release -o out

# Stage 2: Run
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/out .


# Expose HTTP & HTTPS
EXPOSE 80
EXPOSE 443

ENTRYPOINT ["dotnet", "DrinToDoor.WebAPI.dll"]