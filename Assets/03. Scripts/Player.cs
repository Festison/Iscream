using System.Collections;
using System.Collections.Generic;
using UnityEngine;



<<<<<<< Updated upstream
namespace KimKyeongHun
{
=======

public class Player : MonoBehaviour
{


    public GameObject playerEye; // ����ĳ��Ʈ �÷��̾� ��
    public Transform doorGet;


    public InteractStratagy stratagy;
    public InteractableObject target;

    IInteractable interactable;

    MicComponent mic;


    public void Interact()
    {
        //������, �� �ѱ� ���
        RaycastHit hit;


        if (Physics.Raycast(playerEye.transform.position, transform.forward * 10f, out hit, 10))
        {
            interactable = hit.transform.GetComponent<IInteractable>();
            interactable.Owner = this;
            interactable.Interact();
        }


    }

    public void Click()
    {
        //interactable.Interact();

        Interact();

    }
>>>>>>> Stashed changes

    public class Player : MonoBehaviour
    {
<<<<<<< Updated upstream
        public Camera playerCam;

        IInteractable interactable;
        public void Interact()
        {
            //문열림, 불 켜기 등등

            RaycastHit hit;

            Debug.DrawRay(playerCam.transform.position, transform.forward * 10f, Color.red);

            if (Physics.Raycast(playerCam.transform.position, transform.forward * 10f, out hit, 10))
            {
                interactable = hit.transform.GetComponent<IInteractable>();
                interactable.Owner = this;
                interactable.Interact();

                Debug.Log(hit.transform.GetComponent<Transform>() + "정보 ");
                
            }


        }

        public void Click()
        {
            Interact();
        }



        // Start is called before the first frame update
        void Start()
        {
            playerCam = FindObjectOfType<Camera>();
        }

        // Update is called once per frame
        void Update()
        {



            if (Input.GetKey(KeyCode.JoystickButton0))
            {

                Debug.Log("a버튼 ");
                Click();

            }

            //if (Input.GetKey(KeyCode.Space))
            //{

            //    Debug.Log("a버튼 ");
            //    Click();

            //}



      
=======
        target = new InteractableObject();
        mic = GetComponent<MicComponent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.JoystickButton0))
        {

            Debug.Log("a��ư ");
            Click();
            //Click();
>>>>>>> Stashed changes
        }
        
    }

    
}
