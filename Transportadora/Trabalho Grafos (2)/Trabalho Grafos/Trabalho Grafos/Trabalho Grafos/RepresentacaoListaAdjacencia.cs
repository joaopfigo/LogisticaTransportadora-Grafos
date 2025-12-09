using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trabalho_Grafos
{
    internal class RepresentacaoListaAdjacencia : IRepresentacaoGrafo
    {
        private int numeroArestas;
        private int numeroVertices;
        private List<Aresta>[] listaAdjacencia;
        public RepresentacaoListaAdjacencia(int nVertices)
        {
            this.numeroVertices = nVertices;
            this.listaAdjacencia = new List<Aresta>[nVertices];
            for (int i = 0; i < nVertices; i++)
            {
                this.listaAdjacencia[i] = new List<Aresta>();
            }
            this.numeroArestas = 0;
        }
        public void AdicionarAresta(Aresta a)
        {
            listaAdjacencia[a.getOrigem()-1].Add(a);
            numeroArestas++;
        }
        public List<Aresta> ObterVizinhos(int vertice)
        {
            return listaAdjacencia[vertice-1];
        }
    }
}
