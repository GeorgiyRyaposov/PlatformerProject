using System;
using Game.Gui.MainMenu;
using UnityEngine;

namespace Game.Gui.GameMenu
{
    public class GameMenuController : MonoBehaviour, IDisposable
    {
        [SerializeField] private MainMenuView mainMenuView;
        
        private GameMenuPresenter menuPresenter;
        
        public void Start()
        {
            menuPresenter = new GameMenuPresenter();
            menuPresenter.Attach(mainMenuView);
        }

        public void Dispose()
        {
            menuPresenter.Detach();
        }
    }
}