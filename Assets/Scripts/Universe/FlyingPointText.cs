using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FlyingPointText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI pointTMP;

    public void SetPointText(int point)
    {
        pointTMP.text = "+" + point;
    }
    
    public void DestroyTextObj()
    {
        Destroy(gameObject);
    }
}
