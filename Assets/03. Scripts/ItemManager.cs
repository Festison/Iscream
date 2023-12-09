using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KimKyeongHun;
using No;
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
    }
    public class Medicine : RecoveryItem
    {
        public Medicine(ItemManager im) : base(im) { }
        public override void Interact() { Debug.Log("���̴�"); }
        public override void Active()
        {
            //im.player.���ŷ� ���� += ȸ����
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
    }
    public class Lantern : EquipmentItem
    {
        public Lantern(ItemManager im) : base(im) { }
        public override void Interact() { Debug.Log("�����̴�"); }
        public override void Active()
        {
        }
    }
    public class FlashLight : EquipmentItem
    {

        public FlashLight(ItemManager im) : base(im) { }
        public override void Interact() { Debug.Log("�÷��ö���Ʈ!"); }
        public override void Active()
        {
            Debug.Log("����?");
            im.BatteryTime -= Time.deltaTime;
            Debug.Log(im.BatteryTime);
            /*while (im.BatteryTime > 0)
            {
                
                im.flashLight.intensity -= 0.0001f;
                
                
                
            }*/

        }
    }
    public class Key : EquipmentItem
    {
        public Key(ItemManager im) : base(im) { }

        public override void Active()
        {
        }

        public override void Interact() { Debug.Log("�����"); }
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
    }
    public class Battery : ConsumableItem
    {
        public Battery(ItemManager im) : base(im) { }
        public override void Interact() { Debug.Log("���͸���"); }
        public override void Active()
        {
            im.flashLight.intensity = 2.1f;
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
        // --��ſ� �ּ�--
        // ������ ��������� �� ������ �ȵȰ� ���Ƽ� �ּ� ���ܿ�
        // �ϴ� �������� ���� ���� �����ؾߵǴ� ������Ʈ�̰�
        // �����ɽ�Ʈ�� ���� �ݶ��̴��� �浹ó���� �ؼ�
        // �� ������Ʈ�� ������ �ִ� Ŭ����(�������̺� ��ӹ��� ������Ʈ)�� �������̽��� �����ͼ� ����Ѵٴ� ���̿���� //Player.cs 107�� ���� ����
        // �׷��� MonoBehaviour�� IInteractivable�̶� ���ÿ� ����� �޾ƾ� �ؿ�
        // ���������� ����ϰ�����Ŵ� ���ظ� �߰�
        // �׷� ItemManager�� �������̽��� ��ӹ����� ���� �� ���ƿ�
        // �׸��� �̸��� ��ü�� �ϳ��ϳ� �� ������Ʈ�ϱ� �Ŵ������ٴ� ItemObject�� �ٸ� �̸����� ���°� �� �� �������ϰŶ�� �����ؿ�.
        // + Start���� if�� �����Ȱ� switch case ������ �ٲ�����ϴ�.
        // ++ �������� �ҹ��ڷ� �������ֽñ� �Լ�, ������Ƽ���� �빮�� �������� ���ּ���!!
        // �о�ð� ���ؾȵǰų� �ñ��ϰų� �̰��� �ƴѰͰ����� �����Ŵ� ����or���� ���ֽð� �� �ּ��� �� ���������� �����ֽø� �˴ϴ�
        // ���� ������Ʈ �Ⱓ ȭ�����̿���

        public Player player;
        public Item item;
        public ITEM_TYPE itemType;
        public Light flashLight;
        public float BatteryTime = 60f;
        ScreenShot screenShot;

        // Start is called before the first frame update
        void Start()
        {
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


