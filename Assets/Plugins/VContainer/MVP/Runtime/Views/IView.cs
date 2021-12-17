using System;
using UnityEngine;
using UnityEngine.Events;

namespace VContainer.Unity.MVP
{
    public interface IView : IDisposable
    {
        string Name { get; set; }

        Transform Transform { get; }

        bool IsVisible { get; set; }

        UnityEvent PreShow { get; }

        UnityEvent PostShow { get; }

        UnityEvent OnCompleteShow { get; }

        UnityEvent PreHide { get; }

        UnityEvent PostHide { get; }

        UnityEvent OnCompleteHide { get; }

        IAnimation Show(bool animated = true);

        IAnimation Hide(bool animated = true);
    }

    public interface IView<TPresenter, TView> : IView
        where TPresenter : IPresenter<TView, TPresenter>
        where TView : IView<TPresenter, TView>
    {
    }
}
