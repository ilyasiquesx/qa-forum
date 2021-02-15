FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build
WORKDIR /src
COPY ["WebApi/WebApi.csproj", "./"]
COPY ["Application/Application.csproj", "./"]
COPY ["Domain/Domain.csproj", "./"]
COPY ["Infrastructure/Infrastructure.csproj", "./"]
RUN dotnet restore "WebApi.csproj"
COPY . .
RUN dotnet publish "WebApi/WebApi.csproj" -c Release -o /app/publish --no-cache

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "WebApi.dll"]
