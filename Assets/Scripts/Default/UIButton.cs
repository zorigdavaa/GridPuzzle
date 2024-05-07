using UnityEngine;
using ZPackage;

public enum UIButtonType { Pause, Settings, Replay, Resume, Play, Save, Done, Clear }

public class UIButton : MonoBehaviour
{

    public UIButtonType type;

    public void Click()
    {
        switch (type)
        {
            case UIButtonType.Pause:
                Z.GM.PauseGame(true);
                break;
            case UIButtonType.Replay:
                Z.GM.RestartGame();
                break;
            case UIButtonType.Resume:
                Z.GM.PauseGame(false);
                break;
            case UIButtonType.Play:
                Z.GM.PlayGame();
                break;
            case UIButtonType.Done:
                // Z.Player.DoneBoard();
                break;
                // case UIButtonType.Banner:
                //     FindObjectOfType<Admob>().RequestBannerAd();
                //     print("bannaer");
                //     break;
                // case UIButtonType.Interstitial:
                //     FindObjectOfType<Admob>().ShowInterstitialAd();
                //     print("inter");
                //     break;
                // case UIButtonType.rewardedVide:
                //     FindObjectOfType<Admob>().ShowRewardedAd();
                //     print("reward");
                //     break;
                // case UIButtonType.interReaward:
                //     FindObjectOfType<Admob>().ShowRewardedInterstitialAd();
                //     print("interReward");
                //     break;
        }
    }
}