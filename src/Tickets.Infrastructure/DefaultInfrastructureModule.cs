using Autofac;
using Tickets.Core.Aggregates.MovieSessionAggregate;
using Tickets.Infrastructure.Data;
using Tickets.SharedKernel;
using Tickets.SharedKernel.Interfaces;
using MediatR;
using MediatR.Pipeline;
using System.Reflection;
using Module = Autofac.Module;

namespace Tickets.Infrastructure;

public class DefaultInfrastructureModule : Module
{
  private readonly List<Assembly> _assemblies = new List<Assembly>();

  public DefaultInfrastructureModule(Assembly? callingAssembly = null)
  {
    var coreAssembly =
      Assembly.GetAssembly(typeof(MovieSession));
    var infrastructureAssembly = Assembly.GetAssembly(typeof(StartupSetup));
    if (coreAssembly != null)
    {
      _assemblies.Add(coreAssembly);
    }

    if (infrastructureAssembly != null)
    {
      _assemblies.Add(infrastructureAssembly);
    }

    if (callingAssembly != null)
    {
      _assemblies.Add(callingAssembly);
    }
  }

  protected override void Load(ContainerBuilder builder)
  {
    RegisterCommonDependencies(builder);
  }

  private void RegisterCommonDependencies(ContainerBuilder builder)
  {
    builder.RegisterGeneric(typeof(EfRepository<>))
      .As(typeof(IRepository<>))
      .As(typeof(IReadRepository<>))
      .InstancePerLifetimeScope();

    builder
      .RegisterType<Mediator>()
      .As<IMediator>()
      .InstancePerLifetimeScope();

    builder
      .RegisterType<DomainEventDispatcher>()
      .As<IDomainEventDispatcher>()
      .InstancePerLifetimeScope();

    var mediatrOpenTypes = new[]
    {
          typeof(IRequestHandler<,>),
          typeof(IRequestExceptionHandler<,,>),
          typeof(IRequestExceptionAction<,>),
          typeof(INotificationHandler<>),
        };

    foreach (var mediatrOpenType in mediatrOpenTypes)
    {
      builder
        .RegisterAssemblyTypes(_assemblies.ToArray())
        .AsClosedTypesOf(mediatrOpenType)
        .AsImplementedInterfaces();
    }
  }
}