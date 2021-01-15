using System.Threading.Tasks;

namespace ReduxSharp
{
    /// <summary>
    /// Invoked when an instance is being created to allow you to configure some options for the type.
    /// </summary>
    public delegate void ConfigurationDelegate<T>(T options);

    /// <summary>
    /// Used to process actions and in most cause should just be awaited. 
    /// </summary>
    /// <param name="actionContext">The current action context</param>
    /// <returns>A task to await on</returns>
    public delegate Task ActionDispatchDelegate(IActionContext actionContext);

    /// <summary>
    /// Used to capture a member of the state object and return back a sub section. 
    /// </summary>
    /// <typeparam name="TState">The current state</typeparam>
    /// <typeparam name="TSectionType">The type of the section that is being returned.</typeparam>
    /// <param name="state">The current state object</param>
    /// <returns>The isntance of the section</returns>
    public delegate TSectionType StateSubSectionSelectionDelegate<TState, TSectionType>(TState state);

}
