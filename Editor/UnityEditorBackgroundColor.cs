using UnityEngine;

public static class UnityEditorBackgroundColor
{
    private static readonly Color k_defaultColor = new(0.2196f, 0.2196f, 0.2196f, 1f);
    private static readonly Color k_selectedColor = new(0.172549f, 0.3647059f, 0.5294118f, 1f);
    private static readonly Color k_selectedUnFocusedColor = new(0.3f, 0.3f, 0.3f, 1f);
    private static readonly Color k_selectedHoveredColor = new(0.2706f, 0.2706f, 0.2706f, 1f);

    public static readonly Color SimpleColor = new(0.1196f, 0.1196f, 0.1196f, 1f);
    
    public static Color Get(bool isSelected, bool isHovered, bool isWindowFocused)
    {
        if (isSelected)
            return isWindowFocused ? k_selectedColor : k_selectedUnFocusedColor;

        return isHovered ? k_selectedHoveredColor : k_defaultColor;
    }
}