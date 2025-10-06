$(document).ready(function () {

    var table = $('#dataTables-Chamados').DataTable({
        "language": {
            "lengthMenu": "Apresentar _MENU_ linhas por página",
            "zeroRecords": "Não há registros",
            "search": "Pesquisar:",
            "emptyTable": "Sem dados disponíveis na tabela",
            "info": "Mostrando _START_ a _END_ de _TOTAL_ registros",
            "infoEmpty": "Não há registros",
            "infoFiltered": "(Filtrando de _MAX_ registros)",
            "loadingRecords": "Carregando...",
            "processing": "Processando...",
            "paginate": {
                "first": "Primeiro",
                "last": "Último",
                "next": "Próximo",
                "previous": "Anterior"
            }
        },
        paging: true,
        pageLength: 10,
        lengthMenu: [5, 10, 25, 50],
        ordering: true,
        info: true,
        searching: true,
        processing: true,
        serverSide: false,
        ajax: config.contextPath + 'Chamados/Datatable',
        columns: [
            { data: 'ID' },
            { data: 'Assunto' },
            { data: 'Solicitante' },
            { data: 'Departamento' },
            { data: 'DataAberturaWrapper', title: 'Data Abertura' },
            {
                data: null,
                title: 'Ações',
                render: function (data, type, row) {
                    return `
              <div class="acoes-container">
                  <button class="btn btn-sm btn-primary btn-editar" data-id="${row.ID}">
                      <i class="fa fa-edit"></i>
                  </button>
                  <button class="btn btn-sm btn-danger btn-excluir" data-id="${row.ID}">
                      <i class="fa fa-trash"></i>
                  </button>
              </div>`;
                },
                orderable: false
            }
        ],
    });

    $('#dataTables-Chamados tbody').on('click', 'tr', function () {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
        } else {
            table.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
        }
    });

    $('#dataTables-Chamados tbody').on('dblclick', 'tr', function () {
        var data = table.row(this).data();

        if (data && data.ID) {
            window.location.href = config.contextPath + 'Chamados/Editar/' + data.ID;
        }
    });

    $('#btnRelatorio').click(function () {
        window.location.href = config.contextPath + 'Chamados/Report';
    });

    $('#btnAdicionar').click(function () {
        window.location.href = config.contextPath + 'Chamados/Cadastrar';
    });

    //$('#btnEditar').click(function () {
    //    window.location.href = config.contextPath + 'Chamados/Editar';
    //});

    //$('#btnEditar').click(function () {
    //    var data = table.row('.selected').data();
    //    window.location.href = config.contextPath + 'Chamados/Editar/' + data.ID;
    //});

    //$('#btn-editar').click(function () {
    //    const data = table.row('.selected').data();

    //    if (!data || !data.ID) {
    //        Swal.fire({
    //            icon: 'warning',
    //            title: 'Selecione um registro!',
    //            text: 'Por favor, selecione um chamado antes de clicar em Editar.'
    //        });
    //        return;
    //    }

    //    // Redireciona para o controller com o ID
    //    //window.location.href = config.contextPath + 'Chamados/Editar/' + data.ID;

    //    if (data && data.ID) {
    //        window.location.href = config.contextPath + 'Chamados/Editar/' + data.ID;
    //    }
    //});

    $('#dataTables-Chamados').on('click', '.btn-editar', function () {
        const id = $(this).data('id'); // pega o id direto do botão

        if (!id) {
            Swal.fire('Erro', 'ID do chamado não encontrado.', 'error');
            return;
        }

        window.location.href = config.contextPath + 'Chamados/Editar/' + id;
    });

    $('#btnExcluir').click(function () {

        let data = table.row('.selected').data();
        let idRegistro = data.ID;
        if (!idRegistro || idRegistro <= 0) {
            return;
        }

        if (idRegistro) {
            Swal.fire({
                text: "Tem certeza de que deseja excluir " + data.Assunto + " ?",
                type: "warning",
                showCancelButton: true,
            }).then(function (result) {

                if (result.value) {
                    $.ajax({
                        url: config.contextPath + 'Chamados/Excluir/' + idRegistro,
                        type: 'DELETE',
                        contentType: 'application/json',
                        error: function (result) {

                            Swal.fire({
                                text: result,
                                confirmButtonText: 'OK',
                                icon: 'error'
                            });

                        },
                        success: function (result) {

                            Swal.fire({
                                type: result.Type,
                                title: result.Title,
                                text: result.Message,
                            }).then(function () {
                                table.draw();
                            });
                        }
                    });
                } else {
                    console.log("Cancelou a exclusão.");
                }

            });
        }
        language: {
            url: '//cdn.datatables.net/plug-ins/1.13.7/i18n/pt-BR.json'
        }
    });

});


//$('#dataTables-Chamados').on('click', '.btn-excluir', function () {
//    const id = $(this).data('id');
//    const assunto = $(this).closest('tr').find('td:eq(1)').text();

//    Swal.fire({
//        title: "Excluir Chamado",
//        text: `Deseja realmente excluir o chamado "${assunto}"?`,
//        icon: "warning",
//        showCancelButton: true,
//        confirmButtonText: "Sim, excluir",
//        cancelButtonText: "Cancelar"
//    }).then(result => {
//        if (result.isConfirmed) {
//            $.ajax({
//                url: window.apiBaseUrl + 'api/Chamados/Excluir/' + id,
//                type: 'DELETE',
//                success: function (result) {
//                    Swal.fire({
//                        icon: result.Type,
//                        title: result.Title || 'Sucesso',
//                        text: result.Message
//                    }).then(() => table.ajax.reload());
//                },
//                error: function (xhr) {
//                    Swal.fire('Erro', 'Falha ao excluir o registro.', 'error');
//                }
//            });
//        }
//    });
//});


$('#dataTables-Chamados').on('click', '.btn-excluir', function () {
    const id = $(this).data('id');
    const assunto = $(this).closest('tr').find('td:eq(1)').text();

    Swal.fire({
        title: "Excluir Chamado",
        text: `Deseja realmente excluir o chamado "${assunto}" (ID ${id})?`,
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Sim, excluir",
        cancelButtonText: "Cancelar"
    }).then(result => {
        if (result.isConfirmed) {
            $.ajax({
                url: `${window.apiBaseUrl}api/Chamados/Excluir/${id}`,
                type: 'DELETE',
                success: function (result) {
                    // Exibe alerta personalizado com ID e assunto
                    Swal.fire({
                        icon: 'success',
                        title: 'Chamado Excluído!',
                        text: `Chamado ${id} – "${assunto}" foi excluído com sucesso.`,
                        confirmButtonText: 'OK'
                    }).then(() => {
                        // Recarrega a página Listar
                        window.location.href = config.contextPath + 'Chamados/Listar';
                    });
                },
                error: function (xhr) {
                    let msg = 'Falha ao excluir o registro.';
                    try {
                        const parsed = JSON.parse(xhr.responseText);
                        msg = parsed.Message || msg;
                    } catch { }
                    Swal.fire('Erro', msg, 'error');
                }
            });
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
