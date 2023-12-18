using No;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace PangGom
{
    public class JigsawPuzzle : Puzzle, IPunObservable
    {
        PhotonView pv;
        float distance = 100f;
        [SerializeField]
        LayerMask layerMaskPZPiece;

        public LayerMask PieceLayer
        { 
            get { return piece.layer; } 
            set { piece.layer = value; }
        }

        GameObject piece = null;

        Vector3 puzzlePoint;//���� �ǽ� ���� ��ġ

        float rangeValue = 0.05f;
        public bool puzzleSole = false;

        int solCount = 0;
        [SerializeField]
        public int SolCount
        { get { return solCount; }
            set 
            { 
                solCount = value;
                if (solCount == 9)
                    puzzleSole = true;
            }
        }

        private void Start()
        {
            pv = GetComponent<PhotonView>();
        }
        void Update()
        {
            //Debug.Log(Owner);
            if (Owner != null && Owner.controller.photonView.IsMine)
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
            else
                return;
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
                    PieceLayer = 0;
                    SolCount++;
                }
                else
                    piece.transform.position = puzzlePoint; //���� �����߸� ��ġ ����
            }
        }

        public override void Interact()
        {
            pv.TransferOwnership(Owner.controller.photonView.Owner);
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if(stream.IsWriting)
            {
                stream.SendNext(SolCount);
                stream.SendNext(PieceLayer);
            }
            else
            {
                SolCount = (int)stream.ReceiveNext();
                PieceLayer = (LayerMask)stream.ReceiveNext();
            }
        }
    }
}
