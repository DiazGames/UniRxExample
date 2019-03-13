using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace UniRxLession
{
    public class UnityAPIExample : MonoBehaviour
    {
        void Start()
        {
            // 支持⾮常多细分类型的 Update 事件捕获。
            //Observable.EveryFixedUpdate().Subscribe(_ => { Debug.Log("EveryFixedUpdate"); });
            //Observable.EveryEndOfFrame().Subscribe(_ => { Debug.Log("EveryEndOfFrame"); });
            //Observable.EveryLateUpdate().Subscribe(_ => { Debug.Log("EveryLateUpdate"); });
            //Observable.EveryAfterUpdate().Subscribe(_ => { Debug.Log("EveryAfterUpdate"); });

            Observable.EveryApplicationPause().Subscribe(paused => { });
            Observable.EveryApplicationFocus().Subscribe(focused => { });
            Observable.OnceApplicationQuit().Subscribe(_ => { });

        }
    }
}