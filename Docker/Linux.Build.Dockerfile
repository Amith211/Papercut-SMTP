FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine as build

COPY . /Papercut
WORKDIR /Papercut

RUN dotnet publish src/Papercut.Service -c Release -o /release/Papercut.Service

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine
COPY --from=build /release/Papercut.Service /Papercut

WORKDIR /Papercut

EXPOSE 25 37408
ENTRYPOINT ["dotnet", "./Papercut.Service.dll"]
