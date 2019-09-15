<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="testComboBox.aspx.cs" Inherits="VideoSystemWeb.TEMPLATE.testComboBox" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>

        $(document).ready(function () {
            $('.loader').hide();
        });
    </script>
    
    <ajaxToolkit:DropDownExtender ID="txt_Pagamento_DropDownExtender" runat="server" BehaviorID="txt_Pagamento_DropDownExtender" TargetControlID="txt_Pagamento" DropDownControlID="Panel1">
    </ajaxToolkit:DropDownExtender>
    <asp:TextBox ID="txt_Pagamento" runat="server"></asp:TextBox>
    <asp:Panel 
        ID="Panel1"
        runat="server"
        BorderColor="Pink"
        BorderWidth="2"
        >
        <asp:BulletedList 
            ID="BulletedList1"
            runat="server"
            DisplayMode="LinkButton"
            OnClick="BulletedList1_Click">
                <asp:ListItem>30</asp:ListItem>
                <asp:ListItem>60</asp:ListItem>
                <asp:ListItem>90</asp:ListItem>
                <asp:ListItem>120</asp:ListItem>
        </asp:BulletedList>
    </asp:Panel>
    <br />
    <ajaxToolkit:ComboBox ID="ComboBox1" runat="server" >
        <asp:ListItem Value="30">30 giorni</asp:ListItem>
        <asp:ListItem Value="60">60 giorni</asp:ListItem>
        <asp:ListItem Value="90">90 giorni</asp:ListItem>
    </ajaxToolkit:ComboBox>


</asp:Content>