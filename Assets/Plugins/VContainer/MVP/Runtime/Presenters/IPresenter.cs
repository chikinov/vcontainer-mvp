using System;

namespace VContainer.Unity.MVP
{
    public interface IPresenter : IInitializable, IDisposable
    {
    }

    public interface IPresenter<TView, TPresenter> : IPresenter
        where TView : IView<TPresenter, TView>
        where TPresenter : IPresenter<TView, TPresenter>
    {
        void OnViewShow();

        void OnViewHide();
    }
}
