#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["samples/IdentityService.Host/IdentityService.Host.csproj", "samples/IdentityService.Host/"]
RUN dotnet restore "samples/IdentityService.Host/IdentityService.Host.csproj"
COPY . .
WORKDIR "/src/samples/IdentityService.Host"
RUN dotnet build "IdentityService.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IdentityService.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IdentityService.Host.dll"]