﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class AssetData : ScriptableObject {

    [System.Serializable]
    public class CraftingIngredient {
        public AssetData ingredient;
        public int amount;
    }

    public enum Type { Furniture, Food, RawMaterial, Tech, Transport, Item, Living, Wearable }
    /*
     * Furniture - regular furniture, storage, workbenches, decorations, toilets, shower, etc...
     * Food - any food stuff, even cooking/raw ingredients, anything that can be eaten
     * Raw Material - wood planks, metal scrap, cloth, iron ingot, glass, paper, stone, fiber, wire, copper, plastic, crystal (potions), cardboard etc...
     * Tech - most electronics - cpu, phone, appliances, stove, oven, microwave, alarms, radios, arcade machine, game console, etc...
     * Transport - cars, bikes, motorbikes, boats, handgliders, scooters, skateboards, rollerskates, karts, planes, helicopters, etc...
     * Items - ANYthing that can be used - weapons, brooms, drugs, alcohol?, cash?, music (cds), cellphone, medkits, shovel, etc...
     * Living - Dogs, cats, farm animals - pigs, cows, chickens, horses, turtles, ants (ant farm?), can all be sold.
     * Wearable - anything that can be worn - clothes, jewelry, hats, bracelets, shoes, glasses, accessories, peircings, tatoos?, 
     */

    public Type type;
    public string useTag;
    public int assetLevel = 1;
    public float baseValue;
    public float currentValue;
    public List<float> valueHistory = new List<float>();
    public float currentPop;
    public bool physicsAsset;
    public bool smallAsset;
    public float mass = 1;
    public bool liquid;
    public GameObject modelPrefab;
    public float modelYOffset;
    public float miniModelScaleMultiplier = 1;
    public float invModelScaleMultiplier = 1;
    public float invModelYOffset;
    public float invModelRotOffset;
    public int craftingOutput = 1;
    public bool cantBeSold;
    public List<AssetData> craftingMaterials = new List<AssetData>();
    public List<CraftingIngredient> craftingIngredients = new List<CraftingIngredient>();
    public List<AssetData> breakdownMaterials = new List<AssetData>();
    public WorkbenchType workbenchNeeded;

}
