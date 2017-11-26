using Subjector.API.Models;
using Subjector.Core;

namespace Subjector.API.Controllers
{
    public class BaseController
    {
        internal UserJwtModel CurrentUser { get; set; }

        private UserManager _userManager;
        protected UserManager UserManager => _userManager ?? (_userManager = new UserManager());
    }
}
