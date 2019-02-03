<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="prova.aspx.cs" Inherits="VideoSystemWeb.Agenda.prova" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>



    <script type="text/javascript" src="/Scripts/jquery-3.3.1.min.js"></script>
    <script type="text/javascript" src='/Scripts/moment-with-locales.js'></script>
    <%--<script type="text/javascript" src="/Scripts/transition.js"></script>
    <script type="text/javascript" src="/Scripts/collapse.js"></script>--%>
    <script type="text/javascript" src='/Scripts/bootstrap.min.js'></script>
    <script type="text/javascript" src="/Scripts/bootstrap-datetimepicker.min.js"></script>


    <link rel="stylesheet" href="/Content/bootstrap.css" />
    <link rel="stylesheet" href="/Css/bootstrap-datetimepicker.min.css" />




</head>
<body>
  <div class="container">
    <div class="row">
        <div class='col-sm-6'>
            <div class="form-group">
                <div class='input-group date' id='datetimepicker3'>
                    <input type='text' class="form-control" />
                    <span class="input-group-addon">
                        <span class="glyphicon glyphicon-time"></span>
                    </span>
                </div>
            </div>
        </div>
        <script type="text/javascript">
            $(function () {
                $('#datetimepicker3').datetimepicker({
                    format: 'LT'
                });
            });
        </script>
    </div>
</div>
    <br /><br /><br /><br /><br />
    <div class="container">
    <div class="row">
        <div class='col-sm-6'>
            <input type='text' class="form-control" id='datetimepicker5' />
        </div>
        <script type="text/javascript">
            $(function () {
                $('#datetimepicker5').datetimepicker({
                    format: 'LT'
                });
            });
        </script>
    </div>
</div>
     <br /><br /><br /><br /><br />
   <div class="container">
    <div class='col-md-5'>
        <div class="form-group">
            <div class='input-group date' id='datetimepicker6'>
                <input type='text' class="form-control" />
                <span class="input-group-addon">
                    <span class="glyphicon glyphicon-calendar"></span>
                </span>
            </div>
        </div>
    </div>
    <div class='col-md-5'>
        <div class="form-group">
            <div class='input-group date' id='datetimepicker7'>
                <input type='text' class="form-control" />
                <span class="input-group-addon">
                    <span class="glyphicon glyphicon-calendar"></span>
                </span>
            </div>
        </div>
    </div>
</div>
<script type="text/javascript">
    $(function () {
        $('#datetimepicker6').datetimepicker();
        $('#datetimepicker7').datetimepicker({
            useCurrent: false //Important! See issue #1075
        });
        $("#datetimepicker6").on("dp.change", function (e) {
            $('#datetimepicker7').data("DateTimePicker").minDate(e.date);
        });
        $("#datetimepicker7").on("dp.change", function (e) {
            $('#datetimepicker6').data("DateTimePicker").maxDate(e.date);
        });
    });
</script>
    <br /><br /><br /><br /><br />

<div style="overflow:hidden;">
    <div class="form-group">
        <div class="row">
            <div class="col-md-8">
                <div id="datetimepicker12"></div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(function () {
            $('#datetimepicker12').datetimepicker({
                inline: true,
                locale: 'it',
                format: 'DD/MM/YYYY'
            });
        });
    </script>
</div>







</body>
</html>
