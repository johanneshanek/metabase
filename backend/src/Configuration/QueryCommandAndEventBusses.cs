using NSwag.Generation.Processors.Security;
using IAggregateRepository = Icon.Infrastructure.Aggregate.IAggregateRepository;
using IdentityServer4.AccessTokenValidation;
using Icon.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSwag.AspNetCore;
using Microsoft.OpenApi;
using NSwag;
using IdentityServer4.AspNetIdentity;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using Command = Icon.Infrastructure.Command;
using Query = Icon.Infrastructure.Query;
using Events = Icon.Events;
using Models = Icon.Models;
using System.Threading.Tasks;
using System;
using WebPWrecover.Services;
using IdentityServer4.Validation;
using Queries = Icon.Queries;
using Commands = Icon.Commands;
using Handlers = Icon.Handlers;
using IError = HotChocolate.IError;
using CSharpFunctionalExtensions;
using MediatR;

namespace Icon.Configuration
{
    public sealed class QueryCommandAndEventBusses
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddMediatR(typeof(Startup));

            /* services.AddScoped<MediatR.IMediator, MediatR.Mediator>(); */
            /* services.AddTransient<MediatR.ServiceFactory>(sp => t => sp.GetService(t)); */

            services.AddScoped<Query.IQueryBus, Query.QueryBus>();
            services.AddScoped<Command.ICommandBus, Command.CommandBus>();
            services.AddScoped<Events.IEventBus, Events.EventBus>();

            AddComponentHandlers(services);
            AddDatabaseHandlers(services);
            AddInstitutionHandlers(services);
            AddMethodHandlers(services);
            AddPersonHandlers(services);
            AddStakeholderHandlers(services);
            AddStandardHandlers(services);

            AddComponentManufacturerHandlers(services);
            AddInstitutionRepresentativeHandlers(services);
            AddMethodDeveloperHandlers(services);
            AddPersonAffiliationHandlers(services);

            // TODO Shall we broadcast events?
            /* services.AddScoped<INotificationHandler<ClientCreated>, ClientsEventHandler>(); */
            /* services.AddScoped<IRequestHandler<CreateClient, Unit>, ClientsCommandHandler>(); */
            /* services.AddScoped<IRequestHandler<GetClientView, ClientView>, ClientsQueryHandler>(); */
        }

        public static void AddModelHandlers(IServiceCollection services)
        {
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetModelsForTimestampedIds<Models.IModel>,
              IEnumerable<Result<Models.IModel, Errors>>
                >,
              Handlers.GetModelsOfUnknownTypeForTimestampedIdsHandler
                >();
        }

        public static void AddComponentHandlers(IServiceCollection services)
        {
            // Create
            services.AddScoped<
              MediatR.IRequestHandler<
              Commands.CreateComponent,
              Result<ValueObjects.TimestampedId, Errors>
                >,
              Handlers.CreateModelHandler<Commands.CreateComponent, Aggregates.ComponentAggregate>>(serviceProvider =>
                  new Handlers.CreateModelHandler<Commands.CreateComponent, Aggregates.ComponentAggregate>(
                    serviceProvider.GetRequiredService<IAggregateRepository>(),
                    Events.ComponentCreated.From
                    )
                  );

            // Get
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetModelsAtTimestamps<Models.Component>,
              IEnumerable<Result<IEnumerable<Result<Models.Component, Errors>>, Errors>>
                >,
              Handlers.GetModelsAtTimestampsHandler<Models.Component, Aggregates.ComponentAggregate, Events.ComponentCreated>
                >();
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetModelsForTimestampedIds<Models.Component>,
              IEnumerable<Result<Models.Component, Errors>>
                >,
              Handlers.GetModelsForTimestampedIdsHandler<Models.Component, Aggregates.ComponentAggregate>
                >();
        }

        public static void AddDatabaseHandlers(IServiceCollection services)
        {
            // Create
            services.AddScoped<
              MediatR.IRequestHandler<
              Commands.CreateDatabase,
              Result<ValueObjects.TimestampedId, Errors>
                >,
              Handlers.CreateModelHandler<Commands.CreateDatabase, Aggregates.DatabaseAggregate>>(serviceProvider =>
                  new Handlers.CreateModelHandler<Commands.CreateDatabase, Aggregates.DatabaseAggregate>(
                    serviceProvider.GetRequiredService<IAggregateRepository>(),
                    Events.DatabaseCreated.From
                    )
                  );

            // Get
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetModelsAtTimestamps<Models.Database>,
              IEnumerable<Result<IEnumerable<Result<Models.Database, Errors>>, Errors>>
                >,
              Handlers.GetModelsAtTimestampsHandler<Models.Database, Aggregates.DatabaseAggregate, Events.DatabaseCreated>
                >();
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetModelsForTimestampedIds<Models.Database>,
              IEnumerable<Result<Models.Database, Errors>>
                >,
              Handlers.GetModelsForTimestampedIdsHandler<Models.Database, Aggregates.DatabaseAggregate>
                >();
        }

        public static void AddInstitutionHandlers(IServiceCollection services)
        {
            // Create
            services.AddScoped<
              MediatR.IRequestHandler<
              Commands.CreateInstitution,
              Result<ValueObjects.TimestampedId, Errors>
                >,
              Handlers.CreateModelHandler<Commands.CreateInstitution, Aggregates.InstitutionAggregate>>(serviceProvider =>
                  new Handlers.CreateModelHandler<Commands.CreateInstitution, Aggregates.InstitutionAggregate>(
                    serviceProvider.GetRequiredService<IAggregateRepository>(),
                    Events.InstitutionCreated.From
                    )
                  );

            // Get
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetModelsAtTimestamps<Models.Institution>,
              IEnumerable<Result<IEnumerable<Result<Models.Institution, Errors>>, Errors>>
                >,
              Handlers.GetModelsAtTimestampsHandler<Models.Institution, Aggregates.InstitutionAggregate, Events.InstitutionCreated>
                >();
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetModelsForTimestampedIds<Models.Institution>,
              IEnumerable<Result<Models.Institution, Errors>>
                >,
              Handlers.GetModelsForTimestampedIdsHandler<Models.Institution, Aggregates.InstitutionAggregate>
                >();
        }

        public static void AddMethodHandlers(IServiceCollection services)
        {
            // Create
            services.AddScoped<
              MediatR.IRequestHandler<
              Commands.CreateMethod,
              Result<ValueObjects.TimestampedId, Errors>
                >,
              Handlers.CreateModelHandler<Commands.CreateMethod, Aggregates.MethodAggregate>>(serviceProvider =>
                  new Handlers.CreateModelHandler<Commands.CreateMethod, Aggregates.MethodAggregate>(
                    serviceProvider.GetRequiredService<IAggregateRepository>(),
                    Events.MethodCreated.From
                    )
                  );

            // Get
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetModelsAtTimestamps<Models.Method>,
              IEnumerable<Result<IEnumerable<Result<Models.Method, Errors>>, Errors>>
                >,
              Handlers.GetModelsAtTimestampsHandler<Models.Method, Aggregates.MethodAggregate, Events.MethodCreated>
                >();
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetModelsForTimestampedIds<Models.Method>,
              IEnumerable<Result<Models.Method, Errors>>
                >,
              Handlers.GetModelsForTimestampedIdsHandler<Models.Method, Aggregates.MethodAggregate>
                >();
        }

        public static void AddPersonHandlers(IServiceCollection services)
        {
            // Create
            services.AddScoped<
              MediatR.IRequestHandler<
              Commands.CreatePerson,
              Result<ValueObjects.TimestampedId, Errors>
                >,
              Handlers.CreateModelHandler<Commands.CreatePerson, Aggregates.PersonAggregate>>(serviceProvider =>
                  new Handlers.CreateModelHandler<Commands.CreatePerson, Aggregates.PersonAggregate>(
                    serviceProvider.GetRequiredService<IAggregateRepository>(),
                    Events.PersonCreated.From
                    )
                  );

            // Get
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetModelsAtTimestamps<Models.Person>,
              IEnumerable<Result<IEnumerable<Result<Models.Person, Errors>>, Errors>>
                >,
              Handlers.GetModelsAtTimestampsHandler<Models.Person, Aggregates.PersonAggregate, Events.PersonCreated>
                >();
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetModelsForTimestampedIds<Models.Person>,
              IEnumerable<Result<Models.Person, Errors>>
                >,
              Handlers.GetModelsForTimestampedIdsHandler<Models.Person, Aggregates.PersonAggregate>
                >();
        }

        public static void AddStakeholderHandlers(IServiceCollection services)
        {
            // Get
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetModelsForTimestampedIds<Models.Stakeholder>,
              IEnumerable<Result<Models.Stakeholder, Errors>>
                >,
              Handlers.GetStakeholdersForTimestampedIdsHandler
                >();
        }

        public static void AddStandardHandlers(IServiceCollection services)
        {
            // Create
            services.AddScoped<
              MediatR.IRequestHandler<
              Commands.CreateStandard,
              Result<ValueObjects.TimestampedId, Errors>
                >,
              Handlers.CreateModelHandler<Commands.CreateStandard, Aggregates.StandardAggregate>>(serviceProvider =>
                  new Handlers.CreateModelHandler<Commands.CreateStandard, Aggregates.StandardAggregate>(
                    serviceProvider.GetRequiredService<IAggregateRepository>(),
                    Events.StandardCreated.From
                    )
                  );

            // Get
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetModelsAtTimestamps<Models.Standard>,
              IEnumerable<Result<IEnumerable<Result<Models.Standard, Errors>>, Errors>>
                >,
              Handlers.GetModelsAtTimestampsHandler<Models.Standard, Aggregates.StandardAggregate, Events.StandardCreated>
                >();
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetModelsForTimestampedIds<Models.Standard>,
              IEnumerable<Result<Models.Standard, Errors>>
                >,
              Handlers.GetModelsForTimestampedIdsHandler<Models.Standard, Aggregates.StandardAggregate>
                >();
        }

        public static void AddComponentManufacturerHandlers(IServiceCollection services)
        {
            // Add
            services.AddScoped<
              MediatR.IRequestHandler<
              Commands.AddComponentManufacturer,
              Result<ValueObjects.TimestampedId, Errors>
                >,
              Handlers.CreateModelHandler<Commands.AddComponentManufacturer, Aggregates.ComponentManufacturerAggregate>>(serviceProvider =>
                  new Handlers.CreateModelHandler<Commands.AddComponentManufacturer, Aggregates.ComponentManufacturerAggregate>(
                    serviceProvider.GetRequiredService<IAggregateRepository>(),
                    Events.ComponentManufacturerAdded.From
                    )
                  );

            // Get
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetModelsForTimestampedIds<Models.ComponentManufacturer>,
              IEnumerable<Result<Models.ComponentManufacturer, Errors>>
                >,
              Handlers.GetModelsForTimestampedIdsHandler<Models.ComponentManufacturer, Aggregates.ComponentManufacturerAggregate>
                >();

            // Get associations
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetAssociatesOfModelsIdentifiedByTimestampedIds<Models.Component, Models.ComponentManufacturer>,
              IEnumerable<Result<IEnumerable<Result<Models.ComponentManufacturer, Errors>>, Errors>>
                >,
              Handlers.GetForwardAssociationsOfModelsIdentifiedByTimestampedIdsHandler<Models.Component, Models.ComponentManufacturer, Aggregates.ComponentManufacturerAggregate, Events.ComponentManufacturerAdded>
                >();
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetAssociatesOfModelsIdentifiedByTimestampedIds<Models.Institution, Models.ComponentManufacturer>,
              IEnumerable<Result<IEnumerable<Result<Models.ComponentManufacturer, Errors>>, Errors>>
                >,
              Handlers.GetBackwardAssociationsOfModelsIdentifiedByTimestampedIdsHandler<Models.Institution, Models.ComponentManufacturer, Aggregates.ComponentManufacturerAggregate, Events.ComponentManufacturerAdded>
                >();

            // Get associates
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetAssociatesOfModelsIdentifiedByTimestampedIds<Models.Component, Models.Institution>,
              IEnumerable<Result<IEnumerable<Result<Models.Institution, Errors>>, Errors>>
                >,
              Handlers.GetForwardAssociatesOfModelsIdentifiedByTimestampedIdsHandler<Models.Component, Models.Institution, Aggregates.InstitutionAggregate, Events.ComponentManufacturerAdded>
                >();
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetAssociatesOfModelsIdentifiedByTimestampedIds<Models.Institution, Models.Component>,
              IEnumerable<Result<IEnumerable<Result<Models.Component, Errors>>, Errors>>
                >,
              Handlers.GetBackwardAssociatesOfModelsIdentifiedByTimestampedIdsHandler<Models.Institution, Models.Component, Aggregates.ComponentAggregate, Events.ComponentManufacturerAdded>
                >();
        }

        public static void AddInstitutionRepresentativeHandlers(IServiceCollection services)
        {
            // Add
            services.AddScoped<
              MediatR.IRequestHandler<
              Commands.AddInstitutionRepresentative,
              Result<ValueObjects.TimestampedId, Errors>
                >,
              Handlers.CreateModelHandler<Commands.AddInstitutionRepresentative, Aggregates.InstitutionRepresentativeAggregate>>(serviceProvider =>
                  new Handlers.CreateModelHandler<Commands.AddInstitutionRepresentative, Aggregates.InstitutionRepresentativeAggregate>(
                    serviceProvider.GetRequiredService<IAggregateRepository>(),
                    Events.InstitutionRepresentativeAdded.From
                    )
                  );

            // Get
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetModelsForTimestampedIds<Models.InstitutionRepresentative>,
              IEnumerable<Result<Models.InstitutionRepresentative, Errors>>
                >,
              Handlers.GetModelsForTimestampedIdsHandler<Models.InstitutionRepresentative, Aggregates.InstitutionRepresentativeAggregate>
                >();

            // Get associations
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetAssociatesOfModelsIdentifiedByTimestampedIds<Models.Institution, Models.InstitutionRepresentative>,
              IEnumerable<Result<IEnumerable<Result<Models.InstitutionRepresentative, Errors>>, Errors>>
                >,
              Handlers.GetForwardAssociationsOfModelsIdentifiedByTimestampedIdsHandler<Models.Institution, Models.InstitutionRepresentative, Aggregates.InstitutionRepresentativeAggregate, Events.InstitutionRepresentativeAdded>
                >();
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetAssociatesOfModelsIdentifiedByTimestampedIds<Models.User, Models.InstitutionRepresentative>,
              IEnumerable<Result<IEnumerable<Result<Models.InstitutionRepresentative, Errors>>, Errors>>
                >,
              Handlers.GetBackwardAssociationsOfModelsIdentifiedByTimestampedIdsHandler<Models.User, Models.InstitutionRepresentative, Aggregates.InstitutionRepresentativeAggregate, Events.InstitutionRepresentativeAdded>
                >();

            // Get associates
            // TODO Add the following.
            /* services.AddScoped< */
            /*   MediatR.IRequestHandler< */
            /*   Queries.GetAssociatesOfModelsIdentifiedByTimestampedIds<Models.Institution, Models.User>, */
            /*   IEnumerable<Result<IEnumerable<Result<Models.User, Errors>>, Errors>> */
            /*     >, */
            /*   Handlers.GetForwardAssociatesOfModelsIdentifiedByTimestampedIdsHandler<Models.Institution, Models.User, Aggregates.UserAggregate, Events.InstitutionRepresentativeAdded> */
            /*     >(); */
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetAssociatesOfModelsIdentifiedByTimestampedIds<Models.User, Models.Institution>,
              IEnumerable<Result<IEnumerable<Result<Models.Institution, Errors>>, Errors>>
                >,
              Handlers.GetBackwardAssociatesOfModelsIdentifiedByTimestampedIdsHandler<Models.User, Models.Institution, Aggregates.InstitutionAggregate, Events.InstitutionRepresentativeAdded>
                >();
        }

        public static void AddMethodDeveloperHandlers(IServiceCollection services)
        {
            // Add
            services.AddScoped<
              MediatR.IRequestHandler<
              Commands.AddMethodDeveloper,
              Result<ValueObjects.TimestampedId, Errors>
                >,
              Handlers.AddMethodDeveloperHandler>();

            // Get
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetModelsForTimestampedIds<Models.MethodDeveloper>,
              IEnumerable<Result<Models.MethodDeveloper, Errors>>
                >,
              Handlers.GetMethodDevelopersForTimestampedIdsHandler
                >();

            // Get associates
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetAssociatesOfModelsIdentifiedByTimestampedIds<Models.Method, Models.Stakeholder>,
              IEnumerable<Result<IEnumerable<Result<Models.Stakeholder, Errors>>, Errors>>
                >,
              Handlers.GetDevelopersOfMethodsIdentifiedByTimestampedIdsHandler
                >();
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetAssociatesOfModelsIdentifiedByTimestampedIds<Models.Person, Models.Method>,
              IEnumerable<Result<IEnumerable<Result<Models.Method, Errors>>, Errors>>
                >,
              Handlers.GetBackwardAssociatesOfModelsIdentifiedByTimestampedIdsHandler<Models.Person, Models.Method, Aggregates.MethodAggregate, Events.PersonMethodDeveloperAdded>
                >();
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetAssociatesOfModelsIdentifiedByTimestampedIds<Models.Institution, Models.Method>,
              IEnumerable<Result<IEnumerable<Result<Models.Method, Errors>>, Errors>>
                >,
              Handlers.GetBackwardAssociatesOfModelsIdentifiedByTimestampedIdsHandler<Models.Institution, Models.Method, Aggregates.MethodAggregate, Events.InstitutionMethodDeveloperAdded>
                >();
        }

        public static void AddPersonAffiliationHandlers(IServiceCollection services)
        {
            // Add
            services.AddScoped<
              MediatR.IRequestHandler<
              Commands.AddPersonAffiliation,
              Result<ValueObjects.TimestampedId, Errors>
                >,
              Handlers.CreateModelHandler<Commands.AddPersonAffiliation, Aggregates.PersonAffiliationAggregate>>(serviceProvider =>
                  new Handlers.CreateModelHandler<Commands.AddPersonAffiliation, Aggregates.PersonAffiliationAggregate>(
                    serviceProvider.GetRequiredService<IAggregateRepository>(),
                    Events.PersonAffiliationAdded.From
                    )
                  );

            // Get
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetModelsForTimestampedIds<Models.PersonAffiliation>,
              IEnumerable<Result<Models.PersonAffiliation, Errors>>
                >,
              Handlers.GetModelsForTimestampedIdsHandler<Models.PersonAffiliation, Aggregates.PersonAffiliationAggregate>
                >();

            // Get associates
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetAssociatesOfModelsIdentifiedByTimestampedIds<Models.Person, Models.Institution>,
              IEnumerable<Result<IEnumerable<Result<Models.Institution, Errors>>, Errors>>
                >,
              Handlers.GetForwardAssociatesOfModelsIdentifiedByTimestampedIdsHandler<Models.Person, Models.Institution, Aggregates.InstitutionAggregate, Events.PersonAffiliationAdded>
                >();
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetAssociatesOfModelsIdentifiedByTimestampedIds<Models.Institution, Models.Person>,
              IEnumerable<Result<IEnumerable<Result<Models.Person, Errors>>, Errors>>
                >,
              Handlers.GetBackwardAssociatesOfModelsIdentifiedByTimestampedIdsHandler<Models.Institution, Models.Person, Aggregates.PersonAggregate, Events.PersonAffiliationAdded>
                >();
        }
    }
}