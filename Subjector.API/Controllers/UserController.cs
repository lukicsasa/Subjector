using Microsoft.AspNetCore.Mvc;
using Subjector.Data;
using Subjector.Data.Entities;

namespace Subjector.API.Controllers
{
    public class UserController : Controller
    {
        [HttpGet]
        public string Index()
        {
            return "Running...";
        }
    }
}