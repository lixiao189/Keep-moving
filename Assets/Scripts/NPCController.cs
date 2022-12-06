using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    // Prefebs for NPC cars
    public GameObject[] carPrefeb;

    // Car state
    public int maxCarNum = 6;

    private int tick = 0;
    private int period = 150;

    void FixedUpdate()
    {
        // Get the number of sub objects
        var carNum = transform.childCount;

        if (tick == 0)
        {
            if (carNum < maxCarNum)
            {
                var carSize = carPrefeb.Length;
                var random = Random.Range(0, carSize);

                // Instantiate a random car
                var carObj = Instantiate(carPrefeb[random], transform.position, transform.rotation);

                // Add sub object
                carObj.transform.parent = transform;
            }
        }

        tick = (tick + 1) % period;
    }

    // Get state of NPC cars
    public bool IsAllCarEnd()
    {
        var carNum = transform.childCount;

        for (int i = 0; i < carNum; i++)
        {
            var carObj = transform.GetChild(i).gameObject;
            var carController = carObj.GetComponent<NPCCarController>();

            if (carController.state != NPCCarController.NPCCarState.End)
            {
                return false;
            }
        }

        return true;
    }

}
