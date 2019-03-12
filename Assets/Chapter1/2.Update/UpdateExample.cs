using UnityEngine;
using UniRx;

namespace UniRxLession
{
    public class UpdateExample : MonoBehaviour
    {
        private void Start()
        {
            bool mButtonClicked = false;

            ButtonState mButtonState = ButtonState.None;

            // 监听鼠标左键
            Observable.EveryUpdate()
                .Subscribe(_ =>
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        Debug.Log("left mouse button clickec.");
                        mButtonClicked = true;
                    }
                });

            // 监听鼠标右键
            Observable.EveryUpdate()
                .Subscribe(_ =>
                {
                    if (Input.GetMouseButtonDown(1))
                    {
                        Debug.Log("right mouse button clickec.");
                        mButtonClicked = true;
                    }
                });

            // 监听状态
            if (mButtonClicked && mButtonState == ButtonState.None)
            {
                mButtonState = ButtonState.Clicked;
            }
        }

        enum ButtonState
        {
            None,
            Clicked
        }



        void Update()
        {
            //if (Input.GetMouseButtonDown(0))
            //{
            //    Debug.Log("left mouse button clickec.");
            //    mButtonClicked = true;
            //}

            //if (Input.GetMouseButtonDown(1))
            //{
            //    Debug.Log("right mouse button clickec.");
            //    mButtonClicked = true;
            //}

            //if (mButtonClicked && mButtonState == ButtonState.None)
            //{
            //    mButtonState = ButtonState.Clicked;
            //}
        }
    }
}