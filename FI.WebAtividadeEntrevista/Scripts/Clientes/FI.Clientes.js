
$(document).ready(function () {
    $('#formCadastro').submit(function (e) {
        e.preventDefault();

        var beneficiariosData = preparaBeneficiarios();

        $.ajax({
            url: urlPost,
            method: "POST",
            data: {
                "NOME": $(this).find("#Nome").val(),
                "CEP": $(this).find("#CEP").val(),
                "Email": $(this).find("#Email").val(),
                "Sobrenome": $(this).find("#Sobrenome").val(),
                "Nacionalidade": $(this).find("#Nacionalidade").val(),
                "Estado": $(this).find("#Estado").val(),
                "Cidade": $(this).find("#Cidade").val(),
                "Logradouro": $(this).find("#Logradouro").val(),
                "Telefone": $(this).find("#Telefone").val(),
                "CPF": $(this).find("#CPF").val(),
                "Beneficiarios": beneficiariosData
            },
            error:
                function (r) {
                    if (r.status == 400) {
                        ModalDialog("Ocorreu um erro", r.responseJSON);
                    }
                    else if (r.status == 500) {
                        ModalDialog("Ocorreu um erro", "Ocorreu um erro interno no servidor.");
                    }
                },
            success:
                function (r) {
                    if (r.status == -1) {
                        ModalDialogEspecific("Atenção", r.responseJSON, r.idCliente);
                        return;
                    }
                    ModalDialog("Sucesso!", r)
                    $("#formCadastro")[0].reset();
                    $('#gridBeneficiarios tbody').empty();
            }
        });
    })

    paragrafoBeneficiarios("Cadastrar");
})
function changeURL(idCliente) {
    window.location.href = `/Cliente/Alterar/` + idCliente;
}

function mascaraCpf(i) {

    var v = i.value;

    if (isNaN(v[v.length - 1])) { // impede entrar outro caractere que não seja número
        i.value = v.substring(0, v.length - 1);
        return;
    }

    i.setAttribute("maxlength", "14");
    if (v.length == 3 || v.length == 7) i.value += ".";
    if (v.length == 11) i.value += "-";
}