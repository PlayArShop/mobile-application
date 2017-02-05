using UnityEngine;
using System;
using System.Collections;
using Facebook.Unity;
using System.Linq;
using System.Collections.Generic;

namespace Assets.Scripts.Menus
{
    public class ScoresPage : MenuNode
    {
        public GameObject scoresTable;
        public GameObject shareContainer;
        private Table _scoresTable;
        private PSApi.Communication.ScoreModel.Data _lastScore;

        public void OnEnable()
        {
            this._api.getScores(this.scoresCallback);
            if (FB.IsLoggedIn && !this.shareContainer.activeSelf) this.shareContainer.SetActive(true);
        }

        public void OnDisable()
        {
            this._api.StartCoroutine(this._scoresTable.clear());
        }

        private IEnumerator fillTable()
        {
            string[] data = new string[3];

            for (int i = 0; i < this._api.Score.data.Length; ++i)
            {
                if (i == 0) this._lastScore = this._api.Score.data[i];
                data[0] = this._api.Score.data[i].name;
                data[1] = this._api.Score.data[i].score;
                data[2] = DateTime.Parse(this._api.Score.data[i].created_a).ToString("d/M/yyyy HH:mm");
                this._scoresTable.addRow(
                    (i < this._api.Score.data.Length - 1) ? Table.RowPosition.Middle : Table.RowPosition.Bottom,
                    data
                );
                if (i % 5 == 0) yield return null;
            }
        }

        private int scoresCallback(int k, string l)
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
            this._scoresTable =  this.scoresTable.GetComponent<Table>();
            this._scoresTable.init(4, 4);
        }

        public void FacebookShareButtonEvent()
        {
            if (!AccessToken.CurrentAccessToken.Permissions.Contains("publish_actions"))
            {
                FB.LogInWithPublishPermissions(new List<string>() {"publish_actions"}, (ILoginResult res) =>
                {
                    if (AccessToken.CurrentAccessToken.Permissions.Contains("publish_actions"))
                    {
                        shareScoreToFacebook(this._lastScore);
                    }
                });
            }
            else shareScoreToFacebook(this._lastScore);
        }

        private void shareScoreToFacebook(PSApi.Communication.ScoreModel.Data score)
        {
            FB.FeedShare("",
                         new Uri("http://playarshop.com/"),
                         "PlayARshop pour Android et iOS",
                         "Jouez pour gagner des réductions!",
                         "J'ai fait un score de " + score.score + " sur le mini-jeu " + score.name + " !",
                         new Uri("https://eip.epitech.eu/img/projects/2017/playarshop.png")
            );
        }
    }
}