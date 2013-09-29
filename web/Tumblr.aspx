<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Tumblr.aspx.cs" Inherits="Tumblr" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        function OpenTumblrWindow() {
            var txt = document.getElementById('hdnField')
            var urlstring = txt.value;
            window.open(urlstring, '_blank', 'height=500,width=800,status=yes,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=no,titlebar=no');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager runat="server" />
    <div>
        Tumblr
        <br />
        <br />
        <asp:Button runat="server" Text="Authenticate and Authorize" ID="btnAuthorize" OnClientClick="OpenTumblrWindow();"
            OnClick="btnAuthorize_Click" />
        <br />
        <br />
        Blog Title :&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:TextBox runat="server" Width="500" ID="txtBlogTitle" />
        <br />
        <br />
        Blog Content :
        <asp:TextBox ID="txtBlogContent" runat="server" Width="700" Height="250" TextMode="MultiLine" />
        <br />
        <br />
        <asp:Button runat="server" Text="Post Blog" ID="btnPost" Visible="false" OnClick="btnPost_Click" />
        &nbsp;&nbsp;&nbsp;
        <asp:Label runat="server" ForeColor="Green" Font-Bold="true" ID="lblSuccess" />
        <asp:Timer runat="server" Enabled="false" Interval="2000" ID="timer" OnTick="timer_Tick1" />
        <asp:HiddenField runat="server" ID="hdnField" ClientIDMode="Static" />
    </div>
    </form>
</body>
</html>
