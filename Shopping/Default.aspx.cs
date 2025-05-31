using System;
using System.Collections.Generic;
using System.Web.UI;

namespace Shopping
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        { 
        }

        protected string GetColorOptionsHtml(List<string> colors)
        {
            if (colors == null || colors.Count == 0)
                return string.Empty;

            var html = "";
            foreach (var color in colors)
            {
                html += $"<span class=\"color-option\" style=\"background-color: {color}\" title=\"Màu {color}\"></span>";
            }
            return html;
        }
    }
}