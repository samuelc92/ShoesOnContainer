FROM microsoft/dotnet AS build-env
WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# copy everything else and build
COPY . ./

RUN dotnet publish -c Release -o out

# build runtime image
FROM microsoft/dotnet
WORKDIR /app
COPY --from=build-env /app/out .
EXPOSE 80/tcp
ENTRYPOINT ["dotnet", "OrderApi.dll"]
