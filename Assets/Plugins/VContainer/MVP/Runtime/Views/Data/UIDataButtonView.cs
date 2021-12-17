using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace VContainer.Unity.MVP
{
    public abstract class UIDataButtonView<TData> :
        UIDataView<TData>, IUIDataButtonView<TData>
    {
        [SerializeField] private Button button;

        [field: SerializeField]
        public UnityEvent<TData> OnClick { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            if (!button && !TryGetComponent(out button)) return;

            button.onClick.AddListener(OnClickButton);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (button) button.onClick.RemoveListener(OnClickButton);
        }

        private void OnClickButton() => OnClick?.Invoke(Data);
    }
}
