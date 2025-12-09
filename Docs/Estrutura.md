# Estrutura do Projeto

## Visão Geral da Arquitetura

O projeto segue uma arquitetura em camadas com separação clara de responsabilidades:

```
LogisticsOptimization/
├── Models/              # Estruturas de dados fundamentais
├── Algorithms/          # Implementações dos algoritmos
│   └── Results/         # Classes de resultado
├── Utils/               # Utilitários e helpers
├── grafos/              # Arquivos de grafos DIMACS
├── TestData/            # Grafos de teste
├── logs/                # Logs de execução (gerado)
├── Docs/                # Documentação
├── bin/                 # Binários compilados (gerado)
└── obj/                 # Objetos intermediários (gerado)
```

## Estrutura Detalhada

### 📁 Diretório Raiz

#### `LogisticsOptimization.csproj`
**Tipo:** Arquivo de configuração do projeto  
**Propósito:** Define configurações de compilação .NET 9  
**Conteúdo:**
- Target Framework: net9.0
- Output Type: Console Application
- Nullable: enable
- ImplicitUsings: enable

#### `Program.cs`
**Tipo:** Ponto de entrada da aplicação  
**Propósito:** Interface console e coordenação geral  
**Responsabilidades:**
- Menu interativo
- Carregamento de grafos
- Chamada de algoritmos
- Formatação de saída
- Gerenciamento de logging

**Componentes principais:**
```csharp
- Main(string[] args)              # Entry point
- LoadGraphFromFile()              # Carrega grafo DIMACS
- RunDijkstra()                    # Executa Dijkstra
- RunEdmondsKarp()                 # Executa Edmonds-Karp
- RunKruskal()                     # Executa Kruskal
- RunWelshPowell()                 # Executa Welsh-Powell
- RunEulerian()                    # Executa Fleury
- RunHamiltonian()                 # Executa Hamiltoniano
```

#### `.gitignore`
**Tipo:** Configuração Git  
**Propósito:** Ignora arquivos desnecessários no versionamento  
**Ignora:**
- `bin/` - Binários compilados
- `obj/` - Objetos intermediários
- `logs/` - Arquivos de log

---

### 📂 Models/ - Estruturas de Dados

Contém as classes fundamentais que representam o grafo e seus componentes.

#### `Node.cs`
**Propósito:** Representa um hub (centro de distribuição)  
**Propriedades:**
- `int Id` - Identificador único do hub
- `string Name` - Nome descritivo (padrão: "Hub_{Id}")

**Métodos:**
- `Equals(object?)` - Comparação por ID
- `GetHashCode()` - Hash baseado no ID
- `ToString()` - Representação textual

#### `Edge.cs`
**Propósito:** Representa uma rota direcionada entre dois hubs  
**Propriedades:**
- `Node Source` - Hub de origem
- `Node Target` - Hub de destino
- `double Cost` - Custo financeiro (R$)
- `double Capacity` - Capacidade máxima (toneladas)
- `double Flow` - Fluxo atual (usado em max flow)

**Propriedades Calculadas:**
- `double ResidualCapacity` - Capacity - Flow

**Métodos:**
- `Clone()` - Cria cópia profunda da aresta
- `ToString()` - Representação textual

#### `LogisticsGraph.cs`
**Propósito:** Representa o grafo completo usando lista de adjacências  
**Estruturas Internas:**
- `Dictionary<int, Node> _nodes` - Mapa ID → Node
- `Dictionary<int, List<Edge>> _adjacencyList` - Lista de adjacências

**Métodos Principais:**
```csharp
// Modificação
+ AddNode(Node node)
+ AddEdge(Edge edge)

// Consulta
+ GetNode(int id) : Node?
+ GetOutgoingEdges(int nodeId) : List<Edge>
+ GetIncomingEdges(int nodeId) : List<Edge>
+ GetAllNodes() : IEnumerable<Node>
+ GetAllEdges() : IEnumerable<Edge>

// Informações
+ NodeCount : int
+ EdgeCount : int
+ ContainsNode(int nodeId) : bool
+ GetOutDegree(int nodeId) : int
+ GetInDegree(int nodeId) : int

// Utilidades
+ Clone() : LogisticsGraph
```

---

### 📂 Algorithms/ - Implementações

Cada algoritmo é implementado como uma classe estática com método público principal.

#### `DijkstraAlgorithm.cs`
**Propósito:** Caminho mínimo entre dois vértices  
**Método:** `FindShortestPath(graph, sourceId, targetId) : DijkstraResult`  
**Complexidade:** O(E + V log V)  
**Estruturas usadas:**
- `PriorityQueue<int, double>` para seleção eficiente
- `Dictionary<int, double>` para distâncias
- `Dictionary<int, Edge?>` para predecessores

#### `EdmondsKarpAlgorithm.cs`
**Propósito:** Fluxo máximo entre fonte e sumidouro  
**Método:** `CalculateMaxFlow(graph, sourceId, sinkId) : MaxFlowResult`  
**Complexidade:** O(V E²)  
**Métodos auxiliares:**
- `BFS(...)` - Encontra caminho aumentante
- `GetReachableNodes(...)` - Identifica min-cut

#### `KruskalAlgorithm.cs`
**Propósito:** Árvore geradora mínima (MST)  
**Método:** `FindMinimumSpanningTree(graph) : MSTResult`  
**Complexidade:** O(E log E)  
**Dependência:** `UnionFind` para detecção de ciclos

#### `WelshPowellAlgorithm.cs`
**Propósito:** Coloração de grafos (agendamento)  
**Métodos:**
- `ScheduleMaintenanceShifts(graph, conflicts) : ColoringResult`
- `GenerateConflictsFromSharedNodes(graph) : List<(int, int)>`

**Complexidade:** O(V²)

#### `EulerianAlgorithm.cs`
**Propósito:** Caminho/ciclo euleriano (visita todas arestas)  
**Método:** `FindEulerianPath(graph) : EulerianResult`  
**Complexidade:** O(E²)  
**Algoritmo:** Fleury (evita pontes)  
**Métodos auxiliares:**
- `FleuryAlgorithm(...)` - Implementação principal
- `IsBridge(...)` - Detecta arestas-ponte
- `CountReachableNodes(...)` - Verifica conectividade
- `RemoveEdge(...)` - Remove aresta do grafo
- `IsWeaklyConnected(...)` - Verifica conectividade fraca

#### `HamiltonianAlgorithm.cs`
**Propósito:** Ciclo hamiltoniano (visita todos vértices)  
**Método:** `FindHamiltonianCycle(graph, timeout) : HamiltonianResult`  
**Complexidade:** Exponencial (NP-completo)  
**Estratégia:** Backtracking com timeout  
**Métodos auxiliares:**
- `BacktrackHamiltonian(...)` - Busca recursiva

---

### 📂 Algorithms/Results/ - Classes de Resultado

Cada algoritmo tem sua classe de resultado dedicada.

#### `DijkstraResult.cs`
```csharp
+ double TotalCost           # Custo total do caminho
+ List<Edge> Path            # Sequência de arestas
+ bool Success               # Se caminho foi encontrado
```

#### `MaxFlowResult.cs`
```csharp
+ double MaxFlow             # Fluxo máximo calculado
+ List<Edge> BottleneckEdges # Arestas gargalo (min-cut)
```

#### `MSTResult.cs`
```csharp
+ double TotalCost           # Custo total da MST
+ List<Edge> Edges           # Arestas da MST
```

#### `ColoringResult.cs`
```csharp
+ Dictionary<int, List<Edge>> Shifts  # Turno → Rotas
```

#### `EulerianResult.cs`
```csharp
+ bool IsPossible            # Se caminho existe
+ List<Edge> Path            # Sequência de arestas
```

#### `HamiltonianResult.cs`
```csharp
+ bool IsPossible            # Se ciclo existe
+ List<Node> Cycle           # Sequência de vértices
```

---

### 📂 Utils/ - Utilitários

Classes auxiliares para funcionalidades transversais.

#### `DimacsParser.cs`
**Propósito:** Leitura e escrita de arquivos DIMACS  
**Métodos:**
- `LoadFromFile(filePath) : LogisticsGraph` - Carrega grafo
- `SaveToFile(graph, filePath)` - Salva grafo

**Validações:**
- Existência do arquivo
- Formato do cabeçalho
- Tipos de dados
- IDs de vértices válidos

#### `OutputFormatter.cs`
**Propósito:** Formatação colorida de saída no console  
**Métodos de Utilidade:**
- `PrintHeader(title)` - Cabeçalho formatado
- `PrintSuccess(message)` - Mensagem verde
- `PrintError(message)` - Mensagem vermelha
- `PrintWarning(message)` - Mensagem amarela
- `PrintInfo(message)` - Mensagem branca

**Métodos Específicos:**
- `PrintDijkstraResult(result)`
- `PrintMaxFlowResult(result)`
- `PrintMSTResult(result)`
- `PrintColoringResult(result)`
- `PrintEulerianResult(result)`
- `PrintHamiltonianResult(result)`
- `PrintGraphInfo(graph)`

#### `UnionFind.cs`
**Propósito:** Disjoint Set Union para detecção de ciclos  
**Métodos:**
- `Find(x) : int` - Encontra raiz com path compression
- `Union(x, y) : bool` - Une conjuntos com union by rank
- `Connected(x, y) : bool` - Verifica se estão no mesmo conjunto

**Otimizações:**
- Path compression: O(α(n)) amortizado
- Union by rank: Árvores balanceadas

#### `QueryLogger.cs`
**Propósito:** Sistema de logging de operações  
**Métodos:**
- `StartLogging(graphFileName)` - Inicia novo log
- `LogGraphInfo(nodeCount, edgeCount)` - Registra info do grafo
- `LogAlgorithmExecution(name, params, result)` - Registra execução
- `LogError(error)` - Registra erro
- `CloseCurrentLog()` - Fecha log atual
- `Dispose()` - Cleanup de recursos

**Propriedades:**
- `bool IsActive` - Se logging está ativo

---

### 📂 grafos/ - Grafos de Produção

Contém grafos para uso em produção/análise.

| Arquivo | Vértices | Arestas | Características |
|---------|----------|---------|-----------------|
| grafo01.dimacs | 6 | 12 | Balanceado, médio |
| grafo02.dimacs | 5 | 6 | Simples, pequeno |
| grafo03.dimacs | 8 | 10 | Esparso |
| grafo04.dimacs | 10 | 15 | Médio |
| grafo05.dimacs | 10 | 30 | Denso |
| grafo06.dimacs | 50 | 200 | Grande |
| grafo07.dimacs | 100 | 400 | Muito grande |
| grafo08.dimacs | 15 | 45 | **Ciclo euleriano garantido** |

---

### 📂 TestData/ - Grafos de Teste

Grafos simples para testes e validação.

- `sample_graph_1.dimacs` - Exemplo básico 5×6
- `sample_graph_2_with_capacity.dimacs` - Com capacidades 5×7
- `sample_graph_3_mst.dimacs` - Para testes MST 6×9

---

### 📂 logs/ - Logs de Execução

**Gerado automaticamente** pelo sistema.

Contém logs nomeados como: `<arquivo>_<YYYYMMDD>.log`

**Exemplo:**
- `grafo01_20251208.log`
- `grafo08_20251208.log`

---

### 📂 Docs/ - Documentação

Documentação completa do projeto.

- `Projeto.md` - Especificações técnicas e execução
- `Estrutura.md` - Este arquivo (estrutura do código)
- `Algoritmos.md` - Detalhes dos algoritmos
- `Logging.md` - Sistema de logging

---

## Fluxo de Dados

```
[Arquivo DIMACS]
       ↓
  DimacsParser
       ↓
 LogisticsGraph
       ↓
  [Algoritmos] → [Results]
       ↓
OutputFormatter → Console
       ↓
  QueryLogger → [Arquivo .log]
```

## Dependências entre Módulos

```
Program.cs
  ├─ Models/*
  ├─ Algorithms/*
  │    └─ Results/*
  └─ Utils/*
       ├─ DimacsParser
       ├─ OutputFormatter
       ├─ UnionFind
       └─ QueryLogger

Algorithms/KruskalAlgorithm.cs
  └─ Utils/UnionFind.cs

Algorithms/EdmondsKarpAlgorithm.cs
  └─ Models/LogisticsGraph.Clone()
```

## Convenções de Código

### Nomenclatura
- **Classes:** PascalCase (`LogisticsGraph`)
- **Métodos:** PascalCase (`AddNode`)
- **Propriedades:** PascalCase (`NodeCount`)
- **Variáveis locais:** camelCase (`currentNode`)
- **Campos privados:** _camelCase (`_adjacencyList`)

### Organização
- Um arquivo por classe
- Namespace reflete estrutura de diretórios
- Métodos públicos no topo
- Métodos privados abaixo

### Comentários
**Removidos** para código de produção. Documentação externa em Docs/.
