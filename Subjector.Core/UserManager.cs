using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
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

                if (uow.UserRepository.Any(a => a.RefCode == user.RefCode))
                {
                    throw new ValidationException("Account with that ref code already exists!");
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

        public List<User> GetUsersRequests(int role)
        {
            using (var uow = new UnitOfWork())
            {
                return uow.UserRepository.FindAll(a => !a.Archived && a.Role == role).ToList();
            }
        }

        public void AcceptRequest(int userId)
        {
            using (var uow = new UnitOfWork())
            {
                var user = uow.UserRepository.Find(a => a.Id == userId);
                if (user == null)
                    throw new ValidationException("User doesn't exist");
                if (user.Role != (int)Role.PendingStudent)
                    throw new ValidationException("User is already active");

                user.Role = (int)Role.Student;
                var pass = PasswordHelper.RandomPasswordGenerator(6);
                user.Password = PasswordHelper.CreateHash(pass);
                try
                {
                    SmtpClient client = new SmtpClient("smtp.gmail.com")
                    {
                        UseDefaultCredentials = false,
                        EnableSsl = true,
                        Credentials = new NetworkCredential("lukic.sasa11@gmail.com", "Mementomori1!")
                    };

                    MailMessage mailMessage = new MailMessage { From = new MailAddress("lukic.sasa11@gmail.com", "Subjector Customer Support") };
                    mailMessage.To.Add(user.Email);
                    mailMessage.Body = $"Hello {user.FirstName} {user.LastName}, \n \n your request to join Subjector has been approved. \n Here is your password: {pass} \n \n We would advice you to change password when you log in.";
                    mailMessage.Subject = "subject";
                    client.Send(mailMessage);
                }
                catch (Exception e)
                {
                    throw new ValidationException(e.Message);
                }

                uow.Save();
            }
        }

        public void DeleteRequest(int userId)
        {
            using (var uow = new UnitOfWork())
            {
                var user = uow.UserRepository.Find(a => a.Id == userId);
                if (user == null)
                    throw new ValidationException("User doesn't exist");
                if (user.Role != (int)Role.PendingStudent)
                    throw new ValidationException("User is already active");

                uow.UserRepository.Delete(user);
                uow.Save();
            }
        }
    }
}
