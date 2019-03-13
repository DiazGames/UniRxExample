using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace UniRxLession
{
    public class LoginPanel : MonoBehaviour
    {
        Button btnLogin;
        Button btnRegister;
        InputField inpUsername;
        InputField inpPassword;
        
        void Start()
        {
            btnLogin = transform.Find("BtnLogin").GetComponent<Button>();
            btnRegister = transform.Find("BtnRegister").GetComponent<Button>();

            inpUsername = transform.Find("InpUsername").GetComponent<InputField>();
            inpPassword = transform.Find("InpPassword").GetComponent<InputField>();

            btnLogin.OnClickAsObservable().Subscribe(_ =>
            {
                Debug.Log("login button clicked");
            });

            btnRegister.OnClickAsObservable().Subscribe(_ =>
            {
                Debug.Log("btnRegister button clicked");
            });

            inpUsername.OnEndEditAsObservable()
                .Subscribe(result =>
                {
                    Debug.Log("用户名是：" + result);
                    inpPassword.Select();
                });

            inpPassword.OnEndEditAsObservable()
                .Subscribe(result =>
                {
                    btnLogin.onClick.Invoke();
                });
        }
    }
}