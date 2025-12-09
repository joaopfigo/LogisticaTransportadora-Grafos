using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trabalho_Grafos
{
    internal class RepresentacaoMatrizAdjacencia : IRepresentacaoGrafo
    {
        private int numeroVertices;
        private int numeroArestas;
        private Aresta[,] matrizAdjacencia;
        public RepresentacaoMatrizAdjacencia(int nVertices) 
        {
            this.numeroVertices = nVertices;
            this.matrizAdjacencia = new Aresta [nVertices, nVertices];
            this.numeroArestas = 0;
        }
        public void AdicionarAresta(Aresta a)
        {
            if (matrizAdjacencia[a.getOrigem()-1, a.getDestino()-1] == null)
            {
                numeroArestas++;
            }
            matrizAdjacencia[a.getOrigem()-1, a.getDestino()-1] = a;
        }
        public List<Aresta> ObterVizinhos(int vertice)
        {
            List<Aresta> vizinhos = new List<Aresta>();
            for (int j = 0; j < numeroVertices; j++)
            {
                if (matrizAdjacencia[vertice-1, j] != null)
                {
                    vizinhos.Add(matrizAdjacencia[vertice-1,j]);
                }
            }
            return vizinhos;
        }
    }
}
