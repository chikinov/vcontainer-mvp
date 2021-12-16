using VContainer;
using VContainer.Unity.MVP;

namespace Example
{
    public class RootPresenter : Presenter<RootView, RootPresenter>
    {
        [Inject]
        public RootPresenter(RootView view) : base(view)
        {
        }
    }
}
