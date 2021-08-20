### API - Cadastro Simplificado

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

Nesse projeto, minha intenção é apresentar um pouco sobre como podemos criar aplicações (desde as mais simples) com uma proposta de arquitetura limpa, que eventualmente pode demandar a  implementação de uma etapa de autenticação. Neste caso, utilizo o JWT (Json Web Token) com NetCore 5 e persistência em um banco de dados SQL SERVER e mais o Entity Frame Work Core (**EF Core**) e suas facilidades. 

Trata-se de aplicação *bem* simples, apenas para cadastro de usuários (telefones e seus respectivos endereços). Nada além  disso! 😅 

Contudo, você verá o uso de boas práticas de desenvolvimento; a utilização de alguns padrões de projeto, assim como a adoção de testes de software (entre testes unitários, testes de aplicação e de integração) e alguns recursos que facilitam (e muito) a vida do DEV. 

### Para baixar:

> Clone repository:

`https://github.com/fabioborges-ti/webapi.netcore-efcore-ddd-tests`

### launchSettings.json

Para tanto, primeiro você já deve ter instalado o SQL Server, criar um banco de dados para a aplicação e sua  *ConnectionString*. Tendo isso em mãos, basta alterar o valor da variável abaixo:

```bash
"CONNECTION_STRING_DEV": "<connectionString>"
```

### HealthCheck

Depois de configurar sua *ConnectionString*, execute a aplicação em modo DEBUG e verifique se está tudo **OK**! 

```bash
https://localhost:<port>/api/health
```

### Documentação da API

Para acessar a documentação da API e seus recursos, acesse: 

```bash
https://localhost:5001/swagger/index.html
```

### 💥 Importante

Você já tem o banco de dados criado e já editou sua ***launchSettings***? Se sua resposta for **SIM**, agora chegou a hora de fazer as **migrações**!

Para isso, você deve abrir o *prompt* do **Package Manager Console** no Visual Studio,  selecionar o projeto **API.Data** e digitar na linha de comando os seguintes comandos:

```bash
Add-Migration InitialCreate
```

Aguarde alguns segundos... é bem rapidinho! Logo em seguida...

```bash
Update-Database
```

Agora sim! Tudo está funcionando, você já pode usar a API 🏃

### 📚 Para mais informações:

Se você não conhece muito sobre este processo e quer mais detalhes, consulte em:

https://docs.microsoft.com/pt-br/ef/core/managing-schemas/migrations/?tabs=vs

E bom estudos! 🚀
