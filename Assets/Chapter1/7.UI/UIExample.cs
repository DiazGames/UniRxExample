using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using UnityEngine.Events;

namespace UniRxLession
{
    public class UIExample : MonoBehaviour
    {

        // Unity 的Event 也可以使用 AsObservable 进行订阅。
        UnityEvent mEvent;
        void Start()
        {
            Button button = transform.Find("Button").GetComponent<Button>();
            Toggle toggle = transform.Find("Toggle").GetComponent<Toggle>();

            button.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    Debug.Log("Button on clicked");
                })
                .AddTo(this);

            toggle.OnValueChangedAsObservable()
                .Subscribe(on =>
                {
                    if (on)
                    {
                        Debug.Log("toggle is on");
                    }
                });

            // toggle 通过Where过滤 简化
            toggle.OnValueChangedAsObservable()
                .Where(on => !on)
                .Subscribe(on =>
                {
                    Debug.Log("toggle is off");
                });

            // 对带有Ragcast Target标签的Graphic类型如（Text，Image，Button等），进行拖拽监听
            Graphic imgGraphic = transform.Find("Image").GetComponent<Graphic>();

            imgGraphic.OnBeginDragAsObservable().Subscribe(_ => Debug.Log("开始拖拽了！"));
            imgGraphic.OnDragAsObservable().Subscribe(_ => Debug.Log("dragging"));
            imgGraphic.OnEndDragAsObservable().Subscribe(_ => Debug.Log("end drag"));



            mEvent.AsObservable()
                .Subscribe(_ =>
                {
                    // do something
                })
                .AddTo(this);
        }
    }
}