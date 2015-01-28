using System.Collections.Generic;
using FluentNh.Stereotypes;

namespace FluentNh.Entities
{
    [Entity]
    public class Invoice
    {
        public virtual int Id { get; set; }
        public virtual Client Client { get; set; }
        public virtual string Code { get; set; }
        public virtual IList<InvoiceDetail> Details { get; set; }
    }
}