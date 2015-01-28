using System;
using System.Linq;
using FluentNh.Stereotypes;
using FluentNHibernate;
using FluentNHibernate.Automapping;

namespace FluentNh.Mapping
{
    public class DemoAutomappingConfiguration : DefaultAutomappingConfiguration
    {
        public override bool ShouldMap(Type type)
        {
            return type.Namespace == "FluentNh.Entities";
            //return type.CustomAttributes.Any(a => a.AttributeType == typeof (EntityAttribute));
        }

        public override bool IsComponent(Type type)
        {
            //return type.Namespace == "FluentNh.Components";
            return type.CustomAttributes.Any(a => a.AttributeType == typeof (ComponentAttribute));
        }

        public override string GetComponentColumnPrefix(Member member)
        {
            //return member.PropertyType.Name + "_";
            return member.PropertyType.Name;
        }

        
        public override bool IsId(Member member)
        {
            //return member.Name == member.DeclaringType.Name + "Id";
            return base.IsId(member);
        }
        
    }
}