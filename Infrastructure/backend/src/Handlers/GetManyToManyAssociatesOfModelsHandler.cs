using Infrastructure.Aggregates;
using Infrastructure.Models;

namespace Infrastructure.Handlers
{
    public abstract class GetManyToManyAssociatesOfModelsHandler<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate>
      : GetAssociatesOfModelsHandler<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate>
      where TModel : IModel
      where TAssociationModel : IManyToManyAssociation
      where TAssociateModel : IModel
      where TAggregate : class, IAggregate, IConvertible<TModel>, new()
      where TAssociationAggregate : class, IManyToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
      where TAssociateAggregate : class, IAggregate, IConvertible<TAssociateModel>, new()
    {
        protected GetManyToManyAssociatesOfModelsHandler(
            IModelRepository repository
            )
          : base(repository)
        {
        }
    }
}