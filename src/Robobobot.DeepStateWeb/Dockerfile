FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Robobobot.DeepStateWeb/Robobobot.DeepStateWeb.csproj", "Robobobot.DeepStateWeb/"]
RUN dotnet restore "Robobobot.DeepStateWeb/Robobobot.DeepStateWeb.csproj"
COPY . .
WORKDIR "/src/Robobobot.DeepStateWeb"
RUN dotnet build "Robobobot.DeepStateWeb.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Robobobot.DeepStateWeb.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Robobobot.DeepStateWeb.dll"]
