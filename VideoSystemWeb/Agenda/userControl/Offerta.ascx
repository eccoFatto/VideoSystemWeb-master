﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Offerta.ascx.cs" Inherits="VideoSystemWeb.Agenda.userControl.Offerta" %>

<script>
    
    function confermaSalvataggio() {
        return confirm("Confermare il salvataggio dell'offerta?"); 
    }
</script>


<asp:Panel runat="server" ID="panelAppuntamenti" >
    <div class="w3-container w3-center w3-xlarge">OFFERTA</div>

    <div class="alert alert-danger alert-dismissible fade in out" role="alert" runat="server" id="panelErrore" style="display: none">
        <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
        <asp:Label ID="lbl_MessaggioErrore" runat="server" CssClass="form-control-sm"></asp:Label>
    </div>

    

    <div class="w3-col m12">
        <div class="w3-center w3-margin">
            <asp:Button ID="btnSalva" runat="server" Text="Salva" class=" w3-btn w3-white w3-border w3-border-green w3-round-large" OnClick="btnSalva_Click" Visible="false" />
            <asp:Button ID="btnAnnulla" runat="server" Text="Annulla" class="w3-btn w3-white w3-border w3-border-red w3-round-large" OnClick="btnAnnulla_Click" Visible="false" />
        </div>
    </div>
</asp:Panel>