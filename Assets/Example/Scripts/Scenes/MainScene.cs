using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;
using VContainer.Unity.MVP;

namespace Example
{
    public class MainScene : IAsyncStartable
    {
        private readonly LifetimeScope lifetimeScope;
        private readonly IObjectResolver container;

        private readonly Func<string, string, string, string, AlertDialog> alertDialogFactory;

        [Inject]
        public MainScene(LifetimeScope lifetimeScope, IObjectResolver container)
        {
            this.lifetimeScope = lifetimeScope;
            this.container = container;

            alertDialogFactory = container.Resolve<Func<string, string, string, string, AlertDialog>>();
        }

        async UniTask IAsyncStartable.StartAsync(CancellationToken cancellation)
        {
            if (alertDialogFactory.Target is IAsyncStartable asyncStartable)
                await asyncStartable.StartAsync(default);

            await LoadViewsAsync();
        }

        private async Task LoadViewsAsync()
        {
            var settings = lifetimeScope.Container.Resolve<MainSceneSettings>();

            var loadRootViewTask = settings.RootViewRef.InstantiateAsync().Task;
            var loadMainViewTask = settings.MainViewRef.InstantiateAsync().Task;

            await Task.WhenAll(loadRootViewTask, loadMainViewTask);

            if (!loadRootViewTask.Result.TryGetComponent<RootView>(
                out var rootView)) return;

            if (!loadMainViewTask.Result.TryGetComponent<MainView>(
                out var mainView)) return;

            var container = lifetimeScope.CreateChild(
                builder =>
                {
                    builder.RegisterInstance(rootView);
                    builder.Register<RootPresenter>(Lifetime.Scoped);

                    builder.RegisterInstance(mainView);
                    builder.Register<MainPresenter>(Lifetime.Scoped);
                })
                .Container;

            container.Inject(rootView);
            container.Inject(mainView);

            mainView.Parent = rootView;

            rootView.Show();

            mainView.Show();
        }
    }
}
