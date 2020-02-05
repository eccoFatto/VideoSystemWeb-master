<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NotaSpese.ascx.cs" Inherits="VideoSystemWeb.Agenda.userControl.NotaSpese" %>


 
<div id="modalNotaSpese" class="w3-modal">
    <div class="w3-modal-content  w3-animate-zoom " style="position: fixed; top: 5%; width: 70%; left: 15%; overflow: auto; height: 90%; background-color: transparent">
        <div class="w3-center w3-padding-small" style="position: relative; background-color: white">
            <asp:Button ID="btnStampaNotaSpese" runat="server" Text="Stampa" class="w3-btn w3-white w3-border w3-border-green w3-round-large " Style="font-size: smaller; padding: 4px 8px" OnClick="btnStampaNotaSpese_Click" />
            <button onclick="document.getElementById('modalNotaSpese').style.display='none'" type="button" class=" w3-btn w3-white w3-border w3-border-red w3-round-large" style="font-size: smaller; padding: 4px 8px">Chiudi</button>
        </div>

        <div id="DivFramePdfNotaSpese" runat="server" style=" width:100%; height:90%;" >
            <iframe id="framePdfNotaSpese" runat="server" src="~/Images/logoVSP_trim.png" style=" width:100%; height:100%;"></iframe>
        </div>
    </div>
</div>