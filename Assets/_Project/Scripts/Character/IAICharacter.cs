public interface IAICharacter
{
    EnemyStates CurrentState { get; set; }
    void SetState(EnemyStates newState);
    void UpdateState();

    void InitIdleState();
    void UpdateIdleState();

    void InitMovingState();
    void UpdateMovingState();

    void InitAttackState();
    void UpdateAttackState();
}
