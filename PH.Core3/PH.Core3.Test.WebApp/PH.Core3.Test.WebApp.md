<a name='assembly'></a>
# PH.Core3.Test.WebApp

## Contents

- [HttpTenantIdentificationStrategy](#T-PH-Core3-Test-WebApp-HttpTenantIdentificationStrategy 'PH.Core3.Test.WebApp.HttpTenantIdentificationStrategy')
  - [TryIdentifyTenant(tenantId)](#M-PH-Core3-Test-WebApp-HttpTenantIdentificationStrategy-TryIdentifyTenant-System-Object@- 'PH.Core3.Test.WebApp.HttpTenantIdentificationStrategy.TryIdentifyTenant(System.Object@)')
- [MailSenderService](#T-PH-Core3-Test-WebApp-HostedService-MailSenderService 'PH.Core3.Test.WebApp.HostedService.MailSenderService')
  - [#ctor()](#M-PH-Core3-Test-WebApp-HostedService-MailSenderService-#ctor-PH-Core3-AspNetCoreApi-Services-IViewRenderService- 'PH.Core3.Test.WebApp.HostedService.MailSenderService.#ctor(PH.Core3.AspNetCoreApi.Services.IViewRenderService)')
  - [Dispose(disposing)](#M-PH-Core3-Test-WebApp-HostedService-MailSenderService-Dispose-System-Boolean- 'PH.Core3.Test.WebApp.HostedService.MailSenderService.Dispose(System.Boolean)')
  - [Dispose()](#M-PH-Core3-Test-WebApp-HostedService-MailSenderService-Dispose 'PH.Core3.Test.WebApp.HostedService.MailSenderService.Dispose')
  - [StartAsync(cancellationToken)](#M-PH-Core3-Test-WebApp-HostedService-MailSenderService-StartAsync-System-Threading-CancellationToken- 'PH.Core3.Test.WebApp.HostedService.MailSenderService.StartAsync(System.Threading.CancellationToken)')
  - [StopAsync(cancellationToken)](#M-PH-Core3-Test-WebApp-HostedService-MailSenderService-StopAsync-System-Threading-CancellationToken- 'PH.Core3.Test.WebApp.HostedService.MailSenderService.StopAsync(System.Threading.CancellationToken)')
- [MainwebModule](#T-PH-Core3-Test-WebApp-AutofacModules-MainwebModule 'PH.Core3.Test.WebApp.AutofacModules.MainwebModule')
  - [Load(builder)](#M-PH-Core3-Test-WebApp-AutofacModules-MainwebModule-Load-Autofac-ContainerBuilder- 'PH.Core3.Test.WebApp.AutofacModules.MainwebModule.Load(Autofac.ContainerBuilder)')
- [PerRouteApiVersionSelector](#T-PH-Core3-Test-WebApp-PerRouteApiVersionSelector 'PH.Core3.Test.WebApp.PerRouteApiVersionSelector')
  - [SelectVersion(request,model)](#M-PH-Core3-Test-WebApp-PerRouteApiVersionSelector-SelectVersion-Microsoft-AspNetCore-Http-HttpRequest,Microsoft-AspNetCore-Mvc-Versioning-ApiVersionModel- 'PH.Core3.Test.WebApp.PerRouteApiVersionSelector.SelectVersion(Microsoft.AspNetCore.Http.HttpRequest,Microsoft.AspNetCore.Mvc.Versioning.ApiVersionModel)')
- [SwaggerDefaultValues](#T-PH-Core3-Test-WebApp-SwaggerDefaultValues 'PH.Core3.Test.WebApp.SwaggerDefaultValues')
  - [Apply(operation,context)](#M-PH-Core3-Test-WebApp-SwaggerDefaultValues-Apply-Swashbuckle-AspNetCore-Swagger-Operation,Swashbuckle-AspNetCore-SwaggerGen-OperationFilterContext- 'PH.Core3.Test.WebApp.SwaggerDefaultValues.Apply(Swashbuckle.AspNetCore.Swagger.Operation,Swashbuckle.AspNetCore.SwaggerGen.OperationFilterContext)')
- [ValuesController](#T-PH-Core3-Test-WebApp-Areas-v0-Controllers-ValuesController 'PH.Core3.Test.WebApp.Areas.v0.Controllers.ValuesController')
- [ValuesController](#T-PH-Core3-Test-WebApp-Areas-v1-Controllers-ValuesController 'PH.Core3.Test.WebApp.Areas.v1.Controllers.ValuesController')

<a name='T-PH-Core3-Test-WebApp-HttpTenantIdentificationStrategy'></a>
## HttpTenantIdentificationStrategy `type`

##### Namespace

PH.Core3.Test.WebApp

<a name='M-PH-Core3-Test-WebApp-HttpTenantIdentificationStrategy-TryIdentifyTenant-System-Object@-'></a>
### TryIdentifyTenant(tenantId) `method`

##### Summary

Attempts to identify the tenant from the current execution context.

##### Returns

`true` if the tenant could be identified; `false`
if not.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| tenantId | [System.Object@](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Object@ 'System.Object@') | The current tenant identifier. |

<a name='T-PH-Core3-Test-WebApp-HostedService-MailSenderService'></a>
## MailSenderService `type`

##### Namespace

PH.Core3.Test.WebApp.HostedService

<a name='M-PH-Core3-Test-WebApp-HostedService-MailSenderService-#ctor-PH-Core3-AspNetCoreApi-Services-IViewRenderService-'></a>
### #ctor() `constructor`

##### Summary

Initialize a new instance of [CoreDisposable](#T-PH-Core3-Common-CoreSystem-CoreDisposable 'PH.Core3.Common.CoreSystem.CoreDisposable')

##### Parameters

This constructor has no parameters.

<a name='M-PH-Core3-Test-WebApp-HostedService-MailSenderService-Dispose-System-Boolean-'></a>
### Dispose(disposing) `method`

##### Summary

Dispose Pattern.
This method check if already [Disposed](#P-PH-Core3-Common-CoreSystem-CoreDisposable-Disposed 'PH.Core3.Common.CoreSystem.CoreDisposable.Disposed') (and set it to True).

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| disposing | [System.Boolean](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Boolean 'System.Boolean') | True if disposing |

<a name='M-PH-Core3-Test-WebApp-HostedService-MailSenderService-Dispose'></a>
### Dispose() `method`

##### Summary

Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.

##### Parameters

This method has no parameters.

<a name='M-PH-Core3-Test-WebApp-HostedService-MailSenderService-StartAsync-System-Threading-CancellationToken-'></a>
### StartAsync(cancellationToken) `method`

##### Summary

Triggered when the application host is ready to start the service.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| cancellationToken | [System.Threading.CancellationToken](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Threading.CancellationToken 'System.Threading.CancellationToken') | Indicates that the start process has been aborted. |

<a name='M-PH-Core3-Test-WebApp-HostedService-MailSenderService-StopAsync-System-Threading-CancellationToken-'></a>
### StopAsync(cancellationToken) `method`

##### Summary

Triggered when the application host is performing a graceful shutdown.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| cancellationToken | [System.Threading.CancellationToken](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Threading.CancellationToken 'System.Threading.CancellationToken') | Indicates that the shutdown process should no longer be graceful. |

<a name='T-PH-Core3-Test-WebApp-AutofacModules-MainwebModule'></a>
## MainwebModule `type`

##### Namespace

PH.Core3.Test.WebApp.AutofacModules

<a name='M-PH-Core3-Test-WebApp-AutofacModules-MainwebModule-Load-Autofac-ContainerBuilder-'></a>
### Load(builder) `method`

##### Summary

Override to add registrations to the container.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| builder | [Autofac.ContainerBuilder](#T-Autofac-ContainerBuilder 'Autofac.ContainerBuilder') | The builder through which components can be
registered. |

##### Remarks

Note that the ContainerBuilder parameter is unique to this module.

<a name='T-PH-Core3-Test-WebApp-PerRouteApiVersionSelector'></a>
## PerRouteApiVersionSelector `type`

##### Namespace

PH.Core3.Test.WebApp

<a name='M-PH-Core3-Test-WebApp-PerRouteApiVersionSelector-SelectVersion-Microsoft-AspNetCore-Http-HttpRequest,Microsoft-AspNetCore-Mvc-Versioning-ApiVersionModel-'></a>
### SelectVersion(request,model) `method`

##### Summary

Selects an API version given the specified HTTP request and API version information.

##### Returns

The selected [ApiVersion](#T-Microsoft-AspNetCore-Mvc-ApiVersion 'Microsoft.AspNetCore.Mvc.ApiVersion').

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| request | [Microsoft.AspNetCore.Http.HttpRequest](#T-Microsoft-AspNetCore-Http-HttpRequest 'Microsoft.AspNetCore.Http.HttpRequest') | The current [HttpRequest](#T-Microsoft-AspNetCore-Http-HttpRequest 'Microsoft.AspNetCore.Http.HttpRequest') to select the version for. |
| model | [Microsoft.AspNetCore.Mvc.Versioning.ApiVersionModel](#T-Microsoft-AspNetCore-Mvc-Versioning-ApiVersionModel 'Microsoft.AspNetCore.Mvc.Versioning.ApiVersionModel') | The [ApiVersionModel](#T-Microsoft-AspNetCore-Mvc-Versioning-ApiVersionModel 'Microsoft.AspNetCore.Mvc.Versioning.ApiVersionModel') to select the version from. |

<a name='T-PH-Core3-Test-WebApp-SwaggerDefaultValues'></a>
## SwaggerDefaultValues `type`

##### Namespace

PH.Core3.Test.WebApp

<a name='M-PH-Core3-Test-WebApp-SwaggerDefaultValues-Apply-Swashbuckle-AspNetCore-Swagger-Operation,Swashbuckle-AspNetCore-SwaggerGen-OperationFilterContext-'></a>
### Apply(operation,context) `method`

##### Summary

Applies the filter to the specified operation using the given context.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| operation | [Swashbuckle.AspNetCore.Swagger.Operation](#T-Swashbuckle-AspNetCore-Swagger-Operation 'Swashbuckle.AspNetCore.Swagger.Operation') | The operation to apply the filter to. |
| context | [Swashbuckle.AspNetCore.SwaggerGen.OperationFilterContext](#T-Swashbuckle-AspNetCore-SwaggerGen-OperationFilterContext 'Swashbuckle.AspNetCore.SwaggerGen.OperationFilterContext') | The current operation filter context. |

<a name='T-PH-Core3-Test-WebApp-Areas-v0-Controllers-ValuesController'></a>
## ValuesController `type`

##### Namespace

PH.Core3.Test.WebApp.Areas.v0.Controllers

##### Summary

Controller V0

<a name='T-PH-Core3-Test-WebApp-Areas-v1-Controllers-ValuesController'></a>
## ValuesController `type`

##### Namespace

PH.Core3.Test.WebApp.Areas.v1.Controllers

##### Summary

Controller V1
