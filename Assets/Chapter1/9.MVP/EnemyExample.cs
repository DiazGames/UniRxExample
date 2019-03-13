using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace UniRxLession
{
    // View
    // Controller P(Presenter)
    public class EnemyExample : MonoBehaviour
    {
        public LongReactiveProperty showProToUIDemo;

        private EnemyModel mEnemy = new EnemyModel(200);

        void Start()
        {
            Button btnAttack = transform.Find("Button").GetComponent<Button>();
            Text txtHP = transform.Find("Text").GetComponent<Text>();

            // 点击事件监听
            btnAttack.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    mEnemy.HP.Value -= 99;
                    if (mEnemy.HP.Value <= 0)
                    {
                        mEnemy.HP.Value = 0;
                    }
                });

            // 数据变化后更新到UI
            mEnemy.HP.SubscribeToText(txtHP);
            mEnemy.IsDead
                .Where(isDead => isDead)
                .Subscribe(_ =>
                {
                    btnAttack.interactable = false;
                });
        }
    }

    // Model
    public class EnemyModel
    {
        public ReactiveProperty<long> HP;
        public IReadOnlyReactiveProperty<bool> IsDead;

        public EnemyModel(long initHP)
        {
            HP = new ReactiveProperty<long>(initHP);
            IsDead = HP.Select(hp => hp <= 0).ToReactiveProperty();
        }
    }
}