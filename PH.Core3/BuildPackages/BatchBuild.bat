echo "Build nuget packages"



dotnet pack "P:\Dev\Gitlab\PH.Core3\PH.Core3\PH.Core3.AspNetCoreApi\PH.Core3.AspNetCoreApi.csproj" -c Release --include-symbols --include-source  -o "P:\Dev\Gitlab\PH.Core3\PH.Core3\BuildPackages"

dotnet pack "P:\Dev\Gitlab\PH.Core3\PH.Core3\PH.Core3.Common\PH.Core3.Common.csproj" -c Release --include-symbols --include-source -o "P:\Dev\Gitlab\PH.Core3\PH.Core3\BuildPackages"

dotnet pack "P:\Dev\Gitlab\PH.Core3\PH.Core3\PH.Core3.Common.Services\PH.Core3.Common.Services.csproj" -c Release --include-symbols --include-source -o "P:\Dev\Gitlab\PH.Core3\PH.Core3\BuildPackages"

dotnet pack "P:\Dev\Gitlab\PH.Core3\PH.Core3\PH.Core3.Common.Services.Components\PH.Core3.Common.Services.Components.csproj" -c Release --include-symbols --include-source -o "P:\Dev\Gitlab\PH.Core3\PH.Core3\BuildPackages"


dotnet pack "P:\Dev\Gitlab\PH.Core3\PH.Core3\\PH.Core3.EntityFramework.Services.Components\PH.Core3.EntityFramework.Services.Components.csproj" -c Release --include-symbols --include-source -o "P:\Dev\Gitlab\PH.Core3\PH.Core3\BuildPackages"



echo "done"
