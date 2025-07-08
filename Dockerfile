# ──────────────── Stage 1: Build & Publish ────────────────
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 1) Chỉ copy các .csproj cần thiết
COPY DrinkToDoor.Data/DrinkToDoor.Data.csproj        DrinkToDoor.Data/
COPY WEBAPI/DrinkToDoor.BLL/DrinkToDoor.BLL.csproj    DrinkToDoor.BLL/
COPY WEBAPI/DrinToDoor.WebAPI/DrinToDoor.WebAPI.csproj DrinToDoor.WebAPI/

# 2) Restore cho WebAPI (sẽ kéo theo 2 project kia qua project refs)
RUN dotnet restore DrinToDoor.WebAPI/DrinToDoor.WebAPI.csproj

# 3) Copy code của 3 project
COPY DrinkToDoor.Data/        DrinkToDoor.Data/
COPY WEBAPI/DrinkToDoor.BLL/  DrinkToDoor.BLL/
COPY WEBAPI/DrinToDoor.WebAPI/ DrinToDoor.WebAPI/

# 4) Publish WebAPI
WORKDIR /src/DrinToDoor.WebAPI
RUN dotnet publish -c Release -o /app/out --no-restore

# ──────────────── Stage 2: Runtime ────────────────
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# 5) Chép output từ build stage
COPY --from=build /app/out .

# 6) Môi trường & cổng
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80
EXPOSE 443

# 7) Khởi chạy WebAPI
ENTRYPOINT ["dotnet", "DrinToDoor.WebAPI.dll"]
