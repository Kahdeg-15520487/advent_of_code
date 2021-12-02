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

#day 2
#part 1
Push-Location .\day2
Invoke-Expression  $($vmPath + " d2.p1.xil")
Invoke-Expression  $($vmPath + " d2.p2.xil")
Pop-Location