using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class IntermediatePage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Intermediate.TumblrVerifier = Request.QueryString["oauth_verifier"];
        Intermediate.IsResponseReceived = true;

        ScriptManager.RegisterClientScriptBlock(this, typeof(string), "CloseSelf", "CloseSelf();", true);
    }
}

public static class Intermediate
{
    public static string TumblrVerifier = string.Empty;

    public static bool IsResponseReceived = false;
}