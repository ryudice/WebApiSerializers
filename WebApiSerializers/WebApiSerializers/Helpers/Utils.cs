using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WebApiSerializers.Helpers
{
    internal static class Utils
    {
        internal static string GetPropertyNameFromExpression<T, T2>(Expression<Func<T, T2>> expression)
        {
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression != null)
                return memberExpression.Member.Name;

            throw new ArgumentException("Not a valid member expression", "property");
        }
    }
}
