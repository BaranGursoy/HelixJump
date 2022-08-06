using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class MainPlatform : MonoBehaviour
{
    [SerializeField] private List<Floor> floors;

    [Button(ButtonSizes.Large)]
    public void CreateRandomLevel() //FIXME Sonradan katlari da yarat ve belirli araliklar ver y'de
    {
        floors = GetComponentsInChildren<Floor>().ToList();

        floors = floors.OrderByDescending(x => x.transform.position.y).ToList();

        for(int i=1; i<floors.Count; i++)
        {
            floors[i].RandomlyCreatePlatformPiece();
        }
    }
}
