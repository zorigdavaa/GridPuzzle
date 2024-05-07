using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
// using UnityEngine.Rendering.Universal;

public class URPPP : MonoBehaviour
{
    // Volume volume;
    // ChromaticAberration chromaticAberration;
    // Bloom bloom;
    // // Start is called before the first frame update
    // void Start()
    // {
    //     volume = GetComponent<Volume>();
    //     volume.profile.TryGet<ChromaticAberration>(out chromaticAberration);
    //     volume.profile.TryGet<Bloom>(out bloom);
    // }

    // public void ChromaticAberration(bool enable = true, float duration = 1, float formIntensity = 0, float toIntensity = 1, Action afteAction = null)
    // {
    //     StartCoroutine(localFunction());
    //     IEnumerator localFunction()
    //     {
    //         float time = 0;
    //         float intentity;
    //         chromaticAberration.active = enable;
    //         bloom.active = enable;
    //         if (enable)
    //         {
    //             while (time < duration)
    //             {
    //                 time += Time.deltaTime;
    //                 intentity = Mathf.Lerp(formIntensity, toIntensity, time / duration);
    //                 chromaticAberration.intensity.value = intentity;
    //                 yield return null;
    //             }
    //             if (afteAction != null)
    //             {
    //                 afteAction();
    //             }
    //         }
    //         else
    //         {
    //             yield break;
    //         }
    //     }
    // }
}
