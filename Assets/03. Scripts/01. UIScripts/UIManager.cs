using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace LeeJungChul
{
    public class UIManager : MonoBehaviour
    {
        [Header("�ɼ� ȭ��")]
        [Tooltip("�ػ󵵿� ���带 ������ �� �ִ� �ɼ� ȭ��")]
        [SerializeField] private GameObject option;

        [Header("�ε� ȭ��")]
        [Tooltip("��Ƽ���ӿ��� �÷��̾� ��Ī�� ��ٸ��� �ε� ȭ��")]
        [SerializeField] private GameObject panel;

        #region �ε�ȭ�� Ȱ��ȭ �� ��Ȱ��ȭ ���
        public void StartLoading()
        {
            panel.SetActive(true);
        }
        public void StartLoadingExit()
        {
            panel.SetActive(false);
        }
        #endregion

        #region �ɼ�ȭ�� Ȱ��ȭ �� ��Ȱ��ȭ ���
        public void OnClick()
        {
            option.SetActive(true);
        }

        public void OnClickExit()
        {
            option.SetActive(false);
        }
        #endregion

        public void SoloPlay()
        {
            SceneManager.LoadScene("PracticeScene");
        }

        #region ���� ����
        public void GameExit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }
        #endregion
    }


}
