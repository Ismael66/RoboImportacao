if (typeof Customizacao === "undefined") {
    Customizacao = {};
}
Customizacao.Helper =
{
    calculaDigitoCpf: function (control, cpf) {
        let soma = 0;
        const maximo = control - 1;
        for (let i = 0; i < maximo; i++, control--) {
            soma += parseInt(cpf[i]) * control;
        }
        return soma;
    },

    calculaDigitoCnpj: function (control, cnpj) {
        let soma = 0;
        let multiplicador = [];
        if (control == 12) multiplicador = [5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
        else if (control == 13) multiplicador = [6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
        for (let i = 0; i < multiplicador.length; i++) {
            soma += parseInt(cnpj[i]) * multiplicador[i];
        }
        return soma;
    },

    resto: function (soma) {
        const resto = soma % 11;
        if (resto >= 2) {
            return 11 - resto;
        }
        else {
            return 0;
        }
    },

    isCpfValido: function (campoCpf) {
        if (!campoCpf) { return false; }
        campoCpf = campoCpf.replace(/\D/g, "");
        if (!campoCpf || campoCpf.length != 11 ||
            campoCpf == "00000000000" ||
            campoCpf == "11111111111" ||
            campoCpf == "22222222222" ||
            campoCpf == "33333333333" ||
            campoCpf == "44444444444" ||
            campoCpf == "55555555555" ||
            campoCpf == "66666666666" ||
            campoCpf == "77777777777" ||
            campoCpf == "88888888888" ||
            campoCpf == "99999999999") {
            return false;
        }
        const cpf = [];
        for (let i = 0; i < campoCpf.length; i++) {
            cpf[i] = parseInt(campoCpf[i]);
        }
        if (this.resto(this.calculaDigitoCpf(10, cpf)) === cpf[9]
            && this.resto(this.calculaDigitoCpf(11, cpf)) === cpf[10]) {
            return true;
        }
        else {
            return false;
        }
    },

    isCnpjValido: function (campoCnpj) {
        if (!campoCnpj) { return false; }
        campoCnpj = campoCnpj.replace(/\D/g, "");
        if (!campoCnpj || campoCnpj.length != 14 ||
            campoCnpj == "00000000000000" ||
            campoCnpj == "11111111111111" ||
            campoCnpj == "22222222222222" ||
            campoCnpj == "33333333333333" ||
            campoCnpj == "44444444444444" ||
            campoCnpj == "55555555555555" ||
            campoCnpj == "66666666666666" ||
            campoCnpj == "77777777777777" ||
            campoCnpj == "88888888888888" ||
            campoCnpj == "99999999999999") {
            return false;
        }
        const cnpj = [];
        for (let i = 0; i < campoCnpj.length; i++) {
            cnpj[i] = parseInt(campoCnpj[i]);
        }
        if (this.resto(this.calculaDigitoCnpj(12, cnpj)) === cnpj[12]
            && this.resto(this.calculaDigitoCnpj(13, cnpj)) === cnpj[13]) {
            return true;
        }
        else {
            return false;
        }
    },

    isCampoDuplicado: function (campo) {
        var fetchXml = `?fetchXml=
        <fetch top='1' no-lock='true'>
          <entity name='${campo.entidade}'>
            <attribute name='${campo.entidade}id' />
            <filter>
              <condition attribute='${campo.nome}' operator='eq' value='${campo.value}'/>
            </filter>
          </entity>
        </fetch>`;
        return Xrm.WebApi.retrieveMultipleRecords(campo.entidade, fetchXml)
            .then((result) => {
                return (result.entities.length > 0);
            })
    },

    viaCep: async function (cep) {
        const url = `https://viacep.com.br/ws/${cep}/json/`;
        const params = {
            method: "GET",
            headers: {
                "Content-Type": "application/json; charset=utf-8"
            },
            timeout: 5000
        };
        if (this.CepIsValid(cep)) {
            return fetch(url, params)
                .then((resp) => {
                    if (resp.ok) return resp.json();
                    throw new Error("Falha ao se conectar com o serviço viaCep.");
                })
                .then((data) => {
                    if ("erro" in data) throw new Error("Cep não encontrado.");
                    return data;
                });
        }
        else {
            throw new Error("Cep inválido.");
        }
    },

    CepIsValid: (cep) => {
        if (cep) {
            cep = cep.replace(/\D/g, '');
            const validacaoCEP = /^[0-9]{8}$/;
            if (cep && validacaoCEP.test(cep)) {
                return true;
            }
        }
        return false;
    },

    notificacaoCampo: function (executionContext, messagem, campo, id) {
        const formContext = typeof executionContext.getFormContext === "function" ? executionContext.getFormContext() : executionContext;
        formContext.getControl(campo).setNotification(messagem, id);
        setTimeout(() => {
            formContext.getControl(campo).clearNotification(id);
        }, 10000);
    },

    notificacaoFormulario: function (executionContext, messagem, id, level = "ERROR") {
        const formContext = typeof executionContext.getFormContext === "function" ? executionContext.getFormContext() : executionContext;
        formContext.ui.setFormNotification(messagem, level, id);
        setTimeout(() => {
            formContext.ui.clearFormNotification(id);
        }, 10000);
    }
}