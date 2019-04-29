<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="resetPassword.aspx.cs" Inherits="VideoSystemWeb.resetPassword" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Reset Password</title>
    <link rel="stylesheet" href="/Css/w3.css" />
    <link rel="stylesheet" href="/Css/w3-colors-win8.css" />
</head>
<body>
    <form id="form1" runat="server" style="align-content:center;">
        <div class="w3-container " style="align-content:center;width:500px;margin:0 auto;">
            <div class="w3-container w3-green w3-card-4 w3-margin w3-padding-16 w3-center">
                <h3>Recupero Password VIDEOSYSTEM</h3>
            </div>

            <div class="w3-container w3-card-4 w3-margin w3-padding-32">
                <p>
                    <label>Nome Utente</label>
                    <asp:TextBox ID="tbUser" runat="server" CssClass="w3-input w3-border w3-round" Style="width: 99%" required="required"></asp:TextBox>
                </p>
                <p>
                    <label>E-mail</label>
                    <asp:TextBox ID="tbEmail" runat="server" CssClass="w3-input w3-border w3-round" Style="width: 99%" required="required"></asp:TextBox>
                </p>
                <p>
                    <asp:Button ID="btnReset" runat="server" OnClick="btnReset_Click" CssClass="w3-btn w3-section w3-green w3-ripple" Text="Reset Password" OnClientClick="return confirm('Confermi reset Password?');"></asp:Button>
                </p>
                <p>
                    <asp:Label ID="lblErrorLogin" runat="server" ForeColor="Red" Visible="False"></asp:Label>
                </p>
            </div>
        </div>
        <br />
        <br />
        <br />
        <br />
        <hr />
        <footer class="w3-container w3-teal">
            <h5 style="text-align: center">&copy; <%: DateTime.Now.Year %> - VIDEOSYSTEM</h5>
        </footer>
    </form>
</body>
</html>
