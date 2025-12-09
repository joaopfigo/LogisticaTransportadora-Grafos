using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trabalho_Grafos
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool rodando = true;
            while (rodando)
            {
                Console.WriteLine("Qual o nome do arquivo contendo o grafo DIMACS?");
                string nomeArquivo = Console.ReadLine();
                Grafo grafo = LeitorGrafoDimacs.Ler(nomeArquivo);
                MenuPrincipal menu = new MenuPrincipal(grafo);
                menu.ExibirMenu();
                Console.WriteLine("Deseja carregar outro grafo? (s/n)");
                char resposta = char.Parse(Console.ReadLine());
                rodando = (resposta == 's' || resposta == 'S');
            }
        }
    }
}
