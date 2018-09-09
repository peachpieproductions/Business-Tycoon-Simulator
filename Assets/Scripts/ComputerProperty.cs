using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerProperty : MonoBehaviour {

    public Computer computer;


    public void resetListings() {

        foreach(PropertyListing p in GetComponentsInChildren<PropertyListing>()) {
            p.owned = false;
            p.buyButtonText.text = "BUY";
        }

    }

    public void UpdatePropertyWebsite() {



    }

}
