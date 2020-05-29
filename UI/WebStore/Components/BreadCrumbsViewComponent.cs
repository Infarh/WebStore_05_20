using Microsoft.AspNetCore.Mvc;

namespace WebStore.Components
{
    public class BreadCrumbsViewComponent : ViewComponent
    {

        public IViewComponentResult Invoke() => View();
    }
}
