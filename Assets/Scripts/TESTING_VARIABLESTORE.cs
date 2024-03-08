using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TESTING_VARIABLESTORE : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        VariableStore.CreateDatabase("DB1");
        VariableStore.CreateDatabase("DB2");
        VariableStore.CreateDatabase("DB3");

        VariableStore.CreateVariable("num1", 1);
        VariableStore.CreateVariable("num5", 5);
        VariableStore.CreateVariable("ligthison", true);
        VariableStore.CreateVariable("float1", 1f);
        VariableStore.CreateVariable("string1", "hello");

        VariableStore.PrintAllDatabases();
        VariableStore.PrintAllVariables();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            VariableStore.RemoveAllVariables();
        }
    }
}
