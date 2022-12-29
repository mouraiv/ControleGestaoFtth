﻿using ControleGestaoFtth.Models;

namespace ControleGestaoFtth.Repository.Interface
{
    public interface IConstrutoraRepository
    {
        Construtora Cadastrar(Construtora construtora);
        Construtora Atualizar(Construtora construtora);
        bool Deletar(int id);
        IEnumerable<Construtora> Pesquisar(int? pagina);
        Construtora CarregarId(int id);
        IEnumerable<Construtora> Construtoras();
    }
}
