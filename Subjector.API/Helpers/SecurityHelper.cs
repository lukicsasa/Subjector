﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Remotion.Linq.Clauses;
using StackExchange.Redis;
using Subjector.API.Models;
using Subjector.Common;
using Subjector.Common.Exceptions;
using Subjector.Data;
using Subjector.Data.Entities;

namespace Subjector.API.Helpers
{
    public static class SecurityHelper
    {
        private const string SecretKey =
                "f2HDuRfAb7zN75E9BmtvR5MmdjVgV79ddaxTJEdwpNHfEVHq5uDk3CASmrrxqQE6KVCXgRZhD3CRPd2QFwuB5xUdHGgRZEmr6SN8YMX2hEYytXyuLd6dEPb7dPFTm8vHdBLQSkB5YzLEACPDzT6QVE8HrDHQ9FQbhQpkuC2TUk3wjfffdK8WDEDQzKSXjyLX4r66zgptavd29AeU4KFsZmuFvbYxPq4yEcCXmUhPVmKD9BeHv2vNQ9Gb73AXKeW4";

        public static string CreateLoginToken(User user)
        {
            UserJwtModel userModel = new UserJwtModel
            {
                Email = user.Email,
                Id = user.Id,
                Role = user.Role,
                ExpirationDate = DateTime.UtcNow.AddDays(1),
            };

            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(new HMACSHA256Algorithm(), serializer, urlEncoder);
            string token = encoder.Encode(userModel, SecretKey);
            return token;
        }

        public static T Decode<T>(string value)
        {
            IJsonSerializer serializer = new JsonNetSerializer();
            IDateTimeProvider provider = new UtcDateTimeProvider();
            IJwtValidator validator = new JwtValidator(serializer, provider);
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);
            return decoder.DecodeToObject<T>(value);
        }

        public static void ValidateCertificate(HttpContext httpContext, UserJwtModel currentUser)
        {
            var cert = httpContext.Connection.ClientCertificate;
            if (cert == null)
                throw new ValidationException("Certificate not provided!");

            var certNumber = cert.SerialNumber;

            if (cert.Archived)
                throw new ValidationException("Certificate has been disabled, please contact support!");
            if (!cert.Verify())
                throw new ValidationException("Invalid certificate, please contact support!");

            using (var uow = new UnitOfWork())
            {
                var user = uow.UserRepository.Get(currentUser.Id);
                if (user.Cert.All(a => a.CertNumber != certNumber))
                {
                    throw new ValidationException("Invalid certificate, please contact support!");
                }
            }
        }
    }
}
