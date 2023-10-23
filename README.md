

# 📚 PROJETO ESCOLA

Aplicação desenvolvida com o intuito de cadastrar alunos, cursos e posteriormente criar as matrículas dos alunos, utilizando validações como status, duplicidade e quantidade de vagas do curso.

-----------------
### ⚙️ Funcionalidades

- Cadastro dos aluno com validação de CPF;
- Cadastro dos cursos passando a quantidade de vagas disponíveis;
- Criar a matrícula vinculando o alnuo com o curso;
- No momento de criação e edição da matricula é validado se o curso está ativo e se tem vaga disponível;
- Não é permitido a inativação do aluno e/ou do curso caso exista matrículas ativas.

-----------------
-----------------
### 📋 Requisitos

- Visual Studio Community 2022;
- Instância do Sql Server.

-----------------
### 🔧 Configuração

Depois da instalação dos programas necessários, seguir os passos:

  - Clonar o projeto;
       ```
       SSH: git@github.com:paulogabri-el/projetoEscolaCurso.git
       HTTPS: https://github.com/paulogabri-el/projetoEscolaCurso.git
       ```
  
  - Configurar a string de conexão do seu Sql Server no program.cs;
      ``` "string" ```
      
  - Salvar o projeto e rodar o comando "Update-Database" no console do gerenciador de pacotes;
  
      Uma vez feito, o EF vai o banco de dados seguindo a string de conexão configurada e o devido mapeamento esquematizado no projeto.
  
  - Ao executar a aplicação você pode criar um usuário e testar a aplicação que contem originalmente as funcionalidades descritas.

-----------------
## ✒️ Autor

* **Paulo Gabriel** - [Perfil GitHub](https://github.com/paulogabri-el) / [Perfil LinkedIn](https://www.linkedin.com/in/paulogabri-el/)

-----------------
## 📌 Extra

* Projeto desenvolvido para uma avaliação prática;
* Aprimorou conhecimentos já existentes e contribuiu para o aprendizado de novo recursos das tecnologias utilizadas.


-----------------
