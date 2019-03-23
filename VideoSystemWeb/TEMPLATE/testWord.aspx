<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="testWord.aspx.cs" Inherits="VideoSystemWeb.TEMPLATE.testWord" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <label>Inserisci testo</label><br />
            <asp:TextBox ID="tbParametro1" runat="server" TextMode="MultiLine" Rows="10"></asp:TextBox>
            <asp:Button ID="btnAggiornaModello" runat="server" OnClick="btnAggiornaModello_Click" Text="Ok" />
            <br />
            <label>Elenco Segnalibri trovati</label><br />
            <asp:ListBox ID="lbBookmarks" runat="server"></asp:ListBox>
        </div>
    </form>
</body>
</html>
