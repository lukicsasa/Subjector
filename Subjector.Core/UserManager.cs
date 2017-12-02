using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
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
        public User Login(string email, string password, int role)
        {
            using (var uow = new UnitOfWork())
            {
                var user = uow.UserRepository.Find(u => u.Email.ToLower().Trim() == email.ToLower().Trim());

                if (user == null) throw new ValidationException("Wrong email or password!");
                if (user.Role == (int)Role.PendingStudent) throw new ValidationException("You are not approved yet!");
                if (!PasswordHelper.ValidatePassword(password, user.Password))
                    throw new ValidationException("Wrong email or password!");
                if (role != user.Role && user.Role != (int)Role.Admin) throw new ValidationException($"You can't login as {(Role)role}!");
                return user;
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
                var pass = PasswordHelper.RandomPasswordGenerator(8);
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
                    mailMessage.Subject = "Subjector approval";
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

        public void AddProfessor(UserModel user)
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
                    Role = (int)Role.Professor,
                };

                var pass = PasswordHelper.RandomPasswordGenerator(8);
                newUser.Password = PasswordHelper.CreateHash(pass);

                var cert = CertHelper.GenerateCertificate();

                newUser.Cert.Add(new Cert
                {
                    Active = true,
                    CertNumber = cert.SerialNumber
                });

                uow.UserRepository.Add(newUser);
                uow.Save();

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
                    mailMessage.Attachments.Add(new Attachment(cert.Path));
                    mailMessage.Body = $"Hello {user.FirstName} {user.LastName}, \n \n You have been added into Subjector project. \n You will need to install certificate we've sent you as attachment. \n \n Here is your password: {pass} \n \n We would advice you to change password when you log in.";
                    mailMessage.Subject = "Subjector invitation";
                    Task.Run(() => client.Send(mailMessage));
                }
                catch (Exception e)
                {
                    throw new ValidationException(e.Message);
                }
            }
        }

        public void ChangePassword(int currentUserId, string currentPassword, string newPassword)
        {
            using (var uow = new UnitOfWork())
            {
                var user = uow.UserRepository.Get(currentUserId);
                ValidationHelper.ValidateNotNull(user);

                if(!PasswordHelper.ValidatePassword(currentPassword, user.Password)) 
                    throw new ValidationException("Incorrect current password!");

                user.Password = PasswordHelper.CreateHash(newPassword);
                uow.Save();
            }

        }
    }
}
