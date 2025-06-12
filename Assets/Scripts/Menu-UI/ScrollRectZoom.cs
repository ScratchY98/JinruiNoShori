using UnityEngine;
using UnityEngine.UI;

public class ScrollRectZoom : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private float zoomSpeed = 0.1f;
    [SerializeField] private float minZoom = 0.5f;
    [SerializeField] private float maxZoom = 3f;

    private RectTransform contentRect;
    private RectTransform viewportRect;

    void Start()
    {
       scrollRect = GetComponent<ScrollRect>();

        contentRect = scrollRect.content;
        viewportRect = scrollRect.viewport;
    }

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll == 0f) return;

        Vector2 localCursor;
        Vector2 mousePosition = Input.mousePosition;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(viewportRect, mousePosition, null, out localCursor))
        {
            Vector3 prevScale = contentRect.localScale;

            Vector2 pivot = new Vector2(
                (localCursor.x / viewportRect.rect.width) + 0.5f,
                (localCursor.y / viewportRect.rect.height) + 0.5f
            );

            pivot.x = Mathf.Clamp01(pivot.x);
            pivot.y = Mathf.Clamp01(pivot.y);

            contentRect.pivot = pivot;

            float scaleFactor = 1 + scroll * zoomSpeed;
            float newScaleX = Mathf.Clamp(contentRect.localScale.x * scaleFactor, minZoom, maxZoom);
            float newScaleY = Mathf.Clamp(contentRect.localScale.y * scaleFactor, minZoom, maxZoom);

            contentRect.localScale = new Vector3(newScaleX, newScaleY, 1);

            Vector2 deltaPivot = contentRect.pivot - pivot;
            Vector3 deltaPosition = new Vector3(deltaPivot.x * contentRect.rect.width * contentRect.localScale.x,
                                                deltaPivot.y * contentRect.rect.height * contentRect.localScale.y);

            contentRect.localPosition -= deltaPosition;
        }
    }
}
