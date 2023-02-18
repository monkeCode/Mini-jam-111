using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
    public static UserInterface Instance { get; private set; }
    [SerializeField] private Slider _hpBar;
    [SerializeField] private Image[] _sequence;
    [SerializeField] private GameObject _sequenceBase;
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private GameObject _losePanel;
    [SerializeField] private GameObject _winPanel;
    [SerializeField] private GameObject _inventoryPanel;
    [SerializeField] private GameObject _mapPanel;
    [SerializeField] private TextMeshProUGUI _coinsText;
    [SerializeField] private EventText _eventText;
    private bool _showSequence;
    private bool _showPause;

    private void OnDestroy()
    {
        Player.Instance.CoinsChanged -= UpdateCoins;
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Player.Instance.input.Menu.Sequences.performed += context => ShowPanel(_sequenceBase);
        Player.Instance.input.Menu.Pause.performed += context => ShowPausePanel();
        Player.Instance.input.Menu.Inventory.performed += context => ShowPanel(_inventoryPanel);
        Player.Instance.input.Menu.Map.performed += context => ShowPanel(_mapPanel);
        Player.Instance.CoinsChanged += UpdateCoins;
        Player.Instance.HpChanged += UpdateHpBar;
    }

    public void UpdateHpBar(int hitPoints)
    {
        var hp = (float)hitPoints / Player.Instance.MaxHitPoints;
        _hpBar.value = hp;
    }

    public void UpdateSequence(Dance.Color[] colors)
    {
        for (int i = 0; i < colors.Length; i++)
        {
            _sequence[i].color = GameManager.Colors[colors[i]];
        }
    }

   
    public void ShowPausePanel()
    {
        if(_showPause)
        {
            _pausePanel.SetActive(false);
            _showPause = false;
            Player.Instance.input.Player.Enable();
        }
        else
        {
            CloseAllPanels();
            _pausePanel.SetActive(true);
            _showPause = true;
            Player.Instance.input.Player.Disable();
        }
    }

    private void CloseAllPanels()
    {
        _inventoryPanel.SetActive(false);
        _mapPanel.SetActive(false);
        _sequenceBase.SetActive(false);
    }

    private void ShowPanel(GameObject panel)
    {
        if(_showPause) return;
        var activeState = panel.activeSelf;
        CloseAllPanels();
        panel.SetActive(!activeState);

    }

    public void ShowWinPanel()
    {
        _winPanel.SetActive(true);
        Player.Instance.input.Player.Disable();
        Player.Instance.input.Menu.Disable();
    }
    public void ShowLosePanel()
    {
        _losePanel.SetActive(true);
        Player.Instance.input.Player.Disable();
        Player.Instance.input.Menu.Disable();
    }

    private void UpdateCoins(int newCoinsValue)
    {
        StopAllCoroutines();
        StartCoroutine(CoinsUpdater(newCoinsValue));
    }

    IEnumerator CoinsUpdater(int coins)
    {
        for (var value = int.Parse(_coinsText.text); value != coins; value = int.Parse(_coinsText.text))
        {
            if (coins == value) yield break;
            value += value > coins ? -1 : 1;
            _coinsText.text = value.ToString();
            yield return new WaitForSeconds(0.05f);
        }

    }

    public void UpdateStats()
    {
        UpdateHpBar(Player.Instance.HitPoints);
    }

    public void CreateText(string text, Vector2 pos)
    {
        Instantiate(_eventText, pos, Quaternion.identity).Init(text);
    }
}
