using System.Collections.Generic;
using UnityEngine;

public class Example : MonoBehaviour
{
    public PlayerData playerData;
    [TextArea(4, 10)] public string json;

    private void Start()
    {
        playerData = JsonUtility.FromJson<PlayerData>(json);
    }
}

[System.Serializable]
public class PlayerData
{
    public string username;
    public float powerModifier;
    public KingdomResources resources;
    public List<string> units;
}

[System.Serializable]
public class KingdomResources
{
    public int gold;
    public int wood;
    public int iron;
}
