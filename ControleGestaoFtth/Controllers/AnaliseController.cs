using Aspose.Pdf.Operators;
using ControleGestaoFtth.Models;
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

        public AnaliseController(IAnaliseRepository analiseRepository)
        {
            _analiseRepository = analiseRepository;
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
            Analise analise = _analiseRepository.CarregarIdTesteOptico(id);
            var statusList = new[]
                      {
                            new SelectListItem { Value = "1", Text = "APROVADO" },
                            new SelectListItem { Value = "0", Text = "REPROVADO" }
                        };
            ViewData["selectStatusFilter"] = statusList;
            return View(analise);
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
        public IActionResult Inserir(Analise analise, List<IFormFile> files)
        {
            try
            {
                var statusList = new[]
                    {
                            new SelectListItem { Value = "1", Text = "APROVADO" },
                            new SelectListItem { Value = "0", Text = "REPROVADO" }
                        };

                ViewData["selectStatusFilter"] = statusList;

                //RECUPERA ESTADO UF E SIGLA ESTAÇÃO DO FORMULÁRIO
                var uf = Request.Form["Uf"];
                var slg = Request.Form["Slg"];
                var cdo = Request.Form["Cdo"];

                //CAMINHO PARA UPLOADS DE ARQUIVOS
                string pasta = "Upload\\TesteOptico\\Anexos\\" + uf + "\\" + slg + "\\TESTE_OPTICO\\";

                analise.Id = _analiseRepository.LastId() + 1;
                analise.TecnicoId = int.Parse(Request.Form["TecnicoId"]);
                analise.DataAnalise = DateTime.Now;
                analise.Observacao = Request.Form["Observacao"];
                analise.CDOIA = "";
                analise.CDOIAStatus = "";
                analise.CDOIA_Obs = "";

                _analiseRepository.Cadastrar(analise);

                foreach (var file in files)
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

                TempData["Sucesso"] = "Inserido com sucesso.";
                return RedirectToAction("Detalhe", new { id = analise.TesteOpticoId });

            }
            catch (Exception error)
            {
                TempData["Falha"] = $"Erro ao inserir - {error.Message}.";
                return RedirectToAction("Detalhe", new { id = analise.TesteOpticoId });
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
