using UnityEngine;
using UniRx;

namespace UniRxLession
{
    public class FirstExample : MonoBehaviour
    {
        void Start()
        {
            //Observable.EveryUpdate()
                //.Where(_ => Input.GetMouseButtonDown(0))
                //.First()
                //.Subscribe(_ =>
                //{
                //    Debug.Log("left mouse button clicked");
                //})
                //.AddTo(this);

            // 更好的实现，first直接传一个条件
            Observable.EveryUpdate()
                .First(_ => Input.GetMouseButtonDown(0))
                .Subscribe(_ =>
                {
                    Debug.Log("left mouse button clicked");
                })
                .AddTo(this);
        }
    }
}