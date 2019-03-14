using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace UniRxLession
{
    public class ButtonWhenAllExample : MonoBehaviour
    {
        [SerializeField] public Button mButtonA;
        [SerializeField] public Button mButtonB;
        [SerializeField] public Button mButtonC;

        void Start()
        {
            var streamA = mButtonA.OnClickAsObservable().First();
            var streamB = mButtonB.OnClickAsObservable().First();
            var streamC = mButtonC.OnClickAsObservable().First();

            Observable.WhenAll(streamA, streamB, streamC)
                .Subscribe(_ =>
                {
                    Debug.Log("All buttons were clicked once time!");
                }).AddTo(this);
        }
    }
}