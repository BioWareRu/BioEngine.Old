FROM microsoft/aspnetcore-build:2 AS builder
WORKDIR /source

COPY *.csproj .
RUN dotnet restore

COPY . .
RUN dotnet publish --output /app/ --configuration Release

FROM microsoft/aspnetcore:2

WORKDIR /app
COPY --from=builder /app .

CMD ["dotnet", "BioEngine.API.dll"]