<img src='https://camo.githubusercontent.com/f28b5bc7822f1b7bb28a96d8d09e7d79169248fc/687474703a2f2f692e696d6775722e636f6d2f4a65567164514d2e706e67' height='60' alt='Redux Logo' aria-label='redux.js.org' /> 
&nbsp; &nbsp; &nbsp; &nbsp;
<img src='https://upload.wikimedia.org/wikipedia/commons/a/a3/.NET_Logo.svg' height='60' alt='Redux Logo' aria-label='redux.js.org' />

<br />
<br />
<br />

# Redux + DotNet 
This repository contains a reimplementation of the beloved Redux technoligy used in the the web development world.  Redux is a predictable state container for ~~JavaScript~~ C# apps.

Before getting started it is suggested you read up on how Redux works because it matches the usage in this project. Here are a few useful topics to cover.

 * Stores
 * Reducers
 * Middleware
 * Actions
 * Dispatchers


# Getting Started

It all starts with your application state object. This will contain ever value your application will use. 

```csharp

public record AppState 
{
    public int Counter { get; init; }
}

```
> If you are not using C#9 you can just replace the `record` with `class` and remove `init` and add a constructor to create your type. Records are not required but they simplify the code later on.

Next we have to setup our store. This store will hold our state and manage dispatching of events and mutating of the data, these we will cover later on. 


The store should be defined at the start of the application starting up and there should also only ever be a single instace in your application.

```csharp
IStore<AppState> store = new StoreConfiguration<ApplicationState>()
                      .CreateStore();
```
This is all you need for the most basic of store however we are going to add some features to allow us to get more ussage. 


## Action 
In redux we need to define actions, actions are the thing that will triger our state to be mutated. We want to define two actions, one to increment our counter and one to decrement it.

```csharp
public class CounterIncrementByAmount : IAction 
{
    public string Type => "counter/incrementByAmount";
    public int Amount { get; init; }
} 

public class CounterDecrementByAmount : IAction 
{
    public string Type => "counter/incrementByAmount";
    public int Amount { get; init; }
} 

```

The goal of this actions is to tell the store to change the value of the counter in our state. To do this we need to implement a Reducer.

## Reducers

The reducer is what takes the action and figures out how it modifies the state. The state *MUST* never be edited directly, you should always make a copy and return the new version. 

```csharp
public class CounterReducer : IMiddleware<AppStateT>
{
    public TState Reduce(AppState currentState, IAction action)
    {
        switch (action)
        {
            case CounterIncrementByAmount increment:
            {
                return currentState with { Counter = currentState.Counter + increment.Value  };
            }

            case CounterDecrementByAmount decrement:
            {
                return currentState with { Counter = currentState.Counter + decrement.Value  };
            }
        }
        return currentState;
    }
}
```
> As noted before using the `with` syntax makes writing this code less of a pain. Now that we have the reducer we need to add it to the store. Lets go back to the initialization logic 



```csharp
IStore<AppState> store = new StoreConfiguration<ApplicationState>()
                      .UserReducer<CounterReducer>()
                      .CreateStore();
```


TODO: Dispatching events docs

TODO: Dev tools integration docs 