FROM mcr.microsoft.com/dotnet/core/runtime:2.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:2.1 AS build
WORKDIR /src
COPY ["Skanderbro.csproj", "./"]
RUN dotnet restore "./Skanderbro.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Skanderbro.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Skanderbro.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Skanderbro.dll"]
