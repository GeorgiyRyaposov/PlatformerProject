using System.Collections.Generic;
using UnityEngine;

namespace Game.Data.Scenes
{
    [CreateAssetMenu(fileName = "LocationsHolder", menuName = "ScriptableObjects/LocationsHolder")]
    public class LocationsHolder : ScriptableObject
    {
        [SerializeField] private List<LocationScenes> values;
        public List<LocationScenes> Values => values;
        
        [SerializeField] private LocationScenes startingLocation;
        public LocationScenes StartingLocation => startingLocation; 
    }
}