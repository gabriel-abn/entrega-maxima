# Sistema de Otimização Logística

> **Entrega Máxima Logística S.A.**  
> Sistema de otimização de malha de distribuição implementando 5 algoritmos clássicos de teoria dos grafos


## Problema

A **Entrega Máxima Logística S.A.** enfrenta desafios complexos na gestão de sua rede de distribuição:

- ✈️ **Roteamento:** Encontrar rotas de menor custo entre centros de distribuição
- 🚛 **Capacidade:** Calcular fluxo máximo de mercadorias na rede
- 🏗️ **Expansão:** Planejar crescimento da rede minimizando investimento
- 🔧 **Manutenção:** Agendar manutenções em rotas sem conflitos de recursos
- 📋 **Inspeção:** Definir rotas ótimas para inspeção de infraestrutura

## Solução

Sistema computacional em **C# .NET 9** que implementa:

| Problema | Algoritmo | Complexidade |
|----------|-----------|--------------|
| Caminho Mínimo | **Dijkstra** | O(E + V log V) |
| Fluxo Máximo | **Edmonds-Karp** | O(V E²) |
| MST (Expansão) | **Kruskal** | O(E log E) |
| Agendamento | **Welsh-Powell** | O(V²) |
| Rota Euleriana | **Fleury** | O(E²) |
| Rota Hamiltoniana | **Backtracking** | Exponencial |

## Características

- 📊 **Entrada:** Formato DIMACS estendido com custos e capacidades
- 🎨 **Interface:** Console interativa com saída formatada e colorida
- 📝 **Logging:** Registro automático de todas as operações
- 🧪 **Testes:** 10 grafos de teste incluídos (5 a 100 vértices)
- ✅ **Validação:** 100% de testes passando

## Início Rápido

```bash
# Compilar
dotnet build

# Executar
dotnet run

# Usar
1. Carregar grafo (opção 1)
2. Executar algoritmos (opções 2-7)
3. Consultar logs em logs/
```

## Exemplo de Uso

```bash
$ dotnet run
> Escolha: 1
> Caminho: grafos/grafo08.dimacs
✓ Grafo carregado com sucesso!

> Escolha: 2  # Dijkstra
> Origem: 1
> Destino: 15
✓ Custo Total: R$ 25.00
  Caminho: 1 → 2 → 5 → 11 → 13 → 15

> Escolha: 6  # Euleriano
✓ Caminho euleriano encontrado com 45 arestas!
```

## Documentação Completa

### 📘 [Especificações Técnicas](Docs/Projeto.md)
- Enunciado do problema
- Formato DIMACS detalhado
- Instruções de compilação e execução
- Requisitos do sistema
- Grafos de teste incluídos

### 🏗️ [Estrutura do Código](Docs/Estrutura.md)
- Arquitetura do projeto
- Descrição de cada diretório e arquivo
- Classes e responsabilidades
- Fluxo de dados
- Convenções de código

### 🧮 [Algoritmos Implementados](Docs/Algoritmos.md)
- Problema de negócio de cada algoritmo
- Complexidades e justificativas
- Pseudocódigo e implementação
- Comparação entre algoritmos
- Referências teóricas

### 📊 [Sistema de Logging](Docs/Logging.md)
- Funcionamento do logging automático
- Formato dos arquivos de log
- Exemplos de logs gerados
- Análise e auditoria

## Estrutura do Projeto

```
LogisticsOptimization/
├── Models/              # Node, Edge, LogisticsGraph
├── Algorithms/          # Implementações dos 6 algoritmos
│   └── Results/         # Classes de resultado (DTOs)
├── Utils/               # Parser, Formatter, Logger, UnionFind
├── grafos/              # 8 grafos DIMACS de produção
├── TestData/            # 3 grafos de teste
├── Docs/                # Documentação completa
└── logs/                # Logs automáticos (gerado)
```

## Tecnologias

- **Linguagem:** C# 11
- **Framework:** .NET 9.0
- **Tipo:** Console Application
- **Paradigma:** Orientado a Objetos
- **Estrutura de Dados:** Lista de Adjacências

## Requisitos

- .NET SDK 9.0 ou superior
- Windows, Linux ou macOS
- 512 MB RAM mínimo

## Resultados de Testes

✅ **58/58 testes passaram (100%)**

Testado com 10 grafos diferentes:
- Grafos pequenos: 5-6 vértices
- Grafos médios: 8-15 vértices
- Grafos grandes: 50-100 vértices

Todos os algoritmos funcionando corretamente com validação matemática.

## Grafos Especiais

### grafo08.dimacs
Grafo especialmente projetado com **ciclo euleriano garantido**:
- 15 vértices, 45 arestas
- Todos os vértices com `in-degree = out-degree = 3`
- Fortemente conectado
- Ideal para demonstração do algoritmo de Fleury

## Funcionalidades Avançadas

- ✅ Clone de grafos para preservar estado original
- ✅ Union-Find otimizado (path compression + union by rank)
- ✅ Proteção de timeout em algoritmos NP-completos
- ✅ Validação rigorosa de entrada DIMACS
- ✅ Tratamento de erros com mensagens descritivas
- ✅ Logging automático com timestamps

## Logs Automáticos

Cada consulta é registrada automaticamente:

```
logs/grafo08_20251208.log:
----------------------------------------------------------------------
Sessão iniciada: 2025-12-08 22:00:00
Arquivo carregado: grafos/grafo08.dimacs
----------------------------------------------------------------------
Algoritmo: Dijkstra - Roteamento de Menor Custo
Timestamp: 2025-12-08 22:00:30
Parâmetros: Origem: 1, Destino: 15
Resultado:
  Custo Total: R$ 25.00
  Caminho: 1 → 2 → 5 → 11 → 13 → 15
----------------------------------------------------------------------
```

## Autores

**Entrega Máxima Logística S.A. - Projeto Acadêmico**

Disciplina de Teoria dos Grafos  
Implementação: C# .NET 9  
Data: Dezembro 2025

## Licença

Projeto acadêmico - Todos os direitos reservados

---

## Links Rápidos

- 📘 **[Projeto](Docs/Projeto.md)** - Especificações técnicas completas
- 🏗️ **[Estrutura](Docs/Estrutura.md)** - Arquitetura e organização do código
- 🧮 **[Algoritmos](Docs/Algoritmos.md)** - Detalhes de implementação
- 📊 **[Logging](Docs/Logging.md)** - Sistema de registro de operações

---

**Complexidades em Resumo:**
- Dijkstra: O(E + V log V) | Edmonds-Karp: O(V E²) | Kruskal: O(E log E)
- Welsh-Powell: O(V²) | Fleury: O(E²) | Hamiltoniano: Exponencial
