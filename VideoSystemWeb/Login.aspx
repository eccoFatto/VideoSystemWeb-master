<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="VideoSystemWeb.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>LOGIN</title>
<%--    <link rel="stylesheet" href="https://www.w3schools.com/w3css/4/w3.css"/>
    <link rel="stylesheet" href="https://www.w3schools.com/lib/w3-colors-win8.css"/>--%>
    <link rel="stylesheet" href="/Css/w3.css"/>
    <link rel="stylesheet" href="/Css/w3-colors-win8.css"/>
    <link rel="stylesheet" href="https://unpkg.com/spectre.css/dist/spectre.min.css"/>
    <link rel="stylesheet" href="https://unpkg.com/spectre.css/dist/spectre-exp.min.css"/>
    <link rel="stylesheet" href="https://unpkg.com/spectre.css/dist/spectre-icons.min.css"/>
</head>
<body>
    <form id="form1" runat="server">
      
        <!-- CON SPECTRE SI PUO' DIVIDERE LA PAGINA IN 12 COLONNE CON I DIV -->
        <div class="container">
            <div class="columns">
                <div class="column col-2"></div>
                <div class="column col-8">
                    <div class="w3-0 mydiv">
                        
                        <div class="w3-container w3-card-4 w3-margin-top  w3-padding-16">

                            <p>
                                <label>Name</label>
                                <asp:TextBox ID="tbUser" runat="server" class="w3-input w3-border w3-round"  style="width:99%" required="required" ></asp:TextBox>
                                </p>
                            <p>
                                <label>Password</label>
                                <asp:TextBox ID="tbPassword" runat="server" TextMode="Password" class="w3-input w3-border w3-round"  style="width:99%" required="required"></asp:TextBox>
                                </p>
                            <p>
                                <input id="cbStayLogged" class="w3-check" type="checkbox" checked="checked"/>
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
                <div class="column col-2"></div>
            
            </div>
        </div>
        <br /><br /><br /><br />
        <hr />
        <footer class="w3-container w3-teal" >
            <h5 style="text-align:center">&copy; <%: DateTime.Now.Year %>- VIDEOSYSTEM</h5>
        </footer>

    </form>
</body>
</html>
