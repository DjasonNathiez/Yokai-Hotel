using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEditor;

[CustomEditor(typeof(Shop))]
public class InspectorShop : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();


        Shop shop = (Shop)target;
        if (GUILayout.Button("Add Enchantement to Player"))
        {
            shop.Achat();
            
        }
    }
}

public class Shop : MonoBehaviour
{
    Enchantement enchant;
    InventoryManager inventory;
    public GameObject[] itemFolder;
    public int selectItem;


    private void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryManager>();
        enchant = GetComponent<Enchantement>();
        itemFolder = Resources.LoadAll<GameObject>("Enchants");


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //apparition du bouton d'interraction

            if (Input.GetButtonDown("Interagir"))
            {
                selectItem = Random.Range(0, itemFolder.Length); //generation des items

                //ouvrir l'onglet d'achat

            }
        }
    }

    void GenerateItems()
    {

        //itemFolder[selectItem];
    }

    public void Achat()
    {
        GameObject enchantement = Instantiate(itemFolder[selectItem]);
        enchantement.transform.parent = GameObject.FindGameObjectWithTag("Player").transform;
        enchantement.GetComponent<Enchantement>().isActivated = true;
    }
}
