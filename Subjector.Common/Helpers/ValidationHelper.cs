using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Subjector.Common.Exceptions;

namespace Subjector.Common.Helpers
{
    public static class ValidationHelper
    {
        public static void ValidateNotNull<T>(T entity) where T : class
        {
            string entityName = typeof(T).Name.ToSentenceCase();

            if (entity == null)
            {
                throw new ValidationException($"{entityName} does not exist!");
            }
        }

        public static void ValidateEntityExist<T>(T entity) where T : class
        {
            string entityName = typeof(T).Name.ToSentenceCase();

            if (entity != null)
            {
                throw new ValidationException($"{entityName} already exists!");
            }
        }

        public static string ToSentenceCase(this string str)
        {
            return Regex.Replace(str, "[a-z][A-Z]", m => m.Value[0] + " " + char.ToLower(m.Value[1]));
        }

    }
}
