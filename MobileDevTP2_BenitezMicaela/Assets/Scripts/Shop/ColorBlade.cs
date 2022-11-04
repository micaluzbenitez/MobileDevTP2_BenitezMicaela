using UnityEngine;

[CreateAssetMenu(fileName = "Blade", menuName = "Blade/Blade Color")]
public class ColorBlade : ScriptableObject
{
    public int ID;
    public Gradient color;
    public int cost;
}