using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusUI : MonoBehaviour
{
    [SerializeField] StatusText prefab;

    PlayerState player;
    StatusText[] texts;

    void Start()
    {
        player = PlayerState.Instance;
        texts = new StatusText[5];
        for(int i = 0; i<texts.Length;i++)
        {
            texts[i] = Instantiate(prefab, transform);
        }
        
    }

    void Update()
    {
        ShowStatus();
    }

    void ShowStatus()
    {
        texts[0].Show("HP", ((int)player.Hp).ToString());
        texts[1].Show("Attack", player.power.ToString(), player.AddAttack);
        texts[2].Show("Defence", player.defense.ToString(),player.AddDefense);
        texts[3].Show("Hungry", ((int)player.Hungry).ToString());
        texts[4].Show("Thirst", ((int)player.Thirst).ToString());
    }
}
