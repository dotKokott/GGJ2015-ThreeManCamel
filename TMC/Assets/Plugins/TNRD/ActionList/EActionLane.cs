public enum EActionLane {
    AI = 1,
    Animation = 2,
    Audio = 4,
    Behaviour = 8,
    /// <summary>
    /// Is used by the iTweenAction
    /// </summary>
    Tween = 16,
    /// <summary>
    /// Stops every lane from going past this point. This does NOT call Blocked and UnBlocked on the other actions
    /// </summary>
    Sync = 32,
    Utility = 64,
    /// <summary>
    /// Using this will break the ActionLane, so don't use it
    /// </summary>
    _DoNotUse = 65536
}