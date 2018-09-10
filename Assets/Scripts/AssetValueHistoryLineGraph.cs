using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class AssetValueHistoryLineGraph : MonoBehaviour {


    public ShopAssetListing assetListing;
    public LineRenderer priceLine;
    public TextMeshProUGUI lowPrice;
    public TextMeshProUGUI highPrice;
    public TextMeshProUGUI assetName;

    public void UpdateGraph(ShopAssetListing listing) {
        if (!gameObject.activeSelf) gameObject.SetActive(true);
        assetListing = listing;
        var data = assetListing.asset;
        assetName.text = data.name;
        lowPrice.text = "$" + data.baseValue * .5f;
        highPrice.text = "$" + data.baseValue * 1.8f;
        priceLine.positionCount = 0;
        for (int i = 0; i < data.valueHistory.Count; i++) {
            priceLine.positionCount++;
            priceLine.SetPosition(i, new Vector3(50 * i, 
                C.RemapFloat(data.valueHistory[i], data.baseValue * .5f, data.baseValue * 1.8f, 0, 240)));
        }
    }


}
