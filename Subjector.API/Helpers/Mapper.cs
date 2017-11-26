using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Subjector.API.Models;
using Subjector.API.Models.User;
using Subjector.Common.Models;
using Subjector.Data.Entities;

namespace Subjector.API.Helpers
{
    public class Mapper
    {
        public static TDestination AutoMap<TSource, TDestination>(TSource source)
            where TDestination : class
            where TSource : class
        {
            var config = new MapperConfiguration(c => c.CreateMap<TSource, TDestination>());
            var mapper = config.CreateMapper();
            var result = mapper.Map<TDestination>(source);

            return result;
        }

        public static UserModel Map(User user)
        {
            if (user == null) return null;

            var userModel = new UserModel
            {
                Id = user.Id,
                Email = user.Email,
                DateCreated = user.DateCreated,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
            return userModel;
        }
    }
}
