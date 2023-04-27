using Serilog;

namespace Hdeleon_Facturacion
{
    public static class SerilogClass
    {
        public static readonly ILogger _log;
        static SerilogClass()
        {
            _log = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File(@"c:\temp\cosmixLogs.txt")
                .CreateLogger();
        }
    }
}