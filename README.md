# UniRxExample


# UniRx魅力

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



## 4.操作符 Where

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



## 5.操作符 First

只处理第一次鼠标点击实现：

```csh
Observable.EveryUpdate()
                .Where(_ => Input.GetMouseButtonDown(0))
                .First()
                .Subscribe(_ =>
                {
                    Debug.Log("left mouse button clicked");
                })
                .AddTo(this);
```

事件通过Where过滤后，又通过First过滤了一次，获取第一个通过的事件。

### 更好的实现

First 可以直接传一个过滤条件，不使用Where，如：

```csharp
			// 更好的实现，first直接传一个条件
            Observable.EveryUpdate()
                .First(_ => Input.GetMouseButtonDown(0))
                .Subscribe(_ =>
                {
                    Debug.Log("left mouse button clicked");
                })
                .AddTo(this);
```



## 6.AddTo()

绑定声明周期，更安全。



## 7.UGUI 的支持

Button 点击事件注册，如：

```csh
			button.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    Debug.Log("Button on clicked");
                })
                .AddTo(this);
```

Toggle 单选事件注册，如：

```cs
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
```

支持EventSystem的各种Trigger接口的监听，如：

Image本身是一个Graphic类型，实现IDragHandler就可以实现对拖拽事件的监听，但使用UniRx更方便。

Ragcast Targeet 示例：

![Ragcast Targeet 示例](http://po8veecle.bkt.clouddn.com/Raycast%20Target.jpg)

```csh
// 对带有Ragcast Target标签的Graphic类型如（Text，Image，Button等），进行拖拽监听
Graphic imgGraphic = transform.Find("Image").GetComponent<Graphic>();

imgGraphic.OnBeginDragAsObservable().Subscribe(_ => Debug.Log("开始拖拽了！"));
imgGraphic.OnDragAsObservable().Subscribe(_ => Debug.Log("dragging"));
imgGraphic.OnEndDragAsObservable().Subscribe(_ => Debug.Log("end drag"));
```

Unity 的Event 也可以使用 AsObservable 进行订阅。

```csh
            // Unity 的Event 也可以使用 AsObservable 进行订阅。
            UnityEvent mEvent;

            mEvent.AsObservable()
                .Subscribe(_ =>
                {
                    // do something
                })
                .AddTo(this);
```



## 8.ReactiveProperty 响应式属性

监听一个值发生变化，如：

```csh
using UnityEngine;
using UniRx;

namespace UniRxLession
{
    public class ReactivePropertyExample : MonoBehaviour
    {
        public ReactiveProperty<int> Age = new ReactiveProperty<int>();
        
        void Start()
        {
            Age.Subscribe(age =>
            {
                // do age
            });

            Age.Value = 5;
        }
    }
}
```

当Age的值被设置，就会通知所有 Subscribe 的回调函数，可以被 Subscribe 多次，同样支持First， Where等操作符。

这样就实现了MVP架构模式，在 Ctrl 中，进行 Model 和 View 的绑定。

Model 的所有属性都是用 ReactiveProperty，然后再 Ctrl 中进行订阅。

用过 View 更改 Model 的属性值。

形成 View -> Ctrl -> Model -> Ctrl -> View 时间相应环。



## 9.MVP 实现

### UGUI 增强

实现简单的 MVP 模式，如：

```csh
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace UniRxLession
{
    // View
    // Controller P(Presenter)
    public class EnemyExample : MonoBehaviour
    {
        EnemyModel mEnemy = new EnemyModel(200);

        void Start()
        {
            Button btnAttack = transform.Find("Button").GetComponent<Button>();
            Text txtHP = transform.Find("Text").GetComponent<Text>();

            // 点击事件监听
            btnAttack.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    mEnemy.HP.Value -= 99;
                    if (mEnemy.HP.Value <= 0)
                    {
                        mEnemy.HP.Value = 0;
                    }
                });

            // 数据变化后更新到UI
            mEnemy.HP.SubscribeToText(txtHP);
            mEnemy.IsDead
                .Where(isDead => isDead)
                .Subscribe(_ =>
                {
                    btnAttack.interactable = false;
                });
        }
    }

    // Model
    public class EnemyModel
    {
        public ReactiveProperty<long> HP;
        public IReadOnlyReactiveProperty<bool> IsDead;

        public EnemyModel(long initHP)
        {
            HP = new ReactiveProperty<long>(initHP);
            IsDead = HP.Select(hp => hp <= 0).ToReactiveProperty();
        }
    }
}
```

EnemyModel 是一个数据类，理解成 Model 层。

EnemyExample 是 P （Presenter）层，将 UI 与 Model 绑定在一起，Model 层数据有改变则通知 UI 更新。

当从 UI 接收到点击事件对 Model 进行数据更改，这就是简单的 MVP 模式。

UniRx 支持了序列化的 ReactiveProperty 类型，在编辑器上直接看到参数，如：

* IntReactiveProperty
* LongReactiveProperty
* FloatReactiveProperty
* DoubleReactiveProperty
* StringReactiveProperty
* BoolReactiveProperty
* 更多参见InspectableReactiveProperty.cs

```csha
public LongReactiveProperty showProToUIDemo;
```

![](http://po8veecle.bkt.clouddn.com/LongReactiveProperty.jpg)



### MVP 设计模式 Model - View - (Reactive) Presenter Pattern

UniRx 很容易实现MVP（MVRP）模式，结构模式如：

![UniRx 实现 MVP 模式](http://po8veecle.bkt.clouddn.com/UniRx_MVP.jpg)



## 10.操作符 Merge

UniRx 可以开启两个或多个事件流，使用 Merge 进行事件流的合并。

```csh
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
```

实现某个按钮点击时，使所有当前页面的按钮不可点击，并知道哪个按钮被点击了，如：

```csh
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;

namespace UniRxLession
{
    public class PanelEventLockExample : MonoBehaviour
    {
        void Start()
        {
            Button btnA = transform.Find("ButtonA").GetComponent<Button>();
            Button btnB = transform.Find("ButtonB").GetComponent<Button>();
            Button btnC = transform.Find("ButtonC").GetComponent<Button>();

            var eventA = btnA.OnClickAsObservable().Select(_ => "A");
            var eventB = btnB.OnClickAsObservable().Select(_ => "B");
            var eventC = btnC.OnClickAsObservable().Select(_ => "C");

            Observable.Merge(eventA, eventB, eventC)
                .First()
                .Subscribe(btnId =>
                {
                    Debug.LogFormat("button {0} clicked", btnId);

                    //1 秒后隐藏当前页面
                    Observable.Timer(TimeSpan.FromSeconds(1.0f))
                    .Subscribe(__ =>
                    {
                        gameObject.SetActive(false);
                    });
                });
        }
    }
}
```



# Unity 与 UniRx

UniRx 单独对 Unity 做了很多功能上的增强。

* UI 增强
* GameObject/MonoBehaviour 增强以及引擎的事件增强（OnApplicationPause、UnityEvent）
* Coroutine/Thread 增强
* 网络请求（WWW 等）
* ReactiveProperty、ReactiveCollection、ReactiveDictionary 等。
* ReactiveCommand 命令系统。
* ...

## UI 增强

所有的 UGUI 控件⽀支持列列出如下 :

```csh
[SerializeField] Button mButton;
[SerializeField] Toggle mToggle;
[SerializeField] Scrollbar mScrollbar;
[SerializeField] ScrollRect mScrollRect;
[SerializeField] Slider mSlider;
[SerializeField] InputField mInputField;
void Start()
{
    mButton.OnClickAsObservable().Subscribe(_ => Debug.Log("On Button
Clicked"));
    mToggle.OnValueChangedAsObservable().Subscribe(on => Debug.Log("Toggle " +
on));
	mScrollbar.OnValueChangedAsObservable().Subscribe(scrollValue =>
Debug.Log("Scrolled " + scrollValue));
    mScrollRect.OnValueChangedAsObservable().Subscribe(scrollValue =>
Debug.Log("Scrolled " + scrollValue);
    mSlider.OnValueChangedAsObservable().Subscribe(sliderValue =>
Debug.Log("Slider Value " + sliderValue));
    mInputField.OnValueChangedAsObservable().Subscribe(inputText =>
Debug.Log("Input Text: " + inputText));
    mInputField.OnEndEditAsObservable().Subscribe(result =>
Debug.Log("Result :" + result));
}
```

以上就是所有的 Observable ⽀支持。
当然除了了 Observable 增强，还⽀支持了了 Subscribe 的增强。
⽐比如 SubscribeToText

```csh
Text resultText = GetComponent<Text>();
mInputField.OnValueChangedAsObservable().SubscribeToText(resultText);
```

这段代码实现的功能是，当 mInputField 的输⼊入值改变，则会⻢马上显示在 resultText 上。
也就是完成了了，mInputField 与 resultText 的绑定。
除此之外还⽀支持，SubscribeToInteractable。