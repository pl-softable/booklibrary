FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["BookLibrary.LibraryWebApi/BookLibrary.LibraryWebApi.csproj", "BookLibrary.LibraryWebApi/"]
RUN dotnet restore "BookLibrary.LibraryWebApi/BookLibrary.LibraryWebApi.csproj"
COPY . .
WORKDIR "/src/BookLibrary.LibraryWebApi"
RUN dotnet build "BookLibrary.LibraryWebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BookLibrary.LibraryWebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BookLibrary.LibraryWebApi.dll"]