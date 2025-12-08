# Como Usar o Sistema

## Passo 1: Instalar o .NET 9 SDK

Se você ainda não tem o .NET SDK instalado:

```bash
# Ubuntu/Debian
wget https://dot.net/v1/dotnet-install.sh -O dotnet-install.sh
chmod +x dotnet-install.sh
./dotnet-install.sh --version latest

# Ou siga as instruções oficiais em:
# https://dotnet.microsoft.com/download/dotnet/9.0
```

## Passo 2: Compilar o Projeto

```bash
cd /home/gabrielabn/projects/puc/grafos/trabalho
dotnet build
```

Saída esperada:
```
Build succeeded.
    0 Warning(s)
    0 Error(s)
```

## Passo 3: Executar a Aplicação

```bash
dotnet run
```

Você verá o menu principal:

```
╔═══════════════════════════════════════════════════════════╗
║          ENTREGA MÁXIMA LOGÍSTICA S.A.                   ║
║          Sistema de Otimização de Malha de Distribuição  ║
╚═══════════════════════════════════════════════════════════╝
```

## Passo 4: Usar os Algoritmos

### Exemplo 1: Carregar um Grafo

```
Escolha uma opção: 1
Digite o caminho do arquivo DIMACS: TestData/sample_graph_1.dimacs
```

### Exemplo 2: Encontrar Caminho Mínimo (Dijkstra)

```
Escolha uma opção: 2
Digite o ID do hub de origem: 1
Digite o ID do hub de destino: 5
```

Saída:
```
═══════════════════════════════════════════════════════════
  RESULTADO: Rota de Menor Custo (Dijkstra)
═══════════════════════════════════════════════════════════

✓ Custo Total: R$ 6.00

Caminho:
  1. Hub 1 → Hub 4 (Custo: R$ 2.00)
  2. Hub 4 → Hub 5 (Custo: R$ 3.00)
```

### Exemplo 3: Calcular Fluxo Máximo (Edmonds-Karp)

```
Escolha uma opção: 3
Digite o ID do hub de origem (fonte): 1
Digite o ID do hub de destino (sumidouro): 5
```

Use o arquivo `TestData/sample_graph_2_with_capacity.dimacs` que contém valores de capacidade.

### Exemplo 4: Árvore Geradora Mínima (Kruskal)

```
Escolha uma opção: 4
```

Saída:
```
═══════════════════════════════════════════════════════════
  RESULTADO: Expansão da Rede - Árvore Geradora Mínima
═══════════════════════════════════════════════════════════

✓ Custo Total de Instalação: R$ 12.00

Conexões Necessárias (4 rotas):
  • Hub 3 ↔ Hub 4 (Custo: R$ 1.00)
  • Hub 1 ↔ Hub 2 (Custo: R$ 2.00)
  • Hub 1 ↔ Hub 4 (Custo: R$ 2.00)
  • Hub 4 ↔ Hub 5 (Custo: R$ 3.00)
```

### Exemplo 5: Agendamento de Manutenções (Welsh-Powell)

```
Escolha uma opção: 5
```

O sistema automaticamente gera conflitos baseado em rotas que compartilham hubs.

### Exemplo 6: Rota Euleriana

```
Escolha uma opção: 6
```

Verifica se é possível percorrer todas as rotas exatamente uma vez.

### Exemplo 7: Ciclo Hamiltoniano

```
Escolha uma opção: 7
Digite o timeout em segundos (padrão: 10): 10
```

Busca um ciclo que visita todos os hubs exatamente uma vez.

## Arquivos de Teste Disponíveis

### TestData/sample_graph_1.dimacs
- Grafo básico com 5 hubs e 6 rotas
- Baseado no exemplo da imagem fornecida
- Bom para testes rápidos de todos os algoritmos

### TestData/sample_graph_2_with_capacity.dimacs
- Inclui valores de capacidade (4ª coluna)
- Ideal para testar o algoritmo de Fluxo Máximo
- 5 hubs, 7 rotas

### TestData/sample_graph_3_mst.dimacs
- Grafo maior com 6 hubs e 9 rotas
- Útil para testar MST e performance

## Usar Seus Próprios Grafos

Crie um arquivo no formato DIMACS:

```
V E
origem1 destino1 custo1 [capacidade1]
origem2 destino2 custo2 [capacidade2]
...
```

Exemplo:
```
3 3
1 2 10 5
2 3 20 8
1 3 15 10
```

- Primeira linha: `3` vértices, `3` arestas
- Capacidade é opcional (use para max flow)
- IDs de vértices devem ser sequenciais começando em 1

## Dicas

1. **Para Max Flow:** Certifique-se de incluir capacidades no arquivo DIMACS
2. **Para Hamiltoniano:** Grafos grandes podem exceder o timeout - ajuste conforme necessário
3. **Para Coloração:** O algoritmo gera automaticamente conflitos baseado em hubs compartilhados
4. **Debugging:** Use os grafos pequenos em TestData/ primeiro para validar seus dados

## Solução de Problemas

### "dotnet: command not found"
- Instale o .NET 9 SDK (veja Passo 1)

### "File not found"
- Certifique-se de usar caminhos relativos ou absolutos corretos
- Exemplo: `TestData/sample_graph_1.dimacs` (relativo)
- Ou: `/home/gabrielabn/projects/puc/grafos/trabalho/TestData/sample_graph_1.dimacs` (absoluto)

### "Invalid DIMACS format"
- Verifique se a primeira linha tem o formato `V E`
- Verifique se cada aresta tem pelo menos 3 valores: `origem destino custo`
- IDs de vértices devem estar entre 1 e V

### Algoritmo Hamiltoniano não encontra solução
- É um problema NP-completo
- Nem todos os grafos têm ciclos hamiltonianos
- Tente aumentar o timeout se o grafo for grande
