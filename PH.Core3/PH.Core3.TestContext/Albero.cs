using System;
using PH.Core3.Common.Models.Entities;

namespace PH.Core3.TestContext
{
    public class Albero : TreeEntity<Albero,Guid>
    {
        public string Description { get; set; }
        public Guid? CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}