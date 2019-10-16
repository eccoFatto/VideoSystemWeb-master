<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RiepilogoOfferta.ascx.cs" Inherits="VideoSystemWeb.Agenda.userControl.RiepilogoOfferta" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<script>

    $(document).ready(function () {

        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {

            $('.limited-lines').keydown(function (event) {

                if ( event.which == 13 ) {
                    var numberOfLines = $(this).val().split('\n').length;
                    if(numberOfLines >= 5){
                        event.preventDefault();  
                    }
                }
            });

        });

    });
    
<%--    function aggiornaRiepilogo() {

        $("#<%=note.ClientID%>").text($("#<%=txt_Note.ClientID%>").val());
    }--%>

</script>

<style>
    .cella {
        padding-top:10px;
        vertical-align:top;
    }


</style>
<div id="modalRiepilogoOfferta" class="w3-modal">
    <div class="w3-modal-content  w3-animate-zoom " style="position: fixed; top: 5%; width: 70%; left: 15%; overflow: auto; height: 90%; background-color: transparent">
        <div class="w3-center w3-padding-small" style="position: relative; background-color: white">
            <asp:Button ID="btnStampaOfferta" runat="server" Text="Stampa" class="w3-btn w3-white w3-border w3-border-green w3-round-large " Style="font-size: smaller; padding: 8px 4px" />
            <asp:Button ID="btnModificaNote" runat="server" Text="Modifica note" class="w3-btn w3-white w3-border w3-border-orange w3-round-large " Style="font-size: smaller; padding: 8px 4px" OnClick="btnModificaNote_Click" />
            <button onclick="document.getElementById('modalRiepilogoOfferta').style.display='none'" type="button" class=" w3-btn w3-white w3-border w3-border-red w3-round-large" style="font-size: smaller; padding: 8px 4px">Chiudi</button>
        </div>
        <%--<div id="modalRiepilogoContent" runat="server" style="background-color: white;">--%>

            <div id="DivFramePdf" runat="server" style=" width:100%; height:90%;" >
                <iframe id="framePdf" runat="server" src="#" style=" width:100%; height:100%;"></iframe>
            </div>
        <%--</div>--%>
    </div>
</div>



<div id="panelModificaNote" class="w3-modal " style="position: fixed;">

    <div class="w3-modal-content w3-card-4 w3-animate-top round" style="position: relative; width: 50%; background-color: white; overflow: auto;">
        <div class="w3-row-padding">
            <div class="w3-center">
                <br>
                <span onclick="document.getElementById('panelModificaNote').style.display='none'" class="w3-button w3-xlarge w3-hover-red w3-display-topright" title="Chiudi">&times;</span>
            </div>

            <div class="w3-center">
                <h3>Modifica note Offerta</h3>
            </div>

            <div class="w3-row" style="padding: 5px;">

                <div class="w3-quarter">
                    <label style="margin-bottom: 0.2rem;">Banca</label>
                </div>
                <div class="w3-threequarter">
                    <asp:DropDownList ID="ddl_Banca" runat="server"></asp:DropDownList>
                </div>
            </div>
            <div class="w3-row" style="padding: 5px;">
                <div class="w3-quarter">
                    <label style="margin-bottom: 0.2rem;">Pagamento (GG)</label>
                </div>
                <div class="w3-threequarter">
                    <asp:TextBox ID="tbMod_Pagamento" runat="server"/>    
                    <%--<ajaxToolkit:FilteredTextBoxExtender runat="server" ID="ftbe" FilterMode="ValidChars" FilterType="Numbers" TargetControlID="tbMod_Pagamento" /> --%>
                    <ajaxToolkit:ComboBox ID="ComboMod_Pagamento" runat="server" Visible="false"/>
                </div>
            </div>
            <div class="w3-row" style="padding: 5px;">
                <div class="w3-quarter" style="padding: 5px">
                    <label style="margin-bottom: 0.2rem;">Consegna</label>
                </div>
                <div class="w3-threequarter">
                    <asp:TextBox ID="txt_Consegna" runat="server" class="w3-input w3-border" placeholder="Consegna" Style="padding: 2px;"></asp:TextBox>
                </div>
            </div>
            <div class="w3-row" style="padding: 5px;">
                <div class="w3-quarter" style="padding: 5px">
                    <label style="margin-bottom: 0.2rem;">Note</label>
                </div>
                <div class="w3-threequarter">
                    <asp:TextBox ID="txt_Note" runat="server" CssClass="w3-input w3-border limited-lines" placeholder="Note" Style="padding: 2px;" Rows="6" TextMode="MultiLine"></asp:TextBox>
                </div>
            </div>
        </div>

        <div class="w3-center" style="margin: 10px">
            <asp:Button ID="btnOKModificaNote" runat="server" Text="OK" class=" w3-btn w3-white w3-border w3-border-green w3-round-large" Style="font-size: smaller; padding: 4px 8px" OnClick="btnOKModificaNote_Click" />
            <button onclick="document.getElementById('panelModificaNote').style.display='none'" type="button" class=" w3-btn w3-white w3-border w3-border-red w3-round-large" style="font-size: smaller; padding: 4px 8px">Annulla</button>
        </div>
    </div>
</div>

