using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class FlexibleGridLayout : LayoutGroup
{
    public enum FitType
    {
        Uniform,
        Width,
        Height,
        FixedRows,
        FixedColumns,
        Fixed
    }

    public FitType fitType;
    public int Rows;
    public int Columns;
    public Vector2 CellSize;
    public Vector2 Spacing;

    public bool FitX;
    public bool FitY;

    public override void CalculateLayoutInputVertical()
    {
        if (fitType == FitType.Uniform || fitType == FitType.Width || fitType == FitType.Height)
        {
            FitX = true;
            FitY = true;
            float sqrRT = Mathf.Sqrt((transform.childCount));
            Rows = Mathf.CeilToInt(sqrRT);
            Columns = Mathf.CeilToInt(sqrRT);
        }

        if (fitType != FitType.Fixed)
        {
            if (fitType == FitType.Width || fitType == FitType.FixedColumns)
            {
                Rows = Mathf.CeilToInt(transform.childCount / (float) Columns);
            }
            else if (fitType == FitType.Height || fitType == FitType.FixedRows)
            {
                Columns = Mathf.CeilToInt(transform.childCount / (float) Rows);
            }
        }



        float parentWidth = rectTransform.rect.width;
        float parentHeight = rectTransform.rect.height;

        float cellWidth = (parentWidth / (float) Columns) - ((Spacing.x / (float) Columns) * (Columns - 1)) - (padding.left / (float)Columns) - (padding.right / (float)Columns);
        float cellHeight = (parentHeight / (float) Rows) - ((Spacing.y / (float) Rows) * (Rows - 1)) - (padding.top / (float)Rows) - (padding.bottom / (float)Rows);

        CellSize.x = FitX ? cellWidth : CellSize.x;
        CellSize.y = FitY ? cellHeight : CellSize.y;

        int columnCount = 0;
        int rowCount = 0;

        for (int i = 0; i < rectChildren.Count; i++)
        {
            rowCount = i / Columns;
            columnCount = i % Columns;

            var item = rectChildren[i];

            //get last child bottom pos
            float lastBottom = 0.0f;
            if (i > 0)
            {
                lastBottom = rectChildren[i - 1].rect.yMax;
                Debug.Log(lastBottom);
            }

            var xPos = (CellSize.x * columnCount) + (Spacing.x * columnCount) + padding.left;
            var yPos = (CellSize.y * rowCount) + (Spacing.y * rowCount) + padding.top;

            SetChildAlongAxis(item, 0, xPos, CellSize.x);
            SetChildAlongAxis(item, 1, yPos, CellSize.y);
        }
    }

    public override void SetLayoutHorizontal()
    {

    }

    public override void SetLayoutVertical()
    {

    }
}
