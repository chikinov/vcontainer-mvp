using UnityEngine;
using UnityEngine.Events;

namespace VContainer.Unity.MVP
{
    public class UIDataButtonListView<TData, TDataView> :
        UIDataListView<TData, TDataView>,
        IUIDataButtonListView<TData, TDataView>
        where TDataView : UIDataButtonView<TData>
    {
        [field: SerializeField]
        public UnityEvent<TData> OnClick { get; private set; }

        public override TDataView Add(TData data)
        {
            var view = base.Add(data);
            view.OnClick.AddListener(OnClickView);
            return view;
        }

        public override void Clear()
        {
            foreach (TDataView child in children)
                child.OnClick.RemoveListener(OnClickView);

            base.Clear();
        }

        private void OnClickView(TData data) => OnClick?.Invoke(data);
    }
}
