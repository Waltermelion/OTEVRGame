using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CameraTakeDamageVisualEffect : MonoBehaviour {
    private Volume _volume;
    private Vignette _vignette;

    private float itensity;

    private void Start() {
        _volume = GetComponent<Volume>();
        _volume.profile.TryGet(out _vignette);

        if (!_vignette) {
            print("error, vignette empty");
        } else {
            _vignette.active = false;
        }
    }

    public void CameraTakeDamageEffect() {
        StartCoroutine(TakeDamageEffect());
    }

    private IEnumerator TakeDamageEffect() {
        itensity = .3f;

        _vignette.active = true;
        _vignette.intensity.value = .3f;

        yield return new WaitForSeconds(.3f);

        while (itensity > 0) {
            itensity -= .01f;

            if (itensity < 0) itensity = 0;

            _vignette.intensity.value = itensity;

            yield return new WaitForSeconds(.1f);

        }

        _vignette.active = false;
        yield break;
    }
}
