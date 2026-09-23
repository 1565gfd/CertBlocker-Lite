# Сборка CertBlockerLite.exe компилятором .NET Framework (csc.exe).
# Запуск:  powershell -ExecutionPolicy Bypass -File .\build.ps1
$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent $MyInvocation.MyCommand.Path
$csc = Join-Path $env:WINDIR 'Microsoft.NET\Framework64\v4.0.30319\csc.exe'
if (-not (Test-Path $csc)) { $csc = Join-Path $env:WINDIR 'Microsoft.NET\Framework\v4.0.30319\csc.exe' }
if (-not (Test-Path $csc)) { throw 'Не найден csc.exe (.NET Framework 4.x).' }
$outDir = Join-Path $root 'build'
New-Item -ItemType Directory -Force -Path $outDir | Out-Null
$out = Join-Path $outDir 'CertBlockerLite.exe'
& $csc /nologo /optimize+ /target:winexe "/out:$out" "/win32manifest:$root\src\app.manifest" `
    /reference:System.dll /reference:System.Drawing.dll `
    /reference:System.Windows.Forms.dll /reference:System.Security.dll `
    "$root\src\Program.cs" "$root\src\AssemblyInfo.cs"
if ($LASTEXITCODE -eq 0) { Write-Host "Готово: $out" -ForegroundColor Green }
else { throw "Сборка завершилась с ошибкой ($LASTEXITCODE)." }
