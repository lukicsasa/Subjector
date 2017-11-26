using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Subjector.API.Helpers;
using Subjector.API.Models;
using Subjector.API.Models.User;
using Subjector.Common.Models;
using Subjector.Data.Entities;

namespace Subjector.API.Controllers
{
    [ValidateModel]
    public class UserController : BaseController
    {
        [AllowAnonymous]
        [HttpPost]
        public object Login([FromBody]LoginModel model)
        {
            User user = UserManager.Login(model.Email, model.Password);
            var userModel = Mapper.Map(user);
            return new { User = userModel, Token = SecurityHelper.CreateLoginToken(user) };
        }

        [AllowAnonymous]
        [HttpPost]
        public UserModel Register([FromBody]UserModel userModel)
        {
            User user = UserManager.Register(userModel);
            UserModel model = Mapper.Map(user);
            return model;
        }

        [TokenAuthorize]
        [HttpGet]
        public List<UserModel> GetUsersRequests(int role)
        {
            var users = UserManager.GetUsersRequests(role);
            return users.Select(Mapper.Map).ToList();
        }

        [TokenAuthorize]
        [HttpPost]
        public void AcceptRequest(int userId)
        {
            UserManager.AcceptRequest(userId);
        }

        [TokenAuthorize]
        [HttpPost]
        public void DeleteRequest(int userId)
        {
            UserManager.DeleteRequest(userId);
        }
    }
}