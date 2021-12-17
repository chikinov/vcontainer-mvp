using UnityEngine.Events;

namespace VContainer.Unity.MVP
{
    public interface IUIDataButtonListView<TData, TDataView> :
        IDataListView<TData, TDataView>
        where TDataView : IUIDataButtonView<TData>
    {
        UnityEvent<TData> OnClick { get; }
    }
}
