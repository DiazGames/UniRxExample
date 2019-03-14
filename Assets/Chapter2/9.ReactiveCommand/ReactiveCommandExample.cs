using UnityEngine;
using UniRx;

namespace UniRxLession
{
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
}