using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.Rendering.PostProcessing;

public class PPController : MonoBehaviour
{

    // PostProcessVolume PPV;
    // // Start is called before the first frame update
    // void Start()
    // {
    //     PPV = GetComponent<PostProcessVolume>();
    // }

    // public void ChromaticAberration(bool enable = true, float duration = 1, float formIntensity = 0, float toIntensity = 1, Action afteAction = null)
    // {
    //     StartCoroutine(localFunction());
    //     IEnumerator localFunction()
    //     {
    //         float time = 0;
    //         float intentity;
    //         ChromaticAberration chromaticAberration = PPV.profile.GetSetting<ChromaticAberration>();
    //         chromaticAberration.enabled.Override(enable);
    //         if (enable)
    //         {
    //             while (time < duration)
    //             {
    //                 time += Time.deltaTime;
    //                 intentity = Mathf.Lerp(formIntensity, toIntensity, time / duration);
    //                 chromaticAberration.intensity.Override(intentity);
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
    // float intensity = 0;
    // public void ChromaticAberration(bool enable = true, float speed = 0.05f, float fromIntensity = 0, float toIntensity = 1)
    // {
    //     ChromaticAberration chromaticAberration = PPV.profile.GetSetting<ChromaticAberration>();
    //     chromaticAberration.enabled.Override(enable);
    //     if (!enable || intensity == toIntensity)
    //     {
    //         return;
    //     }
    //     else
    //     {
    //         intensity = Mathf.MoveTowards(intensity, toIntensity, speed);
    //         chromaticAberration.intensity.Override(intensity);
    //     }

    // }
    // public void Vignette(bool enable = true, float duration = 1, float formIntensity = 0, float toIntensity = 1, Action afteAction = null)
    // {
    //     StartCoroutine(localFunction());
    //     IEnumerator localFunction()
    //     {
    //         float time = 0;
    //         float intentity;
    //         Vignette vignette = PPV.profile.GetSetting<Vignette>();
    //         vignette.enabled.Override(enable);
    //         if (!enable)
    //         {
    //             yield break;
    //         }
    //         while (time < duration)
    //         {
    //             time += Time.deltaTime;
    //             intentity = Mathf.Lerp(formIntensity, toIntensity, time / duration);
    //             vignette.intensity.Override(intentity);
    //             yield return null;
    //         }
    //         if (afteAction != null)
    //         {
    //             afteAction();
    //         }
    //     }
    // }
    // public void DepthOfField(bool enable = true, float duration = 1, float formIntensity = 0.1f, float toIntensity = 32, Action afteAction = null)
    // {
    //     StartCoroutine(localFunction());
    //     IEnumerator localFunction()
    //     {
    //         float time = 0;
    //         float aperture;
    //         DepthOfField depthOfField = PPV.profile.GetSetting<DepthOfField>();
    //         depthOfField.enabled.Override(enable);
    //         if (!enable)
    //         {
    //             yield break;
    //         }
    //         while (time < duration)
    //         {
    //             time += Time.deltaTime;
    //             aperture = Mathf.Lerp(formIntensity, toIntensity, time / duration);
    //             depthOfField.aperture.Override(aperture);
    //             yield return null;
    //         }
    //         if (afteAction != null)
    //         {
    //             afteAction();
    //         }
    //     }
    // }
    // public void Grain(bool enable = true, float Intensity = 1f)
    // {
    //     Grain depthOfField = PPV.profile.GetSetting<Grain>();
    //     depthOfField.enabled.Override(enable);
    //     depthOfField.intensity.Override(Intensity);
    // }
    // public void GrainOff()
    // {
    //     Grain depthOfField = PPV.profile.GetSetting<Grain>();
    //     depthOfField.intensity.Override(0);
    //     depthOfField.enabled.Override(false);
    // }
    // ///<Summary>EXpo 3 baihad defaulttai oiroltsoo utga ni 1.6<Summary>
    // ///<Summary>-9 ruu tsairna +9 ruu harlana<Summary>
    // public void AutoExposure(bool enable = true, float duration = 1, float formIntensity = 9f, float toIntensity = -9, Action afteAction = null)
    // {
    //     StartCoroutine(localFunction());
    //     IEnumerator localFunction()
    //     {
    //         float time = 0;
    //         float minEV;
    //         AutoExposure autoExposure = PPV.profile.GetSetting<AutoExposure>();
    //         autoExposure.enabled.Override(enable);
    //         if (!enable)
    //         {
    //             yield break;
    //         }
    //         while (time < duration)
    //         {
    //             time += Time.deltaTime;
    //             minEV = Mathf.Lerp(formIntensity, toIntensity, time / duration);
    //             autoExposure.minLuminance.Override(minEV);
    //             yield return null;
    //         }
    //         if (afteAction != null)
    //         {
    //             afteAction();
    //         }
    //     }
    // }
    // Color originalColor = Color.white;
    // public void ColorGradingColorFilter(float duration = 1, Action afteAction = null)
    // {
    //     StartCoroutine(localFunction());
    //     IEnumerator localFunction()
    //     {
    //         float time = 0;
    //         ColorGrading colorGrading = PPV.profile.GetSetting<ColorGrading>();
    //         // if (originalColor == null)
    //         // {
    //         //     originalColor = colorGrading.colorFilter;
    //         // }

    //         colorGrading.enabled.Override(true);
    //         while (time < duration)
    //         {
    //             time += Time.deltaTime;
    //             colorGrading.colorFilter.value = originalColor * FlickeringLight.EvalWave(3f, FlickeringLight.WaveForm.noise, 3);
    //             yield return null;
    //         }
    //         colorGrading.colorFilter.value = originalColor;
    //         colorGrading.enabled.Override(false);
    //         if (afteAction != null)
    //         {
    //             afteAction();
    //         }
    //     }
    // }
}
