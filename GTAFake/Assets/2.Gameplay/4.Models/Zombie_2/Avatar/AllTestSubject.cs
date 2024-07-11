using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllTestSubject : MonoBehaviour
{
    int index = 0;
    public GameObject[] Zombies;
    private void Start()
    {
        for (int i = 0; i < Zombies.Length; i++)
        {
            Zombies[i].SetActive(index == i);
        }
    }
    // Start is called before the first frame update
    public void nextZombie()
    {
        index++;
        for (int i = 0; i < Zombies.Length; i++)
        {
            Zombies[i].SetActive(index % Zombies.Length == i);
        }
    }
}
