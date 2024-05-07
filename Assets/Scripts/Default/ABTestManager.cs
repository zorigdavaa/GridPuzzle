using UnityEngine;

#if ANALYTICS_SDKS
using GameAnalyticsSDK;
#endif

public class ABTestManager : MonoBehaviour
{
    public static ABTestManager Instance;

    public string TestABVariable { get; private set; }

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void Init()
    {
#if ANALYTICS_SDKS
        TestABVariable = GameAnalytics.GetRemoteConfigsValueAsString("ab_test_key", "default");

        print("AB:TestABVariable: " + TestABVariable);
#endif
    }
}
