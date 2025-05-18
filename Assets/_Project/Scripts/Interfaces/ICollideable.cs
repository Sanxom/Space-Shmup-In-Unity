using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodeLabTutorial
{
    public interface ICollideable
    {
        public int DamageAmount { get; set; }

        public void Collide();
    }
}