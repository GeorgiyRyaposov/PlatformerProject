using System;
using UnityEngine;

namespace Game.Gui.MainMenu
{
    public class MainMenuController : MonoBehaviour, IDisposable
    {
        [SerializeField] private MainMenuView mainMenuView;
        
        private MainMenuPresenter mainMenuPresenter;
        
        public void Start()
        {
            mainMenuPresenter = new MainMenuPresenter();
            mainMenuPresenter.Attach(mainMenuView);
        }

        public void Dispose()
        {
            mainMenuPresenter.Detach();
        }
    }
}