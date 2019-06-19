echo "Build nuget packages"



dotnet pack "P:\Dev\Gitlab\PH.Core3\PH.Core3\PH.Core3.AspNetCoreApi\PH.Core3.AspNetCoreApi.csproj" -c Release --include-source  -o "P:\Dev\Gitlab\PH.Core3\PH.Core3\BuildPackages"
rem dotnet pack "P:\Dev\Gitlab\PH.Core3\PH.Core3\PH.Core3.AspNetCoreApi\PH.Core3.AspNetCoreApi.csproj" -c Release --include-source --include-symbols -o "P:\Dev\Gitlab\PH.Core3\PH.Core3\BuildPackages"


dotnet pack "P:\Dev\Gitlab\PH.Core3\PH.Core3\PH.Core3.Common\PH.Core3.Common.csproj" -c Release --include-source -o "P:\Dev\Gitlab\PH.Core3\PH.Core3\BuildPackages"
rem dotnet pack "P:\Dev\Gitlab\PH.Core3\PH.Core3\PH.Core3.Common\PH.Core3.Common.csproj" -c Release --include-source --include-symbols -o "P:\Dev\Gitlab\PH.Core3\PH.Core3\BuildPackages"

dotnet pack "P:\Dev\Gitlab\PH.Core3\PH.Core3\PH.Core3.Common.Services\PH.Core3.Common.Services.csproj" -c Release --include-source -o "P:\Dev\Gitlab\PH.Core3\PH.Core3\BuildPackages"
rem dotnet pack "P:\Dev\Gitlab\PH.Core3\PH.Core3\PH.Core3.Common.Services\PH.Core3.Common.Services.csproj" -c Release --include-source --include-symbols -o "P:\Dev\Gitlab\PH.Core3\PH.Core3\BuildPackages"

dotnet pack "P:\Dev\Gitlab\PH.Core3\PH.Core3\PH.Core3.Common.Services.Components\PH.Core3.Common.Services.Components.csproj" -c Release --include-source -o "P:\Dev\Gitlab\PH.Core3\PH.Core3\BuildPackages"
rem dotnet pack "P:\Dev\Gitlab\PH.Core3\PH.Core3\PH.Core3.Common.Services.Components\PH.Core3.Common.Services.Components.csproj" -c Release --include-source --include-symbols -o "P:\Dev\Gitlab\PH.Core3\PH.Core3\BuildPackages"

dotnet pack "P:\Dev\Gitlab\PH.Core3\PH.Core3\PH.Core3.EntityFramework\PH.Core3.EntityFramework.csproj" -c Release --include-source -o "P:\Dev\Gitlab\PH.Core3\PH.Core3\BuildPackages"
rem dotnet pack "P:\Dev\Gitlab\PH.Core3\PH.Core3\PH.Core3.EntityFramework\PH.Core3.EntityFramework.csproj" -c Release --include-source --include-symbols -o "P:\Dev\Gitlab\PH.Core3\PH.Core3\BuildPackages"


dotnet pack "P:\Dev\Gitlab\PH.Core3\PH.Core3\PH.Core3.UnitOfWork\PH.Core3.UnitOfWork.csproj" -c Release --include-source  -o "P:\Dev\Gitlab\PH.Core3\PH.Core3\BuildPackages"
rem dotnet pack "P:\Dev\Gitlab\PH.Core3\PH.Core3\PH.Core3.UnitOfWork\PH.Core3.UnitOfWork.csproj" -c Release --include-source --include-symbols -o "P:\Dev\Gitlab\PH.Core3\PH.Core3\BuildPackages"

echo "done"
