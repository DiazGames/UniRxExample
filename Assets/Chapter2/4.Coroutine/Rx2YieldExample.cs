using System.Collections;
using UnityEngine;
using UniRx;
using System;

namespace UniRxLession
{
    public class Rx2YieldExample : MonoBehaviour
    {
        IEnumerator Delay1Second()
        {
            yield return Observable.Timer(TimeSpan.FromSeconds(1.0f)).ToYieldInstruction();
            Debug.Log("--------B");
        }

        // Use this for initialization
        void Start()
        {
            StartCoroutine(Delay1Second());
        }
    }
}
