<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Magazzino.aspx.cs" Inherits="VideoSystemWeb.MAGAZZINO.Magazzino" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $('.loader').hide();
        });

        // VISUALIZZO DATI SELEZIONATI DALLA GRIGLIA
        function mostracella(id, column, nomeCampo, headerCampo, valore) {
            $('.loader').show();
            $("#<%=hfIdRiga.ClientID%>").val(id);
            $("#<%=hfIdColonna.ClientID%>").val(column);
            $("#<%=hfNomeCampo.ClientID%>").val(nomeCampo);
            $("#<%=hfHeaderCampo.ClientID%>").val(headerCampo);
            $("#<%=hfValoreGriglia.ClientID%>").val(valore);
            $("#<%=btnEditEvent.ClientID%>").click();
            //alert('id: ' + id + ' colonna: ' + column + ' nome campo: ' + nomeCampo + ' header campo: ' + headerCampo);
        }

        // CHIUDO POPUP
        function chiudiPopup() {
            // QUANDO APRO IL POPUP RIPARTE SEMPRE DA MAGAZZINO E NON DALL'ULTIMA TAB APERTA
            $("#<%=hfIdRiga.ClientID%>").val('');
            $("#<%=hfIdColonna.ClientID%>").val('');
            $("#<%=hfNomeCampo.ClientID%>").val('');
            $("#<%=hfHeaderCampo.ClientID%>").val('');
            $("#<%=hfValoreGriglia.ClientID%>").val('');
            $("#<%=btnChiudiPopupServer.ClientID%>").click();

        }

        // APRO LE TAB DETTAGLIO MAGAZZINO
        function openTabMagazzino(tipoName) {
            if (document.getElementById(tipoName) != undefined) {
                var i;
                var x = document.getElementsByClassName("maga");
                for (i = 0; i < x.length; i++) {
                    x[i].style.display = "none";
                }
                document.getElementById(tipoName).style.display = "block";
            }
        }

        // AZZERO TUTTI I CAMPI RICERCA
        function azzeraCampiRicerca() {
            $("#<%=tbCodiceVS.ClientID%>").val('');
            $("#<%=ddlTipoCategoria.ClientID%>").val('');
            $("#<%=ddlTipoSubCategoria.ClientID%>").val('');
            $("#<%=tbDescrizione.ClientID%>").val('');
            $("#<%=tbSeriale.ClientID%>").val('');
            $("#<%=ddlTipoGruppoMagazzino.ClientID%>").val('');
            $("#<%=ddlTipoPosizioneMagazzino.ClientID%>").val('');
            $("#<%=tbMarca.ClientID%>").val('');
            $("#<%=tbModello.ClientID%>").val('');
        }

        // SELEZIONO I DATI PER LA MODIFICA DELLA RIGA
        function selezionaAttrezzaturaDaInserire(idAttrezzatura, valoreAttrezzature) {
            $('.loader').show();
            $("#<%=hfIdAttrezzaturaDaModificare.ClientID%>").val(idAttrezzatura);
            $("#<%=hfValoreAttrezzaturaDaModificare.ClientID%>").val(valoreAttrezzature);

            $("#<%=btnAggiornaRigaAgendaMagazzino.ClientID%>").click();
        }


    </script>
    <style type="text/css">
      .hiddencol
      {
        display: none;
      }
    </style>

    <label><asp:Label ID="lblMagazzino" runat="server" Text="MAGAZZINO" ForeColor="SteelBlue"></asp:Label></label>

    <asp:UpdatePanel ID="UpdatePanelMagazzino" runat="server">
        <ContentTemplate>

            <%-- HiddenField per selezione campi da modificare --%>
            <asp:HiddenField ID="hfIdRiga" runat="server" Value="" />
            <asp:HiddenField ID="hfIdColonna" runat="server" Value="" />
            <asp:HiddenField ID="hfNomeCampo" runat="server" Value="" />
            <asp:HiddenField ID="hfHeaderCampo" runat="server" Value="" />
            <asp:HiddenField ID="hfValoreGriglia" runat="server" Value="" />

            <%-- HiddenField per selezione valore e id campo da modificare --%>
            <asp:HiddenField ID="hfIdAttrezzaturaDaModificare" runat="server" Value="" />
            <asp:HiddenField ID="hfValoreAttrezzaturaDaModificare" runat="server" Value="" />

            <div class="w3-row-padding">
                <div class="w3-quarter">
                    <label style="font-weight:bold">Cliente</label>
                    <asp:label ID="lbl_Cliente" runat="server"></asp:label>
                </div>
                <div class="w3-threequarter">
                    <div class="w3-half">
                        <label style="font-weight:bold">Lavorazione</label>
                        <asp:label ID="lbl_Lavorazione" runat="server"></asp:label>
                    </div>
                    <div class="w3-half">
                        <label style="font-weight:bold">Produzione</label>
                        <asp:label ID="lbl_Produzione" runat="server"></asp:label>
                    </div>
                </div>
                
            </div>
            <div class="w3-row-padding">
                <div class="w3-quarter">
                    <label style="font-weight:bold">Tipologia</label>
                    <asp:label ID="lbl_Tipologia" runat="server"></asp:label>   
                </div>
                <div class="w3-quarter">
                    <label style="font-weight:bold">Data inizio lavorazione</label>
                    <asp:label ID="lbl_DataInizio" runat="server"></asp:label>   
                </div>
                <div class="w3-quarter">
                    <label style="font-weight:bold">Data fine lavorazione</label>
                    <asp:label ID="lbl_DataFine" runat="server"></asp:label>   
                </div>
                <div class="w3-quarter">
                    <label style="font-weight:bold">Codice lavoro</label>
                    <asp:label ID="lbl_CodLavoro" runat="server"></asp:label>
                </div>
            </div>
            <br />
            <div class="w3-bar w3-light-grey">
                <asp:Button ID="btnInserisciRiga" runat="server" class="w3-bar-item w3-btn w3-white w3-border w3-border-green w3-round-large" OnClick="btnInserisciRiga_Click" OnClientClick="$('.loader').show();" Text="Inserisci Riga" />
                <asp:Button ID="btnStampa" runat="server" class="w3-bar-item w3-btn w3-white w3-border w3-border-blue w3-round-large w3-right" OnClientClick="$('.loader').show();" Text="Stampa" OnClick="btnStampa_Click" />
            </div>

            <br />
            <div class="round">
                
                <asp:GridView ID="gv_attrezzature" runat="server" Style="font-size: 10pt; width: 100%; position: relative; background-color: #EEF1F7;" CssClass="grid" OnRowDataBound="gv_attrezzature_RowDataBound" AllowPaging="True" OnPageIndexChanging="gv_attrezzature_PageIndexChanging" OnRowCommand="gv_attrezzature_RowCommand" PageSize="20" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center" AutoGenerateColumns="false">
                    <Columns>
                        <asp:TemplateField ShowHeader="False" HeaderStyle-Width="2%">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgDelete" runat="server" CausesValidation="false" Text="Apri" ImageUrl="~/Images/delete.png" ToolTip="Cancella Riga" ImageAlign="AbsMiddle" Height="30px" CommandName="ELIMINA_RIGA" CommandArgument='<%#Eval("id")%>' OnClientClick="return confirm('Confermi cancellamento riga?');" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    
                        <asp:BoundField DataField="id" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"/>
                        
                        <asp:BoundField DataField="descrizione_Camera" HeaderText="Descrizione" HeaderStyle-Width="8%" ItemStyle-HorizontalAlign="Center"/>
                        <asp:BoundField DataField="nome_Camera" HeaderText="Camera" HeaderStyle-Width="8%" ItemStyle-HorizontalAlign="Center"/>
                        <asp:BoundField DataField="nome_Fibra_Trax" HeaderText="Fibra/Trax" HeaderStyle-Width="8%" ItemStyle-HorizontalAlign="Center"/>
                        <asp:BoundField DataField="nome_Ottica" HeaderText="Ottica" HeaderStyle-Width="8%" ItemStyle-HorizontalAlign="Center"/>
                        <asp:BoundField DataField="nome_Viewfinder" HeaderText="Viewfinder" HeaderStyle-Width="8%" ItemStyle-HorizontalAlign="Center"/>
                        <asp:BoundField DataField="nome_Loop" HeaderText="Loop" HeaderStyle-Width="8%" ItemStyle-HorizontalAlign="Center"/>
                        <asp:BoundField DataField="nome_Mic" HeaderText="Mic" HeaderStyle-Width="8%" ItemStyle-HorizontalAlign="Center"/>
                        <asp:BoundField DataField="nome_Testa" HeaderText="Testa" HeaderStyle-Width="8%" ItemStyle-HorizontalAlign="Center"/>
                        <asp:BoundField DataField="nome_Lensholder" HeaderText="Lensholder" HeaderStyle-Width="8%" ItemStyle-HorizontalAlign="Center"/>
                        <asp:BoundField DataField="nome_Cavalletto" HeaderText="Cavalletto" HeaderStyle-Width="7%" ItemStyle-HorizontalAlign="Center"/>
                        <asp:BoundField DataField="nome_Cavi" HeaderText="Cavi" HeaderStyle-Width="7%" ItemStyle-HorizontalAlign="Center"/>
                        <asp:BoundField DataField="nome_Altro1" HeaderText="Altro 1" HeaderStyle-Width="6%" ItemStyle-HorizontalAlign="Center"/>
                        <asp:BoundField DataField="nome_Altro2" HeaderText="Altro 2" HeaderStyle-Width="6%" ItemStyle-HorizontalAlign="Center"/>

                        <%--COLONNE CON ID NON VISIBILI--%>
                        <asp:BoundField DataField="id_Camera" HeaderText="id_Camera" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"  />
                        <asp:BoundField DataField="id_Fibra_Trax" HeaderText="id_Fibra_Trax" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                        <asp:BoundField DataField="id_Ottica" HeaderText="id_Ottica" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                        <asp:BoundField DataField="id_Viewfinder" HeaderText="id_Viewfinder" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                        <asp:BoundField DataField="id_Loop" HeaderText="id_Loop" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                        <asp:BoundField DataField="id_Mic" HeaderText="id_Mic" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                        <asp:BoundField DataField="id_Testa" HeaderText="id_Testa" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                        <asp:BoundField DataField="id_Lensholder" HeaderText="id_Lensholder" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                        <asp:BoundField DataField="id_Cavalletto" HeaderText="id_Cavalletto" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                        <asp:BoundField DataField="id_Cavi" HeaderText="id_Cavi" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                        <asp:BoundField DataField="id_Cavi" HeaderText="id_Cavi" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                        <asp:BoundField DataField="id_Altro2" HeaderText="id_Altro2" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />                    
                    
                    </Columns>
                </asp:GridView>
            </div>
            <br />

            <div class="w3-bar w3-light-grey">
                <label class="w3-bar-item" style="font-weight:bold">Note</label>
                <asp:Button ID="btnAggiornaNote" runat="server" class="w3-bar-item w3-btn w3-white w3-border w3-border-red w3-round-large w3-right" OnClick="btnAggiornaNote_Click" OnClientClick="$('.loader').show();" Text="Salva Note" />
            </div>

            <div class="w3-row-padding w3-margin-top">
                <asp:TextBox ID="txt_Note" runat="server" Rows="5" TextMode="MultiLine" width="100%"/>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:Button runat="server" ID="btnEditEvent" Style="display: none" OnClick="btnEditEvent_Click" />
    <asp:Button runat="server" ID="btnAggiornaRigaAgendaMagazzino" Style="display: none" OnClick="btnAggiornaRigaAgendaMagazzino_Click" />
    <asp:Button runat="server" ID="btnChiudiPopupServer" Style="display: none" OnClick="btnChiudiPopupServer_Click" />

    <asp:UpdatePanel ID="upEvento" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
        <ContentTemplate>
            <div runat="server" id="pnlContainer" style="display: none">
                <div class="modalBackground"></div>
                <asp:Panel runat="server" ID="innerContainer" CssClass="containerPopupStandard round" ScrollBars="Auto">
                    <div class="w3-container w3-center w3-xlarge">
                        GESTIONE ELEMENTI MAGAZZINO
                    </div>
                    <br />

                    <div class="w3-container">
                        <!-- ELENCO TAB DETTAGLI MAGAZZINO -->
                        <div class="w3-bar w3-yellow w3-round">
                            <div class="w3-bar-item w3-button w3-yellow" onclick="openTabMagazzino('Magazzino')">Magazzino</div>
                            <div class="w3-bar-item w3-button w3-yellow w3-right">
                                <div id="btnChiudiPopup" class="w3-button w3-yellow w3-small w3-round" onclick="chiudiPopup();">Chiudi</div>
                            </div>
                        </div>
                    </div>
                    <!-- TAB ELEMENTI MAGAZZINO -->
                    <div id="Magazzino" class="w3-container w3-border maga" style="display: block">
                        <%--<label>Protocolli</label>--%>
                        <div class="w3-container">
                            <p>
                                <div class="w3-row-padding">
                                    <div class="w3-quarter">
                                        <label style="font-weight:bold">Id Lav.Magazzino</label>
                                        <asp:label ID="lblNumeroRiga" runat="server"></asp:label>
                                    </div>
                                    <div class="w3-quarter">
                                        <label style="font-weight:bold">Colonna Griglia</label>
                                        <asp:label ID="lblNumeroColonna" runat="server"></asp:label>
                                    </div>
                                    <div class="w3-quarter">
                                        <label style="font-weight:bold">Campo</label>
                                        <asp:label ID="lblNomeCampo" runat="server"></asp:label>
                                    </div>
                                    <div class="w3-quarter">
                                        <label style="font-weight:bold">Header</label>
                                        <asp:label ID="lblHeaderCampo" runat="server"></asp:label>
                                    </div>
                                </div>
                                <br />

                                <asp:PlaceHolder ID="phDescrizioneCamera" runat="server" Visible="false">
                                    <div class="w3-row-padding">
                                        <div class="w3-quarter">
                                            <label style="font-weight:bold">Descrizione Camera:</label>
                                            <asp:TextBox ID="tbModDescrizioneCamera" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="w3-quarter">
                                            &nbsp;
                                        </div>
                                        <div class="w3-quarter">
                                            &nbsp;
                                        </div>
                                        <div class="w3-quarter">
                                            <asp:Button ID="btnAggiornaDescrizioneCamera" runat="server" class="w3-btn w3-white w3-border w3-border-green w3-round-large" OnClick="btnAggiornaDescrizioneCamera_Click" OnClientClick="$('.loader').show();" Text="Aggiorna" />
                                        </div>
                                    </div>
                                </asp:PlaceHolder>
                                <asp:PlaceHolder ID="phModificaAttrezzature" runat="server" Visible="false">
                                    <%-- RICERCA ATTREZZATURA --%>
                                    <asp:UpdatePanel ID="UpdatePanelRicerca" runat="server">
                                        <ContentTemplate>
                                            <div class="w3-row-padding">
                                                <div class="w3-quarter">
                                                    <label>Codice Video System</label>
                                                    <asp:TextBox ID="tbCodiceVS" runat="server" MaxLength="20" class="w3-input w3-border" placeholder=""></asp:TextBox>
                                                </div>
                                                <div class="w3-quarter">
                                                    <label>Categoria</label>
                                                    <asp:DropDownList ID="ddlTipoCategoria" runat="server" AutoPostBack="True" Width="100%" class="w3-input w3-border" OnSelectedIndexChanged="ddlTipoCategoria_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="w3-quarter">
                                                    <label>Sub Categoria</label>
                                                    <asp:DropDownList ID="ddlTipoSubCategoria" runat="server" AutoPostBack="True" Width="100%" class="w3-input w3-border" OnSelectedIndexChanged="ddlTipoSubCategoria_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="w3-quarter">
                                                    <label>Gruppo</label>
                                                    <asp:DropDownList ID="ddlTipoGruppoMagazzino" runat="server" AutoPostBack="True" Width="100%" class="w3-input w3-border">
                                                    </asp:DropDownList>   
                                                </div>
                                            </div>
                                            <div class="w3-row-padding">
                                                <div class="w3-quarter">
                                                    <label>Posizione Magazzino</label>
                                                    <asp:DropDownList ID="ddlTipoPosizioneMagazzino" runat="server" AutoPostBack="True" Width="100%" class="w3-input w3-border">
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="w3-quarter">
                                                    <label>Seriale</label>
                                                    <asp:TextBox ID="tbSeriale" runat="server" MaxLength="20" class="w3-input w3-border" placeholder=""></asp:TextBox>
                                                </div>
                                                <div class="w3-half">
                                                    <label>Descrizione</label>
                                                    <asp:TextBox ID="tbDescrizione" runat="server" MaxLength="100" class="w3-input w3-border" placeholder=""></asp:TextBox>
                                                </div>

                                            </div>
                                            <div class="w3-row-padding">
                                                <div class="w3-half">
                                                    <label>Marca</label>
                                                    <asp:TextBox ID="tbMarca" runat="server" MaxLength="100" class="w3-input w3-border" placeholder=""></asp:TextBox>
                                                </div>
                                                <div class="w3-half">
                                                    <label>Modello</label>
                                                    <asp:TextBox ID="tbModello" runat="server" MaxLength="100" class="w3-input w3-border" placeholder=""></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="w3-row-padding w3-margin-bottom">
                                                <div class="w3-half">
                                                    <label>&nbsp;</label>
                                                    &nbsp;
                                                </div>
                                                <div class="w3-quarter" style="position:relative;">
                                                    <label>&nbsp;</label>
                                                    &nbsp;
                                                </div>
                                                <div class="w3-quarter">
                                                    <label>&nbsp;</label>
                                                    <table style="width: 100%;">
                                                        <tr>
                                                            <td style="width: 80%;">
                                                                <asp:Button ID="btnRicercaAttrezzatura" runat="server" class="w3-btn w3-white w3-border w3-border-green w3-round-large" OnClick="btnRicercaAttrezzatura_Click" OnClientClick="$('.loader').show();" Text="Ricerca" />
                                                            </td>
                                                            <td style="width: 20%;">
                                                                <asp:Button ID="BtnPulisciCampiRicerca" runat="server" class="w3-btn w3-circle w3-red" Text="&times;" OnClientClick="azzeraCampiRicerca();" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                            <div class="round">
                                                <asp:GridView ID="gvAttrezzatureDaInserire" runat="server" Style="font-size: 10pt; width: 100%; position: relative; background-color: #EEF1F7;" CssClass="grid" OnRowDataBound="gvAttrezzatureDaInserire_RowDataBound" AllowPaging="True" OnPageIndexChanging="gvAttrezzatureDaInserire_PageIndexChanging" PageSize="20" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center">
                                                </asp:GridView>
                                            </div>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnRicercaAttrezzatura" EventName="Click" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </asp:PlaceHolder>
                                <asp:Label ID="lblStatus" runat="server" Style="font-family: Arial; font-size: small;"></asp:Label>
                                <br />
                                <div style="text-align: center;">
<%--                                <asp:Button ID="btnGestisciProtocollo" runat="server" Text="Gestisci Protocollo" class="w3-panel w3-green w3-border w3-round" OnClick="btnGestisciProtocollo_Click" />
                                    <asp:Button ID="btnInserisciProtocollo" runat="server" Text="Inserisci Protocollo" class="w3-panel w3-green w3-border w3-round" OnClick="btnInserisciProtocollo_Click" OnClientClick="return confirm('Confermi inserimento Protocollo?')" />
                                    <asp:Button ID="btnModificaProtocollo" runat="server" Text="Modifica Protocollo" class="w3-panel w3-green w3-border w3-round" OnClick="btnModificaProtocollo_Click" OnClientClick="return confirm('Confermi modifica Protocollo?')" Visible="false" />
                                    <asp:Button ID="btnEliminaProtocollo" runat="server" Text="Elimina Protocollo" class="w3-panel w3-green w3-border w3-round" OnClick="btnEliminaProtocollo_Click" OnClientClick="return confirm('Confermi eliminazione Protocollo?')" Visible="false" />
                                    <asp:Button ID="btnAnnullaProtocollo" runat="server" Text="Annulla" class="w3-panel w3-green w3-border w3-round" OnClick="btnAnnullaProtocollo_Click" />--%>
                                </div>
                                <p>
                                </p>
                            </p>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
