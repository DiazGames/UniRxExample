using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;

namespace UniRxLession
{
    public class PanelEventLockExample : MonoBehaviour
    {
        void Start()
        {
            Button btnA = transform.Find("ButtonA").GetComponent<Button>();
            Button btnB = transform.Find("ButtonB").GetComponent<Button>();
            Button btnC = transform.Find("ButtonC").GetComponent<Button>();

            var eventA = btnA.OnClickAsObservable().Select(_ => "A");
            var eventB = btnB.OnClickAsObservable().Select(_ => "B");
            var eventC = btnC.OnClickAsObservable().Select(_ => "C");

            Observable.Merge(eventA, eventB, eventC)
                .First()
                .Subscribe(btnId =>
                {
                    Debug.LogFormat("button {0} clicked", btnId);

                    //1 秒后隐藏当前页面
                    Observable.Timer(TimeSpan.FromSeconds(1.0f))
                    .Subscribe(__ =>
                    {
                        gameObject.SetActive(false);
                    });
                });
        }
    }
}