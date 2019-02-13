<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="prova.aspx.cs" Inherits="VideoSystemWeb.Agenda.prova" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>



    <script type="text/javascript" src="/Scripts/jquery-3.3.1.min.js"></script>
    <script type="text/javascript" src='/Scripts/moment-with-locales.js'></script>
    <script type="text/javascript" src='/Scripts/bootstrap.min.js'></script>
    <script type="text/javascript" src="/Scripts/bootstrap-datetimepicker.min.js"></script>


    <link rel="stylesheet" href="/Content/bootstrap.css" />
    <link rel="stylesheet" href="/Css/bootstrap-datetimepicker.min.css" />



    <script src='/Scripts/Utility.js'></script>

    <link rel="stylesheet" href="/Css/w3.css"/>
    <link rel="stylesheet" href="/Css/w3-colors-win8.css"/>


    <link rel='stylesheet' href='/Css/Style.css' />





    <link href="/Css/multi-select.css" media="screen" rel="stylesheet" type="text/css"/>
    <script src="/Scripts/jquery.multi-select.js" type="text/javascript"></script>

    <script>
        $(document).ready(function () {
            $('#my-select').multiSelect();
        });

    </script>
</head>
<body>
   <select multiple="multiple" id="my-select" name="my-select[]">
      <option value='elem_1'>elem 1</option>
      <option value='elem_2'>elem 2</option>
      <option value='elem_3'>elem 3</option>
      <option value='elem_4'>elem 4</option>
      <option value='elem_100'>elem 100</option>
    </select>


</body>
</html>
