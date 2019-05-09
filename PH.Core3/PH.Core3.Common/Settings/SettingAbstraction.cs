using System;
using JetBrains.Annotations;

namespace PH.Core3.Common.Settings
{
    /// <summary>
    /// Setting Abstraction
    /// </summary>
    /// <typeparam name="T">Type of Value</typeparam>
    public class SettingAbstraction<T>
    {
        /// <summary>
        /// Original value
        /// </summary>
        protected internal T _v;
        
        /// <summary>
        /// If True Setting new value is enabled.
        /// </summary>
        public bool CanSet { get; protected set; }

        /// <summary>
        /// Init new instance of <see cref="SettingAbstraction{T}"/>
        /// </summary>
        public SettingAbstraction()
        {
            CanSet = true;
        }
        /// <summary>
        /// Init new instance of <see cref="SettingAbstraction{T}"/> with initial value
        /// </summary>
        /// <param name="v">Setting Value</param>
        protected SettingAbstraction(T v)
        {
            _v     = v;
            CanSet = false;
        }


        /// <summary>
        /// Get the Value of the Setting
        /// </summary>
        public T Value
        {
            get => GetValue();
            set => SetValue(value);
        }

        /// <summary>
        /// Set the Setting Value
        /// </summary>
        /// <param name="value">value</param>
        public virtual void SetValue(T value)
        {
            if(!CanSet)
            {
                throw new InvalidOperationException("Can't assign new value to const setting");
            }

            _v     = value;
            CanSet = false;
            
        }

        /// <summary>
        /// Return the Value of the Setting
        /// </summary>
        /// <returns></returns>
        public virtual T GetValue()
        {
            return _v;
        }

        /// <summary>
        /// Implicit conversion for value
        /// </summary>
        /// <param name="d">Setting instance</param>
        /// <returns>Value of the Setting</returns>
        public static implicit operator T([NotNull] SettingAbstraction<T> d) // implicit digit to byte conversion operator
        {
            if (d is null)
            {
                throw new ArgumentNullException(nameof(d));
            }

            return d.GetValue();
        }
        
    }
}