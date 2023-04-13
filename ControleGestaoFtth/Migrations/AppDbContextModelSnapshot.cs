﻿// <auto-generated />
using System;
using ControleGestaoFtth.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ControleGestaoFtth.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("ControleGestaoFtth.Models.Construtora", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Construtoras");
                });

            modelBuilder.Entity("ControleGestaoFtth.Models.Enderecostotais", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("BAIRRO")
                        .HasColumnType("longtext");

                    b.Property<string>("CELULA")
                        .HasColumnType("longtext");

                    b.Property<string>("CEP")
                        .HasColumnType("longtext");

                    b.Property<string>("COD_LOCALIDADE")
                        .HasColumnType("longtext");

                    b.Property<string>("COD_LOGRADOURO")
                        .HasColumnType("longtext");

                    b.Property<string>("COD_SURVEY")
                        .HasColumnType("longtext");

                    b.Property<int?>("COD_VIABILIDADE")
                        .HasColumnType("int");

                    b.Property<string>("COMPLEMENTO")
                        .HasColumnType("longtext");

                    b.Property<string>("COMPLEMENTO2")
                        .HasColumnType("longtext");

                    b.Property<string>("COMPLEMENTO3")
                        .HasColumnType("longtext");

                    b.Property<string>("DATA_ESTADO_CONTROLE")
                        .HasColumnType("longtext");

                    b.Property<string>("DISP_COMERCIAL")
                        .HasColumnType("longtext");

                    b.Property<string>("ESTACAO_ABASTECEDORA")
                        .HasColumnType("longtext");

                    b.Property<string>("ESTADO_CONTROLE")
                        .HasColumnType("longtext");

                    b.Property<string>("ID_CELULA")
                        .HasColumnType("longtext");

                    b.Property<int?>("ID_ENDERECO")
                        .HasColumnType("int");

                    b.Property<string>("LATITUDE")
                        .HasColumnType("longtext");

                    b.Property<string>("LOCALIDADE")
                        .HasColumnType("longtext");

                    b.Property<string>("LOCALIDADE_ABREV")
                        .HasColumnType("longtext");

                    b.Property<string>("LOGRADOURO")
                        .HasColumnType("longtext");

                    b.Property<string>("LONGITUDE")
                        .HasColumnType("longtext");

                    b.Property<string>("MUNICIPIO")
                        .HasColumnType("longtext");

                    b.Property<string>("NOME_CDO")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("NUM_FACHADA")
                        .HasColumnType("longtext");

                    b.Property<int?>("NUM_PISOS")
                        .HasColumnType("int");

                    b.Property<string>("PROJETO")
                        .HasColumnType("longtext");

                    b.Property<string>("QUANTIDADE_HCS")
                        .HasColumnType("longtext");

                    b.Property<int?>("QUANTIDADE_UMS")
                        .HasColumnType("int");

                    b.Property<string>("REDE_EDIF_CERT")
                        .HasColumnType("longtext");

                    b.Property<string>("REDE_INTERNA")
                        .HasColumnType("longtext");

                    b.Property<string>("TIPO_REDE")
                        .HasColumnType("longtext");

                    b.Property<string>("TIPO_SURVEY")
                        .HasColumnType("longtext");

                    b.Property<string>("TIPO_VIABILIDADE")
                        .HasColumnType("longtext");

                    b.Property<string>("UCS_COMERCIAIS")
                        .HasColumnType("longtext");

                    b.Property<string>("UCS_RESIDENCIAIS")
                        .HasColumnType("longtext");

                    b.Property<string>("UF")
                        .HasColumnType("longtext");

                    b.Property<string>("UMS_CERTIFICADAS")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Enderecostotais");
                });

            modelBuilder.Entity("ControleGestaoFtth.Models.Estacoe", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("NomeEstacao")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Sigla")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Estacoes");
                });

            modelBuilder.Entity("ControleGestaoFtth.Models.EstadoCampo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("EstadoCampos");
                });

            modelBuilder.Entity("ControleGestaoFtth.Models.Netwin", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("Codigo")
                        .HasColumnType("int");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Tipo")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Netwins");
                });

            modelBuilder.Entity("ControleGestaoFtth.Models.State", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("EstadodeControle")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("EstadodeProjeto")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("States");
                });

            modelBuilder.Entity("ControleGestaoFtth.Models.Tecnico", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Funcao")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int?>("UsuarioId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UsuarioId");

                    b.ToTable("Tecnicos");
                });

            modelBuilder.Entity("ControleGestaoFtth.Models.TesteOptico", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime?>("AceitacaoData")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("AceitacaoMesRef")
                        .HasColumnType("longtext");

                    b.Property<int?>("BobinadeLancamento")
                        .HasColumnType("int");

                    b.Property<int?>("BobinadeRecepcao")
                        .HasColumnType("int");

                    b.Property<string>("CDO")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Cabo")
                        .HasColumnType("int");

                    b.Property<int?>("Capacidade")
                        .HasColumnType("int");

                    b.Property<int?>("Celula")
                        .HasColumnType("int");

                    b.Property<int?>("ConstrutorasId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("DatadeConstrucao")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("DatadeRecebimento")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("DatadoTeste")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Endereco")
                        .HasColumnType("longtext");

                    b.Property<string>("EquipedeConstrucao")
                        .HasColumnType("longtext");

                    b.Property<int?>("EstacoesId")
                        .HasColumnType("int");

                    b.Property<int?>("EstadoCamposId")
                        .HasColumnType("int");

                    b.Property<string>("FibraDGO")
                        .HasColumnType("longtext");

                    b.Property<string>("Meta")
                        .HasColumnType("longtext");

                    b.Property<int?>("NetwinId")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<string>("Observacoes")
                        .HasColumnType("longtext");

                    b.Property<string>("PosicaoICX_DGO")
                        .HasColumnType("longtext");

                    b.Property<int?>("QuantidadeDeTeste")
                        .HasColumnType("int");

                    b.Property<string>("SplitterCEOS")
                        .HasColumnType("longtext");

                    b.Property<int?>("StatesId")
                        .HasColumnType("int");

                    b.Property<string>("Tecnico")
                        .HasColumnType("longtext");

                    b.Property<int?>("TipoObraId")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<int?>("TotalUms")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ConstrutorasId");

                    b.HasIndex("EstacoesId");

                    b.HasIndex("EstadoCamposId");

                    b.HasIndex("NetwinId");

                    b.HasIndex("StatesId");

                    b.HasIndex("TipoObraId");

                    b.ToTable("TesteOpticos");
                });

            modelBuilder.Entity("ControleGestaoFtth.Models.TipoObra", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("TipoObras");
                });

            modelBuilder.Entity("ControleGestaoFtth.Models.Usuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("Externo")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Senha")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Tipo")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Usuarios");
                });

            modelBuilder.Entity("ControleGestaoFtth.Models.Tecnico", b =>
                {
                    b.HasOne("ControleGestaoFtth.Models.Usuario", "usuario")
                        .WithMany()
                        .HasForeignKey("UsuarioId");

                    b.Navigation("usuario");
                });

            modelBuilder.Entity("ControleGestaoFtth.Models.TesteOptico", b =>
                {
                    b.HasOne("ControleGestaoFtth.Models.Construtora", "Construtora")
                        .WithMany()
                        .HasForeignKey("ConstrutorasId");

                    b.HasOne("ControleGestaoFtth.Models.Estacoe", "Estacao")
                        .WithMany()
                        .HasForeignKey("EstacoesId");

                    b.HasOne("ControleGestaoFtth.Models.EstadoCampo", "EstadoCampo")
                        .WithMany()
                        .HasForeignKey("EstadoCamposId");

                    b.HasOne("ControleGestaoFtth.Models.Netwin", "Netwin")
                        .WithMany()
                        .HasForeignKey("NetwinId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ControleGestaoFtth.Models.State", "State")
                        .WithMany()
                        .HasForeignKey("StatesId");

                    b.HasOne("ControleGestaoFtth.Models.TipoObra", "TipoObra")
                        .WithMany()
                        .HasForeignKey("TipoObraId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Construtora");

                    b.Navigation("Estacao");

                    b.Navigation("EstadoCampo");

                    b.Navigation("Netwin");

                    b.Navigation("State");

                    b.Navigation("TipoObra");
                });
#pragma warning restore 612, 618
        }
    }
}
