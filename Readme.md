# Busdriver Rest
## Introduction

Busdriver rest application. With this application you can keep track of drivers that are taking a break. Application will calculate busiest break time.

## Setup

To setup application:

1. Cloning the repository : Git link: https://github.com/toadstylemaster/busdriverbreaks
2. Build solution
3. Run BusdriveBreaks project

### Calculate time from file

#### From solution root directory run
~~~
dotnet build BusdriverBreaks/BusdriverBreaks.csproj
dotnet publish BusdriverBreaks/BusdriverBreaks.csproj -c Release -r win-x64 --self-contained
~~~

#### Navigate to the publish folder and run the executable with the filename switch

~~~
cd BusdriverBreaks/bin/Release/net8.0/win-x64/publish/
.\BusdriverBreaks.exe filename D:\breaks.txt
~~~