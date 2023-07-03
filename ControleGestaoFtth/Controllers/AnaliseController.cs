using Aspose.Pdf.Operators;
using ControleGestaoFtth.Models;
using ControleGestaoFtth.Models.ViewModel;
using ControleGestaoFtth.Repository;
using ControleGestaoFtth.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Drawing;

namespace ControleGestaoFtth.Controllers
{
    public class AnaliseController : Controller
    {
        private readonly IAnaliseRepository _analiseRepository;
        private readonly ITesteOpticoRepository _testeOpticoRepository;

        public AnaliseController(IAnaliseRepository analiseRepository, ITesteOpticoRepository testeOpticoRepository)
        {
            _analiseRepository = analiseRepository;
            _testeOpticoRepository = testeOpticoRepository;
        }

        public IActionResult Index()
        {
            ViewData["selectRegiao"] = _analiseRepository.Regiao();
            ViewData["selectEstado"] = _analiseRepository.Estado();
            ViewData["selectEstacao"] = _analiseRepository.Estacoes("");

            return View();
        }
        public IActionResult Inserir(int id)
        {
            TesteOptico testeOptico = _testeOpticoRepository.CarregarId(id);
            Analise analise = new();

            var analiseView = new AnaliseView { TesteOptico = testeOptico, Analise = analise };

            var statusList = new[]
                      {
                            new SelectListItem { Value = "1", Text = "APROVADO" },
                            new SelectListItem { Value = "0", Text = "REPROVADO" }
                        };
            ViewData["selectStatusFilter"] = statusList;

            return View(analiseView);
        }
        public IActionResult Editar(int id)
        {
            //ViewData["selectEstado"] = _estacaoRepository.Estado();
            Analise analise = _analiseRepository.CarregarId(id);

            return PartialView(analise);
        }
        public IActionResult Visualizar(int id)
        {
            //ViewData["selectEstado"] = _estacaoRepository.Estado();
            Analise analise = _analiseRepository.CarregarId(id);

            return PartialView(analise);
        }
        public IActionResult Confirmacao(int id)
        {
            Analise analise = _analiseRepository.CarregarId(id);

            return PartialView(analise);
        }
        [HttpPost]
        public IActionResult InserirImagem(List<IFormFile> filesData)
        {
            try
            {
                //RECUPERA ESTADO UF E SIGLA ESTAÇÃO DO FORMULÁRIO
                var uf = Request.Form["Uf"];
                var slg = Request.Form["Slg"];
                var cdo = Request.Form["Cdo"];


                //CAMINHO PARA UPLOADS DE ARQUIVOS
                string pasta = "Upload\\TesteOptico\\Anexos\\" + uf + "\\" + slg + "\\TESTE_OPTICO\\";

                foreach (var file in filesData)
                {
                    if (file != null && file.Length > 0)
                    {
                        // Obtém o nome do arquivo
                        var fileName = Path.GetFileName(file.FileName);

                        // Cria a pasta com o nome do arquivo
                        var folderPath = Path.Combine(pasta, cdo);
                        Directory.CreateDirectory(folderPath);

                        // Salva o arquivo no diretório criado
                        var filePath = Path.Combine(folderPath, fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                    }
                }
                return Json(new { resposta = "SUCESSO" });
            }catch(Exception error)
            {
                TempData["Falha"] = $"Erro ao inserir a imagem - {error.Message}.";
                return Json(new { resposta = "FALHA" });
            }
        }
       [HttpPost]
        public IActionResult Inserir(AnaliseView analiseView)
        {
            try
            {
                var statusList = new[]
                    {
                            new SelectListItem { Value = "1", Text = "APROVADO" },
                            new SelectListItem { Value = "0", Text = "REPROVADO" }
                        };

                ViewData["selectStatusFilter"] = statusList;

                //analiseView.Analise.Id = _analiseRepository.LastId() + 1;
                analiseView.Analise.TesteOpticoId = int.Parse(Request.Form["TesteOpticoId"]);
                analiseView.Analise.TecnicoId = int.Parse(Request.Form["TecnicoId"]);
                analiseView.Analise.DataAnalise = DateTime.Now;
                analiseView.Analise.Observacao = Request.Form["Observacao"];
                analiseView.Analise.CDOIA = Request.Form["Cdoia"];
                analiseView.Analise.CDOIAStatus = Request.Form["CdoiaStatus"];
                analiseView.Analise.CDOIA_Obs = Request.Form["CdoiaObs"];

                _analiseRepository.Cadastrar(analiseView.Analise);

                if (analiseView.Analise.Id > 0) {
                    TempData["Sucesso"] = "Inserido com sucesso.";
                    return RedirectToAction("Detalhe", new { id = analiseView.Analise.TesteOpticoId });
                }
                else
                {
                    TempData["Sucesso"] = "Inserido com sucesso.";
                    return View("Index");
                }

            }
            catch (Exception error)
            {
                if (analiseView.Analise.Id > 0)
                {
                    TempData["Falha"] = $"Erro ao inserir - {error.Message}.";
                    return RedirectToAction("Detalhe", new { id = analiseView.Analise.TesteOpticoId });
                }
                else
                {
                    TempData["Falha"] = $"Erro ao inserir - {error.Message}.";
                    return View("Index");
                }
            }
        }
       
        [HttpPost]
        public IActionResult Editar(Analise analise)
        {
            try
            {
                //ViewData["selectEstado"] = _estacaoRepository.Estado();
                _analiseRepository.Atualizar(analise);
                TempData["Sucesso"] = "Editado com sucesso.";
                return RedirectToAction("Editar", new { id = analise.Id });
            }
            catch (Exception error)
            {
                TempData["Falha"] = $"Erro ao editar - {error.Message}.";

                return View(analise);
            }
        }
        [HttpGet]
        public IActionResult Apagar(int id)
        {
            try
            {
                var testeOpticoId = _analiseRepository.CarregarId(id).TesteOpticoId;
                _analiseRepository.Deletar(id);
                TempData["Sucesso"] = $"Analise Técnica excluída com sucesso.";
                return RedirectToAction("Detalhe", new { id = testeOpticoId });
            }
            catch (Exception error)
            {
                var testeOpticoId = _analiseRepository.CarregarId(id).TesteOpticoId;
                TempData["Falha"] = $"Erro ao excluir - {error.Message}.";
                return RedirectToAction("Detalhe", new { id = testeOpticoId });
            }
        }
        [HttpGet]
        public IActionResult Listar(int? pagina, string? regiao, string estado, string? estacao, int? cdo, int? tecnico, int? status)
        {
            try
            {
                var statusList = new[]
                        {
                            new SelectListItem { Value = "1", Text = "APROVADO" },
                            new SelectListItem { Value = "0", Text = "REPROVADO" }
                        };
                ViewData["selectStatusFilter"] = statusList;

                if (estacao != null)
                {
                    ViewData["selectCdoFilter"] = _analiseRepository.TesteOptico(estacao);
                    ViewData["selectTecnicoFilter"] = _analiseRepository.Tecnico();
                }
                else
                {
                    ViewData["selectCdoFilter"] = _analiseRepository.TesteOptico("");
                    ViewData["selectTecnicoFilter"] = _analiseRepository.Tecnico();
                }

                IEnumerable<Analise> listar = _analiseRepository.Listar(pagina, regiao, estado, estacao, cdo, tecnico, status);

                return PartialView(listar);
            }
            catch (Exception error)
            {
                TempData["Falha"] = $"Erro ao listar - {error.Message}.";
                return PartialView();
            }

        }
        [HttpGet]
        public IActionResult Detalhe(int id)
        {

            try
            {
                Analise analise = _analiseRepository.CarregarIdTesteOptico(id);

                //TempData["HstRegiao"] = testeOptico.Estacao.Estado.Regiao.Nome.ToString();
                //TempData["HstEstacao"] = testeOptico.Estacao.NomeEstacao.ToString();
                //TempData["HstEstado"] = testeOptico.Estacao.Estado.Nome.ToString();

                return View(analise);
            }
            catch (Exception error)
            {
                TempData["Falha"] = $"Erro ao listar - {error.Message}.";

                return RedirectToAction("Index");
            }

        }
        public IActionResult GetListDropDown(string regiao, string estado, string estacao)
        {
            if (regiao != null && estado == null)
            {
                var estadoList = _analiseRepository.Estado(regiao).Select(value => new Estado { Nome = value.Nome });
                return Json(new { estado = estadoList });

            }
            else if (estado != null)
            {
                var estacaoList = _analiseRepository.Estacoes(estado).Select(value => new Estacoe { NomeEstacao = value.NomeEstacao });
                return Json(new { estacao = estacaoList });
            }
            else
            {
                return Json(new
                {
                    estado = _analiseRepository.Estado().Select(value => new Estado { Nome = value.Nome }),
                    estacao = _analiseRepository.Estacoes("").Select(value => new Estacoe { NomeEstacao = value.NomeEstacao })
                });
            }
        }
        public IActionResult GetHistoricoAnalise(int id)
        {
           return Json(new
             {
                historico = _analiseRepository.Historico(id)
             });
          
        }
    }
}
