using FI.AtividadeEntrevista.BLL;
using FI.AtividadeEntrevista.DML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using WebAtividadeEntrevista.Models;

namespace WebAtividadeEntrevista.Controllers
{
    public class ClienteController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Incluir()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Incluir(ClienteModel model)
        {
            BoCliente bo = new BoCliente();

            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {
                if (!bo.ValidarCPF(model.CPF))
                {
                    Response.StatusCode = 400;
                    return Json($"O CPF informado: {model.CPF} não é um CPF válido.");
                }

                if (bo.VerificarExistencia(model.CPF))
                {
                    Response.StatusCode = 400;
                    return Json($"O CPF informado: {model.CPF} já existe em nossa base.");
                }

                model.Id = bo.Incluir(new Cliente()
                {                    
                    CEP = model.CEP,
                    Cidade = model.Cidade,
                    Email = model.Email,
                    Estado = model.Estado,
                    Logradouro = model.Logradouro,
                    Nacionalidade = model.Nacionalidade,
                    Nome = model.Nome,
                    Sobrenome = model.Sobrenome,
                    Telefone = model.Telefone,
                    CPF = model.CPF
                });

                if (model.Beneficiarios != null && model.Beneficiarios.Any())
                    if (IncluirBeneficiarios(model.Beneficiarios, model.Id))
                    {
                        string message = model.Beneficiarios.Count > 1 ?
                            "Revisar os CPFs dos beneficiários" 
                            : "Revisar o CPF do beneficiário";
                        return Json(new  { responseJSON = message, idCliente = model.Id, status = -1 });
                    }
           
                return Json("Cadastro efetuado com sucesso");
            }
        }

        [HttpPost]
        public JsonResult Alterar(ClienteModel model)
        {
            BoCliente bo = new BoCliente();
       
            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {
                if (!bo.ValidarCPF(model.CPF))
                {
                    Response.StatusCode = 400;
                    return Json($"CPF informado: {model.CPF} não é um CPF válido.");
                }

                Cliente cliente = new BoCliente().Consultar(model.Id);
                if (!cliente.CPF.Equals(model.CPF))
                {
                    if (bo.VerificarExistencia(model.CPF))
                    {
                        Response.StatusCode = 400;
                        return Json($"CPF informado: {model.CPF} já existe em nossa base.");
                    }
                }

                bo.Alterar(new Cliente()
                {
                    Id = model.Id,
                    CEP = model.CEP,
                    Cidade = model.Cidade,
                    Email = model.Email,
                    Estado = model.Estado,
                    Logradouro = model.Logradouro,
                    Nacionalidade = model.Nacionalidade,
                    Nome = model.Nome,
                    Sobrenome = model.Sobrenome,
                    Telefone = model.Telefone,
                    CPF = model.CPF
                });

                if (model.Beneficiarios != null && model.Beneficiarios.Any())
                    if (IncluirBeneficiarios(model.Beneficiarios, model.Id))
                    {
                        //Response.StatusCode = 105;
                        string message = model.Beneficiarios.Count > 1 ?
                            "Revisar os CPFs dos beneficiários"
                            : "Revisar o CPF do beneficiário";
                        return Json(new { responseJSON = message, idCliente = model.Id, status = -1 });
                    }

                return Json("Cadastro alterado com sucesso");
            }
        }

        [HttpGet]
        public ActionResult Alterar(long id)
        {
            BoCliente bo = new BoCliente();
            Cliente cliente = bo.Consultar(id);
            Models.ClienteModel model = null;

            if (cliente != null)
            {
                model = new ClienteModel()
                {
                    Id = cliente.Id,
                    CEP = cliente.CEP,
                    Cidade = cliente.Cidade,
                    Email = cliente.Email,
                    Estado = cliente.Estado,
                    Logradouro = cliente.Logradouro,
                    Nacionalidade = cliente.Nacionalidade,
                    Nome = cliente.Nome,
                    Sobrenome = cliente.Sobrenome,
                    Telefone = cliente.Telefone,
                    CPF = cliente.CPF
                };

                List<Beneficiario> beneficiarios = bo.ListarBeneficiariosByCliente(model.Id);
                if (beneficiarios != null && beneficiarios.Any())
                {
                    model.Beneficiarios = new List<BeneficiarioModel>();
                    model.Beneficiarios.AddRange(beneficiarios.Select(b => new BeneficiarioModel
                    {
                        Id = b.Id,
                        Nome = b.Nome,
                        CPF = b.CPF,
                        IdCliente = b.IdCliente
                    }));
                }
            }

            return View(model);
        }

        [HttpPost]
        public JsonResult ClienteList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                int qtd = 0;
                string campo = string.Empty;
                string crescente = string.Empty;
                string[] array = jtSorting.Split(' ');

                if (array.Length > 0)
                    campo = array[0];

                if (array.Length > 1)
                    crescente = array[1];

                List<Cliente> clientes = new BoCliente().Pesquisa(jtStartIndex, jtPageSize, campo, crescente.Equals("ASC", StringComparison.InvariantCultureIgnoreCase), out qtd);

                //Return result to jTable
                return Json(new { Result = "OK", Records = clientes, TotalRecordCount = qtd });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        private bool IncluirBeneficiarios(List<BeneficiarioModel> model, long idCliente)
        {
            BoCliente bo = new BoCliente();

            bool erroCPF = false;
            List<BeneficiarioModel> list = new List<BeneficiarioModel>();

            foreach (var item in model)
                if (!bo.ValidarCPF(item.CPF) || bo.VerificarExistencia(item.CPF))
                    erroCPF = true;
                else
                    list.Add(item);

            if (list.Any())
            {
                bo.ExcluirBeneficiarioByCliente(idCliente);

                foreach (var item in list)
                    bo.IncluirBeneficiario(new Beneficiario()
                    {
                        Nome = item.Nome,
                        CPF = item.CPF,
                        IdCliente = idCliente
                    });
            }

            return erroCPF;
        }

        [HttpGet]
        public JsonResult ListarBeneficiarios(long id)
        {
            try
            {
                List<Beneficiario> beneficiarios = new BoCliente().ListarBeneficiariosByCliente(id);

                return Json(new { Result = "OK", Records = beneficiarios }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult AlterarBeneficiario(BeneficiarioModel model)
        {
            BoCliente bo = new BoCliente();

            if (!this.ModelState.IsValid || model.IdCliente == 0)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {
                if (!bo.ValidarCPF(model.CPF))
                {
                    Response.StatusCode = 400;
                    return Json($"O CPF informado: {model.CPF} não é um CPF válido.");
                }

                if (bo.VerificarExistencia(model.CPF))
                {
                    Response.StatusCode = 400;
                    return Json($"O CPF informado: {model.CPF} já existe em nossa base.");
                }

                bo.AlterarBeneficiario(new Beneficiario()
                {
                    Id = model.Id,
                    Nome = model.Nome,
                    CPF = model.CPF,
                });

                return Json("Alteração efetuada com sucesso");
            }
        }
    }
}