function setRowSelected(TableBody, table, callback) {
    // $(TableBody).dataTable().fnDraw();
    $(TableBody + ' tbody').off('click', 'tr');
    $(TableBody + ' tbody').on('click', 'tr', function () {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
        }
        else {
            table.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
        }
        var d = table.row(this).data();
        callback(d);
    });
}

function CallAjax(formdata, params, callback) {


    if (params.Method == undefined) {
        params.Method = "POST"
    }

    if (params.Async == undefined) {
        params.Async = true;
    }

    if (params.BlockPage == undefined) {
        params.BlockPage = true;
    }

    if (params.ShowMessage == undefined) {
        params.ShowMessage = true;
    }

    if (params.MessageLoading == undefined) {
        params.MessageLoading = "Procesando Transaccion...";
    }
    var res = {};
    $.ajax({
        type: params.Method,
        url: getPath + params.Url,
        data: formdata,
        processData: false,
        contentType: false,
        cache: false,
        data: formdata,
        async: params.Async,
        success: function (data) {
            if (params.BlockPage) {
                $.unblockUI('.portlet-body');
            }

            if (params.ShowMessage) {
                $("#mensaje").html(Mensaje(1, params.MessageOk))
            }
            res = data;

            callback(res);
        },
        beforeSend: function () {

            if (params.BlockPage) {
                $.blockUI({
                    target: '#frmFormulario',
                    boxed: true,
                    message: "<img class='_img' src='http://www.keenthemes.com/preview/metronic/theme/assets/global/img/loading-spinner-grey.gif'/> " + params.MessageLoading
                });

            }

        },
        complete: function () {

        },
        error: function (request, status, error) {

            if (params.BlockPage) {
                $.unblockUI('.portlet-body');
            }
            $("#mensaje").html(Mensaje(-1, request.responseText))
            callback(request);
        }
    })
    .always(function (jqXHR, textStatus) {

        if (textStatus != "success") {
            var rpta = "";
            var rpta2 = "";
            if (jqXHR.status == 0) {
                rpta = "La URL solicitada no se encuentra disponible";
            }
            else {
                rpta2 = jqXHR.responseText;
                rpta = "Error desconocido";
            }

            callback(
                    {
                        Cod: -1,
                        Mensaje: rpta,
                        Mensaje2: rpta2
                    }
                );
        }
    });

    return res;
}

function Bloquear(MessageLoading) {
    if (MessageLoading == undefined) {
        MessageLoading = "";
    }

    bar = new $.peekABar({
        html: "<img src = '" + rutaimageload + "' /> " + MessageLoading,
        onShow: function () {
            // Do something after bar is shown.
        },
        onHide: function () {
            // Do something after bar is hidden.
        }
    });
    bar.show();

}
function ShowMessage(type, message, callOK, callCancel,id) {

    switch (parseInt(type)) {
        case 0:

            Swal.fire({
                icon: 'warning',
                title: 'Advertencia',
                text: message
            })

            break;
        case -1:
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: message
            })

            break;
        case 1:
        
            Swal.fire({                
                icon: 'success',
                showDenyButton: false,
                showCancelButton: false,
                confirmButtonText: 'OK',
                title: '',
                text: message,
                allowOutsideClick: false
            }).then((result) => {
                
             if (result.isConfirmed) {
                 callOK();
              } 
            });

            break;
        case 2:

            Swal.fire({
                icon: 'success',
                showDenyButton: false,
                showCancelButton: false,
                confirmButtonText: 'OK',
                title: '',
                text: message,
                footer: '<table style="text-align:center;"><tr><td><a href="#" onclick="EmitirGuiaRemision('+ id  +')"><i class="fa fa-file-text-o"></i> Emitir Guía de Remisión </a> </td></tr><tr><td><a href="#" onclick="ExportarPdf('+ id  +')"> <i class="fa fa-file-pdf-o"></i> Exportar a PDF</a></td></tr></table>'
            }).then((result) => {
                
                if (result.isConfirmed) {
                    callOK();
                } 
            });

            break;
        case 3:

            Swal.fire({
                icon: 'success',
                showDenyButton: false,
                showCancelButton: false,
                confirmButtonText: 'OK',
                title: '',
                text: message,
                footer: '<table style="text-align:center;"><tr><td><a href="#" onclick="Imprimir('+ id  +')"><i class="fa fa-print"></i> Imprimir Formato A4 </a> </td></tr><tr><td><a href="#" onclick="Ticket('+ id  +')"> <i class="fa fa-print"></i> Imprimir Formato Ticket</a></td></tr></table>'
            }).then((result) => {
                
                if (result.isConfirmed) {
                    callOK();
                } 
            });

            break;
    }
}

function Desbloquear() {
    bar.hide();
}

function DesbloquearModal() {
    $('.modal-dialog').unblock();
}


function BloquearModal(MessageLoading) {

    if (MessageLoading == undefined) {
        MessageLoading = "";
    }

    $('.modal-dialog').block({
        message: "<img src = '" + rutaimageloadModal + "' /> " + MessageLoading,
        css: {
            border: 'none',
            padding: '15px',
            backgroundColor: "#000",
            fontSize: "16px",
            '-webkit-border-radius': '10px',
            '-moz-border-radius': '10px',
            opacity: .5,
            color: '#fff'
        }
    });
}

function CallAjaxfront(formdata, params, callback) {


    if (params.Method == undefined) {
        params.Method = "POST"
    }

    if (params.Async == undefined) {
        params.Async = true;
    }

    if (params.BlockPage == undefined) {
        params.BlockPage = true;
    }

    if (params.ShowMessage == undefined) {
        params.ShowMessage = true;
    }

    if (params.MessageLoading == undefined) {
        params.MessageLoading = "Procesando solicitud. Espere por favor...";
    }
    var res = {};

    if (params.ShowLoading == undefined) {
        params.ShowLoading = true;
    }

    if (params.ShowLoading) {

        Swal.fire({         
            title: 'Procesando',
            showSpinner: true,
            text: "Espere por favor...",
            imageUrl: "../Content/loading.gif",
            showConfirmButton: false,
            allowOutsideClick: false
        })

    }

    $.ajax({
        type: params.Method,
        url: params.Url,
        data: formdata,
        processData: false,
        contentType: false,
        cache: false,
        data: formdata,
        async: params.Async,
        success: function (data) {
            callback(data);
            //$.unblockUI();
        },
        beforeSend: function () {

        },
        complete: function () {
            //$.unblockUI();
        },
        error: function (request, status, error) {
            console.log(request);
            callback(request);
           // $.unblockUI();
        }
    })
    .always(function (jqXHR, textStatus) {

        if (textStatus != "success") {
            var rpta = "";

            if (jqXHR.status == 0) {
                rpta = "La URL solicitada no se encuentra disponible";
            }
            else {
                rpta = "Error desconocido";
            }

            callback(
                    {
                        error: -1,
                        rpta: rpta
                    }
                );
        }
    });
    return res;
}

var dialogMensaje = $("<div></div>");

var helpMensaje = {
    okEvent: null,
    type: "",
    mensaje: "",
    params: {},
    ShowMessageModal: function () {

        Swal.fire({
            title: helpMensaje.params.Mensaje,
            text: "",
            type: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Si, seguro!',
            cancelButtonText: 'No, cancelar!',
            confirmButtonClass: 'btn btn-success',
            cancelButtonClass: 'btn btn-danger',
            buttonsStyling: false
        }).then((result) => {
            if (result.isConfirmed) {
                helpMensaje.params.Events.Ok();
            }
        });
    }
    , HideMessageModal: function () {
        $('#mensajeAlertError').modal('hide');
        $('#mensajeAlertSuccess').modal('hide');
        $('#mensajeAlertQuestion').modal('hide');
        $('#mensajeSuccessQuestion').modal('hide');
    }
}


var utilitario = {
    CloseDialog: function () {
        dialogMensaje.dialog("close");
    },
    ShowDialog: function (tipomensaje, mensaje) {
        dialogMensaje.html("<div style=\"font-size:13px\"><h3 class=\"header smaller lighter grey\" style=\"border-bottom:0px solid black;font-size:15px\"><i class=\"icon-spinner icon-spin orange bigger-125\"></i>" + mensaje + "</h3></div>");
        dialogMensaje.dialog({
            title: "Mensaje del Sistema",
            width: 340,
            height: 200,
            modal: true,

            buttons: {
                "Cerrar": function () {
                    dialogMensaje.dialog("close");
                }
            },
            open: function () {
                $(this).find('.ui-dialog-buttonpane button:contains("Guardar")').addClass('btn btn-small btn-primary').html("<i class=\"icon-save bigger-125\"></i>Guardar");
                $(this).find('.ui-dialog-buttonpane button:contains("Cerrar")').addClass('btn btn-small btn-danger').html("<i class=\"icon-remove-sign\"></i>Cerrar");
            }
        });
    },

    ShowMensajeDialog: function (tipomensaje, mensaje) {
        //dialogMensaje.html("<div style=\"font-size:13px\"><h3 class=\"header smaller lighter grey\" style=\"border-bottom:0px solid black;font-size:15px\"><i class=\"icon-spinner icon-spin orange bigger-125\"></i>" + mensaje + "</h3></div>");
        //tipomensaje 

        // 0 => Error
        switch (tipomensaje) {
            case -1:
                var mensaje1 = '<div class="alert alert-error"><strong><i class="icon-remove"></i>Error!<br/></strong>';

                mensaje1 += mensaje
                mensaje1 += "</div>"

                $("<div></div>").html(mensaje1).dialog({
                    title: "Mensaje de Error",
                    width: "auto",
                    height: "auto",
                    modal: true,
                    buttons: {
                        "Aceptar": function () {

                            $(this).dialog("close");
                        }
                    },
                    open: function () {
                        $('.ui-dialog-buttonpane').find('button:contains("Aceptar")').addClass('btn  btn-primary').html("<i class=\"icon-ok bigger-125\"></i>Aceptar");

                    }
                });
                break;
            case 1:
                // Exito
                var mensaje1 = '<div class="alert alert-success"><strong><i class="icon-remove"></i>Exito!<br/></strong>';

                mensaje1 += mensaje
                mensaje1 += "</div>"

                $("<div></div>").html(mensaje1).dialog({
                    title: "Mensaje de Exito",
                    width: "auto",
                    height: "auto",
                    modal: true,
                    buttons: {
                        "Aceptar": function () {

                            $(this).dialog("close");
                        }
                    },
                    open: function () {
                        $('.ui-dialog-buttonpane').find('button:contains("Aceptar")').addClass('btn btn-primary').html("<i class=\"icon-ok bigger-125\"></i>Aceptar");

                    },
                });
                break;
            case 2:
                // Exito
                var mensaje1 = '<div class="alert alert-info"><strong><i class="icon-remove"></i>Informacion!<br/></strong>';

                mensaje1 += mensaje
                mensaje1 += "</div>"

                $("<div></div>").html(mensaje1).dialog({
                    title: "Mensaje de Información",
                    width: "auto",
                    height: "auto",
                    modal: true,
                    buttons: {
                        "Aceptar": function () {

                            $(this).dialog("close");
                        }
                    },
                    open: function () {
                        $('.ui-dialog-buttonpane').find('button:contains("Aceptar")').addClass('btn btn-primary').html("<i class=\"icon-ok bigger-125\"></i>Aceptar");

                    },
                });
                break;
            default:

        }

    }
}

function GetRowSelectedTableRadio(nameElemento) {
    var valor = ($("input[name = '" + nameElemento + "']:checked").val() == null) ? 0 : $("input[name = '" + nameElemento + "']:checked").val();
    return valor;
}

function getValueCellSelectedTableRadio(nameElemento, index) {
    var valor = $("input[name = '" + nameElemento + "']:checked").parent().parent().find("td:eq(" + index + ")").text()

    return valor;
}

function comprobarFecha(_fecha) {
    var _temp = _fecha.split("/");
    var day = _temp[0];
    var month = _temp[1];
    var year = _temp[2];

    var time = day + "/" + month + "/" + year;
    var date = time;

    if (day < 0 || day > 31) {
        //alert("Los dias deben comprender entre 1 y 31");
        return false;
    }
    if (month < 1 || month > 12) {
        //alert("El mes debe comprender entre 1 y 12");
        return false;
    }
    if (year < 0 || year > 9999) {
        //alert("Los años deben comprender entre 2000 y 9999");
        return false;
    }
    var reg = /(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d/;
    if (date.match(reg)) {
        if (!EsFechaValida(year, day, month)) {
            //alert("Fecha No Valida");
            return false;
        }
        return true;
    }
}

function GetDateNow() {
    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1;

    var yyyy = today.getFullYear();

    var _FechaAsignacion = dd + '/' + mm + '/' + yyyy;

    return _FechaAsignacion;
}


function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;

    return true;
}

function soloNumerosv2(evt, tipo) {
    //asignamos el valor de la tecla a keynum
    if (window.event) {// IE
        keynum = evt.keyCode;
    } else {
        keynum = evt.which;
    }

    if (tipo == 1) {
        if ((keynum > 47 && keynum < 58) || keynum == 8 || keynum == 9) {
            return true;
        } else {
            return false;
        }
    } else {
        if ((keynum > 47 && keynum < 58) || keynum == 8 || keynum == 39 || keynum == 46 || keynum == 9 || keynum == 0) {
            return true;
        } else {
            return false;
        }

    }

}

function SoloTexto(cadena) {
    var patron = /^[a-zA-Z\s\ñ\Ñ\á\Á\é\É\í\Í\Ó\ó\ú\Ú]*$/;

    ///^[a-zA-z\s\ñ\Ñ]+$/
    // En caso de querer validar cadenas con espacios usar: /^[a-zA-Z\s]*$/
    if (!cadena.search(patron))
        return true;
    else
        return false;
}

function soloLetras(e) {
    key = e.keyCode || e.which;
    tecla = String.fromCharCode(key).toLowerCase();
    letras = " áéíóúabcdefghijklmnñopqrstuvwxyz";
    especiales = [8, 37, 39, 46];

    tecla_especial = false
    for (var i in especiales) {
        if (key == especiales[i]) {
            tecla_especial = true;
            break;
        }
    }

    if (letras.indexOf(tecla) == -1 && !tecla_especial)
        return false;
}

function MensajeLineal(elemento, class1, contenido) {
    $(elemento).removeClass("alert-success")
               .removeClass("alert-warning")
               .removeClass("alert-error")
               .removeClass("alert-info")
               .addClass(class1).html(contenido);
}

function SoloNumeros(e) {

    var keynum = window.event ? window.event.keyCode : e.which;
    if ((keynum == 8) || (keynum == 46))
        return true;

    return /\d/.test(String.fromCharCode(keynum));
}

function validarEmail(valor) {

    var filtro = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    if (!filtro.test(valor)) {
        return false;
    }
    else {
        return true;
    }
}

function ObtenerFechaActual(separator) {
    var d = new Date(Date.now()),
           month = '' + (d.getMonth() + 1),
           day = '' + d.getDate(),
           year = d.getFullYear();

    if (month.length < 2) month = '0' + month;
    if (day.length < 2) day = '0' + day;

    return [day, month, year].join(separator);
}

function trim(cadena) {
    var expresionRegular = /^\s+|\s+$/g;
    return cadena.replace(expresionRegular, "");
}

function EstablecerMensajeModal(codigo, mensaje) {
    $("#msgModal").html(Mensaje(codigo, mensaje));
}

function Mensaje(codigo, mensaje) {
    if (codigo == -1) {
        return "<div class=\"alert alert-block alert-error\">" +
                                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\">" +
                                        "<i class=\"fa fa-close\"></i>" +
                                    "</button>" +
                                    "<i class=\"icon-remove\"></i>" +

                                        "<i class=\"fa fa-remove\"></i> " + mensaje +
                                    "</div>";
    }
    else {
        if (codigo == 0) {
            return "<div class=\"alert alert-block alert-info\">" +
                                               "<button type=\"button\" class=\"close\" data-dismiss=\"alert\">" +
                                                   "<i class=\"fa fa-close\"></i>" +
                                               "</button>" +


                                                   "<i class=\"fa fa-question\"></i> " + mensaje +
                                               "</div>";
        }
        else {
            if (codigo >= 1) {
                return "<div class=\"alert alert-block alert-success\">" +
                                                   "<button type=\"button\" class=\"close\" data-dismiss=\"alert\">" +
                                                       "<i class=\"icon-remove\"></i>" +
                                                   "</button>" +


                                                      "<i class=\"fa fa-check\"></i> " + mensaje +
                                                   "</div>";
            }
        }
    }
}

function ShowNotification(mensaje) {
    $("#alertify-logs").html("");
    alertify.log(mensaje);
}

function HideNotification() {
    $("#alertify-logs").addClass("alertify-logs-hidden");
}

function comprobarFecha(_fecha, result) {
    var done = false;

    var _temp = _fecha.split("-");
    var day = _temp[0];
    var month = _temp[1];
    var year = _temp[2];

    var time = day + "-" + month + "-" + year;
    var date = time;

    if (day < 0 || day > 31) {
        $(result).html("* Dia entre 1 y 31");
        return false;
    }
    if (month < 1 || month > 12) {
        $(result).html("* Mes entre 1 y 12");
        return false;
    }
    if (year < 0 || year > 9999) {
        $(result).html("* Año 2000 y 9999");
        return false;
    }

    var reg = /(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d/;
    if (date.match(reg)) {
        if (!EsFechaValida(year, day, month)) {
            $(result).html("* Fecha No Válida");
            return false;
        }
        else {

            $(result).html("");
        }
        return true;
    }
    else {
        return false;
    }
}

function EsFechaValida(_anio, _dia, _mes) {
    var anio = _anio;
    var dia = _dia;
    var mes = _mes;

    switch (mes) {
        case "01":
        case "03":
        case "05":
        case "07":
        case "08":
        case "10":
        case "12":
            numDias = 31;
            break;
        case "04":
        case "06":
        case "09":
        case "11":
            numDias = 30;
            break;
        case "02":
            if (comprobarSiBisisesto(anio)) { numDias = 29 } else { numDias = 28 };
            break;
        default:
            return false;
    }

    if (dia > numDias || dia == 0) {
        return false;
    }
    return true;
}

/*Comprueba si un año es bisiesto o no*/
function comprobarSiBisisesto(anio) {
    if ((anio % 100 != 0) && ((anio % 4 == 0) || (anio % 400 == 0))) {
        return true;
    }
    else {
        return false;
    }
}

function ChangeFormatDate(format, value) {
    var _fecha = value;
    var _arrayFecha = _fecha.split("-");
    var formateado = "";
    switch (format) {
        case "yyyy-mm-dd":
            _fecha = _arrayFecha[2].toString().concat("-", _arrayFecha[1], "-", _arrayFecha[0]);
            break;
        default:
            break;
    }

    return _fecha;
}

function validateFloatKeyPress(el, evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    var number = el.value.split('.');
    if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    //just one dot
    if (number.length > 1 && charCode == 46) {
        return false;
    }
    //get the carat position
    var caratPos = getSelectionStart(el);
    var dotPos = el.value.indexOf(".");
    if (caratPos > dotPos && dotPos > -1 && (number[1].length > 1)) {
        return false;
    }
    return true;
}


function NumCheckComma(event) {
    var regex = new RegExp("^[0-9-!@#$%*?,]");
    var key = String.fromCharCode(event.charCode ? event.which : event.charCode);
    if (!regex.test(key)) {
        event.preventDefault();
        return false;
    }
}