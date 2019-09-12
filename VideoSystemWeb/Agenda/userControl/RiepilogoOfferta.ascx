<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RiepilogoOfferta.ascx.cs" Inherits="VideoSystemWeb.Agenda.userControl.RiepilogoOfferta" %>

<script>

    function aggiornaRiepilogo() {
        $("#<%=val_bancaSchermo.ClientID%>").text($("#<%=ddl_Banca.ClientID%>").val());

        $("#<%=val_pagamentoSchermo.ClientID%>").text($("#<%=cmbMod_Pagamento.ClientID%>").find('option:selected').val() + " gg DFFM");

        $("#<%=val_consegnaSchermo.ClientID%>").text($("#<%=txt_Consegna.ClientID%>").val());

        $("#<%=note.ClientID%>").text($("#<%=txt_Note.ClientID%>").val());
    }
</script>

<style>
    .cella {
        padding-top:18px;
        vertical-align:top;
    }


</style>
<div id="modalRiepilogoOfferta" class="w3-modal">
    <div class="w3-modal-content  w3-animate-zoom " style="position: fixed; top: 5%; width: 70%; left: 15%; overflow: auto; height: 90%; background-color: transparent">
        <div class="w3-center w3-padding-small" style="position: relative; background-color: white">
            <%--<asp:Button ID="btnStampaOfferta" runat="server" Text="Stampa" class="w3-btn w3-white w3-border w3-border-green w3-round-large " Style="font-size: smaller; padding: 8px 4px" OnClick="btnStampa_Click" />--%>
            <asp:Button ID="btnStampaOfferta" runat="server" Text="Stampa" class="w3-btn w3-white w3-border w3-border-green w3-round-large " Style="font-size: smaller; padding: 8px 4px" />
            <asp:Button ID="btnModificaNote" runat="server" Text="Modifica note" class="w3-btn w3-white w3-border w3-border-orange w3-round-large " Style="font-size: smaller; padding: 8px 4px" OnClick="btnModificaNote_Click" />
            <button onclick="document.getElementById('modalRiepilogoOfferta').style.display='none'" type="button" class=" w3-btn w3-white w3-border w3-border-red w3-round-large" style="font-size: smaller; padding: 8px 4px">Chiudi</button>
        </div>

<%--        <div class="w3-center" style="background-color: white">
            <br />
            <span onclick="document.getElementById('modalRiepilogoOfferta').style.display='none'" class="w3-button w3-xlarge w3-hover-red w3-display-topright" title="Chiudi" style="top: 22px;">&times;</span>
        </div>--%>
        <div id="modalRiepilogoContent" runat="server" style="background-color: white;">
            
            <!-- COMMENTO DA QUI -->
            <!--
            <div class="w3-row  w3-padding-large w3-small">


                <div id="intestazioneSchermo" visible="false" runat="server">
                    <div class="w3-half ">
                        <div class="w3-col">
                            <div class="w3-section ">
                                <div class="w3-row">
                                    <div class="w3-third">
                                        <label><b>Roma</b></label>
                                    </div>
                                    <div class="w3-twothird">
                                        <asp:Label ID="lbl_Data" runat="server"></asp:Label>
                                    </div>
                                    <br />
                                    <br />
                                </div>
                                <div class="w3-row">
                                    <div class="w3-third">
                                        <label><b>Produzione</b></label>
                                    </div>
                                    <div class="w3-twothird">
                                        <asp:Label ID="lbl_Produzione" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div class="w3-row">
                                    <div class="w3-third">
                                        <label><b>Lavorazione</b></label>
                                    </div>
                                    <div class="w3-twothird">
                                        <asp:Label ID="lbl_Lavorazione" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div class="w3-row">
                                    <div class="w3-third">
                                        <label><b>Data Lav.ne</b></label>
                                    </div>
                                    <div class="w3-twothird">
                                        <asp:Label ID="lbl_DataLavorazione" runat="server"></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="w3-half">
                        <div class="w3-section ">

                            <div class="w3-row">
                                <div class="w3-third">
                                    <label><b>Spettabile</b></label>
                                </div>
                                <div class="w3-twothird">
                                    <asp:Label ID="lbl_Cliente" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="w3-row">
                                <div class="w3-third">
                                    <label><b>Indirizzo</b></label>
                                </div>
                                <div class="w3-twothird">
                                    <asp:Label ID="lbl_IndirizzoCliente" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="w3-row">
                                <div class="w3-third">
                                    <label><b>P. Iva / C.F.</b></label>
                                </div>
                                <div class="w3-twothird">
                                    <asp:Label ID="lbl_PIvaCliente" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div id="protocolloSchermo" visible="false" runat="server">
                <div class="w3-card-4 w3-margin-left w3-margin-right">
                    <div class="w3-row w3-padding w3-small">
                        <div class="w3-half">
                            <div class="w3-section ">
                                <div class="w3-third">
                                    <label><b>Offerta numero</b></label>
                                </div>
                                <div class="w3-twothird">
                                    <asp:Label ID="lbl_CodLavorazione" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="w3-half">
                            <div class="w3-section ">
                                <div class="w3-third">
                                    <label><b>Rif. prot.</b></label>
                                </div>
                                <div class="w3-twothird">
                                    <asp:Label ID="lbl_Protocollo" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            -->
            <!-- COMMENTO A QUI (RIEPILOGO SCHERMO) -->

            <!-- STAMPA-->
            <div id="intestazioneStampa" runat="server" visible="true" style="font-size: 8pt">
                <div class="w3-row  w3-padding-large w3-small">
                <div class="w3-col">
                    <asp:Image ID="imgLogo" runat="server" ImageUrl="~/Images/logoVSP_trim.png" Style="height: 120px" />
                </div>
                </div>
                <br />
                <br />
                <table style="width: 100%">
                    <tr>
                        <td style="width: 50%">
                            <table style="width:70%">
                                <tr>
                                    <td colspan="2">
                                        <table border="1" style="width: 100%;border-collapse:collapse;border-width:thin;">
                                            <tr>
                                                <td style="width: 33%; text-align:center">
                                                    <label><b>Roma</b></label>
                                                </td>
                                                <td style="width: 66%; text-align:center">
                                                    <asp:Label ID="lbl_DataStampa" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                    </td>
                                </tr>

                                <tr>
                                    <td style="width: 33%">
                                        <label><b>Produzione</b></label></td>
                                    <td style="width: 66%">
                                        <asp:Label ID="lbl_ProduzioneStampa" runat="server"></asp:Label></td>
                                </tr>

                                <tr>
                                    <td style="width: 33%">
                                        <label><b>Lavorazione</b></label></td>
                                    <td style="width: 66%">
                                        <asp:Label ID="lbl_LavorazioneStampa" runat="server"></asp:Label></td>
                                </tr>

                                <tr>
                                    <td style="width: 33%">
                                        <label><b>Data Lav.ne</b></label></td>
                                    <td style="width: 66%">
                                        <asp:Label ID="lbl_DataLavorazioneStampa" runat="server"></asp:Label></td>
                                </tr>

                            </table>
                        </td>

                        <td style="width: 50%">
                            <table  border="1" style="width:100%;border-collapse:collapse">
                                <tr>
                                    <td style="width: 33%;height:50px;vertical-align:top;padding:5px;">
                                        <label><b>Spettabile</b></label></td>
                                    <td style="width: 66%; vertical-align:top;padding:5px;">
                                        <asp:Label ID="lbl_ClienteStampa" runat="server"></asp:Label></td>
                                </tr>

                                <tr>
                                    <td style="width: 33%;padding:5px;">
                                        <label><b>Indirizzo</b></label></td>
                                    <td style="width: 66%;padding:5px;">
                                        <asp:Label ID="lbl_IndirizzoClienteStampa" runat="server"></asp:Label></td>
                                </tr>

                                <tr>
                                    <td style="width: 33%;padding:5px;">
                                        <label><b>P. Iva / C.F.</b></label></td>
                                    <td style="width: 66%;padding:5px;">
                                        <asp:Label ID="lbl_PIvaClienteStampa" runat="server"></asp:Label></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <br />
                <br />
                <table border="1" style="border-collapse:collapse;width: 100%">
                    <tr>
                        <td style="width: 15%;padding:5px;">
                            
                            <label><b>Offerta numero</b></label>

                        </td>
                        <td style="width: 18%;padding:5px;">
                            <asp:Label ID="lbl_CodLavorazioneStampa" runat="server" style="font-weight:bold"></asp:Label>
                        </td>
                                    
                        <td style="width: 34%;text-align:center">
                            <label>elenco offerte</label>
                        </td>
                        <td style="width: 33%; padding:5px;">
                            <label>Rif. prot.</label>
                            <asp:Label ID="lbl_ProtocolloStampa" runat="server"></asp:Label>
                        </td>
                            
                    </tr>

                </table>
            </div>
            <%--FINE STAMPA--%>

            <div id="articoliStampa" runat="server" class="w3-row w3-section w3-padding w3-small">

                <asp:GridView ID="gvArticoli" runat="server" AutoGenerateColumns="False"
                    Style="font-size: 8pt; max-height: 200px; width: 100%; position: relative; background-color: #FFF; text-align: center"
                    HeaderStyle-BackColor="#2196F3" HeaderStyle-Font-Bold="true" HeaderStyle-ForeColor="White" BorderWidth="0"
                    GridLines="None" OnRowDataBound="gvArticoli_RowDataBound" >
                    <Columns>
                        <asp:TemplateField HeaderStyle-Width="20%" ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Top" ItemStyle-CssClass="cella" >
                            <HeaderTemplate>
                                <div style="text-align: center;">Codice</div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblCodice" Text='<%# Eval("Descrizione") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Descrizione" HeaderStyle-Width="40%" ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Top" ItemStyle-CssClass="cella">
                            <HeaderTemplate>
                                <div style="text-align: center;">Descrizione</div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblDescrizione" Text='<%# Eval("DescrizioneLunga") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField DataField="Prezzo" HeaderText="Listino" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Right" ItemStyle-VerticalAlign="Top" ItemStyle-CssClass="cella" />
                        <asp:BoundField DataField="Costo" HeaderText="Costo" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Right" ItemStyle-VerticalAlign="Top" ItemStyle-CssClass="cella" />
                        <asp:BoundField DataField="Quantita" HeaderText="Q.tà" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="6%" ItemStyle-HorizontalAlign="Right" ItemStyle-VerticalAlign="Top" ItemStyle-CssClass="cella" />
                        <asp:BoundField DataField="Iva" HeaderText="Iva" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="6%" ItemStyle-HorizontalAlign="Right" ItemStyle-VerticalAlign="Top" ItemStyle-CssClass="cella" />
                        <asp:TemplateField HeaderText="Totale" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="8%" ItemStyle-HorizontalAlign="Right" ItemStyle-VerticalAlign="Top" ItemStyle-CssClass="cella">
                            <ItemTemplate>
                                <asp:Label ID="totaleRiga" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>

            <!--
            <div id="totaliSchermo" visible="false"  runat="server">
                <div class="w3-row  w3-small">
                    <div class="w3-col">
                        <div class="w3-twothird">&nbsp;</div>
                        <div class="w3-third">
                            <div class="w3-half" style="padding-left: 50px;">
                                <label><b>Totale</b></label>
                            </div>
                            <div class="w3-half" style="text-align: right; padding-right: 20px;">
                                <b>
                                    <asp:Label ID="totale" runat="server" /></b>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="w3-row w3-small">
                    <div class="w3-col">
                        <div class="w3-twothird">&nbsp;</div>
                        <div class="w3-third">
                            <div class="w3-half" style="padding-left: 50px;">
                                <label><b>Totale i.v.a.</b></label>
                            </div>
                            <div class="w3-half" style="text-align: right; padding-right: 20px;">
                                <b>
                                    <asp:Label ID="totaleIVA" runat="server" /></b>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="w3-row  w3-small">
                    <div class="w3-col">
                        <div class="w3-twothird">&nbsp;</div>
                        <div class="w3-third">
                            <div class="w3-half" style="padding-left: 50px;">
                                <label><b>Totale Euro</b></label>
                            </div>
                            <div class="w3-half" style="text-align: right; padding-right: 20px;">
                                <b>
                                    <asp:Label ID="totaleEuro" runat="server" /></b>
                            </div>
                        </div>
                    </div>
                </div>

            </div>
            -->
            <div id="totaliStampa" style="width: 100%; font-size: 8pt" runat="server" visible="true">
                <table style="width: 100%">
                    <tr>
                        <td style="width: 23%; text-align: left; padding-left: 20px;vertical-align:top;">
                            <b><asp:Label ID="Label1" runat="server" Text="Note:" Font-Bold="true" /></b></td>
                        <td style="width: 47%; text-align: left; padding-left: 5px;padding-right:5px;border:solid 1px #000;vertical-align:top;">
                            <asp:Label runat="server" Text="" ID="note"></asp:Label>
                        </td>
                        <td style="width: 30%;vertical-align:top;">
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 60%;border:solid 1px #000;">
                                        <label><b>Totale</b></label></td>
                                    <td style="width: 40%; text-align: right; padding-right: 20px;border:solid 1px #000;">
                                        <b>
                                            <asp:Label ID="totaleStampa" runat="server" /></b>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 60%;border:solid 1px #000;">
                                        <label><b>Totale i.v.a.</b></label></td>
                                    <td style="width: 40%; text-align: right; padding-right: 20px;border:solid 1px #000;">
                                        <b>
                                            <asp:Label ID="totaleIVAStampa" runat="server" /></b>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 60%;border:solid 1px #000;">
                                        <label><b>Totale Euro</b></label></td>
                                    <td style="width: 40%; text-align: right; padding-right: 20px;border:solid 1px #000;">
                                        <b>
                                            <asp:Label ID="totaleEuroStampa" runat="server" /></b>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
            <!--
            <div id="footerSchermo" visible="false"  style="margin-left: 10px; margin-right: 10px; margin-top: 25px; position: relative; width: 98%; font-size: 8pt" runat="server">
                <table style="width: 100%;">
                    <tr>
                        <td style="width: 16%; background-color: #2196F3; color: white; border: solid 1px #fff; text-align: center">Banca</td>
                        <td style="width: 50%; background-color: #DDD; border: solid 1px #fff">
                            <asp:Label ID="val_bancaSchermo" runat="server" />
                        </td>
                        <td style="width: 34%; background-color: #2196F3; color: white; border: solid 1px #fff"><b>Timbro e firma per accettazione</b></td>
                    </tr>
                    <tr>
                        <td style="width: 16%; background-color: #2196F3; color: white; border: solid 1px #fff; text-align: center">Pagamento</td>
                        <td style="width: 50%; background-color: #DDD; border: solid 1px #fff">
                            <asp:Label ID="val_pagamentoSchermo" runat="server" />
                            
                        </td>
                        <td style="width: 34%; background-color: #DDD; color: white; border: solid 1px #fff" rowspan="2">&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width: 16%; background-color: #2196F3; color: white; border: solid 1px #fff; text-align: center">Consegna</td>
                        <td style="width: 50%; background-color: #DDD; border: solid 1px #fff">
                            <asp:Label ID="val_consegnaSchermo" runat="server" />
                        </td>
                    </tr>
                </table>

                <div style="padding: 10px; position: relative; font-size: 8pt; text-align: center;">
                    <b>Videosystem Production srl&nbsp;&nbsp;P.IVA 13121341005<br />
                        Sede legale:  Via Tavernelle Val di Pesa 34 - 00148 Roma RM&nbsp;&nbsp;e-mail: info@vsproduction.it</b>
                </div>

            </div>
            -->
            <div id="footerStampa" style="margin: 10px; margin-top: 25px; position: absolute; bottom: -30px; width: 98%; font-size: 8pt" runat="server" visible="true">
                <table style="width: 100%;">
                    <tr>
                        <td style="width: 16%; background-color: #2196F3; color: white; border: solid 1px #fff; text-align: center">Banca</td>
                        <td style="width: 50%; background-color: #DDD; border: solid 1px #fff">
                            <asp:Label ID="val_bancaStampa" runat="server" />
                        </td>
                        <td style="width: 34%; background-color: #2196F3; color: white; border: solid 1px #fff"><b>Timbro e firma per accettazione</b></td>
                    </tr>
                    <tr>
                        <td style="width: 16%; background-color: #2196F3; color: white; border: solid 1px #fff; text-align: center">Pagamento</td>
                        <td style="width: 50%; background-color: #DDD; border: solid 1px #fff">
                            <asp:Label ID="val_pagamentoStampa" runat="server" />
                        </td>
                        <td style="width: 34%; background-color: #DDD; color: white; border: solid 1px #fff" rowspan="2">&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width: 16%; background-color: #2196F3; color: white; border: solid 1px #fff; text-align: center">Consegna</td>
                        <td style="width: 50%; background-color: #DDD; border: solid 1px #fff">
                            <asp:Label ID="val_consegnaStampa" runat="server" />
                        </td>
                    </tr>
                </table>

                <table style="padding: 10px; position: relative;">
                    <tr>
                        <td style="width: 90%; text-align: center; font-size: 8pt;">
                            <b>Videosystem Production srl&nbsp;&nbsp;P.IVA 13121341005<br />
                                Sede legale:  Via Tavernelle Val di Pesa 34 - 00148 Roma RM&nbsp;&nbsp;e-mail: info@vsproduction.it</b>

                        </td>
                        <td style="width: 10%">
                            <asp:Image ID="imgDNV" runat="server" ImageUrl="~/Images/DNV_2008_ITA2.jpg" Style="height: 80px" />
                        </td>

                    </tr>

                </table>
            </div>
        </div>


        <div id="DivFramePdf" runat="server" style=" width:100%; height:90%;" >
            <%--<iframe id="framePdf" runat="server" src="" allowTransparency="true" frameborder="0" style="overflow:hidden; width:100%; height:600px"></iframe>--%>
            <iframe id="framePdf" runat="server" src="" style=" width:100%; height:100%;"></iframe>
        </div>


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
                    <%--<asp:TextBox ID="txt_Banca" runat="server" class="w3-input w3-border" MaxLength="60" placeholder="Banca" Style="padding: 2px;"></asp:TextBox>--%>
                    <asp:DropDownList ID="ddl_Banca" runat="server"></asp:DropDownList>
                </div>
            </div>
            <div class="w3-row" style="padding: 5px;">
                <div class="w3-quarter">
                    <label style="margin-bottom: 0.2rem;">Pagamento</label>
                </div>
                <div class="w3-threequarter">
                    <%--<asp:TextBox ID="txt_Pagamento" runat="server" class="w3-input w3-border" placeholder="Pagamento" Style="padding: 2px;"></asp:TextBox>--%>
                    <asp:DropDownList ID="cmbMod_Pagamento" runat="server" class="w3-input w3-border"></asp:DropDownList>
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
                    <asp:TextBox ID="txt_Note" runat="server" class="w3-input w3-border" placeholder="Note" Style="padding: 2px;" Rows="5" TextMode="MultiLine"></asp:TextBox>
                </div>
            </div>
        </div>

        <div class="w3-center" style="margin: 10px">
            <asp:Button ID="btnOKModificaNote" runat="server" Text="OK" class=" w3-btn w3-white w3-border w3-border-green w3-round-large" Style="font-size: smaller; padding: 4px 8px" OnClick="btnOKModificaNote_Click" />
            <button onclick="document.getElementById('panelModificaNote').style.display='none'" type="button" class=" w3-btn w3-white w3-border w3-border-red w3-round-large" style="font-size: smaller; padding: 4px 8px">Annulla</button>
        </div>
    </div>
</div>

