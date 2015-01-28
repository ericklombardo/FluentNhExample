using FluentNh.Stereotypes;

namespace FluentNh.Entities
{
    [Entity]
    public class Client
    {
        public virtual int Id { get; set; }
        public virtual Name Name { get; set; }
        public virtual Address Address { get; set; }
    }
}