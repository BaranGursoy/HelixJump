using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDataHolder : MonoBehaviour
{
    public Ball ball;
    public MainPlatform mainPlatform;

    /*public void AdjustMainPlatformManually(MainPlatform mainPlatform)
    {
        Destroy(this.mainPlatform);
        this.mainPlatform = mainPlatform;
        this.mainPlatform.transform.SetParent(transform);
    }*/
}
