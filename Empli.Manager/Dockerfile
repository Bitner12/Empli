FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["Empli.Manager/Empli.Manager.csproj", "Empli.Manager/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Infrastructures/Infrastructures.csproj", "Infrastructures/"]
COPY ["Shared/Shared.csproj", "Shared/"]

RUN dotnet restore "Empli.Manager/Empli.Manager.csproj"

COPY . .
WORKDIR "/src/Empli.Manager"
RUN dotnet publish "Empli.Manager.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "Empli.Manager.dll"]
