using UnityEngine;
using UniRx;

namespace UniRxLession
{
    public class MouseUpExample : MonoBehaviour
    {
        void Start()
        {
            // 创建鼠标按下事件流，返回true
            var mouseClickDownStream = Observable.EveryUpdate()
                .Where(_ => Input.GetMouseButtonDown(0))
                .Select(_ => true);

            // 创建鼠标抬起事件流，返回false
            var mouseClickUpStream = Observable.EveryUpdate()
                .Where(_ => Input.GetMouseButtonUp(0))
                .Select(_ => false);

            // 合并事件流
            var mergeStream = Observable.Merge(mouseClickDownStream, mouseClickUpStream);

            // 创建命令
            var reactiveCommand = new ReactiveCommand(mergeStream, false);

            // 订阅命令
            reactiveCommand.Subscribe(x =>
            {
                Debug.Log(x);
            });

            // 订阅Update，执行命令
            Observable.EveryUpdate()
                .Subscribe(_ =>
                {
                    reactiveCommand.Execute();
                });
        }
    }
}