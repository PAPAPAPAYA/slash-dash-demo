using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHpDisplayScript : MonoBehaviour
{
    public List<GameObject> player_HpIndicators;
    private int indicatorsShowed;
    #region SINGLETON
    public static PlayerHpDisplayScript me;
    private void Awake()
    {
        me = this;
    }
    #endregion
    private void Start()
    {
        indicatorsShowed = player_HpIndicators.Count;
    }
    private void Update()
    {
        if (indicatorsShowed > 0 &&
            indicatorsShowed > GameManager.me.playerHp) // when more indicator is showed than player actual hp
        {
            player_HpIndicators[indicatorsShowed - 1].SetActive(false); // turn an indicator off
            indicatorsShowed--; // update indicator showed amount
        }
    }
    public void ShowPlayerHp() // show all hp
    {
        foreach (var indicator in player_HpIndicators)
        {
            indicator.SetActive(true);
            indicatorsShowed++;
        }
    }
}
