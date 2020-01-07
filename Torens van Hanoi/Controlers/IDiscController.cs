using TowersOfHanoi.Views;

namespace TowersOfHanoi.Controllers
{
    public interface IDiscController
    {
        DiscPanel[] GetDiscs();
        DiscPanel GetLastDiscFromDiscParent(DiscPanel disc);
    }
}