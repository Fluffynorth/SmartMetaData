FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SmartMetaData/SmartMetaData.csproj", "SmartMetaData/"]
RUN dotnet restore "SmartMetaData/SmartMetaData.csproj"
COPY . .
WORKDIR "/src/SmartMetaData"
RUN dotnet build "SmartMetaData.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SmartMetaData.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SmartMetaData.dll"]
