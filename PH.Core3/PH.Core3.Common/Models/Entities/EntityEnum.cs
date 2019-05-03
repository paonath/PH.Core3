using System;
using System.ComponentModel.DataAnnotations;

namespace PH.Core3.Common.Models.Entities
{
    /// <summary>
    /// Entity Class For Mapping Enums
    /// </summary>
    public class EntityEnum : IEntityEnum
    {
        /// <summary>
        /// Unique Id of current class
        /// </summary>
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Value { get; set; }

        [StringLength(4000)]
        public string Description { get; set; }

        /// <summary>
        /// Row Version and Concurrency Check Token
        /// </summary>
        [Timestamp]
        public byte[] Timestamp { get; set; }

        [Required]
        [StringLength(1000)]
        public string AssemblyFullName { get; set; }

        
    }

    //public static class EntityEnumExtensions
    //{
    //    public static EntityEnum ToEntityEnum(this Enum enumValue)
    //    {
    //        var t = enumValue.GetTypeCode().GetType().FullName;
    //        return new EntityEnum()
    //        {
    //            Description = enumValue.ToString(),
    //            Value = enumValue.ToString(),
    //            Id = (int)enumValue,
    //            AssemblyFullName = t,
                
    //        }
    //    }
    //}
}