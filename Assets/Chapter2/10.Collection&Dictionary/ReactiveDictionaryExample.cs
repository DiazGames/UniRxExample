using UnityEngine;
using UniRx;

namespace UniRxLession
{
    public class ReactiveDictionaryExample : MonoBehaviour
    {
        ReactiveDictionary<string, string> dic = new ReactiveDictionary<string, string>()
        {
            {"en","英语"},
            {"cn","中文"}
        };

        void Start()
        {
            foreach (var item in dic)
            {
                Debug.LogFormat("key:{0}, value:{1}", item.Key, item.Value);
            }

            dic.ObserveAdd()
                .Subscribe(item =>
                {
                    Debug.LogFormat("Add key:{0}, value:{1}", item.Key, item.Value);
                });

            dic.ObserveRemove()
                .Subscribe(item =>
                {
                    Debug.LogFormat("Remove key:{0}, value:{1}", item.Key, item.Value);
                });

            dic.ObserveCountChanged()
                .Subscribe(count =>
                {
                    Debug.Log("Dic count " + count);
                });

            dic.Add("fn", "法语");
            dic.Remove("en");
        }
    }
}
