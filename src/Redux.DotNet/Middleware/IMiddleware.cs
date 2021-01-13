using System.Threading;
using System.Threading.Tasks;

namespace ReduxSharp.Middleware
{
    public interface IAbstractMiddleware
    { }


    public interface IMiddleware<TSTate> : IAbstractMiddleware
    {
        /// <summary>
        /// Invoked once for every action being preformed. 
        /// </summary>
        /// <param name="actionContext">The action context</param>
        /// <returns>A task to await apon</returns>
        Task InvokeAsync(IActionContext<TSTate> actionContext);
    }


    /// <summary>
    /// Defines an class that can act as middleware 
    /// </summary>
    public interface IMiddleware : IAbstractMiddleware
    {
        /// <summary>
        /// Invoked once for every action being preformed. 
        /// </summary>
        /// <param name="actionContext">The action context</param>
        /// <returns>A task to await apon</returns>
        Task InvokeAsync(IActionContext actionContext);
    }


}
