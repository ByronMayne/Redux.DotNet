using ReduxSharp.Activation;
using ReduxSharp.Logging;
using ReduxSharp.Middleware;
using ReduxSharp.Reducers;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ReduxSharp
{
    public interface IStoreConfiguration<T>
    {
        /// <summary>
        /// Gets the list of currently configured middleware 
        /// </summary>
        IReadOnlyList<Type> Middleware { get; }

        /// <summary>
        /// Changes the Redux to output to a log of this type
        /// </summary>
        /// <typeparam name="TLogType">The type of log to use</typeparam>
        IStoreConfiguration<T> UseLogger<TLogType>() 
            where TLogType : ILog;

        /// <summary>
        /// Adds an instance that can be used to resolve constructors used for activation of any types
        /// </summary>
        IStoreConfiguration<T> UseSingleton<TInstanceType, TOptionsType>(ConfigurationDelegate<TOptionsType> options = null)
            where TOptionsType : new();

        /// <summary>
        /// Adds an instance that can be used to resolve constructors used for activation of any types
        /// </summary>
        IStoreConfiguration<T> UseSingleton<TInstanceType, TBindAs, TOptionsType>(ConfigurationDelegate<TOptionsType> options = null)
            where TInstanceType : TBindAs
            where TOptionsType : new();

        /// <summary>
        /// Changes the Redux to output to a log of this type
        /// </summary>
        /// <typeparam name="TLogType">The type of log to use</typeparam>
        IStoreConfiguration<T> UseLogger<TLogType, TOptionsType>(ConfigurationDelegate<TOptionsType> options)
            where TLogType : ILog
            where TOptionsType : new();

        /// <summary>
        /// Sets the store to be initialized using this initial state. 
        /// </summary>
        /// <param name="initialState">The starting state for the store</param>
        IStoreConfiguration<T> UseInitialState(T initialState);

        /// <summary>
        /// Allows you to override the activator that will be used to created instances of the reducers, middleware and any
        /// other types that Redux Sharp needs to create. This is useful if you are using a dependency injection framework.
        /// </summary>
        /// <typeparam name="TActivatorType">The activator type to use</typeparam>
        /// <returns>The current store configuration</returns>
        IStoreConfiguration<T> UseActivator<TActivatorType>()
            where TActivatorType : IActivator;

        /// <summary>
        /// Allows you to override the activator that will be used to created instances of the reducers, middleware and any
        /// other types that Redux Sharp needs to create. This is useful if you are using a dependency injection framework. This
        /// also takes in a delegate so you can send custom options.
        /// </summary>
        /// <typeparam name="TActivatorType">The activator type to use</typeparam>
        /// <typeparam name="TOptions">The type of the options object</typeparam>
        /// <returns>The current store configuration</returns>
        IStoreConfiguration<T> UseActivator<TActivatorType, TOptions>(ConfigurationDelegate<TOptions> options)
            where TOptions : new();

        /// <summary>
        /// Adds a new redurcer to the store. The applcation should have at least one redurcer defined otherwise nothing
        /// will have an effect. 
        /// </summary>
        /// <typeparam name="TReducerType">The type of the reducer</typeparam>
        /// <returns>The current store configuration</returns>
        IStoreConfiguration<T> UseRootReducer<TReducerType>()
            where TReducerType : IAbstractReducer<T>;

        /// <summary>
        /// Adds a new reducer to the store with the options of passing in a configuration delete that will be invoked before it's created. 
        /// </summary>
        /// <typeparam name="TReducerType">The type of the reducer</typeparam>
        /// <typeparam name="TOptions">The type of the options object</typeparam>
        /// <param name="options">The options to send to it</param>
        /// <returns>The current store configuration</returns>
        IStoreConfiguration<T> UseRootReducer<TReducerType, TOptions>(ConfigurationDelegate<TOptions> options)
            where TOptions : new()
            where TReducerType : IAbstractReducer<T>;

        /// <summary>
        /// Adds a new reducer that processes a subsection of the state tree.
        /// </summary>
        /// <typeparam name="TType">The state type</typeparam>
        /// <typeparam name="TSectionType">The type the member returns</typeparam>
        /// <param name="selectionSelector">The expression for fetching the property to get and set the value.</param>
        /// <returns>The current configuration</returns>
        IStoreConfiguration<T> UseReducer<TReducerType, TSectionType>(Expression<StateSubSectionSelectionDelegate<T, TSectionType>> selectionSelector)
             where TReducerType : IReducer<TSectionType>;

        /// <summary>
        /// Adds a new reducer that can be used to process a sub section of the object state.
        /// </summary>
        /// <typeparam name="TReducerType">The current type fo state</typeparam>
        /// <typeparam name="TSectionType">The type of the sub section</typeparam>
        /// <param name="selectionSelector">The expression for fetching the property to get and set the value.</param>
        /// <returns>The current configuration</returns>
        IStoreConfiguration<T> UseReducer<TReducerType, TSectionType, TOptions>(
            Expression<StateSubSectionSelectionDelegate<T, TSectionType>> sectionSelector,
            ConfigurationDelegate<TOptions> options)
                where TOptions : new()
                where TReducerType : IReducer<TSectionType>;

        /// <summary>
        /// Adds the given middleware to the list 
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <returns></returns>
        IStoreConfiguration<T> UseMiddleware<TType>()
            where TType : IAbstractMiddleware;

        /// <summary>
        /// Adds the given middle ware to the list with an options object to initialize the data
        /// </summary>
        /// <typeparam name="TType">The type of the middlware</typeparam>
        /// <typeparam name="TOptionsType">The type of the options object</typeparam>
        /// <param name="options">The delegate used to setup the configuration for the middlware once it's created</param>
        /// <returns>The current store configuration</returns>
        IStoreConfiguration<T> UseMiddleware<TType, TOptionsType>(ConfigurationDelegate<TOptionsType> options)
            where TOptionsType : new()
            where TType : IAbstractMiddleware;

        /// <summary>
        /// Creates a new IStore<typeparamref name="T"/> from the current settings and sets the initial sto
        /// from the builder.
        /// </summary>
        /// <returns>The newly created store</returns>
        IStore<T> CreateStore();

        /// <summary>
        /// Allows you to create a store that is a different type
        /// </summary>
        TStoreType CreateStore<TStoreType>() where TStoreType : IStore<T>;
    }
}
