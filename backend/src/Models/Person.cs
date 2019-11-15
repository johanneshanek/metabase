using System.Collections.Generic;
using Guid = System.Guid;
using DateTime = System.DateTime;

#nullable enable
namespace Icon.Models
{
    public sealed class Person
      : Stakeholder
    {
        public string Name { get; }
        public Guid ContactInformationId { get; }
        public IEnumerable<Institution> Affiliations { get; }

        public Person(
            Guid id,
            string name,
            Guid contactInformationId,
            IEnumerable<Institution> affiliations,
            DateTime timestamp
            )
          : base(id, timestamp)
        {
            Name = name;
            ContactInformationId = contactInformationId;
            Affiliations = affiliations;
        }
    }
}