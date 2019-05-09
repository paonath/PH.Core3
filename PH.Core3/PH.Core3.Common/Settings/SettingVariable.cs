using JetBrains.Annotations;

namespace PH.Core3.Common.Settings
{
    /// <summary>
    /// Variable Setting
    /// </summary>
    /// <typeparam name="T">Type of Value</typeparam>
    public class SettingVariable<T> : SettingConst<T>, IVariableSettingValue<T>
    {
        private  T _currentValue;

        /// <summary>
        /// Init new instance of <see cref="SettingVariable{T}"/>
        /// </summary>
        public SettingVariable()
            :base()
        {
            Changed = false;
            CanSet  = true;
        }
        
        /// <summary>
        /// Init new instance of <see cref="SettingVariable{T}"/> with initial value
        /// </summary>
        /// <param name="v">Setting Value</param>
        public SettingVariable(T v) : base(v)
        {
            _currentValue = v;
            Changed       = false;
        }

        /// <summary>
        /// Implicit conversion for value
        /// </summary>
        /// <param name="d">Setting instance</param>
        /// <returns>Value of the Setting</returns>
        public static implicit operator T([NotNull] SettingVariable<T> d)  
        {
            return d.GetValue();
        }


        /// <summary>
        /// Implicit conversion for value
        /// </summary>
        /// <param name="value">Value of the setting</param>
        /// <returns>Setting Variable</returns>
        [NotNull]
        public static implicit operator SettingVariable<T>(T value)
        {
            return new SettingVariable<T>(value);
        }

        /// <summary>
        /// Return the current value of setting.
        /// </summary>
        /// <returns></returns>
        public override T GetValue()
        {
            return _currentValue;
        }

        /// <summary>
        /// If True the current value is changed from original value
        /// </summary>
        public bool Changed { get; private set; }


        /// <summary>
        /// Set the Setting Value
        /// </summary>
        /// <param name="value">value</param>
        public override void SetValue(T value)
        {
            Changed       = !_currentValue.Equals(value);
            _currentValue = value;
        }


        /// <summary>
        /// If changed during scope life reset to original value, otherwise do not perform nothing.
        ///
        /// Return Original value of Setting
        /// </summary>
        /// <returns>Setting Original Value</returns>
        public T Reset()
        {
           
            _currentValue = _v;
            Changed       = false;
            return _currentValue;
        }

      
    }
}