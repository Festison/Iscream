using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using No;
using Photon.Pun;

namespace PangGom
{
    public class DirectionKey : Puzzle, IPunObservable
    {
        float distance = 100f;
        [SerializeField]
        LayerMask layerMaskKey;

        public GameObject key = null;
        Vector3 keyVec;
        public Vector3 keyPoint;

        float rangeValue = 0.045f;
        public bool keyInput = false;
        [SerializeField]
        OpenCase openCase;
        bool keySole = false;
        public bool KeySole
        {
            get { return keySole; }
            set
            {
                keySole = value;
                if (keySole)
                {
                    SoundManager.Instance.PlayAudio(SoundManager.Instance.solveSound, false, this.transform.position);
                    openCase.Open();
                    Cursor.lockState = CursorLockMode.Locked;
                    Owner.IsMoveable = true;
                    Cursor.visible = false;
                    this.gameObject.SetActive(false);
                }
            }
        }

        private void Update()
        {
            if(Owner != null && Input.GetKeyDown(KeyCode.E))
            {
                Cursor.lockState = CursorLockMode.Locked;
                Owner.IsMoveable = true;
                Cursor.visible = false;
            }
            if (Input.GetMouseButtonDown(0))
            {
                KeyPoint();//Ű�ʱ� ��ġ ����
            }
            if (key == null)
                return;
            else if (Input.GetMouseButton(0))
            {
                KeyPosition();//Ű������
            }
            else if (Input.GetMouseButtonUp(0))
            {
                key.transform.position = keyPoint; //Ű��ġ ����
            }
        }
        void KeyPoint()//Ű�� �ʱ�ȭ�� ���� ������
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, distance, layerMaskKey))
            {
                keyPoint = hit.transform.position;//�⺻ Ű�� ����
                key = hit.transform.gameObject;//���� ���� ������Ʈ ����
                keyInput = true;
            }
        }
        void KeyPosition()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, distance, layerMaskKey))
            {
                keyVec = hit.transform.position;//�ʱ�ȭ ���� ������ ���� ��� ����

                if (Mathf.Abs(keyPoint.z - hit.point.z) < Mathf.Abs(keyPoint.y - hit.point.y))
                {
                    key.transform.position = new Vector3(keyPoint.x, hit.point.y, keyPoint.z);
                }
                else if (Mathf.Abs(keyPoint.z - hit.point.z) > Mathf.Abs(keyPoint.y - hit.point.y))
                {
                    key.transform.position = new Vector3(keyPoint.x, keyPoint.y, hit.point.z);
                }
                /*
                if (Mathf.Abs(keyPoint.x - hit.point.x) < Mathf.Abs(keyPoint.y - hit.point.y))
                {
                    key.transform.position = new Vector3(keyPoint.x, hit.point.y, keyPoint.z);
                }
                else if (Mathf.Abs(keyPoint.x - hit.point.x) > Mathf.Abs(keyPoint.y - hit.point.y))
                {
                    key.transform.position = new Vector3(hit.point.x, keyPoint.y, keyPoint.z);
                }*/
                if (Vector3.Distance(keyPoint, keyVec) > rangeValue)
                {

                    key.transform.position = keyPoint;
                }
                //�ʹ� ���콺 �����Ͷ� �Ȱ��� �����̴ϱ� �����Ű��Ƽ� ���� ������ �����̰� �ϰ� ������ 
                //���� �������� ���� ��� �������ϱ� ���ϴ´�� �߾ȿ����δ�.
            }
        }

        public override void Interact()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Owner.IsMoveable = false;
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(KeySole);
            }
            else
            {
                KeySole = (bool)stream.ReceiveNext();
            }
        }
    }
}
