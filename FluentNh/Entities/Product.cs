using FluentNh.Stereotypes;

namespace FluentNh.Entities
{
    [Entity]
    public class Product
    {
        public virtual int Id { get; set; }
        public virtual string Description { get; set; }
        public virtual decimal Price { get; set; }
    }
}