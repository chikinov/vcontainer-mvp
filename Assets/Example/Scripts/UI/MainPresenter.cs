using VContainer;
using VContainer.Unity.MVP;

namespace Example
{
    public class MainPresenter : Presenter<MainView, MainPresenter>
    {
        [Inject]
        public MainPresenter(MainView view) : base(view)
        {
        }
    }
}
