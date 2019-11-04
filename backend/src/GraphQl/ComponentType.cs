using HotChocolate.Types;
using HotChocolate.Types.Relay;

namespace Icon.GraphQl
{
    public class ComponentType : ObjectType<Component>
    {
        protected override void Configure(IObjectTypeDescriptor<Component> descriptor)
        {
            descriptor.Name("Component");

            descriptor.Field(f => f.Id)
                .Type<NonNullType<UuidType>>();

            descriptor.Field(f => f.Version)
                .Type<NonNullType<IntType>>();

            descriptor.Field<ComponentResolvers>(f => f.GetVersions(default))
                .Type<NonNullType<ListType<NonNullType<ComponentVersionType>>>>();
        }
    }
}