using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class Unit : MonoBehaviour
{
    [SerializeField] int health;

    public async void Start()
    {
        Debug.Log("Health at Start " + health);

        if (health < 100)
        {
            StartCoroutine(ReceiveHealing());
        }

        await Task.Delay(3000);        
        Debug.Log("Health at End " + health);
    }

    public IEnumerator ReceiveHealing()
    {
        float timer = 3f;

        while(timer > 0)
        {
            health += 5;
            if (health >= 100)
            {
                health = 100;
                Debug.Log(health);
                yield break;
            }
            Debug.Log(health);
            timer -= 0.5f;
            yield return new WaitForSeconds(0.5f);
        }
    }
}

