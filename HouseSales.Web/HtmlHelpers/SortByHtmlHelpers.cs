using HouseSales.Domain;
using System;
using System.Web.Mvc;

namespace HouseSales.Web.HtmlHelpers
{
    public static class SortByHtmlHelpers
    {
        public static MvcHtmlString BootstrapSortByToggle(this HtmlHelper helper, String id, OrderField field, OrderDirection direction, OrderBy selectedOrdering)
        {          
            TagBuilder outerSpan = new TagBuilder("span");
            outerSpan.AddCssClass("btn btn-sm");
            outerSpan.MergeAttribute("id", id);
            outerSpan.MergeAttribute("field", field.ToString());
            outerSpan.MergeAttribute("direction", direction.ToString());

            TagBuilder innerSpan = new TagBuilder("span");

            if (selectedOrdering.Field == field && selectedOrdering.Direction == direction)
                outerSpan.AddCssClass("btn-success");
            else
                outerSpan.AddCssClass("btn-outline-default");

            if (direction == OrderDirection.Descending)
                innerSpan.AddCssClass("glyphicon glyphicon-chevron-down");
            else
                innerSpan.AddCssClass("glyphicon glyphicon-chevron-up");

            outerSpan.InnerHtml = innerSpan.ToString();

            return new MvcHtmlString(outerSpan.ToString());
        }
    }
}