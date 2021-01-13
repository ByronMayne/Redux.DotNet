using System.Threading.Tasks;

namespace ReduxSharp
{
    public delegate Task ActionDelegate<TState>(ActionContext<TState> context);
    
    public interface IActionContext
    {
        public IAction Action { get; }

        public IStore Store { get; }

        public object Result { get; }
    }

    public interface IActionContext<TState> : IActionContext
    {
        public new IStore<TState> Store { get; }

        public new TState Result { get; }
    }

    public class ActionContext<TState> : IActionContext<TState>
    {
        public IAction Action { get; init; }

        public IStore<TState> Store { get; init; }

        public TState Result { get; set; }

        IStore IActionContext.Store => Store;
        object IActionContext.Result => Result;
    }
}
