FROM mcr.microsoft.com/dotnet/framework/sdk:4.8-windowsservercore-ltsc2019 AS build

WORKDIR /src
COPY src/Pi.sln .
COPY src/Pi.Math/Pi.Math.csproj ./Pi.Math/
COPY src/Pi.Runtime/Pi.Runtime.csproj ./Pi.Runtime/
COPY src/Pi.Runtime.NetFx/Pi.Runtime.NetFx.csproj ./Pi.Runtime.NetFx/
COPY src/Pi.Runtime.NetFx/packages.config ./Pi.Runtime.NetFx/
RUN mkdir 'C:/Program Files/dotnet/sdk/NuGetFallbackFolder'; \
    nuget restore Pi.sln   

COPY src/Pi.Math/ /src/Pi.Math/
COPY src/Pi.Runtime /src/Pi.Runtime/
COPY src/Pi.Runtime.NetFx /src/Pi.Runtime.NetFx/
RUN msbuild Pi.Runtime.NetFx/Pi.Runtime.NetFx.csproj /t:Restore; \
    msbuild Pi.Runtime.NetFx/Pi.Runtime.NetFx.csproj /p:OutputPath=C:/out

# app image
FROM mcr.microsoft.com/dotnet/framework/runtime:4.8

ENTRYPOINT ["/app/Pi.Runtime.NetFx"]
CMD ["-m", "console", "-dp", "6"]

WORKDIR /app
COPY --from=build /out/ .