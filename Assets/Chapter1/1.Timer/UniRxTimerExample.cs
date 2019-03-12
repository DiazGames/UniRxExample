using UnityEngine;
using UniRx;
using System;

namespace UniRxLession
{
    public class UniRxTimerExample : MonoBehaviour
    {
        void Start()
        {
            Observable.Timer(TimeSpan.FromSeconds(5))
                .Subscribe(_ =>
                {
                    Debug.Log("do something");
                })
                .AddTo(this);
        }
    }
}