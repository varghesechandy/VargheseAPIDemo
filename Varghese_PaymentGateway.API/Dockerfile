#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app 

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Varghese_PaymentGateway.API/Varghese_PaymentGateway.API.csproj", "Varghese_PaymentGateway.API/"]
RUN dotnet restore "Varghese_PaymentGateway.API/Varghese_PaymentGateway.API.csproj"
COPY . . 
WORKDIR "/src/Varghese_PaymentGateway.API"
RUN dotnet build "Varghese_PaymentGateway.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Varghese_PaymentGateway.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 12121
ENTRYPOINT ["dotnet", "Varghese_PaymentGateway.API.dll"]