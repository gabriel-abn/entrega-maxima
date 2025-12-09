# Sistema de Logging de Consultas

## Visão Geral

O sistema implementa logging automático de todas as operações realizadas em grafos carregados. Cada arquivo de grafo carregado gera seu próprio arquivo de log com timestamp.

## Formato do Nome do Arquivo de Log

```
<nome_do_arquivo>_<data>.log
```

**Exemplos:**
- Arquivo: `grafo01.dimacs`
- Log: `grafo01_20251208.log`

- Arquivo: `grafos/grafo08.dimacs`
- Log: `grafo08_20251208.log`

## Localização dos Logs

Todos os arquivos de log são salvos no diretório `logs/` na raiz do projeto.

## Comportamento do Logger

### Início de Sessão
- **Quando**: Ao carregar um grafo (opção 1 do menu)
- **Ação**: 
  - Fecha log anterior (se houver)
  - Cria novo arquivo de log com nome datado
  - Registra informações do grafo carregado

### Durante a Sessão
Cada execução de algoritmo é registrada com:
- Nome do algoritmo
- Timestamp da execução
- Parâmetros utilizados
- Resumo dos resultados

### Fim de Sessão
- **Quando**: Carregar novo arquivo OU sair do programa (opção 8)
- **Ação**: Fecha arquivo de log atual

## Algoritmos Registrados

### 1. Dijkstra (Roteamento de Menor Custo)
**Registro:**
```
Algoritmo: Dijkstra - Roteamento de Menor Custo
Timestamp: 2025-12-08 22:00:00
Parâmetros: Origem: 1, Destino: 5
Resultado:
  Custo Total: R$ 15.00
  Número de saltos: 3
  Caminho: 1 → 2 → 4 → 5
```

### 2. Edmonds-Karp (Capacidade Máxima)
**Registro:**
```
Algoritmo: Edmonds-Karp - Capacidade Máxima de Escoamento
Timestamp: 2025-12-08 22:01:30
Parâmetros: Origem: 1, Destino: 6
Resultado:
  Fluxo Máximo: 33.00 toneladas
  Arestas críticas (gargalos): 4
```

### 3. Kruskal (MST)
**Registro:**
```
Algoritmo: Kruskal - Expansão da Rede (MST)
Timestamp: 2025-12-08 22:02:15
Resultado:
  Custo Total: R$ 45.00
  Número de conexões: 14
```

### 4. Welsh-Powell (Agendamento)
**Registro:**
```
Algoritmo: Welsh-Powell - Agendamento de Manutenções
Timestamp: 2025-12-08 22:03:00
Parâmetros: Conflitos detectados: 120
Resultado:
  Número de turnos necessários: 5
  Turno 0: 8 rotas
  Turno 1: 9 rotas
  ...
```

### 5. Euleriano (Fleury)
**Registro:**
```
Algoritmo: Euleriano (Fleury) - Rota de Inspeção
Timestamp: 2025-12-08 22:04:00
Resultado:
  Caminho euleriano encontrado com 45 arestas
```

### 6. Hamiltoniano (Backtracking)
**Registro:**
```
Algoritmo: Hamiltoniano (Backtracking) - Rota de Inspeção
Timestamp: 2025-12-08 22:05:00
Parâmetros: Timeout: 10s
Resultado:
  Ciclo hamiltoniano encontrado visitando 15 vértices
```

## Estrutura do Arquivo de Log

### Cabeçalho da Sessão
```
----------------------------------------------------------------------
Sessão iniciada: 2025-12-08 22:00:00
Arquivo carregado: grafos/grafo01.dimacs
----------------------------------------------------------------------
Informações do Grafo:
  - Vértices: 6
  - Arestas: 12
```

### Registro de Operação
```
----------------------------------------------------------------------
Algoritmo: [Nome do Algoritmo]
Timestamp: [Data e Hora]
Parâmetros: [Parâmetros usados]
Resultado:
  [Resumo dos resultados]
----------------------------------------------------------------------
```

### Rodapé da Sessão
```
----------------------------------------------------------------------
Sessão encerrada: 2025-12-08 22:10:00
----------------------------------------------------------------------
```

## Exemplo de Log Completo

```
----------------------------------------------------------------------
Sessão iniciada: 2025-12-08 22:00:00
Arquivo carregado: grafos/grafo08.dimacs
----------------------------------------------------------------------
Informações do Grafo:
  - Vértices: 15
  - Arestas: 45
----------------------------------------------------------------------
Algoritmo: Dijkstra - Roteamento de Menor Custo
Timestamp: 2025-12-08 22:00:30
Parâmetros: Origem: 1, Destino: 15
Resultado:
  Custo Total: R$ 25.00
  Número de saltos: 5
  Caminho: 1 → 2 → 5 → 11 → 13 → 15
----------------------------------------------------------------------
Algoritmo: Euleriano (Fleury) - Rota de Inspeção
Timestamp: 2025-12-08 22:01:15
Resultado:
  Caminho euleriano encontrado com 45 arestas
----------------------------------------------------------------------
Sessão encerrada: 2025-12-08 22:05:00
----------------------------------------------------------------------
```

## Implementação Técnica

### Classe Principal: `QueryLogger`

**Localização:** `Utils/QueryLogger.cs`

**Métodos Principais:**

1. **`StartLogging(string graphFileName)`**
   - Fecha log anterior
   - Cria novo arquivo de log
   - Registra cabeçalho

2. **`LogGraphInfo(int nodeCount, int edgeCount)`**
   - Registra informações do grafo carregado

3. **`LogAlgorithmExecution(string name, string params, string result)`**
   - Registra execução de algoritmo com timestamp

4. **`LogError(string error)`**
   - Registra erros encontrados

5. **`CloseCurrentLog()`**
   - Fecha arquivo de log atual
   - Registra rodapé

### Integração no Program.cs

```csharp
private static QueryLogger _logger = new QueryLogger();

// Ao carregar grafo
_logger.StartLogging(filePath);
_logger.LogGraphInfo(_currentGraph.NodeCount, _currentGraph.EdgeCount);

// Ao executar algoritmo
_logger.LogAlgorithmExecution(
    "Nome do Algoritmo",
    "Parâmetros",
    "Resultado formatado");

// Ao encerrar
_logger.Dispose();
```

## Múltiplas Sessões no Mesmo Dia

Se o mesmo arquivo for carregado múltiplas vezes no mesmo dia, os logs serão **acumulados** no mesmo arquivo (modo append).

**Exemplo:**
```
Sessão 1 (10:00):
----------------------------------------------------------------------
Sessão iniciada: 2025-12-08 10:00:00
...
Sessão encerrada: 2025-12-08 10:05:00
----------------------------------------------------------------------

Sessão 2 (15:30):
----------------------------------------------------------------------
Sessão iniciada: 2025-12-08 15:30:00
...
Sessão encerrada: 2025-12-08 15:45:00
----------------------------------------------------------------------
```

## Considerações

1. **Diretório logs/**: Criado automaticamente se não existir
2. **Performance**: Flush após cada operação para garantir persistência
3. **Encoding**: UTF-8 padrão
4. **Separadores**: 70 caracteres de traço para visibilidade
5. **Timestamp**: Formato ISO: `yyyy-MM-dd HH:mm:ss`

## Uso Prático

### Análise de Performance
Os logs permitem rastrear:
- Quais algoritmos foram executados
- Com quais parâmetros
- Resultados obtidos
- Histórico de consultas

### Auditoria
- Rastreabilidade completa de operações
- Histórico datado de análises
- Verificação de resultados anteriores

### Debug
- Identificação de padrões de uso
- Verificação de resultados anteriores
- Análise de erros
