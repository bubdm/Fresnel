using System;
using System.Linq.Expressions;

namespace Envivo.Fresnel.DomainTypes.Utils
{

    public static class LambdaExtensions
    {
        /// <summary>
        /// Returns the member's name for the given Lambda expression
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static string NameOf<T>(Expression<Func<T>> expression)
        {
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression != null)
            {
                return memberExpression.Member.Name;
            }

            var methodExpression = expression.Body as MethodCallExpression;
            if (methodExpression != null)
            {
                return methodExpression.Method.Name;
            }

            throw new ArgumentOutOfRangeException("expression", "Unable to determine member/method name for given expression");
        }

    }

}
