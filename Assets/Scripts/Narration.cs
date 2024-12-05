using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Narration
{
    public string[] names;

    [TextArea(3, 10)]
    public string[] sentences;

}
