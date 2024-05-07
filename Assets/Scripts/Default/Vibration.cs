using System.Collections;
using UnityEngine;

public static class Vibration
{
#if UNITY_ANDROID && !UNITY_EDITOR
    public static AndroidJavaClass unityPlayer = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
    public static AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject> ("currentActivity");
    public static AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject> ("getSystemService", "vibrator");
    public static AndroidJavaObject context = currentActivity.Call<AndroidJavaObject> ("getApplicationContext");
    public static void Vibrate (long milliseconds) {
        vibrator.Call ("vibrate", milliseconds);
    }
    public static void Vibrate (long[] pattern, int rep) {
        vibrator.Call ("vibrate", pattern, rep);
    }
    public static void Cancel () {
        vibrator.Call ("cancel");
    }
#endif
    public static bool HasVibrator()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaClass contextClass = new AndroidJavaClass ("android.content.Context");
        string Context_VIBRATOR_SERVICE = contextClass.GetStatic<string> ("VIBRATOR_SERVICE");
        AndroidJavaObject systemService = context.Call<AndroidJavaObject> ("getSystemService", Context_VIBRATOR_SERVICE);
        if (systemService.Call<bool> ("hasVibrator")) {
            return true;
        } else {
            return false;
        }
#else
        return false;
#endif
    }
    public static void Vibrate()
    {
#if UNITY_EDITOR
        Debug.Log("Vibrate");
#endif
        Handheld.Vibrate();
    }
}