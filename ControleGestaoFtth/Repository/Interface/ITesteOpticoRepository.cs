﻿using ControleGestaoFtth.Models;

namespace ControleGestaoFtth.Repository.Interface
{
    public interface ITesteOpticoRepository
    {
        TesteOptico Cadastrar(TesteOptico testeOptico);
        TesteOptico Atualizar(TesteOptico testeOptico);
        bool Deletar(int id);
        TesteOptico CarregarId(int id);
        IEnumerable<TesteOptico> Listar(int? pagina, string estacao, string cdo, int? cabo, int? celula);
        IEnumerable<TesteOptico> UniqueCdo();
        IEnumerable<Estacoe> Estacoes();
        IEnumerable<Netwin> Netwins();
        IEnumerable<TipoObra> TipoObras();
        IEnumerable<EstadoCampo> EstadoCampos();
        IEnumerable<TesteOptico> FilterCdo(string estacao);
        IEnumerable<TesteOptico> FilterCabo(string estacao);
        IEnumerable<TesteOptico> FilterCelula(string estacao);
    }
}