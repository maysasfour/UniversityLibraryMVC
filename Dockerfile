FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY UniversityLibraryMVC.csproj ./
RUN dotnet restore UniversityLibraryMVC.csproj
COPY . .
RUN dotnet publish UniversityLibraryMVC.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
RUN mkdir -p /app/data
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000
ENTRYPOINT ["dotnet", "UniversityLibraryMVC.dll"]
