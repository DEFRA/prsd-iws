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
    using System.Collections;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass",
        Justification = "File contains generic and non-generic of the same class.")]
    public class Pager : IHtmlString
    {
        private readonly int currentPage;
        private readonly HtmlHelper htmlHelper;
        protected readonly PagerOptions PagerOptions;
        private readonly int pageSize;
        private readonly int totalItemCount;

        private const int MaxNumberOfPages = 10;
        private const string PreviousPageText = "Prev";
        private const string PreviousPageTitle = "Previous page";
        private const string NextPageText = "Next";
        private const string NextPageTitle = "Next page";

        public Pager(HtmlHelper htmlHelper, int pageSize, int currentPage, int totalItemCount)
        {
            this.htmlHelper = htmlHelper;
            this.pageSize = pageSize;
            this.currentPage = currentPage;
            this.totalItemCount = totalItemCount;
            PagerOptions = new PagerOptions();
        }

        public virtual string ToHtmlString()
        {
            var model = BuildPaginationModel(GeneratePageUrl);

            var sb = new StringBuilder();

            sb.Append("<div class=\"pager\">");
            sb.Append("<div class=\"pager-controls\">");
            sb.Append("<ul class=\"pager-items\">");

            foreach (var paginationLink in model.PaginationLinks)
            {
                if (paginationLink.Active)
                {
                    if (paginationLink.IsCurrent || !paginationLink.PageIndex.HasValue)
                    {
                        sb.AppendFormat("<li>{0}</li>", paginationLink.DisplayText);
                    }
                    else
                    {
                        var linkBuilder = new TagBuilder("a");
                        linkBuilder.MergeAttribute("href", paginationLink.Url);
                        linkBuilder.MergeAttribute("title", paginationLink.DisplayTitle);
                        linkBuilder.SetInnerText(paginationLink.DisplayText);

                        sb.AppendFormat("<li>{0}</li>", linkBuilder.ToString());
                    }
                }
                else
                {
                    sb.AppendFormat("<li>{0}</li>", paginationLink.DisplayText);
                }
            }

            sb.Append("</ul>");
            sb.Append("</div>");
            sb.Append(PagerSummary());
            sb.Append("</div>");

            return sb.ToString();
        }

        public Pager Options(Action<PagerOptionsBuilder> buildOptions)
        {
            buildOptions(new PagerOptionsBuilder(PagerOptions));
            return this;
        }

        public virtual PaginationModel BuildPaginationModel(Func<int, string> generateUrl)
        {
            int pageCount = (int)Math.Ceiling(totalItemCount / (double)pageSize);

            var model = new PaginationModel
            {
                PageSize = pageSize,
                CurrentPage = currentPage,
                TotalItemCount = totalItemCount,
                PageCount = pageCount
            };

            // Previous page
            if (currentPage != 1)
            {
                var previousPageText = PreviousPageText;
                model.PaginationLinks.Add(currentPage > 1
                    ? new PaginationLink
                    {
                        Active = true,
                        DisplayText = previousPageText,
                        DisplayTitle = PreviousPageTitle,
                        PageIndex = currentPage - 1,
                        Url = generateUrl(currentPage - 1)
                    }
                    : new PaginationLink { Active = false, DisplayText = previousPageText });
            }

            var start = 1;
            var end = pageCount;
            var numberOfPagesToDisplay = MaxNumberOfPages;

            if (pageCount > numberOfPagesToDisplay)
            {
                var middle = (int)Math.Ceiling(numberOfPagesToDisplay / 2d) - 1;
                var below = (currentPage - middle);
                var above = (currentPage + middle);

                if (below < 2)
                {
                    above = numberOfPagesToDisplay;
                    below = 1;
                }
                else if (above > (pageCount - 2))
                {
                    above = pageCount;
                    below = (pageCount - numberOfPagesToDisplay + 1);
                }

                start = below;
                end = above;
            }

            if (start > 1)
            {
                model.PaginationLinks.Add(new PaginationLink
                {
                    Active = true,
                    PageIndex = 1,
                    DisplayText = "1",
                    Url = generateUrl(1)
                });
                if (start > 3)
                {
                    model.PaginationLinks.Add(new PaginationLink
                    {
                        Active = true,
                        PageIndex = 2,
                        DisplayText = "2",
                        Url = generateUrl(2)
                    });
                }
                if (start > 2)
                {
                    model.PaginationLinks.Add(new PaginationLink { Active = false, DisplayText = "..." });
                }
            }

            for (var i = start; i <= end; i++)
            {
                if (i == currentPage || (currentPage <= 0 && i == 1))
                {
                    model.PaginationLinks.Add(new PaginationLink
                    {
                        Active = true,
                        PageIndex = i,
                        IsCurrent = true,
                        DisplayText = i.ToString()
                    });
                }
                else
                {
                    model.PaginationLinks.Add(new PaginationLink
                    {
                        Active = true,
                        PageIndex = i,
                        DisplayText = i.ToString(),
                        Url = generateUrl(i)
                    });
                }
            }

            if (end < pageCount)
            {
                if (end < pageCount - 1)
                {
                    model.PaginationLinks.Add(new PaginationLink { Active = false, DisplayText = "..." });
                }
                if (end < pageCount - 2)
                {
                    model.PaginationLinks.Add(new PaginationLink
                    {
                        Active = true,
                        PageIndex = pageCount,
                        DisplayText = (pageCount).ToString(),
                        Url = generateUrl(pageCount)
                    });
                }
            }

            // Next page
            if (currentPage != pageCount)
            {
                var nextPageText = NextPageText;
                model.PaginationLinks.Add(currentPage < pageCount
                    ? new PaginationLink
                    {
                        Active = true,
                        PageIndex = currentPage + 1,
                        DisplayText = nextPageText,
                        DisplayTitle = NextPageTitle,
                        Url = generateUrl(currentPage + 1)
                    }
                    : new PaginationLink { Active = false, DisplayText = nextPageText });
            }

            model.Options = PagerOptions;
            return model;
        }

        protected virtual string GeneratePageUrl(int pageNumber)
        {
            var viewContext = htmlHelper.ViewContext;
            var routeDataValues = viewContext.RequestContext.RouteData.Values;
            RouteValueDictionary pageLinkValueDictionary;

            // Avoid canonical errors when pageNumber is equal to 1.
            if (pageNumber == 1)
            {
                pageLinkValueDictionary = new RouteValueDictionary(PagerOptions.RouteValues);

                if (routeDataValues.ContainsKey(PagerOptions.PageRouteValueKey))
                {
                    routeDataValues.Remove(PagerOptions.PageRouteValueKey);
                }
            }
            else
            {
                pageLinkValueDictionary = new RouteValueDictionary(PagerOptions.RouteValues)
                {
                    { PagerOptions.PageRouteValueKey, pageNumber }
                };
            }

            // To be sure we get the right route, ensure the controller and action are specified.
            if (!pageLinkValueDictionary.ContainsKey("controller") && routeDataValues.ContainsKey("controller"))
            {
                pageLinkValueDictionary.Add("controller", routeDataValues["controller"]);
            }

            if (!pageLinkValueDictionary.ContainsKey("action") && routeDataValues.ContainsKey("action"))
            {
                pageLinkValueDictionary.Add("action", routeDataValues["action"]);
            }

            // Fix the dictionary if there are arrays in it.
            pageLinkValueDictionary = FixListRouteDataValues(pageLinkValueDictionary);

            // 'Render' virtual path.
            var virtualPathForArea = RouteTable.Routes.GetVirtualPathForArea(viewContext.RequestContext,
                pageLinkValueDictionary);

            return virtualPathForArea == null ? null : virtualPathForArea.VirtualPath;
        }

        private static RouteValueDictionary FixListRouteDataValues(RouteValueDictionary routes)
        {
            var newRv = new RouteValueDictionary();
            foreach (var key in routes.Keys)
            {
                var value = routes[key];
                if (value is IEnumerable && !(value is string))
                {
                    var index = 0;
                    foreach (var val in (IEnumerable)value)
                    {
                        newRv.Add(string.Format("{0}[{1}]", key, index), val);
                        index++;
                    }
                }
                else
                {
                    newRv.Add(key, value);
                }
            }
            return newRv;
        }

        private string PagerSummary()
        {
            var start = ((currentPage - 1) * pageSize) + 1;
            var end = Math.Min(totalItemCount, currentPage * pageSize);

            var builder = new TagBuilder("div");
            builder.AddCssClass("pager-summary");
            builder.SetInnerText(string.Format("Showing {0} &ndash; {1} of {2} results", start, end, totalItemCount));

            return HttpUtility.HtmlDecode(builder.ToString());
        }
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass",
        Justification = "File contains generic and non-generic of the same class.")]
    public class Pager<TModel> : Pager
    {
        private readonly HtmlHelper<TModel> htmlHelper;

        public Pager(HtmlHelper<TModel> htmlHelper, int pageSize, int currentPage, int totalItemCount)
            : base(htmlHelper, pageSize, currentPage, totalItemCount)
        {
            this.htmlHelper = htmlHelper;
        }

        public Pager<TModel> Options(Action<PagerOptionsBuilder<TModel>> buildOptions)
        {
            buildOptions(new PagerOptionsBuilder<TModel>(PagerOptions, htmlHelper));
            return this;
        }
    }
}