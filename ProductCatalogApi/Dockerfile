FROM microsoft/aspnetcore-build:latest AS build

WORKDIR /code

COPY . .

RUN dotnet restore

RUN dotnet publish --output /out/ --configuration Release

FROM microsoft/aspnetcore:latest

COPY --from=build /out /app/

WORKDIR /app

ENTRYPOINT ["dotnet", "ProductCatalogApi.dll"]

