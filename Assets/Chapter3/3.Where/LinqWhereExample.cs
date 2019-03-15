using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UniRx;

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
                new Student { Name = "Jack", Age = 30},
                new Student { Name = "Jack", Age = 40}
            };

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

            // LINQ First 查询式示例
            Debug.Log(
                (from student in students select student)
                .First((Student arg1) => arg1.Age > 20)
                .Name
                );

            // UniRx First 示例代码
            Observable.EveryUpdate()
                .First(_ => Input.GetMouseButtonDown(0))
                .Subscribe(_ => Debug.Log("mouse down"))
                .AddTo(this);
        }
    }
}