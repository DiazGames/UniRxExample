using UnityEngine;
using UniRx;

namespace UniRxLession
{
    public class MergeExample : MonoBehaviour
    {
        private void Start()
        {
            var leftClickEvent = Observable.EveryUpdate()
            .Where(_ => Input.GetMouseButtonDown(0));

            var rightClickEvent = Observable.EveryUpdate()
            .Where(_ => Input.GetMouseButtonDown(1));

            Observable.Merge(leftClickEvent, rightClickEvent)
                .Subscribe(_ =>
                {
                    Debug.Log("当鼠标左键或右键点击时都会进行处理");
                });
        }
    }
}