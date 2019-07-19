﻿using System;

namespace PH.Core3.AspNetCoreApi.Filters
{
    /// <summary>
    /// Do not perfom loggin on InterceptionAttributeFilter
    /// </summary>
    [Obsolete("Use instead LogActionAttribute")]
    [AttributeUsage(AttributeTargets.Method)]
    public class SkipInterceptionLogAttribute : Attribute
    {

    }
}