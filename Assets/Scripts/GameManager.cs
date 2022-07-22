using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static Dictionary<Dance.Color, Color> Colors = new()
    {
        {Dance.Color.Yellow, Color.yellow },
        {Dance.Color.Blue, Color.blue},
        {Dance.Color.Green, Color.green},
        {Dance.Color.Red, Color.red},
        {Dance.Color.Purple, Color.magenta}
    };
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
