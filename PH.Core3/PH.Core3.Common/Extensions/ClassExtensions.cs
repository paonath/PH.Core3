using System;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace PH.Core3.Common.Extensions
{
    /// <summary>
    /// Class and Object Extensions
    /// </summary>
    public static class ClassExtensions
    {
        /// <summary>Return a child instance from a parent instance</summary>
        /// <typeparam name="TParent">The type of the parent.</typeparam>
        /// <typeparam name="TChild">The type of the child.</typeparam>
        /// <param name="p">The parent instance.</param>
        /// <returns></returns>
        [CanBeNull]
        public static TChild AsChild<TParent,TChild>([CanBeNull] this TParent p)
            where TParent : class, new()
            where TChild : class, TParent, new()
        {
            return ParseParent<TChild, TParent>(p);
        }


        /// <summary>Parses the parent and return as his child element</summary>
        /// <typeparam name="TChild">The type of the child.</typeparam>
        /// <typeparam name="TParent">The type of the parent.</typeparam>
        /// <param name="parentInstance">The parent instance.</param>
        /// <returns>the child instance</returns>
        [CanBeNull]
        public static TChild ParseParent<TChild, TParent>([CanBeNull] TParent parentInstance) 
            where TParent : class , new()
            where TChild : class, TParent, new()
        {
            if (null == parentInstance)
            {
                return null;
            }
            else
            {
                var child = Activator.CreateInstance<TChild>();
                var parentInfos = parentInstance.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                                .OrderBy(x => x.Name).ToArray();
                var childInfos = child.GetType().GetProperties().ToArray();
                foreach (var info in parentInfos)
                {
                    var v = info.GetValue(parentInstance);
                    var p = childInfos.FirstOrDefault(x => x.Name == info.Name);
                    if (null != p && p.CanWrite)
                    {
                        p.SetValue(child, v);
                    }

                }

                return child;
            }
        }
    }
}