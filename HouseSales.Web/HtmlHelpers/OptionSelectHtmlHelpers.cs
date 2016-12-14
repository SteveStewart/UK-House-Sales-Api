using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace HouseSales.Web.HtmlHelpers
{
    /// <summary>
    /// Provide a razor Html helper for a Bootstrap styled select list hyelpers
    /// </summary>
    public static class OptionSelectHtmlHelpers
    {
        /// <summary>
        /// Simple helper for a bootstrap single select dropdown
        /// Dependency: bootstrap-select.js
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="id"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static MvcHtmlString BootstrapSingleSelectList(this HtmlHelper helper, String id, IEnumerable<SelectListItem> options)
        {
            TagBuilder selectTag = new TagBuilder("select");
            selectTag.AddCssClass("form-control selectpicker");
            selectTag.MergeAttribute("id", id);

            foreach(SelectListItem option in options)
            {
                TagBuilder optionTag = new TagBuilder("option");
                optionTag.MergeAttribute("value", option.Value);

                if (option.Selected)
                    optionTag.MergeAttribute("selected", "true");

                optionTag.SetInnerText(option.Text);

                selectTag.InnerHtml += optionTag.ToString();
            }

            return new MvcHtmlString(selectTag.ToString());
        }
    }
}