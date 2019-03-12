# UniRxExample


# 0.UniRx介绍

UniRx是一个Unity3D编程框架，专注于解决异步逻辑，使异步逻辑实现更简洁优雅

## 如何体现

例如实现一个“只处理第一个鼠标左键点击事件”

``` csharp
using UnityEngine;
using UniRx;

namespace UniRxLession
{
    public class IntroExample : MonoBehaviour
    {
        private void Start()
        {
            Observable.EveryUpdate()                            // update 监听
                .Where(_ => Input.GetMouseButtonDown(0))        // 判断鼠标左键抬起判断
                .First()                                        // 只处理第一次点击
                .Subscribe(_ =>                                 // 订阅处理事件
                {
                    Debug.Log("mouse clicked");
                });
        }
    }
}
```

避免在Update()中充斥大量判断逻辑，代码易读。

UniRx还可以：

* 优雅实现MVP（MVC）架构模式。
* 对 UGUI/Unity API 提供了增强，大量UI逻辑，使用UniRx优雅实现。
* 轻松实现复杂的异步任务处理



## 为什么用UniRx？

UniRx 就是 Unity Reactive Extensions，擅长处理时间上异步的逻辑。

游戏开发中，开发者要实现大量异步（时间上）任务，Unity 提供了 Coroutine（协程）概念。

游戏中时间上异步逻辑：

* 动画的播放
* 声音的播放
* 网络请求
* 资源加载/卸载
* Tween
* 场景过渡
* 游戏循环（Every Update，OnCollisionEnter, etc）
* 传感器数据（Kinect，Leap Motion， VR Input， etc.）

以上逻辑实现可以使用大量回调，随着项目扩张，容易陷入“回调地狱”。

相对较好方法是使用消息/事件的发送，导致“消息满天飞”，代码难以阅读。

使用Corountine非常不错，但是Coroutine是以一个方法格式定义，写起来面向过程，逻辑复杂后Coroutine嵌套Coroutine，代码强耦合不宜阅读。

UniRx刚好解决这些问题，介于回调和事件之间。



## 1.定时功能实现

常见延时功能实现：

```csharp
using UnityEngine;

namespace UniRxLession
{
    public class CommonTimerExample : MonoBehaviour
    {
        private float mStartTime;

        private void Start()
        {
            mStartTime = Time.time;
        }

        private void Update()
        {
            if (Time.time - mStartTime > 5)
            {
                Debug.Log("do Something!");

                // 避免再次执行
                mStartTime = float.MaxValue;
            }
        }
    }
}
```

使用Coroutine更好的实现

```csharp
using System;
using System.Collections;
using UnityEngine;

namespace UniRxLession
{
    public class CoroutineTimerExample : MonoBehaviour
    {
        private void Start()
        {
            StartCoroutine(Timer(5.0f, () => {
                Debug.Log("do something!");
            }));
        }

        private IEnumerator Timer(float seconds, Action callbback)
        {
            yield return new WaitForSeconds(seconds);
            callbback();
        }
    }
}
```

 使用 UniRx 实现

```csh
using UnityEngine;
using UniRx;
using System;

namespace UniRxLession
{
    public class UniRxTimerExample : MonoBehaviour
    {
        void Start()
        {
            Observable.Timer(TimeSpan.FromSeconds(5))
                .Subscribe(_ =>
                {
                    Debug.Log("do something");
                });
        }
    }
}
```

以上代码没有和Monobehaviour进行生命周期绑定，通过.AddTo(this)方式绑定。

```csha
Observable.Timer(TimeSpan.FromSeconds(5))
                .Subscribe(_ =>
                {
                    Debug.Log("do something");
                })
                .AddTo(this);
```

 当this(Monobehaviour)Destroy的时候，这个延时逻辑也会销毁掉，避免空指针异常。



## 2.独立的Update

Update 中掺杂大量无关逻辑，如：

```csh
using UnityEngine;

namespace UniRxLession
{
    public class UpdateExample : MonoBehaviour
    {
        enum ButtonState
        {
            None,
            Clicked
        }

        private bool mButtonClicked = false;

        private ButtonState mButtonState;

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("left mouse button clickec.");
                mButtonClicked = true;
            }

            if (Input.GetMouseButtonDown(1))
            {
                Debug.Log("right mouse button clickec.");
                mButtonClicked = true;
            }

            if (mButtonClicked && mButtonState == ButtonState.None)
            {
                mButtonState = ButtonState.Clicked;
            }
        }
    }
}
```

UniRx 改善此问题

```csha
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
    }
}
```



## 3.UniRx 的基本语法格式

Observable.XXX().Subscribe() 是非常典型的 UniRx格式。

```csh
Observable.Timer(TimeSpan.FromSeconds(5))
                .Subscribe(_ =>
                {
                    Debug.Log("do something");
                })
                .AddTo(this);
```

Observable：可观察的，形容后面的（Timer）是可观察的，可以把Observable后面的理解成发布者。

Timer：定时器，被Observable描述，所以是发布者，是事件的发送方。

Subscribe：订阅，订阅前面的Timer，可以理解成订阅者，事件的接收方。

addTo：绑定生命周期。

连接起来是：可被观察（监听）的.Timer().订阅()

顺序是：订阅可被观察（监听）的定时器。

概念关系：

* Timer 是可观察的
* 可观察的才能被订阅



## 4.操作符Where

UniRx 的侧重点，不是发布者和订阅者如何使用，而是事件从发布者到订阅者之间的过程如何处理。

之间介绍Update API 的代码可使用Where优化如下：

```csha
Observable.EveryUpdate()
                .Where(_ => Input.GetMouseButtonDown(0))
                .Subscribe(_ =>
                {
                    Debug.Log("left mouse button clicked");
                })
                .AddTo(this);
```

Where 可以理解成一个条件语句，类似if，过滤掉不满足条件的事件。

```csha
if(Input.GetMouseButtonDown(0))
```

解释：

* EveryUpdate 是事件的发布者，它会每帧发送一个事件过来。
* Subscribe 是事件的接收者，接收EveryUpdate发送来的事件。
* Where 是在事件的发布者和接收者之间的一个过滤操作，过滤掉不满足条件的事件。



![](http://po8veecle.bkt.clouddn.com/4.%E6%93%8D%E4%BD%9C%E7%AC%A6Where%E7%A4%BA%E4%BE%8B%E5%9B%BE.jpg)

事件的本身可以是参数，但是 EveryUpdate 没有参数，所以在Where中不需要接收参数，使用 _ 来表示不用参数。

