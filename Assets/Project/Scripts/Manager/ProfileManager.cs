using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gdev
{
    public enum State
    {
        Normal,
        Critical
    }
    public abstract class ProfileManager : MonoBehaviour
    {
        protected abstract void SetColorAdjustments(State state);
        protected abstract void SetVignette(State state);
        protected abstract void SetAberration(State state);

        public void SetProfile(State type)
        {
            SetColorAdjustments(type);
            SetVignette(type);
            SetAberration(type);
        }
    }
}
