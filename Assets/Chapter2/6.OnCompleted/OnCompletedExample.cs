using UnityEngine;
using UniRx;
using System;
using System.Collections;

namespace UniRxLession
{
    public class OnCompletedExample : MonoBehaviour
    {
        void Start()
        {
            Observable.Timer(TimeSpan.FromSeconds(1.0f))
            .Subscribe(_ =>
            {
                Debug.Log("OnNext: 1 second");
            }, () => {
                Debug.Log("OnCompleted!");
            });

            Observable.FromCoroutine(A)
                .Subscribe(_ =>
                {
                    Debug.Log("OnNext: 2 second");
                }, () => {
                    Debug.Log("OnCompleted!");
                });
        }

        private IEnumerator A()
        {
            yield return new WaitForSeconds(2.0f);
        }
    }
}