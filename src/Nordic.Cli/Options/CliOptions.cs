using Microsoft.Extensions.Logging;
using TeleScope.UI.Cli.Options;

namespace Nordic.Cli.Options
{
    /// <summary>
    /// This class contains all options for command line arguments
    /// </summary>
    public class CliOptions
    {
        // own cli properties

        [Cli(Short = "v", Long = "verbose")]
        public bool Verbose { get; set; }

        [Cli(Short = "b", Long = "break")]
        public bool Break { get; set; }

        [Cli(Short = "w", Long = "workspace")]
        public string Workspace { get; set; }

        [Cli(Short = "t", Long = "runtime")]
        public string RuntimeFile { get; set; }

        [Cli(Short = "d", Long = "devices")]
        public string DevicesFile { get; set; }

        [Cli(Short = "a", Long = "antenna")]
        public string AntennaFile { get; set; }

        [Cli(Short = "r", Long = "radio")]
        public string ChannelFile { get; set; }

        [Cli(Short = "c", Long = "comm")]
        public string CommunicationlFile { get; set; }

        [Cli(Short = "e", Long = "energy")]
        public string EnergyFile { get; set; }

        [Cli(Short = "s", Long = "scene")]
        public string SceneFile { get; set; }

        // -- constructors

        public CliOptions()
        {
            Workspace = "";
        }

        // -- methods

        public LogLevel GetLogLevel()
		{
            if (Verbose)
            {
                return LogLevel.Trace;
            }
            else
            {
#if DEBUG
				return LogLevel.Debug;
#else
                return LogLevel.Information;
#endif
            }
        }
    }
}
