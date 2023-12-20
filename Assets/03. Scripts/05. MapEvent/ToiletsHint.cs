using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PangGom;
using KimKyeongHun;

namespace PangGom
{
    public class ToiletsHint : MonoBehaviour, IListenable
    {

        float timer = 0;
        float loudness;

        public float Loudness { get => loudness; set => loudness = value; }
        public Vector3 Pos => transform.position;
        Player loudPlayer;
        public Player LoudPlayer { get => loudPlayer; set => loudPlayer = value; }

        void Start()
        {
            SoundManager.Instance.PlayAudio(SoundManager.Instance.toilelEventFoot, false, transform.position);
            Invoke("soundWaiting", 5f);
        }

        void Update()
        {
            // �������̵�
            //�ִϸ��̼�
            //�Ҹ��� ���� ����
        }
        IEnumerator Event()
        {
            while (timer < 15)
            {
                timer += Time.deltaTime;
                Debug.Log("Ÿ�̸�" + timer);
                if (Loudness > 20)
                    //����
                yield return new WaitForEndOfFrame();
            }
        }
        void soundWaiting()
        {
            Debug.Log("���� ������");
            SoundManager.Instance.PlayAudio(SoundManager.Instance.toilelEventHumming, false, transform.position);
        }
    }
}
