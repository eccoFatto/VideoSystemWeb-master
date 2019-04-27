<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="testMail.aspx.cs" Inherits="VideoSystemWeb.testMail" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link rel="stylesheet" href="/Css/w3.css" />
    <link rel="stylesheet" href="/Css/w3-colors-win8.css" />
</head>
<body>
    <form id="form1" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" EnablePartialRendering="true" runat="server" />
        <div>
            <div class="w3-row-padding w3-margin w3-center">
                <asp:Label ID="Label1" runat="server" CssClass="w3-input w3-border w3-center w3-xxlarge" Text="Invio E-Mail" ></asp:Label>
            </div>
            <div class="w3-row-padding w3-margin">
                <div class="w3-third">
                    <label>Client</label>
                    <asp:TextBox ID="tbClient" runat="server" CssClass="w3-input w3-border" Text="smtps.aruba.it" placeholder=""></asp:TextBox>
                </div>
                <div class="w3-third">
                    <label>Mail From</label>
                    <asp:TextBox ID="tbMailFrom" runat="server" CssClass="w3-input w3-border" Text="info@videosystemproduction.it" placeholder=""></asp:TextBox>
                </div>
                <div class="w3-third">
                    <label>Mail To</label>
                    <asp:TextBox ID="tbMailTo" runat="server" CssClass="w3-input w3-border" Text="claudio.calderai@gmail.com" placeholder=""></asp:TextBox>
                </div>
            </div>
            <div class="w3-row-padding w3-margin"">
                <div class="w3-quarter">
                    <label>User</label>
                    <asp:TextBox ID="tbUser" runat="server" CssClass="w3-input w3-border" Text="info@videosystemproduction.it" placeholder=""></asp:TextBox>
                </div>
                <div class="w3-quarter">
                    <label>Password</label>
                    <asp:TextBox ID="tbPassword" runat="server" CssClass="w3-input w3-border" Text="salamandra" TextMode="Password" placeholder=""></asp:TextBox>
                </div>
                <div class="w3-quarter">
                    <label>Porta</label>
                    <asp:TextBox ID="tbPorta" runat="server" CssClass="w3-input w3-border" Text="465" MaxLength="4" placeholder=""></asp:TextBox>
                    <ajaxToolkit:MaskedEditExtender ID="tbPorta_MaskedEditExtender" runat="server" TargetControlID="tbPorta" MaskType="None" Mask="9999"  ></ajaxToolkit:MaskedEditExtender>
                </div>
                <div class="w3-quarter">
                    <label>SSl</label>
                    <asp:DropDownList ID="ddlSSL" runat="server" CssClass="w3-input w3-border" >
                        <asp:ListItem Text="Si" Value="true"></asp:ListItem>
                        <asp:ListItem Text="No" Value="false"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>

            <div class="w3-row-padding w3-margin">
                <label>Oggetto</label>
                <asp:TextBox ID="tbSubject" runat="server" CssClass="w3-input w3-border" Text="PROVA DA ARUBA" placeholder=""></asp:TextBox>
            </div>

            <div class="w3-row-padding w3-margin">
                <label>Testo</label>
                <asp:TextBox ID="tbBody" runat="server" CssClass="w3-input w3-border" Text="Body Prova da Aruba" placeholder="" Rows="10" TextMode="MultiLine"></asp:TextBox>
            </div>
            <div class="w3-row-padding w3-margin">
                <asp:Button runat="server" ID="btnSendMail" OnClick="btnSendMail_Click" CssClass="w3-button w3-orange w3-round-medium w3-hover-red" Text="Invia" OnClientClick="return confirm('Confermi invio Mail?');" />
            </div>
            <div class="w3-row-padding w3-margin">
                <asp:Label ID="lblInfo" runat="server" CssClass="w3-input w3-border" ></asp:Label>
            </div>
        </div>
    </form>
</body>
</html>
