<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="VideoSystemWeb.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>LOGIN</title>
    <link rel="stylesheet" href="https://www.w3schools.com/w3css/4/w3.css"/>
    <link rel="stylesheet" href="https://www.w3schools.com/lib/w3-colors-win8.css"/>
    <link rel="stylesheet" href="https://unpkg.com/spectre.css/dist/spectre.min.css"/>
    <link rel="stylesheet" href="https://unpkg.com/spectre.css/dist/spectre-exp.min.css"/>
    <link rel="stylesheet" href="https://unpkg.com/spectre.css/dist/spectre-icons.min.css"/>
</head>
<body>
    <form id="form1" runat="server">
        <div id="MENU" class="w3-container w3-0 mydiv" style="align-content:center;">
    <table id="tblmenu" style="width:100%;">
        <tr>
            <td style="width:10%;align-content:center;text-align:center;"></td>
            <td style="width:10%;align-content:center;text-align:center;"></td>
            <td style="width:10%;align-content:center;text-align:center;"></td>
            <td style="width:10%;align-content:center;text-align:center;"></td>
            <td style="width:10%;align-content:center;text-align:center;"></td>
            <td style="width:10%;align-content:center;text-align:center;"></td>
            <td style="width:10%;align-content:center;text-align:center;"></td>
            <td style="width:10%;align-content:center;text-align:center;"></td>
            <td style="width:10%;align-content:center;text-align:center;"></td>
            <td style="width:10%;align-content:center;text-align:center;"></td>
        </tr>
    </table>
</div>
        <!-- CON SPECTRE SI PUO' DIVIDERE LA PAGINA IN 12 COLONNE CON I DIV -->
        <div class="container">
            <div class="columns">
                <div class="column col-2"></div>
                <div class="column col-8">
                    <div class="w3-0 mydiv">
                        <div class="w3-container w3-margin w3-teal"><h5 style="text-align:center;">Login Utente</h5></div>
                        <div class="w3-panel w3-yellow w3-border w3-round" style="text-align:center;">
                            <table style="width:400px;padding-top:40px;padding-bottom:30px;margin:auto;">
                                <tr>
                                    <td style="width:50%;text-align:right;" class="w3-container w3-margin w3-teal"><asp:Label runat="server" Text="User Name:"> </asp:Label></td>
                                    <td style="width:50%;"><asp:TextBox ID="tbUser" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="width:50%;text-align:right;" class="w3-container w3-margin w3-teal"><asp:Label runat="server" Text="Password:"> </asp:Label></td>
                                    <td style="width:50%;"><asp:TextBox ID="tbPassword" runat="server" TextMode="Password"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="width:50%;text-align:right;"></td>
                                    <td style="width:50%;text-align:right;padding-right:10px" class="tooltip" data-tooltip="Conferma dati"><asp:Button ID="btnLogIn" runat="server" Text="Conferma" OnClick="btnLogIn_Click" /></td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="width:100%;">
                                        <asp:GridView ID="GridView1" runat="server" Visible="false">
                                        </asp:GridView>
                                        <asp:Label ID="lblErrorLogin" runat="server" ForeColor="Red" Visible="False"></asp:Label>
                                    </td>
                                </tr>
                            </table>
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
