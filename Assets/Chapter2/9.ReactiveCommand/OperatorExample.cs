using UnityEngine;
using UniRx;

namespace UniRxLession
{
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
}