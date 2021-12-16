namespace VContainer.Unity.MVP
{
    public interface IDataView<TData> : IView
    {
        public TData Data { get; set; }

        public void Refresh();
    }
}
