using System.Collections;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    #region Inspector
    [SerializeField] private float radius;
    [SerializeField] private float minHeight;
    [SerializeField] private float speed;
    #endregion

    private Item item;
    private Transform target;
    private Vector3 spreadPosition;

    private void Awake()
    {
        target = Managers.Game.Player.transform;
    }

    public void Play(Item item)
    {
        this.item = item;
        StartCoroutine(Spreading());
    }

    private IEnumerator Spreading()
    {
        Vector3 spreadDirection = Random.insideUnitSphere * radius;
        spreadPosition = transform.position + spreadDirection;
        spreadPosition.y = Mathf.Max(spreadPosition.y, transform.position.y + minHeight);

        float spreadTime = Vector3.Distance(transform.position, spreadPosition) / speed;
        float elapsedTime = 0.0f;

        Vector3 startPosition = transform.position;
        while (elapsedTime < spreadTime)
        {
            elapsedTime += Time.deltaTime;
            transform.position = Vector3.Lerp(startPosition, spreadPosition, elapsedTime / spreadTime);
            yield return null;
        }

        StartCoroutine(Moving());
    }

    private IEnumerator Moving()
    {
        while (true)
        {
            Vector3 targetPosition = target.position + Vector3.up;
            float moveTime = Vector3.Distance(spreadPosition, targetPosition) / speed;
            float elapsedTime = 0.0f;

            while (elapsedTime < moveTime)
            {
                elapsedTime += Time.deltaTime;
                transform.position = Vector3.Lerp(spreadPosition, targetPosition, elapsedTime / moveTime);
                yield return null;
            }

            targetPosition = target.position + Vector3.up;
            if (Vector3.Distance(transform.position, targetPosition) < 0.5f)
            {
                break;
            }

            spreadPosition = transform.position;
        }

        Managers.Item.AddItem(item);
        Managers.Resource.Instantiate(Define.EFFECT_ITEM_GET, transform.position, Define.PATH_EFFECT);
        Managers.Resource.Destroy(gameObject);
        Managers.UI.NavagationUI.Open_NavigationItem(item);
        Managers.UI.UpdateMenuUI();
    }
}