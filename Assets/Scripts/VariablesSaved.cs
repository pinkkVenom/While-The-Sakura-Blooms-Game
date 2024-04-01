using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariablesSaved : MonoBehaviour
{
    [SerializeField] GameObject slot1;
    [SerializeField] GameObject slot2;
    [SerializeField] GameObject slot3;
    GameObject item;
    public string itemName;

    public string var_str;

    // Start is called before the first frame update
    void Start()
    {

        VariableStore.CreateDatabase("Items");

        VariableStore.CreateVariable("Items.itemtype", itemName, () => itemName, value => itemName = value);

        //VariableStore.PrintAllDatabases();
        //VariableStore.PrintAllVariables();
    }

    // Update is called once per frame
    void Update()
    {
        VariableStore.TryGetValue("Default.money", out var v);
        slot1.name = (string)v;
        if (slot1.transform.childCount > 0)
        {
            item = slot1;
            itemName = item.transform.GetChild(0).name;
            //Debug.Log($"{itemName}");
        }
        else
        {
            item = null;
            itemName = "";
            //Debug.Log($"{itemName}");
        }
    }
}
