using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    private Animator animator;

    public void SetBool(int id, bool value) => animator.SetBool(id, value);
    public void SetFloat(int id, float value) => animator.SetFloat(id, value);

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetTrigger(ObjectType type)
    {
        switch (type)
        {
            case ObjectType.Tree:
                animator.SetTrigger(Define.HASH_ACTION_TREE);
                break;
            case ObjectType.Rock:
                animator.SetTrigger(Define.HASH_ACTION_ROCK);
                break;
        }
    }
}