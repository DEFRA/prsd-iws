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
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using System.Web.Routing;

    /// <summary>
    /// Pager options builder class. Enables a fluent interface for adding options to the pager.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass",
        Justification = "File contains generic and non-generic of the same class.")]
    public class PagerOptionsBuilder
    {
        protected PagerOptions pagerOptions;

        public PagerOptionsBuilder(PagerOptions pagerOptions)
        {
            this.pagerOptions = pagerOptions;
        }

        /// <summary>
        /// Set the action name for the pager links. Note that we're always using the current controller.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public PagerOptionsBuilder Action(string action)
        {
            if (action != null)
            {
                if (pagerOptions.RouteValues.ContainsKey("action"))
                {
                    throw new ArgumentException("The valuesDictionary already contains an action.", "action");
                }
                pagerOptions.RouteValues.Add("action", action);
                pagerOptions.Action = action;
            }
            return this;
        }

        /// <summary>
        /// Add a custom route value parameter for the pager links.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public PagerOptionsBuilder AddRouteValue(string name, object value)
        {
            pagerOptions.RouteValues[name] = value;
            return this;
        }

        /// <summary>
        /// Set custom route value parameters for the pager links.
        /// </summary>
        /// <param name="routeValues"></param>
        /// <returns></returns>
        public PagerOptionsBuilder RouteValues(object routeValues)
        {
            RouteValues(new RouteValueDictionary(routeValues));
            return this;
        }

        /// <summary>
        /// Set custom route value parameters for the pager links.
        /// </summary>
        /// <param name="routeValues"></param>
        /// <returns></returns>
        public PagerOptionsBuilder RouteValues(RouteValueDictionary routeValues)
        {
            if (routeValues == null)
            {
                throw new ArgumentException("routeValues may not be null", "routeValues");
            }
            this.pagerOptions.RouteValues = routeValues;
            if (!string.IsNullOrWhiteSpace(pagerOptions.Action) && !pagerOptions.RouteValues.ContainsKey("action"))
            {
                pagerOptions.RouteValues.Add("action", pagerOptions.Action);
            }
            return this;
        }

        /// <summary>
        /// Set the page routeValue key for pagination links
        /// </summary>
        /// <param name="pageRouteValueKey"></param>
        /// <returns></returns>
        public PagerOptionsBuilder PageRouteValueKey(string pageRouteValueKey)
        {
            if (pageRouteValueKey == null)
            {
                throw new ArgumentException("pageRouteValueKey may not be null", "pageRouteValueKey");
            }
            this.pagerOptions.PageRouteValueKey = pageRouteValueKey;
            return this;
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass",
        Justification = "File contains generic and non-generic of the same class.")]
    public class PagerOptionsBuilder<TModel> : PagerOptionsBuilder
    {
        private readonly HtmlHelper<TModel> htmlHelper;

        public PagerOptionsBuilder(PagerOptions pagerOptions, HtmlHelper<TModel> htmlHelper)
            : base(pagerOptions)
        {
            this.htmlHelper = htmlHelper;
        }

        /// <summary>
        /// Adds a strongly typed route value parameter based on the current model.
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression"></param>
        /// <example>AddRouteValueFor(m => m.SearchQuery)</example>
        /// <returns></returns>
        public PagerOptionsBuilder<TModel> AddRouteValueFor<TProperty>(Expression<Func<TModel, TProperty>> expression)
        {
            var name = ExpressionHelper.GetExpressionText(expression);
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);

            AddRouteValue(name, metadata.Model);

            return this;
        }

        /// <summary>
        /// Set the action name for the pager links. Note that we're always using the current controller.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public new PagerOptionsBuilder<TModel> Action(string action)
        {
            base.Action(action);
            return this;
        }

        /// <summary>
        /// Add a custom route value parameter for the pager links.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public new PagerOptionsBuilder<TModel> AddRouteValue(string name, object value)
        {
            base.AddRouteValue(name, value);
            return this;
        }

        /// <summary>
        /// Set custom route value parameters for the pager links.
        /// </summary>
        /// <param name="routeValues"></param>
        /// <returns></returns>
        public new PagerOptionsBuilder<TModel> RouteValues(object routeValues)
        {
            base.RouteValues(routeValues);
            return this;
        }

        /// <summary>
        /// Set custom route value parameters for the pager links.
        /// </summary>
        /// <param name="routeValues"></param>
        /// <returns></returns>
        public new PagerOptionsBuilder<TModel> RouteValues(RouteValueDictionary routeValues)
        {
            base.RouteValues(routeValues);
            return this;
        }

        /// <summary>
        /// Set the page routeValue key for pagination links
        /// </summary>
        /// <param name="pageRouteValueKey"></param>
        /// <returns></returns>
        public new PagerOptionsBuilder<TModel> PageRouteValueKey(string pageRouteValueKey)
        {
            if (pageRouteValueKey == null)
            {
                throw new ArgumentException("pageRouteValueKey may not be null", "pageRouteValueKey");
            }
            this.pagerOptions.PageRouteValueKey = pageRouteValueKey;
            return this;
        }
    }
}