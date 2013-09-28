using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;

public partial class Tumblr : System.Web.UI.Page
{
    Helper requestHelper = new Helper();
    
    protected void Page_Load(object sender, EventArgs e)
    {
        Helper.TumblrConsumerKey = "lQS712YUjSnDppww0NV8EcFn1Y4Ch7p6cSnUB4g11MKt314hJs"; //Your consumer key/client key provided by Tumblr after you register your application
        Helper.TumblrConsumerSecret = "fXC9IhAMyr91jzS437ycXrw4GdI5fHyI3ZesWCpacZ7zS1EILL"; //Your client secret provided by Tumblr after you register your application
        requestHelper.CallBackUrl = "http://vanish.apphb.com/IntermediatePage.aspx"; //Your callback URL here. You should provide this in your application settings redirect url
        
        if (!IsPostBack)
        {
            hdnField.Value = requestHelper.GetAuthorizationLink();
        }
    }

    protected void btnAuthorize_Click(object sender, EventArgs e)
    {
        timer.Enabled = true;
    }

    protected void timer_Tick1(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(Intermediate.TumblrVerifier))
        {
            btnPost.Visible = true;
            lblSuccess.Visible = true;
            timer.Enabled = false;
            lblSuccess.Text = "Authentication Successful..";
        }
        else if (Intermediate.IsResponseReceived)
        {
            lblSuccess.Text = "User denied access";
            lblSuccess.Visible = true;
            timer.Enabled = false;
            Intermediate.IsResponseReceived = false;
        }
    }

    protected void btnPost_Click(object sender, EventArgs e)
    {
        try
        {
            string AccessTokenResponse = requestHelper.GetAccessToken(Helper.TumblrToken, Intermediate.TumblrVerifier);

            string[] tokens = AccessTokenResponse.Split('&'); //extract access token & secret from response
            string accessToken = tokens[0].Split('=')[1];
            string accessTokenSecret = tokens[1].Split('=')[1];

            KeyValuePair<string, string> LoginDetails = new KeyValuePair<string, string>(accessToken, accessTokenSecret);

            var url2 = "http://api.tumblr.com/v2/user/dashboard";

            //extract default blog URL for posting
            string BLOGURL = Helper.OAuthData(url2, "GET", LoginDetails.Key, LoginDetails.Value, null);
            BLOGURL = BLOGURL.Substring(BLOGURL.IndexOf("post_url") + 10, BLOGURL.IndexOf("slug") - BLOGURL.IndexOf("post_url") - 10);
            BLOGURL = BLOGURL.Replace("http:", "").Replace("\"", "").Replace("/", "").Replace("\\", "").Trim();
            BLOGURL = BLOGURL.Substring(0, BLOGURL.IndexOf(".com") + 4);

            //Pass required parameters
            var prms = new Dictionary<string, object>();
            prms.Add("type", "text");
            prms.Add("title", txtBlogTitle.Text);
            prms.Add("body", txtBlogContent.Text);

            var postUrl2 = "http://api.tumblr.com/v2/blog/" + BLOGURL + "/post";

            string result = Helper.OAuthData(postUrl2, "POST", LoginDetails.Key, LoginDetails.Value, prms);

            lblSuccess.Text = result; //this returns status 200 and 'Created' if success  
        }
        catch (WebException ex)
        {
            if (ex.Response is HttpWebResponse && ex.Response != null)
            {
                StreamReader exReader = new StreamReader(ex.Response.GetResponseStream());
                lblSuccess.Text = exReader.ReadToEnd();
            }
        }
    }
}