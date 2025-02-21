using KimKyeongHun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenMouthObj : MonoBehaviour,IListenable
{

    [SerializeField]
    Vector3 originMouthPos1;
    [SerializeField]
    Vector3 originMouthPos2;

    [SerializeField]
    Transform MouthTr1;
    [SerializeField]
    Transform MouthTr2;

    [SerializeField]
    Vector3 targetMouthPos1;
    [SerializeField]
    Vector3 targetMouthPos2;

    float maxLoudness = 100f;

    float loudness;
    bool isFixed = false;
    public float Loudness { get => loudness; set => loudness = value; }

    public Vector3 Pos => transform.position;

    Player loudPlayer;
    public Player LoudPlayer { get => loudPlayer; set => loudPlayer = value; }

    private void Start()
    {
        ListenerManager.Instance.listeners.Add(this);
    }
    private void Update()
    {
        if (loudness / maxLoudness > 0.6f)
            isFixed = true;
        if (isFixed)
            return;
        MouthTr1.localPosition = Vector3.Lerp(originMouthPos1, targetMouthPos1, Loudness / maxLoudness);
        MouthTr2.localPosition = Vector3.Lerp(originMouthPos2, targetMouthPos2, Loudness / maxLoudness);
    }
}
