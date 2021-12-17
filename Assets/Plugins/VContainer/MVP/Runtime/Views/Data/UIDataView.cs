namespace VContainer.Unity.MVP
{
    public abstract class UIDataView<TData> : UIView, IDataView<TData>
    {
        private TData data;
        public virtual TData Data
        {
            get => data;
            set
            {
                data = value;

                Refresh();
            }
        }

        public abstract void Refresh();
    }
}
