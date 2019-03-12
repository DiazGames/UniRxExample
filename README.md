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