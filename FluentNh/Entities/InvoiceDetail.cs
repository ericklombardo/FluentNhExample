using FluentNh.Stereotypes;

namespace FluentNh.Entities
{
    [Entity]
    public class InvoiceDetail
    {
        public virtual int Id { get; set; }
        public virtual Product Product { get; set; }
        public virtual int Quantity { get; set; }
        public virtual decimal Price { get; set; }
        public virtual Invoice Invoice { get; set; }
    }
}