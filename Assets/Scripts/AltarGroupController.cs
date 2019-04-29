using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pincushion.LD44
{
    public class AltarGroupController : MonoBehaviour
    {
        public Altar altar1;
        public Altar altar2;
        public Altar altar3;
        public Altar altar4;
        public GrandAltar grandAltar;

        public void SacrificePerformed()
        {
            if (altar1.SacrificePerformed &&
               altar2.SacrificePerformed &&
               altar3.SacrificePerformed &&
               altar4.SacrificePerformed)
            {
                grandAltar.SacrificePerformed = true;
            }
        }
    }
}