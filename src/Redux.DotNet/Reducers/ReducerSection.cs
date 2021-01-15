﻿using Redux.DotNet.Exceptions;
using ReduxSharp.Activation;
using ReduxSharp.Activation.IOC;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace ReduxSharp.Reducers
{
    internal class ReducerSection<TState, TReducerType, TSectionType> : IReducer<TState> 
        where TReducerType : IReducer<TSectionType>
    {
        private delegate TSectionType GetterDelegate(TState state);
        private delegate void SetterDelegate(TState state, TSectionType section);


        private Expression<StateSubSectionSelectionDelegate<TState, TSectionType>> m_sectionSelector;
        private IReducer<TSectionType> m_sectionReducer;

        private GetterDelegate m_getValue;
        private SetterDelegate m_setValue;

        public ReducerSection(IActivator activator, ITypeRequest subReducer, Expression<StateSubSectionSelectionDelegate<TState, TSectionType>> sectionSelector)
        {
            m_sectionSelector = sectionSelector;
            m_sectionReducer = activator.Get<IReducer<TSectionType>>(subReducer);

            MemberExpression memberExpression = (MemberExpression)sectionSelector.Body;
            PropertyInfo propertyInfo = (PropertyInfo)memberExpression.Member;

            if(!propertyInfo.CanWrite || !propertyInfo.CanRead)
            {
                throw new NotAccessableSubSectionProperty($"The property {propertyInfo.Name} is being used as a Sub Section reducer however it is an invalid target. Any property used *MUST* be both to be able to be read and written to.");
            }

            m_getValue = (GetterDelegate)Delegate.CreateDelegate(typeof(GetterDelegate), propertyInfo.GetMethod);
            m_setValue = (SetterDelegate)Delegate.CreateDelegate(typeof(SetterDelegate), propertyInfo.SetMethod);
        }


        TState IReducer<TState>.Reduce(TState currentState, IAction actionContext)
        {
            TSectionType current = m_getValue(currentState);

            TSectionType after = m_sectionReducer.Reduce(current, actionContext);

            if(!ReferenceEquals(current, after))
            {
                m_setValue(currentState, after);
            }

            return currentState;
        }
    }
}
