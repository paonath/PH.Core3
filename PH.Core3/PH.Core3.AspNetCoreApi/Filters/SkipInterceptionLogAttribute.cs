using System;

namespace PH.Core3.AspNetCoreApi.Filters
{
    /// <summary>
    /// Do not perfom loggin on InterceptionAttributeFilter
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class SkipInterceptionLogAttribute : Attribute
    {

    }
}