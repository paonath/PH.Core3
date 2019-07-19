using System;
using Microsoft.Extensions.Logging;

namespace PH.Core3.AspNetCoreApi.Filters
{
    /// <summary>
    /// Attribute for auto-logging Actions on Controllers using <see cref="InterceptionAttributeFilter">InterceptionAttributeFilter</see>
    ///
    /// 
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class LogActionAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets a value indicating whether log ip caller.
        /// <para>Default <c>false</c></para>
        /// </summary> 
        /// <value><c>true</c> if [log ip caller]; otherwise, <c>false</c>.</value>
        public bool LogIpCaller { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether log action arguments: if <c>true</c> the model of action is json serialized and logged.
        /// <para>Default <c>true</c></para>
        /// </summary>
        /// <value><c>true</c> if log action arguments; otherwise, <c>false</c>.</value>
        public bool LogActionArguments { get; set; }

        /// <summary>Gets or sets the prefix on logged message.</summary>
        /// <para>Prepend this prefix to log message</para>
        /// <para>Default CALL</para>
        /// <value>The prefix.</value>
        public string Prefix { get; set; }

        /// <summary>Gets or sets the log level.
        ///<para>Default <c>LogLevel.Debug</c></para>
        /// </summary>
        /// <value>The log level.</value>
        public LogLevel LogLevel { get; set; }

        /// <summary>Gets or sets the postfix message. A custom string entry to append to log message.
        ///<para>Default <c>string.Empty</c></para>
        /// </summary>
        /// <value>The postfix message.</value>
        public string PostfixMessage { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="LogActionAttribute"/> class.
        /// </summary>
        public LogActionAttribute():this(LogLevel.Debug)
        {
            

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="LogActionAttribute"/> class.
        /// </summary>
        /// <param name="level">The log level.</param>
        public LogActionAttribute(LogLevel level)
        {
            LogIpCaller        = false;
            LogActionArguments = true;
            Prefix             = "CALL";
            LogLevel           = level;
            PostfixMessage     = string.Empty;
        }

        
    }
}