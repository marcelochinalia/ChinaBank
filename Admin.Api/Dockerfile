FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["Admin.Api/Admin.Api.csproj", "Admin.Api/"]
RUN dotnet restore "Admin.Api/Admin.Api.csproj"
COPY . .
WORKDIR "/src/Admin.Api"
RUN dotnet build  "Admin.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Admin.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Admin.Api.dll"]