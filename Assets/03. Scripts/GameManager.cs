using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace LeeJungChul
{
    public class GameManager : Singleton<GameManager>
    {

        private void Start()
        {
           // CreatePlayer();
        }

        /// <summary>
        /// ������ ���� �ɶ� ������ġ�� �÷��̾��� ����ŭ �÷��̾ �����ϴ� �Լ�
        /// </summary>
        void CreatePlayer()
        {
            Transform[] spawnPoints = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
            int idx = Random.Range(1, spawnPoints.Length);
            PhotonNetwork.Instantiate("Player", spawnPoints[idx].position, spawnPoints[idx].rotation, 0);
        }
    }
}


