using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if SW_STAGE_STAGE10_OR_ABOVE
using SupersonicWisdomSDK;
#endif

public class Initializer : MonoBehaviour
{

#if SW_STAGE_STAGE10_OR_ABOVE
    const float SecondsToWait = 3f;
#else
    const float SecondsToWait = 1f;
#endif

    float timer = 0f;
    bool mainSceneLoaded = false;

#if SW_STAGE_STAGE10_OR_ABOVE
    void Awake()
    {
        // Subscribe
        SupersonicWisdom.Api.AddOnReadyListener(OnSupersonicWisdomReady);
        // Then initialize
        SupersonicWisdom.Api.Initialize();
    }
#endif

    void OnSupersonicWisdomReady()
    {
        print("-------------- Wisdom ready. --------------");
        LoadMainScene();

    }

    void LoadMainScene()
    {
        if (mainSceneLoaded) return;

        ABTestManager.Instance.Init();
        SceneManager.LoadScene("Main");
        mainSceneLoaded = true;
        enabled = false;
    }

#if SW_STAGE_STAGE10_OR_ABOVE
    void OnDestroy()
    {
        SupersonicWisdom.Api.RemoveOnReadyListener(OnSupersonicWisdomReady);
    }
#endif

}
