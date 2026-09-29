# ---------- Etapa 1: Build ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["SistemaGestionPedidos.csproj", "./"]
RUN dotnet restore "SistemaGestionPedidos.csproj"

COPY . .
RUN dotnet build "SistemaGestionPedidos.csproj" -c Release -o /app/build

# ---------- Etapa 2: Publish ----------
FROM build AS publish
RUN dotnet publish "SistemaGestionPedidos.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ---------- Etapa 3: Runtime ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "SistemaGestionPedidos.dll"]
