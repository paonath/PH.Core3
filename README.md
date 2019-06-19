# PH.Core3

## NetStandard2.0 and NetCoreApp2.2 Utilities for Crud API System

This repo is a small framework for CRUD operation on a API System, structuread as packages:

| Package | Description | TargetFramework|
|----:|:----|:----|
| **PH.Core3.Common** | Main package, should be referenced by all projects on solution | netstandard2.0 |
| **PH.Core3.Common.Services** | Base Services Abstraction amd Interfaces. Should be referenced by Services project containing Interfaces | netstandard2.0 |
| **PH.Core3.Common.Services.Components** | Base Services Implementation: should be referenced by project containing Services implementations | netstandard2.0 | 
| **PH.Core3.EntityFramework** | DAL Abstraction: should be referenced by project containing Entrity Framework Core DbContext | netstandard2.0 | 
| **PH.Core3.UnitOfWork** | Unit Of Work Interface: should be referenced only by API project for Commit and Rollback actions. | netstandard2.0 | 
| **PH.Core3.AspNetCoreApi** | API Base Package: should be referenced by Asp.Net Core API Project | netcoreapp2.2 |


PH.Core3.Common: [![NuGet Badge](https://buildstats.info/nuget/PH.Core3.Common)](https://www.nuget.org/packages/PH.Core3.Common)
