<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="pageError.aspx.cs" Inherits="VideoSystemWeb.pageError" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>PAGINA ERRORE</title>
    <link rel="stylesheet" href="https://www.w3schools.com/w3css/4/w3.css"/>
    <link rel="stylesheet" href="https://www.w3schools.com/lib/w3-colors-win8.css"/>
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
