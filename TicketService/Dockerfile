FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["TicketService/TicketService.csproj", "TicketService/"]
RUN dotnet restore "TicketService/TicketService.csproj"

COPY . .
WORKDIR "/src/TicketService"
RUN dotnet publish "TicketService.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "TicketService.dll"]