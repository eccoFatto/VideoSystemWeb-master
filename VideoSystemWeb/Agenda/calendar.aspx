<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="calendar.aspx.cs" Inherits="VideoSystemWeb.Agenda.calendar" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script src="http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.10.0.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="http://maxcdn.bootstrapcdn.com/bootstrap/3.3.2/js/bootstrap.min.js"></script>
    <link rel="stylesheet" href="http://maxcdn.bootstrapcdn.com/bootstrap/3.3.2/css/bootstrap.min.css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.5.1/css/bootstrap-datepicker.css" />
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.5.1/js/bootstrap-datepicker.min.js"></script>
    <style type="text/css">
        .table-condensed
        {
            width: 200px;
            height: 10px;
            font-size: 12px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <br />
    <br />
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div class="row">
                    <div class="col-md-4">
                        <div id='datepicker' class="input-group date datepicker col-md-6" data-date="" data-link-field="dtp_input2"
                            style="max-width: 220px">
                            <asp:TextBox ID="txtBookDate" runat="server" CssClass="form-control" size="16" type="text"
                                value="" AutoPostBack="true"  />
                            <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span>
                            </span>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <script type="text/javascript">
        //On Page Load.
        $(function () {
            SetDatePicker();
        });
 
        //On UpdatePanel Refresh.
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    SetDatePicker();
                    $(".datepicker-orient-bottom").hide();
                }
            });
        };
 
        function SetDatePicker() {
            $("#datepicker").datepicker();
            if ($("#txtBookDate").val() == "") {
                var dateNow = new Date();
                $('#datepicker').datepicker("setDate", dateNow);
            }
        }
    </script>
    </form>
</body>
</html>
