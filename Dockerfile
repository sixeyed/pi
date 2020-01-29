FROM mcr.microsoft.com/dotnet/core/sdk:3.1.101 AS builder

WORKDIR /src
COPY src/Pi.Web.csproj .
RUN dotnet restore

COPY src/ .
RUN dotnet publish -c Release -o /out Pi.Web.csproj

# app image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1.1

EXPOSE 80
ENTRYPOINT ["dotnet", "Pi.Web.dll"]
CMD ["-m", "console", "-dp", "6"]

WORKDIR /app
COPY --from=builder /out/ .