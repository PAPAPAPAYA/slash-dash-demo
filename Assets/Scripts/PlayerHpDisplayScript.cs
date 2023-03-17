using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHpDisplayScript : MonoBehaviour
{
    public static PlayerHpDisplayScript me;
    public List<GameObject> player_HpIndicators;
    private int indicatorsShowed;
    private void Awake()
    {
        me = this;
    }
    private void Start()
    {
        indicatorsShowed = player_HpIndicators.Count;
    }
    private void Update()
    {
        if (indicatorsShowed >0 &&
            indicatorsShowed > GameManager.me.playerHp)
        {
            player_HpIndicators[indicatorsShowed - 1].SetActive(false);
            indicatorsShowed--;
        }
    }
    public void ShowPlayerHp()
    {
        foreach (var indicator in player_HpIndicators)
        {
            indicator.SetActive(true);
            indicatorsShowed++;
        }
    }
}
