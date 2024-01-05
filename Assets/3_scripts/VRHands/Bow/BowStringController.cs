using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class BowStringController : MonoBehaviour {
    [SerializeField] private BowString bowString;

    private XRGrabInteractable interactable;

    [SerializeField] private Transform midPointGrab, midPointVisual, midPointParent;

    [SerializeField] private float bowStrigStretchLimit = 0.3f;

    private Transform interactor;
    private float stength, previousStrenght;

    [SerializeField] private float stringSoundThreshold = 0.001f;

    [SerializeField] private AudioSource audioSource;

    public UnityEvent OnBowPulled;
    public UnityEvent<float> OnBowReleased;

    private void Awake() {
        interactable = midPointGrab.GetComponent<XRGrabInteractable>();
    }

    private void Start() {
        interactable.selectEntered.AddListener(PrepareBowString);
        interactable.selectExited.AddListener(ResetBowString);
    }

    private void PrepareBowString(SelectEnterEventArgs arg0) {
        interactor = arg0.interactorObject.transform;
        OnBowPulled?.Invoke();
    }
    private void ResetBowString(SelectExitEventArgs arg0) {

        OnBowReleased?.Invoke(stength);
        stength = 0;
        previousStrenght = 0;
        audioSource.pitch = 1;
        audioSource.Stop();

        interactor = null;
        midPointGrab.localPosition = Vector3.zero;
        midPointVisual.localPosition = Vector3.zero;
        bowString.CreateString(null);
    }

    private void Update() {
        if (interactor != null) {

            Vector3 midPointLocalSpace = midPointParent.InverseTransformPoint(midPointGrab.localPosition);

            float midPointLocalZAbs = Math.Abs(midPointLocalSpace.z);

            previousStrenght = stength;

            HandleStringPushed(midPointLocalSpace);
            HandleStringPulled(midPointLocalZAbs, midPointLocalSpace);
            HandlePulligString(midPointLocalZAbs, midPointLocalSpace);

            bowString.CreateString(midPointVisual.position);
        }
    }

    private void HandleStringPushed(Vector3 midPointLocalSpace) {
        if (midPointLocalSpace.z >= 0) {
            audioSource.pitch = 1;
            audioSource.Stop();    
            stength = 0;
            midPointVisual.localPosition = Vector3.zero;
        }
    }

    private void HandleStringPulled(float midPointLocalZAbs, Vector3 midPointLocalSpace) {
        if (midPointLocalSpace.z < 0 && midPointLocalZAbs >= bowStrigStretchLimit) {

            audioSource.Pause();
            stength = 1;

            midPointVisual.localPosition = new Vector3(0, 0, -bowStrigStretchLimit);
        }
    }

    private void HandlePulligString(float midPointLocalZAbs, Vector3 midPointLocalSpace) {
        if (midPointLocalSpace.z < 0 && midPointLocalZAbs < bowStrigStretchLimit) {

            if (audioSource.isPlaying == false && stength <= 0.01f) {
                audioSource.Play();
            }

            stength = Remap(midPointLocalZAbs, 0, bowStrigStretchLimit, 0, 1);
            midPointVisual.localPosition = new Vector3(0, 0, midPointLocalSpace.z);

            PlayStringPullingSound();
        }
    }

    private void PlayStringPullingSound() {
        if (Math.Abs(stength - previousStrenght) > stringSoundThreshold) {
            if (stength < previousStrenght) {
                audioSource.pitch = -1;
            } else {
                audioSource.pitch = 1;
            }
            audioSource.UnPause();
        } else {
            audioSource.Pause();
        }

    }

    private float Remap(float value, int fromMin, float fromMax, int toMin, int toMax) {
        return (value - fromMin) / (fromMax - fromMin) * (toMax - toMin) + toMin;
    }
}
