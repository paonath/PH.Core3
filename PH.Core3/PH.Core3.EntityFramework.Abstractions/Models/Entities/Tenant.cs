//using System.ComponentModel.DataAnnotations;

//namespace PH.Core3.EntityFramework.Abstractions.Models.Entities
//{
//    /// <summary>
//    /// Tenant Entity
//    /// </summary>
//    public class Tenant
//    {
//        /// <summary>Gets or sets the identifier.</summary>
//        /// <value>The identifier.</value>
//        public int Id { get; set; }

//        /// <summary>Gets or sets the name.</summary>
//        /// <value>The name.</value>
//        [StringLength(128)]
//        public string Name { get; set; }

//        /// <summary>Gets or sets the name normalized.</summary>
//        /// <value>The name  normalized.</value>
//        [StringLength(128)]
//        public string NormalizedName { get; set; }

//        /// <summary>
//        /// Row Version and Concurrency Check Token
//        /// </summary>
//        [Timestamp]
//        public byte[] Timestamp { get; set; }
//    }
//}