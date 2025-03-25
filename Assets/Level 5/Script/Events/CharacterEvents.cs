using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
namespace level5
{
    public class CharacterEvents
    {
        public static UnityAction<GameObject, int> characterDamaged;
        public static UnityAction<GameObject, int> characterHealed;
    }
}