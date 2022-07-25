using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
    public static UserInterface Instance { get; private set; }
    [SerializeField] private Slider _hpBar;
    [SerializeField] private Image[] _sequence;
    [SerializeField] private GameObject _sequenceList;
    [SerializeField] private GameObject _sequenceBase;
    [SerializeField] private AvaliableSequence _sequencePrefab;
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private GameObject _losePanel;
    [SerializeField] private GameObject _winPanel;
    private bool _showSequence = false;
    private bool _showPause = false;
    void Start()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
        }

        Player.Instance.input.Menu.Sequences.performed += context => LookSequences();
        Player.Instance.input.Menu.Pause.performed += context => ShowPausePanel();
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

    private List<GameObject> _sequenceObjects = new List<GameObject>();

    private void LookSequences()
    {
        if(_showSequence)
        {
            _sequenceBase.SetActive(false);
            _showSequence = false;
            foreach (var it in _sequenceObjects)
            {
                Destroy(it);
            }
            _sequenceObjects.Clear();
        }
        else
        {
            _sequenceBase.SetActive(true);
            _showSequence = true;


            for (int i = 0; i < Player.Instance.Abilities.Count; i++)
            {
                var item = Instantiate(_sequencePrefab, _sequenceList.transform);
                item.transform.localPosition = new Vector3(10, -i * 100, 0);
                    item.Init(Player.Instance.Abilities[i].Sequence.ToArray(), Player.Instance.Abilities[i].Name);
                    _sequenceObjects.Add(item.gameObject);
            }
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
            _pausePanel.SetActive(true);
            _showPause = true;
            Player.Instance.input.Player.Disable();
        }
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
}
