FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app

# -------------- Step 1. Copy only required files for nuget restore for better caching
# 1.1 Copy solution & nuget config file (if exists)
COPY *.sln NuGet*.Config ./
# 1.2 Copy csproj and create folders for projects
COPY ./src/*/*.csproj ./
# 1.3 Create folders for projects
RUN for file in $(ls *.csproj); do mkdir -p src/${file%.*} && mv $file src/${file%.*}/; done
# 1.4 Copy tests
COPY ./test/*/*.csproj ./
# 1.5 Create projects for tests
RUN for file in $(ls *.csproj); do mkdir -p test/${file%.*} && mv $file test/${file%.*}/; done
# 1.6 Restore
RUN dotnet restore

# -------------- Step 2. Copy all other files
COPY . .

# -------------- Step 3. Build all
RUN dotnet build -c Release --no-restore

# TODO Support flag test=false
# TODO Export test xml results  files
# -------------- Step 4. Run tests
RUN dotnet vstest test/*/bin/Release/*/*Tests.dll

#-------------- Step 5. Build runtime image
RUN dotnet publish src/App -c Release -o /app/out --no-restore

FROM mcr.microsoft.com/dotnet/runtime:5.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "App.dll"]
