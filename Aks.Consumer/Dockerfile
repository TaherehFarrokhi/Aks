FROM microsoft/dotnet:2.2-aspnetcore-runtime-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk-stretch AS build
WORKDIR /src
COPY ["Aks.Consumer/Aks.Consumer.csproj", "Aks.Consumer/"]
COPY ["Aks.SeviceClient/Aks.ServiceClient.csproj", "Aks.SeviceClient/"]
RUN dotnet restore "Aks.Consumer/Aks.Consumer.csproj"
COPY . .
WORKDIR "/src/Aks.Consumer"
RUN dotnet build "Aks.Consumer.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Aks.Consumer.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Aks.Consumer.dll"]