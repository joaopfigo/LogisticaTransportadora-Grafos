using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trabalho_Grafos
{
    internal static class LeitorGrafoDimacs
    {
        public static Grafo Ler(string caminhoArquivo)
        {
            string linha;
            StreamReader arq = new StreamReader(caminhoArquivo, Encoding.UTF8);
            linha = arq.ReadLine();
            string[] partes = linha.Split(' ');
            int nVertices = int.Parse(partes[0]);
            int nArestas = int.Parse(partes[1]);
            List<Aresta> arestas = new List<Aresta>();
            linha = arq.ReadLine();
            while (linha != null)
            {
                partes = linha.Split(' ');
                int origem = int.Parse(partes[0]);
                int destino = int.Parse(partes[1]);
                double peso = double.Parse(partes[2]);
                double capacidade = double.Parse(partes[3]);
                arestas.Add(new Aresta(origem, destino, peso, capacidade));
                linha = arq.ReadLine();
            }
            arq.Close(); 
            Grafo grafo = new Grafo(nVertices, nArestas, arestas);
            return grafo;
        }
    }
}
