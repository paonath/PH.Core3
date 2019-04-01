namespace PH.Core3.Common.Settings
{
    /// <summary>
    /// Read-Only Setting
    /// </summary>
    /// <typeparam name="T">Type of Value</typeparam>
    public class SettingConst<T> : SettingAbstraction<T> , ISettingValue<T>
    {
        /// <summary>
        /// Init new instance of <see cref="SettingConst{T}"/>
        /// 
        /// </summary>
        public SettingConst():base()
        {
            
        }

        /// <summary>
        /// Init new instance of <see cref="SettingConst{T}"/> with read-only value
        /// </summary>
        /// <param name="v">Setting value</param>
        public SettingConst(T v) : base(v)
        {
        }
        
        /// <summary>
        /// Implicit conversion for value
        /// </summary>
        /// <param name="d">Setting instance</param>
        /// <returns>Value of the Setting</returns>
        public static implicit operator T(SettingConst<T> d)  
        {
            return d.GetValue();
        }
        


    }
}