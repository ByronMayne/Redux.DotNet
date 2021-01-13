using ReduxSharp.Activation;
using ReduxSharp.Logging;
using ReduxSharp.Middleware;
using ReduxSharp.Reducers;
using System;
using System.Collections.Generic;

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
            where TLogType : ILogger;

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
            where TLogType : ILogger
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
        /// <typeparam name="TOptionsType">The type of the options object</typeparam>
        /// <returns>The current store configuration</returns>
        IStoreConfiguration<T> UseActivator<TActivatorType, TOptionsType>(ConfigurationDelegate<TOptionsType> options)
            where TOptionsType : new();

        /// <summary>
        /// Adds a new redurcer to the store. The applcation should have at least one redurcer defined otherwise nothing
        /// will have an effect. 
        /// </summary>
        /// <typeparam name="TType">The type of the reducer</typeparam>
        /// <returns>The current store configuration</returns>
        IStoreConfiguration<T> UseReducer<TType>()
            where TType : IAbstractReducer<T>;

        /// <summary>
        /// Adds a new reducer to the store with the options of passing in a configuration delete that will be invoked before it's created. 
        /// </summary>
        /// <typeparam name="TType">The type of the reducer</typeparam>
        /// <typeparam name="TOptionsType">The type of the options object</typeparam>
        /// <param name="options">The options to send to it</param>
        /// <returns>The current store configuration</returns>
        IStoreConfiguration<T> UseReducer<TType, TOptionsType>(ConfigurationDelegate<TOptionsType> options)
            where TOptionsType : new()
            where TType : IAbstractReducer<T>;

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
