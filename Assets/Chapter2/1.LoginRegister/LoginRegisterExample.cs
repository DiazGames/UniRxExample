using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UniRxLession
{
    public class LoginRegisterExample : MonoBehaviour
    {
        public LoginPanel Loginpanel;
        public RegisterPanel RegisterPanel;

        public static LoginRegisterExample PanelMgr;

        private void Awake()
        {
            PanelMgr = this;
        }

        void Start()
        {
            Loginpanel = transform.Find("LoginPanel").GetComponent<LoginPanel>();
            RegisterPanel = transform.Find("RegisterPanel").GetComponent<RegisterPanel>();

            Loginpanel.gameObject.SetActive(true);
            RegisterPanel.gameObject.SetActive(false);
        }
    }
}