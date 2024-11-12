using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FI.AtividadeEntrevista.BLL
{
    public class BoCliente
    {
        /// <summary>
        /// Inclui um novo cliente
        /// </summary>
        /// <param name="cliente">Objeto de cliente</param>
        public long Incluir(DML.Cliente cliente)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.Incluir(cliente);
        }

        /// <summary>
        /// Altera um cliente
        /// </summary>
        /// <param name="cliente">Objeto de cliente</param>
        public void Alterar(DML.Cliente cliente)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            cli.Alterar(cliente);
        }

        /// <summary>
        /// Consulta o cliente pelo id
        /// </summary>
        /// <param name="id">id do cliente</param>
        /// <returns></returns>
        public DML.Cliente Consultar(long id)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.Consultar(id);
        }

        /// <summary>
        /// Excluir o cliente pelo id
        /// </summary>
        /// <param name="id">id do cliente</param>
        /// <returns></returns>
        public void Excluir(long id)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            cli.Excluir(id);
        }

        /// <summary>
        /// Lista os clientes
        /// </summary>
        public List<DML.Cliente> Listar()
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.Listar();
        }

        /// <summary>
        /// Lista os clientes
        /// </summary>
        public List<DML.Cliente> Pesquisa(int iniciarEm, int quantidade, string campoOrdenacao, bool crescente, out int qtd)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.Pesquisa(iniciarEm,  quantidade, campoOrdenacao, crescente, out qtd);
        }

        /// <summary>
        /// VerificaExistencia
        /// </summary>
        /// <param name="CPF"></param>
        /// <returns></returns>
        public bool VerificarExistencia(string CPF)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.VerificarExistencia(CPF);
        }

        public bool ValidarCPF(string cpf)
        {
            // Remove caracteres não numéricos
            cpf = new string(cpf.Where(char.IsDigit).ToArray());

            // Verifica se o CPF possui 11 dígitos ou se todos os números são iguais (inválido)
            if (cpf.Length != 11 || cpf.Distinct().Count() == 1)
                return false;

            // Calcula o primeiro dígito verificador
            int soma = 0;
            for (int i = 0; i < 9; i++)
                soma += (cpf[i] - '0') * (10 - i);

            int primeiroDigitoVerificador = (soma * 10) % 11;
            if (primeiroDigitoVerificador == 10)
                primeiroDigitoVerificador = 0;

            // Valida o primeiro dígito verificador
            if (primeiroDigitoVerificador != (cpf[9] - '0'))
                return false;

            // Calcula o segundo dígito verificador
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += (cpf[i] - '0') * (11 - i);

            int segundoDigitoVerificador = (soma * 10) % 11;
            if (segundoDigitoVerificador == 10)
                segundoDigitoVerificador = 0;

            // Valida o segundo dígito verificador
            return segundoDigitoVerificador == (cpf[10] - '0');
        }

        public long IncluirBeneficiario(DML.Beneficiario beneficiario)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.IncluirBeneficiario(beneficiario);
        }

        /// <summary>
        /// Lista os beneficiários
        /// </summary>
        public List<DML.Beneficiario> ListarBeneficiariosByCliente(long id)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.ListarBeneficiariosByCliente(id);
        }

        public void AlterarBeneficiario(DML.Beneficiario cliente)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            cli.AlterarBeneficiario(cliente);
        }

        public void ExcluirBeneficiarioByCliente(long id)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            cli.ExcluirBeneficiarioByCliente(id);
        }
    }
}
