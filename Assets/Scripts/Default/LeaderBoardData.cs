
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LeaderBoardData {

    public string name;

    public float score;

    public float score2;

    public bool isPlayer;

    public bool isLive;

    public Color col;

    public LeaderBoardData(string name, float score, float score2, bool isPlayer, bool isLive, Color col) {
        this.name = name;
        this.score = score;
        this.score2 = score2;
        this.isPlayer = isPlayer;
        this.isLive = isLive;
        this.col = col;
    }

    public static void Sort(List<LeaderBoardData> datas) {
        List<LeaderBoardData> lives = new List<LeaderBoardData>();
        List<LeaderBoardData> deads = new List<LeaderBoardData>();
        foreach (LeaderBoardData data in datas) {
            if (data.isLive)
                lives.Add(data);
            else
                deads.Add(data);
        }
        lives.Sort((x, z) => -x.score.CompareTo(z.score));
        deads.Sort((x, z) => -x.score.CompareTo(z.score));
        for (int i = 0; i < datas.Count; i++)
            datas[i] = i < lives.Count ? lives[i] : deads[i - lives.Count];
    }

    public static int PlayerIdx(List<LeaderBoardData> datas) {
        return datas.IndexOf(datas.Find(i => i.isPlayer == true));
    }

    public static List<LeaderBoardData> GetDatas() {
        List<LeaderBoardData> leaderBoardDatas = new List<LeaderBoardData>();
        // A.Characters.ForEach(x => leaderBoardDatas.Add(x.data));
        return leaderBoardDatas;
    }

    public static void SetPlayerData(string name) {
        // if (A.Player)
        //     A.Player.CreateName(new LeaderBoardData(name, 0, 0, true, true, A.GC.playerCol));
    }

    public static void SetDatas() {
        // List<string> randomNames = Rnd.List<string>(A.Names, A.Characters.Count);
        // List<Color> randomColors = Rnd.List<Color>(A.GC.botCols, A.Characters.Count);
        // for (int i = 0; i < A.Characters.Count; i++) {
        //     if (A.Characters[i].data.isPlayer)
        //         SetPlayerData("");
        //     else
        //         A.Characters[i].CreateName(new LeaderBoardData(randomNames[i], 0, 0, false, true, randomColors[i]));
        // }
    }

    public static void RemoveNames() {
        // for (int i = 0; i < A.Characters.Count; i++)
        //     A.Characters[i].RemoveName();
    }
}