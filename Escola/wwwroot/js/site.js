
$(document).ready(function () {

    $('#inputCPF').mask('000.000.000-00', { placeholder: "___.___.___-__" });

    //MATRICULA
    $('.btn-nova-matricula').click(function () {

        $.ajax({
            type: 'GET',
            url: '/Matricula/Create',
            success: function (result) {
                $('#dadosCadastroMatricula').html(result);
                $('#modalNovaMatricula').modal();
            }
        });
    });

    $('.btn-editar-matricula').click(function () {
        var matriculaId = $(this).attr('matricula-id');

        $.ajax({
            type: 'GET',
            url: '/Matricula/Editar/' + matriculaId,
            success: function (result) {
                $('#dadosEdicaoMatricula').html(result);
                $('#modalEditarMatricula').modal();
            }
        });
    });

    $('.btn-detalhes-matricula').click(function () {
        var matriculaId = $(this).attr('matricula-id');

        $.ajax({
            type: 'GET',
            url: '/Matricula/Details/' + matriculaId,
            success: function (result) {
                $('#dadosDetalhesMatricula').html(result);
                $('#modalDetalhesMatricula').modal();
            }
        });
    });

    $('.btn-inativar-matricula').click(function () {
        var matriculaId = $(this).attr('matricula-id');

        $.ajax({
            type: 'GET',
            url: '/Matricula/Delete/' + matriculaId,
            success: function (result) {
                $('#dadosInativarMatricula').html(result);
                $('#modalInativarMatricula').modal();
            }
        });
    });

    $('.btn-reativar-matricula').click(function () {
        var matriculaId = $(this).attr('matricula-id');

        $.ajax({
            type: 'GET',
            url: '/Matricula/Reativar/' + matriculaId,
            success: function (result) {
                $('#dadosReativarMatricula').html(result);
                $('#modalReativarMatricula').modal();
            }
        });
    });

    //CURSO
    $('.btn-novo-curso').click(function () {

        $.ajax({
            type: 'GET',
            url: '/Curso/Create',
            success: function (result) {
                $('#dadosCadastroCurso').html(result);
                $('#modalNovoCurso').modal();
            }
        });
    });

    $('.btn-editar-curso').click(function () {
        var cursoId = $(this).attr('curso-id');

        $.ajax({
            type: 'GET',
            url: '/Curso/Edit/' + cursoId,
            success: function (result) {
                $('#dadosEdicaoCurso').html(result);
                $('#modalEditarCurso').modal();
            }
        });
    });

    $('.btn-detalhes-curso').click(function () {
        var cursoId = $(this).attr('curso-id');

        $.ajax({
            type: 'GET',
            url: '/Curso/Details/' + cursoId,
            success: function (result) {
                $('#dadosDetalhesCurso').html(result);
                $('#modalDetalhesCurso').modal();
            }
        });
    });

    $('.btn-inativar-curso').click(function () {
        var cursoId = $(this).attr('curso-id');

        $.ajax({
            type: 'GET',
            url: '/Curso/Delete/' + cursoId,
            success: function (result) {
                $('#dadosInativarCurso').html(result);
                $('#modalInativarCurso').modal();
            }
        });
    });

    $('.btn-reativar-curso').click(function () {
        var cursoId = $(this).attr('curso-id');

        $.ajax({
            type: 'GET',
            url: '/Curso/Reativar/' + cursoId,
            success: function (result) {
                $('#dadosReativarCurso').html(result);
                $('#modalReativarCurso').modal();
            }
        });
    });


    //ALUNO
    $('.btn-novo-aluno').click(function () {

        $.ajax({
            type: 'GET',
            url: '/Aluno/Create',
            success: function (result) {
                $('#dadosCadastroAluno').html(result);
                $('#modalNovoAluno').modal();
            }
        });
    });

    $('.btn-editar-aluno').click(function () {
        var alunoId = $(this).attr('aluno-id');

        $.ajax({
            type: 'GET',
            url: '/Aluno/Edit/' + alunoId,
            success: function (result) {
                $('#dadosEdicaoAluno').html(result);
                $('#modalEditarAluno').modal();
            }
        });
    });

    $('.btn-detalhes-aluno').click(function () {
        var alunoId = $(this).attr('aluno-id');

        $.ajax({
            type: 'GET',
            url: '/Aluno/Details/' + alunoId,
            success: function (result) {
                $('#dadosDetalhesAluno').html(result);
                $('#modalDetalhesAluno').modal();
            }
        });
    });

    $('.btn-inativar-aluno').click(function () {
        var alunoId = $(this).attr('aluno-id');

        $.ajax({
            type: 'GET',
            url: '/Aluno/Inativar/' + alunoId,
            success: function (result) {
                $('#dadosInativarAluno').html(result);
                $('#modalInativarAluno').modal();
            }
        });
    });

    $('.btn-reativar-aluno').click(function () {
        var alunoId = $(this).attr('aluno-id');

        $.ajax({
            type: 'GET',
            url: '/Aluno/Reativar/' + alunoId,
            success: function (result) {
                $('#dadosReativarAluno').html(result);
                $('#modalReativarAluno').modal();
            }
        });
    });

    $('.btn-sou-aluno').click(function () {

        $.ajax({
            type: 'GET',
            url: '/Aluno/SouAluno',
            success: function (result) {
                $('#dadosSouAluno').html(result);
                $('#modalSouAluno').modal();
            }
        });
    });

    $('.formBuscaMatriculas').submit(function (event) {
        event.preventDefault();

        var cpf = $('#CPF').val();
        var dataNascimento = $('#DataNascimento').val();

        $.ajax({

            type: 'GET',
            url: '/Aluno/SouAlunoList',
            data: {
                cpf: cpf,
                dataNascimento: dataNascimento
            },
            success: function (result) {
                $('#dadosSouAlunoMatriculas').html(result);
                $('#modalSouAlunoMatriculas').modal();
            }
        });
    });
});