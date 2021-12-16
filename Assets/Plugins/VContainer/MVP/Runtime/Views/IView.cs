using System;
using UniRx;
using UnityEngine;

namespace VContainer.Unity.MVP
{
    public interface IView : IDisposable
    {
        string Name { get; set; }

        Transform Transform { get; }

        bool IsVisible { get; set; }

        IObservable<Unit> ObservePreShow();

        IObservable<Unit> ObservePostShow();

        IObservable<Unit> ObserveCompleteShow();

        IObservable<Unit> ObservePreHide();

        IObservable<Unit> ObservePostHide();

        IObservable<Unit> ObserveCompleteHide();

        IAnimation Show(bool animated = true);

        IAnimation Hide(bool animated = true);
    }

    public interface IView<TPresenter, TView> : IView
        where TPresenter : IPresenter<TView, TPresenter>
        where TView : IView<TPresenter, TView>
    {
    }
}
