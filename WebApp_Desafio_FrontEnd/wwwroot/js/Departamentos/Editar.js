$(document).ready(function () {

    // Calendário
    $('.glyphicon-calendar').closest("div.date").datepicker({
        todayBtn: "linked",
        keyboardNavigation: false,
        forceParse: false,
        calendarWeeks: false,
        format: 'dd/mm/yyyy',
        autoclose: true,
        language: 'pt-BR'
    });

    // Botão cancelar
    $('#btnCancelar').click(function () {
        Swal.fire({
            html: "Deseja cancelar a edição? As alterações serão perdidas.",
            icon: "warning",
            showCancelButton: true,
            confirmButtonText: 'Sim, cancelar',
            cancelButtonText: 'Não'
        }).then((result) => {
            if (result.isConfirmed) {
                history.back();
            }
        });
    });



    // Parametrização do plugin JQuery Validation
    if ($.validator != null) {
        $.extend($.validator, {
            messages: {
                required: "Obrigat&oacute;rio.",
                remote: "Por favor, corrija este campo.",
                email: "Forne&ccedil;a um endere&ccedil;o eletr&ocirc;nico v&aacute;lido.",
                url: "Forne&ccedil;a uma URL v&aacute;lida.",
                date: "Forne&ccedil;a uma data v&aacute;lida.",
                dateITA: "Forne&ccedil;a uma data v&aacute;lida.",
                dateISO: "Forne&ccedil;a uma data v&aacute;lida (ISO).",
                number: "Forne&ccedil;a um n&uacute;mero v&aacute;lido.",
                digits: "Forne&ccedil;a somente d&iacute;gitos.",
                creditcard: "Forne&ccedil;a um cart&atilde;o de cr&eacute;dito v&aacute;lido.",
                equalTo: "Forne&ccedil;a o mesmo valor novamente.",
                accept: "Selecione um arquivo com uma extens&atilde;o v&aacute;lida.",
                maxlength: $.validator.format("Forne&ccedil;a n&atilde;o mais que {0} caracteres."),
                minlength: $.validator.format("Forne&ccedil;a ao menos {0} caracteres."),
                rangelength: $.validator.format("Forne&ccedil;a um valor entre {0} e {1} caracteres de comprimento."),
                range: $.validator.format("Forne&ccedil;a um valor entre {0} e {1}."),
                max: $.validator.format("Forne&ccedil;a um valor menor ou igual a {0}."),
                min: $.validator.format("Forne&ccedil;a um valor maior ou igual a {0}.")
            }
        });

        $.validator.setDefaults({
            errorElement: 'label',
            errorClass: 'error',
            focusInvalid: false,
            ignore: "",
            highlight: function (element) {
                $(element).closest('.form-group').removeClass('has-success').addClass('has-error');
            },

            unhighlight: function (element) {
                $(element).closest('.form-group').removeClass('has-error');
            },

            success: function (element) {
                $(element).closest('.form-group').removeClass('has-error');
            }
        });
    }

    // Preenche os campos com os dados do departamento (vindos do backend)
    function preencherFormulario() {
        $('#Descricao').val(departamentoModel.descricao);
    }

    preencherFormulario();
    $('#btnSalvar').click(function () {

        let frm = $('#form');

        frm.validate({
            rules: {
                Descricao: {
                    required: true
                }
            }
        });


        if ($('#form').valid() != true) {
            FormularioInvalidoAlert();
            return;
        }

        let departamento = SerielizeForm($('#form'));

        // garante que o campo solicitante vai junto
        departamento.Descricao = $('#Descricao').val();

        let url = $('#form').attr('action');

        $.ajax({
            type: "POST",
            url: url,
            data: departamento,
            success: function (result) {
                // Trata casos em que Message vem como JSON string
                let messageText = '';
                let messageType = 'success';
                let titleText = 'Sucesso!';
                let controller = result.Controller || 'Departamentos';
                let action = result.Action || 'Listar';

                try {
                    // Se o campo Message for um JSON válido
                    let parsed = JSON.parse(result.Message);
                    if (parsed && parsed.Message) {
                        messageText = parsed.Message;
                        messageType = parsed.Type || result.Type || 'info';
                        titleText = messageType === 'error'
                            ? 'Erro!'
                            : messageType === 'warning'
                                ? 'Atenção!'
                                : 'Sucesso!';
                    } else {
                        messageText = result.Message || 'Operação realizada com sucesso.';
                        messageType = result.Type || 'success';
                    }
                } catch {
                    // Se não for JSON, trata como texto normal
                    messageText = result.Message || 'Operação realizada com sucesso.';
                    messageType = result.Type || 'success';
                }

                // Exibe alerta
                Swal.fire({
                    icon: messageType,
                    title: titleText,
                    text: messageText,
                    confirmButtonText: 'OK'
                }).then(function () {
                    if (messageType === 'success') {
                        window.location.href = config.contextPath + controller + '/' + action;
                    }
                });
            },

            // Tratamento de erro inesperado (ex: 500)
            error: function (xhr) {
                let msg = 'Ocorreu um erro inesperado.';
                try {
                    let parsed = JSON.parse(xhr.responseText);
                    msg = parsed.Message || parsed.message || msg;
                } catch { }

                Swal.fire({
                    icon: 'error',
                    title: 'Erro!',
                    text: msg,
                    confirmButtonText: 'OK'
                });
            },
        });
    });

});
