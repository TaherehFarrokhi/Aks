FROM microsoft/dotnet:2.2-aspnetcore-runtime-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk-stretch AS build
WORKDIR /src
COPY ["Aks.ServiceProvider/Aks.ServiceProvider.csproj", "Aks.ServiceProvider/"]
RUN dotnet restore "Aks.ServiceProvider/Aks.ServiceProvider.csproj"
COPY . .
WORKDIR "/src/Aks.ServiceProvider"
RUN dotnet build "Aks.ServiceProvider.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Aks.ServiceProvider.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Aks.ServiceProvider.dll"]