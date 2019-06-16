FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["BraileCs.Web/BraileCs.Web.csproj", "BraileCs.Web/"]
RUN dotnet restore "BraileCs.Web/BraileCs.Web.csproj"
COPY . .
WORKDIR "/src/BraileCs.Web"
RUN dotnet build "BraileCs.Web.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "BraileCs.Web.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "BraileCs.Web.dll"]