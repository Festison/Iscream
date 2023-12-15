using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace PangGom
{
    public class JigsawPuzzle : MonoBehaviour
    {
        float distance = 100f;
        [SerializeField]
        LayerMask layerMaskPZPiece;

        GameObject piece = null;
        //GameObject surchTr;//�ڽ� ��ġ ã����
        //GameObject[] clearTr = new GameObject[9];

        Vector3 puzzlePoint;//���� �ǽ� ���� ��ġ

        float rangeValue = 0.05f;
        public bool puzzleSole = false;
        int solCount = 0;

        /*
        public JigsawPuzzle(InteractableObject target) : base(target)
        {
            this.target = target;
        }
        private void Awake()
        {
            for (int i = 0; i < 9; i++)
            {
                Transform targetTr = this.GetComponent<Transform>();
                surchTr = targetTr.parent.GetComponent<GameObject>();
                surchTr = transform.GetChild(i).GetChild(0).gameObject;
                Debug.Log(surchTr.gameObject.name);
                clearTr[i] = surchTr;
            }
        }
        void Start()
        {
        }*/
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
                PuzzlePoint();//���� �ʱ� ��ġ ����
            else if (Input.GetMouseButton(0))
                PuzzlePosition();//���� ������
            else if (Input.GetMouseButtonUp(0))
                PuzzleMatch();
            if (puzzleSole)
                Destroy(gameObject);
        }
        void PuzzlePoint()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, distance, layerMaskPZPiece))
            {
                puzzlePoint = hit.transform.position;//���� ��ġ ����
                piece = hit.transform.gameObject;//���� ���� ������Ʈ ����
            }
        }
        void PuzzlePosition()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, distance, layerMaskPZPiece))
                piece.transform.position = new Vector3(hit.point.x, hit.point.y, puzzlePoint.z);
        }
        void PuzzleMatch()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, distance, layerMaskPZPiece))
            {
                if (Vector3.Distance(piece.transform.position, piece.transform.parent.position) < rangeValue)
                {
                    piece.transform.position = piece.transform.parent.position;
                    solCount++;
                    if (solCount == 9)
                        puzzleSole = true;
                }
                else
                    piece.transform.position = puzzlePoint; //���� �����߸� ��ġ ����
            }
        }
    }
}
