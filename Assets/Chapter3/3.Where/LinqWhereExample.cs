using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;

namespace UniRxLession
{
    public class LinqWhereExample : MonoBehaviour
    {
        class Student
        {
            public string Name;
            public int Age;
        }

        private void Start()
        {
            var students = new List<Student>
            {
                new Student { Name = "Jack", Age = 20},
                new Student { Name = "Jack Ma", Age = 30},
                new Student { Name = "Jack", Age = 40}
            };

            /*
            // 通过 Where 条件筛选
            students.Where((Student arg1) => arg1.Age > 20)
                .ToList()
                .ForEach((Student obj) =>
                {
                    Debug.Log(obj.Name);
                });

            // 使用查询句式
            (from student in students where student.Age > 20 select student)
                .ToList()
                .ForEach((Student obj) =>
                {
                    Debug.Log(obj.Name);
                });

            // UniRx 查询式示例
            (from updateEvent in Observable.EveryUpdate()
             where Input.GetMouseButtonDown(0)
             select updateEvent)
            .Subscribe(_ =>
            {
                Debug.Log("Mouse Down!");
            });

            // LINQ Select 示例
            (students.Where((Student arg1) => arg1.Age > 20)
                .Select((Student arg1) => arg1.Name))
                .ToList()
                .ForEach((string obj) => Debug.Log(obj));

            // LINQ Select 查询式示例
            (from student in students where student.Age > 20 select student.Name)
                .ToList()
                .ForEach(name =>
                {
                    Debug.Log(name);
                });

            // UniRx Select 示例
            Observable.EveryUpdate()
                .Where(_ => Input.GetMouseButtonUp(0))
                .Select(_ => "mouse up")
                .Subscribe(Debug.Log)
                .AddTo(this);

            // UniRx Select 查询式示例
            (from updateEvent in Observable.EveryUpdate()
             where Input.GetMouseButtonUp(0)
             select "mouse up")
            .Subscribe(Debug.Log)
            .AddTo(this);

            // LINQ First 示例 两种方式
            Debug.Log(students.Where((Student arg1) => arg1.Age > 20).First().Name);
            Debug.Log(students.First((Student arg1) => arg1.Age > 20).Name);


            // UniRx First 示例代码
            Observable.EveryUpdate()
                .First(_ => Input.GetMouseButtonDown(0))
                .Subscribe(_ => Debug.Log("mouse down"))
                .AddTo(this);

            

            // LINQ Distinct 示例
            students.Where((Student arg1) => arg1.Age > 1)
                .Select((Student arg1) => arg1.Name)
                .Distinct()
                .ToList()
                .ForEach((string obj) => Debug.Log(obj));

            // LINQ Distinct 查询式示例
            (from student in students select student.Name)
            .Distinct()
            .ToList()
            .ForEach((string obj) => Debug.Log(obj));

            // UniRx Distinct 示例
            students.ToObservable()
                .Distinct((Student arg1) => arg1.Name)
                .Subscribe(student =>
                {
                    Debug.Log(student.Name);
                });

            

            // Linq Last
            Debug.Log(students.Last().Name);

            // UniRx Last 支持传入一个条件函数
            students.ToObservable()
                .Last(student => student.Age > 20)
                .Subscribe(student => Debug.Log(student.Name));

            // SelectMany 示例
            (students.SelectMany((Student arg1) => arg1.Name + ":" + arg1.Age))
                .ToList()
                .ForEach((char obj) => Debug.Log(obj));

            

            // Take 操作符
            //students.Take(2).ToList().ForEach((Student obj) => Debug.Log(obj.Name));

            this.UpdateAsObservable()
                .Where(_ => Input.GetMouseButtonDown(0))
                .Take(5)
                .Subscribe(_ =>
                {
                    Debug.Log("前5次点击输出");
                });
            

            // concat 操作符 链接两个或多个序列
            (students.Select(student => student.Name).Concat(students.Select(student => student.Name)))
                .ToList()
                .ForEach((string obj) => Debug.Log("名字：" + obj));
            
            

            // OfType 操作符 过滤类型
            // 创建一个 Subject
            var objects = new Subject<object>();

            // 订阅Observable，进行类型过滤
            objects.OfType<object, string>()
                .Subscribe(Debug.Log);

            // 手动发送数据
            objects.OnNext(1);
            objects.OnNext(2);
            objects.OnNext("3");
            objects.OnNext(4);

            // 手动结束
            objects.OnCompleted();


            // Cast 操作符 转换类型
            var objects = new Subject<object>();

            // 订阅Observable，进行类型转换
            objects.Cast<object, int>()
                .Subscribe(i => Debug.Log(i));

            // 手动发送数据
            objects.OnNext(1);
            objects.OnNext(2);
            objects.OnNext(4);

            // 手动结束
            objects.OnCompleted();

            // GroupBy 操作符
            students.GroupBy((Student arg1) => arg1.Name)
                .ToList()
                .ForEach((IGrouping<string, Student> obj) => Debug.Log(obj.Key));
            
            // Range 操作符 生成指定范围内的整数序列
            Observable.Range(5, 10)
                .Select((int arg1) => arg1 * arg1)
                .Skip(3)    // 跳过前三个
                .Subscribe(x => Debug.Log(x));

            // TakeWhile 操作符 如果指定的条件为 true，则返回序列中的元素，然后跳过剩余的元素。
            students.TakeWhile((Student arg1) => arg1.Name == "Jack")
                .ToList()
                .ForEach((Student obj) => Debug.Log(obj.Name + ":" + obj.Age));
            
            // SkipWhile 操作符 如果指定的条件为 true，则跳过序列中的元素，然后返回剩余的元素。
            students.SkipWhile((Student arg1) => arg1.Name == "Jack")
                .ToList()
                .ForEach((Student obj) => Debug.Log(obj.Name + ":" + obj.Age));

            */

            // Repeat 操作 在生成序列中重复该值的次数。
            Enumerable.Repeat("Hello world", 5)
                .ToList()
                .ForEach((string obj) => Debug.Log(obj));
            
        }
    }
}