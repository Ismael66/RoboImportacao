using System;
using Microsoft.Xrm.Sdk;

namespace Plugin
{
    public class PluginValidacaoContact : Base
    {
        public override void Execute()
        {
            try
            {
                if (Validate(MessageName.Create, Stage.PreOperation, Mode.Synchronous)
                || Validate(MessageName.Update, Stage.PreOperation, Mode.Synchronous))
                {
                    var entidadeContexto = GetInputParameter<Entity>();
                    if (entidadeContexto?.LogicalName == "contact")
                    {
                        this.ValidaCpf(entidadeContexto);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException(ex.Message);
            }
        }
        void ValidaCpf(Entity target)
        {
            var validador = target.GetAttributeValue<bool>("cr6de_validadorcpf");
            if (validador == false)
            {
                var cpf = target.GetAttributeValue<string>("cr6de_cpf");
                if (!string.IsNullOrEmpty(cpf))
                {
                    if (Helper.IsCpfValido(cpf))
                    {
                        cpf = Helper.FormatarCpf(cpf);
                        var result = RequisicoesDataverse.ValidarCpfContact(UserService, cpf);
                        if (result.Entities.Count > 0) throw new Exception("Cpf duplicado.");
                        target["cr6de_validadorcpf"] = true;
                        target["cr6de_cpf"] = cpf;
                        Context.InputParameters["Target"] = target;
                    }
                    else
                    {
                        throw new Exception("Cpf inválido.");
                    }
                }
                else
                {
                    throw new Exception("Cpf é obrigatório.");
                }
            }
        }
    }
}
