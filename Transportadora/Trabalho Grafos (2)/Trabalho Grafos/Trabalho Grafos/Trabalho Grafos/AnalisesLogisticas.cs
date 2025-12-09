using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trabalho_Grafos
{
    internal class AnalisesLogisticas
    {
        public void RoteamentoMenorCusto(Grafo grafo, int origem, int destino)
        {
            int origemIndex = origem - 1;
            int destinoIndex = destino - 1;
            int nVertices = grafo.getNVertices();
            int[] predecessores = new int[nVertices];
            double[] distancias = new double[nVertices];
            bool[] visitados = new bool[nVertices];
            for (int i = 0; i < distancias.Length; i++)
            {
                distancias[i] = double.MaxValue;
                predecessores[i] = -1;
                visitados[i] = false;
            }
            distancias[origemIndex] = 0;
            visitados[origemIndex] = true;
            int visitadoCount = 1;
            while (visitadoCount < nVertices)
            {
                int arestaMenorPeso = -1;
                double distanciaMinima = double.MaxValue;
                int predecessorNovo = -1;
                for (int i = 0; i < visitados.Length; i++)
                {
                    if (visitados[i])
                    {
                        List<Aresta> vizinhos = grafo.ObterVizinhos(i + 1);
                        for (int j = 0; j < vizinhos.Count; j++)
                        {
                            int indexVizinho = vizinhos[j].getDestino() - 1;
                            if (!visitados[indexVizinho])
                            {
                                double distanciaAtual = vizinhos[j].getPeso() + distancias[i];
                                if (distanciaAtual < distanciaMinima)
                                {
                                    distanciaMinima = distanciaAtual;
                                    arestaMenorPeso = indexVizinho;
                                    predecessorNovo = i;
                                }
                            }
                        }
                    }
                }
                if (arestaMenorPeso == -1)
                {
                    visitadoCount = nVertices;
                }
                else
                {
                    distancias[arestaMenorPeso] = distanciaMinima;
                    visitados[arestaMenorPeso] = true;
                    predecessores[arestaMenorPeso] = predecessorNovo;
                    visitadoCount++;
                }
            }
            if (distancias[destinoIndex] == double.MaxValue)
            {
                Console.WriteLine("Não existe caminho de " + origem + " até " + destino + ".");
            }
            else
            {
                List<int> caminho = new List<int>();
                int atual = destinoIndex;
                while (atual != -1)
                {
                    caminho.Add(atual + 1);
                    atual = predecessores[atual];
                }
                caminho.Reverse();
                Console.WriteLine();
                Console.WriteLine("Distância mínima de " + origem + " até " + destino + " é: " + distancias[destinoIndex]);
                Console.WriteLine("Caminho: ");
                for (int i = 0; i < caminho.Count; i++)
                {
                    Console.Write(caminho[i]);
                    if (i < caminho.Count - 1)
                    {
                        Console.Write(" -> ");
                    }
                }
                Console.WriteLine();
            }
        }
        public void CapacidadeMaximaEscoamento(Grafo grafo, int origem, int destino)
        {
            List<Aresta> arestasResidual = new List<Aresta>();
            foreach (Aresta a in grafo.getArestas())
            {
                arestasResidual.Add(new Aresta(a.getOrigem(), a.getDestino(), a.getPeso(), a.getCapacidade()));
                arestasResidual.Add(new Aresta(a.getDestino(), a.getOrigem(), a.getPeso(), 0));
            }
            Grafo residual = new Grafo(grafo.getNVertices(), arestasResidual.Count, arestasResidual);
            int n = residual.getNVertices();
            int origemIndex = origem - 1;
            int destinoIndex = destino - 1;
            int[] pais = new int[n];
            Aresta[] arestaPai = new Aresta[n];
            double maxFlow = 0.0;
            while (BFSResidual(residual, origem, destino, pais, arestaPai))
            {
                double delta = double.MaxValue;
                int vIndex = destinoIndex;
                while (vIndex != origemIndex)
                {
                    Aresta aresta = arestaPai[vIndex];
                    double cap = aresta.getCapacidade();
                    if (cap < delta)
                    {
                        delta = cap;
                    }
                    vIndex = pais[vIndex];
                }
                vIndex = destinoIndex;
                while (vIndex != origemIndex)
                {
                    int uIndex = pais[vIndex];
                    int uVertice = uIndex + 1;
                    int vVertice = vIndex + 1;

                    Aresta direta = arestaPai[vIndex];
                    List<Aresta> vizinhosDeV = residual.ObterVizinhos(vVertice);
                    Aresta reversa = null;
                    foreach (Aresta e in vizinhosDeV)
                    {
                        if (e.getDestino() == uVertice)
                        {
                            reversa = e;
                            break;
                        }
                    }
                    direta.setCapacidade(direta.getCapacidade() - delta);
                    if (reversa != null)
                    {
                        reversa.setCapacidade(reversa.getCapacidade() + delta);
                    }

                    vIndex = uIndex;
                }
                maxFlow += delta;
            }

            Console.WriteLine();
            Console.WriteLine("Fluxo máximo de " + origem + " até " + destino + " = " + maxFlow);
            bool[] emSIndex = new bool[n];
            MarcarAlcancaveisNoResidual(residual, origem, emSIndex);
            List<Aresta> corteMinimo = new List<Aresta>();
            double capacidadeCorte = 0.0;

            foreach (Aresta a in grafo.getArestas())
            {
                int uIndex = a.getOrigem() - 1;
                int vIndex = a.getDestino() - 1;

                if (emSIndex[uIndex] && !emSIndex[vIndex])
                {
                    corteMinimo.Add(a);
                    capacidadeCorte += a.getCapacidade();
                }
            }

            Console.WriteLine();
            Console.WriteLine("Corte mínimo (arestas de S para T):");

            if (corteMinimo.Count == 0)
            {
                Console.WriteLine("Nenhuma aresta cruza o corte.");
            }
            else
            {
                foreach (Aresta a in corteMinimo)
                {
                    Console.WriteLine(a.getOrigem() + " -> " + a.getDestino() +
                                      " (capacidade " + a.getCapacidade() + ")");
                }
                Console.WriteLine("Capacidade do corte mínimo = " + capacidadeCorte);
            }

        }
        private bool BFSResidual(Grafo residual, int origem, int destino, int[] pais, Aresta[] arestaPai)
        {
            int n = residual.getNVertices();
            for (int i = 0; i < n; i++)
            {
                pais[i] = -1;
                arestaPai[i] = null;
            }
            int origemIndex = origem - 1;
            int destinoIndex = destino - 1;
            Queue<int> fila = new Queue<int>();
            pais[origemIndex] = origemIndex;
            fila.Enqueue(origemIndex);
            while (fila.Count > 0)
            {
                int uIndex = fila.Dequeue();
                int uVertice = uIndex + 1;
                List<Aresta> vizinhos = residual.ObterVizinhos(uVertice);
                foreach (Aresta e in vizinhos)
                {
                    int v = e.getDestino();
                    int vIndex = v - 1;
                    if (pais[vIndex] == -1 && e.getCapacidade() > 0)
                    {
                        pais[vIndex] = uIndex;
                        arestaPai[vIndex] = e;
                        if (vIndex == destinoIndex)
                        {
                            return true;
                        }
                        fila.Enqueue(vIndex);
                    }
                }
            }
            return false;
        }
        private void MarcarAlcancaveisNoResidual(Grafo residual, int origem, bool[] emSIndex)
        {
            int n = residual.getNVertices();
            for (int i = 0; i < n; i++)
            {
                emSIndex[i] = false;
            }

            int origemIndex = origem - 1;
            Queue<int> fila = new Queue<int>();

            emSIndex[origemIndex] = true;
            fila.Enqueue(origemIndex);

            while (fila.Count > 0)
            {
                int uIndex = fila.Dequeue();
                int uVertice = uIndex + 1;

                List<Aresta> vizinhos = residual.ObterVizinhos(uVertice);
                foreach (Aresta e in vizinhos)
                {
                    if (e.getCapacidade() > 0)
                    {
                        int v = e.getDestino();
                        int vIndex = v - 1;

                        if (!emSIndex[vIndex])
                        {
                            emSIndex[vIndex] = true;
                            fila.Enqueue(vIndex);
                        }
                    }
                }
            }
        }


        public void ExpansaoRedeComunicacao(Grafo grafo)
        {
            List<Aresta> arestasOrdenadas = grafo.getArestas().OrderBy(a => a.getPeso()).ToList();
            int n = grafo.getNVertices();
            List<Aresta> agm = new List<Aresta>();
            agm.Add(arestasOrdenadas[0]);
            int j = 1;
            while (agm.Count < n - 1)
            {
                Aresta e = arestasOrdenadas[j];
                int v = e.getOrigem();
                int w = e.getDestino();
                if (!ExisteCaminhoEntre(v, w, agm, n))
                {
                    agm.Add(e);
                }
                j++;
            }
            double custoTotal = 0;
            Console.WriteLine("Caminho de menor custo:");
            foreach (Aresta a in agm)
            {
                Console.WriteLine(a.getOrigem() + " -> " + a.getDestino() + " (custo " + a.getPeso() + ")");
                custoTotal += a.getPeso();
            }
            Console.WriteLine("Custo total da expansão = " + custoTotal);
        }
        private bool ExisteCaminhoEntre(int origem, int destino, List<Aresta> mst, int n)
        {
            bool[] visitado = new bool[n + 1];
            Queue<int> fila = new Queue<int>();
            visitado[origem] = true;
            fila.Enqueue(origem);
            while (fila.Count > 0)
            {
                int u = fila.Dequeue();
                if (u == destino)
                {
                    return true;
                }
                foreach (Aresta a in mst)
                {
                    int x = a.getOrigem();
                    int y = a.getDestino();
                    if (x == u && !visitado[y])
                    {
                        visitado[y] = true;
                        fila.Enqueue(y);
                    }
                    else if (y == u && !visitado[x])
                    {
                        visitado[x] = true;
                        fila.Enqueue(x);
                    }
                }
            }
            return false;
        }
        public void AgendamentoManutencoes(Grafo grafo)
        {
            List<Aresta> manutencoes = grafo.getArestas();
            int m = manutencoes.Count;
            if (m == 0)
            {
                Console.WriteLine("Não há arestas, logo não há manutenções para agendar.");
                return;
            }
            List<Aresta> arestasConflitos = new List<Aresta>();
            for (int i = 0; i < m; i++)
            {
                int u1 = manutencoes[i].getOrigem();
                int v1 = manutencoes[i].getDestino();

                for (int j = i + 1; j < m; j++)
                {
                    int u2 = manutencoes[j].getOrigem();
                    int v2 = manutencoes[j].getDestino();
                    bool compartilhamVertice =
                        u1 == u2 || u1 == v2 ||
                        v1 == u2 || v1 == v2;
                    if (compartilhamVertice)
                    {
                        int a = i + 1;
                        int b = j + 1;
                        arestasConflitos.Add(new Aresta(a, b, 1.0, 1.0));
                        arestasConflitos.Add(new Aresta(b, a, 1.0, 1.0));
                    }
                }
            }
            Grafo grafoConflitos = new Grafo(m, arestasConflitos.Count, arestasConflitos);
            int[] grau = new int[m + 1];
            for (int v = 1; v <= m; v++)
            {
                grau[v] = grafoConflitos.ObterVizinhos(v).Count;
            }
            List<int> ordem = Enumerable.Range(1, m).ToList();
            ordem.Sort((a, b) => grau[b].CompareTo(grau[a]));
            int[] cor = new int[m + 1];
            for (int i = 1; i <= m; i++)
            {
                cor[i] = -1;
            }
            int corAtual = 0;
            int coloridos = 0;
            while (coloridos < m)
            {
                int primeiro = -1;
                foreach (int v in ordem)
                {
                    if (cor[v] == -1)
                    {
                        primeiro = v;
                        break;
                    }
                }
                if (primeiro == -1)
                {
                    break;
                }
                cor[primeiro] = corAtual;
                coloridos++;
                foreach (int v in ordem)
                {
                    if (cor[v] != -1) continue;
                    bool conflita = false;
                    List<Aresta> vizinhos = grafoConflitos.ObterVizinhos(v);
                    foreach (Aresta e in vizinhos)
                    {
                        int w = e.getDestino();
                        if (cor[w] == corAtual)
                        {
                            conflita = true;
                            break;
                        }
                    }

                    if (!conflita)
                    {
                        cor[v] = corAtual;
                        coloridos++;
                    }
                }
                corAtual++;
            }

            int numCores = corAtual;
            Console.WriteLine();
            Console.WriteLine("Agendamento de Manutenções sem Conflito:");
            Console.WriteLine("(Cada turno pode ser executado em paralelo, sem compartilhar hubs.)");
            Console.WriteLine();

            for (int c = 0; c < numCores; c++)
            {
                Console.WriteLine("Turno " + (c + 1) + ":");

                bool temAlgoNoTurno = false;

                for (int v = 1; v <= m; v++)
                {
                    if (cor[v] == c)
                    {
                        Aresta a = manutencoes[v - 1];
                        Console.WriteLine("  Manutenção na rota " +
                                          a.getOrigem() + " -> " + a.getDestino());
                        temAlgoNoTurno = true;
                    }
                }

                if (!temAlgoNoTurno)
                {
                    Console.WriteLine("  (nenhuma manutenção)");
                }

                Console.WriteLine();
            }
        }
        public void RotaInspecao(Grafo grafo)
        {
            int n = grafo.getNVertices();
            List<Aresta> arestas = grafo.getArestas();

            if (arestas.Count == 0)
            {
                Console.WriteLine("Grafo não possui arestas para inspeção.");
                return;
            }

            int[] inDegree = new int[n + 1];
            int[] outDegree = new int[n + 1];

            foreach (Aresta a in arestas)
            {
                int u = a.getOrigem();
                int v = a.getDestino();
                if (u < 1 || u > n || v < 1 || v > n)
                {
                    Console.WriteLine("Aresta inválida encontrada no grafo.");
                    return;
                }
                outDegree[u]++;
                inDegree[v]++;
            }

            List<int>[] adjUnd = new List<int>[n + 1];
            for (int i = 1; i <= n; i++)
            {
                adjUnd[i] = new List<int>();
            }
            foreach (Aresta a in arestas)
            {
                int u = a.getOrigem();
                int v = a.getDestino();
                adjUnd[u].Add(v);
                adjUnd[v].Add(u);
            }

            int startConnectivity = -1;
            for (int i = 1; i <= n; i++)
            {
                if (inDegree[i] + outDegree[i] > 0)
                {
                    startConnectivity = i;
                    break;
                }
            }

            if (startConnectivity == -1)
            {
                Console.WriteLine("Grafo não possui arestas para inspeção.");
                return;
            }

            bool[] visitado = new bool[n + 1];
            Queue<int> fila = new Queue<int>();
            visitado[startConnectivity] = true;
            fila.Enqueue(startConnectivity);

            while (fila.Count > 0)
            {
                int u = fila.Dequeue();
                foreach (int v in adjUnd[u])
                {
                    if (!visitado[v])
                    {
                        visitado[v] = true;
                        fila.Enqueue(v);
                    }
                }
            }

            for (int i = 1; i <= n; i++)
            {
                if (inDegree[i] + outDegree[i] > 0 && !visitado[i])
                {
                    Console.WriteLine("Não existe rota única de inspeção (grafo desconexo em relação às arestas).");
                    return;
                }
            }

            int startVertex = -1;
            int startCount = 0;
            int endCount = 0;
            bool ok = true;

            for (int i = 1; i <= n; i++)
            {
                int diff = outDegree[i] - inDegree[i];
                if (diff == 1)
                {
                    startVertex = i;
                    startCount++;
                }
                else if (diff == -1)
                {
                    endCount++;
                }
                else if (diff != 0)
                {
                    ok = false;
                    break;
                }
            }

            if (!ok || !((startCount == 1 && endCount == 1) || (startCount == 0 && endCount == 0)))
            {
                Console.WriteLine("Não existe rota única de inspeção (condições de Euler não satisfeitas).");
                return;
            }

            if (startVertex == -1)
            {
                for (int i = 1; i <= n; i++)
                {
                    if (outDegree[i] > 0)
                    {
                        startVertex = i;
                        break;
                    }
                }
            }

            List<int>[] adj = new List<int>[n + 1];
            for (int i = 1; i <= n; i++)
            {
                adj[i] = new List<int>();
            }

            foreach (Aresta a in arestas)
            {
                adj[a.getOrigem()].Add(a.getDestino());
            }

            Stack<int> pilha = new Stack<int>();
            List<int> caminho = new List<int>();
            pilha.Push(startVertex);

            while (pilha.Count > 0)
            {
                int v = pilha.Peek();
                if (adj[v].Count > 0)
                {
                    int u = adj[v][adj[v].Count - 1];
                    adj[v].RemoveAt(adj[v].Count - 1);
                    pilha.Push(u);
                }
                else
                {
                    caminho.Add(pilha.Pop());
                }
            }

            caminho.Reverse();

            if (caminho.Count != arestas.Count + 1)
            {
                Console.WriteLine("Não foi possível construir uma rota de inspeção que percorra todas as arestas exatamente uma vez.");
                return;
            }

            Console.WriteLine("Rota única de inspeção:");
            for (int i = 0; i < caminho.Count; i++)
            {
                Console.Write(caminho[i]);
                if (i < caminho.Count - 1)
                {
                    Console.Write(" -> ");
                }
            }
            Console.WriteLine();
        }
        public void RotaInspecaoHubs(Grafo grafo)
        {
            int n = grafo.getNVertices();
            if (n == 0)
            {
                Console.WriteLine("Não há hubs para inspeção.");
                return;
            }

            bool[] visitado = new bool[n + 1];
            int[] caminho = new int[n];

            int start = 1;
            caminho[0] = start;
            visitado[start] = true;

            bool existe = BuscaHamiltoniana(grafo, 1, n, start, visitado, caminho);

            if (!existe)
            {
                Console.WriteLine("Não existe ciclo hamiltoniano de inspeção de hubs.");
                return;
            }

            Console.WriteLine("Cenário B – Percurso de Hubs:");
            for (int i = 0; i < n; i++)
            {
                Console.Write(caminho[i]);
                if (i < n - 1)
                {
                    Console.Write(" -> ");
                }
            }
            Console.Write(" -> ");
            Console.WriteLine(caminho[0]);
        }

        private bool BuscaHamiltoniana(Grafo grafo, int pos, int n, int start, bool[] visitado, int[] caminho)
        {
            if (pos == n)
            {
                int ultimo = caminho[n - 1];
                List<Aresta> vizinhos = grafo.ObterVizinhos(ultimo);
                foreach (Aresta a in vizinhos)
                {
                    if (a.getDestino() == start)
                    {
                        return true;
                    }
                }
                return false;
            }

            int ultimoVertice = caminho[pos - 1];
            List<Aresta> adj = grafo.ObterVizinhos(ultimoVertice);

            foreach (Aresta e in adj)
            {
                int v = e.getDestino();
                if (v < 1 || v > n) continue;
                if (!visitado[v])
                {
                    visitado[v] = true;
                    caminho[pos] = v;
                    if (BuscaHamiltoniana(grafo, pos + 1, n, start, visitado, caminho))
                    {
                        return true;
                    }
                    visitado[v] = false;
                }
            }

            return false;
        }


    }
}