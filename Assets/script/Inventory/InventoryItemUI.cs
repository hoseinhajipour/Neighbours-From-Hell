using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class InventoryItemUI : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Vector2 startPosition;
    private RectTransform rectTransform;

    private Transform parent;

    Button button;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        parent = transform.parent;
        button = GetComponent<Button>();

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.parent = GameObject.Find("Canvas").transform;
        startPosition = rectTransform.anchoredPosition;
        //canvasGroup.blocksRaycasts = false; // Disable raycasts on the button
        button.interactable = false; // Disable the button's interactability
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {

        GameObject pointerOverObject = eventData.pointerEnter;
        Debug.Log(pointerOverObject.name);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject hitObject = hit.collider.gameObject;

            Sabotage sabotageComponent = hitObject.GetComponent<Sabotage>();
            if (sabotageComponent)
            {
                sabotageComponent.DoSabotage();
                Debug.Log(hitObject.name);
                Destroy(gameObject);
            }
            else
            {
                rectTransform.anchoredPosition = startPosition;
                transform.parent = parent;
                button.interactable = true;
            }
        }

    }
}
