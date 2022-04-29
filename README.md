# Projeto de gerador de rotas

### Funcionalidades do projeto:


• Cadastro de cidades, pessoas, times e usuários

• Upload do arquivo xlsx

• Leitura do arquivo

• Filtragem dos dados

• Escrita do relatório

### Fluxo:


•Primeiro há a necessidade de fzer login para utilizar os recursos do programa

•Depois será preciso fazer o upload do arquivo xlsx

•Seleção das colunas para a escrita do relatório

•Filtragem por cidade e serviço

•Escolha dos times

•Geração do relatório

•Download do relatório

### Detalhes e como utilizar:


• Devido ao fato de a aplicação contar com a funcionalidade de login, o que impede que usuários não cadastrados tenham acesso às funcionalidades desta, por isso, na primeira iniciação é criada uma conta temporária de login "Admin" e senha "Admin". Entrando por essa conta será possível criar um novo usuário para assim seguir o fluxo.


• Após feito o login, na primeira iniciação, é necessário que antes da criação da rota, seja feito o upload da planilha que será lida e cadastrar as pessoas, cidades e times.


• Os itens que compõem a barra de navegação são: 

- "Menu", onde é possível fazer o upload da planilha;

- "Pessoas", onde é possivel ver a lista de pessoas, cadastrar, editar, deletar e ver os detalhes de uma pessoa;

- "Cidades", onde é possivel ver a lista de cidades, cadastrar, editar, deletar e ver os detalhes de uma cidade;

- "Time", onde é possível ver a lista de times, cadastrar, editar, deletar e ver os detalhes de um time;

- "Rotas de Serviço", onde é feito toda a filtragem e geração do relatório;

- "Login", onde o usuário entra com sua conta e pode criar uma nova caso tenha a *'role'* "Admin".

• Para que a aplicação funcione corretamente, é necessário que todas as APIs estejam rodando juntamente com o MVC, pois é por elas que são feitas as consultas ao banco de dados.

• O banco de dados utilizado foi o MongoDB.


