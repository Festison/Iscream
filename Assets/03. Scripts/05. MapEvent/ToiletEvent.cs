using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PangGom
{
    public class ToiletEvent : MonoBehaviour
    {
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
            }
        }
        [SerializeField]
        bool toiletFull = false;
        public bool ToiletFull
        {
            get { return toiletFull; }
            set { toiletFull = value; }
        }
        bool allClose = true;
        public bool AllClose
        {
            get { return allClose; }
            set
            { allClose = value;
                //if(ȭ��� ������ 0���� ũ��)
                //allClose = false
            }
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
                    EventPlay();
            }
        }
        //Area�� ToiletFull�� True�� tDCCount = 0�϶� �ش� �̺�Ʈ �߻�
        void Update()
        {
            if (ToiletEventOn)
                return;
            else
            {
                if (ToiletFull && AllClose)
                {
                    ToiletEventOn = true;
                }
            }
        }
        void EventPlay()
        {
            Debug.Log("ȭ��� �̺�Ʈ ����");
        }
    }
}
