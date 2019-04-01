namespace PH.Core3.Common.Settings
{
    /// <summary>
    /// Abstraction for a Setting that can change during scope.
    /// </summary>
    /// <typeparam name="T">Type of Value</typeparam>
    public interface IVariableSettingValue<T> : ISettingValue<T>
    {
        /// <summary>
        /// The Value of the Setting
        /// </summary>
        new T Value { get; set; }
        
        /// <summary>
        /// True if changed during scope life.
        /// </summary>
        bool Changed { get; }
        
        /// <summary>
        /// Set Value for Setting
        /// </summary>
        /// <param name="currentValue"></param>
        /// <typeparam name="T">Type of the setting value</typeparam>
        void SetValue(T currentValue);
        
        /// <summary>
        /// If changed during scope life reset to original value, otherwise do not perform nothing.
        ///
        /// Return Original value of Setting
        /// </summary>
        /// <returns>Setting Original Value</returns>
        T Reset();
    }
}