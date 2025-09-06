# Imagem base para runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Imagem base para build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar csproj
COPY ["src/ContaCorrente/BankMore.ContaCorrente.Api/BankMore.ContaCorrente.Api.csproj", "BankMore.ContaCorrente.Api/"]
COPY ["src/ContaCorrente/BankMore.ContaCorrente.Application/BankMore.ContaCorrente.Application.csproj", "BankMore.ContaCorrente.Application/"]
COPY ["src/ContaCorrente/BankMore.ContaCorrente.Domain/BankMore.ContaCorrente.Domain.csproj", "BankMore.ContaCorrente.Domain/"]
COPY ["src/ContaCorrente/BankMore.ContaCorrente.Infrastructure/BankMore.ContaCorrente.Infrastructure.csproj", "BankMore.ContaCorrente.Infrastructure/"]


RUN dotnet restore "BankMore.ContaCorrente.Api/BankMore.ContaCorrente.Api.csproj"

# Copiar todo o restante do código
COPY . .

WORKDIR "/src/BankMore.ContaCorrente.Api"

# Build da aplicação
RUN dotnet build "BankMore.ContaCorrente.Api.csproj" -c Release -o /app/build

# Publicação da aplicação
FROM build AS publish
RUN dotnet publish "BankMore.ContaCorrente.Api.csproj" -c Release -o /app/publish

# Imagem final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BankMore.ContaCorrente.Api.dll"]
