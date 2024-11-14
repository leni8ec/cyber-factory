using CyberFactory.Plants.Core.Models;
using UnityEngine;

namespace CyberFactory.Common.Models {
    /// <summary>
    /// Модель объекта в игре (<see cref="ScriptableObject"/>). <br/>
    /// <br/>
    /// Например: <see cref="PlantModel"/>, <see cref="CyberFactory.Products.Models.ProductModel"/>.<br/>
    /// <br/>
    /// Отличается от конфига тем, что это корневой объект, который уже в себе может содержать конфиги. <br/>
    /// <br/>
    /// Так-же не путать с <b>DataModel</b>, в ECS - такой сущности нет, так как ее заменяет непосредственно сама Entity. 
    /// </summary>
    public abstract class ItemModelBase : ScriptableObject { }
}