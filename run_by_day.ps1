#checkout submodule
git pull --recurse-submodules

#build vm
Push-Location .\XIL\testconsole
.\publish.cmd
Pop-Location
$vmPath = "..\XIL\testconsole\bin\publish\xil_cli\testconsole.exe"

#build aoc module
Push-Location .\XIL.AdventOfCode
.\publish.cmd
Pop-Location
Copy-Item .\XIL.AdventOfCode\bin\publish\xil_aoc\XIL.AdventOfCode.dll .\XIL\testconsole\bin\publish\xil_cli

#day 1
Write-Host "day1:"
Push-Location .\day1
dotnet run
Pop-Location
Write-Host ""

#day 2
Write-Host "day2:"
Push-Location .\day2
Invoke-Expression  $($vmPath + " d2.p1.xil")
Invoke-Expression  $($vmPath + " d2.p2.xil")
Pop-Location
Write-Host ""

#day 3
Write-Host "day3:"
Push-Location .\day3
dotnet run
Pop-Location
Write-Host ""

#day 4
Write-Host "day4:"
Push-Location .\day4
dotnet run
Pop-Location
Write-Host ""

#day 5
Write-Host "day5:"
Push-Location .\day5
dotnet run
Pop-Location
Write-Host ""