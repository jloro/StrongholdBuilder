using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice 
{
    public int faces { get; protected set; }

    /// <summary>
    /// faces need to be >= 2.
    /// </summary>
    /// <param name="faces"> must be >= 2.</param>
    public Dice(int faces = 6) 
    {
        if (faces >= 2)
            this.faces = faces;
        else
            this.faces = 6;
    }

    virtual public int Roll()
    {
        return Random.Range(1, this.faces + 1); ;
    }
    /// <summary>
    /// Roll qtt dices and add the results 
    /// </summary>
    /// <param name="qtt"> nomber of dices to roll</param>
    /// <returns> add up of the dices results </returns>
    virtual public int Roll(int qtt)
    {
        int Result = 0;

        if (qtt < 0) { return (-1); }
        else if (qtt == 0) { return 0; }
        while (qtt > 0)
        {
            Result += Roll();
            qtt--;
        }
        return (Result);
    }
    virtual public int Roll(int dices_roll, int dices_keep)
    {
        List<int> Rolls = new List<int>(dices_roll);
        int result = 0;

        if (dices_roll <= 0 || dices_roll < dices_keep)
            return (-1);
        for (int i = 0; i < dices_roll; ++i)
        {
            Rolls[i] = Roll();
        }
        Rolls.Sort();
        for (int i = 0;  i < dices_keep; ++i)
        {
            result += Rolls[i];
        }
        return (result);
    }
}
