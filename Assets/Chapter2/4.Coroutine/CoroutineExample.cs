using System.Collections;
using UnityEngine;
using UniRx;

namespace UniRxLession
{
    public class CoroutineExample : MonoBehaviour
    {
        IEnumerator CoroutineA()
        {
            yield return new WaitForSeconds(1.0f);
            Debug.Log("--------- A");
        }

        private void Start()
        {
            Observable.FromCoroutine(_ => CoroutineA())
                .Subscribe(_ =>
                {
                    Debug.Log("1 second later print complete!");
                }).AddTo(this);
        }
    }

}