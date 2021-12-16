using UniRx;

namespace VContainer.Unity.MVP
{
    public abstract class Presenter<TView, TPresenter>
        : IPresenter<TView, TPresenter>
        where TView : IView<TPresenter, TView>
        where TPresenter : IPresenter<TView, TPresenter>
    {
        protected readonly TView view;

        protected readonly CompositeDisposable disposables = new CompositeDisposable();

        [Inject]
        public Presenter(TView view)
        {
            this.view = view;
        }

        public virtual void Initialize()
        {
            view.ObservePreShow().Subscribe(
                _ => OnViewPreShow()).AddTo(disposables);
            view.ObservePostShow().Subscribe(
                _ => OnViewPostShow()).AddTo(disposables);
            view.ObserveCompleteShow().Subscribe(
                _ => OnViewCompleteShow()).AddTo(disposables);

            view.ObservePreHide().Subscribe(
                _ => OnViewPreHide()).AddTo(disposables);
            view.ObservePostHide().Subscribe(
                _ => OnViewPostHide()).AddTo(disposables);
            view.ObserveCompleteHide().Subscribe(
                _ => OnViewCompleteHide()).AddTo(disposables);
        }

        protected virtual void OnViewPreShow() { }

        protected virtual void OnViewPostShow() { }

        protected virtual void OnViewCompleteShow() { }

        protected virtual void OnViewPreHide() { }

        protected virtual void OnViewPostHide() { }

        protected virtual void OnViewCompleteHide() { }

        public virtual void Dispose()
        {
            disposables?.Dispose();
        }
    }
}
