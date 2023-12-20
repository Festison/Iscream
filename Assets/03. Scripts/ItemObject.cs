using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KimKyeongHun;
using PangGom;
using LeeJungChul;
using No;
using Photon.Pun;
using UnityEngine.Audio;
using static UnityEngine.UI.GridLayoutGroup;
using UnityEngine.Rendering.RendererUtils;

namespace YoungJaeKim
{
    public abstract class Item
    {
        public ItemObject itemObj;
        public Item(ItemObject itemObj)
        {
            this.itemObj = itemObj;
        }

        public abstract void Interact();
        public abstract void Active();
        public abstract void Explain();

    }
    public abstract class EquipmentItem : Item
    {
        public EquipmentItem(ItemObject im) : base(im) { }
        public override void Interact()
        {
            itemObj.Owner.inven.AddItem(this.itemObj);
            SoundManager.Instance.PlayAudio(SoundManager.Instance.itemGet, false);
            Equip();
        }
        public virtual void Equip()
        {
            itemObj.transform.SetParent(itemObj.fpsTr);
            itemObj.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.Euler(Vector3.zero));

            if (itemObj.Owner.controller.photonView.IsMine)
            {
                foreach (Renderer render in itemObj.rendererList)
                {
                    render.enabled = true;
                }
            }
            else
            {
                itemObj.Owner.inven.tpsItemDic[itemObj.itemType].SetActive(true);
                itemObj.GetComponent<Renderer>().enabled = false;
            }
        }
        public virtual void UnEquip()
        {
            if (itemObj.Owner.controller.photonView.IsMine)
            {
                foreach (Renderer render in itemObj.rendererList)
                {
                    render.enabled = false;
                }
            }
            else
                itemObj.Owner.inven.tpsItemDic[itemObj.itemType].SetActive(false);

        }
    }

    public class LightMirrorItem : EquipmentItem
    {
        public LightMirrorItem(ItemObject im) : base(im)
        {

        }
        public override void Active()
        {
            itemObj.Owner.StartCoroutine(RotateMirrorCo());

        }

        public override void Explain()
        {
        }

        IEnumerator RotateMirrorCo()
        {
            itemObj.Owner.IsMoveable = false;
            StarterAssetsInputs input = itemObj.Owner.GetComponent<StarterAssetsInputs>();
            FirstPersonController controller = itemObj.Owner.GetComponent<FirstPersonController>();
            float threshold = 0.01f;
            while (!Input.GetKey(KeyCode.E))
            {
                if (input.look.sqrMagnitude >= threshold)
                {
                    float deltaTimeMultiplier = controller.IsCurrentDevice ? 1.0f : Time.deltaTime;


                    itemObj.transform.Rotate(Vector3.up, input.look.x * controller.RotationSpeed * deltaTimeMultiplier);
                    itemObj.transform.Rotate(Vector3.right, input.look.y * controller.RotationSpeed * deltaTimeMultiplier);
                    yield return new WaitForEndOfFrame();
                }
                else
                    yield return new WaitForEndOfFrame();

            }
            itemObj.Owner.IsMoveable = true;
        }
    }


    public class CameraEquip : EquipmentItem
    {
        ScreenShot screenShot;
        public CameraEquip(ItemObject im) : base(im) { }
        public override void Active()
        {
            Debug.Log("������");

            //string timestamp = System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
            string fileName = "SCREENSHOT-" + ".png";
            string filePath = Application.dataPath + "/05. Data/ScreenShot/" + fileName;
            Debug.Log("����"!);
            itemObj.Owner.StartCoroutine(ScreenShotCapture1(filePath));

            //ScreenCapture.CaptureScreenshot(Application.dataPath+"/05. Data/ScreenShot/" + fileName);
            //�����Ŵ� �Ǳ��ϴµ� ������ �ʹ� �����ɸ�

        }
        public override void Explain()
        {
            //ī�޶��̴�. ����� ������ ������, ���� ������ ��Ʈ�� ��ϵǾ�����.
        }

        public IEnumerator ScreenShotCapture1(string filePath)
        {
            yield return new WaitForEndOfFrame();
            RenderTexture.active = itemObj.screenShotTexture;
            Texture2D texture = new Texture2D(itemObj.screenShotTexture.width, itemObj.screenShotTexture.height, TextureFormat.RGB24, true);
            texture.ReadPixels(new Rect(0, 0, itemObj.screenShotTexture.width, itemObj.screenShotTexture.height), 0, 0);
            texture.Apply();

            byte[] photo = texture.EncodeToPNG();
            System.IO.File.WriteAllBytes(filePath, photo);
        }

    }
    public class FlashLight : EquipmentItem
    {
        bool isActive = false;
        float lightPower;
        Light light;
        float maxBatttery = 100;
        float curBattery = 0;

        public FlashLight(ItemObject im) : base(im)
        {
            light = itemObj.transform.GetChild(0).GetComponent<Light>();
            curBattery = maxBatttery;
            lightPower = light.intensity;
        }
        public override void Active()
        {
            isActive = !isActive;
            if (isActive)
                itemObj.Owner.StartCoroutine(LightCo());
            //itemObj.player.playerCam.cullingMask = ~(1 << 10);


        }
        IEnumerator LightCo()
        {
            while (isActive && curBattery > 0)
            {
                light.intensity = lightPower;
                curBattery -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }
        public override void Explain()
        {
            //�÷��ö���Ʈ��. ���� �����ټ� ������, �ð��� ���� ���� ���Ⱑ �پ���
            //�ᱹ ������ ������ ���͸��� �������� �ʿ��ϴ�.
        }

        public void AddBattery(float value)
        {
            curBattery += value;
        }

    }
    public class Key : EquipmentItem
    {
        public Key(ItemObject im) : base(im) { }

        public override void Active()
        {

        }

        public override void Explain()
        {
            //�����. � ���ڸ� ���� ��������??
        }


    }

    public class Battery : Item
    {
        float batteryValue = 50f;
        public Battery(ItemObject im) : base(im) { }
        public override void Interact() { Active(); }
        public override void Active()
        {
            FlashLight tempLight = ((FlashLight)itemObj.Owner.inven.FindItem(ITEM_TYPE.FLASHLIGHT).item);
            tempLight.AddBattery(batteryValue);
        }
        public override void Explain()
        {
            //���͸��̴�. �÷��ö���Ʈ�� �����ϴµ� ���δ�.
        }
    }
    public class Lantern : EquipmentItem
    {
        public Lantern(ItemObject im) : base(im) { }
        public override void Interact()
        {
            base.Interact();
            itemObj.Owner.GetComponent<Player>().isHidden = true;
            itemObj.Owner.GetComponent<Player>().playerCam.cullingMask = -1;
        }
        public override void Active()
        {
            itemObj.Owner.isHidden = false;
            itemObj.Owner.GetComponent<Player>().playerCam.cullingMask = -1;
        }
        public override void Equip()
        {
            base.Equip();
            itemObj.transform.Rotate(Vector3.right, -90);
            itemObj.transform.Rotate(Vector3.up, 90);
        }
        public override void Explain()
        {
            //�����̴�. Ư���� ���� �վ�� �������ִ� �۾��� ������ �ִ�.
        }

    }
    public class RadioDetector : EquipmentItem
    {
        public RadioDetector(ItemObject im) : base(im) { }
        public override void Active()
        {

            if (Vector3.Distance(itemObj.transform.position, itemObj.detective.transform.position) < 1000f)
            {
                SoundManager.Instance.PlayAudio(SoundManager.Instance.detectiveSound, false);
                Debug.Log("�ͽŰ���");

                itemObj.radioSound.SetFloat("DetectiveSound", 5);
                if (Vector3.Distance(itemObj.transform.position, itemObj.detective.transform.position) < 50f)
                {
                    itemObj.radioSound.SetFloat("DetectiveSound", 10);
                    if (Vector3.Distance(itemObj.transform.position, itemObj.detective.transform.position) < 25f)
                    {
                        itemObj.radioSound.SetFloat("DetectiveSound", 15);
                    }
                }
            }
            Debug.Log("�Ҹ� ����");
        }
        public override void Explain()
        {

        }

    }

    public enum ITEM_TYPE
    {
        CAMERA,
        KEY,
        BATTERY,
        FLASHLIGHT,
        LANTERN,
        RADIODETECTOR,
        MIRROR
    }

    public class ItemObject : MonoBehaviourPunCallbacks, IInteractable
    {
        public Player player;
        public AudioMixer radioSound;
        public AudioSource audioSource;
        public LayerMask ghostLayer;
        public Detective detective;
        Player owner;
        public Item item;
        public ITEM_TYPE itemType;
        public Collider[] PlayerCol;
        public GameManager gameManager;
        public Transform fpsTr;
        public RenderTexture screenShotTexture;
        [HideInInspector]
        public List<Renderer> rendererList = new List<Renderer>();
        public Player Owner
        {
            get => owner;
            set
            {
                owner = value;
                if (owner != null)
                {
                    fpsTr = owner.fpsHandTr;
                }
            }
        }
        public void Interact()
        {
            item.Interact();
        }


        //�κ��丮�� �� �� ��ġ ���� �� �ַ��� ���� �޼���
        public void Equip()
        {
            ((EquipmentItem)item).Equip();
        }
        public void UnEquip()
        {
            ((EquipmentItem)item).UnEquip();
        }

        public void Discard()
        {
            UnEquip();
            GetComponent<Renderer>().enabled = true;
            transform.SetParent(null);
        }
        public override void OnEnable()
        {
            base.OnEnable();

            rendererList.Add(GetComponent<Renderer>());
            rendererList.AddRange(GetComponentsInChildren<Renderer>());
            switch (itemType)
            {
                case ITEM_TYPE.CAMERA:
                    item = new CameraEquip(this); break;
                case ITEM_TYPE.BATTERY:
                    item = new Battery(this); break;
                case ITEM_TYPE.KEY:
                    item = new Key(this); break;
                case ITEM_TYPE.FLASHLIGHT:
                    item = new FlashLight(this); break;
                case ITEM_TYPE.LANTERN:
                    item = new Lantern(this); break;
                case ITEM_TYPE.RADIODETECTOR:
                    item = new RadioDetector(this); break;
                case ITEM_TYPE.MIRROR:
                    item = new LightMirrorItem(this); break;
            }
        }
        private void Start()
        {
            audioSource = gameObject.GetComponent<AudioSource>();
        }
    }
}


