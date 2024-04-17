FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app

# grpc and prometheus
EXPOSE 8080 
EXPOSE 9090

ARG DB_DOMAIN_NAME="db.domain.tld"
ENV DB_DOMAIN_NAME "${DB_DOMAIN_NAME}"

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Valmar.csproj", "Valmar/"]
RUN dotnet restore "Valmar/Valmar.csproj"
COPY . ./Valmar
WORKDIR "/src/Valmar"
#RUN ls -lR
RUN dotnet build "Valmar.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Valmar.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Copy the entrypoint script
COPY .docker/entrypoint.sh .
USER root
RUN chmod +x entrypoint.sh
USER $APP_UID

ENTRYPOINT ["/app/entrypoint.sh"]
CMD ["sh", "-c", "dotnet Valmar.dll"]
