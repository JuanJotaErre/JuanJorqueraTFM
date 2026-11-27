
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{

    [SerializeField]
    HealthController player;

    [SerializeField]
    GameObject healthPrefab;

    [SerializeField]
    float healthSpacing = 100;

    List<GameObject> hearts = new List<GameObject>();





    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FillHealthBar();
    }

    private void FillHealthBar()
    {
        var health = player.maxHealth;
        for(int i = 0; i < health; i++)
        {
            var position2 = new Vector3(i*healthSpacing, 0, 0);
            var obj2 = Instantiate(healthPrefab, transform);
            obj2.transform.localPosition = position2;
            obj2.GetComponent<Image>().color = Color.black;
            
            var position = new Vector3(i*healthSpacing, 0, 1);
            var obj = Instantiate(healthPrefab, transform);
            obj.transform.localPosition = position;
            hearts.Add(obj);
        }
    }

    // Update is called once per frame
    void Update()
    {
        var health = player.currentHealth;
        for(int i = 0; i < hearts.Count; i++)
        {
            var heart = hearts[i];
            if (i < health)
            {
                heart.SetActive(true);
            } else
            {
                heart.SetActive(false);
            }
        }

    }
}
