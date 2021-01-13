#nullable enable

using ReduxSharp.Activation;
using ReduxSharp.Activation.IOC;
using ReduxSharp.Exceptions;
using ReduxSharp.Logging;
using ReduxSharp.Middleware;
using ReduxSharp.Reducers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReduxSharp
{
    /// <summary>
    /// A delegates used to configure different options
    /// </summary>
    public delegate void ConfigurationDelegate<T>(T options);

    public delegate Task ActionDispatchDelegate(IActionContext actionContext);

    /// <summary>
    /// Configuration object for creating <see cref="IStore{TState}"/> instances.
    /// </summary>
    public class StoreConfiguration<TState> : IStoreConfiguration<TState> where TState : class
    {
        private TState? m_initialState;
        private ITypeRequest? m_loggerTypeRequest;
        private ITypeRequest? m_activatorTypeRequest;
        private readonly List<ITypeRequest> m_middleware;
        private readonly List<ITypeRequest> m_reducers;
        private readonly List<ITypeRequest> m_singletonTypeRequests;

        /// <inheritdoc cref="IStoreConfiguration{T}"/>
        public IReadOnlyList<Type> Middleware => m_middleware.Select(m => m.Type).ToArray();

        /// <summary>
        /// Creates a new instance of a store configuration
        /// </summary>
        public StoreConfiguration()
        {
            m_initialState = default(TState);
            m_middleware = new List<ITypeRequest>();
            m_reducers = new List<ITypeRequest>();
            m_singletonTypeRequests = new List<ITypeRequest>();
            m_activatorTypeRequest = null;

            this.UseDefaultLogger();
            this.UseDefaultActivator();
        }

        /// <inheritdoc cref="IStoreConfiguration{T}"/>
        public IStoreConfiguration<TState> UseInitialState(TState initialState)
        {
            m_initialState = initialState;
            return this;
        }

        /// <inheritdoc cref="IStoreConfiguration{T}"/>
        public IStoreConfiguration<TState> UseMiddleware<TType>()
            where TType : IAbstractMiddleware
        {
            m_middleware.Add(new TypeRequest(typeof(TType)));
            return this;
        }

        /// <inheritdoc cref="IStoreConfiguration{T}"/>
        public IStoreConfiguration<TState> UseMiddleware<TType, TOptionsType>(ConfigurationDelegate<TOptionsType>? options = null)
            where TOptionsType : new()
            where TType : IAbstractMiddleware
        {
            IParameter parameter = CreateFactoryParameter(options);
            m_middleware.Add(new TypeRequest(typeof(TType), parameter));
            return this;
        }

        /// <inheritdoc cref="IStoreConfiguration{T}"/>
        public IStoreConfiguration<TState> UseReducer<TType>()
            where TType : IAbstractReducer<TState>
        {
            m_reducers.Add(new TypeRequest(typeof(TType)));
            return this;
        }

        /// <inheritdoc cref="IStoreConfiguration{T}"/>
        public IStoreConfiguration<TState> UseReducer<TType, TOptionsType>(ConfigurationDelegate<TOptionsType>? options = null)
            where TOptionsType : new()
            where TType : IAbstractReducer<TState>
        {
            IParameter parameter = CreateFactoryParameter(options);
            m_reducers.Add(new TypeRequest(typeof(TType), parameter));
            return this;
        }

        /// <inheritdoc cref="IStoreConfiguration{T}"/>
        public IStoreConfiguration<TState> UseActivator<TActivatorType>()
            where TActivatorType : IActivator
        {
            m_activatorTypeRequest = new TypeRequest(typeof(TActivatorType));
            return this;
        }

        /// <inheritdoc cref="IStoreConfiguration{T}"/>
        public IStoreConfiguration<TState> UseActivator<TActivatorType, TOptionsType>(ConfigurationDelegate<TOptionsType>? options = null)
            where TOptionsType : new()
        {
            IParameter parameter = CreateFactoryParameter(options);
            m_activatorTypeRequest = new TypeRequest(typeof(TActivatorType), parameter);
            return this;
        }

        /// <inheritdoc cref="IStoreConfiguration{T}"/>
        public IStoreConfiguration<TState> UseLogger<TLogType>() where TLogType : ILog
        {
            m_loggerTypeRequest = new TypeRequest(typeof(TLogType), typeof(ILog));
            return this;
        }

        /// <inheritdoc cref="IStoreConfiguration{T}"/>
        public IStoreConfiguration<TState> UseLogger<TLogType, TOptionsType>(ConfigurationDelegate<TOptionsType>? options = null)
            where TLogType : ILog
            where TOptionsType : new()
        {
            IParameter parameter = CreateFactoryParameter(options);
            m_loggerTypeRequest = new TypeRequest(typeof(TLogType), typeof(ILog), parameter);
            return this;
        }

        /// <inheritdoc cref="IStoreConfiguration{T}"/>
        public IStoreConfiguration<TState> UseSingleton<TInstanceType, TOptionsType>(ConfigurationDelegate<TOptionsType>? options = null)
             where TOptionsType : new()
        {
            IParameter parameter = CreateFactoryParameter(options);
            m_singletonTypeRequests.Add(new TypeRequest(typeof(TInstanceType), typeof(TInstanceType), parameter));
            return this;
        }

        /// <inheritdoc cref="IStoreConfiguration{T}"/>
        public IStoreConfiguration<TState> UseSingleton<TInstanceType, TBindAs, TOptionsType>(ConfigurationDelegate<TOptionsType>? options = null)
              where TInstanceType : TBindAs
            where TOptionsType : new()
        {
            IParameter parameter = CreateFactoryParameter(options);
            m_singletonTypeRequests.Add(new TypeRequest(typeof(TInstanceType), typeof(TBindAs), parameter));
            return this;
        }

        /// <inheritdoc cref="IStoreConfiguration{T}"/>
        public IStore<TState> CreateStore()
            => CreateStore<Store<TState>>();

        public TStoreType CreateStore<TStoreType>() where TStoreType : IStore<TState>
        {
            if (m_activatorTypeRequest == null)
            {
                throw new MissingActivatorException();
            }

            List<IReducer<TState>> reducers = new List<IReducer<TState>>();
            List<IMiddleware> middlewares = new List<IMiddleware>();
            List<IParameter> parameters = new List<IParameter>();

            // Create activator 
            IActivator activator = DefaultActivator.Get<IActivator>(m_activatorTypeRequest);

            // Logging
            ILog logger = activator.Get<ILog>(m_loggerTypeRequest);

            parameters.Add(new ConstantParameter("log", logger, typeof(ILog)));

            foreach (ITypeRequest singletonRequest in m_singletonTypeRequests)
            {
                object instance = activator.Get(singletonRequest, parameters);

                IParameter parameter = new ConstantParameter("", instance, singletonRequest.Type);

                parameters.Add(parameter);
            }


            foreach (ITypeRequest reducerTypeRequest in m_reducers)
            {
                IReducer<TState> reducer = activator.Get<IReducer<TState>>(reducerTypeRequest, parameters);
                reducers.Add(reducer);
            }

            // IAction 
            //  -> Middleware_1
            //      -> Middlware_2
            //          -> Reducer_1
            //          -> Reducer_2
            //      -> Middlware_2 
            //  -> Middleware_1
            // Update State 

            ReducerMiddleware<TState> rootDispatcher = new ReducerMiddleware<TState>(reducers);

            ActionDispatchDelegate next = rootDispatcher.InvokeAsync;

            foreach (ITypeRequest middlewareTypeRequest in m_middleware)
            {
                IParameter nextParameter = ConstantParameter.Create<ActionDispatchDelegate>("next", next);
                IAbstractMiddleware abstractMiddleware = activator.Get<IAbstractMiddleware>(middlewareTypeRequest, parameters.Clone(nextParameter));
                switch (abstractMiddleware)
                {
                    case IMiddleware middleware:
                        next = (p) => middleware.InvokeAsync(p);
                        break;
                    case IMiddleware<TState> typedMiddleware:
                        next = (p) => typedMiddleware.InvokeAsync((ActionContext<TState>)p);
                        break;
                    default:
                        throw new InvalidMiddlewareException(abstractMiddleware.GetType());
                }
            }

            IList<IParameter> storeParameters = parameters.Clone(
                ConstantParameter.Create<TState>("state", m_initialState),
                ConstantParameter.Create<ActionDispatchDelegate>("dispatch", next)
            );

            TypeRequest storeRequest = new TypeRequest(typeof(TStoreType));

            TStoreType store = DefaultActivator.Get<TStoreType>(storeRequest, storeParameters);

            return store;
        }

        private IParameter CreateFactoryParameter<TOptionsType>(ConfigurationDelegate<TOptionsType>? options = null)
            where TOptionsType : new()
        {
            return new FactoryParameter<TOptionsType>(nameof(options), () =>
            {
                TOptionsType instance = new();
                options?.Invoke(instance);
                return instance;
            });
        }

    }
}
