FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base
WORKDIR /app
EXPOSE 5044
EXPOSE 7117

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "SmartMetaData.sln"
RUN dotnet build "SmartMetaData.sln" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SmartMetaData.sln" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
ENV ASPNETCORE_ENVIRONMENT='Production'
ENV ASPNETCORE_URLS='http://*:5044;https://*:7117'
COPY --from=publish /app/publish .
RUN dotnet dev-certs https
ENTRYPOINT ["dotnet", "SmartMetaData.Host.dll"]
