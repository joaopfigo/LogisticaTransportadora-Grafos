using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trabalho_Grafos
{
    internal class Grafo
    {
        private int nVertices;
        private int nArestas;
        private IRepresentacaoGrafo representacaoGrafo;
        private List<Aresta> arestas;
        public Grafo(int nVertices, int nArestas,List<Aresta> arestas)
        {
            this.nVertices = nVertices;
            this.nArestas = nArestas;
            this.arestas = arestas;
            double densidade = (double)nArestas / (nVertices * (nVertices - 1));
            if (densidade < 0.5)
            {
                representacaoGrafo = new RepresentacaoListaAdjacencia(nVertices);
            }
            else
            {
                representacaoGrafo = new RepresentacaoMatrizAdjacencia(nVertices);
            }
            foreach (Aresta a in arestas)
            {
                representacaoGrafo.AdicionarAresta(a);
            }
        }
        public List<Aresta> ObterVizinhos(int vertice)
        {
            return representacaoGrafo.ObterVizinhos(vertice);
        }
        public int getNVertices()
        {
            return nVertices;
        }
        public List<Aresta> getArestas()
        {
            return arestas;
        }
    }
}
