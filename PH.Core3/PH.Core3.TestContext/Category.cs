using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PH.Core3.Common.Models.Entities;

namespace PH.Core3.TestContext
{

    public enum ColorEnum 
    {
        [Display(Name = "giallo")]
        Yellow = 1,
        [Display(Name = "verde")]
        Green = 2,
        [Display(Name = "rosso")]
        Red = 55
    }


    public class Category : TreeEntity<Category, Guid>
    {
        public string Name { get; set; }
        public virtual ICollection<Albero> Alberi { get; set; }

        public virtual ColorEnum Colore { get; set; }

        public Category()
        {
            Alberi = new HashSet<Albero>();
        }
    }

    
}
