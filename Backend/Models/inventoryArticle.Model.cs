using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Example_FrontEnd.Backend.Models
{

    /// <summary>
    /// Model of InventoryArticle
    /// </summary>
    public class InventoryArticle
    {
        public string id { get; set; }
        public string producttitle { get; set; }
        public string varianttitle { get; set; }
        public int amount { get; set; }
        public int delta { get; set; }
    }
}
