using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

internal class Program
{
    private static void Main(string[] args)
    {
        var host = new HostBuilder()
        .ConfigureFunctionsWorkerDefaults()
        .ConfigureServices(services =>
        {
            services.AddApplicationInsightsTelemetryWorkerService();
            services.ConfigureFunctionsApplicationInsights();
            services.AddSingleton<IHighCPUService, HighCPUService>();
        })
        .Build();

        host.Run();
    }
}

public class HighCPUService : IHighCPUService
{
    private bool _isRunning = false;
    private static CancellationTokenSource cts;

    public void StartHighCPU()
    {
        if (_isRunning)
        {
            return;
        }

        _isRunning = true;

        try
        {

            while (!cts.Token.IsCancellationRequested)
            {
                Console.WriteLine("Thread is running...");

                int numCores = Environment.ProcessorCount;

                Console.WriteLine($"Detected {numCores} processor cores.");

                // Create tasks to run CPU-intensive work on each core
                Task[] tasks = new Task[numCores];
                for (int i = 0; i < numCores; i++)
                {
                    tasks[i] = Task.Factory.StartNew(() =>
                    {
                        // Simulate a CPU-intensive operation
                        while (true) { }
                    }, TaskCreationOptions.LongRunning);
                }

                // Wait for user input to stop the CPU-intensive work
                Console.WriteLine("Press Enter to stop the CPU-intensive tasks...");
                Console.ReadLine();

                // Stop all tasks
                foreach (var task in tasks)
                {
                    task.Dispose(); // Dispose to release resources
                }
            }
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Thread has been canceled.");
        }
        finally
        {
            _isRunning = false;
        }
    }
}

public interface IHighCPUService
{
    public void StartHighCPU();
}