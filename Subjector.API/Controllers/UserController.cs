using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Subjector.API.Helpers;
using Subjector.API.Models;
using Subjector.API.Models.User;
using Subjector.Common;
using Subjector.Common.Models;
using Subjector.Data.Entities;

namespace Subjector.API.Controllers
{
    [ValidateModel]
    public class UserController : BaseController
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private HttpContext _context => _contextAccessor.HttpContext;

        public UserController(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        [AllowAnonymous]
        [HttpPost]
        public object Login([FromBody]LoginModel model)
        {
            User user = UserManager.Login(model.Email, model.Password, model.Role);
            if (user.Role == (int)Role.Professor)
                SecurityHelper.ValidateCertificate(_context, CurrentUser);
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

        [TokenAuthorize(Roles = "Professor")]
        [HttpGet]
        public List<UserModel> GetUsersRequests(int role)
        {
            var users = UserManager.GetUsersRequests(role);
            return users.Select(Mapper.Map).ToList();
        }

        [TokenAuthorize(Roles = "Professor")]
        [HttpPost]
        public void AcceptRequest(int userId)
        {
            UserManager.AcceptRequest(userId);
        }

        [TokenAuthorize(Roles = "Professor")]
        [HttpPost]
        public void DeleteRequest(int userId)
        {
            UserManager.DeleteRequest(userId);
        }

        [TokenAuthorize(Roles = "Professor")]
        [HttpPost]
        public void AddProfessor([FromBody]UserModel user)
        {
            UserManager.AddProfessor(user);
        }

        [TokenAuthorize]
        [HttpPost]
        public void ChangePassword([FromBody] PasswordModel password)
        {
            UserManager.ChangePassword(CurrentUser.Id, password.CurrentPassword, password.NewPassword);
        }
    }
}