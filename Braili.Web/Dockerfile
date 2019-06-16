FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Braili.Web/Braili.Web.fsproj", "Braili.Web/"]
RUN dotnet restore "Braili.Web/Braili.Web.fsproj"
COPY . .
WORKDIR "/src/Braili.Web"
RUN dotnet build "Braili.Web.fsproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Braili.Web.fsproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Braili.Web.dll"]