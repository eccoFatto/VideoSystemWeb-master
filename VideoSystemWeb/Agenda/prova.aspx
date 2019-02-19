<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="prova.aspx.cs" Inherits="VideoSystemWeb.Agenda.prova" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>



    <script type="text/javascript" src="/Scripts/jquery-3.3.1.min.js"></script>
    <script type="text/javascript" src='/Scripts/moment-with-locales.js'></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.0/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="/Scripts/bootstrap-datetimepicker.min.js"></script>


    <link rel="stylesheet" href="/Content/bootstrap.css" />
    <link rel="stylesheet" href="/Css/bootstrap-datetimepicker.min.css" />



    <script src='/Scripts/Utility.js'></script>

    <link rel="stylesheet" href="/Css/w3.css" />
    <link rel="stylesheet" href="/Css/w3-colors-win8.css" />


    <link rel='stylesheet' href='/Css/Style.css' />


    <script src="/Scripts/jquery.easy-autocomplete.min.js"></script>
    <link rel="stylesheet" href="/Css/easy-autocomplete.min.css">

    <style>
        /* Style the input field */
        #myInput {
            padding: 20px;
            margin-top: -6px;
            border: 0;
            border-radius: 0;
            background: #f1f1f1;
        }
    </style>
</head>
<body>

    <div class="container">
        <h2>Filterable Dropdown</h2>
        <p>Open the dropdown menu and type something in the input field to search for dropdown items:</p>
        <p>Note that we have styled the input field to fit the dropdown items.</p>
        <div class="dropdown">
            <button class="btn btn-primary dropdown-toggle" type="button" data-toggle="dropdown">
                Dropdown Example
    <span class="caret"></span>
            </button>
            <ul class="dropdown-menu">
                <input class="form-control" id="myInput" type="text" placeholder="Search..">
                <li><a href="#">HTML</a></li>
                <li><a href="#">CSS</a></li>
                <li><a href="#">JavaScript</a></li>
                <li><a href="#">jQuery</a></li>
                <li><a href="#">Bootstrap</a></li>
                <li><a href="#">Angular</a></li>
            </ul>
        </div>
        <br />
        <br />
        <input id="basics" />
    </div>

    <script>
        $(document).ready(function () {
            $("#myInput").on("keyup", function () {
                var value = $(this).val().toLowerCase();
                $(".dropdown-menu li").filter(function () {
                    $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
                });
            });
        });

        var options = {
            data: ["blue", "green", "pink", "red", "yellow"],
            list: {
                match: {
                    enabled: true
                }
            }
        };
        $("#basics").easyAutocomplete(options);
    </script>

</body>
</html>
