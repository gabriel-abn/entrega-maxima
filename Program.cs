using LogisticsOptimization.Models;
using LogisticsOptimization.Algorithms;
using LogisticsOptimization.Algorithms.Results;
using LogisticsOptimization.Utils;

namespace LogisticsOptimization;

class Program
{
    private static LogisticsGraph? _currentGraph;
    private static string? _currentFileName;

    static void Main(string[] args)
    {
        Console.Clear();
        PrintWelcomeBanner();

        while (true)
        {
            PrintMenu();
            
            Console.Write("\nEscolha uma opção: ");
            string? choice = Console.ReadLine();

            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    LoadGraphFromFile();
                    break;
                case "2":
                    RunDijkstra();
                    break;
                case "3":
                    RunEdmondsKarp();
                    break;
                case "4":
                    RunKruskal();
                    break;
                case "5":
                    RunWelshPowell();
                    break;
                case "6":
                    RunEulerian();
                    break;
                case "7":
                    RunHamiltonian();
                    break;
                case "8":
                    Console.WriteLine("Encerrando o sistema...");
                    return;
                default:
                    OutputFormatter.PrintError("Opção inválida. Tente novamente.");
                    break;
            }

            if (choice != "8")
            {
                Console.WriteLine("\nPressione qualquer tecla para continuar...");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }

    static void PrintWelcomeBanner()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(@"
╔═══════════════════════════════════════════════════════════════════╗
║                                                                   ║
║          ENTREGA MÁXIMA LOGÍSTICA S.A.                           ║
║          Sistema de Otimização de Malha de Distribuição          ║
║                                                                   ║
╚═══════════════════════════════════════════════════════════════════╝
        ");
        Console.ResetColor();
    }

    static void PrintMenu()
    {
        Console.WriteLine("╔═══════════════════════════════════════════════════════════╗");
        Console.WriteLine("║                    MENU PRINCIPAL                         ║");
        Console.WriteLine("╠═══════════════════════════════════════════════════════════╣");
        
        if (_currentGraph != null)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"║  Grafo Carregado: {_currentFileName,-39} ║");
            Console.WriteLine($"║  Hubs: {_currentGraph.NodeCount,-3}  Rotas: {_currentGraph.EdgeCount,-3}                                  ║");
            Console.ResetColor();
            Console.WriteLine("╠═══════════════════════════════════════════════════════════╣");
        }
        
        Console.WriteLine("║                                                           ║");
        Console.WriteLine("║  1. Carregar Grafo (DIMACS)                               ║");
        Console.WriteLine("║                                                           ║");
        Console.WriteLine("║  ALGORITMOS DE OTIMIZAÇÃO:                                ║");
        Console.WriteLine("║  2. Roteamento de Menor Custo (Dijkstra)                 ║");
        Console.WriteLine("║  3. Capacidade Máxima de Escoamento (Edmonds-Karp)       ║");
        Console.WriteLine("║  4. Expansão da Rede - MST (Kruskal)                      ║");
        Console.WriteLine("║  5. Agendamento de Manutenções (Welsh-Powell)             ║");
        Console.WriteLine("║  6. Rota de Inspeção - Euleriano                          ║");
        Console.WriteLine("║  7. Rota de Inspeção - Hamiltoniano                       ║");
        Console.WriteLine("║                                                           ║");
        Console.WriteLine("║  8. Sair                                                  ║");
        Console.WriteLine("║                                                           ║");
        Console.WriteLine("╚═══════════════════════════════════════════════════════════╝");
    }

    static void LoadGraphFromFile()
    {
        Console.Write("Digite o caminho do arquivo DIMACS: ");
        string? filePath = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(filePath))
        {
            OutputFormatter.PrintError("Caminho inválido.");
            return;
        }

        try
        {
            _currentGraph = DimacsParser.LoadFromFile(filePath);
            _currentFileName = Path.GetFileName(filePath);
            
            OutputFormatter.PrintSuccess($"Grafo carregado com sucesso!");
            OutputFormatter.PrintGraphInfo(_currentGraph);
        }
        catch (Exception ex)
        {
            OutputFormatter.PrintError($"Erro ao carregar arquivo: {ex.Message}");
            _currentGraph = null;
            _currentFileName = null;
        }
    }

    static void RunDijkstra()
    {
        if (!CheckGraphLoaded()) return;

        Console.Write("Digite o ID do hub de origem: ");
        if (!int.TryParse(Console.ReadLine(), out int source))
        {
            OutputFormatter.PrintError("ID inválido.");
            return;
        }

        Console.Write("Digite o ID do hub de destino: ");
        if (!int.TryParse(Console.ReadLine(), out int target))
        {
            OutputFormatter.PrintError("ID inválido.");
            return;
        }

        var result = DijkstraAlgorithm.FindShortestPath(_currentGraph!, source, target);
        OutputFormatter.PrintDijkstraResult(result);
    }

    static void RunEdmondsKarp()
    {
        if (!CheckGraphLoaded()) return;

        Console.Write("Digite o ID do hub de origem (fonte): ");
        if (!int.TryParse(Console.ReadLine(), out int source))
        {
            OutputFormatter.PrintError("ID inválido.");
            return;
        }

        Console.Write("Digite o ID do hub de destino (sumidouro): ");
        if (!int.TryParse(Console.ReadLine(), out int sink))
        {
            OutputFormatter.PrintError("ID inválido.");
            return;
        }

        var result = EdmondsKarpAlgorithm.CalculateMaxFlow(_currentGraph!, source, sink);
        OutputFormatter.PrintMaxFlowResult(result);
    }

    static void RunKruskal()
    {
        if (!CheckGraphLoaded()) return;

        var result = KruskalAlgorithm.FindMinimumSpanningTree(_currentGraph!);
        OutputFormatter.PrintMSTResult(result);
    }

    static void RunWelshPowell()
    {
        if (!CheckGraphLoaded()) return;

        Console.WriteLine("Gerando conflitos automaticamente (rotas que compartilham hubs)...");
        
        var conflicts = WelshPowellAlgorithm.GenerateConflictsFromSharedNodes(_currentGraph!);
        var result = WelshPowellAlgorithm.ScheduleMaintenanceShifts(_currentGraph!, conflicts);
        
        OutputFormatter.PrintColoringResult(result);
    }

    static void RunEulerian()
    {
        if (!CheckGraphLoaded()) return;

        var result = EulerianAlgorithm.FindEulerianPath(_currentGraph!);
        OutputFormatter.PrintEulerianResult(result);
    }

    static void RunHamiltonian()
    {
        if (!CheckGraphLoaded()) return;

        Console.Write("Digite o timeout em segundos (padrão: 10): ");
        string? timeoutStr = Console.ReadLine();
        int timeout = 10;
        
        if (!string.IsNullOrWhiteSpace(timeoutStr))
        {
            if (!int.TryParse(timeoutStr, out timeout) || timeout <= 0)
            {
                timeout = 10;
            }
        }

        OutputFormatter.PrintWarning($"Buscando ciclo hamiltoniano (timeout: {timeout}s)...");
        OutputFormatter.PrintInfo("Este é um problema NP-completo e pode levar tempo.");
        Console.WriteLine();

        var result = HamiltonianAlgorithm.FindHamiltonianCycle(_currentGraph!, timeout);
        OutputFormatter.PrintHamiltonianResult(result);
    }

    static bool CheckGraphLoaded()
    {
        if (_currentGraph == null)
        {
            OutputFormatter.PrintError("Nenhum grafo carregado. Por favor, carregue um arquivo DIMACS primeiro (opção 1).");
            return false;
        }
        return true;
    }
}
