<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportCollaboratoriPerGiornataExt.aspx.cs" Inherits="VideoSystemWeb.REPORT.ReportCollaboratoriPerGiornataExt" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="repcoll" TagName="ReportCollaboratori" Src="~/REPORT/userControl/collaboratoriPerGiornata.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Report Collaboratori per Giornata</title>

    <link href="~/favicon.ico" rel="shortcut icon" type="image/x-icon" />

    <script type="text/javascript" src='/Scripts/Utility.js'></script>
    <script type="text/javascript" src="/Scripts/jquery-3.3.1.min.js"></script>
    <script type="text/javascript" src='/Scripts/moment-with-locales.js'></script>
    <script type="text/javascript" src='/Scripts/popper.min.js'></script>
    <script type="text/javascript" src='/Scripts/bootstrap.min.js'></script>
    <script type="text/javascript" src="/Scripts/bootstrap-datetimepicker.min.js"></script>
    <script type="text/javascript" src="/Scripts/jquery.easy-autocomplete.min.js"></script>

    <link rel="stylesheet" href="/Css/bootstrap.css" />
    <link rel="stylesheet" href="/Css/bootstrap-glyphicons.css" />
    <link rel="stylesheet" href="/Css/bootstrap-datetimepicker.min.css" />
    <link rel="stylesheet" href="/Css/w3.css" />
    <link rel="stylesheet" href="/Css/w3-colors-win8.css" />
    <link rel="stylesheet" href="/Css/easy-autocomplete.min.css" />
    <link rel='stylesheet' href='/Css/Style.css' />

</head>
<body>
    <form id="frmCollaboratoriPerGiornata" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" EnablePartialRendering="true" runat="server" />
        <div class="container body-content" style="height: 90%; width: 100%; max-width: none">
            <repcoll:ReportCollaboratori id="controlCollab" runat="server"></repcoll:ReportCollaboratori>
        </div>
    </form>
</body>
</html>
