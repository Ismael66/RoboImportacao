using System;
using Microsoft.Xrm.Sdk;

namespace Plugin
{
    public class PluginValidacaoAccount : Base
    {
        public override void Execute()
        {
            try
            {
                if (Validate(MessageName.Create, Stage.PreOperation, Mode.Synchronous)
                || Validate(MessageName.Update, Stage.PreOperation, Mode.Synchronous))
                {
                    var entidadeContexto = GetInputParameter<Entity>();
                    if (entidadeContexto?.LogicalName == "account")
                    {
                        this.ValidaCnpj(entidadeContexto);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException(ex.Message);
            }
        }
        void ValidaCnpj(Entity target)
        {
            var validador = target.GetAttributeValue<bool>("cr6de_validadorcnpj");
            if (validador == false)
            {
                var cnpj = target.GetAttributeValue<string>("cr6de_cnpj");
                if (!string.IsNullOrEmpty(cnpj))
                {
                    if (Helper.IsCnpjValido(cnpj))
                    {
                        cnpj = Helper.FormatarCnpj(cnpj);
                        var result = RequisicoesDataverse.ValidarCnpjAccount(UserService, cnpj);
                        if (result.Entities.Count > 0) throw new Exception("Cnpj duplicado.");
                        target["cr6de_validadorcnpj"] = true;
                        target["cr6de_cnpj"] = cnpj;
                        Context.InputParameters["Target"] = target;
                    }
                    else
                    {
                        throw new Exception("Cnpj inválido.");
                    }
                }
            }
            else
            {
                throw new Exception("Cnpj é obrigatório.");
            }
        }
    }
}


