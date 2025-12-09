# Especificações Técnicas do Projeto

## Visão Geral

Sistema de otimização de malha logística desenvolvido para a **Entrega Máxima Logística S.A.**, implementando cinco algoritmos clássicos de teoria dos grafos para resolver problemas reais de distribuição e logística.

## Enunciado do Projeto

A Entrega Máxima Logística S.A. necessita de um sistema computacional para otimizar sua rede de distribuição. O sistema deve ser capaz de:

1. **Encontrar rotas de menor custo** entre centros de distribuição
2. **Calcular capacidade máxima de escoamento** entre pontos da rede
3. **Planejar expansão da rede** minimizando custos de instalação
4. **Agendar manutenções** em rotas sem conflitos de recursos
5. **Definir rotas de inspeção** visitando todas as rotas ou todos os hubs

## Especificações Técnicas

### Linguagem e Framework
- **Linguagem:** C# 11.0
- **Framework:** .NET 9.0
- **Paradigma:** Orientado a Objetos
- **Tipo de Aplicação:** Console Application

### Requisitos do Sistema
- **.NET SDK:** 9.0 ou superior
- **Sistema Operacional:** Windows, Linux ou macOS
- **Memória RAM:** Mínimo 512 MB
- **Espaço em Disco:** 50 MB

### Estrutura de Dados Principal

O sistema utiliza **representação de grafos por lista de adjacências**:

```csharp
Dictionary<int, List<Edge>> adjacencyList
```

**Vantagens:**
- Acesso O(1) para obter arestas de um vértice
- Eficiente em memória para grafos esparsos
- Facilita travessia de vizinhos

### Formato de Entrada: DIMACS

O sistema lê grafos no formato DIMACS estendido:

```
V E
source target cost [capacity]
source target cost [capacity]
...
```

**Onde:**
- `V` = número de vértices (hubs)
- `E` = número de arestas (rotas)
- `source` = ID do hub de origem
- `target` = ID do hub de destino
- `cost` = custo financeiro da rota (R$)
- `capacity` = capacidade máxima em toneladas (opcional, padrão: ∞)

**Exemplo:**
```
5 6
1 2 10 15
2 3 8 20
3 4 12 18
4 5 7 22
5 1 9 16
2 4 15 25
```

### Validações de Entrada

O parser implementa validações rigorosas:
- ✓ Verificação de existência do arquivo
- ✓ Validação do cabeçalho (V E)
- ✓ Validação de IDs de vértices
- ✓ Validação de tipos numéricos (cost, capacity)
- ✓ Tratamento de linhas vazias
- ✓ Mensagens de erro descritivas

## Como Executar o Projeto

### 1. Pré-requisitos

Instale o .NET SDK 9.0:

**Linux (Ubuntu/Debian):**
```bash
wget https://dot.net/v1/dotnet-install.sh
chmod +x dotnet-install.sh
./dotnet-install.sh --channel 9.0
```

**macOS:**
```bash
brew install dotnet-sdk
```

**Windows:**
Baixe o instalador em: https://dotnet.microsoft.com/download

### 2. Compilação

No diretório do projeto:

```bash
dotnet build LogisticsOptimization.csproj
```

**Saída esperada:**
```
Build succeeded.
    0 Warning(s)
    0 Error(s)
```

### 3. Execução

```bash
dotnet run
```

### 4. Uso Interativo

O sistema apresenta um menu interativo:

```
╔═══════════════════════════════════════════════════════════╗
║                    MENU PRINCIPAL                         ║
╠═══════════════════════════════════════════════════════════╣
║  1. Carregar Grafo (DIMACS)                               ║
║  2. Roteamento de Menor Custo (Dijkstra)                  ║
║  3. Capacidade Máxima de Escoamento (Edmonds-Karp)        ║
║  4. Expansão da Rede - MST (Kruskal)                      ║
║  5. Agendamento de Manutenções (Welsh-Powell)             ║
║  6. Rota de Inspeção - Euleriano                          ║
║  7. Rota de Inspeção - Hamiltoniano                       ║
║  8. Sair                                                  ║
╚═══════════════════════════════════════════════════════════╝
```

**Fluxo de trabalho:**
1. Carregar grafo DIMACS (opção 1)
2. Executar algoritmos desejados (opções 2-7)
3. Visualizar resultados formatados
4. Consultar logs gerados

### 5. Exemplo de Uso Completo

```bash
# 1. Compilar
dotnet build

# 2. Executar
dotnet run

# 3. No menu:
Escolha: 1
Caminho: grafos/grafo01.dimacs
> ✓ Grafo carregado com sucesso!

Escolha: 2
Origem: 1
Destino: 6
> Resultado: Custo Total: R$ 15.00

# 4. Verificar log
cat logs/grafo01_20251208.log
```

## Grafos de Teste Incluídos

### Grafos Principais (grafos/)
- **grafo01.dimacs** - 6 vértices, 12 arestas, balanceado
- **grafo02.dimacs** - 5 vértices, 6 arestas, simples
- **grafo03.dimacs** - 8 vértices, 10 arestas, esparso
- **grafo04.dimacs** - 10 vértices, 15 arestas, médio
- **grafo05.dimacs** - 10 vértices, 30 arestas, denso
- **grafo06.dimacs** - 50 vértices, 200 arestas, grande
- **grafo07.dimacs** - 100 vértices, 400 arestas, muito grande
- **grafo08.dimacs** - 15 vértices, 45 arestas, **ciclo euleriano garantido**

### Grafos de Teste (TestData/)
- **sample_graph_1.dimacs** - Exemplo básico
- **sample_graph_2_with_capacity.dimacs** - Com capacidades
- **sample_graph_3_mst.dimacs** - Para testes de MST

## Saídas do Sistema

### 1. Saída Console
Formatação colorida com:
- ✓ Verde para sucessos
- ✗ Vermelho para erros
- ⚠ Amarelo para avisos
- ℹ Branco para informações

### 2. Arquivos de Log
Gerados em `logs/`:
- Formato: `<arquivo>_<YYYYMMDD>.log`
- Conteúdo: timestamps, operações, resultados
- Persistência: múltiplas sessões acumuladas

## Performance Esperada

### Grafos Pequenos (< 20 vértices)
- Dijkstra: < 1ms
- Edmonds-Karp: < 5ms
- Kruskal: < 1ms
- Welsh-Powell: < 5ms
- Euleriano: < 10ms
- Hamiltoniano: < 100ms

### Grafos Médios (20-50 vértices)
- Dijkstra: < 5ms
- Edmonds-Karp: < 50ms
- Kruskal: < 10ms
- Welsh-Powell: < 20ms
- Euleriano: < 100ms
- Hamiltoniano: timeout configurável

### Grafos Grandes (> 50 vértices)
- Dijkstra: < 20ms
- Edmonds-Karp: < 500ms
- Kruskal: < 50ms
- Welsh-Powell: < 100ms
- Euleriano: < 1s
- Hamiltoniano: não recomendado (NP-completo)

## Limitações Conhecidas

1. **Hamiltoniano:** Limitado a grafos pequenos devido à complexidade exponencial
2. **Memória:** Grafos muito grandes (>1000 vértices) podem consumir muita RAM
3. **Formato:** Apenas DIMACS suportado para entrada
4. **Interface:** Apenas console (sem GUI)

## Recursos Avançados

### Clone de Grafos
Alguns algoritmos (Edmonds-Karp, Fleury) modificam o grafo durante execução. O sistema clona o grafo original para preservá-lo:

```csharp
var workingGraph = graph.Clone();
```

### Union-Find Otimizado
Implementação com:
- Path compression
- Union by rank
- Complexidade amortizada O(α(n))

### Proteção de Timeout
Algoritmos NP-completos têm proteção de timeout configurável:

```csharp
HamiltonianAlgorithm.FindHamiltonianCycle(graph, timeoutSeconds: 10);
```

## Manutenção e Extensão

### Adicionar Novo Algoritmo

1. Criar classe em `Algorithms/`
2. Criar classe de resultado em `Algorithms/Results/`
3. Adicionar método de formatação em `Utils/OutputFormatter.cs`
4. Integrar no menu em `Program.cs`
5. Adicionar logging correspondente

### Adicionar Novo Formato de Entrada

1. Criar parser em `Utils/`
2. Implementar conversão para `LogisticsGraph`
3. Adicionar opção no menu de carregamento

## Referências Técnicas

- **Estruturas de Dados:** Cormen, Leiserson, Rivest - "Introduction to Algorithms"
- **Teoria dos Grafos:** Bondy, Murty - "Graph Theory"
- **C# .NET:** Microsoft Documentation - https://docs.microsoft.com/dotnet
- **DIMACS Format:** https://mat.tepper.cmu.edu/COLOR/general/ccformat.ps
