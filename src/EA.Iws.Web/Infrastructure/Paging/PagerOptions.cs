/*
 * Modified from https://github.com/martijnboland/MvcPaging
 * 
 * The MIT license
 * 
 * Copyright (c) 2008-2016 Martijn Boland, Bart Lenaerts, Rajeesh CV
 * 
 * Permission is hereby granted, free of charge, to any person obtaining
 * a copy of this software and associated documentation files (the
 * "Software"), to deal in the Software without restriction, including
 * without limitation the rights to use, copy, modify, merge, publish,
 * distribute, sublicense, and/or sell copies of the Software, and to
 * permit persons to whom the Software is furnished to do so, subject to
 * the following conditions:
 * 
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
 * MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
 * LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
 * OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
 * WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 * 
 */
namespace EA.Iws.Web.Infrastructure.Paging
{
    using System.Web.Routing;

    public class PagerOptions
    {
        private static class DefaultDefaults
        {
            public const string DefaultPageRouteValueKey = "page";
        }

        /// <summary>
        /// The static Defaults class allows you to set Pager defaults for the entire application.
        /// Set values at application startup.
        /// </summary>
        public static class Defaults
        {
            public static string DefaultPageRouteValueKey = DefaultDefaults.DefaultPageRouteValueKey;

            public static void Reset()
            {
                DefaultPageRouteValueKey = DefaultDefaults.DefaultPageRouteValueKey;
            }
        }

        public RouteValueDictionary RouteValues { get; internal set; }

        public string Action { get; internal set; }

        public string PageRouteValueKey { get; set; }

        public PagerOptions()
        {
            RouteValues = new RouteValueDictionary();
            PageRouteValueKey = Defaults.DefaultPageRouteValueKey;
        }
    }
}