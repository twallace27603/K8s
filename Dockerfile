FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS build
WORKDIR /src
COPY ["K8s.csproj", "./"]
RUN dotnet restore "K8s.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "K8s.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "K8s.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "K8s.dll"]
