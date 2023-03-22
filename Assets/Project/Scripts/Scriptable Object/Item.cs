using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gdev
{
    public class Item : ScriptableObject
    {
        [Header("Item Information")]
        public string ItemName;
        public Sprite ItemIcon;
    }
}

