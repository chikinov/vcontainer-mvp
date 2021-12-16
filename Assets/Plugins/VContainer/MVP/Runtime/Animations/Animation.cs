using System;
using UnityEngine;

namespace VContainer.Unity.MVP
{
    public abstract class Animation : MonoBehaviour, IAnimation
    {
        protected Action onComplete;

        public abstract bool IsDone { get; }

        public abstract IAnimation Play();

        public abstract void Stop();

        public abstract void Complete();

        public virtual IAnimation OnComplete(Action callback)
        {
            if (IsDone) callback?.Invoke();
            else onComplete += callback;

            return this;
        }
    }
}
