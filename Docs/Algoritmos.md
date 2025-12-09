# Algoritmos Implementados

## Contexto do Problema

A **Entrega Máxima Logística S.A.** enfrent5 desafios típicos de otimização de redes de distribuição:

1. **Minimizar custos** de transporte entre centros de distribuição
2. **Maximizar capacidade** de escoamento de produtos
3. **Planejar expansão** da rede minimizando investimento
4. **Organizar manutenções** sem conflitos de recursos
5. **Otimizar rotas de inspeção** da infraestrutura

Cada algoritmo implementado resolve um desses problemas específicos.

---

## 1. Dijkstra - Roteamento de Menor Custo

### Problema de Negócio
**"Qual a rota mais econômica para transportar produtos do hub A para o hub B?"**

Uma transportadora precisa enviar mercadorias entre dois centros de distribuição. Existem múltiplas rotas possíveis, cada uma com custos diferentes (combustível, pedágios, desgaste). O objetivo é encontrar a rota que minimiza o custo total.

### Entrada
- Grafo direcionado com custos nas arestas
- Hub de origem
- Hub de destino

### Saída
- Custo total mínimo
- Sequência de hubs no caminho ótimo
- Indicação de sucesso/falha

### Algoritmo
**Dijkstra com fila de prioridade**

**Passos:**
1. Inicializa distâncias: origem = 0, demais = ∞
2. Usa fila de prioridade para processar vértices por distância
3. Para cada vértice, relaxa arestas de saída
4. Reconstrói caminho usando predecessores

**Complexidade:** O(E + V log V)
- E = número de arestas
- V = número de vértices
- log V do PriorityQueue

### Implementação
```csharp
public static DijkstraResult FindShortestPath(
    LogisticsGraph graph, 
    int sourceId, 
    int targetId)
{
    // Inicialização
    var distances = new Dictionary<int, double>();
    var predecessors = new Dictionary<int, Edge?>();
    var pq = new PriorityQueue<int, double>();
    
    // Algoritmo de Dijkstra
    while (pq.Count > 0)
    {
        var current = pq.Dequeue();
        foreach (var edge in graph.GetOutgoingEdges(current))
        {
            // Relaxação
            double newDist = distances[current] + edge.Cost;
            if (newDist < distances[edge.Target.Id])
            {
                distances[edge.Target.Id] = newDist;
                predecessors[edge.Target.Id] = edge;
                pq.Enqueue(edge.Target.Id, newDist);
            }
        }
    }
    
    // Reconstrução do caminho
    // ...
}
```

### Motivação da Escolha

**[Espaço para justificativa do aluno]**

```
Por que Dijkstra foi escolhido para este problema?
Outras alternativas consideradas?
Vantagens e desvantagens?
```

---

## 2. Edmonds-Karp - Capacidade Máxima de Escoamento

### Problema de Negócio
**"Quantas toneladas conseguimos transportar simultaneamente da fábrica para o centro de distribuição final?"**

A empresa precisa saber a capacidade máxima da rede para planejar produção e estoques. Cada rota tem limite de carga. É necessário encontrar o fluxo máximo respeitando essas restrições.

### Entrada
- Grafo direcionado com capacidades nas arestas
- Hub fonte (origem)
- Hub sumidouro (destino)

### Saída
- Fluxo máximo em toneladas
- Arestas gargalo (min-cut) que limitam o fluxo

### Algoritmo
**Edmonds-Karp (Ford-Fulkerson com BFS)**

**Passos:**
1. Cria grafo residual (capacidade - fluxo)
2. Usa BFS para encontrar caminho aumentante
3. Calcula gargalo do caminho (mínima capacidade residual)
4. Atualiza fluxo e capacidades residuais
5. Repete até não haver mais caminhos

**Complexidade:** O(V E²)
- V = vértices
- E = arestas
- BFS é O(E), executado O(VE) vezes no pior caso

### Implementação
```csharp
public static MaxFlowResult CalculateMaxFlow(
    LogisticsGraph graph, 
    int sourceId, 
    int sinkId)
{
    var workingGraph = graph.Clone();
    var residualCapacity = new Dictionary<(int, int), double>();
    double maxFlow = 0;
    
    // Edmonds-Karp
    while (true)
    {
        var parent = BFS(workingGraph, sourceId, sinkId, residualCapacity);
        if (parent == null) break;
        
        // Encontra gargalo
        double pathFlow = double.PositiveInfinity;
        int current = sinkId;
        while (current != sourceId)
        {
            int prev = parent[current];
            pathFlow = Math.Min(pathFlow, residualCapacity[(prev, current)]);
            current = prev;
        }
        
        // Atualiza fluxos
        current = sinkId;
        while (current != sourceId)
        {
            int prev = parent[current];
            residualCapacity[(prev, current)] -= pathFlow;
            residualCapacity[(current, prev)] += pathFlow;
            current = prev;
        }
        
        maxFlow += pathFlow;
    }
    
    // Identifica min-cut (gargalos)
    var reachable = GetReachableNodes(workingGraph, sourceId, residualCapacity);
    // ...
}
```

### Motivação da Escolha

**[Espaço para justificativa do aluno]**

```
Por que Edmonds-Karp (Ford-Fulkerson com BFS)?
Alternativas como Push-Relabel ou Dinic foram consideradas?
Trade-offs entre simplicidade e performance?
```

---

## 3. Kruskal - Expansão da Rede (MST)

### Problema de Negócio
**"Como expandir a rede conectando todos os hubs com custo mínimo de instalação?"**

A empresa quer expandir operações para novos hubs. Precisa decidir quais rotas construir para conectar todos os pontos, minimizando o investimento em infraestrutura.

### Entrada
- Grafo (tratado como não-direcionado)
- Custos de instalação nas arestas

### Saída
- Custo total de instalação
- Lista de rotas a serem construídas

### Algoritmo
**Kruskal com Union-Find**

**Passos:**
1. Ordena arestas por custo crescente
2. Para cada aresta em ordem:
   - Verifica se conecta componentes diferentes (Union-Find)
   - Se sim, adiciona à MST
   - Se não, descarta (evita ciclo)
3. Termina quando tem V-1 arestas

**Complexidade:** O(E log E)
- Ordenação: O(E log E)
- Union-Find: O(α(n)) amortizado por operação
- Total dominado pela ordenação

### Implementação
```csharp
public static MSTResult FindMinimumSpanningTree(LogisticsGraph graph)
{
    var result = new MSTResult();
    var edges = graph.GetAllEdges().OrderBy(e => e.Cost).ToList();
    
    // Union-Find setup
    var uf = new UnionFind(nodeCount);
    
    // Kruskal
    foreach (var edge in edges)
    {
        int sourceIdx = idToIndex[edge.Source.Id];
        int targetIdx = idToIndex[edge.Target.Id];
        
        if (uf.Union(sourceIdx, targetIdx))
        {
            result.Edges.Add(edge);
            result.TotalCost += edge.Cost;
            
            if (result.Edges.Count == nodeCount - 1)
                break;
        }
    }
    
    return result;
}
```

### Motivação da Escolha

**[Espaço para justificativa do aluno]**

```
Por que Kruskal e não Prim?
Quando Prim seria preferível?
Vantagens do Union-Find?
```

---

## 4. Welsh-Powell - Agendamento de Manutenções

### Problema de Negócio
**"Como agendar manutenções em rotas que compartilham recursos sem conflitos?"**

Rotas que compartilham hubs não podem sofrer manutenção simultaneamente (recurso limitado). É necessário agrupar rotas em turnos de modo que rotas no mesmo turno não compartilhem recursos.

### Entrada
- Grafo de rotas
- Lista de conflitos entre rotas (pares de rotas que compartilham hubs)

### Saída
- Número mínimo de turnos necessários
- Agrupamento de rotas por turno

### Algoritmo
**Welsh-Powell (Greedy Graph Coloring)**

**Passos:**
1. Constrói grafo de conflitos (rotas = vértices, conflitos = arestas)
2. Ordena rotas por grau decrescente (número de conflitos)
3. Para cada rota:
   - Encontra menor "cor" (turno) não usada por vizinhos
   - Atribui essa cor à rota
4. Agrupa rotas por cor

**Complexidade:** O(V²)
- Construção do grafo de conflitos: O(E²) no pior caso
- Coloração: O(V²)

### Implementação
```csharp
public static ColoringResult ScheduleMaintenanceShifts(
    LogisticsGraph graph, 
    List<(int, int)> conflicts)
{
    // Constrói grafo de conflitos
    var conflictGraph = new Dictionary<int, HashSet<int>>();
    foreach (var (idx1, idx2) in conflicts)
    {
        conflictGraph[idx1].Add(idx2);
        conflictGraph[idx2].Add(idx1);
    }
    
    // Ordena por grau
    var degrees = conflictGraph
        .Select(kvp => new { EdgeIdx = kvp.Key, Degree = kvp.Value.Count })
        .OrderByDescending(x => x.Degree)
        .ToList();
    
    // Welsh-Powell coloring
    var colors = new Dictionary<int, int>();
    foreach (var item in degrees)
    {
        var usedColors = new HashSet<int>();
        foreach (var neighbor in conflictGraph[item.EdgeIdx])
        {
            if (colors.ContainsKey(neighbor))
                usedColors.Add(colors[neighbor]);
        }
        
        int color = 0;
        while (usedColors.Contains(color))
            color++;
        
        colors[item.EdgeIdx] = color;
    }
    
    // Agrupa por cor (turno)
    // ...
}
```

### Motivação da Escolha

**[Espaço para justificativa do aluno]**

```
Por que algoritmo guloso?
Coloração ótima é NP-completo - qual aproximação?
Welsh-Powell vs outras heurísticas?
```

---

## 5. Fleury - Rota de Inspeção Euleriana

### Problema de Negócio
**"Como inspecionar todas as rotas da rede visitando cada uma exatamente uma vez?"**

Equipes de manutenção precisam inspecionar todas as rotas. O objetivo é encontrar um percurso que visite cada rota exatamente uma vez, minimizando deslocamentos desnecessários.

### Entrada
- Grafo direcionado

### Saída
- Indicação se caminho/ciclo euleriano existe
- Sequência de arestas do caminho (se existir)

### Condições de Existência
**Ciclo Euleriano:** Todos os vértices têm `in-degree = out-degree`  
**Caminho Euleriano:** Exatamente um vértice com `out-degree = in-degree + 1`

### Algoritmo
**Fleury (evita pontes)**

**Passos:**
1. Verifica se grafo satisfaz condições eulerianas
2. Escolhe vértice inicial apropriado
3. A cada passo:
   - Entre vizinhos disponíveis, evita arestas-ponte
   - Aresta-ponte: cuja remoção desconecta grafo
   - Adiciona aresta ao caminho e remove do grafo
4. Continua até visitar todas as arestas

**Complexidade:** O(E²)
- Para cada aresta (E), verifica se é ponte: O(E)

### Implementação
```csharp
public static EulerianResult FindEulerianPath(LogisticsGraph graph)
{
    // Verifica condições
    int oddDegreeCount = 0;
    foreach (var node in graph.GetAllNodes())
    {
        if (graph.GetInDegree(node.Id) != graph.GetOutDegree(node.Id))
            oddDegreeCount++;
    }
    
    if (oddDegreeCount > 2)
        return result; // Não é euleriano
    
    // Fleury
    var workingGraph = graph.Clone();
    var path = new List<Edge>();
    int currentNode = startNode;
    
    while (workingGraph.EdgeCount > 0)
    {
        var edges = workingGraph.GetOutgoingEdges(currentNode);
        
        Edge chosenEdge;
        if (edges.Count == 1)
        {
            chosenEdge = edges[0];
        }
        else
        {
            // Escolhe aresta que não é ponte
            chosenEdge = edges.FirstOrDefault(e => !IsBridge(workingGraph, e))
                       ?? edges[0];
        }
        
        path.Add(chosenEdge);
        RemoveEdge(workingGraph, chosenEdge);
        currentNode = chosenEdge.Target.Id;
    }
    
    return result;
}
```

### Motivação da Escolha

**[Espaço para justificativa do aluno]**

```
Por que Fleury e não Hierholzer?
Trade-off: O(E²) vs O(E) - vale a intuitividade?
Quando Hierholzer seria melhor?
```

---

## 6. Hamiltoniano - Rota de Inspeção de Todos os Hubs

### Problema de Negócio
**"Como visitar todos os hubs da rede exatamente uma vez e retornar à origem?"**

Auditoria precisa visitar todos os centros de distribuição. O objetivo é encontrar uma rota que passe por cada hub exatamente uma vez e retorne ao ponto inicial.

### Entrada
- Grafo direcionado
- Timeout em segundos (proteção)

### Saída
- Indicação se ciclo hamiltoniano existe
- Sequência de vértices do ciclo (se encontrado)

### Algoritmo
**Backtracking com Timeout**

**Passos:**
1. Inicia do primeiro vértice
2. Recursivamente tenta estender caminho:
   - Marca vértice como visitado
   - Para cada vizinho não visitado:
     - Adiciona ao caminho
     - Recursão
     - Se falhar, remove (backtrack)
3. Quando visita todos vértices:
   - Verifica se existe aresta de volta ao início
4. Timeout protege contra explosão combinatória

**Complexidade:** O(V!) - Exponencial (NP-completo)
- Sem otimizações: testa todas permutações
- Com poda: ainda exponencial no pior caso

### Implementação
```csharp
public static HamiltonianResult FindHamiltonianCycle(
    LogisticsGraph graph, 
    int timeoutSeconds = 10)
{
    var path = new List<Node> { nodes[0] };
    var visited = new HashSet<int> { nodes[0].Id };
    var stopwatch = Stopwatch.StartNew();
    
    bool found = BacktrackHamiltonian(
        graph, 
        nodes[0].Id, 
        path, 
        visited, 
        nodes.Count,
        stopwatch,
        timeoutSeconds * 1000);
    
    if (found)
    {
        path.Add(nodes[0]); // Fecha ciclo
        result.IsPossible = true;
        result.Cycle = path;
    }
    
    return result;
}

private static bool BacktrackHamiltonian(/* ... */)
{
    if (stopwatch.ElapsedMilliseconds > timeoutMs)
        return false; // Timeout
    
    if (path.Count == totalNodes)
    {
        // Verifica se pode retornar ao início
        return graph.GetOutgoingEdges(currentNodeId)
            .Any(e => e.Target.Id == startNodeId);
    }
    
    // Recursão
    foreach (var edge in graph.GetOutgoingEdges(currentNodeId))
    {
        if (!visited.Contains(edge.Target.Id))
        {
            visited.Add(edge.Target.Id);
            path.Add(edge.Target);
            
            if (BacktrackHamiltonian(/* ... */))
                return true;
            
            // Backtrack
            visited.Remove(edge.Target.Id);
            path.RemoveAt(path.Count - 1);
        }
    }
    
    return false;
}
```

### Motivação da Escolha

**[Espaço para justificativa do aluno]**

```
Por que backtracking simples?
Otimizações consideradas (branch-and-bound, programação dinâmica)?
Trade-off entre simplicidade e eficiência?
Por que timeout é essencial?
```

---

## Comparação de Complexidades

| Algoritmo | Complexidade | Tipo | Garantia |
|-----------|-------------|------|----------|
| Dijkstra | O(E + V log V) | Polinomial | Ótimo |
| Edmonds-Karp | O(V E²) | Polinomial | Ótimo |
| Kruskal | O(E log E) | Polinomial | Ótimo |
| Welsh-Powell | O(V²) | Polinomial | Heurística |
| Fleury | O(E²) | Polinomial | Ótimo |
| Hamiltoniano | O(V!) | Exponencial | Ótimo (se terminar) |

## Estruturas de Dados Especializadas

### Union-Find (Disjoint Set Union)
**Usado em:** Kruskal  
**Otimizações:**
- Path compression: O(α(n)) por Find
- Union by rank: Árvores balanceadas

### Priority Queue
**Usado em:** Dijkstra  
**Implementação:** Binary heap .NET  
**Complexidade:** O(log n) por operação

### Hash Sets e Dictionaries
**Usado em:** Todos os algoritmos  
**Complexidade:** O(1) médio para busca/inserção

## Referências dos Algoritmos

- **Dijkstra:** E. W. Dijkstra, "A note on two problems in connexion with graphs" (1959)
- **Ford-Fulkerson/Edmonds-Karp:** L. R. Ford Jr. e D. R. Fulkerson (1956), Jack Edmonds e Richard Karp (1972)
- **Kruskal:** Joseph Kruskal, "On the shortest spanning subtree of a graph" (1956)
- **Welsh-Powell:** Welsh e Powell, "An upper bound for the chromatic number of a graph" (1967)
- **Fleury:** M. Fleury, "Deux problèmes de géométrie de situation" (1883)
- **Problema Hamiltoniano:** William Rowan Hamilton (1857)
