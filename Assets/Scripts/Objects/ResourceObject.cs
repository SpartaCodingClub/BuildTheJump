using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceObject : InteractableObject
{
    #region UI_ObjectStatus
    private UI_ObjectStatus objectStatusUI;

    public void Open_ObjectStatusUI()
    {
        if (objectStatusUI == null)
        {
            objectStatusUI = Managers.UI.Open<UI_ObjectStatus>();
        }

        objectStatusUI.UpdateUI(currentHP, baseData);
    }

    private void Close_ObjectStatusUI()
    {
        if (objectStatusUI == null)
        {
            return;
        }

        objectStatusUI.Close();
        objectStatusUI = null;
    }
    #endregion

    private static readonly float SHAKE_AMOUNT = 5.0f;
    private static readonly float SHAKE_DURATION = 0.5f;

    private Quaternion originalRotation;

    private void Start()
    {
        originalRotation = transform.rotation;
    }

    public override void InteractionEnter(int damage = 0)
    {
        base.InteractionEnter(damage);

        // 플레이어가 현재 공격 중인 대상이라면, 체력바 UI를 갱신합니다.
        if (objectStatusUI != null)
        {
            objectStatusUI.UpdateUI(currentHP, baseData);
        }

        // 오브젝트가 사망했다면, 아이템을 드롭합니다.
        if (IsDead)
        {
            ObjectData objectData = baseData as ObjectData;
            List<Item> dropItems = Managers.Item.GetDropItems(objectData.DropTable);
            foreach (Item dropItem in dropItems)
            {
                GameObject gameObject = Managers.Resource.Instantiate(Define.EFFECT_ITEM, transform.position, Define.PATH_EFFECT);
                gameObject.GetComponent<ItemObject>().Play(dropItem);
            }

            return;
        }

        // 공격 방향
        Vector3 attackDirection = (transform.position - Managers.Game.Player.transform.position).normalized;
        StartCoroutine(Shaking(attackDirection));
    }

    public override void InteractionExit(bool isPlayer)
    {
        base.InteractionExit(isPlayer);
        if (isPlayer) Close_ObjectStatusUI();
    }

    private IEnumerator Shaking(Vector3 attackDirection)
    {
        // 회전 축, 회전 방향
        Vector3 shakeAxis = Vector3.Cross(Vector3.up, attackDirection).normalized;
        Quaternion targetRotation = Quaternion.AngleAxis(SHAKE_AMOUNT, shakeAxis) * originalRotation;

        // 회전
        float elapsedTime = 0.0f;
        while (elapsedTime < SHAKE_DURATION * 0.5f)
        {
            transform.rotation = Quaternion.Slerp(originalRotation, targetRotation, elapsedTime / (SHAKE_DURATION * 0.5f));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 복귀
        elapsedTime = 0.0f;
        while (elapsedTime < SHAKE_DURATION * 0.5f)
        {
            transform.rotation = Quaternion.Slerp(targetRotation, originalRotation, elapsedTime / (SHAKE_DURATION * 0.5f));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}