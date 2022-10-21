# Doulex.DistributedCache
[![Build status](https://ci.appveyor.com/api/projects/status/21y6ksao6ll7gwgp?svg=true)](https://ci.appveyor.com/project/nepton/doulex-distributedcache)
![GitHub issues](https://img.shields.io/github/issues/nepton/Doulex.DistributedCache.svg)
[![GitHub license](https://img.shields.io/badge/license-MIT-blue.svg)](https://github.com/nepton/Doulex.DistributedCache/blob/master/LICENSE)  

Doulex.DistributedCache is a extension library of Microsoft DistributedCache. It's provide some extensions methods like IOBjectCahce etc.

The library is under .NET Standard 2.0. It's compatible with .NET Core 2.0 and .NET Framework 4.6.1.
DI is supported by Microsoft.Extensions.DependencyInjection.

## Nuget packages
| Name                    | Version                                                                                                                         | Downloads                                                                                                                        |
|-------------------------|---------------------------------------------------------------------------------------------------------------------------------|----------------------------------------------------------------------------------------------------------------------------------|
| Doulex.DistributedCache | [![nuget](https://img.shields.io/nuget/v/Doulex.DistributedCache.svg)](https://www.nuget.org/packages/Doulex.DistributedCache/) | [![stats](https://img.shields.io/nuget/dt/Doulex.DistributedCache.svg)](https://www.nuget.org/packages/Doulex.DistributedCache/) |

## Usage

### Add package
add nuget package
```
Install-Package Doulex.DistributedCache
```

### Basic usage

At your startup.cs, add the following code
```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddDistributedMemoryCache()
    
    // add the extension of distributed cache
    services.AddDistributedCacheExtensions();
}
```

Injection IObjectCache
```csharp
public class HomeController : Controller
{
    private readonly IObjectCache<UserModel> _userModelCache;

    public HomeController(IObjectCache<UserModel> userModelCache)
    {
        _userModelCache = userModelCache;
    }
    
    public IActionResult Index()
    {
        var userModel = _userModelCache.GetAsync("userModel");
        return View(userModel);
    }
}
```

## Final
Leave a comment on GitHub if you have any questions or suggestions.

Turn on the star if you like this project.

## License
This project is licensed under the MIT License
