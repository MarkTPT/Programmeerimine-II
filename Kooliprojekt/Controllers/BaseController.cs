using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.Controllers
{
    public abstract class BaseController : Controller
    {
        protected void AddModelErrors(OperationResponse response)
        {
            foreach (var key in response.Errors.Keys)
            {
                ModelState.AddModelError(key, response.Errors[key]);
            }
        }
    }
}
