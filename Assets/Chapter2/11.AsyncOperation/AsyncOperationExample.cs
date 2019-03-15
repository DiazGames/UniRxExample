using UnityEngine;
using UniRx;
using UnityEngine.SceneManagement;

namespace UniRxLession
{
    public class AsyncOperationExample : MonoBehaviour
    {
        void Start()
        {
            var progressObservable = new ScheduledNotifier<float>();
            SceneManager.LoadSceneAsync(0).AsAsyncOperationObservable(progressObservable)
                .Subscribe(asyncOperation =>
                {
                    Debug.Log("Load done");

                    Resources.LoadAsync<GameObject>("TestCanvas")
                    .AsAsyncOperationObservable()
                    .Subscribe(resourceRequest => {
                        Instantiate(resourceRequest.asset);
                    });
                });

            progressObservable.Subscribe(progress =>
            {
                Debug.LogFormat("加载了： {0}", progress);
            });
        }
    }
}