using UnityEngine;
using System.Collections.Generic;

namespace DuloGames.UI
{
    public class UIWindowManager : MonoBehaviour
    {
        private static UIWindowManager m_Instance;
        public static UIWindowManager Instance { get { return m_Instance; } }

        [SerializeField] private string m_EscapeInputName = "Cancel";
        private bool m_EscapeUsed = false;

        public string escapeInputName { get { return this.m_EscapeInputName; } }
        public bool escapedUsed { get { return this.m_EscapeUsed; } }

        public GameObject gameMenuWindow;

        protected virtual void Awake()
        {
            if (m_Instance == null)
            {
                m_Instance = this;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        protected virtual void OnDestroy()
        {
            if (m_Instance != null && m_Instance.Equals(this))
                m_Instance = null;
        }

        protected virtual void Update()
        {
            if (this.m_EscapeUsed)
                this.m_EscapeUsed = false;

            if (Input.GetButtonDown(this.m_EscapeInputName))
            {
                List<UIWindow> windows = UIWindow.GetWindows();

                foreach (UIWindow window in windows)
                {
                    if (window.escapeKeyAction != UIWindow.EscapeKeyAction.None)
                    {
                        if (window.IsOpen && (window.escapeKeyAction == UIWindow.EscapeKeyAction.Hide || window.escapeKeyAction == UIWindow.EscapeKeyAction.Toggle || (window.escapeKeyAction == UIWindow.EscapeKeyAction.HideIfFocused && window.IsFocused)))
                        {
                            window.Hide();
                            this.m_EscapeUsed = true;
                        }
                    }
                }

                if (this.m_EscapeUsed)
                {
                    ResumeGame();
                    return;
                }

                foreach (UIWindow window in windows)
                {
                    if (!window.IsOpen && window.escapeKeyAction == UIWindow.EscapeKeyAction.Toggle)
                    {
                        window.Show();
                        if (gameMenuWindow != null) gameMenuWindow.SetActive(true);
                        Time.timeScale = 0f;
                    }
                }
            }
        }

        public void ResumeGame()
        {
            Time.timeScale = 1f;
            if (gameMenuWindow != null) gameMenuWindow.SetActive(false);

            List<UIWindow> windows = UIWindow.GetWindows();
            foreach (UIWindow window in windows)
            {
                if (window.IsOpen)
                {
                    window.Hide();
                }
            }
        }
    }
}
