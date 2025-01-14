using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Interactions/Item")]
public class Item : ScriptableObject
{
    public string Name = "";
    public Sprite image;
}
