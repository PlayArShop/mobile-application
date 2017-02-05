using UnityEngine;
using System;
using System.Collections;

namespace Assets.Scripts.Menus
{
    public class DiscountsPage : MenuNode
    {
        public GameObject discountsTable;
        private Table _discountsTable;

        public void OnEnable()
        {
            this._api.getDiscounts(this.discountsCallback);
        }

        public void OnDisable()
        {
            this._api.StartCoroutine(this._discountsTable.clear());
        }
     
       private IEnumerator fillTable()
        {
            string[] data = new string[3];

            for (int i = 0; i < this._api.Discount.data.Length; ++i)
            {
                data[0] = this._api.Discount.data[i].name;
                data[1] = this._api.Discount.data[i].success;
                data[2] = DateTime.Parse(this._api.Discount.data[i].created_a).ToString("d/M/yyyy HH:mm");
                this._discountsTable.addRow(
                    (i < this._api.Discount.data.Length - 1) ? Table.RowPosition.Middle : Table.RowPosition.Bottom,
                    data
                );
                if (i % 5 == 0) yield return null;
            }
        }

        public int discountsCallback(int k, string l)
        {
            if (k == 0)
            {
                StartCoroutine(this.fillTable());
            }
            return 0;
        }

        protected override void MenuUpdate() {}

        protected override void MenuCreation()
        {
            this._discountsTable =  this.discountsTable.GetComponent<Table>();
            this._discountsTable.init(4, 4);
        }
    }
}