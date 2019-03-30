<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="VideoSystemWeb.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>LOGIN</title>
    <link rel="stylesheet" href="/Css/w3.css" />
    <link rel="stylesheet" href="/Css/w3-colors-win8.css" />
    
    <script type="text/javascript" src="/Scripts/jquery-3.3.1.min.js"></script>
    <script type="text/javascript" src='/Scripts/bootstrap.bundle.min.js'></script>
    <link rel='stylesheet' href='/Css/Style.css' />
</head>
<body>
    
    <form id="form1" runat="server">
        <div class="container">
            <div class="w3-row">
                <div class="w3-third">&nbsp;</div>
                <div class="w3-third">
                    <div class="w3-0 mydiv">
                        <%--<div class="w3-panel w3-gray w3-center w3-margin-top w3-text-white w3-card-2 w3-round">LOGIN APPLICATIVO VIDEOSYSTEM</div>--%>
                        <div class="w3-container w3-teal w3-center w3-margin-top">
                            <h2>Login VIDEOSYSTEM</h2>
                        </div>

                        <div class="w3-container w3-card-4 w3-margin-bottom w3-padding-32">
                            <p>
                                <label>Name</label>
                                <asp:TextBox ID="tbUser" runat="server" class="w3-input w3-border w3-round" Style="width: 99%" required="required"></asp:TextBox>
                            </p>
                            <p>
                                <label>Password</label>
                                <asp:TextBox ID="tbPassword" runat="server" TextMode="Password" class="w3-input w3-border w3-round" Style="width: 99%" required="required"></asp:TextBox>
                            </p>
                            <p style="display:none;">
                                <input id="cbStayLogged" class="w3-check" type="checkbox" checked="checked" />
                                <label>Rimani Connesso</label>
                            </p>
                            <p>
                                <asp:Button ID="btnLogin" runat="server" OnClick="btnLogIn_Click" class="w3-btn w3-section w3-teal w3-ripple" Text="Log in"></asp:Button>
                            </p>
                            <p>
                                <asp:Label ID="lblErrorLogin" runat="server" ForeColor="Red" Visible="False"></asp:Label>
                            </p>
                        </div>
                    </div>
                </div>
               <div class="w3-third">&nbsp;</div>
            </div>
        </div>
        <br />
        <br />
        <br />
        <br />
        <hr />
        <footer class="w3-container w3-teal">
            <h5 style="text-align: center">&copy; <%: DateTime.Now.Year %>- VIDEOSYSTEM</h5>
        </footer>
    </form>
</body>
</html>
