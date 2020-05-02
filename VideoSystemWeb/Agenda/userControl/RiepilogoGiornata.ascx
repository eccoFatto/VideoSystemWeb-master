<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RiepilogoGiornata.ascx.cs" Inherits="VideoSystemWeb.Agenda.userControl.RiepilogoGiornata" %>

<script>
</script>

<style>
    .cella {
        padding-top:18px;
        vertical-align:top;
    }
</style>
<div id="modalGiornata" class="w3-modal">
    <div class="w3-modal-content  w3-animate-zoom " style="position: fixed; top: 5%; width: 70%; left: 15%; overflow: auto; height: 90%; background-color: transparent">
        <div class="w3-center w3-padding-small" style="position: relative; background-color: white">
        </div>
        <div class="w3-bar w3-light-grey">
            <label class="w3-bar-item" style="font-size: smaller;padding: 4px 4px 8px;font-weight:bold">Giornata di lavoro: </label>&nbsp;
            <asp:TextBox ID="tbDataElaborazione" runat="server" MaxLength="10" class="w3-bar-item w3-input w3-border calendar" placeholder="GG/MM/AAAA" Style="font-size: smaller; padding: 4px 8px"></asp:TextBox>&nbsp;
            <asp:Button ID="btnCreaGiornata" runat="server" Text="Elabora" class="w3-bar-item w3-btn w3-white w3-border w3-border-green w3-round-large " Style="font-size: smaller; padding: 4px 8px" OnClick="btnCreaGiornata_Click" />&nbsp;
            <asp:Button ID="btnStampaGiornata" runat="server" Text="Stampa" class="w3-bar-item w3-btn w3-white w3-border w3-border-green w3-round-large " Style="font-size: smaller; padding: 4px 8px" />&nbsp;
            <button onclick="document.getElementById('modalGiornata').style.display='none'" type="button" class="w3-bar-item w3-btn w3-white w3-border w3-border-red w3-round-large" style="font-size: smaller; padding: 4px 8px">Chiudi</button>
        </div>
        <div id="DivFramePdfGiornata" runat="server" style=" width:100%; height:90%;" >
            <iframe id="framePdfGiornata" runat="server" src="~/Images/logoVSP_trim.png" style=" width:100%; height:100%;"></iframe>
        </div>
    </div>
</div>


