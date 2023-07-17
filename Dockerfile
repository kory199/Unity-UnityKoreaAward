FROM mcr.microsoft.com/dotnet/sdk:7.0 AS base
ENV ASPNETCORE_URLS=http://+:44376
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["APIServer/APIServer.csproj", "APIServer/"]
RUN dotnet restore "APIServer/APIServer.csproj"
COPY . .
WORKDIR "/src/APIServer"
RUN dotnet build "APIServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "APIServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "APIServer.dll"]
