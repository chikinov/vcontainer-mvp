using UnityEngine;
using VContainer;
using VContainer.Unity;
using VContainer.Unity.MVP;

namespace Example
{
    public class RootLifetimeScope : LifetimeScope
    {
        [SerializeField]
        private AlertDialogFactory.Settings alertDialogFactorySettings;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(alertDialogFactorySettings);
            builder.RegisterFactory<string, string, string, string, AlertDialog>(
                container => container.Resolve<AlertDialogFactory>().Create,
                Lifetime.Singleton);

            builder.UseEntryPoints(
                Lifetime.Singleton,
                entryPoints => entryPoints.Add<AlertDialogFactory>().AsSelf());
        }
    }
}
