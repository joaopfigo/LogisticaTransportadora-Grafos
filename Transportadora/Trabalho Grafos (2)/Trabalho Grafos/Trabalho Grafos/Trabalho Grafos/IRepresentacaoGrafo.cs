using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trabalho_Grafos
{
    internal interface IRepresentacaoGrafo
    {
        void AdicionarAresta(Aresta a);
        List<Aresta> ObterVizinhos(int vertice);
    }
}
