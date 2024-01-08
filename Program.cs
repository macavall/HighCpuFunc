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

    public void CancelHighCPU()
    {
        _isRunning = false;
    }

    public bool getIsRunning()
    {
        return _isRunning;
    }

    public async Task StartHighCPU()
    {
        if (_isRunning)
        {
            return;
        }

        _isRunning = true;

        try
        {
            Console.WriteLine("High CPU load started.");

            int numCores = Environment.ProcessorCount;
            Task[] tasks = new Task[numCores];

            for (int i = 0; i < numCores; i++)
            {
                tasks[i] = Task.Run(() =>
                {
                    while (_isRunning)
                    {
                        // Simulate CPU-intensive work
                        //Task.Delay(10);
                    }
                });
            }

            await Task.WhenAll(tasks);
        }
        finally
        {
            Console.WriteLine("High CPU load stopped.");
        }
    }
}

public interface IHighCPUService
{
    public Task StartHighCPU();
    public void CancelHighCPU();
    public bool getIsRunning();
}