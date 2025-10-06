$(document).ready(function () {

    //function preencherFormulario(chamado) {
    //    $('#Assunto').val(chamado.Assunto);
    //    $('#DataAbertura').val(chamado.DataAbertura);
    //    $('#Departamento').val(chamado.IdDepartamento).trigger('change');
    //    const solicitanteAtual = $('#Solicitante').data('solicitante');

    //    if (solicitanteAtual) {
    //        const option = new Option(solicitanteAtual, solicitanteAtual, true, true);
    //        $('#Solicitante').append(option).trigger('change');
    //    }
    //}

    //preencherFormulario({
    //    Assunto: '@Model.Assunto',
    //    DataAbertura: '@Model.DataAbertura.ToString("dd/MM/yyyy")',
    //    IdDepartamento: '@Model.IdDepartamento',
    //    Solicitante: '@Model.Solicitante'
    //});

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

    // Select2 — campo Solicitante
    $('#Solicitante').select2({
        language: 'pt-BR',
        ajax: {
            url: 'https://localhost:44388/api/Chamados/solicitantes',
            dataType: 'json',
            delay: 250,
            data: params => ({ nome: params.term }),
            processResults: data => ({
                results: data.map(x => ({ id: x.id, text: x.nome }))
            })
        },
        placeholder: 'Digite o nome do solicitante',
        minimumInputLength: 1,
        width: '100%',
        tags: true,
        createTag: function (params) {
            const term = $.trim(params.term);
            if (term === '') return null;
            return { id: term, text: term, newOption: true };
        },
        templateResult: function (data) {
            if (data.newOption)
                return $('<span><strong>Adicionar novo solicitante:</strong> ' + data.text + '</span>');
            return data.text;
        }
    });

    // Função para preencher os campos
    //function preencherFormulario() {
    //    //$('#Assunto').val('@Model.Assunto');
    //    //$('#DataAbertura').val('@Model.DataAbertura.ToString("dd/MM/yyyy")');
    //    //$('#Departamento').val('@Model.IdDepartamento').trigger('change');

    //    const solicitanteAtual = $('#Solicitante').data('solicitante');
    //    if (solicitanteAtual) {
    //        const option = new Option(solicitanteAtual, solicitanteAtual, true, true);
    //        $('#Solicitante').append(option).trigger('change');
    //    }

    //    $('#Assunto').val(chamado.Assunto);
    //    $('#DataAbertura').val(chamado.DataAbertura);
    //    $('#Departamento').val(chamado.IdDepartamento).trigger('change');


    //}

    //// Chama a função DEPOIS de inicializar o select2
    //preencherFormulario();


    //const chamado = {
    //    Assunto: $('#Assunto').val(),
    //    DataAbertura: $('#DataAbertura').val(),
    //    IdDepartamento: '@Model.IdDepartamento',
    //    Solicitante: '@Model.Solicitante',
    //    departamento: '@Model.Departamento',
    //};

    //function preencherFormulario() {
    //    $('#Assunto').val(chamado.Assunto);
    //    $('#DataAbertura').val(chamado.DataAbertura);
    //    $('#Departamento').val(chamado.departamento).trigger('change');

    //    const solicitanteAtual = chamado.Solicitante;
    //    if (solicitanteAtual) {
    //        const option = new Option(solicitanteAtual, solicitanteAtual, true, true);
    //        $('#Solicitante').append(option).trigger('change');
    //    }
    //}

    //// chama depois de inicializar o select2
    //preencherFormulario();

    // Preenche os campos com os dados do chamado (vindos do backend)


    function preencherFormulario() {
        $('#Assunto').val(chamadoModel.assunto);
        $('#DataAbertura').val(chamadoModel.dataAbertura);
        $('#Departamento').val(chamadoModel.idDepartamento).trigger('change');

        if (chamadoModel.solicitante) {
            const option = new Option(chamadoModel.solicitante, chamadoModel.solicitante, true, true);
            $('#Solicitante').append(option).trigger('change');
        }
    }

    preencherFormulario();

    //// Botão salvar alterações
    //$('#btnSalvar').click(function () {

    //    if ($('#form').valid() != true) {
    //        Swal.fire('Atenção', 'Por favor, corrija os erros do formulário.', 'warning');
    //        return;
    //    }

    //    //let chamado = $('#form').serialize();
    //    let chamadoVM = $('#form').serializeArray();
    //    chamadoVM.push({ name: 'Solicitante', value: $('#Solicitante').val() });

    //    $.ajax({
    //        type: "POST",
    //        //url: $('#form').attr('action'),
    //        //url: '/Chamados/Cadastrar',
    //        url: $('#form').attr('action') || '/Chamados/Cadastrar',
    //        data: chamadoVM,
    //        success: function (result) {
    //            Swal.fire({
    //                icon: result.Type,
    //                title: 'Sucesso!',
    //                text: result.Message,
    //            }).then(() => {
    //                window.location.href = '/' + result.Controller + '/' + result.Action;
    //            });
    //        },
    //        error: function (xhr) {
    //            let msg = xhr.responseJSON?.Message || 'Erro ao salvar o chamado.';
    //            Swal.fire('Erro', msg, 'error');
    //        }
    //    });
    //});


    $('#btnSalvar').click(function () {

        let frm = $('#form');

        frm.validate({
            rules: {
                Assunto: {
                    required: true
                },
                Solicitante: {
                    required: true
                }
            }
        });


        if ($('#form').valid() != true) {
            FormularioInvalidoAlert();
            return;
        }

        //let chamado = SerielizeForm($('#form'));
        //let url = $('#form').attr('action');

        let chamado = SerielizeForm($('#form'));

        // garante que o campo solicitante vai junto
        chamado.Solicitante = $('#Solicitante').val();

        let url = $('#form').attr('action');

        $.ajax({
            type: "POST",
            url: url,
            data: chamado,
            success: function (result) {
                // Trata casos em que Message vem como JSON string
                let messageText = '';
                let messageType = 'success';
                let titleText = 'Sucesso!';
                let controller = result.Controller || 'Chamados';
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
