using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawmillSlot : MonoBehaviour
{
    public int maxSawmillOnSlot = 3;
    private int nbSawmillOnSlot = 0;

    public bool AddSawmill() {
   //     Debug.Log("NB ON SLOT b" + nbSawmillOnSlot);
        if (nbSawmillOnSlot < maxSawmillOnSlot) {
            nbSawmillOnSlot++;
//            Debug.Log("NB ON SLOT a" + nbSawmillOnSlot);
            return true;
        }
        return false;
    }

    public bool RemoveSawmill() {
        if (nbSawmillOnSlot > 0) {
            nbSawmillOnSlot--;
            return true;
        }
        return false;
    }

    public bool CanPlace() {
        return nbSawmillOnSlot < maxSawmillOnSlot;
    }

}
