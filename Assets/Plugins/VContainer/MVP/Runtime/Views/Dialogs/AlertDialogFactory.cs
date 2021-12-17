using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;

namespace VContainer.Unity.MVP
{
    public class AlertDialogFactory : IAsyncStartable
    {
        private readonly Settings settings;

        private readonly Stack<AlertDialog> pool = new Stack<AlertDialog>();

        private readonly SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);

        private IUIView parentView;

        private AlertDialog prefab;

        private bool initialized;

        public AlertDialogFactory(Settings settings)
        {
            this.settings = settings;
        }

        public async UniTask StartAsync(CancellationToken cancellation = default)
        {
            await semaphoreSlim.WaitAsync();

            try
            {
                if (initialized) return;

                await LoadParentViewAsync();
                await LoadPoolAsync();

                initialized = true;
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }

        private async Task LoadParentViewAsync()
        {
            var parentViewRef = settings.ParentViewRef;
            if (parentViewRef == null) return;

            var handle = parentViewRef.InstantiateAsync();
            await handle.Task;

            var gameObject = handle.Result;
            if (!(gameObject.TryGetComponent<IUIView>(out parentView)))
                throw new Exception("Failed to load parent view");
        }

        private async Task LoadPoolAsync()
        {
            var handle = settings.AlertDialogRef.LoadAssetAsync();
            await handle.Task;

            var gameObject = handle.Result;
            if (!(gameObject.TryGetComponent<AlertDialog>(out prefab)))
                throw new Exception("Failed to load prefab");

            for (var i = 0; i < settings.InitialSize; i++)
            {
                var alertDialog = UnityEngine.Object.Instantiate(prefab);
                alertDialog.Parent = parentView;
                pool.Push(alertDialog);
            }
        }

        public virtual AlertDialog Create(
            string title,
            string message,
            string positiveText,
            string negativeText)
        {
            var alertDialog = pool.Pop();
            alertDialog.Title = title;
            alertDialog.Message = message;
            alertDialog.PositiveText = positiveText;
            alertDialog.NegativeText = negativeText;

            UnityAction action = null;
            action = new UnityAction(
                () =>
                {
                    alertDialog.OnCompleteHide.RemoveListener(action);
                    pool.Push(alertDialog);
                });
            alertDialog.OnCompleteHide.AddListener(action);

            return alertDialog;
        }

        [Serializable]
        public class Settings
        {
            [field: SerializeField]
            public int InitialSize { get; private set; }

            [field: SerializeField]
            public AssetReferenceGameObject ParentViewRef { get; private set; }

            [field: SerializeField]
            public AssetReferenceGameObject AlertDialogRef { get; private set; }
        }
    }
}
