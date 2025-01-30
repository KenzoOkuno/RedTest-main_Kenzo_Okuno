using UnityEngine;

public abstract class PlayerState : MonoBehaviour
{
    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void HandleMovement() { }
}
