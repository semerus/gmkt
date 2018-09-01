using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class Race
{
    [JsonProperty]
    public Vector3 StartPos;
    [JsonProperty]
    public Vector3 EndPos;
}
