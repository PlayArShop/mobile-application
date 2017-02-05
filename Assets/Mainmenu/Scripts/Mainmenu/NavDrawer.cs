using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Menus
{
    public class NavDrawer : MenuNode
    {
        public struct SwitchButtonState
        {
            public Action Callback;
            public Sprite Image;

            public SwitchButtonState(Action c, Sprite i)
            {
                this.Callback = c;
                this.Image = i;
            }
        };

        [System.Serializable]
        public struct SwitchButton
        {
            public Button element;
            public Sprite menu, previous;
        }
        public GameObject container;
        public SwitchButton switchButton;
        public GameObject overlay;
        private readonly Dictionary<MenuState, SwitchButtonState> _buttonStates = new Dictionary<MenuState, SwitchButtonState>();
        private readonly Stack<GameObject> _menuPageStack = new Stack<GameObject>();
        private string[] _animationNames;
        public bool IsOpened
        {
            get
            {
                if (this.container == null) return false;
                return this.container.transform.position.x >= 0;
            }
        }
        public enum MenuState
        {
            Normal,
            Nested
        };
        private MenuState _state
        {
            get
            {
                UnityEngine.UI.Image button = this.switchButton.element.GetComponent<UnityEngine.UI.Image>();
                foreach (KeyValuePair<MenuState, SwitchButtonState> pair in _buttonStates)
                {
                    if (pair.Value.Image == button.overrideSprite)
                        return pair.Key;
                }
                return MenuState.Normal;
            }
            set
            {
                this.switchButton.element.GetComponent<UnityEngine.UI.Image>().overrideSprite = this._buttonStates[value].Image;
                this.switchButton.element.onClick.RemoveAllListeners();
                this.switchButton.element.onClick.AddListener(this._buttonStates[value].Callback.Invoke);
            }
        }

        public void QuitNestedMenu()
        {
            if (this._menuPageStack == null) return;
            this.PreviousNestedPage(this._menuPageStack.Count);
        }

        public void SwitchToNestedMenu(GameObject menuPage)
        {
            if (menuPage == null) return;
            this._menuPageStack.Push(menuPage);
            menuPage.SetActive(true);
            if (this._state == MenuState.Normal) this._state = MenuState.Nested;
        }

        public void Toggle()
        {
            this.overlay.SetActive(!this.IsOpened);
            if (this._animationNames == null)
            {
                int i = 0;
                this._animationNames = new string[2];
                foreach (AnimationState state in this.container.GetComponent<Animation>())
                {
                    this._animationNames[i++] = state.name;
                }
            }
            this.container.GetComponent<Animation>().Play(this._animationNames[this.IsOpened ? 1 : 0]);
        }

        public void PreviousNestedPage(int count)
        {
            if (this._menuPageStack == null) return;
            for (; count > 0 && this._menuPageStack.Count > 0; count--)
                this._menuPageStack.Pop().SetActive(false);
            if (this._menuPageStack.Count == 0)
                this._state = MenuState.Normal;
        }

        public void PreviousNestedPage()
        {
            this.PreviousNestedPage(1);
        }

        protected override void MenuUpdate() {}
        protected override void MenuCreation()
        {
            // Load switch button resources: callbacks and sprites
            this._buttonStates.Add(MenuState.Normal, new SwitchButtonState(this.Toggle, this.switchButton.menu));
            this._buttonStates.Add(MenuState.Nested, new SwitchButtonState(this.PreviousNestedPage, this.switchButton.previous));
            
            // Set default to normal
            this._state = MenuState.Normal;
        }
    }
}