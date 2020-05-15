FROM mcr.microsoft.com/dotnet/core/sdk:3.1.202 as builder

COPY src/Pi.Api/Pi.Api.csproj /src/Pi.Api/
WORKDIR /src/Pi.Api
RUN dotnet restore

COPY src/Pi.Api/ /src/Pi.Api/
RUN dotnet publish -c Release -o /out Pi.Api.csproj

# app image
FROM  mcr.microsoft.com/dotnet/core/aspnet:3.1.4

EXPOSE 80
WORKDIR /app
ENTRYPOINT ["dotnet", "Pi.Api.dll"]

COPY --from=builder /out/ .