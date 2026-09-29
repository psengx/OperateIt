FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["ReceptionService/ReceptionService.csproj", "ReceptionService/"]

RUN dotnet restore "ReceptionService/ReceptionService.csproj"

COPY . .

WORKDIR "/src/ReceptionService"
RUN dotnet publish "ReceptionService.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "ReceptionService.dll"]