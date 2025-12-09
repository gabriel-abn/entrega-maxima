# Sistema de Otimização Logística - Entrega Máxima S.A.

Uma aplicação console em C# (.NET 9) que implementa 5 algoritmos clássicos de grafos para otimização de malha de distribuição logística.

## 📋 Visão Geral

O sistema modela a rede logística como um **Grafo Direcionado e Ponderado**, onde:
- **Vértices (V):** Centros de Distribuição (Hubs)
- **Arestas (E):** Rotas Rodoviárias
- **Peso (w):** Custo financeiro (R$)
- **Capacidade (c):** Limite de carga (toneladas)

## 🎯 Algoritmos Implementados

### 1. Roteamento de Menor Custo (Dijkstra)
- **Problema:** Encontrar a rota mais barata entre dois hubs
- **Complexidade:** O(E + V log V)
- **Entrada:** Hub origem e destino
- **Saída:** Caminho mínimo e custo total

### 2. Capacidade Máxima de Escoamento (Edmonds-Karp)
- **Problema:** Calcular volume máximo de toneladas entre origem e destino
- **Complexidade:** O(V E²)
- **Entrada:** Hub fonte e sumidouro
- **Saída:** Fluxo máximo e arestas críticas (gargalos)

### 3. Expansão da Rede - MST (Kruskal)
- **Problema:** Conectar todos os hubs com custo mínimo
- **Complexidade:** O(E log E)
- **Entrada:** Grafo completo
- **Saída:** Árvore geradora mínima e custo total

### 4. Agendamento de Manutenções (Welsh-Powell)
- **Problema:** Agrupar rotas em turnos sem conflito de recursos
- **Complexidade:** Greedy O(V²)
- **Entrada:** Grafo e conflitos
- **Saída:** Número mínimo de turnos e atribuições

### 5. Rota de Inspeção
- **Euleriano (Fleury):** Percorrer todas as rotas sem repetições
- **Hamiltoniano (Backtracking):** Visitar todos os hubs exatamente uma vez
- **Saída:** Viabilidade e sequência (se possível)

## 📁 Formato DIMACS

O sistema utiliza arquivos no formato DIMACS para representar grafos:

```
V E
source1 target1 cost1 [capacity1]
source2 target2 cost2 [capacity2]
...
```

**Exemplo:**
```
5 6
1 2 2
1 4 2
2 3 5
3 4 1
4 5 3
5 1 4
```

- **Linha 1:** Número de vértices e arestas
- **Linhas seguintes:** origem, destino, custo, [capacidade opcional]

## 🚀 Como Usar

### Pré-requisitos
- .NET 9 SDK

### Compilar e Executar

```bash
cd /home/gabrielabn/projects/puc/grafos/trabalho
dotnet build
dotnet run
```

### Fluxo de Uso

1. **Carregar Grafo (opção 1)**
   - Informe o caminho para arquivo DIMACS
   - Exemplo: `TestData/sample_graph_1.dimacs`

2. **Executar Algoritmos (opções 2-7)**
   - Cada algoritmo solicitará parâmetros específicos
   - Resultados são exibidos formatados no console

## 📂 Estrutura do Projeto

```
LogisticsOptimization/
├── Models/
│   ├── Node.cs              # Classe de nó (hub)
│   ├── Edge.cs              # Classe de aresta (rota)
│   └── LogisticsGraph.cs    # Grafo com lista de adjacência
├── Algorithms/
│   ├── Results/             # Classes de resultado
│   ├── DijkstraAlgorithm.cs
│   ├── EdmondsKarpAlgorithm.cs
│   ├── KruskalAlgorithm.cs
│   ├── WelshPowellAlgorithm.cs
│   ├── EulerianAlgorithm.cs
│   └── HamiltonianAlgorithm.cs
├── Utils/
│   ├── UnionFind.cs         # Estrutura Union-Find
│   ├── DimacsParser.cs      # Parser de arquivos
│   └── OutputFormatter.cs   # Formatação de saída
├── TestData/                # Grafos de teste
└── Program.cs               # Aplicação console
```

## 🧪 Grafos de Teste

O projeto inclui grafos de exemplo em `TestData/`:
- `sample_graph_1.dimacs` - Exemplo básico (5 nós, 6 arestas)
- `sample_graph_2_with_capacity.dimacs` - Com capacidades para max flow
- `sample_graph_3_mst.dimacs` - Maior para testar MST

## ⚙️ Detalhes Técnicos

### Data Structures
- **Grafo:** Lista de adjacência (Dictionary<int, List<Edge>>)
- **Union-Find:** Path compression + union by rank
- **Priority Queue:** Nativa do .NET 6+ para Dijkstra

### Complexidades
| Algoritmo | Complexidade | Estrutura Chave |
|-----------|--------------|-----------------|
| Dijkstra | O(E + V log V) | PriorityQueue |
| Edmonds-Karp | O(V E²) | BFS + Residual Graph |
| Kruskal | O(E log E) | Union-Find |
| Welsh-Powell | O(V²) | Greedy Coloring |
| Hierholzer | O(E) | Stack-based DFS |
| Hamiltonian | Exponencial* | Backtracking |

*Com timeout protection

## 📝 Notas Importantes

- **Problema NP-Completo:** O algoritmo hamiltoniano possui timeout configurável (padrão: 10s)
- **Grafos Direcionados:** Todos os algoritmos consideram a direção das arestas, exceto Kruskal (MST trata como não-direcionado)
- **Capacidades:** Opcionais no arquivo DIMACS, usadas apenas para max flow

## 👥 Autor

Desenvolvido para Entrega Máxima Logística S.A.
Projeto acadêmico - PUC - Teoria dos Grafos

## 📄 Licença

Este projeto é para fins educacionais.
