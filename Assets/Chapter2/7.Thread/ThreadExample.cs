using UnityEngine;
using UniRx;
using System;
using System.Threading;

namespace UniRxLession
{
    public class ThreadExample : MonoBehaviour
    {
        void Start()
        {
            var threadAStream = 
            Observable.Start(() =>                             // 开启一个线程流
            {
                Thread.Sleep(TimeSpan.FromSeconds(1.0f));
                return 10;
            });

            var threadBStream = 
            Observable.Start(() =>                             // 开启一个线程流
            {
                Thread.Sleep(TimeSpan.FromSeconds(2.0f));
                return 10;
            });

            Observable.WhenAll(threadAStream, threadBStream)
                .ObserveOnMainThread()                         // WhenAll的结果转回到主线程
                .Subscribe(rs =>
                {
                    Debug.Log(rs[0] + ":" + rs[1]);
                });
        }
    }
}
