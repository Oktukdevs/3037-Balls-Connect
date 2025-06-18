using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("Layout/Grid Layout Group Percent")]
public class GridLayoutGroupPercent : LayoutGroup
{
    [Range(0.01f, 1f)]
    public float cellWidthPercent = 0.25f;

    [Range(0.01f, 1f)]
    public float cellHeightPercent = 0.25f;

    public Vector2 spacing;

    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();
        SetCellsAlongAxis(0);
    }

    public override void CalculateLayoutInputVertical()
    {
        SetCellsAlongAxis(1);
    }

    public override void SetLayoutHorizontal()
    {
        SetCellsAlongAxis(0);
    }

    public override void SetLayoutVertical()
    {
        SetCellsAlongAxis(1);
    }

    private void SetCellsAlongAxis(int axis)
    {
        int childCount = rectChildren.Count;
        if (childCount == 0) return;

        float parentWidth = rectTransform.rect.width - padding.horizontal;
        float parentHeight = rectTransform.rect.height - padding.vertical;

        int maxColumns = Mathf.Max(1, Mathf.FloorToInt(1f / cellWidthPercent));
        int columns = Mathf.Min(maxColumns, childCount);
        int rows = Mathf.CeilToInt((float)childCount / columns);

        // Total spacing space
        float totalHorizontalSpacing = spacing.x * (columns - 1);
        float totalVerticalSpacing = spacing.y * (rows - 1);

        // Available space for cells after spacing is removed
        float availableWidth = parentWidth - totalHorizontalSpacing;
        float availableHeight = parentHeight - totalVerticalSpacing;

        float cellWidth = availableWidth * cellWidthPercent;
        float cellHeight = availableHeight * cellHeightPercent;

        for (int i = 0; i < childCount; i++)
        {
            int row = i / columns;
            int column = i % columns;

            float xPos = padding.left + column * (cellWidth + spacing.x);
            float yPos = padding.top + row * (cellHeight + spacing.y);

            SetChildAlongAxis(rectChildren[i], 0, xPos, cellWidth);
            SetChildAlongAxis(rectChildren[i], 1, yPos, cellHeight);
        }
    }
}
