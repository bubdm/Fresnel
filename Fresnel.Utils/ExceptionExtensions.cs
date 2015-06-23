using System;
using System.Collections.Generic;
using System.Linq;

namespace Envivo.Fresnel.Utils
{

    public static class ExceptionExtensions
    {
        public static IEnumerable<Exception> FlattenAll(this Exception ex)
        {
            var allExceptions = new List<Exception>();
            Flatten(ex, allExceptions);
            return allExceptions;
        }

        private static void Flatten(Exception exception, List<Exception> existingExceptions)
        {
            var aggregateException = exception as AggregateException;
            if (aggregateException != null)
            {
                foreach (var ex in aggregateException.InnerExceptions)
                {
                    Flatten(ex, existingExceptions);
                }
            }
            else
            {
                existingExceptions.Add(exception);
            }
        }

        /// <summary>
        /// Returns a single message containing all of the errors in the given AggregateException
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static string ToSingleMessage(this AggregateException exception)
        {
            var exceptions = exception.Flatten().InnerExceptions;

            var messages = exceptions.Select(e => e.Message).ToArray();

            var result = string.Join(Environment.NewLine, messages);

            return result;
        }

    }
}