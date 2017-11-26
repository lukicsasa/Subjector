using System;
using Subjector.Common;
using Subjector.Common.Exceptions;
using Subjector.Common.Helpers;
using Subjector.Common.Models;
using Subjector.Data;
using Subjector.Data.Entities;

namespace Subjector.Core
{
    public class UserManager
    {
        public User Login(string email, string password)
        {
            using (var uow = new UnitOfWork())
            {
                if (!string.IsNullOrWhiteSpace(email))
                {
                    var user = uow.UserRepository.Find(u => u.Email.ToLower().Trim() == email.ToLower().Trim());

                    if (user == null) throw new ValidationException("Wrong email or password!");

                    if (!PasswordHelper.ValidatePassword(password, user.Password))
                        throw new ValidationException("Wrong email or password!");
                    return user;
                }

                throw new ValidationException("You must provide login data!");
            }
        }

        public User Register(UserModel user)
        {
            using (var uow = new UnitOfWork())
            {
                if (!string.IsNullOrEmpty(user.Email))
                {
                    var existingUsers = uow.UserRepository.Find(u => u.Email == user.Email);
                    if (existingUsers != null)
                    {
                        throw new ValidationException("Email is already taken!");
                    }
                }

                var newUser = new User
                {
                    DateCreated = DateTime.UtcNow,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Role = (int)Role.PendingStudent,
                    RefCode = user.RefCode
                };

                uow.UserRepository.Add(newUser);
                uow.Save();
                return newUser;
            }
        }
    }
}
