function Exec  		
{		
    [CmdletBinding()]		
    param(		
        [Parameter(Position=0,Mandatory=1)][scriptblock]$cmd,		
        [Parameter(Position=1,Mandatory=0)][string]$errorMessage = ($msgs.error_bad_command -f $cmd)		
    )		
    & $cmd		
    if ($lastexitcode -ne 0) {		
        throw ("Exec: " + $errorMessage)		
    }		
}

if(Test-Path .\artifacts) 
{ 
    Remove-Item .\artifacts -Force -Recurse 
}

exec { & dotnet restore }
exec { & dotnet pack .\src\Doulex.DistributedCache -c Release -o .\artifacts }
exec { & dotnet nuget push .\src\Doulex.DistributedCache\artifacts\*.nupkg -k +vFbJEEHK/etwRAa2QrXRWfYQHV0nwS4nfk6oi2RKmdpUgDkERWGJR5NKkK/OI5k -s https://api.nuget.org/v3/index.json }
