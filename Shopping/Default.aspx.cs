using System;
using System.Collections.Generic;
using System.Web.UI;

namespace Shopping
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string username = Session["Username"] as string;
                if (!string.IsNullOrEmpty(username))
                {
                    pnlUserInfo.Visible = true;
                    lblUsername.Text = username;
                }
                else
                {
                    pnlUserInfo.Visible = false;
                }
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("~/Default.aspx");
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