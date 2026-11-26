using UnityEngine;

[ExecuteAlways]
public sealed class L4_ThumbsUpFeedback : MonoBehaviour
{
    [SerializeField] private Renderer _indicatorRenderer;
    [SerializeField] private Color _notRecognized = new Color(0.28f, 0.31f, 0.36f, 1f);
    [SerializeField] private Color _recognized = new Color(0.12f, 0.88f, 0.38f, 1f);

    private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
    private static readonly int ColorId = Shader.PropertyToID("_Color");
    private MaterialPropertyBlock _block;

    private void OnEnable()
    {
        OnPoseLost();
    }

    public void OnPoseRecognized()
    {
        Apply(_recognized);
    }

    public void OnPoseLost()
    {
        Apply(_notRecognized);
    }

    private void Apply(Color color)
    {
        if (_indicatorRenderer == null) return;
        _block ??= new MaterialPropertyBlock();
        _indicatorRenderer.GetPropertyBlock(_block);
        _block.SetColor(BaseColorId, color);
        _block.SetColor(ColorId, color);
        _indicatorRenderer.SetPropertyBlock(_block);
    }
}