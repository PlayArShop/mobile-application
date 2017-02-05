using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Assets.Scripts.Menus
{
    public class Table : MonoBehaviour
    {
        public enum RowPosition
        {
            Top,
            Middle,
            Bottom
        }
        private struct TableLayout
        {
            public int spacing, border;
            public TableLayout(int s, int b)
            {
                this.spacing = s;
                this.border = b;
            }
        }
        public GameObject pRow, pCell;
        private TableLayout _layout;
        private string _rowContainerPath = "TableContainer/Rows";
        private bool _hasHeader;

        private void setRowLayout(GameObject row, RowPosition pos)
        {
            VerticalLayoutGroup vlg;
            HorizontalLayoutGroup hlg;

            vlg = row.GetComponent<VerticalLayoutGroup>();
            hlg = row.transform.GetChild(0).gameObject.GetComponent<HorizontalLayoutGroup>();
            if (this._layout.border > 0)
            {
                vlg.padding.left = vlg.padding.right = this._layout.border;
                vlg.padding.top = (pos == RowPosition.Top) ? this._layout.border : this._layout.border / 2;
                vlg.padding.bottom = (pos == RowPosition.Bottom) ? this._layout.border : this._layout.border / 2;
            }
            if (this._layout.spacing > 0) hlg.spacing = this._layout.spacing;
        }

        public void init(int spacing, int border)
        {
            Transform header;
            
            this._layout = new TableLayout(spacing, border);
            if (header = this.gameObject.transform.Find(this._rowContainerPath).GetChild(0))
            {
                this.setRowLayout(header.gameObject, RowPosition.Top);
                this._hasHeader = true;
            }
        }

        public void addRow(RowPosition pos, string[] values)
        {
            GameObject row;

            row = Object.Instantiate(this.pRow);
            this.setRowLayout(row, pos);
            foreach (string value_ in values)
            {
                GameObject cell;
                cell = Object.Instantiate(this.pCell);
                cell.transform.GetChild(0).gameObject.GetComponent<Text>().text = value_;
                cell.transform.SetParent(row.transform.GetChild(0), false);
            }
            row.transform.SetParent(this.gameObject.transform.Find(this._rowContainerPath), false);
        }

        // To remove all rows
        public IEnumerator clear()
        {
            Transform rowContainer = this.gameObject.transform.Find(this._rowContainerPath);
            Transform row;
            int i = 0;
            
            // Slow process
            while (rowContainer.childCount > (this._hasHeader ? 1 : 0))
            {
                row = rowContainer.GetChild(this._hasHeader ? 1 : 0);
                row.SetParent(null, false);
                Object.Destroy(row.gameObject);
                if (i++ % 10 == 0)
                {
                    yield return null;
                }
            }
        }

        public virtual void Awake()
        {
/*            if (this.transform.parent == null || this.transform.parent.gameObject == null)
            {
                DontDestroyOnLoad(this.transform.gameObject);
            }*/
        }
    }
}