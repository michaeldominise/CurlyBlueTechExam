using UnityEngine;

public class HighlightableItem : MonoBehaviour, IHighlightable
{
    [Header("Renderer")]
    [SerializeField] private Renderer targetRenderer;

    [Header("Highlight")]
    [SerializeField] private Color highlightColor = Color.green;

    private Color _originalColor;

    private void Awake()
    {
        // Fallback if not assigned
        if (targetRenderer == null)
            targetRenderer = GetComponentInChildren<Renderer>();

        if (targetRenderer == null)
        {
            Debug.LogError($"No Renderer found on {name}");
            return;
        }

        _originalColor = targetRenderer.material.color;
    }

    public void OnHoverEnter() => SetColor(highlightColor);

    public void OnHoverExit() => SetColor(_originalColor);

    private void SetColor(Color color)
    {
        if (targetRenderer == null) return;

        targetRenderer.material.color = color;
    }
}