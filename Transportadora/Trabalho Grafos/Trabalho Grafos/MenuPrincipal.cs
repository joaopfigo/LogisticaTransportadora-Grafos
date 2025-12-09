using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trabalho_Grafos
{
    internal class MenuPrincipal
    {
        private Grafo grafo;
        private AnalisesLogisticas analises = new AnalisesLogisticas();
        bool ativo;
        public MenuPrincipal(Grafo grafo)
        {
            this.grafo = grafo;
            ativo = true;
        }
        public void ExibirMenu()
        {
            while (ativo)
            {
                Console.WriteLine();
                Console.WriteLine("Menu de Análises Logísticas:");
                Console.WriteLine("1. Roteamento de Menor Custo");
                Console.WriteLine("2. Capacidade Máxima de Escoamento");
                Console.WriteLine("3. Expansão da Rede de Comunicação");
                Console.WriteLine("4. Agendamento de Manutenções sem Conflito");
                Console.WriteLine("5. Rota Única de Inspeção");
                Console.WriteLine("0. Sair");
                Console.Write("Selecione uma opção: ");
                string opcao = Console.ReadLine();

                switch (opcao)
                {
                    case "1":
                        Console.Write("Origem: ");
                        int origem = int.Parse(Console.ReadLine());

                        Console.Write("Destino: ");
                        int destino = int.Parse(Console.ReadLine());

                        analises.RoteamentoMenorCusto(grafo, origem, destino);
                        break;
                    case "2":
                        Console.Write("Hub central: ");
                        int s = int.Parse(Console.ReadLine());
                        Console.Write("Terminal de destino: ");
                        int t = int.Parse(Console.ReadLine());
                        analises.CapacidadeMaximaEscoamento(grafo, s, t);
                        break;
                    case "3":
                        analises.ExpansaoRedeComunicacao(grafo);
                        break;
                    case "4":
                        analises.AgendamentoManutencoes(grafo);
                        break;
                    case "5":
                        analises.RotaInspecao(grafo);
                        analises.RotaInspecaoHubs(grafo);
                        break;
                    case "0":
                        ativo = false;
                        break;
                    default:
                        Console.WriteLine("Opção inválida. Tente novamente.");
                        break;
                }
            }
        }
    }
}