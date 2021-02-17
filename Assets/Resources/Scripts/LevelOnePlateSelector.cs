using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelOnePlateSelector : MonoBehaviour {
    
    List<GameObject> removedSpears = new List<GameObject>();
    List<GameObject> reservedSpears = new List<GameObject>();

    public int plateIndex;
    public int xSpear; //The spear along the long wall
    public int ySpear; //The spear along the short wall
    public GameObject correctPlate;

	// Use this for initialization
	void Start () {
        colourSpears();
        selectCorrectPlate();
        obfuscatePuzzle();
        //cleanPlatesSpearLists();
    }


    public void colourSpears()
    {
        foreach (GameObject o in GameObject.FindGameObjectsWithTag("Trap"))
        {
            o.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.color = Color.magenta;
        }
    }
    public void selectCorrectPlate() {

        plateIndex = Random.Range(0, 72);
        correctPlate = GameObject.Find("pressurePlate (" + plateIndex + ")");

        char[] trimChars = { 'w', 'a', 'l', 's', 'p', 'e', 'r', ' ', '(', ')' };

        foreach (GameObject o in correctPlate.GetComponent<PressurePlate>().targetList)
        {
            removedSpears.Add(o);
        }

        correctPlate.GetComponent<PressurePlate>().targetList.Clear();
        correctPlate.GetComponent<PressurePlate>().targetList.Add(GameObject.Find("Door"));
        correctPlate.gameObject.transform.Find("Cube_001").GetComponent<MeshRenderer>().material.color = Color.blue;
        
    }

    public void obfuscatePuzzle() {

        //4 on the long wall, 2 on the short wall
        //This is to remove a few extra spears so the puzzle solution is not obvious to the player
        //We do this by removing spears but not any spears that result in a pressure plate being empty
        int iterations = 0;
        for (int i = 0; i <= 4;)
        {
            print("-------- " + i + " -------");
            int plateSelector = -1;
            do
            {
                plateSelector = Random.Range(0, 72);
            } while (plateSelector == plateIndex);

            GameObject plate = GameObject.Find("pressurePlate (" + plateSelector + ")");

            print("Selecting Plate " + plateSelector);
            bool invalid = false;
            foreach (GameObject o in plate.GetComponent<PressurePlate>().targetList)
            {
                print(plateSelector + " " + o.name);
                if (removedSpears.Contains(o) || reservedSpears.Contains(o))
                {
                    invalid = true;
                    print("This exists in a list");
                }
            }

            if (!invalid) {
                if (i <= 2
                    && (!removedSpears.Contains(plate.GetComponent<PressurePlate>().targetList[0])
                    || !reservedSpears.Contains(plate.GetComponent<PressurePlate>().targetList[0])))
                {
                    //reservePlateLine(plate.GetComponent<PressurePlate>().targetList[0]);
                    removedSpears.Add(plate.GetComponent<PressurePlate>().targetList[0]);
                    reservedSpears.Add(plate.GetComponent<PressurePlate>().targetList[1]);
                    //reservedSpears.Add(plate.GetComponent<PressurePlate>().targetList[1]);
                }
                else if (i > 2 
                    && (!removedSpears.Contains(plate.GetComponent<PressurePlate>().targetList[1])
                    || !reservedSpears.Contains(plate.GetComponent<PressurePlate>().targetList[1])))
                {
                    //reservePlateLine(plate.GetComponent<PressurePlate>().targetList[1]);
                    removedSpears.Add(plate.GetComponent<PressurePlate>().targetList[1]);
                    reservedSpears.Add(plate.GetComponent<PressurePlate>().targetList[0]);
                    //reservedSpears.Add(plate.GetComponent<PressurePlate>().targetList[0]);
                }

                plate.gameObject.transform.Find("Cube_001").GetComponent<MeshRenderer>().material.color = Color.red;
                i++;
            } else
            {
                plate.gameObject.transform.Find("Cube_001").GetComponent<MeshRenderer>().material.color = Color.green;
            }
            iterations++;
            if (iterations == 100) i = 7;

            /* if (plate.GetComponent<PressurePlate>().targetList.Count > 1) {
                 if (i <= 3 && !removedSpears.Contains(plate.GetComponent<PressurePlate>().targetList[1])) {
                     removedSpears.Add(plate.GetComponent<PressurePlate>().targetList[0]);
                 } else if (removedSpears.Contains(plate.GetComponent<PressurePlate>().targetList[1])) {
                     print(plateSelector + ":1: " + plate.GetComponent<PressurePlate>().targetList[1].name);

                 }

                 if (i > 3 && !removedSpears.Contains(plate.GetComponent<PressurePlate>().targetList[0])) {
                     removedSpears.Add(plate.GetComponent<PressurePlate>().targetList[1]);
                     print(plateSelector + ":0: " + plate.GetComponent<PressurePlate>().targetList[0].name);
                 }

                 i++;
             }*/

            //print(plate.name + " : " + plate.GetComponent<PressurePlate>().targetList.Count);
        }
        print("Reserved Spears ----------------");
        foreach (GameObject spear in reservedSpears)
        {
            print(spear.name);
        }
        print("Removed Spears ----------------");
        foreach (GameObject spear in removedSpears)
        {
            print(spear.name);
            if(reservedSpears.Contains(spear))
            {
                print("Cannot delete " + spear.name + " this is reserved");
            } else
            {
                Destroy(spear);
            }
        }
    }

    public void reservePlateLine(GameObject spear)
    {
        //Reserve all tiles' other spears where spear being deleted
        //Not used. While this would result in only one x/y plate being safe (the door), it means that every other spear MUST be active
        //While the other solution has fakes (not ideal), this solution is less interesting
        for (int i = 0; i <= 71; i++)
        {
            GameObject tile = GameObject.Find("pressurePlate (" + i + ")");
            print(tile);
            print(tile + " " + tile.GetComponent<PressurePlate>().targetList);
            if (tile.GetComponent<PressurePlate>().targetList.Contains(spear)) {
                if (tile.GetComponent<PressurePlate>().targetList[0].name == spear.name) {
                    reservedSpears.Add(tile.GetComponent<PressurePlate>().targetList[1]);
                } else if (tile.GetComponent<PressurePlate>().targetList[1].name == spear.name) {
                    reservedSpears.Add(tile.GetComponent<PressurePlate>().targetList[0]);
                } else {
                    print("?????????");
                }
                tile.gameObject.transform.Find("Cube_001").GetComponent<MeshRenderer>().material.color = Color.magenta;
            }
        }
    }

    public void cleanPlatesSpearLists()
    {
        for (int i = 0; i < 72; i++)
        {
            GameObject plate = GameObject.Find("pressurePlate (" + i + ")");
            foreach (GameObject o in plate.GetComponent<PressurePlate>().targetList)
            {
                if (o == null) { print("ALERT"); }
                print(i + ": " + o.name + " "  + " " + plate.GetComponent<PressurePlate>().targetList.Count);
            }
        }
    }

    // Update is called once per frame
    void Update () {
        GameObject plate = GameObject.Find("pressurePlate (52)");
       // print(plate.GetComponent<PressurePlate>().targetList[0].name);
	}
}
