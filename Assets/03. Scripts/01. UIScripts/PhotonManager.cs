using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

namespace LeeJungChul
{
    public class PhotonManager : MonoBehaviourPunCallbacks
    {
        public TMP_InputField nickNameInputField;           // �г��� �Է¹޴� ��.
        public TMP_Dropdown maxPlayerDropDown;              // �ִ� �ο� �� ����� ����.
        public TMP_Dropdown gamePlayTimeDropDown;           // ���� �ð��� �� �ʷ� ���� ����.

        public GameObject loadingUi;                        // �ε� UI.
        public TextMeshProUGUI currentPlayerCountText;      // �ε� UI �߿��� ���� �ο� ���� ��Ÿ��.

        void Awake()
        {
            // ������ Ŭ���̾�Ʈ�� PhotonNetwork.LoadLevel()�� ȣ���� �� �ְ�, ��� ����� �÷��̾�� �ڵ������� ������ ������ �ε��Ѵ�.
            PhotonNetwork.AutomaticallySyncScene = true;

            loadingUi.SetActive(false);

            Screen.SetResolution(1960, 1080, true);
        }

        void Start()
        {
            Debug.Log("���� ���� �õ�.");
            PhotonNetwork.ConnectUsingSettings();
        }

        /// <summary>
        /// ���� �� ����� �Լ�
        /// </summary>
        public void JoinRandomOrCreateRoom()
        {
            string nick = nickNameInputField.text;

            if (nick.Length > 0)
            {
                Debug.Log($"{nick} ���� ��Ī ����.");
                PhotonNetwork.LocalPlayer.NickName = nick;  // ���� �÷��̾� �г��� ����

                // UI���� �� ������.
                byte maxPlayers = byte.Parse(maxPlayerDropDown.options[maxPlayerDropDown.value].text); // ��Ӵٿ�� �� ������.
                int maxTime = int.Parse(gamePlayTimeDropDown.options[gamePlayTimeDropDown.value].text);

                RoomOptions roomOptions = new RoomOptions();
                roomOptions.MaxPlayers = maxPlayers; // �ο� ����.
                roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "maxTime", maxTime } }; // ���� �ð� ����.
                roomOptions.CustomRoomPropertiesForLobby = new string[] { "maxTime" }; // ���⿡ Ű ���� ����ؾ�, ���͸��� �����ϴ�.

                // �� ������ �õ��ϰ�, �����ϸ� �����ؼ� ������.
                PhotonNetwork.JoinRandomOrCreateRoom
                (
                    expectedCustomRoomProperties: new ExitGames.Client.Photon.Hashtable() { { "maxTime", maxTime } },
                    expectedMaxPlayers: maxPlayers, // ������ ���� ����.
                    roomOptions: roomOptions // ������ ���� ����.
                );
            }
            else
            {
                Debug.Log("�г����� �����ϼ���");
            }
        }

        /// <summary>
        /// ��Ī ��� �Լ�
        /// </summary>
        public void CancelMatching()
        {
            Debug.Log("��Ī ���.");
            loadingUi.SetActive(false);

            Debug.Log("�� ����.");
            PhotonNetwork.LeaveRoom();
        }

        /// <summary>
        /// �÷��̾� ī��Ʈ �ǽð� �ݿ� �Լ�
        /// </summary>
        private void UpdatePlayerCounts()
        {
            currentPlayerCountText.text = $"{PhotonNetwork.CurrentRoom.PlayerCount} / {PhotonNetwork.CurrentRoom.MaxPlayers}";
        }

        #region ���� �ݹ� �Լ�

        public override void OnConnectedToMaster()
        {
            Debug.Log("���� ���� �Ϸ�.");
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("�� ���� �Ϸ�.");

            Debug.Log($"{PhotonNetwork.LocalPlayer.NickName}�� �ο��� {PhotonNetwork.CurrentRoom.MaxPlayers} ��Ī ��ٸ��� ��.");
            Debug.Log($"{ PhotonNetwork.CurrentRoom.PlayerCount }");
            UpdatePlayerCounts();

            loadingUi.SetActive(true);
        }

        public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
        {
            Debug.Log($"�÷��̾� {newPlayer.NickName} �� ����.");
            UpdatePlayerCounts();

            if (PhotonNetwork.IsMasterClient)
            {
                // ��ǥ �ο� �� ä������, �� �̵��� �Ѵ�. ������ ������ Ŭ���̾�Ʈ��.

                if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
                {
                    PhotonNetwork.LoadLevel("PracticeScene");                   
                }
            }
        }

        public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
        {
            Debug.Log($"�÷��̾� {otherPlayer.NickName} �� ����.");
            UpdatePlayerCounts();
        }
        #endregion
    }
}
