using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KimKyeongHun;
using No;
using JetBrains.Annotations;

namespace YoungJaeKim
{
    public abstract class Item : IInteractable
    {
        public ItemManager im;
        Player owner;
        public Item(ItemManager im)
        {
            this.im = im;
        }
        public Player Owner
        {
            get => owner;
            set => owner = value;
        }

        public abstract void Interact();
        public abstract void Active();
        public abstract void Explain();

    }
    public abstract class RecoveryItem : Item
    {
        public RecoveryItem(ItemManager im) : base(im) { }
        public override void Interact()
        {

        }
    }
    public class Food : RecoveryItem
    {
        public Food(ItemManager im) : base(im) { }

        public override void Interact() { Debug.Log("�����̴�"); }

        public override void Active()
        {
            //im.player.���ŷ� ���� += ȸ����
        }
        public override void Explain()
        {
            //�����̴�. �÷��̾��� ���ŷ��� ȸ�������ش�.
        }

    }
    public class Medicine : RecoveryItem
    {
        public Medicine(ItemManager im) : base(im) { }
        public override void Interact() { Debug.Log("���̴�"); }
        public override void Active()
        {
            //im.player.���ŷ� ���� += ȸ����
        }
        public override void Explain()
        {
            //���̴�. �÷��̾��� ���ŷ��� �뷮 ȸ�������ش�.
        }


    }
    public class Beverage : RecoveryItem
    {
        public Beverage(ItemManager im) : base(im) { }
        public override void Interact() { Debug.Log("�������"); }
        public override void Active()
        {
            //im.player.���ŷ� ���� += ȸ����
        }
        public override void Explain()
        {
            //������̴�. �÷��̾��� ���ŷ��� �ҷ� ȸ�������ش�.
        }
    }
    public abstract class EquipmentItem : Item
    {
        public EquipmentItem(ItemManager im) : base(im) { }
        public override void Interact()
        {

        }
    }
    public class CameraEquip : EquipmentItem
    {
        public CameraEquip(ItemManager im) : base(im) { }
        public override void Interact() { Debug.Log("ī�޶��"); }
        public override void Active()
        {
            Debug.Log("������");


            //ScreenCapture.CaptureScreenshot(Application.dataPath+"/05. Data/ScreenShot/" + fileName);
            //�����Ŵ� �Ǳ��ϴµ� ������ �ʹ� �����ɸ�



        }
        public override void Explain()
        {
            //ī�޶��̴�. ����� ������ ������, ���� ������ ��Ʈ�� ��ϵǾ�����.
        }

    }
    public class Lantern : EquipmentItem
    {
        public Lantern(ItemManager im) : base(im) { }
        public override void Interact() { Debug.Log("�����̴�"); }
        public override void Active()
        {
        }
        public override void Explain()
        {
            //�����̴�. ���� ������ �뵵�ε� ������, Ư���� �ɷ��� ������ �ִ�.
        }

    }
    public class FlashLight : EquipmentItem
    {

        public FlashLight(ItemManager im) : base(im) { }
        public override void Interact() { Debug.Log("�÷��ö���Ʈ!"); }
        public override void Active()
        {
            Debug.Log("����?");
            
            
            //im.flashLight.intensity = im.lightPower;
            im.lightOn = true;
            

        }
        public override void Explain()
        {
            //�÷��ö���Ʈ��. ���� �����ټ� ������, �ð��� ���� ���� ���Ⱑ �پ���
            //�ᱹ ������ ������ ���͸��� �������� �ʿ��ϴ�.
        }

    }
    public class Key : EquipmentItem
    {
        public Key(ItemManager im) : base(im) { }

        public override void Active()
        {
        }

        public override void Interact() { Debug.Log("�����"); }
        public override void Explain()
        {
            //�����. � ���ڸ� ���� ��������??
        }

    }
    public class ConsumableItem : Item
    {
        public ConsumableItem(ItemManager im) : base(im) { }

        public override void Active()
        {
        }

        public override void Interact()
        {

        }
        public override void Explain() { }
    }
    public class Battery : ConsumableItem
    {
        public Battery(ItemManager im) : base(im) { }
        public override void Interact() { Debug.Log("���͸���"); }
        public override void Active()
        {
            im.flashLight.intensity = 2.1f;
        }
        public override void Explain()
        {
            //���͸��̴�. �÷��ö���Ʈ�� �����ϴµ� ���δ�.
        }

    }

    public enum ITEM_TYPE
    {
        FOOD = 1 << 0,
        MEDICINE = 1 << 1,
        BEVERAGE = 1 << 2,
        CAMERA = 1 << 3,
        LANTERN = 1 << 4,
        KEY = 1 << 5,
        BATTERY = 1 << 6,
        FLASHLIGHT = 1 << 7,
    }

    public class ItemManager : MonoBehaviour
    {
        public bool lightOn;
        public float lightPower;
        public Player player;
        public Item item;
        public ITEM_TYPE itemType;
        public Light flashLight;
        public float batteryTime;
        ScreenShot screenShot;

        // Start is called before the first frame update
        void Start()
        {
            lightOn = false;
            //flashLight = GetComponent<Light>();
            switch (itemType)
            {
                case ITEM_TYPE.FOOD:
                    item = new Food(this); break;
                case ITEM_TYPE.MEDICINE:
                    item = new Medicine(this); break;
                case ITEM_TYPE.BEVERAGE:
                    item = new Beverage(this); break;
                case ITEM_TYPE.CAMERA:
                    item = new CameraEquip(this); break;
                case ITEM_TYPE.BATTERY: 
                    item = new Battery(this); break;
                case ITEM_TYPE.LANTERN:
                    item = new Lantern(this); break;
                case ITEM_TYPE.KEY:
                    item = new Key(this); break;
                case ITEM_TYPE.FLASHLIGHT: 
                    item = new FlashLight(this); break;
            }
            item.Interact();
            item.Active();
            
        }

        // Update is called once per frame
        void Update()
        {

            if (Input.GetKeyDown(KeyCode.P))
            {
                //string timestamp = System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
                string fileName = "SCREENSHOT-" + ".png";
                string filePath = Application.dataPath + "/05. Data/ScreenShot/" + fileName;
                Debug.Log("����"!);
                StartCoroutine(ScreenShotCapture1(filePath));
            }
            //lightPower -= Time.deltaTime;
            batteryTime += 0.0001f;
            if(batteryTime == 1) { batteryTime = 1; }
            if (lightOn)
            {
                
               
                lightPower = Mathf.Lerp(2.3f, 0, batteryTime);


                flashLight.intensity = lightPower;


                if (flashLight.intensity <= 0)
                {
                    lightOn = false;
                }
                Debug.Log(flashLight.intensity);
                
            }

        }

        public IEnumerator ScreenShotCapture1(string filePath)
        {
            yield return new WaitForEndOfFrame();


            Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, true);
            texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            texture.Apply();

            byte[] photo = texture.EncodeToPNG();
            System.IO.File.WriteAllBytes(filePath, photo);
        }
    }
}


