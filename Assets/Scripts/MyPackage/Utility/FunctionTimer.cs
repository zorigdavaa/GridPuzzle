using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

namespace ZPackage
{
    public class FunctionTimer
    {
        public static List<FunctionTimer> activeTimers;
        private static GameObject initGameObject;
        private static void InitIfNeeded()
        {
            if (initGameObject == null)
            {
                initGameObject = new GameObject("FunctionTimerAll");
                activeTimers = new List<FunctionTimer>();
            }
        }
        public static async void WaitAndFireAsync(float time,Action action)
        {
            await Task.Delay(TimeSpan.FromSeconds(time));
            action();
        }
        public static FunctionTimer Create(Action action, float timer, string name = null)
        {
            InitIfNeeded();
            GameObject gameObject = new GameObject(nameof(FunctionTimer), typeof(MonobehaviourHook));
            FunctionTimer functionTimer = new FunctionTimer(action, timer, name, gameObject);
            gameObject.GetComponent<MonobehaviourHook>().onUpdate = functionTimer.Update;
            activeTimers.Add(functionTimer);
            return functionTimer;
        }
        public static void StopTimer(string name)
        {
            for (int i = 0; i < activeTimers.Count; i++)
            {
                if (activeTimers[i]._name == name)
                {
                    activeTimers[i].DestroySelf();
                    i--;
                }

            }
        }
        private static void RemoveTimer(FunctionTimer timer)
        {
            InitIfNeeded();
            activeTimers.Remove(timer);
        }

        private class MonobehaviourHook : MonoBehaviour
        {
            public Action onUpdate;
            private void Update()
            {
                if (onUpdate != null)
                {
                    onUpdate();
                }
            }
        }
        Action _action;
        float _timer;
        string _name;
        bool _isDestroyed;
        GameObject _gameObject;
        public FunctionTimer(Action action, float timer, string name, GameObject gameObject)
        {
            _action = action;
            _timer = timer;
            _name = name;
            _isDestroyed = false;
            _gameObject = gameObject;
        }
        public void Update()
        {
            if (_isDestroyed)
            {
                return;
            }
            _timer -= Time.deltaTime;
            if (_timer < 0)
            {
                _action();
                DestroySelf();
            }
        }
        private void DestroySelf()
        {
            _isDestroyed = true;
            GameObject.Destroy(_gameObject);
            RemoveTimer(this);
        }
    }

}
