FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["BookLibrary.GatewayWebApi/BookLibrary.GatewayWebApi.csproj", "BookLibrary.GatewayWebApi/"]
RUN dotnet restore "BookLibrary.GatewayWebApi/BookLibrary.GatewayWebApi.csproj"
COPY . .
WORKDIR "/src/BookLibrary.GatewayWebApi"
RUN dotnet build "BookLibrary.GatewayWebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BookLibrary.GatewayWebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BookLibrary.GatewayWebApi.dll"]