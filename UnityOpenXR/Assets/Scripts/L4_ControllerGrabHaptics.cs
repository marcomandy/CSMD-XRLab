using System.Collections;
using Oculus.Interaction;
using Oculus.Interaction.Input;
//using Oculus.Haptics;
using UnityEngine;

//using HapticsController = Oculus.Haptics.Controller;
using ISDKHandedness = Oculus.Interaction.Input.Handedness;

/// <summary>
/// Plays one selectable haptic style when the controller-only GrabInteractable
/// is selected. The controller side is taken from the selecting ISDK interactor.
/// Tracked-hand grabs use a separate HandGrabInteractable and do not trigger this.
/// </summary>
public sealed class L4_ControllerGrabHaptics : MonoBehaviour
{
    private enum FeedbackMode
    {
        MetaHapticsSdk = 0,
        BasicOvrInput = 1
    }

    [SerializeField] private GrabInteractable _controllerGrabInteractable;
    [SerializeField] private FeedbackMode _feedbackMode = FeedbackMode.MetaHapticsSdk;
    //[SerializeField] private HapticSource _hapticSource;
    [SerializeField, Range(0f, 1f)] private float _ovrFrequency = 0.5f;
    [SerializeField, Range(0f, 1f)] private float _ovrAmplitude = 0.2f;
    [SerializeField, Min(0.01f)] private float _ovrPulseSeconds = 0.12f;

    private OVRInput.Controller _activeOvrController = OVRInput.Controller.None;

    private void OnEnable()
    {
        if (_controllerGrabInteractable != null)
        {
            _controllerGrabInteractable.WhenSelectingInteractorViewAdded += OnControllerGrabStarted;
        }
    }

    private void OnDisable()
    {
        if (_controllerGrabInteractable != null)
        {
            _controllerGrabInteractable.WhenSelectingInteractorViewAdded -= OnControllerGrabStarted;
        }

        StopOvrPulse();
        //if (_hapticSource != null)
        //{
            //_hapticSource.Stop();
        //}
    }

    private void OnControllerGrabStarted(IInteractorView interactorView)
    {
        if (!TryGetControllerHandedness(interactorView, out ISDKHandedness handedness))
        {
            // Do not guess which controller acted. A tracked-hand interaction or
            // an unexpected interactor therefore produces no controller feedback.
            return;
        }

        if (_feedbackMode == FeedbackMode.MetaHapticsSdk)
        {
            StopOvrPulse();
            /*if (_hapticSource == null)
            {
                Debug.LogWarning("[L4 Haptics] Meta Haptics mode is selected, but HapticSource is not assigned.", this);
                return;
            }*/

            //_hapticSource.Play(ToHapticsController(handedness));
            return;
        }

        /*if (_hapticSource != null)
        {
            _hapticSource.Stop();
        }*/

        OVRInput.Controller controller = ToOvrController(handedness);
        StopOvrPulse();
        _activeOvrController = controller;
        OVRInput.SetControllerVibration(_ovrFrequency, _ovrAmplitude, controller);
        StartCoroutine(StopPulse(controller));
    }

    private static bool TryGetControllerHandedness(IInteractorView interactorView, out ISDKHandedness handedness)
    {
        handedness = default;
        if (interactorView?.Data is not Component interactorComponent)
        {
            return false;
        }

        // In this scene, each v207 ControllerGrabInteractor has a ControllerRef
        // component on the same GameObject. That ref delegates handedness to the
        // matching ComprehensiveInteractorsLeft/Right controller data source.
        ControllerRef controllerRef = interactorComponent.GetComponent<ControllerRef>();
        if (controllerRef == null)
        {
            return false;
        }

        handedness = controllerRef.Handedness;
        return handedness == ISDKHandedness.Left || handedness == ISDKHandedness.Right;
    }

    /*private static HapticsController ToHapticsController(ISDKHandedness handedness)
    {
        return handedness == ISDKHandedness.Left
            ? HapticsController.Left
            : HapticsController.Right;
    }*/

    private static OVRInput.Controller ToOvrController(ISDKHandedness handedness)
    {
        return handedness == ISDKHandedness.Left
            ? OVRInput.Controller.LTouch
            : OVRInput.Controller.RTouch;
    }

    private IEnumerator StopPulse(OVRInput.Controller controller)
    {
        yield return new WaitForSecondsRealtime(_ovrPulseSeconds);
        OVRInput.SetControllerVibration(0f, 0f, controller);
        if (_activeOvrController == controller)
        {
            _activeOvrController = OVRInput.Controller.None;
        }
    }

    private void StopOvrPulse()
    {
        StopAllCoroutines();
        if (_activeOvrController == OVRInput.Controller.None)
        {
            return;
        }

        OVRInput.SetControllerVibration(0f, 0f, _activeOvrController);
        _activeOvrController = OVRInput.Controller.None;
    }
}
