FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine-arm64v8 AS publish
ARG BUILDCONFIG=Release
WORKDIR /src
COPY . .
RUN dotnet publish "SmartMetaData.sln" --runtime alpine-arm64 --self-contained true -c $BUILDCONFIG -p:PublishSingleFile=true -p:PublishTrimmed=true -o /app/publish

FROM mcr.microsoft.com/dotnet/runtime-deps:6.0-alpine-arm64v8 AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 80
ENTRYPOINT [ "./SmartMetaData"]
