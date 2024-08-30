FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app

# grpc and prometheus
EXPOSE 8080 
EXPOSE 9090

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["tobeh.Valmar.Server.csproj", "tobeh.Valmar.Server/"]
RUN dotnet restore "tobeh.Valmar.Server/tobeh.Valmar.Server.csproj"
COPY . ./tobeh.Valmar.Server
WORKDIR "/src/tobeh.Valmar.Server"
#RUN ls -lR
RUN dotnet build "tobeh.Valmar.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "tobeh.Valmar.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

CMD ["sh", "-c", "dotnet tobeh.Valmar.Server.dll"]
