$(document).ready(function () {

    $('#btnBeneficiarios').on('click', function () {
        $('#modalBeneficiarios').show();
    });

    $('.close').on('click', function () {
        $('#modalBeneficiarios').hide();
    });
})

function paragrafoBeneficiarios(nomeTela) {
    $(".paragrafoBeneficiarios").text(`Para salvar as informações pressione o botão Salvar na tela ${nomeTela} Cliente`);
}

function ModalDialog(titulo, texto) {
    var random = Math.random().toString().replace('.', '');
    var texto = '<div id="' + random + '" class="modal fade">                                                               ' +
        '        <div class="modal-dialog">                                                                                 ' +
        '            <div class="modal-content">                                                                            ' +
        '                <div class="modal-header">                                                                         ' +
        '                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>         ' +
        '                    <h4 class="modal-title">' + titulo + '</h4>                                                    ' +
        '                </div>                                                                                             ' +
        '                <div class="modal-body">                                                                           ' +
        '                    <p>' + texto + '</p>                                                                           ' +
        '                </div>                                                                                             ' +
        '                <div class="modal-footer">                                                                         ' +
        '                    <button type="button" class="btn btn-default" data-dismiss="modal">Fechar</button>             ' +
        '                                                                                                                   ' +
        '                </div>                                                                                             ' +
        '            </div><!-- /.modal-content -->                                                                         ' +
        '  </div><!-- /.modal-dialog -->                                                                                    ' +
        '</div> <!-- /.modal -->                                                                                        ';

    $('body').append(texto);
    $('#' + random).modal('show');
}

function ModalDialogEspecific(titulo, texto, idCliente) {
    var random = Math.random().toString().replace('.', '');
    var texto = '<div id="' + random + '" class="modal fade">                                                               ' +
        '        <div class="modal-dialog">                                                                                 ' +
        '            <div class="modal-content">                                                                            ' +
        '                <div class="modal-header">                                                                         ' +
        '                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>         ' +
        '                    <h4 class="modal-title">' + titulo + '</h4>                                                    ' +
        '                </div>                                                                                             ' +
        '                <div class="modal-body">                                                                           ' +
        '                    <p>' + texto + '</p>                                                                           ' +
        '                </div>                                                                                             ' +
        '                <div class="modal-footer">                                                                         ' +
        '                    <button type="button" class="btn btn-default" onclick="changeURL('+idCliente+')" data-dismiss="modal">OK</button>             ' +
        '                                                                                                                   ' +
        '                </div>                                                                                             ' +
        '            </div><!-- /.modal-content -->                                                                         ' +
        '  </div><!-- /.modal-dialog -->                                                                                    ' +
        '</div> <!-- /.modal -->                                                                                        ';

    $('body').append(texto);
    $('#' + random).modal('show');
}

function addRow() {
    const name = document.getElementById("ModalNome").value;
    let cpf = document.getElementById("ModalCPF").value;

    if (name && cpf) {
        cpf = cpf.replace(/\D/g, '');

        if (cpf.length != 11) {
            alert("CPF deve ter 11 números!");
            return false;
        }

        // Formata o CPF: 000.000.000-00
        cpf = cpf.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, '$1.$2.$3-$4');

        let newRow = templateRowBeneficiarios().replace("#CPF#", cpf).replace("#NOME#", name);

        $('#gridBeneficiarios tbody').append(newRow);

        document.getElementById("formBeneficiarios").reset();
    } else {
        alert("Preencha os campos Nome e CPF!");
    }
}

function templateRowBeneficiarios() {
    return `<tr>
                <td class="editable" id="modalBenfCPF">#CPF#</td>
                <td class="editable" id="modalBenfNOME">#NOME#</td>
                <td>
                    <button class="btn btn-sm btn-primary" onclick="alterarBeneficiario(this)">Alterar</button>
                    <button class="btn btn-sm btn-primary" onclick="saveEdit(this)" style="display: none;">Salvar</button>
                    <button class="btn btn-sm btn-primary" onclick="excluirBeneficiario(this)">Excluir</button>
                </td>   
            </tr>
            `;
}

function preparaBeneficiarios() {
    const table = document.getElementById('gridBeneficiarios');
    const rows = table.querySelectorAll('tbody tr');
    const data = [];

    rows.forEach(row => {
        const cells = row.querySelectorAll('td');
        const cpf = cells[0].textContent.trim();
        const name = cells[1].textContent.trim();
        data.push({ NOME: name, CPF: cpf });
    });

    // Converte os dados da tabela para JSON
    const tableDataInput = document.getElementById('gridBeneficiarios');
    tableDataInput.value = JSON.stringify(data);

    return data;
}

function alterarBeneficiario(button) {

    const row = button.parentNode.parentNode;

    row.querySelectorAll('.editable').forEach(cell => {
        cell.contentEditable = "true";
        cell.style.backgroundColor = "#f0f8ff"; 
    });

    button.style.display = "none"; 
    button.nextElementSibling.style.display = "inline"; 
}

function saveEdit(button) {
    const row = button.parentNode.parentNode;

    row.querySelectorAll('.editable').forEach(cell => {
        cell.contentEditable = "false";
        cell.style.backgroundColor = "";
    });

    button.style.display = "none";
    button.previousElementSibling.style.display = "inline";
}

function excluirBeneficiario(button) {
    $(button).closest('tr').remove();
}