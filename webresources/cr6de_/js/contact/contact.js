if (typeof Customizacao === "undefined") {
    Customizacao = {};
}
Customizacao.Contact =
{
    validacaoCpf: function (executionContext) {
        const formContext = typeof executionContext.getFormContext === "function" ? executionContext.getFormContext() : executionContext;
        const valorCpf = formContext.getAttribute("cr6de_cpf")?.getValue();
        if (Customizacao.Helper.isCpfValido(valorCpf)) {
            formContext.getControl("cr6de_cpf").clearNotification("error.cpf");
            const campo = {
                nome: "cr6de_cpf",
                entidade: "contact",
                value: valorCpf
            }
            Customizacao.Helper.isCampoDuplicado(campo)
                .then((isDuplicada) => {
                    if (isDuplicada) {
                        formContext.getControl("cr6de_cpf").setNotification("Cpf duplicado.", "error.cpf");
                    }
                    else {
                        formContext.getControl("cr6de_cpf").clearNotification("error.cpf");
                        formContext.getAttribute("cr6de_cpf").setValue(valorCpf.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, "$1.$2.$3-$4"));
                        formContext.getAttribute("cr6de_validadorcpf").setValue(true);
                    }
                })
                .catch(error => {
                    Customizacao.Helper.notificacaoFormulario(executionContext, error.message, "error.cpf");
                });
        }
        else {
            formContext.getControl("cr6de_cpf").setNotification("Cpf inválido.", "error.cpf");
        }
    },

    insereCamposEndereco: function (executionContext) {
        const formContext = typeof executionContext.getFormContext === "function" ? executionContext.getFormContext() : executionContext;
        const cep = formContext.getAttribute("address1_postalcode").getValue();
        Customizacao.Helper.viaCep(cep)
            .then((data) => {
                formContext.getAttribute("address1_postalcode").setValue(cep.replace(/\D/g, "").replace(/^(\d{5})(\d{3})+?$/, "$1-$2"));
                this.setEndereco(executionContext, data);
            })
            .catch((error) => {
                Customizacao.Helper.notificacaoCampo(executionContext, error.message, "address1_postalcode", "error.viacep");
                this.setEndereco(executionContext);
            })
    },

    setEndereco: function (executionContext, data = null) {
        const formContext = typeof executionContext.getFormContext === "function" ? executionContext.getFormContext() : executionContext;
        formContext.getAttribute("address1_line1").setValue(data?.logradouro);
        formContext.getAttribute("address1_line3").setValue(data?.bairro);
        formContext.getAttribute("address1_city").setValue(data?.localidade);
        formContext.getAttribute("address1_stateorprovince").setValue(data?.uf);
        formContext.getAttribute("address1_country").setValue(data ? "Brasil" : null);
    }
}