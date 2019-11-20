FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["ChinaBank.Api/ChinaBank.Api.csproj", "ChinaBank.Api/"]
RUN dotnet restore "ChinaBank.Api/ChinaBank.Api.csproj"
COPY . .
WORKDIR "/src/ChinaBank.Api"
RUN dotnet build "ChinaBank.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ChinaBank.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ChinaBank.Api.dll"]