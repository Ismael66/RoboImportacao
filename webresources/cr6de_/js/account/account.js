if (typeof Customizacao === "undefined") {
    Customizacao = {};
}
Customizacao.Account =
{
    validacaoCnpj: function (executionContext) {
        const formContext = typeof executionContext.getFormContext === "function" ? executionContext.getFormContext() : executionContext;
        const valorCnpj = formContext.getAttribute("cr6de_cnpj")?.getValue();
        if (Customizacao.Helper.isCnpjValido(valorCnpj)) {
            formContext.getControl("cr6de_cnpj").clearNotification("error.cnpj");
            const campo = {
                nome: "cr6de_cnpj",
                entidade: "account",
                value: valorCnpj
            }
            Customizacao.Helper.isCampoDuplicado(campo)
                .then((isDuplicada) => {

                    if (isDuplicada) {
                        formContext.getControl("cr6de_cnpj").setNotification("Cnpj duplicado.", "error.cnpj");
                    }
                    else {
                        formContext.getControl("cr6de_cnpj").clearNotification("error.cnpj");
                        formContext.getAttribute("cr6de_cnpj").setValue(valorCnpj.replace(/^(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})/, "$1.$2.$3/$4-$5"));
                        //formContext.getAttribute("cr6de_validadorcpf").setValue("Sim");
                    }
                })
                .catch(error => {
                    Customizacao.Helper.notificacaoFormulario(executionContext, error.message, "error.cnpj");
                });
        }
        else {
            formContext.getControl("cr6de_cnpj").setNotification("Cnpj inválido.", "error.cnpj");
        }
    }
}