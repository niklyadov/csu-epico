using Epico.Entity;
using System.Collections.Generic;

namespace Epico.Models
{
    public class ProductViewModel
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Vision { get; set; }
        public string Mission { get; set; }
        public string ProductFormula { get; set; }
        public string OwnerUserId { get; set; }
    }
}
