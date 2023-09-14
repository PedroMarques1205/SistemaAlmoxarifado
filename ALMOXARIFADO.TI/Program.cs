using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;
using System.Threading;
using ALMOXARIFADO.TI.Classes;

namespace Sistema_Almoxarifado_TI
{
    internal class Program
    {
        /// <summary>
        /// FUNÇÃO PARA EXECUTAR UMA BUSCA POR PARAMETROS
        /// </summary>
        /// <param name="querry">Consulta a ser executada</param>
        /// <param name="parametro">Campo não obrigatório de lista de parâmetros</param>
        /// <returns>Retorna os dados em uma Datatable</returns>
        public static DataTable ExecuteQuery(string querry, List<SqlParameter> parametro = null)
        {
            string connectionString = @"Server=BRSAE-W-4422;Database=DB_EQUIPMENTS;
                                        User Id=sa;
                                        password=Saetowers2023;
                                        Trusted_Connection=False;
                                        MultipleActiveResultSets=true;";
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            DataTable dt = new DataTable();

            try
            {
                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand(querry, sqlConnection);

                if (parametro!=null && parametro.Any())
                {
                    foreach (SqlParameter item in parametro)
                    {
                        cmd.Parameters.Add(item);
                    }
                }

                SqlDataReader reader = cmd.ExecuteReader();
                dt.Load(reader);
            }
            catch (Exception ex)
            {
                dt = null;
            }
            finally
            {
                sqlConnection.Close();
            }

            return dt;
        }

        /// <summary>
        /// FUNÇÃO PARA EXECUTAR UMA MUDANÇA NO BANCO POR PARAMETROS
        /// </summary>
        /// <param name="querry">Consulta a ser executada</param>
        /// <param name="parametro">Campo não obrigatório de lista de parâmetros</param>
        public static void ExecuteChanges(string querry, List<SqlParameter> parametro = null)
        {
            string connectionString = "Server=BRSAE-W-4422;Database=DB_EQUIPMENTS;User Id=sa;password=Saetowers2023;Trusted_Connection=False;MultipleActiveResultSets=true;";
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();

            try
            {
                SqlCommand cmd = new SqlCommand(querry, sqlConnection);
                cmd.Transaction = sqlTransaction;

                if (parametro!=null && parametro.Any())
                {
                    foreach (SqlParameter item in parametro)
                    {
                        cmd.Parameters.Add(item);
                    }
                }

                cmd.ExecuteNonQuery();
                sqlTransaction.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                sqlConnection.Close();
                sqlTransaction.Rollback();
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        /// <summary>
        /// FUNÇÃO PARA EXIBIR MENU DE USUÁRIO
        /// </summary>
        public static void ExibirMenu()
        {
            Console.Clear();
            Console.WriteLine("\n");
            Console.WriteLine("MENU DE OPÇÕES");
            Console.WriteLine("(1) Listar items do estoque");
            Console.WriteLine("(2) Adicionar um item ao estoque");
            Console.WriteLine("(3) Editar item do estoque");
            Console.WriteLine("(4) Remover item do estoque");
            Console.WriteLine("(5) Cadastrar um empréstimo de item");
            Console.WriteLine("(6) Cadastrar um depósito de item");
            Console.WriteLine("(7) Filtrar empréstimos por data");
            Console.WriteLine("(8) Filtrar depósitos por data");
            Console.WriteLine("(9) Pesquisar movimentações de uma pessoa");
            Console.WriteLine("(0) FINALIZAR PROGRAMA");
            Console.Write("\nDigite uma das opções:");
        }

        /// <summary>
        /// FUNÇÃO PARA RECEBER NOME DO EQUIPAMENTO DO USUÁRIO
        /// </summary>
        /// <returns>NOME DO EQUIPAMENTO</returns>
        public static string GetNome()
        {
            Console.Write("Digite o nome do item que deseja adicionar no almoxarifado: ");
            string nome = Convert.ToString(Console.ReadLine());

            return nome;
        }

        /// <summary>
        /// FUNÇÃO PARA RECEBER NOME DO MOVIMENTADOR DO USUÁRIO
        /// </summary>
        /// <returns>NOME DO MOVIMENTADOR</returns>
        public static string GetNomeMovimentador()
        {
            Console.Write("Digite o nome do movimentador: ");
            string nome = Convert.ToString(Console.ReadLine());

            return nome;
        }

        /// <summary>
        /// FUNÇÃO PARA RECEBER ID DO EQUIPAMENTO DO USUÁRIO
        /// </summary>
        /// <returns>ID DO EQUIPAMENTO</returns>
        public static int GetId()
        {
            DataTable dt = ExecuteQuery(StaticQuery.MostrarNomeIdEquipamento);
            Console.WriteLine("\nMENU DE ID'S E RESPECTIVOS ITENS:");
            foreach (DataRow item in dt.Rows)
            {
                Console.WriteLine("ID: "+item["Equipmento_ID"]+"\t------->\t"+item["Equipmento_Nome"]);
            }
            Console.Write("\nDigite o id do item que deseja adicionar no almoxarifado: ");
            int id = Convert.ToInt32(Console.ReadLine());

            return id;
        }

        /// <summary>
        /// FUNÇÃO PARA RECEBER SE O EQUIPAMENTO ESTÁ DISPONIVEL PELO USUÁRIO
        /// </summary>
        /// <returns></returns>
        public static int GetQuant()
        {
            Console.Write("Informe a quantidade desse item: ");
            int quant = Convert.ToInt32(Console.ReadLine());

            return quant;
        }

        /// <summary>
        /// FUNÇÃO PARA RECEBER A DATA DA MOVIMENTAÇÃO PELO USUÁRIO
        /// </summary>
        /// <returns>DATA DA MOVIMENTAÇÃO</returns>
        public static DateTime GetDate()
        {
            DateTime dt;
            Console.Write("Digite uma data no formato: DD/MM/YYYY: ");
            string strData = Convert.ToString(Console.ReadLine());
            dt = DateTime.Parse(strData);

            return dt;
        }

        /// <summary>
        /// FUNÇÃO PARA RECEBER O CÓDIGO DO TIPO DA MOVIMENTAÇÃO PARA O USUÁRIO
        /// </summary>
        /// <returns>CÓDIGO DO TIPO DA MOVIMENTAÇÃO</returns>
        public static char GetEstadoConservacao()
        {
            char opcao;
            Console.Write("O item está bem conservado?(S/N): ");
            opcao = Convert.ToChar(Console.ReadLine());

            return opcao;
        }

        /// <summary>
        /// RECEBER MES DO USUÁRIO
        /// </summary>
        /// <returns>MES</returns>
        public static int GetMes()
        {
            Console.Write("Digite o numero do mês desejado (1-12):");
            int mes = Convert.ToInt32(Console.ReadLine());

            return mes;
        }

        /// <summary>
        /// RECEBER ANO DO USUÁRIO
        /// </summary>
        /// <returns>ANO</returns>
        public static int GetAno()
        {
            Console.Write("Digite o ano desejado:");
            int ano = Convert.ToInt32(Console.ReadLine());

            return ano;
        }

        /// <summary>
        /// EXIBE MENU DE INICIALIZAÇÃO AO USUÁRIO
        /// </summary>
        public static void MenuInicial()
        {
            Console.WriteLine("  ___   _     ___  ___ _____ __   __  ___  ______  _____ ______   ___  ______  _____   _____  _____ \r\n / _ \\ | |    |  \\/  ||  _  |\\ \\ / / / _ \\ | ___ \\|_   _||  ___| / _ \\ |  _  \\|  _  | |_   _||_   _|\r\n/ /_\\ \\| |    | .  . || | | | \\ V / / /_\\ \\| |_/ /  | |  | |_   / /_\\ \\| | | || | | |   | |    | |  \r\n|  _  || |    | |\\/| || | | | /   \\ |  _  ||    /   | |  |  _|  |  _  || | | || | | |   | |    | |  \r\n| | | || |____| |  | |\\ \\_/ // /^\\ \\| | | || |\\ \\  _| |_ | |    | | | || |/ / \\ \\_/ /   | |   _| |_ \r\n\\_| |_/\\_____/\\_|  |_/ \\___/ \\/   \\/\\_| |_/\\_| \\_| \\___/ \\_|    \\_| |_/|___/   \\___/    \\_/   \\___/ ");//ALMOXARIFADO TI
            Console.WriteLine(" _____   ___   _____   _                                     \r\n/  ___| / _ \\ |  ___| | |                                    \r\n\\ `--. / /_\\ \\| |__   | |_   ___  __      __  ___  _ __  ___ \r\n `--. \\|  _  ||  __|  | __| / _ \\ \\ \\ /\\ / / / _ \\| '__|/ __|\r\n/\\__/ /| | | || |___  | |_ | (_) | \\ V  V / |  __/| |   \\__ \\\r\n\\____/ \\_| |_/\\____/   \\__| \\___/   \\_/\\_/   \\___||_|   |___/");
            //SAE TOWERS
            Console.WriteLine("\n\n");
            Console.WriteLine("CAREGANDO PORFAVOR AGUARDE...");
            Thread.Sleep(5000);
        }

        /// <summary>
        /// ADICIONA OS EQUIPAMENTOS DO BANCO A UMA LISTA
        /// </summary>
        /// <returns>LISTA RESULTANTE</returns>
        public static List<Equipmentos> ListaEquipamentos()
        {
            List<Equipmentos> equipamentos = new List<Equipmentos>();

            DataTable dt = ExecuteQuery(StaticQuery.ListarEquipamentos);
            foreach (DataRow item in dt.Rows)
            {
                Equipmentos equipamento = new Equipmentos();
                equipamento.Equipmento_ID = Convert.ToInt32(item["Equipmento_ID"]);
                equipamento.Equipmento_Nome = Convert.ToString(item["Equipmento_Nome"]);
                equipamento.Quantidade = Convert.ToInt32(item["Quantidade"]);
                equipamentos.Add(equipamento);
            }

            return equipamentos;
        }


        static void Main(string[] args)
        {
            int opcao = 10;
            List<Equipmentos> equipamentos = ListaEquipamentos();

            Console.BackgroundColor = ConsoleColor.White;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Blue;

            MenuInicial();

            try
            {
                while (opcao != 0)
                {
                    Console.ForegroundColor = ConsoleColor.Black;

                    ExibirMenu();

                    opcao = Convert.ToInt32(Console.ReadLine());
                    switch (opcao)
                    {

                        case 1: //LISTAR ITEM DO ESTOQUE
                            Console.Clear();

                            DataTable dt = ExecuteQuery(StaticQuery.OrderQuery);

                            if (dt!=null && dt.Rows.Count>0)
                            {
                                foreach (DataRow item in dt.Rows)
                                {
                                    Console.WriteLine("Id do Equipamento: "+item["Equipmento_ID"]+"\nNome do equipamento: "+item["Equipmento_Nome"]+"\nQuantidade em estoque:"+item["Quantidade"]);
                                    Console.WriteLine("\n\n");
                                }
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Sem items cadastrados");
                            }

                            Console.ReadKey();
                            break;


                        case 2: //CADASTRAR ITEM NO ESTOQUE

                            Console.Clear();
                            List<SqlParameter> parameters = new List<SqlParameter>
                            {
                                new SqlParameter("@Nome", GetNome()),
                                new SqlParameter("@Id", GetId()),
                                new SqlParameter("@Quantidade", GetQuant()),
                            };

                            ExecuteChanges(StaticQuery.CadastroItemQuery, parameters);

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("EQUIPAMENTO ADICIONADO");

                            Console.ReadKey();

                            break;


                        case 3: //EDITAR ITEM DO ESTOQUE

                            Console.Clear();

                            Console.Write("Digite o id do item que deseja editar: ");
                            int idItem = Convert.ToInt32(Console.ReadLine());

                            Console.WriteLine("Digite os novos valores que se pede.....\n");


                            List<SqlParameter> parametersForEdit = new List<SqlParameter>
                            {
                                new SqlParameter("@Nome", GetNome()),
                                new SqlParameter("@Id", idItem),
                                new SqlParameter("@NewId", GetId()),
                                new SqlParameter("@Quantidade", GetQuant()),
                            };


                            ExecuteChanges(StaticQuery.EditItemQuery, parametersForEdit);

                            Console.ForegroundColor= ConsoleColor.Green;
                            Console.WriteLine("ITEM EDITADO COM SUCESSO");

                            Console.ReadKey();
                            break;


                        case 4: //EXCLUIR ITEM DO ESTOQUE

                            Console.Clear();

                            Console.WriteLine("Digite o ID do equipamento que deseja deletar:");
                            int deleteId = GetId();

                            List<SqlParameter> parametersForDelete = new List<SqlParameter>
                            {
                                new SqlParameter("@Id", deleteId),
                            };

                            ExecuteChanges(StaticQuery.DeleteItemQuery, parametersForDelete);

                            Console.ForegroundColor= ConsoleColor.Red;
                            Console.WriteLine("ITEM EXCLUIDO COM SUCESSO");

                            Console.ReadKey();
                            break;


                        case 5: //CADASTRAR UM EMPRÉSTIMO

                            Console.Clear();
                            DataTable tabela = ExecuteQuery(StaticQuery.ListarEquipamentos);

                            int quantAdd = GetQuant();
                            int itemAddId = GetId();


                            foreach (DataRow item in tabela.Rows)
                            {
                                if (Convert.ToInt32(item["Quantidade"])>=quantAdd && Convert.ToInt32(item["Equipmento_ID"])==itemAddId)
                                {
                                    List<SqlParameter> parametersForEmprestimo = new List<SqlParameter>
                                    {
                                        new SqlParameter("@NomeMovimentador", GetNomeMovimentador()),
                                        new SqlParameter("@Id", itemAddId),
                                        new SqlParameter("@Quantidade", quantAdd),
                                        new SqlParameter("@Data", GetDate()),
                                        new SqlParameter("@Tipo",'E'),
                                        new SqlParameter("@Conservacao", 'S'),
                                    };  
                                    
                                    foreach (Equipmentos equipamento in equipamentos)
                                    {
                                        if (equipamento.Equipmento_ID==itemAddId)
                                        {
                                            int quantNova = equipamento.Quantidade - quantAdd;

                                            List<SqlParameter> parametersForRegistroEmprestimo = new List<SqlParameter>
                                            {
                                                new SqlParameter("@Id", itemAddId),
                                                new SqlParameter("@Quantidade", quantNova)
                                            };

                                            ExecuteChanges(StaticQuery.CadastroMovimentacaoQuery, parametersForEmprestimo);
                                            ExecuteChanges(StaticQuery.RegistrarQuantidadeMovimentada, parametersForRegistroEmprestimo);
                                        }
                                    }

                                    Console.ForegroundColor= ConsoleColor.Green;
                                    Console.WriteLine("Empréstimo cadastrado");
                                }
                                else if (quantAdd > Convert.ToInt32(item["Quantidade"]) && Convert.ToInt32(item["Equipmento_ID"])==itemAddId)
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("ITEM NÃO DISPONIVEL!!!!!");
                                }
                            }

                            Console.ReadKey();
                            break;


                        case 6: //CADASTRAR UM DEPÓSITO

                            Console.Clear();
                            char estadoConservacao = GetEstadoConservacao();
                            int quantAux = GetQuant();
                            int idAux = GetId();

                            if (estadoConservacao.Equals('S'))
                            {
                                List<SqlParameter> parametersForDepositoBom = new List<SqlParameter>
                                {
                                    new SqlParameter("@NomeMovimentador", GetNomeMovimentador()),
                                    new SqlParameter("@Id", idAux),
                                    new SqlParameter("@Quantidade", quantAux),
                                    new SqlParameter("@Data", GetDate()),
                                    new SqlParameter("@Tipo",'D'),
                                    new SqlParameter("@Conservacao",estadoConservacao),
                                };

                                foreach (Equipmentos equipamenta in equipamentos)
                                {
                                    if (equipamenta.Equipmento_ID == idAux)
                                    {
                                        int quantNew = equipamenta.Quantidade + quantAux;
                                        List<SqlParameter> parametersDeposito = new List<SqlParameter>
                                        {
                                            new SqlParameter("@Id", idAux),
                                            new SqlParameter("@Quantidade", quantNew)
                                        };
                                        ExecuteChanges(StaticQuery.CadastroMovimentacaoQuery, parametersForDepositoBom);
                                        ExecuteChanges(StaticQuery.RegistrarQuantidadeMovimentada, parametersDeposito);
                                    }
                                }

                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("Depósito realizado");
                            }
                            else if (estadoConservacao.Equals('N'))
                            {
                                List<SqlParameter> parametersForDepositoal = new List<SqlParameter>
                                {
                                    new SqlParameter("@NomeMovimentador", GetNomeMovimentador()),
                                    new SqlParameter("@Id", idAux),
                                    new SqlParameter("@Quantidade", quantAux),
                                    new SqlParameter("@Data", GetDate()),
                                    new SqlParameter("@Tipo",'D'),
                                    new SqlParameter("@Conservacao",estadoConservacao),
                                };

                                ExecuteChanges(StaticQuery.CadastroMovimentacaoQuery, parametersForDepositoal);
                            }
                            else 
                            {
                                Console.ForegroundColor= ConsoleColor.Red;
                                Console.WriteLine("OPÇÃO INVÁLIDA");
                            }
                            break;

                        case 7: //FILTRAR EMPRÉSTIMOS POR DATA

                            Console.Clear();
                            List<SqlParameter> parametersForFiltro = new List<SqlParameter>
                            {
                                new SqlParameter("@Mes", GetMes()),
                                new SqlParameter("@Ano", GetAno()),
                                new SqlParameter("@Tipo", 'E'),
                            };

                            DataTable dtable = ExecuteQuery(StaticQuery.FiltroEmprestimoQuery, parametersForFiltro);

                            if (dtable.Rows.Count>=1)
                            {
                                foreach (DataRow item in dtable.Rows)
                                {
                                    Console.WriteLine("ID equipmento:\t"+item["Equipamento_ID"]+"\n" +
                                        "Nome Movimentador:\t"+item["Nome_Movimentador"]+"\n" +
                                        "Quantidade Movida:\t"+item["Quantidade"]+"\n" +
                                        "Estado de conservação:\t"+item["Estado_conservacao"]+"\n" +
                                        "Data do Emprestimo:\t"+item["Data_movimentacao"]);

                                    Console.WriteLine("\n\n");
                                }
                            }
                            else
                            {
                                Console.ForegroundColor= ConsoleColor.Red;
                                Console.WriteLine("A pesquisa não retornou resultados");
                            }

                            Console.ReadKey();
                            break;


                        case 8: //FILTRAR DEPÓSITOS POR DATA

                            Console.Clear();
                            List<SqlParameter> parametersForFiltroDeposito = new List<SqlParameter>
                            {
                                new SqlParameter("@Mes", GetMes()),
                                new SqlParameter("@Ano", GetAno()),
                                new SqlParameter("@Tipo", 'D'),
                            };

                            DataTable dtableDeposito = ExecuteQuery(StaticQuery.FiltroDevolucaoQuery, parametersForFiltroDeposito);

                            if (dtableDeposito.Rows.Count>=1)
                            {
                                foreach (DataRow item in dtableDeposito.Rows)
                                {
                                    Console.WriteLine("ID equipmento:\t"+item["Equipamento_ID"]+"\n" +
                                        "Nome Movimentador:\t"+item["Nome_Movimentador"]+"\n" +
                                        "Quantidade Movida:\t"+item["Quantidade"]+"\n" +
                                        "Estado de conservação:\t"+item["Estado_conservacao"]+"\n" +
                                        "Data do Emprestimo:\t"+item["Data_movimentacao"]);

                                    Console.WriteLine("\n\n");
                                }
                            }
                            else
                            {
                                Console.ForegroundColor= ConsoleColor.Red;
                                Console.WriteLine("A pesquisa não retornou resultados");
                            }

                            Console.ReadKey();
                            break;


                        case 9: //PESQUISAR MOVIMENTAÇÕES NO NOME DE UMA PESSOA

                            Console.Clear();

                            List<SqlParameter> parametersForFiltroNome = new List<SqlParameter>
                            {
                                new SqlParameter("@Nome", GetNomeMovimentador()),
                            };


                            DataTable dtableNome = ExecuteQuery(StaticQuery.FiltroMovimentacaoNome, parametersForFiltroNome);


                            if (dtableNome.Rows.Count>=1)
                            {
                                foreach (DataRow item in dtableNome.Rows)
                                {
                                    Console.WriteLine("ID equipmento:\t"+item["Equipamento_ID"]+"\n" +
                                        "Nome Movimentador:\t"+item["Nome_Movimentador"]+"\n" +
                                        "Quantidade Movida:\t"+item["Quantidade"]+"\n" +
                                        "Tipo de movimentação:\t"+item["Tipo_Movimentacao"]+"\n" +
                                        "Estado de conservação:\t"+item["Estado_conservacao"]+"\n" +
                                        "Data do Emprestimo:\t"+item["Data_movimentacao"]);
                                    Console.WriteLine("\n\n");
                                }
                            }
                            else
                            {
                                Console.ForegroundColor= ConsoleColor.Red;
                                Console.WriteLine("A pesquisa não retornou resultados");
                            }

                            Console.ReadKey();
                            break;
                    }
                }
            }
            catch (Exception goku)
            {
                Console.WriteLine(goku.Message);
                Console.ReadKey();
            }
            finally
            {
                Console.WriteLine("FIM");
                Console.ReadKey();
            }
        }
    }
}
