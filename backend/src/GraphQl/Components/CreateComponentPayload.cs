namespace Metabase.GraphQl.Components
{
    public sealed class CreateComponentPayload
      : ComponentPayload<CreateComponentError>
    {
        public CreateComponentPayload(
            Data.Component component
            )
              : base(component)
        {
        }

        public CreateComponentPayload(
          CreateComponentError error
        )
        : base(error)
        {
        }
    }
}