FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.sln ./
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -p:AssemblyVersion=3.2.1 -p:InformationalVersion="lazy dog jumps over the fat brown fox" -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/sdk:3.1
RUN apt-get update \
    && apt-get install -y --allow-unauthenticated \
        libc6-dev \
        libgdiplus \
        libx11-dev \
     && rm -rf /var/lib/apt/lists/*
WORKDIR /app
COPY --from=build-env /app/out .

ENTRYPOINT ["dotnet", "sample_barcode.dll"]