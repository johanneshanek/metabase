using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Icon.Data;
using IdentityServer4.AccessTokenValidation;
using IdentityServer4.AspNetIdentity;
using IdentityServer4.Validation;
using Marten.NodaTime;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using NSwag;
using NSwag.AspNetCore;
using NSwag.Generation.Processors.Security;
using WebPWrecover.Services;
using Aggregate = Icon.Infrastructure.Aggregate;
using Aggregates = Icon.Aggregates;
using Command = Icon.Infrastructure.Command;
using Event = Icon.Events;
using Events = Icon.Events;
using Models = Icon.Models;
using Query = Icon.Infrastructure.Query;

namespace Icon.Configuration
{
    public sealed class EventStore
    {
        public static void ConfigureServices(IServiceCollection services, IWebHostEnvironment environment, AppSettings.DatabaseSettings databaseSettings)
        {
            services.AddScoped(typeof(Marten.IDocumentStore), serviceProvider =>
                BuildDocumentStore(
                  environment,
                  databaseSettings,
                  serviceProvider.GetRequiredService<ILogger<EventStore>>()
                  )
                );
            services.AddScoped<Aggregate.IAggregateRepository, Aggregate.AggregateRepository>();
        }

        public static Marten.IDocumentStore BuildDocumentStore(IWebHostEnvironment environment, AppSettings.DatabaseSettings databaseSettings, ILogger<EventStore> logger)
        {
            var martenLogger = new MartenLogger(logger);
            return Marten.DocumentStore.For(_ =>
                {
                    _.Connection(databaseSettings.ConnectionString);
                    _.DatabaseSchemaName = databaseSettings.SchemaName.EventStore;
                    _.Events.DatabaseSchemaName = databaseSettings.SchemaName.EventStore;
                    /* _.UseNodaTime(); */
                    // For a full list auf auto-create options, see
                    // https://jasperfx.github.io/marten/documentation/schema/
                    if (environment.IsDevelopment() || environment.IsEnvironment("Test"))
                    {
                        _.AutoCreateSchemaObjects = Marten.AutoCreate.All;
                    }
                    else
                    {
                        _.AutoCreateSchemaObjects = Marten.AutoCreate.CreateOrUpdate;
                    }
                    _.Events.UseAggregatorLookup(Marten.Services.Events.AggregationLookupStrategy.UsePrivateApply);
                    _.UseDefaultSerialization( // https://martendb.io/documentation/documents/json/newtonsoft/
                        enumStorage: Marten.EnumStorage.AsString,
                        collectionStorage: Marten.CollectionStorage.AsArray
                        );

                    _.Logger(martenLogger);
                    _.Listeners.Add(martenLogger);

                    ProjectAggregatesInline(_);
                    AddEventTypes(_);
                });
        }

        private static void ProjectAggregatesInline(Marten.StoreOptions options)
        {
            // Originally generated from the command line with the command
            // `ls -1 src/Aggregates/ | grep -E ".*Aggregate.cs$" | grep -v -E "^MethodDeveloperAggregate.cs$" | grep -v -E "^I((Many|One)ToMany)?AssociationAggregate.cs$" | grep -v -E "^DataAggregate.cs$" | sed -e "s/^\(.*\).cs\$/options.Events.InlineProjections.AggregateStreamsWith<Aggregates.\1>();/"`
            options.Events.InlineProjections.AggregateStreamsWith<Aggregates.CalorimetricDataAggregate>();
            options.Events.InlineProjections.AggregateStreamsWith<Aggregates.ComponentAggregate>();
            options.Events.InlineProjections.AggregateStreamsWith<Aggregates.ComponentConcretizationAggregate>();
            options.Events.InlineProjections.AggregateStreamsWith<Aggregates.ComponentManufacturerAggregate>();
            options.Events.InlineProjections.AggregateStreamsWith<Aggregates.ComponentPartAggregate>();
            options.Events.InlineProjections.AggregateStreamsWith<Aggregates.ComponentVariantAggregate>();
            options.Events.InlineProjections.AggregateStreamsWith<Aggregates.ComponentVersionAggregate>();
            options.Events.InlineProjections.AggregateStreamsWith<Aggregates.DatabaseAggregate>();
            options.Events.InlineProjections.AggregateStreamsWith<Aggregates.HygrothermalDataAggregate>();
            options.Events.InlineProjections.AggregateStreamsWith<Aggregates.InstitutionAggregate>();
            options.Events.InlineProjections.AggregateStreamsWith<Aggregates.InstitutionMethodDeveloperAggregate>();
            options.Events.InlineProjections.AggregateStreamsWith<Aggregates.InstitutionOperatedDatabaseAggregate>();
            options.Events.InlineProjections.AggregateStreamsWith<Aggregates.InstitutionRepresentativeAggregate>();
            options.Events.InlineProjections.AggregateStreamsWith<Aggregates.MethodAggregate>();
            options.Events.InlineProjections.AggregateStreamsWith<Aggregates.OpticalDataAggregate>();
            options.Events.InlineProjections.AggregateStreamsWith<Aggregates.PersonAffiliationAggregate>();
            options.Events.InlineProjections.AggregateStreamsWith<Aggregates.PersonAggregate>();
            options.Events.InlineProjections.AggregateStreamsWith<Aggregates.PersonMethodDeveloperAggregate>();
            options.Events.InlineProjections.AggregateStreamsWith<Aggregates.PhotovoltaicDataAggregate>();
            options.Events.InlineProjections.AggregateStreamsWith<Aggregates.StandardAggregate>();
            options.Events.InlineProjections.AggregateStreamsWith<Aggregates.UserAggregate>();
        }

        private static void AddEventTypes(Marten.StoreOptions options)
        {
            // Originally generated from the command line with the command
            // `ls -1 src/Events/ | grep "Created\|Added\|Deleted\|Removed" | grep -v -E "^I?(Created|Deleted)Event.cs$" | grep -v -E "^Data(Created|Deleted)Event.cs$" | grep -v -E "^I?Association(Added|Removed)Event.cs$" | grep -v -E "^MethodDeveloperAdded.cs$" | sed -e "s/^\(.*\).cs\$/options.Events.AddEventType(typeof(Events.\1));/"`
            options.Events.AddEventType(typeof(Events.CalorimetricDataCreated));
            options.Events.AddEventType(typeof(Events.CalorimetricDataDeleted));
            options.Events.AddEventType(typeof(Events.ComponentConcretizationAdded));
            options.Events.AddEventType(typeof(Events.ComponentConcretizationRemoved));
            options.Events.AddEventType(typeof(Events.ComponentCreated));
            options.Events.AddEventType(typeof(Events.ComponentDeleted));
            options.Events.AddEventType(typeof(Events.ComponentManufacturerAdded));
            options.Events.AddEventType(typeof(Events.ComponentManufacturerRemoved));
            options.Events.AddEventType(typeof(Events.ComponentPartAdded));
            options.Events.AddEventType(typeof(Events.ComponentPartRemoved));
            options.Events.AddEventType(typeof(Events.ComponentVariantAdded));
            options.Events.AddEventType(typeof(Events.ComponentVariantRemoved));
            options.Events.AddEventType(typeof(Events.ComponentVersionAdded));
            options.Events.AddEventType(typeof(Events.ComponentVersionRemoved));
            options.Events.AddEventType(typeof(Events.DatabaseCreated));
            options.Events.AddEventType(typeof(Events.DatabaseDeleted));
            options.Events.AddEventType(typeof(Events.HygrothermalDataCreated));
            options.Events.AddEventType(typeof(Events.HygrothermalDataDeleted));
            options.Events.AddEventType(typeof(Events.InstitutionCreated));
            options.Events.AddEventType(typeof(Events.InstitutionDeleted));
            options.Events.AddEventType(typeof(Events.InstitutionMethodDeveloperAdded));
            options.Events.AddEventType(typeof(Events.InstitutionMethodDeveloperRemoved));
            options.Events.AddEventType(typeof(Events.InstitutionOperatedDatabaseAdded));
            options.Events.AddEventType(typeof(Events.InstitutionOperatedDatabaseRemoved));
            options.Events.AddEventType(typeof(Events.InstitutionRepresentativeAdded));
            options.Events.AddEventType(typeof(Events.InstitutionRepresentativeRemoved));
            options.Events.AddEventType(typeof(Events.MethodCreated));
            options.Events.AddEventType(typeof(Events.MethodDeleted));
            options.Events.AddEventType(typeof(Events.MethodDeveloperRemoved));
            options.Events.AddEventType(typeof(Events.OpticalDataCreated));
            options.Events.AddEventType(typeof(Events.OpticalDataDeleted));
            options.Events.AddEventType(typeof(Events.PersonAffiliationAdded));
            options.Events.AddEventType(typeof(Events.PersonAffiliationRemoved));
            options.Events.AddEventType(typeof(Events.PersonCreated));
            options.Events.AddEventType(typeof(Events.PersonDeleted));
            options.Events.AddEventType(typeof(Events.PersonMethodDeveloperAdded));
            options.Events.AddEventType(typeof(Events.PersonMethodDeveloperRemoved));
            options.Events.AddEventType(typeof(Events.PhotovoltaicDataCreated));
            options.Events.AddEventType(typeof(Events.PhotovoltaicDataDeleted));
            options.Events.AddEventType(typeof(Events.StandardCreated));
            options.Events.AddEventType(typeof(Events.StandardDeleted));
            options.Events.AddEventType(typeof(Events.UserCreated));
            options.Events.AddEventType(typeof(Events.UserDeleted));
        }
    }
}