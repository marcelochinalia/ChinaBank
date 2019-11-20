FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["Security.Api/Security.Api.csproj", "Security.Api/"]
RUN dotnet restore "Security.Api/Security.Api.csproj"
COPY . .
WORKDIR "/src/Security.Api"
RUN dotnet build "Security.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Security.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Security.Api.dll"]