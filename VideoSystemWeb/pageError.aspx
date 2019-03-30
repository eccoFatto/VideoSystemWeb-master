<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="pageError.aspx.cs" Inherits="VideoSystemWeb.pageError" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>PAGINA ERRORE</title>
    <link rel="stylesheet" href="/Css/w3.css" />
    <link rel="stylesheet" href="/Css/w3-colors-win8.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="w3-row">
                <div class="w3-quarter">&nbsp;</div>
                <div class="w3-half">
                    <div class="w3-0 mydiv">
                        <div class="w3-container w3-margin w3-teal"><h5 style="text-align:center;">PAGINA DI ERRORE</h5></div>
                        <div class="w3-panel w3-yellow w3-border w3-round" style="text-align:center;">
                            <br /><br /><br />
                            <asp:Label ID="lblInfoErrore" runat="server" Font-Bold="True" Font-Size="Large" ForeColor="Red"></asp:Label>
                            <br /><br /><br />
                            <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click" />
                            <br /><br /><br />
                        </div>
                    </div>
                </div>
                <div class="w3-quarter">&nbsp;</div>
            
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
