using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PH.Core3.EntityFramework.Abstractions.Models.Entities;

namespace PH.Core3.TestContext
{
    public class Category : TreeEntity<Category, Guid>
    {
        [Required]
        [StringLength(23)]
        public string Name { get; set; }
        public virtual ICollection<Albero> Alberi { get; set; }

        public Category()
        {
            Alberi = new HashSet<Albero>();
        }
    }

    
}
