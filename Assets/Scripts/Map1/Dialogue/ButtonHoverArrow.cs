using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverArrow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject hoverArrow;

    void Start()
    {
        if (hoverArrow != null)
            hoverArrow.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverArrow != null)
            hoverArrow.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (hoverArrow != null)
            hoverArrow.SetActive(false);
    }
}
