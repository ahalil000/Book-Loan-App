#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["BookLoan.Loan.API/BookLoan.Loan.API.csproj", "BookLoan.Loan.API/"]
RUN dotnet restore "BookLoan.Loan.API/BookLoan.Loan.API.csproj"
COPY . .
WORKDIR "/src/BookLoan.Loan.API"
RUN dotnet build "BookLoan.Loan.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BookLoan.Loan.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BookLoan.Loan.API.dll"]