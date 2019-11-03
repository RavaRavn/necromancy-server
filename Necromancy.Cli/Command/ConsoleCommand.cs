using Arrowgene.Services.Logging;

namespace Necromancy.Cli.Command
{
    public abstract class ConsoleCommand : IConsoleCommand
    {
        protected ConsoleCommand()
        {
            Logger = LogProvider.Logger(this);
        }

        protected readonly ILogger Logger;

        public abstract string Key { get; }
        public abstract string Description { get; }
        public abstract CommandResultType Handle(ConsoleParameter parameter);

        public virtual void Shutdown()
        {
        }
    }
}
