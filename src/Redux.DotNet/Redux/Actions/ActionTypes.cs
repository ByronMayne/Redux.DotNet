using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace ReduxSharp.Redux.Actions
{
    public enum ActionTypes
    {
        [EnumMember(Value = "PERFORM_ACTION")]
        PerformAction,

        [EnumMember(Value = "RESET")]
        Reset,

        [EnumMember(Value = "ROLLBACK")]
        Roolback,

        [EnumMember(Value = "COMMIT")]
        Commit,

        [EnumMember(Value = "SWEEP")]
        Sweet,

        [EnumMember(Value = "TOGGLE_ACTION")]
        ToggleAction,

        [EnumMember(Value = "SET_ACTIONS_ACTIVE")]
        SetActionsActivate,

        [EnumMember(Value = "JUMP_TO_STATE")]
        JumpToState,

        [EnumMember(Value = "JUMP_TO_ACTION")]
        JumpToAction,

        [EnumMember(Value = "REORDER_ACTION")]
        ReorderAction,

        [EnumMember(Value = "IMPORT_STATE")]
        ImportState,

        [EnumMember(Value = "LOCK_CHANGES")]
        LockChanges,

        [EnumMember(Value = "PAUSE_RECORDING")]
        PauseRecordings,
    }
}
