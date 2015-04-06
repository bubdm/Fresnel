using System;
using System.Linq.Expressions;

namespace Envivo.Fresnel.Utils
{
    public static class LambdaExtensions
    {
        /// <summary>
        /// Returns the member's name for the given Lambda expression
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        public static string NameOf<T>(Expression<Func<T>> expression)
        {
            return GetName(expression as LambdaExpression);
        }

        public static string NameOf<T>(Expression<Func<T, object>> expression)
        {
            return GetName(expression as LambdaExpression);
        }

        public static string NameOf<T>(Expression<Action<T>> expression)
        {
            return GetName(expression as LambdaExpression);
        }

        public static string NameOf(Expression<Action> expression)
        {
            return GetName(expression as LambdaExpression);
        }

        private static string GetName(LambdaExpression expression)
        {
            if (expression == null)
            {
                throw new ArgumentOutOfRangeException("expression", "Unable to determine Lambda expression");
            }

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