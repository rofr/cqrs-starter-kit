using Cafe;
using CafeReadModels;
using OrigoDB.Core;
using OrigoDB.Core.Test;

namespace WebFrontend
{

    /// <summary>
    /// Domain facade 
    /// </summary>
    public static class Domain
    {
        public static CommandDispatcher Dispatcher;
        public static IOpenTabQueries OpenTabQueries;
        public static IChefTodoListQueries ChefTodoListQueries;

        public static void Setup()
        {
            var config = EngineConfiguration.Create().WithInMemoryStore();
            var engine = Engine.For<CafeModel>(config);

            var adapter = new QueryAdapter(engine);
            OpenTabQueries = adapter;
            ChefTodoListQueries = adapter;

            Dispatcher = new CommandDispatcher(engine);
        }
    }

    /// <summary>
    /// Replaces the Edument.CQRS examples MessageDispatcher
    /// </summary>
    public class CommandDispatcher
    {
        private readonly IEngine<CafeModel> _engine;

        public CommandDispatcher(IEngine<CafeModel> engine)
        {
            _engine = engine;
        }

        public void SendCommand(Command<CafeModel> command)
        {
            _engine.Execute(command);
        }
    }

}