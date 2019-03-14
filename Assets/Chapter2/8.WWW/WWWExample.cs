using UnityEngine;
using UniRx;

namespace UniRxLession
{
    public class WWWExample : MonoBehaviour
    {
        void Start()
        {
            ObservableWWW.Get("http://www.baidu.com/")
                .Subscribe(rs =>
                {
                    Debug.LogFormat(" baidu string {0}", rs.Substring(0, 1001));
                });

            var streamA = ObservableWWW.Get("http://sikiedu.com");
            var streamB = ObservableWWW.Get("http://www.baidu.com/");

            Observable.WhenAll(streamA, streamB)
                .Subscribe(results =>
                {
                    Debug.LogFormat("1---- {0}, 2----- {1}", results[0], results[1]);
                }).AddTo(this);
        }
    }
}