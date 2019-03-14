using UnityEngine;
using UniRx;

namespace UniRxLession
{
    public class ReactiveCollectionExample : MonoBehaviour
    {
        ReactiveCollection<int> collection = new ReactiveCollection<int>
        {
            1,2,3,4,5
        };
        void Start()
        {
            foreach (var item in collection)
            {
                Debug.Log(item);
            }

            // 集合添加数据时订阅
            collection.ObserveAdd()
                .Subscribe(addValue =>
                {
                    Debug.Log("added " + addValue);
                });

            // 集合移除数据时订阅
            collection.ObserveRemove()
                .Subscribe(removeValue =>
                {
                    Debug.Log("remove " + removeValue);
                });

            // 集合每次数据变动监听每
            collection.ObserveCountChanged()
                .Subscribe(count =>
                {
                    Debug.Log("collection count " + count);
                });

            collection.Add(6);
            collection.Remove(2);
        }
    }
}