using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Sequenses : MonoBehaviour
{
    private List<GameObject> _sequenceObjects = new();
    [SerializeField] private AvaliableSequence _sequencePrefab;
    [SerializeField] private GameObject _sequenceList;
    private void OnEnable()
    {
        for (int i = 0; i < Player.Instance.Abilities.Count; i++)
        {
            var item = Instantiate(_sequencePrefab, _sequenceList.transform);
            item.transform.localPosition = new Vector3(10, -i * 100, 0);
            item.Init(Player.Instance.Abilities[i].Sequence.ToArray(), Player.Instance.Abilities[i].Name);
            _sequenceObjects.Add(item.gameObject);
        }
    }

    private void OnDisable()
    {
        foreach (var it in _sequenceObjects)
        {
            Destroy(it);
        }
        _sequenceObjects.Clear();
    }
}
