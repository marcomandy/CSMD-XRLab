using UnityEngine;
public sealed class ResetPanelPose : MonoBehaviour
{
    [SerializeField] private Transform panelRoot;
    private Vector3 initialLocalPosition;
    private Quaternion initialLocalRotation;
    private void Awake()
    {
        if (panelRoot == null) panelRoot = transform;
        initialLocalPosition = panelRoot.localPosition;
        initialLocalRotation = panelRoot.localRotation;
    }
    public void ResetPose()
    {
        panelRoot.SetLocalPositionAndRotation(
            initialLocalPosition, initialLocalRotation);
    }
}
