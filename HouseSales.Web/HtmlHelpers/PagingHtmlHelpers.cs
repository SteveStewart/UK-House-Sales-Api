using HouseSales.Domain;
using System;
using System.Collections;
using System.Text;
using System.Web.Mvc;

namespace HouseSales.Web.HtmlHelpers
{
    public static class PagingHtmlHelpers
    {
        /// <summary>
        /// Build the paging buttons with a bootstrap styling
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="paging"></param>
        /// <param name="pageUrl"></param>
        /// <returns></returns>
        public static MvcHtmlString BootstrapPagingLinks<T>(this HtmlHelper helper, PaginatedResult<T> paging, Func<int, String> pageUrl)
            where T : IEnumerable
        {
            StringBuilder pageLinks = new StringBuilder();

            pageLinks.Append(@"<ul class=""pagination"">");
            pageLinks.AppendLine(GetPreviousPageLink("Prev", paging, pageUrl));

            for (int pageCount = 0; pageCount < paging.TotalPages; pageCount++)
            {
                String pageLink = BuildPageLinkButton((pageCount+1).ToString(), pageCount, paging.Page, pageUrl);

                pageLinks.AppendLine(pageLink);
            }

            pageLinks.AppendLine(GetNextPageLink("Next", paging, pageUrl));
            pageLinks.AppendLine(@"</ul>");

            return new MvcHtmlString(pageLinks.ToString());
        }
              

        /// <summary>
        /// A helper for a paging caption string
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="paging"></param>
        /// <returns></returns>
        public static MvcHtmlString GetPagingCaption<T>(this HtmlHelper helper, PaginatedResult<T> paging, int limit)
            where T : IEnumerable
        {
            string caption = String.Format("{0} - {1} of {2}", paging.PageStartIndex, paging.PageEndIndex, paging.TotalItems);

            if (paging.TotalItems >= limit)
                caption += "+";

            return new MvcHtmlString(caption);
        }

        /// <summary>
        /// Build a new page link button
        /// </summary>
        /// <param name="pageCount"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageUrl"></param>
        /// <returns></returns>
        private static String BuildPageLinkButton(String label, int pageCount, int currentPage, Func<int, String> pageUrl)
        {
            String id = String.Format("{0}{1}", Properties.Resources.TXT_PAGE_LINK_PREFIX, pageCount);
            
            TagBuilder btnTag = new TagBuilder("span");
            btnTag.MergeAttribute("id", id);
            btnTag.InnerHtml = label;
            btnTag.AddCssClass("btn");

            if (pageCount == currentPage)
                btnTag.AddCssClass("btn-primary");
            else
                btnTag.AddCssClass("btn-default");

            return btnTag.ToString();
        }

        /// <summary>
        /// Get the next page link button
        /// </summary>
        /// <param name="label"></param>
        /// <param name="paging"></param>
        /// <returns></returns>
        private static String GetNextPageLink<T>(String label, PaginatedResult<T> paging, Func<int, String> pageUrl)
            where T : IEnumerable
        {
            return BuildPageLinkButton(label, paging.Page + 1, paging.Page, pageUrl);
        }

        /// <summary>
        /// Get the previous page link button
        /// </summary>
        /// <param name="label"></param>
        /// <param name="paging"></param>
        /// <returns></returns>
        private static String GetPreviousPageLink<T>(String label, PaginatedResult<T> paging, Func<int, String> pageUrl)
            where T : IEnumerable
        {
            return BuildPageLinkButton(label, paging.Page - 1, paging.Page, pageUrl); 
        }
    }

}
 