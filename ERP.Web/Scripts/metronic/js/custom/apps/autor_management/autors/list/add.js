"use strict";
var KTUsersAddUser = (function () {
    const t = document.getElementById("kt_modal_add_autor"),
        e = t.querySelector("#kt_modal_add_autor_form"),
        n = new bootstrap.Modal(t);
    return {
        init: function () {
            (() => {
                var o = FormValidation.formValidation(e, {
                    fields: {
                        idTipoIdentidad: {
                            validators: { notEmpty: { message: "El tipo de identidad es requerido" } },
                        },
                        nroIdentidad: {
                            validators: { notEmpty: { message: "El nro de documento es requerido" } },
                        },
                        nombres: {
                            validators: { notEmpty: { message: "El nombre es requerido" } },
                        },
                        apellidoPaterno: {
                            validators: { notEmpty: { message: "El apellido paterno es requerido" } },
                        },
                        apellidoMaterno: {
                            validators: { notEmpty: { message: "El apellido materno es requerido" } },
                        },
                        biografia: {
                            validators: { notEmpty: { message: "Ingrese su biografía" } },
                        },
                        username: {
                            validators: { notEmpty: { message: "El usuario es requerido" } },
                        },
                        password: {
                            validators: { notEmpty: { message: "La contraseña es requerida" } },
                        },
                    },
                    plugins: {
                        trigger: new FormValidation.plugins.Trigger(),
                        bootstrap: new FormValidation.plugins.Bootstrap5({
                            rowSelector: ".fv-row",
                            eleInvalidClass: "",
                            eleValidClass: "",
                        }),
                    },
                });
                const i = t.querySelector('[data-kt-autors-modal-action="submit"]');
                i.addEventListener("click", (t) => {
                    t.preventDefault(),
                        o &&
                        o.validate().then(function (t) {

                            if (t === "Valid") {
                                i.setAttribute("data-kt-indicator", "on");
                                i.disabled = true;

                                const formData = new FormData(e);
                                const formDataEnvio = new FormData();
                                formDataEnvio.append("Autor.idAutor", $("#idAutor").val());
                                formDataEnvio.append("Autor.idTipoIdentidad", formData.get('idTipoIdentidad'));
                                formDataEnvio.append("Autor.nroIdentidad", formData.get('nroIdentidad'));
                                formDataEnvio.append("Autor.nombres", formData.get('nombres'));
                                formDataEnvio.append("Autor.apellidoPaterno", formData.get('apellidoPaterno'));
                                formDataEnvio.append("Autor.apellidoMaterno", formData.get('apellidoMaterno'));
                                formDataEnvio.append("Autor.biografia", formData.get('biografia'));
                                formDataEnvio.append("Autor.esAlumno", $('#cboEsAlumno').is(":checked") ? 1 : 0);
                                formDataEnvio.append("Autor.idEstado", 1);

                                formDataEnvio.append("Usuario.username", formData.get('username'));
                                formDataEnvio.append("Usuario.contrasenia", formData.get('password'));

                                // Realizar la solicitud Ajax
                                $.ajax({
                                    url: "Autor/Registrar",
                                    type: "POST",
                                    data: formDataEnvio,
                                    processData: false,
                                    contentType: false,
                                    success: function (response) {


                                        console.log("Formulario enviado con éxito!");
                                        console.log(response);

                                        // Mostrar mensaje de éxito
                                        Swal.fire({
                                            text: "¡El formulario se ha enviado correctamente!",
                                            icon: "success",
                                            buttonsStyling: false,
                                            confirmButtonText: "Ok, entendido",
                                            customClass: { confirmButton: "btn btn-primary" },
                                        }).then(function () {
                                            window.location.reload();
                                            n.hide();
                                        });
                                    },
                                    error: function (error) {
                                        console.error("Error al enviar el formulario");
                                        console.log(error);

                                        // Mostrar mensaje de error
                                        Swal.fire({
                                            text: "Se produjo un error al enviar el formulario. Por favor, inténtalo de nuevo.",
                                            icon: "error",
                                            buttonsStyling: false,
                                            confirmButtonText: "Ok, entendido",
                                            customClass: { confirmButton: "btn btn-primary" },
                                        });
                                    },
                                });
                            } else {
                                Swal.fire({
                                    text: "Lo sentimos, parece que se han detectado algunos errores, inténtalo de nuevo.",
                                    icon: "error",
                                    buttonsStyling: !1,
                                    confirmButtonText: "Ok, ¡entendido!",
                                    customClass: { confirmButton: "btn btn-primary" },
                                });
                            }

                            console.log("validated!")
                                
                        });
                }),
                    t
                        .querySelector('[data-kt-autors-modal-action="cancel"]')
                        .addEventListener("click", (t) => {
                            t.preventDefault(),
                                Swal.fire({
                                    text: "Are you sure you would like to cancel?",
                                    icon: "warning",
                                    showCancelButton: !0,
                                    buttonsStyling: !1,
                                    confirmButtonText: "Yes, cancel it!",
                                    cancelButtonText: "No, return",
                                    customClass: {
                                        confirmButton: "btn btn-primary",
                                        cancelButton: "btn btn-active-light",
                                    },
                                }).then(function (t) {
                                    t.value
                                        ? (e.reset(), n.hide())
                                        : "cancel" === t.dismiss &&
                                        Swal.fire({
                                            text: "Your form has not been cancelled!.",
                                            icon: "error",
                                            buttonsStyling: !1,
                                            confirmButtonText: "Ok, got it!",
                                            customClass: { confirmButton: "btn btn-primary" },
                                        });
                                });
                        }),
                    t
                        .querySelector('[data-kt-autors-modal-action="close"]')
                        .addEventListener("click", (t) => {
                            t.preventDefault(),
                                Swal.fire({
                                    text: "Are you sure you would like to cancel?",
                                    icon: "warning",
                                    showCancelButton: !0,
                                    buttonsStyling: !1,
                                    confirmButtonText: "Yes, cancel it!",
                                    cancelButtonText: "No, return",
                                    customClass: {
                                        confirmButton: "btn btn-primary",
                                        cancelButton: "btn btn-active-light",
                                    },
                                }).then(function (t) {
                                    t.value
                                        ? (e.reset(), n.hide())
                                        : "cancel" === t.dismiss &&
                                        Swal.fire({
                                            text: "Your form has not been cancelled!.",
                                            icon: "error",
                                            buttonsStyling: !1,
                                            confirmButtonText: "Ok, got it!",
                                            customClass: { confirmButton: "btn btn-primary" },
                                        });
                                });
                        });
            })();
        },
    };
})();
KTUtil.onDOMContentLoaded(function () {
    KTUsersAddUser.init();
});
