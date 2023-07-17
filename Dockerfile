# 이미지의 베이스를 설정합니다. dotnet sdk를 사용합니다.
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app

# 프로젝트 파일을 복사합니다.
COPY *.csproj ./
RUN dotnet restore

# 소스 코드를 복사합니다.
COPY . ./
RUN dotnet publish -c Release -o out

# 런타임 이미지를 빌드합니다.
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "APIServer.dll"]
