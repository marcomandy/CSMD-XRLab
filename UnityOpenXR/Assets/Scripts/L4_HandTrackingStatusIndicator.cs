using System.Text;
using Oculus.Interaction.Input;
using UnityEngine;

[ExecuteAlways]
public sealed class L4_HandTrackingStatusIndicator : MonoBehaviour
{
    [Header("Existing Meta Interaction SDK hand data")]
    [SerializeField] private Hand _leftHand;
    [SerializeField] private Hand _rightHand;

    [Header("Raw OVR hand data for Quest diagnostics")]
    [SerializeField] private OVRHand _leftOVRHand;
    [SerializeField] private OVRHand _rightOVRHand;
    [SerializeField] private bool _logRuntimeState = true;
    [SerializeField, Min(0.25f)] private float _logIntervalSeconds = 1f;

    [Header("Lesson 4 indicator renderers")]
    [SerializeField] private Renderer _leftIndicator;
    [SerializeField] private Renderer _rightIndicator;

    [Header("Status colors")]
    [SerializeField] private Color _notTracked = new Color(0.22f, 0.25f, 0.30f, 1f);
    [SerializeField] private Color _lowConfidence = new Color(1f, 0.58f, 0.08f, 1f);
    [SerializeField] private Color _highConfidence = new Color(0.16f, 0.82f, 0.36f, 1f);

    private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
    private static readonly int ColorId = Shader.PropertyToID("_Color");
    private MaterialPropertyBlock _block;
    private float _nextLogTime;

    private void Update()
    {
        ApplyStatus(_leftOVRHand, _leftIndicator);
        ApplyStatus(_rightOVRHand, _rightIndicator);

        if (!Application.isPlaying || !_logRuntimeState || Time.unscaledTime < _nextLogTime)
        {
            return;
        }

        _nextLogTime = Time.unscaledTime + Mathf.Max(0.25f, _logIntervalSeconds);
        var message = new StringBuilder(256);
        message.Append("[L4HandStatus] t=").Append(Time.unscaledTime.ToString("F2"));
        message.Append(" | LEFT ");
        AppendHandState(message, _leftOVRHand, _leftHand);
        message.Append(" | RIGHT ");
        AppendHandState(message, _rightOVRHand, _rightHand);
        Debug.Log(message.ToString(), this);
    }

    private static void AppendHandState(StringBuilder output, OVRHand ovrHand, Hand hand)
    {
        output.Append("OVR(");
        if (ovrHand == null)
        {
            output.Append("null");
        }
        else
        {
            output.Append("tracked=").Append(ovrHand.IsTracked)
                .Append(",dataValid=").Append(ovrHand.IsDataValid)
                .Append(",highConfidence=").Append(ovrHand.IsDataHighConfidence)
                .Append(",confidence=").Append(ovrHand.HandConfidence);
        }

        output.Append(") ISDK(");
        if (hand == null)
        {
            output.Append("null");
        }
        else
        {
            output.Append("connected=").Append(hand.IsConnected)
                .Append(",trackedDataValid=").Append(hand.IsTrackedDataValid)
                .Append(",highConfidence=").Append(hand.IsHighConfidence);
        }

        output.Append(')');
    }

    private void ApplyStatus(OVRHand hand, Renderer indicator)
    {
        if (indicator == null) return;

        Color status = _notTracked;
        if (hand != null && hand.IsDataValid)
        {
            if (!hand.IsTracked)
            {
                status = _notTracked;
            }
            else
            {
                status = hand.IsDataHighConfidence ? _highConfidence : _lowConfidence;
            }
        }

        _block ??= new MaterialPropertyBlock();
        indicator.GetPropertyBlock(_block);
        _block.SetColor(BaseColorId, status);
        _block.SetColor(ColorId, status);
        indicator.SetPropertyBlock(_block);
    }
}
