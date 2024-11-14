using System;
using CyberFactory.Common.States;
using Scellecs.Morpeh;
using UnityEngine;

namespace CyberFactory.Common.Timer {
    /// <summary>
    /// Обычный таймер, при завершении которого, на объект навешивается компонент <see cref="CompleteState"/><br/>
    /// <br/>
    /// // todo: возможно потребуется собственный компонент о завершении таймера (напр: <b>TimerComplete</b>)
    /// </summary>
    [Serializable]
    public struct Timer : IComponent {

        [Tooltip("Total duration of the timer")]
        public float interval;

        [Tooltip("Elapsed time in seconds")]
        public float time;

        public float TimeLeft => time >= interval ? 0 : interval - time;
        public bool IsComplete => time >= interval;

        public float Progress => Mathf.Clamp01(time / interval);

    }
}