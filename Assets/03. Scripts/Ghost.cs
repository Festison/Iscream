using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.HID;

namespace YoungJaeKim
{
    public class Ghost : MonoBehaviour, IListenable
    {
        public Detective detective;
        public Transform targetPlayer;
        Vector3 targetPosition;
        RaycastHit hit;
        NavMeshAgent ghostAgent;
        public bool isRandgeDetection;
        Rigidbody rb;
        [SerializeField]

        float cubeVolume = 10;

        public float Loudness { set => throw new System.NotImplementedException(); }

        public Vector3 Pos => throw new System.NotImplementedException();

        // Start is called before the first frame update
        void Start()
        {           
            rb = GetComponent<Rigidbody>();
            ghostAgent = GetComponent<NavMeshAgent>();   
            
        }
        // Update is called once per frame
        void Update()
        {
            if (detective.IsDection)
            {

                //Vector3 targetVec = (detective.LastDetectivePos - transform.position).normalized;// ��������

                //transform.forward = targetVec;//Ÿ�ٹ������� �����ϵ��� �������

                //rb.velocity = targetVec * enemySpeed;
                ghostAgent.SetDestination(detective.col.transform.position);
                Debug.Log("���� ������");

            }

            Collider[] cols1 = Physics.OverlapSphere(transform.position, cubeVolume, 1 << 7);

            
        }
            
            
        }
    }

