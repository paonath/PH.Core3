using System;
using System.Collections.Generic;
using PH.Core3.Common.Models.Entities;

namespace PH.Core3.TestContext
{
    public class Category : TreeEntity<Category, Guid>
    {
        public string Name { get; set; }
        public virtual ICollection<Albero> Alberi { get; set; }

        public Category()
        {
            Alberi = new HashSet<Albero>();
        }
    }

    
}
