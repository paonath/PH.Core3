# PH.Core3

## NetStandard2.1 and NetCoreApp3.0 Utilities for Crud API System

This repo is a small framework for CRUD operation on a API System, structuread as packages:

| Package | Description | TargetFramework|
|----:|:----|:----|
| **PH.Core3.Common** | Main package, should be referenced by all projects on solution | netstandard2.1 |
| **PH.Core3.Common.Services** | Base Services Abstraction amd Interfaces. Should be referenced by Services project containing Interfaces | netstandard2.1 |
| **PH.Core3.Common.Services.Components** | Base Services Implementation: should be referenced by project containing Services implementations | netstandard2.1 | 
| **PH.Core3.AspNetCoreApi** | API Base Package: should be referenced by Asp.Net Core API Project | netcoreapp3.0 |


PH.Core3.Common:  [![NuGet Badge](https://buildstats.info/nuget/PH.Core3.Common)](https://www.nuget.org/packages/PH.Core3.Common/)

## NetStandard2.0 and NetCoreApp2.2

NetStandard2.0 and NetCoreApp2.2 should use the release [1.2.5](https://github.com/paonath/PH.Core3/releases/tag/1.2.5)

