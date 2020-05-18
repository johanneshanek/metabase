using Errors = Icon.Errors;
using CSharpFunctionalExtensions;
using Guid = System.Guid;
using Commands = Icon.Commands;
using Newtonsoft.Json;

namespace Icon.Events
{
    public sealed class ComponentManufacturerAdded
      : AddedEvent
    {
        public static ComponentManufacturerAdded From(
              Guid componentManufacturerId,
              Commands.Add<ValueObjects.AddComponentManufacturerInput> command
            )
        {
            return new ComponentManufacturerAdded(
                componentManufacturerId: componentManufacturerId,
                componentId: command.Input.ComponentId,
                institutionId: command.Input.InstitutionId,
                marketingInformation: command.Input.MarketingInformation is null ? null : ComponentManufacturerMarketingInformationEventData.From(command.Input.MarketingInformation),
                creatorId: command.CreatorId
                );
        }

        [JsonIgnore]
        public Guid ComponentId { get => ParentId; }

        [JsonIgnore]
        public Guid InstitutionId { get => AssociateId; }

        public ComponentManufacturerMarketingInformationEventData? MarketingInformation { get; set; }

#nullable disable
        public ComponentManufacturerAdded() { }
#nullable enable

        public ComponentManufacturerAdded(
            Guid componentManufacturerId,
            Guid componentId,
            Guid institutionId,
            ComponentManufacturerMarketingInformationEventData? marketingInformation,
            Guid creatorId
            )
          : base(
              aggregateId: componentManufacturerId,
              parentId: componentId,
              associateId: institutionId,
              creatorId: creatorId
              )
        {
            MarketingInformation = marketingInformation;
            EnsureValid();
        }

        public override Result<bool, Errors> Validate()
        {
            return
              Result.Combine(
                  base.Validate(),
                  MarketingInformation?.Validate() ?? Result.Ok<bool, Errors>(true)
                  );
        }
    }
}