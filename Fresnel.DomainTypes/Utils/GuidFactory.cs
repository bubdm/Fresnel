
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System;

namespace Fresnel.DomainTypes.Utils
{

    /// <summary>
    /// A set of methods to create GUID values
    /// </summary>
    public static class GuidFactory
    {
        [DllImport("Rpcrt4.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int UuidCreateSequential(ref Guid guid);

        /// <summary>
        /// Generates a Sequential Guid using Rpcrt4.DLL. The resulting GUID performs very well with SQL Server.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// UuidCreateSequential isn't directly compatible with SQL Server 2005's NewSequentialID()
        /// See http://codebetter.com/blogs/scott.bellware/archive/2006/12/27/156671.aspx
        /// </remarks>
        public static Guid NewSequentialGuid()
        {
            const int RPC_S_OK = 0;

            var g = new Guid();
            var result = UuidCreateSequential(ref g);

            if (result == RPC_S_OK)
            {
                // Transform the Guid to be SQL 2005+ compatible:
                var pre = g.ToByteArray();
                var post = g.ToByteArray();

                post[0] = pre[3];
                post[1] = pre[2];
                post[2] = pre[1];
                post[3] = pre[0];

                post[4] = pre[5];
                post[5] = pre[4];

                post[6] = pre[7];
                post[7] = pre[6];

                return new Guid(post);
            }
            else
            {
                // There's no network attached, so the guid could be duplicated by another machine.
                // We'll use the best alternative (at this time):
                return NewCombGuid();
            }
        }

        public static Guid NewCombGuid()
        {
            var bytes = Guid.NewGuid().ToByteArray();

            var baseDate = new DateTime(1900, 1, 1);
            var now = DateTime.Now;

            // Get the days and milliseconds which will be used to build the byte string:
            var days = new TimeSpan(DateTime.Now.Ticks - baseDate.Ticks);
            var msecs = new TimeSpan(DateTime.Now.Ticks - (new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).Ticks));

            // Convert to a byte array:
            var daysArray = BitConverter.GetBytes(days.Days);
            var msecsArray = BitConverter.GetBytes((long)msecs.TotalMilliseconds / 3.333333);

            // Reverse the bytes to match SQL Servers ordering:
            Array.Reverse(daysArray);
            Array.Reverse(msecsArray);

            // Copy the bytes into the Guid:
            Array.Copy(daysArray, daysArray.Length - 2, bytes, bytes.Length - 6, 2);
            Array.Copy(msecsArray, msecsArray.Length - 4, bytes, bytes.Length - 4, 4);

            return new Guid(bytes);
        }

        public static DateTime GetDateFromComb(Guid comb)
        {
            var daysArray = new byte[5];
            var msecsArray = new byte[5];
            var bytes = comb.ToByteArray();

            // Copy the date parts of the guid to the respective byte arrays.
            Array.Copy(bytes, bytes.Length - 6, daysArray, 2, 2);
            Array.Copy(bytes, bytes.Length - 4, msecsArray, 0, 4);

            // Reverse the arrays to put them into the appropriate order
            Array.Reverse(daysArray);
            Array.Reverse(msecsArray);

            // Convert the bytes to ints
            var days = BitConverter.ToInt32(daysArray, 0);
            var msecs = BitConverter.ToInt32(msecsArray, 0);

            var basedate = new DateTime(1900, 1, 1);
            var date = basedate.AddDays(days);
            date = date.AddMilliseconds(msecs * 3.333333);

            return date;
        }

    }

}
