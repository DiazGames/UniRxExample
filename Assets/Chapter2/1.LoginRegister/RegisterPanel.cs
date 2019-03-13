using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace UniRxLession
{
    public class RegisterPanel : MonoBehaviour
    {
        Button btnBack;
        Button btnRegister;
        InputField inpUsername;
        InputField inpPassword1;
        InputField inpPassword2;

        void Start()
        {
            btnBack = transform.Find("BtnBack").GetComponent<Button>();
            btnRegister = transform.Find("BtnRegister").GetComponent<Button>();
            inpUsername = transform.Find("InpUsername").GetComponent<InputField>();
            inpPassword1 = transform.Find("InpPassword1").GetComponent<InputField>();
            inpPassword2 = transform.Find("InpPassword2").GetComponent<InputField>();


            btnBack.OnClickAsObservable().Subscribe(_ =>
            {
                gameObject.SetActive(false);
                LoginRegisterExample.PanelMgr.Loginpanel.gameObject.SetActive(true);
            });

            btnRegister.OnClickAsObservable().Subscribe(_ =>
            {
                Debug.Log("btnRegister button clicked");
            });

            inpUsername.OnEndEditAsObservable()
                .Subscribe(result =>
                {
                    Debug.Log("用户名是：" + result);
                    inpPassword1.Select();
                });

            inpPassword1.OnEndEditAsObservable()
                .Subscribe(result =>
                {
                    inpPassword2.Select();
                });

            inpPassword2.OnEndEditAsObservable()
                .Subscribe(result =>
                {
                    btnRegister.onClick.Invoke();
                });
        }

    }
}