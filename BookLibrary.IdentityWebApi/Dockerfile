FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["BookLibrary.IdentityWebApi/BookLibrary.IdentityWebApi.csproj", "BookLibrary.IdentityWebApi/"]
RUN dotnet restore "BookLibrary.IdentityWebApi/BookLibrary.IdentityWebApi.csproj"
COPY . .
WORKDIR "/src/BookLibrary.IdentityWebApi"
RUN dotnet build "BookLibrary.IdentityWebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BookLibrary.IdentityWebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BookLibrary.IdentityWebApi.dll"]