FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["BookLoanSignalRHub/BookLoanSignalRHub.csproj", "BookLoanSignalRHub/"]
RUN dotnet restore "BookLoanSignalRHub/BookLoanSignalRHub.csproj"
COPY . .
WORKDIR "/src/BookLoanSignalRHub"
RUN dotnet build "BookLoanSignalRHub.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "BookLoanSignalRHub.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "BookLoanSignalRHub.dll"]