namespace PH.Core3.Common.Settings
{
    /// <summary>
    /// Abstraction for Setting
    /// </summary>
    /// <typeparam name="T">Type of Value</typeparam>
    public interface ISettingValue<out T>
    {
        /// <summary>
        /// The Value of the Setting
        /// </summary>
        T Value { get; }

        /// <summary>
        /// Get Current Value for Setting
        /// </summary>
        /// <returns></returns>
        T GetValue();
    }
}