using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    private Animator animator;

    public void SetBool(int id, bool value) => animator.SetBool(id, value);
    public void SetTrigger(int id) => animator.SetTrigger(id);

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetTrigger(ObjectType type)
    {
        switch (type)
        {
            case ObjectType.Bonfire:
                animator.SetTrigger(Define.ID_ACTION_BONFIRE);
                break;
            case ObjectType.Tree:
                animator.SetTrigger(Define.ID_ACTION_TREE);
                break;
            case ObjectType.Rock:
                animator.SetTrigger(Define.ID_ACTION_ROCK);
                break;
        }
    }
}