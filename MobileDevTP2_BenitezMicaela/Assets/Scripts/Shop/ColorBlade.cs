using UnityEngine;

[CreateAssetMenu(fileName = "Blade", menuName = "Blade/Blade Color")]
public class ColorBlade : ScriptableObject
{
    public Gradient color;
    public int cost;
    public bool unlock;
}