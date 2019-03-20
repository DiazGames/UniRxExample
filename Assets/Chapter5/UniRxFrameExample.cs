﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using System.Threading;

namespace UniRxLession
{
    public class UniRxFrameExample : MonoBehaviour
    {
        void Start()
        {
            /*
            Debug.Log(Time.frameCount);
            Observable.NextFrame()
                .Subscribe(_ => Debug.Log(Time.frameCount));    // 输出结果 1, 2

            Debug.Log(Time.frameCount);
            Observable.ReturnUnit()
                .DelayFrame(10)
                .Subscribe(_ => Debug.Log(Time.frameCount));    // 输出结果 1, 12

            // 输出距离上一次鼠标点击所间隔的帧数
            Observable.EveryUpdate()
                .Where(_ => Input.GetMouseButtonDown(0))
                .FrameInterval()
                .Subscribe(frameInterval => Debug.Log(frameInterval.Interval));

            // 收集每 100 帧内的点击事件，然后进行统一的输出
            Observable.EveryUpdate()
                .Where(_ => Input.GetMouseButtonDown(0))
                .BatchFrame(100, FrameCountType.EndOfFrame)
                .Subscribe(clicks =>
                {
                    Debug.Log(clicks.Count);
                });

            // 输出 0 ~ 9
            Observable.Range(0, 10)
                .ForEachAsync(number => Debug.Log(number))
                .Subscribe();

            // 当点击鼠标时，返回距离上一次点击的 帧总时间 如：帧总时间：00:00:01.0960000
            Observable.EveryUpdate()
                .Where(_ => Input.GetMouseButtonDown(0))
                .FrameTimeInterval()
                .Subscribe(frameTimeInterval => Debug.Log("帧总时间：" + frameTimeInterval.Interval));

            // 每隔 5 帧，输出一次
            Observable.EveryUpdate()
                .SampleFrame(5)
                .Subscribe(_ => Debug.Log(Time.frameCount));

            // 每隔 1 秒输出一次，直到GameObject删除掉
            Observable.Timer(TimeSpan.FromSeconds(1.0f))
                .RepeatUntilDestroy(this)
                .Subscribe(_ => Debug.Log("ticked"));
                */

            // 2 秒后在主线程输出
            Debug.Log(Time.time);
            Observable.Start(() =>
            {
                Thread.Sleep(TimeSpan.FromSeconds(1.0f));
                return 1;
            }).ObserveOnMainThread()
            .Subscribe(threadResult => Debug.LogFormat("{0} : {1}", threadResult, Time.time));
        }
    }
}