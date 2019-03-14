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

```csharp
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

```csharp
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

```csharp
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

```csharp
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

```csharp
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

```csharp
Observable.EveryUpdate()
                .Where(_ => Input.GetMouseButtonDown(0))
                .Subscribe(_ =>
                {
                    Debug.Log("left mouse button clicked");
                })
                .AddTo(this);
```

Where 可以理解成一个条件语句，类似if，过滤掉不满足条件的事件。

```csharp
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

```csharp
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

```csharp
			button.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    Debug.Log("Button on clicked");
                })
                .AddTo(this);
```

Toggle 单选事件注册，如：

```csharp
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

```csharp
// 对带有Ragcast Target标签的Graphic类型如（Text，Image，Button等），进行拖拽监听
Graphic imgGraphic = transform.Find("Image").GetComponent<Graphic>();

imgGraphic.OnBeginDragAsObservable().Subscribe(_ => Debug.Log("开始拖拽了！"));
imgGraphic.OnDragAsObservable().Subscribe(_ => Debug.Log("dragging"));
imgGraphic.OnEndDragAsObservable().Subscribe(_ => Debug.Log("end drag"));
```

Unity 的Event 也可以使用 AsObservable 进行订阅。

```csharp
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

```csharp
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

```csharp
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

```csharp
public LongReactiveProperty showProToUIDemo;
```

![](http://po8veecle.bkt.clouddn.com/LongReactiveProperty.jpg)



### MVP 设计模式 Model - View - (Reactive) Presenter Pattern

UniRx 很容易实现MVP（MVRP）模式，结构模式如：

![UniRx 实现 MVP 模式](http://po8veecle.bkt.clouddn.com/UniRx_MVP.jpg)



## 10.操作符 Merge

UniRx 可以开启两个或多个事件流，使用 Merge 进行事件流的合并。

```csharp
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

```csharp
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

## 1. UI 增强

所有的 UGUI 控件支持列出如下 :

```csharp
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

以上就是所有的 Observable ⽀持。
当然除了 Observable 增强，还支持了 Subscribe 的增强。
比如 SubscribeToText

```csharp
Text resultText = GetComponent<Text>();
mInputField.OnValueChangedAsObservable().SubscribeToText(resultText);
```

这段代码实现的功能是，当 mInputField 的输⼊入值改变，则会⻢上显示在 resultText 上。
也就是完成了，mInputField 与 resultText 的绑定。
除此之外还支持，SubscribeToInteractable。



## 2. Unity 声明周期 与 Trigger

UniRx 支持非常多的细分 Update 事件捕获，如：

```csharp
// 支持⾮常多细分类型的 Update 事件捕获。
Observable.EveryFixedUpdate().Subscribe(_ => { Debug.Log("EveryFixedUpdate"); });
Observable.EveryEndOfFrame().Subscribe(_ => { Debug.Log("EveryEndOfFrame"); });
Observable.EveryLateUpdate().Subscribe(_ => { Debug.Log("EveryLateUpdate"); });
Observable.EveryAfterUpdate().Subscribe(_ => { Debug.Log("EveryAfterUpdate"); });
```

支持其他事件，如

```csharp
Observable.EveryApplicationPause().Subscribe(paused => { });
Observable.EveryApplicationFocus().Subscribe(focused => { });
Observable.OnceApplicationQuit().Subscribe(_ => { });
```

不用再去创建一个单例类去实现⼀个诸如“应用程序退出事件监听”这种逻辑

### Trigger 简介

Observable.EveryUpdate() 这个 API 有的时候在某个脚本中实现，需要绑定 MonoBehaviour 的生命周 

期(主要是 OnDestroy)，当然也有的时候是全局的，而且永远不会被销毁的。 

需要绑定 MonoBehaviour 生命周期的 EveryUpdate。只需要一个 **AddTo(this)**  就可以进⾏行行绑定了。

更简洁的实现：

```csharp
this.UpdateAsObservable().Subscribe(_ => { });
```

这种类型的 Observable 就是 Trigger 触发器。

### Trigger 类型的关键字

当某个事件发生时，会将该事件发送到 Subscribe 函数中，触发器本身是一个挂在 Gameobject 上的功能脚本，来监听 GameObject 的某个事件发生，事件发生则会回调给注册它的 Subscribe 中。

Trigger 同样支持Where，First，Merge 等操作符。

* Trigger 大部分都是XXXAsObservable 命名形式
* 使用 Trigger 的 GameObject 上都会挂对应的 ObservableXXXTrigger.cs脚本。

AddTo() 这个API 其实是封装了一种 Trigger：ObservableDestroyTrigger。

各种细分类型的 Update：

```csharp
this.FixedUpdateAsObservable().Subscribe(_ => {});
this.LateUpdateAsObservable().Subscribe(_ => {});
this.UpdateAsObservable().Subscribe(_ => {});
```

各种碰撞的 Trigger：

```csharp
this.OnCollisionEnterAsObservable(collision => {});
this.OnCollisionExitAsObservable(collision => {});
this.OnCollisionStayAsObservable(collision => {});
// 同样 2D 的也支持
this.OnCollision2DEnterAsObservable(collision => {});
this.OnCollision2DExitAsObservable(collision => {});
this.OnCollision2DStayAsObservable(collision => {});
```

一些脚本的参数监听：

```csharp
this.OnEnableAsObservable().Subscribe(_ => {});
this.OnDisableAsObservable().Subscribe(_ => {});
```

除了 Monobehaviour，Trigger 也支持其他组件类型：

* RectTransform
* Transform
* UIBehaviour
* 更多查看 ObservableTriggerExtensions.cs 和 ObservableTriggerExtensions.Component.cs 

## 3. UI Trigger

Trigger 对 UIBehavior 的支持。

UIBehavior 是 UGUI 所有控件的基类。

比如所有的 Graphic 类型（带有 Raycast Target 选定框）都支持 OnPointDownAsObservable，OnPointEnterAsObservable ，OnPointExitAsObservable 等 Trigger。

项目中用的比较多的几个 Trigger：

```csharp
imgGraphic.OnBeginDragAsObservable().Subscribe(_ => Debug.Log("开始拖拽了！"));
imgGraphic.OnDragAsObservable().Subscribe(_ => Debug.Log("dragging"));
imgGraphic.OnEndDragAsObservable().Subscribe(_ => Debug.Log("end drag"));
imgGraphic.OnPointerClickAsObservable().Subscribe(clickEvent => { });
```

使用各种 Trigger 类型，需要导入命名空间：

```csharp
using UniRx.Triggers;
```

## 4. Coroutine 的操作

UniRx 可以将一个 Coroutine 转化成一个事件源（Observable）如：

```csharp
public class CoroutineExample : MonoBehaviour
    {
        IEnumerator CoroutineA()
        {
            yield return new WaitForSeconds(1.0f);
            Debug.Log("--------- A");
        }

        private void Start()
        {
            Observable.FromCoroutine(_ => CoroutineA())
                .Subscribe(_ =>
                {
                    Debug.Log("1 second later print complete!");
                }).AddTo(this);
        }
    }
```

将 Observable 转为一个 Coroutine 中的 yield 对象，如：

```csharp
public class Rx2YieldExample : MonoBehaviour
    {
        IEnumerator Delay1Second()
        {
            yield return Observable.Timer(TimeSpan.FromSeconds(1.0f)).ToYieldInstruction();
            Debug.Log("--------B");
        }

        // Use this for initialization
        void Start()
        {
            StartCoroutine(Delay1Second());
        }
    }
```

UniRx 支持顺序执行 Coroutine，并行执行 Coroutine 等，可以让 Coroutine 更加强大。

## 5. WhenAll : Coroutine 并行操作

当所有的事件流都结束，触发 Subscribe 注册的回调。

使用 WhenAll 可以实现 Coroutine 的并行操作，如：

```csharp
public class CoroutineWhenAllExample : MonoBehaviour
    {
        IEnumerator A()
        {
            yield return new WaitForSeconds(1.0f);
            Debug.Log("-----A");
        }

        IEnumerator B()
        {
            yield return new WaitForSeconds(2.0f);
            Debug.Log("-----B");
        }

        private void Start()
        {
            var streamA = Observable.FromCoroutine(_ => A());
            var streamB = Observable.FromCoroutine(_ => B());

            Observable.WhenAll(streamA, streamB)
                .Subscribe(_ =>
                {
                    Debug.Log("print completed");

                }).AddTo(this);
        }
    }
```

WhenAll 和 Merge 是同类型的，处理多个流的操作符。

实现多个按钮都点击过一次的逻辑，如：

```csharp
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
                    Debug.Log("All buttons were clicked");
                }).AddTo(this);
        }
    }
```

## 6. 事件流结束 OnCompleted

使用 Subscribe API 订阅的时候，第一个参数是 OnNext 回调的注册，第二个参数是 OnCompleted 事件完成，第三个参数是 OnError，代码如下：

```csharp
public class OnCompletedExample : MonoBehaviour
    {
        void Start()
        {
            Observable.Timer(TimeSpan.FromSeconds(1.0f))
            .Subscribe(_ =>
            {
                Debug.Log("OnNext: 1 second");
            }, () => {
                Debug.Log("OnCompleted!");
            });

            Observable.FromCoroutine(A)
                .Subscribe(_ =>
                {
                    Debug.Log("OnNext: 2 second");
                }, () => {
                    Debug.Log("OnCompleted!");
                });
        }

        private IEnumerator A()
        {
            yield return new WaitForSeconds(2.0f);
        }
    }
```

## 7.Start:让多线程更简单

UniRx 改善了Unity 使用 Thread.Start 开启线程，当逻辑复杂的时候多线程难以管理的状况。

实现”当所有线程运行完成后，在主线程执行某个任务“，如：

```csharp
public class ThreadExample : MonoBehaviour
    {
        void Start()
        {
            var threadAStream = 
            Observable.Start(() =>                             // 开启一个线程流
            {
                Thread.Sleep(TimeSpan.FromSeconds(1.0f));
                return 10;
            });

            var threadBStream = 
            Observable.Start(() =>                             // 开启一个线程流
            {
                Thread.Sleep(TimeSpan.FromSeconds(2.0f));
                return 10;
            });

            Observable.WhenAll(threadAStream, threadBStream)
                .ObserveOnMainThread()                         // WhenAll的结果转回到主线程
                .Subscribe(rs =>
                {
                    Debug.Log(rs[0] + ":" + rs[1]);
                });
        }
    }
```

## 8. WWWObservable 优雅的网络请求

UniRx 网络请求，同样支持 WhenAll 操作符。

```csharp
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
```

下载文件实现：

```csharp
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
```

ObservableWWW 的 API 都可以穿进去一个 ScheduledNotifier<T>()，用来监听下载进度的。

Subscribe 传回来的值是当前的进度。

## 9.ReactiveCommand

ReactiveCommand 定义

```csharp
public interface IReactiveCommand<T> : IObservable<T>
{
	IReadOnlyReactiveProperty<bool> CanExecute { get; }
	bool Execute(T parameter);
}
```

提供了两个 API：

* CanExecute
* Execute

Execute 方法被外部调用，Command 执行。

CanExecute 内部使用，对外部提供只读访问。

当 CanExecute 为 false 时，外部调用 Execute 则该 Command 不会被执行。

当 CanExecute 为 true 时，外部调用 Execute 则该 Command 会被执行。

创建新的 ReactiveCommand 默认 CanExecute 为 true。

 ```csharp
public class ReactiveCommandExample : MonoBehaviour
    {
        void Start()
        {
            ReactiveCommand command = new ReactiveCommand();

            command.Subscribe(_ =>
            {
                Debug.Log("Command executed!");
            });

            command.Execute();
            command.Execute();
        }
    }
 ```

实现鼠标按下持续输出，当抬起鼠标时，则停止输出。

```csharp
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
```

ReactiveCommand 也可以被订阅（Subscribe），订阅之前，也可以使用 Where 等操作符操作。

```csharp
public class OperatorExample : MonoBehaviour
    {
        void Start()
        {
            var command = new ReactiveCommand<int>();

            command.Where(x => (x % 2 == 0))
                .Subscribe(x =>
                {
                    Debug.LogFormat("{0} is Even Number.", x);
                });

            command.Where(x => (x % 2 != 0))
                .Timestamp()
                .Subscribe(x =>
                {
                    Debug.LogFormat("{0} is Odd Number.{1}", x.Value, x.Timestamp);
                });

            command.Execute(2);     //输出 2 is Even Number.
            command.Execute(3);     //输出 3 is Odd Number.03/14/2019 07:54:44 +00:00
        }
    }
```







