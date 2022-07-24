using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
    public static UserInterface Instance { get; private set; }
    [SerializeField] private Slider _hpBar;
    void Start()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateHpBar()
    {
      var hp = (float)Player.Instance.HitPoints / Player.Instance.MaxHitPoints;
      _hpBar.value = hp;
    }
}
