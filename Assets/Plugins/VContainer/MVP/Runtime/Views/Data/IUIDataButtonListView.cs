using System;

namespace VContainer.Unity.MVP
{
    public interface IUIDataButtonListView<TData, TDataView> :
        IDataListView<TData, TDataView>
        where TDataView : IUIDataButtonView<TData>
    {
        public event EventHandler<UIDataView<TData>.EventArgs> OnClick;
    }
}
