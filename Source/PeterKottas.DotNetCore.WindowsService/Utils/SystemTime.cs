﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PeterKottas.DotNetCore.WindowsService.Utils
{
    public static class SystemTime
    {
        /// <summary>
        /// Return current UTC time via <see cref="Func{TResult}" />. Allows easier unit testing.
        /// </summary>
        public static Func<DateTimeOffset> UtcNow = () => DateTimeOffset.UtcNow;

        /// <summary>
        /// Return current time in current time zone via <see cref="Func&lt;T&gt;" />. Allows easier unit testing.
        /// </summary>
        public static Func<DateTimeOffset> Now = () => DateTimeOffset.Now;
    }
}
