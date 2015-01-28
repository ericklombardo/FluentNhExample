using FluentNh.Stereotypes;

namespace FluentNh.Entities
{
    [Component]
    public class Address
    {
        public string Street { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }
}