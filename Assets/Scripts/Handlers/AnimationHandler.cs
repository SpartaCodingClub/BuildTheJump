using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    public Animator Animator { get; private set; }

    public void SetBool(int id, bool value) => Animator.SetBool(id, value);
    public void SetTrigger(int id) => Animator.SetTrigger(id);

    private void Awake()
    {
        Animator = GetComponent<Animator>();
    }

    public void SetTrigger(ObjectType type)
    {
        switch (type)
        {
            case ObjectType.Bonfire:
                Animator.SetTrigger(Define.ID_ACTION_BONFIRE);
                break;
            case ObjectType.Tree:
                Animator.SetTrigger(Define.ID_ACTION_TREE);
                break;
            case ObjectType.Rock:
                Animator.SetTrigger(Define.ID_ACTION_ROCK);
                break;
            case ObjectType.Monster:
                Animator.SetTrigger(Define.ID_ATTACK);
                break;
        }
    }
}