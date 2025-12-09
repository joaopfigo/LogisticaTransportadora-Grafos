# LogisticaTransportadora-Grafos

## Modelagem da rede

* A rede logística é modelada como um **grafo dirigido e ponderado**.
* **Vértices (hubs)** = cidades / centros.
* **Arestas (rotas)** = estradas entre hubs, com:

  * **peso** = custo da rota (distância, tempo, etc.);
  * **capacidade** = quanto dá pra escoar por ali.

A classe `Grafo` guarda:

* número de vértices e arestas;
* lista de arestas;
* uma representação interna que é escolhida automaticamente:

  * **lista de adjacência** quando o grafo é mais esparso;
  * **matriz de adjacência** quando é mais denso.

Isso é pra equilibrar desempenho e memória dependendo do grafo.

---

## Funcionalidades e algoritmos

### 1. Roteamento de Menor Custo

* Problema: dado origem e destino, achar o **caminho de menor custo** (peso).
* Algoritmo: versão de **Dijkstra**.
* Por quê?

  * Não tem pesos negativos.
  * Preciso de caminho mínimo origem→destino, não todos os pares.
* Interpretação logística:

  * Sugere a rota mais barata entre dois hubs, útil pra planejamento de trajeto de caminhões/viagens.

---

### 2. Capacidade Máxima de Escoamento + Corte Mínimo

* Problema: quanto consigo enviar de um hub origem até um hub destino, respeitando as **capacidades** das rotas, e quais arestas são o gargalo.
* Algoritmo: **Edmonds–Karp** (Ford–Fulkerson com BFS).
* O que o código faz:

  * Cria um **grafo residual** com arestas diretas + reversas.
  * Vai achando caminhos aumentantes com BFS e atualizando capacidades residuais.
  * No fim:

    * calcula o **fluxo máximo**;
    * faz um último BFS no residual pra achar o conjunto S (ainda alcançável) e monta o **corte mínimo** (arestas de S para T no grafo original).
* Interpretação logística:

  * Fluxo máximo = limite de escoamento entre dois hubs.
  * Corte mínimo = conjunto de rotas críticas; se elas “caírem”, o escoamento entre origem e destino quebra.

---

### 3. Expansão da Rede de Comunicação (MST)

* Problema: conectar todos os hubs com **custo total mínimo**, sem rotas redundantes.
* Algoritmo: **Kruskal** (Árvore Geradora Mínima).
* Decisão:

  * Para essa análise, as arestas são vistas como **ligações sem direção** entre hubs (interessa ter o link, não o sentido).
  * Ordeno as arestas por peso e vou adicionando apenas as que **não formam ciclo**, até ter `n-1` arestas.
* Interpretação logística:

  * Representa a “espinha dorsal” mais barata da rede.
  * Mostra quais rotas são essenciais para manter todo mundo conectado, e quais são mais “luxo / redundância”.

---

### 4. Agendamento de Manutenções sem Conflito

* Problema: agendar manutenções nas rotas sem que duas manutenções do mesmo turno usem um **mesmo hub**.
* Modelagem:

  * Cada **aresta do grafo original** vira uma **manutenção** (vértice de um grafo novo).
  * Construo um **grafo de conflitos**:

    * vértices = manutenções;
    * aresta entre duas manutenções se as rotas compartilham algum hub.
* Algoritmo: **Welsh–Powell** (coloração gulosa).

  * Calculo o grau de cada manutenção (quantos conflitos ela tem).
  * Ordeno as manutenções por grau decrescente.
  * Vou atribuindo cores (turnos) de forma gulosa, sem colocar vizinhos com a mesma cor.
* Interpretação logística:

  * Cada **cor** = um **turno de manutenção**.
  * Manutenções com a mesma cor podem ocorrer em paralelo sem conflito de hub.
  * Ajuda a planejar janelas de manutenção sem travar vários hubs ao mesmo tempo.

---

### 5. Rota Única de Inspeção

Essa parte tem dois cenários diferentes:

#### Cenário A – Percurso de Rotas (arestas)

* Pergunta: dá pra percorrer **todas as arestas exatamente uma vez** e voltar ao ponto de partida?
* Modelagem: problema **euleriano** em grafo dirigido.
* O código:

  * verifica conectividade em relação às arestas;
  * calcula `inDegree` e `outDegree` de cada vértice;
  * checa condições de Euler;
  * se der certo, monta a rota euleriana com uma variação de **Hierholzer**.
* Interpretação logística:

  * Se existir, o inspetor consegue passar por **todas as rotas** sem repetir caminho, terminando no hub inicial.

#### Cenário B – Percurso de Hubs (vértices)

* Pergunta: dá pra visitar **todos os hubs exatamente uma vez** e voltar ao início?
* Modelagem: **ciclo hamiltoniano** em grafo dirigido.
* Algoritmo:

  * backtracking que tenta construir um caminho visitando cada vértice uma única vez;
  * no final, verifica se há aresta de volta pro vértice inicial.
* Interpretação logística:

  * Se existir, é uma rota de inspeção de hubs “perfeita”: passa uma vez por cada ponto e fecha um ciclo.
  * Se não existir, significa que qualquer plano de inspeção vai precisar repetir hub ou usar mais de um percurso.

---

Se quiser, você pode só adaptar o tom (mais ou menos informal) e, se o professor pedir, acrescentar um mini exemplo com um grafo de teste no final.
