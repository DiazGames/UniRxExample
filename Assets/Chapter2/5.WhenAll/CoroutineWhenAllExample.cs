using System.Collections;
using UnityEngine;
using UniRx;
using System;

namespace UniRxLession
{
    public class CoroutineWhenAllExample : MonoBehaviour
    {
        IEnumerator A()
        {
            yield return new WaitForSeconds(1.0f);
            Debug.Log("-----A");
        }

        IEnumerator B()
        {
            yield return new WaitForSeconds(2.0f);
            Debug.Log("-----B");
        }

        private void Start()
        {
            var streamA = Observable.FromCoroutine(_ => A());
            var streamB = Observable.FromCoroutine(_ => B());

            Observable.WhenAll(streamA, streamB)
                .Subscribe(_ =>
                {
                    Debug.Log("print completed");

                }).AddTo(this);
        }
    }
}