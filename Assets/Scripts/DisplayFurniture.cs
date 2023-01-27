
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayFurniture : MonoBehaviour
{
    public FurnitureItem furnitureItem;
    public Image furnitureItemImage;
    public TextMeshProUGUI furnitureName;
    public int furnitureId;

    Spawner spawner;
    private void Awake() 
    {
        spawner = FindObjectOfType<Spawner>();
    }
    void Start()
    {
        furnitureItemImage.sprite = furnitureItem.furnitureImage;
        furnitureName.text = furnitureItem.furnitureName;
        furnitureId = furnitureItem.furnitureId;
    }

    public void SpawnFurnitureWithId()
    {
        spawner.SpawnObject(furnitureId);
    }
}
