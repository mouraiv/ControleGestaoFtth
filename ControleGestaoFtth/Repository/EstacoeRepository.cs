﻿using ControleGestaoFtth.Context;
using ControleGestaoFtth.Models;
using ControleGestaoFtth.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace ControleGestaoFtth.Repository
{
    public class EstacoeRepository : IEstacoeRepository
    {
        private readonly AppDbContext _context;
        public EstacoeRepository(AppDbContext context)
        {
            _context = context;
        }
        public Estacoe Atualizar(Estacoe estacao)
        {
            Estacoe db = CarregarId(estacao.Id);

            if (db == null) throw new Exception("Houve um erro na atualização");

            db.Responsavel = estacao.Responsavel;
            db.Sigla = estacao.Sigla;

            _context.Estacoes.Update(db);
            _context.SaveChanges();

            return db;
        }

        public Estacoe Cadastrar(Estacoe estacao)
        {
            _context.Estacoes.Add(estacao);
            _context.SaveChanges();
            return estacao;
        }

        public Estacoe CarregarId(int id)
        {
            return _context.Estacoes
                    .Where(p => p.Id == id)
                    .First();
        }

        public bool Deletar(int id)
        {
            Estacoe db = CarregarId(id);

            if (db == null) throw new Exception("Houve um erro ao apagar");

            _context.Estacoes.Remove(db);
            _context.SaveChanges();
            return true;
        }

        public IEnumerable<Estacoe> Listar()
        {
            return _context.Estacoes
                .AsNoTracking()
                .Select(value => new Estacoe
                {
                    Id = value.Id,
                    NomeEstacao = value.NomeEstacao,

                }).OrderBy(p => p.NomeEstacao)
                .ToList();
        }

        public IEnumerable<Estacoe> Listar(int? pagina, string nomeEstacao, string responsavel)
        {
            int paginaTamanho = 10;
            int paginaNumero = (pagina ?? 1);

            IQueryable<Estacoe> resultado = _context.Estacoes.AsNoTracking();

            if (nomeEstacao != null || responsavel != null)
            {
                return resultado.
                    Where(p => p.NomeEstacao.Equals(nomeEstacao) || p.Responsavel.Equals(responsavel))
                   .ToList().ToPagedList(paginaNumero, paginaTamanho);
            }
            
            return resultado
                .ToList().ToPagedList(paginaNumero, paginaTamanho); 
        }

        public IEnumerable<Estacoe> Responsavel()
        {
            return _context.Estacoes
                .AsNoTracking()
                .AsEnumerable()
                .Select(value => new Estacoe
                {
                    Id = value.Id,
                    Responsavel = value.Responsavel

                }).DistinctBy(p => p.Responsavel)
                .OrderBy(p => p.Responsavel)
                .ToList();
        }

        public IEnumerable<Construtora> UniqueFk()
        {
            return _context.Construtoras
               .AsNoTracking()
               .Select(value => new Construtora
               {
                   EstacoesId = value.EstacoesId

               }).ToList();
        }
    }
}
