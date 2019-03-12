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