using UnityEngine;
using UniRx;
using System.Linq;
using System.Collections.Generic;

namespace UniRxLession
{
    public class LinqExample : MonoBehaviour
    {
        void Start()
        {
            var testList = new List<int> { 1, 2, 3 };

            // ForEach 的 Linq 语法
            testList.ForEach((int obj) =>
            {
                Debug.Log("item : " + obj);
            });

            // UniRx 操作符适用
            var firstItem = testList.First();
            Debug.Log("Use UniRx First() get first item : " + firstItem);

            testList.Where(num => num > 1)
                .ToList()
                .ForEach((int obj) =>
                {
                    Debug.Log("> 1 的item：" + obj);
                });
        }

    }
}
