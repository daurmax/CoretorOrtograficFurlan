# How to run and develop Electron desktop application

## Debug

1. Install [Node.js](https://nodejs.org)
2. Install ElectronNET.CLI globally:
```
dotnet tool install ElectronNET.CLI -g
```
3. Open a terminal (do not use the one provided by Visual Studio) and run
```
electronize start
```
Just put the `/watch` option if you want the autoreload functionality after each save.

## Deploy

Just use one of the following commands with the desired platform:
```
electronize build /target win
electronize build /target osx
electronize build /target linux
```