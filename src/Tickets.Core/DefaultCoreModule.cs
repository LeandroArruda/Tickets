using Autofac;

namespace Tickets.Core;

public class DefaultCoreModule : Module
{
  protected override void Load(ContainerBuilder builder)
  {
    //builder.RegisterType<ADomainService>()
    //    .As<IDomainAService>().InstancePerLifetimeScope();
  }
}
