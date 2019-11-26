<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RiepilogoFattura.ascx.cs" Inherits="VideoSystemWeb.Agenda.userControl.RiepilogoFattura" %>

<script>
</script>

<style>
    .cella {
        padding-top:18px;
        vertical-align:top;
    }
</style>
<div id="modalFattura" class="w3-modal">
    <div class="w3-modal-content  w3-animate-zoom " style="position: fixed; top: 5%; width: 70%; left: 15%; overflow: auto; height: 90%; background-color: transparent">
        <div class="w3-center w3-padding-small" style="position: relative; background-color: white">
            <asp:Button ID="btnStampaFattura" runat="server" Text="Stampa" class="w3-btn w3-white w3-border w3-border-green w3-round-large " Style="font-size: smaller; padding: 4px 8px" OnClick="btnStampaFattura_Click" />
            <button onclick="document.getElementById('modalFattura').style.display='none'" type="button" class=" w3-btn w3-white w3-border w3-border-red w3-round-large" style="font-size: smaller; padding: 4px 8px">Chiudi</button>
        </div>

        <div id="DivFramePdfFattura" runat="server" style=" width:100%; height:90%;" >
            <iframe id="framePdfFattura" runat="server" src="~/Images/logoVSP_trim.png" style=" width:100%; height:100%;"></iframe>
        </div>
    </div>
</div>


