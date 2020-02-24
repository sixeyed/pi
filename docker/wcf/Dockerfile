FROM mcr.microsoft.com/dotnet/framework/sdk:4.8-windowsservercore-ltsc2019 AS build

WORKDIR /src
COPY src/Pi.sln .
COPY src/Pi.Math/Pi.Math.csproj ./Pi.Math/
COPY src/Pi.Runtime/Pi.Runtime.csproj ./Pi.Runtime/
COPY src/Pi.Wcf/Pi.Wcf.csproj ./Pi.Wcf/
COPY src/Pi.Wcf/packages.config ./Pi.Wcf/
RUN mkdir 'C:/Program Files/dotnet/sdk/NuGetFallbackFolder'; \
    nuget restore Pi.sln   

COPY src/Pi.Math/ /src/Pi.Math/
COPY src/Pi.Runtime /src/Pi.Runtime/
COPY src/Pi.Wcf /src/Pi.Wcf/
RUN msbuild Pi.Wcf/Pi.Wcf.csproj /t:Restore; \
    msbuild Pi.Wcf/Pi.Wcf.csproj /p:OutputPath=C:/out

# app image
FROM mcr.microsoft.com/dotnet/framework/wcf:4.8
SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

EXPOSE 80 50505

RUN Import-Module WebAdministration; \
    Set-ItemProperty IIS:\AppPools\DefaultAppPool -Name processModel.identityType -Value LocalSystem; 

WORKDIR /inetpub/wwwroot
COPY --from=build /out/_PublishedWebsites/Pi.Wcf .