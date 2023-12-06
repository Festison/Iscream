using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

namespace LeeJungChul
{
    public class LobbyManager : MonoBehaviourPunCallbacks
    {
        private readonly string gameVersion = "1";

        [Header("��Ʈ��ũ ������ ǥ���� �ؽ�Ʈ")]
        public TextMeshProUGUI connectionInfoText;
        [Header("�� ���� ��ư")]
        public Button joinButton;
        [Header("�ε� ȭ��")]
        public GameObject panel;

        private void Start()
        {
            Screen.SetResolution(1920, 1080, false);
            PhotonNetwork.GameVersion = gameVersion;

            PhotonNetwork.ConnectUsingSettings();

            joinButton.interactable = false;

            connectionInfoText.text = "������ ������ ������...";
        }

        public override void OnConnectedToMaster()
        {
            joinButton.interactable = true;
            connectionInfoText.text = "�¶��� : ������ ������ �����";
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            joinButton.interactable = false;

            connectionInfoText.text = "�������� : ������ ������ ������� ���� \n ���� ��õ� ��...";

            PhotonNetwork.ConnectUsingSettings();
        }

        public void Connect()
        {
            joinButton.interactable = false;
            panel.SetActive(true);

            if (PhotonNetwork.IsConnected)
            {
                connectionInfoText.text = "�뿡 ����...";
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                connectionInfoText.text = "�������� : ������ ������ ������� ����\n ���� ��õ� ��...";
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            connectionInfoText.text = "�� ���� ����, ���ο� �� ����...";

            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 });
        }

        public override void OnJoinedRoom()
        {
            connectionInfoText.text = "�� ���� ����";

            PhotonNetwork.LoadLevel("LobbyScene");
        }
    }
}

