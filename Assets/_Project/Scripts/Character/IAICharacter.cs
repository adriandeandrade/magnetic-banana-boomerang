public interface IAICharacter
{
    EnemyStates CurrentState { get; set; }
    void SetState(EnemyStates newState);
    void UpdateState();

    void InitIdle();
    void Idle();

    void InitMoving();
    void Moving();

    void InitInteracting();
    void Interacting();
}
