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
        float eventTimer = 0;
        float loudness;

        public float Loudness { get => loudness; set => loudness = value; }
        public Vector3 Pos => transform.position;
        Player loudPlayer;
        public Player LoudPlayer { get => loudPlayer; set => loudPlayer = value; }

        void Start()
        {
            SoundManager.Instance.PlayAudio(SoundManager.Instance.toilelEventFoot, false, transform.position);
            Invoke("soundWaiting", 4f);
            StartCoroutine(GhostMove());//������ ����
            StartCoroutine(Event());//�Ҹ��� ���� ����
        }
        void Update()
        {

            //�ִϸ��̼�
        }
        IEnumerator GhostMove()
        {
            while (timer < 6)//8�� ���� ���
            {
                transform.Translate(Vector3.left * Time.deltaTime, Space.World);
                timer += Time.deltaTime;
                Debug.Log("Ÿ�̸�" + timer);
                yield return new WaitForEndOfFrame();
            }
            timer = 0;
            transform.Rotate(0, 90, 0);
            while (timer < 6)//8�� ���� ���
            {
                transform.Translate(Vector3.forward * Time.deltaTime, Space.World);
                timer += Time.deltaTime;
                Debug.Log("Ÿ�̸�" + timer);
                yield return new WaitForEndOfFrame();
            }
            timer = 0;
        }
        IEnumerator Event()
        {
            while (eventTimer < 8)//8�� ���� ���
            {
                eventTimer += Time.deltaTime;
                Debug.Log("Ÿ�̸�" + eventTimer);
                yield return new WaitForEndOfFrame();
            }
            eventTimer = 0;
            while (eventTimer < 10)//10�� ���� ����
            {
                eventTimer += Time.deltaTime;
                Debug.Log("Ÿ�̸�" + eventTimer);
                if (Loudness > 20)
                {
                    SoundManager.Instance.PlayAudio(SoundManager.Instance.ghostAttack, false, transform.position);
                    Debug.Log("����!");
                }
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
