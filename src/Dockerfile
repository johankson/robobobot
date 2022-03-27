FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Robobobot.Server/Robobobot.Server.csproj", "Robobobot.Server/"]
RUN dotnet restore "Robobobot.Server/Robobobot.Server.csproj"
COPY . .
WORKDIR "/src/Robobobot.Server"
RUN dotnet build "Robobobot.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Robobobot.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Robobobot.Server.dll"]
