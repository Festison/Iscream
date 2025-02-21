using Cinemachine;
using LeeJungChul;
using Photon.Pun;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using No;
using UnityEngine.UI;
using PangGom;
using YoungJaeKim;

namespace KimKyeongHun
{
    public class Player : MonoBehaviourPun, IPunObservable
    {
        bool isRaycasting;
        public bool IsRaycasting
        {
            get => isRaycasting;
            set
            {
                isRaycasting = value;
                if (isRaycasting)
                {
                    Interact();
                }
            }
        }


        [SerializeField]
        public bool isLocal = false;

        [Header("플레이어 스텟")]
        [Tooltip("플레이어 현재 정신력")]
        [SerializeField] private float currentHp = 100;
        [Tooltip("플레이어 최대 정신력")]
        [SerializeField] private float maxHp = 100;

        [Tooltip("자신의 캐릭터 모델들")]
        [SerializeField]
        Renderer[] tpsRenders;

        public CinemachinePriority cinemachinePriority;
        public Inventory inven;
        public Item item;

        public GameObject ob1; //시네머신 둘리 실행 전 기존 카메라 1번 
        public GameObject ob2; //시네머신 둘리 실행 후 기존 카메라에서 2번 카메라 

        [Header("플레이어가 가지고있는 UI")]
        [Tooltip("상호작용 할 수 있는 상태 일시 이미지가 빨간색으로 변한다.")]
        [SerializeField] private Image InteractImage;
        [Tooltip("플레이어가 피격시 흔들리는 애니메이션 실행")]
        [SerializeField] private Animator mentalityImage;
        [SerializeField] private TextMeshProUGUI mentalityText;
        [SerializeField] private TextMeshProUGUI playerNickname;
        [SerializeField] private GameObject PlayerDeadPanel;

        public CinemachineVirtualCamera vircam;

        public bool isHidden = true;

        public Transform fpsHandTr;
        public Transform tpsHandTr;



        public Camera playerCam;
        // 플레이어 정신력 프로퍼티
        public float Hp
        {
            get
            {
                return currentHp;
            }
            set
            {
                currentHp = value;

                if (currentHp > maxHp)
                {
                    currentHp = maxHp;
                }
            }
        }

        MicComponent mic;

        public FirstPersonController controller;
        public StarterAssetsInputs inputsystem;

        private bool isMoveable = true;

        public bool IsMoveable
        {
            get => isMoveable;
            set
            {
                isMoveable = value;
                if (value)
                {
                    controller.enabled = true;
                    inputsystem.enabled = true;
                }              
                else
                {
                    controller.enabled = false;
                    inputsystem.enabled = false;
                }                 
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            inputsystem = GetComponent<StarterAssetsInputs>();
            controller = GetComponent<FirstPersonController>();
            GameManager.Instance.playerList.Add(this);
            mic = GetComponent<MicComponent>();

            cinemachinePriority = GetComponentInChildren<CinemachinePriority>();

            vircam = GetComponentInChildren<CinemachineVirtualCamera>();

            ob2 = GameManager.Instance.dollyCart2;


            playerNickname.text = photonView.Owner.NickName;

            if (controller.photonView.IsMine)
            {
                foreach (Renderer render in tpsRenders)
                {
                    render.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
                }
            }
            else
            {
                foreach (Behaviour comp in playerCam.GetComponents<Behaviour>())
                {
                    if (comp as PhotonTransformView)
                        continue;
                    comp.enabled = false;
                }

            }

        }

        // Update is called once per frame
        void Update()
        {
            IsInteract();

            if (photonView.IsMine)
            {
                if (isHidden)
                {
                    playerCam.cullingMask = ~(1 << 10);

                }
                else { playerCam.cullingMask = -1; }

                if (currentHp<=0)
                {
                    isMoveable = false;
                    Debug.Log("플레이어 죽음 ");
                    PlayerDeadPanel.SetActive(true);
                    controller.enabled = false;
                    cinemachinePriority.GetIsPlayerCheck = true;
                }
            }
            mic.SetListener();
            if (controller.photonView.IsMine)
            {
                controller.photonView.RPC("DebugDraw", RpcTarget.AllBuffered);
            }

            if (controller.photonView.IsMine && inputsystem.click)
            {
                Debug.Log("a버튼 ");
                Click();
            }

            if (controller.photonView.IsMine && inputsystem.itemUse)
            {
                Debug.Log("b버튼 ");
                ItemActive();
                inputsystem.itemUse = false;
            }


            HpText();
        }

        /// <summary>
        /// 플레이어의 정신력이 줄어들 때 정신력 이미지 애니메이션 활성화
        /// </summary>
        public void HpDown()
        {
            // 정신력이 하락할때
            mentalityImage.SetTrigger("Hit");
            currentHp -= 5;
            SoundManager.Instance.PlayAudio(SoundManager.Instance.playerDamage, false);
        }

        private void HpText()
        {
            mentalityText.text = ("Metality  " + currentHp + " / " + maxHp);
        }



        public void Click()
        {

            IsRaycasting = true;

            inputsystem.click = false;
        }

        [PunRPC]
        void DebugDraw()
        {
            Debug.DrawRay(playerCam.transform.position, playerCam.transform.forward * 10f);

        }
        public void Interact()
        {
            Debug.Log("sendnesxt");
            //문열림, 불 켜기 등등
            RaycastHit hit;

            if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward * 10f, out hit, 10))
            {
                if (hit.transform.TryGetComponent<IInteractable>(out IInteractable interactable))
                {
                    interactable.Owner = this;
                    interactable.Interact();

                    Debug.Log("상호작용");

                }

            }
        }

        public void ItemActive()
        {
            if (inven.curItem != null)
            {
                inven.curItem.item.Active();
            }
        }

        public void InteractionDollyCart()
        {

            IsMoveable = false;


            if (controller.photonView.IsMine)
            {
                vircam.Follow = ob2.gameObject.transform;
                var pov = vircam.AddCinemachineComponent<CinemachinePOV>();

                pov.m_HorizontalAxis.m_MaxSpeed = 100f;
                pov.m_VerticalAxis.m_MaxSpeed = 80f;
            }
            else
            {
                foreach (Renderer renderer in tpsRenders)
                {
                    renderer.enabled = false;
                }
            }

            ob2.GetComponent<CinemachineDollyCart>().enabled = true;

        }

        public void CancelDollyCart()
        {

            IsMoveable = true;

            if (controller.photonView.IsMine)
            {
                vircam.Follow = ob1.transform;
                vircam.DestroyCinemachineComponent<CinemachinePOV>();
                ob2.GetComponent<CinemachineDollyCart>().enabled = false;
                ob2.GetComponent<CinemachineDollyCart>().m_Position = 0;
            }
            else
            {
                foreach (Renderer renderer in tpsRenders)
                {
                    renderer.enabled = true;
                }
            }



        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(IsRaycasting);
            }
            else
            {
                IsRaycasting = (bool)stream.ReceiveNext();
            }
            IsRaycasting = false;
        }

        public void IsInteract()
        {
            RaycastHit hit;

            if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward * 10f, out hit, 10))
            {
                if (hit.transform.TryGetComponent<IInteractable>(out IInteractable interactable))
                    InteractImage.color = Color.red;
                else
                    InteractImage.color = Color.white;
            }
        }
    }
}
