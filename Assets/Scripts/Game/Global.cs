using System.Collections.Generic;
using Common.ServiceLocator;

namespace Game
{
    public static class Global
    {
        public static IServiceLocator ServiceLocator;

        public static readonly List<IServiceLocator> SceneServiceLocators = new();
    }
}