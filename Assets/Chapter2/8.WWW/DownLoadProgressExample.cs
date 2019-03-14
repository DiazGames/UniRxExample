using UnityEngine;
using UniRx;

namespace UniRxLession
{
    public class DownLoadProgressExample : MonoBehaviour
    {
        void Start()
        {
            var progressObservable = new ScheduledNotifier<float>();

            ObservableWWW
                .GetAndGetBytes("http://po8veecle.bkt.clouddn.com/UniRx_MVP.jpg",
                    null,
                    progressObservable)
                .Subscribe(bytes =>
                {
                    Debug.Log("文件大小为：" + (bytes.Length / 1000) + "k");
                });

            progressObservable.Subscribe(progress =>
            {
                Debug.LogFormat("j进度为：{0}", progress);
            });
        }
    }
}