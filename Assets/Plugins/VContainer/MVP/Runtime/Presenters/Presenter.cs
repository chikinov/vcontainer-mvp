namespace VContainer.Unity.MVP
{
    public abstract class Presenter<TView, TPresenter>
        : IPresenter<TView, TPresenter>
        where TView : IView<TPresenter, TView>
        where TPresenter : IPresenter<TView, TPresenter>
    {
        protected readonly TView view;

        [Inject]
        public Presenter(TView view)
        {
            this.view = view;
        }

        public virtual void Initialize()
        {
            view.PreShow.AddListener(PreShowView);
            view.PostShow.AddListener(PostShowView);
            view.OnCompleteShow.AddListener(OnCompleteShowView);

            view.PreHide.AddListener(PreHideView);
            view.PostHide.AddListener(PostHideView);
            view.OnCompleteHide.AddListener(OnCompleteHideView);
        }

        protected virtual void PreShowView() { }

        protected virtual void PostShowView() { }

        protected virtual void OnCompleteShowView() { }

        protected virtual void PreHideView() { }

        protected virtual void PostHideView() { }

        protected virtual void OnCompleteHideView() { }

        public virtual void Dispose()
        {
            view.PreShow.RemoveListener(PreShowView);
            view.PostShow.RemoveListener(PreHideView);
            view.OnCompleteShow.RemoveListener(OnCompleteShowView);

            view.PreHide.RemoveListener(PreHideView);
            view.PostHide.RemoveListener(PostHideView);
            view.OnCompleteHide.RemoveListener(OnCompleteHideView);
        }
    }
}
