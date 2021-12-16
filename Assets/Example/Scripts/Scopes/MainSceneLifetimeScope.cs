using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Example
{
    public class MainSceneLifetimeScope : LifetimeScope
    {
        [SerializeField]
        private MainSceneSettings mainSceneSettings;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(mainSceneSettings);

            builder.RegisterEntryPoint<MainScene>();
        }
    }
}
