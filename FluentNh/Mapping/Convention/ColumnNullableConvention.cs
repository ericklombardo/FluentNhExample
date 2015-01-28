using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using NHibernate.Util;

namespace FluentNh.Mapping.Convention
{
    public class ColumnNullableConvention : IPropertyConvention, IReferenceConvention  
    {
        public void Apply(IPropertyInstance instance)
        {
            if (!instance.Property.MemberInfo.DeclaringType.IsNullable() )
                instance.Not.Nullable();

            if (instance.Property.PropertyType == typeof (string))
            {
                instance.CustomType("AnsiString");
                instance.Length(100);
            }

            if (instance.Property.PropertyType == typeof(decimal))
            {
                instance.Precision(12);
                instance.Scale(2);
            }

        }

        public void Apply(IManyToOneInstance instance)
        {
            instance.Not.Nullable();
        }
    }
}