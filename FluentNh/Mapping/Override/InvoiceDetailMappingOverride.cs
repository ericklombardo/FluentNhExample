using FluentNh.Entities;
using FluentNHibernate;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace FluentNh.Mapping.Override
{
    public class InvoiceDetailMappingOverride : IAutoMappingOverride<Invoice>
    {
        public void Override(AutoMapping<Invoice> mapping)
        {
            //mapping.Map(Reveal.Member<Product>("Abc"))
              //  .Nullable();
            mapping.HasMany(detail => detail.Details).Inverse().Cascade.All();
        }
    }
}