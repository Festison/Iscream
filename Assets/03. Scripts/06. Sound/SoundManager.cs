using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace PangGom
{
    public class SoundManager : Singleton<SoundManager>
    {
        [SerializeField]
        float initLength;
        public GameObject audioSourcePref;
        public AudioClip playerFootSound;//�÷��̾� �߼Ҹ�ok
        public AudioClip playerDamage;//�÷��̾ �������� �޾��� �� ok
        public AudioClip playerDead;//�÷��̾ �׾��� ��ok
        public AudioClip itemGet;//������ ȹ��
        public AudioClip ghostNormal;//�ͽ� ��� �Ҹ�
        public AudioClip ghostAttack;//�ͽ� ���� �Ҹ�
        public AudioClip doorOpen;//�� ���� �Ҹ�ok
        public AudioClip doorClose;//�� �ݴ� �Ҹ�ok
        public AudioClip waterDrop;//���������� �Ҹ�
        public AudioClip duck;//���� �Ҹ�
        public AudioClip duckRun;//���� �Ҹ�
        public AudioClip toilelEventFoot;//ȭ��� �̺�Ʈ �߼Ҹ�
        public AudioClip toilelEventHumming;//ȭ��� �̺�Ʈ ���
        public AudioClip shhSound;//�� �Ҹ�
        public AudioClip laughSound;//�����Ҹ�
        public AudioClip solveSound;//������� ȿ����

        Queue<GameObject> soundQueue = new Queue<GameObject>();

        // Ǯ�� �ʱ�ȭ�ϴ� �޼���
        protected virtual void Init()
        {
            InitQueue();
        }

        void InitQueue()
        {
            for (int i = 0; i < initLength; i++)
            {
                CreateObj();
            }
        }
        void CreateObj()
        {
            GameObject obj = Instantiate(audioSourcePref);
            obj.transform.SetParent(this.transform);
            obj.SetActive(false);
            soundQueue.Enqueue(obj);
        }

        public AudioSource PopObj()
        {
            if (soundQueue.Count == 0)
                CreateObj();
            GameObject returnObj = soundQueue.Dequeue();
            returnObj.SetActive(true);
            returnObj.transform.SetParent(null);
            return returnObj.GetComponent<AudioSource>();
        }
        public void ReturnObj(GameObject obj)
        {
            soundQueue.Enqueue(obj.gameObject);
            obj.SetActive(false);
            obj.transform.parent = this.transform;
        }
        public void PlayAudio(AudioClip clip, bool isLoop)
        {
            AudioSource audio = PopObj();
            audio.clip = clip;
            audio.loop = isLoop;
            audio.PlayOneShot(clip);
        }
        public void PlayAudio(AudioClip clip, bool isLoop, Vector3 pos)
        {
            AudioSource audio = PopObj();
            audio.transform.position = pos;
            audio.clip = clip;
            audio.loop = isLoop;
            audio.spatialBlend = 1.0f;
            audio.PlayOneShot(clip);

        }
    }
}
