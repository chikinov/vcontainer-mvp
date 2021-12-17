using UnityEngine.Events;

namespace VContainer.Unity.MVP
{
    public interface IUIDataButtonView<TData> : IDataView<TData>
    {
        UnityEvent<TData> OnClick { get; }
    }
}
