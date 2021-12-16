using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace VContainer.Unity.MVP
{
    public class UIView : UIBehaviour, IUIView
    {
        [SerializeField] private Animation showAnimation;
        [SerializeField] private Animation hideAnimation;

        protected readonly List<IUIView> children = new List<IUIView>();

        public virtual Transform ChildParentTransform => Transform;

        public string Name
        {
            get => IsDestroyed() || !gameObject ? null : gameObject.name;
            set
            {
                if (IsDestroyed() || !gameObject) return;

                gameObject.name = value;
            }
        }

        public Transform Transform => RectTransform;

        public virtual bool IsVisible
        {
            get => !IsDestroyed() && gameObject && gameObject.activeSelf;
            set
            {
                if (IsDestroyed() || !gameObject) return;

                gameObject.SetActive(value);
            }
        }

        private IUIView parent;
        public IUIView Parent
        {
            get => parent;
            set
            {
                if (value == parent) return;

                if (parent != null) parent.RemoveChild(this);

                parent = value;

                if (parent == null) return;

                parent.AddChild(this);
            }
        }

        private RectTransform rectTransform;
        public RectTransform RectTransform =>
            IsDestroyed() ? null : rectTransform ?
            rectTransform : rectTransform = transform as RectTransform;

        private CanvasGroup canvasGroup;
        public virtual CanvasGroup CanvasGroup =>
            IsDestroyed() ? null : canvasGroup ?
            canvasGroup : canvasGroup = GetComponent<CanvasGroup>();

        public virtual float Alpha
        {
            get => CanvasGroup ? canvasGroup.alpha : 0F;
            set
            {
                if (CanvasGroup) canvasGroup.alpha = value;
            }
        }

        public bool Interactable
        {
            get => CanvasGroup && canvasGroup.interactable;
            set
            {
                if (CanvasGroup) canvasGroup.interactable = value;
            }
        }

        private ISubject<Unit> preShow;
        public IObservable<Unit> ObservePreShow() => preShow ??= new Subject<Unit>();

        private ISubject<Unit> postShow;
        public IObservable<Unit> ObservePostShow() => postShow ??= new Subject<Unit>();

        private ISubject<Unit> completeShow;
        public IObservable<Unit> ObserveCompleteShow() => completeShow ??= new Subject<Unit>();

        private ISubject<Unit> preHide;
        public IObservable<Unit> ObservePreHide() => preHide ??= new Subject<Unit>();

        private ISubject<Unit> postHide;
        public IObservable<Unit> ObservePostHide() => postHide ??= new Subject<Unit>();

        private ISubject<Unit> completeHide;
        public IObservable<Unit> ObserveCompleteHide() => completeHide ??= new Subject<Unit>();

        public virtual IAnimation Show(bool animated = true)
        {
            preShow?.OnNext(Unit.Default);

            if (hideAnimation) hideAnimation.Stop();

            IsVisible = true;

            Interactable = true;

            if (!showAnimation)
                showAnimation = gameObject.AddComponent<DefaultAnimation>();

            showAnimation.Play().OnComplete(
                () => completeShow?.OnNext(Unit.Default));

            postShow?.OnNext(Unit.Default);

            if (!animated) showAnimation.Complete();

            return showAnimation;
        }

        public virtual IAnimation Hide(bool animated = true)
        {
            preHide?.OnNext(Unit.Default);

            if (showAnimation) showAnimation.Stop();

            Interactable = false;

            if (!hideAnimation)
                hideAnimation = gameObject.AddComponent<DefaultAnimation>();

            hideAnimation.Play().OnComplete(
                () =>
                {
                    IsVisible = false;

                    completeHide?.OnNext(Unit.Default);
                });

            postHide?.OnNext(Unit.Default);

            if (!animated) hideAnimation.Complete();

            return hideAnimation;
        }

        void IUIView.AddChild(IUIView view)
        {
            if (children.Contains(view)) return;

            children.Add(view);

            var rectTransform = view.RectTransform;
            rectTransform.SetParent(ChildParentTransform);
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMax = Vector2.zero;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.pivot = new Vector2(0.5F, 0.5F);
            rectTransform.localPosition = Vector3.zero;
            rectTransform.localScale = Vector3.one;
        }

        void IUIView.RemoveChild(IUIView view)
        {
            if (children.Remove(view)) view.Transform.SetParent(null);
        }

        public virtual IUIView FindChild<T>() where T : IUIView
        {
            return children.Find(view => view is T);
        }

        public void Dispose()
        {
            for (var i = children.Count - 1; i >= 0; i--)
                children[i].Dispose();

            Parent = null;

            Destroy(gameObject);
        }
    }

    public class UIView<TPresenter, TView>
        : UIView, IUIView<TPresenter, TView>
        where TPresenter : IPresenter<TView, TPresenter>
        where TView : UIView<TPresenter, TView>
    {
        protected TPresenter presenter;

        protected override void Awake()
        {
            base.Awake();

            base.Hide(false);
        }

        [Inject]
        public virtual void Construct(TPresenter presenter)
        {
            this.presenter = presenter;
            presenter.Initialize();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            presenter.Dispose();
        }
    }
}
