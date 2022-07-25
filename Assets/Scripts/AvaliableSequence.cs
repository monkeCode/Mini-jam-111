using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AvaliableSequence : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Image[] _images;
    public void Init(Dance.Color[] colors, string name)
    {
        colors = colors.Reverse().ToArray();
        for (int i = 0; i < colors.Length; i++)
        {
            _images[i].color = GameManager.Colors[colors[i]];
        }
        _text.text = name;
    }
}
