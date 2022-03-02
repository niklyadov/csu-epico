using System.Collections.Generic;

namespace Epico.Models.ProductEntity
{
    public class ProductEntityViewModel
    {
        public int Id { get; set; }
        public List<Entity.Feature> ElementsList { get; set; }
    }
}
