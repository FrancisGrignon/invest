#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Invest.MVC.csproj", "."]
RUN dotnet restore "./Invest.MVC.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Invest.MVC.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Invest.MVC.csproj" -c Release -o /app/publish

FROM base AS final

# Hardening
RUN addgroup --group friendlygroupname \
    --gid 2000 \
 && adduser \    
    --uid 1000 \
    --gid 2000 \
    --disabled-password \
    --home /app \
    --gecos '' \
    friendlyusername  \
 && chown friendlyusername:friendlygroupname /app

USER friendlyusername:friendlygroupname 

WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Invest.MVC.dll"]