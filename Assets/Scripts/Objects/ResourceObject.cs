using System.Collections;
using UnityEngine;

public class ResourceObject : InteractableObject
{
    #region UI_ObjectStatus
    private UI_ObjectStatus objectStatusUI;

    public void Open_ObjectStatusUI()
    {
        if (data.HP == 0)
        {
            return;
        }

        if (objectStatusUI == null)
        {
            objectStatusUI = Managers.UI.Open<UI_ObjectStatus>();
        }

        objectStatusUI.UpdateUI(currentHP, data);
    }

    public void Close_ObjectStatusUI()
    {
        if (data.HP == 0)
        {
            return;
        }

        if (objectStatusUI == null)
        {
            return;
        }

        objectStatusUI.Close();
        objectStatusUI = null;
    }
    #endregion

    private readonly float SHAKE_AMOUNT = 5.0f;
    private readonly float SHAKE_DURATION = 0.5f;

    private Quaternion originalRotation;

    private void Start()
    {
        originalRotation = transform.rotation;
    }

    public override void OnInteraction(int damage = 0)
    {
        base.OnInteraction(damage);

        if (objectStatusUI != null)
        {
            objectStatusUI.UpdateUI(currentHP, data);
        }

        if (IsDead)
        {
            ObjectData data = this.data as ObjectData;
            foreach (var dropItem in Managers.Item.GetDropItems(data.DropTable))
            {
                Managers.Resource.Instantiate(Define.EFFECT_ITEM, transform.position, Define.PATH_EFFECT).GetComponent<ItemObject>().Play(dropItem);
            }

            return;
        }

        Vector3 attackDirection = (transform.position - Managers.Game.Player.transform.position).normalized;
        StartCoroutine(Shaking(attackDirection));
    }

    private IEnumerator Shaking(Vector3 attackDirection)
    {
        Vector3 shakeAxis = Vector3.Cross(Vector3.up, attackDirection).normalized;
        Quaternion targetRotation = Quaternion.AngleAxis(SHAKE_AMOUNT, shakeAxis) * originalRotation;

        float elapsedTime = 0.0f;
        while (elapsedTime < SHAKE_DURATION * 0.5f)
        {
            transform.rotation = Quaternion.Slerp(originalRotation, targetRotation, elapsedTime / (SHAKE_DURATION * 0.5f));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0.0f;
        while (elapsedTime < SHAKE_DURATION * 0.5f)
        {
            transform.rotation = Quaternion.Slerp(targetRotation, originalRotation, elapsedTime / (SHAKE_DURATION * 0.5f));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}