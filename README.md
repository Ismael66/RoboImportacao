# Robô para importação de dados em massa Dynamics 365 

O objetivo desse projeto é a criação de um robô capaz da importação em massa de dados de um ambiente para outro no Dynamics 365, sem a repetição de dados e com as relações entre as entidades intactas.

# Desenvolvimento

- Primeiramente, foi criado um robô capaz de popular o ambiente de origem, a fim de que não fosse necessário o trabalho de preencher dados manualmente.
- Posteriormente, foi criado o robô designado a fazer a importação, capaz de copiar todos os dados do ambiente de origem para o de destino.

# Adicionais 

- Foi adicionado para as entidades Conta e Contanto, respectivamente, os campos Cnpj e Cpf validando a legitimidade e a repetição de dados em outros registros. A validação é feita por Plugin e Js.
- Foi adicionado uma Api capaz de preencher campos do endereço por meio do cep. (Api usada: ViaCep)
