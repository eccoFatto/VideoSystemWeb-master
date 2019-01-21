<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="griglia.aspx.cs" Inherits="VideoSystemWeb.griglia" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
  $(document).ready(function(){
        $("#list").jqGrid
        ({
            /*Definizione path file xml*/
            url: 'myxml.xml',
            datatype: "xml",
            colNames:["Squadra","Giocate","Vinte","Perse","Pareggiate","Punti"],
            /*Definizione column Model,tutti i tipi disponibili li potete trovare qua:
              trirand.com/jqgridwiki/doku.php?id=wiki:predefined_formatter
            */
            colModel :[
                          {name:'squadra', index:'squadra', width:100, search:true, xmlmap:"squadra"},
                          {name:'giocate', index:'giocate',width:100, xmlmap:"giocate"},
                          {name:'vinte', index:'vinte', width:100, xmlmap:"vinte"},
                          {name:'perse', index:'perse', width:100, xmlmap:"perse"},
                          {name:'pareggiate', index:'pareggiate', width:100, xmlmap:"pareggiate"},
                          {name:'punti', index:'punti', width:100, xmlmap:"punti"}
                      ],
            height:100,
            rowNum:10,
            rowList:[10,20,30],
            pager : '#gridpager',
            viewrecords: true,
            imgpath: 'css/ui-lightness/images',
            loadonce: true,
            xmlReader: {
                        /*radice del file xml*/
                        root : "NewDataSet",
                        row: "Table",
                        repeatitems: false,
                        id: "squadra"
                        },
            sortorder: "punti",
            caption:'Classifica'
        });
        /*modulo di ricerca,occorre selezionare il modulo in fase di download del plugin*/
        $("#list").jqGrid('filterToolbar', {stringResult: true, searchOnEnter: false, defaultSearch:"cn"});
    });
</script>
 
<center>
<!-- attraverso i relativi id, visualizzo la griglia -->
<table id="list"></table>
<div id="pager"></div>
</center>
</asp:Content>
