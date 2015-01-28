using FluentNh.Entities;
using FluentNHibernate;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace FluentNh.Mapping.Override
{
    public class ProductMappingOverride : IAutoMappingOverride<Product>
    {
        public void Override(AutoMapping<Product> mapping)
        {
            //mapping.Map(Reveal.Member<Product>("Abc"))
              //  .Nullable();
            
        }
    }
}