using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Economy : MonoBehaviour {


    private void Start() {
        InitEcon();
        StartCoroutine(EconUpdate());
    }

    public void InitEcon() {
        foreach(AssetData d in C.c.data.assetData) {
            d.currentValue = d.baseValue;
            d.valueHistory.Clear();
            d.currentPop = .5f;
        }
    }

    public IEnumerator EconUpdate() {
        while (true) {
            foreach (AssetData d in C.c.data.assetData) {
                d.currentValue += Random.Range(-1f, 1f);
                d.currentValue = Mathf.Clamp(d.currentValue, d.baseValue * .5f, d.baseValue * 2f);
                if (d.valueHistory.Count == 14) d.valueHistory.RemoveAt(0); 
                d.valueHistory.Add(d.currentValue);
                d.currentPop += Random.Range(-.1f, .1f);
                d.currentPop = Mathf.Clamp01(d.currentPop);
            }
            C.c.currentShop.computer.UpdateComputer();
            yield return new WaitForSeconds(10f);
        }
    }

}
