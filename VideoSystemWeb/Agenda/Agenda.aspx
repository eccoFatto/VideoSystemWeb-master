<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Agenda.aspx.cs" Inherits="VideoSystemWeb.Agenda" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(function () {
            //dichiarazione variabili
            var date = new Date();
            var d = date.getDate();
            var m = date.getMonth();
            var y = date.getFullYear();
            var calEditabile = <%=isCalendarEditable%>;


            // configurazione calendario
            $('#calendar').fullCalendar({
                themeSystem: 'bootstrap4',
                defaultView: 'month',
                height: 500,
                locale: 'it',
                editable: calEditabile, 
                dayClick: function (date, jsEvent, view) {
                    if (calEditabile) {
                        var nomeEvento = prompt('Aggiungere il nome dell\'evento');
                        $('#calendar').fullCalendar('renderEvent', {
                            title: nomeEvento,
                            start: date,
                            allDay: true
                        });

                        alert('Aggiornamento database...');
                    }
                },
                customButtons: {
                    addActivity: {
                        text: 'Aggiungi attività',
                        click: function() {
                        alert('Aggiungi attività!');
                        }
                    },
                    addEventButton: {
                        text: 'Aggiungi evento...',
                        click: function () {
                            $("ModaleEvento").show();
                            var dateStr = prompt('Aggiungere una data in formato YYYY-MM-DD');
                            
                            var date = moment(dateStr);

                            if (date.isValid()) {
                                var nomeEvento = prompt('Aggiungere il nome dell\'evento');
                                $('#calendar').fullCalendar('renderEvent', {
                                    title: nomeEvento,
                                    start: date,
                                    allDay: true
                                });
                                alert('Aggiornamento database...');
                            } else {
                                alert('Data non valida');
                            }
                        }
                  }
                },
                buttonText: {
                    prev: '<',
                    next: '>'
                    
                },
                header: {
                  left: 'prev,next today addActivity addEventButton',
                  center: 'title',
                  right: 'month,agendaWeek,agendaDay,listMonth'
                },
                //events: 'https://fullcalendar.io/demo-events.json',
                eventRender: function(eventObj, $el) {
                  $el.popover({
                    title: eventObj.title,
                    content: eventObj.description,
                    trigger: 'hover',
                    placement: 'top',
                    container: 'body'
                  });
                },
                //events: < %=eventi%>,     
                eventClick: function(calEvent, jsEvent, view) {
                    alert('Event: ' + calEvent.title);
                    alert('View: ' + view.name);

                    // change the border color just for fun
                    $(this).css('border-color', 'red');

                }
              })
        });

    </script>

    <div id='calendar'></div>

    <div class="modal fade" id="ModaleEvento" tabindex="-1" role="dialog" aria-labelledby="ModaleEventoTitolo" aria-hidden="true">






        <div class="modal-dialog modal-lg" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title">
                    <label id="ModaleEventoTitolo">
                        Modifica Evento</label>
                </h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close" >
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <div class="modal-body">
                <div class="row">
                    PROVA1
                </div>
                <div class="row">
                    PROVA2

                </div>
                <div class="row">
                    PROVA3
                </div>
                <div class="row">
                    PROVA4
                </div>
                <div class="row">
                    PROVA5
                </div>
            </div>
            <div class="modal-footer">
                <button type="button" id="btnChiudiEvento" class="btn btn-secondary btn-sm bottonePiccolo"
                    data-dismiss="modal" >
                    Chiudi</button>
                <button type="button" id="btnSalva" class="btn btn-success btn-sm bottonePiccolo">
                    Salva</button>
            </div>
        </div>
    </div>

    </div>
</asp:Content>
