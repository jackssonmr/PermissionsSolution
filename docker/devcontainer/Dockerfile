FROM bitnami/dotnet-sdk:latest
#debian:latest

ENV \
    # Do not generate certificate
    DOTNET_GENERATE_ASPNET_CERTIFICATE=false \
    # Do not show first run text
    DOTNET_NOLOGO=true \
    # SDK version
    DOTNET_SDK_VERSION=8.0.203 \
    # Enable correct mode for dotnet watch (only mode supported in a container)
    DOTNET_USE_POLLING_FILE_WATCHER=true \
    # Skip extraction of XML docs - generally not useful within an image/container - helps performance
    NUGET_XMLDOC_MODE=skip \
    # PowerShell telemetry for docker image usage
    POWERSHELL_DISTRIBUTION_CHANNEL=PSDocker-DotnetSDK-Debian-12-arm64

#RUN apt-get update &&  \
#    apt-get install -y --no-install-recommends curl jq bash git xz-utils libatomic1 wget && \
#    rm -rf /var/lib/apt/lists/*
#    
#RUN wget https://packages.microsoft.com/config/debian/12/packages-microsoft-prod.deb -O packages-microsoft-prod.deb \
#    && dpkg -i packages-microsoft-prod.deb \
#    && rm packages-microsoft-prod.deb 
#
#RUN apt-get update && \
#    apt-get install -y dotnet-sdk-8.0 
#
#RUN apt-get update && \
#    apt-get install -y aspnetcore-runtime-8.0 

    
## Install PowerShell global tool
#RUN powershell_version=7.4.1 \
#    && curl -fSL --output PowerShell.Linux.arm64.$powershell_version.nupkg https://powershellinfraartifacts-gkhedzdeaghdezhr.z01.azurefd.net/tool/$powershell_version/PowerShell.Linux.arm64.$powershell_version.nupkg \
#    && powershell_sha512='30235cd50948a1020c0a4d33a8f8bc5ee37ef4246dc3f4dc7c1cf250645352af8d752cf823242e0917b6d3ab2ea307740fdd18600195a696b2343bc9b13005d4' \
#    && echo "$powershell_sha512  PowerShell.Linux.arm64.$powershell_version.nupkg" | sha512sum -c - \
#    && mkdir -p /usr/share/powershell \
#    && dotnet tool install --add-source / --tool-path /usr/share/powershell --version $powershell_version PowerShell.Linux.arm64 \
#    && dotnet nuget locals all --clear \
#    && rm PowerShell.Linux.arm64.$powershell_version.nupkg \
#    && ln -s /usr/share/powershell/pwsh /usr/bin/pwsh \
#    && ln -s /usr/share/powershell/pwsh /usr/bin/ps \
#    && chmod 755 /usr/share/powershell/pwsh \
#    # To reduce image size, remove the copy nupkg that nuget keeps.
#    && find /usr/share/powershell -print | grep -i '.*[.]nupkg$' | xargs rm