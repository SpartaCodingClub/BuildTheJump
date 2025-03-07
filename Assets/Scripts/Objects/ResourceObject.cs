using System.Collections;
using UnityEngine;

public class ResourceObject : InteractableObject
{
    private readonly float shakeAmount = 5.0f;
    private readonly float shakeDuration = 0.5f;

    private Quaternion originalRotation;

    private void OnEnable()
    {
        originalRotation = transform.rotation;
    }

    public override void OnInteraction(int damage)
    {
        base.OnInteraction(damage);

        if (IsDead)
        {
            var dropItems = Managers.Item.GetDropItems(data.DropTable);
            for (int i = 0; i < dropItems.Count; i++)
            {
                GameObject gameObject = Managers.Resource.Instantiate(Define.EFFECT_ITEM, transform.position, Define.PATH_EFFECT);
                gameObject.GetComponent<Item_Movement>().Play(dropItems[i]);
            }

            return;
        }

        Vector3 attackDirection = (transform.position - Managers.Game.Player.transform.position).normalized;
        StartCoroutine(Shaking(attackDirection));
    }

    private IEnumerator Shaking(Vector3 attackDirection)
    {
        Vector3 shakeAxis = Vector3.Cross(Vector3.up, attackDirection).normalized;
        Quaternion targetRotation = Quaternion.AngleAxis(shakeAmount, shakeAxis) * originalRotation;

        float elapsedTime = 0.0f;
        while (elapsedTime < shakeDuration * 0.5f)
        {
            transform.rotation = Quaternion.Slerp(originalRotation, targetRotation, elapsedTime / (shakeDuration * 0.5f));
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        elapsedTime = 0.0f;
        while (elapsedTime < shakeDuration * 0.5f)
        {
            transform.rotation = Quaternion.Slerp(targetRotation, originalRotation, elapsedTime / (shakeDuration * 0.5f));
            elapsedTime += Time.deltaTime;

            yield return null;
        }
    }
}