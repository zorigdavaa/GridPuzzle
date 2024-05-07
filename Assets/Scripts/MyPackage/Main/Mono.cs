using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ZPackage
{
    public class Mb: MonoBehaviour
    {
        ///<summary>Эхлэж байгааг шалгана</summary>
        public static bool IsStarting => GameManager.Instance.State == GameState.Starting;

        ///<summary>Тоглож байгааг шалгана</summary>
        public static bool IsPlaying => GameManager.Instance.State == GameState.Playing;

        ///<summary>Зогсоосон байгааг шалгана</summary>
        public static bool IsPause => GameManager.Instance.State == GameState.Pause;

        ///<summary>Хожсон эсэхийг шалгана</summary>
        public static bool IsLevelCompleted => GameManager.Instance.State == GameState.LevelCompleted;

        ///<summary>Хожигдсон эсэхийг шалгана</summary>
        public static bool IsGameOver => GameManager.Instance.State == GameState.GameOver;

        ///<summary>Тохиргоо хийж байгааг шалгана</summary>
        public static bool IsSettings => GameManager.Instance.State == GameState.Settings;

        ///<summary>Input.GetMouseButton(0)</summary>
        public static bool IsClick => Input.GetMouseButton(0);

        ///<summary>Input.GetMouseButtonDown(0)</summary>
        public static bool IsDown => Input.GetMouseButtonDown(0);

        ///<summary>Input.GetMouseButtonUp(0)</summary>
        public static bool IsUp => Input.GetMouseButtonUp(0);

        ///<summary>Input.mousePosition</summary>
        public static Vector3 MP => Input.mousePosition;

        [HideInInspector]
        public Vector3 mp;

        ///<summary>Time.deltaTime</summary>
        public static float DT => Time.deltaTime;

        ///<summary>gameObject</summary>
        public GameObject go => gameObject;

        ///<summary>transform</summary>
        public Transform tf => transform;

        ///<summary>Rigidbody</summary>
        public Rigidbody rb
        {
            get
            {
                if (!_rb)
                    _rb = go.GetComponent<Rigidbody>();
                return _rb;
            }
        }
        Rigidbody _rb;
    }
}
