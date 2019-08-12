using System;
using Microsoft.Extensions.Logging;
using PH.Core3.AspNetCoreApi.Filters;

namespace PH.Core3.AspNetCoreApi.Attributes
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
        public bool LogActionIncomeArguments { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether log action outcome data: : if <c>true</c> the model of action outcome is json serialized and logged.
        /// <para>Default <c>true</c></para>
        /// </summary>
        /// <value>
        ///   <c>true</c> if log action outcome data; otherwise, <c>false</c>.
        /// </value>
        public bool LogActionOutcomeData { get; set; }

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

        /// <summary>Gets or sets the message: if any, the message is written to ActionExcecutin before any other parts of this attribute</summary>
        /// <value>The message.</value>
        public string Message { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogActionAttribute"/> class.
        /// </summary>
        public LogActionAttribute(LogLevel level, string message = "") : this()
        {
            LogLevel = level;
            Message  = message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogActionAttribute"/> class.
        /// </summary>
        public LogActionAttribute()
        {
            LogIpCaller              = false;
            LogActionIncomeArguments = true;
            LogActionOutcomeData     = true;
            Prefix                   = "CALL";
            LogLevel                 = LogLevel.Debug;
            PostfixMessage           = string.Empty;
            Message                  = string.Empty;
        }
    }
}