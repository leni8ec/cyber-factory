using Scellecs.Morpeh.Providers;
using TriInspector;
using UnityEngine;

namespace CyberFactory.Plants.Components {

    [AddComponentMenu("ECS/" + nameof(Plant))] [HideMonoScript]
    public class PlantProvider : MonoProvider<Plant> { }

}