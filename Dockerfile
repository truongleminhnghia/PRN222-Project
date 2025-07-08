# ──────────────── Stage 1: Restore & Publish ────────────────
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# 1) Copy chỉ các .csproj (giữ nguyên thư mục WEBAPI)
COPY DrinkToDoor.Data/DrinkToDoor.Data.csproj          DrinkToDoor.Data/
COPY WEBAPI/DrinkToDoor.BLL/DrinkToDoor.BLL.csproj      WEBAPI/DrinkToDoor.BLL/
COPY WEBAPI/DrinToDoor.WebAPI/DrinToDoor.WebAPI.csproj  WEBAPI/DrinToDoor.WebAPI/

# 2) Restore WebAPI (sẽ kéo theo BLL → Data qua ProjectReference)
RUN dotnet restore WEBAPI/DrinToDoor.WebAPI/DrinToDoor.WebAPI.csproj

# 3) Copy toàn bộ source cho 3 project
COPY DrinkToDoor.Data/          DrinkToDoor.Data/
COPY WEBAPI/DrinkToDoor.BLL/    WEBAPI/DrinkToDoor.BLL/
COPY WEBAPI/DrinToDoor.WebAPI/  WEBAPI/DrinToDoor.WebAPI/

# 4) Publish WebAPI
WORKDIR /app/WEBAPI/DrinToDoor.WebAPI
RUN dotnet publish -c Release -o /app/out --no-restore

# ──────────────── Stage 2: Runtime ────────────────
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# 5) Copy kết quả publish
COPY --from=build /app/out .

# 6) Môi trường & cổng
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80
EXPOSE 443

# 7) Khởi động
ENTRYPOINT ["dotnet", "DrinToDoor.WebAPI.dll"]
