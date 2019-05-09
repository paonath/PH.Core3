using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using JetBrains.Annotations;

namespace PH.Core3.Common.Services.Components.Crud.Entities
{
    /// <summary>
    /// Use Data Annotations to valid property of a class.
    /// 
    /// </summary>
    public class DataAnnotationsValidator
    {
        /// <summary>
        /// Try to validate.
        /// </summary>
        /// <param name="object">The object.</param>
        /// <param name="results">The results.</param>
        /// <returns><c>True</c> if valid</returns>
        public static bool TryValidate([NotNull] object @object, [NotNull] out ICollection<ValidationResult> results)
        {
            var context = new ValidationContext(@object, serviceProvider: null, items: null);
            results = new List<ValidationResult>();

            var internalValidate = Validator.TryValidateObject(
                                                               @object, context, results, 
                                                               validateAllProperties: true);

            PropertyInfo[] propInfos = @object.GetType().GetProperties(BindingFlags.Public|BindingFlags.Instance);

            foreach (var propertyInfo in propInfos)
            {
                if (propertyInfo.PropertyType.IsEnum)
                {

                    var v = propertyInfo.GetValue(@object);

                    if (!System.Enum.IsDefined(propertyInfo.PropertyType, v))
                    {
                        results.Add(new ValidationResult($"The {propertyInfo.Name} has value {v} which is not defined", new []{propertyInfo.Name}));
                    }

                    
                }
            }



            return results.Count == 0;
        }

       
    }
}