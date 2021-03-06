﻿using System;
using Serilog;
using Serilog.Events;

namespace Harness
{
    class Program
    {
        static void Main()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel(LogEventLevel.Debug)
                .WithConsoleSink(
                    outputTemplate: "{TimeStamp:HH:mm:ss} ({ThreadId}) [{Level}] {Message:l}{NewLine:l}{Exception:l}")
                .WithDumpFileSink("Dumps\\" + DateTime.Now.Ticks + ".slog")
                .WithDiagnosticTraceSink()
                .EnrichedWithProperty("App", "Test Harness")
                .EnrichedBy(new ThreadIdEnricher(),
                            new HostNameEnricher())
                .CreateLogger();

            Log.Information("Just biting {Fruit} number {Count}", "Apple", 12);
            Log.ForContext<Program>().Information("Just biting {Fruit} number {Count:0000}", "Apple", 12);
            
            // ReSharper disable CoVariantArrayConversion
            Log.Information("I've eaten {Dinner}", new[] { "potatoes", "peas" });
            // ReSharper restore CoVariantArrayConversion
            
            Log.Information("I sat at {@Chair}", new { Back = "straight", Legs = new[] { 1, 2, 3, 4 } });
            Log.Information("I sat at {Chair}", new { Back = "straight", Legs = new[] { 1, 2, 3, 4 } });

            var context = Log.Logger.ForContext("MessageId", 567);
            try
            {
                context.Information("Processing a message");
                throw new NotImplementedException("Nothing doing.");
            }
            catch (Exception ex)
            {
                context.Error(ex, "Rolling back transaction!");
            }
            
            Console.ReadKey(true);
        }
    }
}
