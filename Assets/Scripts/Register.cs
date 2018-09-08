using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Register : MonoBehaviour {


    public Transform assetBeingSoldModel;
    public Transform moneySpawnPoint;



    private void Update() {

        if (assetBeingSoldModel.childCount > 0) {
            assetBeingSoldModel.Rotate(0, Time.deltaTime * 20f, 0);
        }

    }

}
