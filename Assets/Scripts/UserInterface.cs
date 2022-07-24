using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
    public static UserInterface Instance { get; private set; }
    [SerializeField] private Slider _hpBar;
    [SerializeField] private Image[] _sequence;
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

    public void UpdateSequence(Dance.Color[] colors)
    {
        for (int i = 0; i < colors.Length; i++)
        {
            _sequence[i].color = GameManager.Colors[colors[i]];
        }
    }
}
