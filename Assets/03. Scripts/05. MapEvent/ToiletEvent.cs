using KimKyeongHun;
using No;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PangGom
{
    public class ToiletEvent : MonoBehaviourPunCallbacks, IListenable
    {

        public PhotonView PV;

        public InteractableObject[] interactableObjs;
        private List<ToiletDoor> toiletDoors = new List<ToiletDoor>();
        public GameObject femalePrb;
        public GameObject doorCol;
        public GameObject hintObj;
        public GameObject hipObj;
        public Vector3 myDoorVec;

        private void Start()
        {
            ListenerManager.Instance.listeners.Add(this);
            interactableObjs = GetComponentsInChildren<InteractableObject>();
            foreach (InteractableObject obj in interactableObjs)
            {
                toiletDoors.Add((ToiletDoor)obj.stratagy);
            }
        }

        public int TotalTDCCount
        {
            get
            {
                int total = 0;

                foreach (var toiletDoor in toiletDoors)
                {
                    total += toiletDoor.TDCCount;
                }
                return total;
            }
        }

        [SerializeField]
        int toiletPlayerCount = 0;
        public int ToiletPlayerCount
        {
            get { return toiletPlayerCount; }
            set
            {
                toiletPlayerCount = value;
                if (toiletPlayerCount == PhotonNetwork.CurrentRoom.PlayerCount)//�ִ��ο����� �����ϸ� ToiletFull Ʈ��
                    ToiletFull = true;
                //�� ���� üũ
            }
        }
        [SerializeField]
        bool toiletFull = false;
        public bool ToiletFull
        {
            get { return toiletFull; }
            set { toiletFull = value; }
        }

        [SerializeField]
        bool toiletEventOn = false;
        public bool ToiletEventOn
        {
            get { return toiletEventOn; }
            set
            {
                toiletEventOn = value;
                if (toiletEventOn)
                {
                    StartCoroutine(EventPlay());
                }
            }
        }

        float loudness;
        public float Loudness { get => loudness; set => loudness = value; }

        public Vector3 Pos => transform.position;

        Player loudPlayer;
        public Player LoudPlayer { get => loudPlayer; set => loudPlayer = value; }
        float timer = 0;

        //Area�� ToiletFull�� True�� tDCCount = 0�϶� �ش� �̺�Ʈ �߻�
        void Update()
        {
            if (ToiletEventOn)
                return;
            else
            {
                if (ToiletFull && TotalTDCCount == 0)
                {
                    ToiletEventOn = true;
                    Debug.Log(ToiletEventOn);
                }
            }
        }
        IEnumerator EventPlay()
        {
            Debug.Log("ȭ��� ī��Ʈ ����");
            doorCol.SetActive(true);
            SoundManager.Instance.PlayAudio(SoundManager.Instance.shhSound, false, transform.position);//��
            while (timer < 6f)
            {
                timer += Time.deltaTime;
                if (Loudness > 20)
                    timer = 0;
                Debug.Log("Ÿ�̸�" + timer);
                yield return new WaitForEndOfFrame();
            }
            Debug.Log("ȭ��� �̺�Ʈ ����");
            Vector3 pos = new Vector3(-14.8f, 3.87f, -3.5f);
            Instantiate(femalePrb, pos, Quaternion.identity).GetComponent<ToiletsHint>().toiletEvent = this;
            Invoke("HipObj", 5f);
            Invoke("Hint", 32f);
        }
        void Hint()
        {
            hintObj.SetActive(true);
            doorCol.SetActive(false);
            hipObj.SetActive(false);

        }
        void HipObj()
        {
            hipObj.SetActive(true);

        }

    }
}
