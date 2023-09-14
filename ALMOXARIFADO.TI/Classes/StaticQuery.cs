using ALMOXARIFADO.TI.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema_Almoxarifado_TI
{
    public static class StaticQuery
    {
        public const string OrderQuery = @"SELECT * 
                                           FROM Equipmentos 
                                            ORDER BY 
                                                Equipmento_ID;";


        public const string MostrarNomeIdEquipamento = @"SELECT Equipmento_ID,Equipmento_Nome
                                                         FROM Equipmentos";


        public const string ListarEquipamentos = @"SELECT * 
                                                   FROM Equipmentos";


        public const string CadastroItemQuery = @"INSERT INTO Equipmentos (Equipmento_ID, Equipmento_Nome, Quantidade) 
                                                    VALUES (@Id, @Nome, @Quantidade)";


        // EDITA UM ELEMENTO DA TABELA ITEM
        public const string EditItemQuery = @"UPDATE Equipmentos
                                            SET Equipmento_ID = @NewId, 
                                                Equipmento_Nome = @Nome,
                                                Quantidade = @Quantidade
                                            WHERE Equipmento_ID = @Id;";


        // DELETA ELEMENTO TABELA ITEM
        public const string DeleteItemQuery = @"DELETE FROM Equipmentos
                                              WHERE Equipmento_ID = @Id;
                                              SELECT * FROM Equipmentos";


        //CADASTRA/ADICIONA UM MOVIMENTO DO TIPO EMPRÉSTIMO
        public const string CadastroMovimentacaoQuery = @"INSERT INTO Movimentacao(Equipamento_ID,Nome_Movimentador,Tipo_Movimentacao,Quantidade,Estado_Conservacao,Data_movimentacao)
                                                      VALUES(@Id,@NomeMovimentador,@Tipo,@Quantidade,@Conservacao,@Data)";


        //REGISTRA A NOVA QUANTIDADE DE ITEMS DEPOIS DA MOVIMENTAÇÃO
        public const string RegistrarQuantidadeMovimentada = @"UPDATE Equipmentos
                                                              SET Equipmentos.Quantidade = @Quantidade
                                                              WHERE Equipmentos.Equipmento_ID = @Id";


        //BUSCA MOVIMENTOS DO TIPO EMPRÉSTIMO
        public const string FiltroEmprestimoQuery = @" SELECT Equipamento_ID,
                                                         Nome_Movimentador,
                                                         Quantidade,
                                                         CASE
                                                             WHEN Estado_Conservacao = 'S' THEN 'Boa conservação'
                                                             WHEN Estado_Conservacao = 'N' THEN 'Má conservação'
                                                         END AS Estado_Conservacao,
                                                         Data_movimentacao
                                                     FROM Movimentacao
                                                     WHERE
                                                        MONTH(Data_movimentacao)=@Mes AND
                                                        YEAR(Data_movimentacao)=@Ano AND
                                                        Tipo_movimentacao = @Tipo";


        //BUSCA MOVIMENTOS DO TIPO DEVOLUÇÃO
        public const string FiltroDevolucaoQuery = @" SELECT Equipamento_ID,
                                                         Nome_Movimentador,
                                                         Quantidade,
                                                         CASE
                                                             WHEN Estado_Conservacao = 'S' THEN 'Boa conservação'
                                                             WHEN Estado_Conservacao = 'N' THEN 'Má conservação'
                                                         END AS Estado_Conservacao,
                                                         Data_movimentacao
                                                     FROM Movimentacao
                                                     WHERE
                                                        MONTH(Data_movimentacao)=@Mes AND
                                                        YEAR(Data_movimentacao)=@Ano AND
                                                        Tipo_movimentacao = @Tipo";

        //BUSCA MOVIMENTOS POR NOME ESPECIFICO
        public const string FiltroMovimentacaoNome = @" SELECT Equipamento_ID,
                                                         Nome_Movimentador,
                                                         Quantidade,
                                                         CASE
                                                             WHEN Tipo_Movimentacao = 'D' THEN 'Devolução'
                                                             WHEN Tipo_Movimentacao = 'E' THEN 'Empréstimo'
                                                         END AS Tipo_Movimentacao,
                                                         CASE
                                                             WHEN Estado_Conservacao = 'S' THEN 'Boa conservação'
                                                             WHEN Estado_Conservacao = 'N' THEN 'Má conservação'
                                                         END AS Estado_Conservacao,
                                                         Data_movimentacao
                                                     FROM Movimentacao
                                                     WHERE
                                                        Nome_Movimentador = @Nome
                                                     ORDER BY 
                                                        Data_movimentacao;";
    }
}
