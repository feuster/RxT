# Build- und Publish-Skript fuer RxT (nutzt das lokale .NET SDK)
. (Join-Path $PSScriptRoot 'env.ps1')
Set-Location $root
& $dotnet publish 'src\RxT.csproj' -c Release -r win-x64 --self-contained false -p:PublishSingleFile=true -p:DebugType=none -p:DebugSymbols=false -o publish
Copy-Item -LiteralPath (Join-Path $root 'README.txt') -Destination (Join-Path $root 'publish') -Force
