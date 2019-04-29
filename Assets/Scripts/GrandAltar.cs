using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pincushion.LD44
{
    public class GrandAltar : MonoBehaviour
    {
        public MeshRenderer altarIdol1MeshRenderer;
        public MeshRenderer altarIdol2MeshRenderer;
        public MeshRenderer altarIdol3MeshRenderer;
        public MeshRenderer altarIdol4MeshRenderer;

        private bool interactable = false;
        public bool Interactable
        {
            get
            {
                return interactable;
            }
            set
            {
                interactable = value;
            }
        }

        private bool sacrificePerformed;
        public bool SacrificePerformed
        {
            get
            {
                return sacrificePerformed;
            }
            set
            {
                sacrificePerformed = value;

                if (sacrificePerformed == true)
                {
                    interactable = true;
                    altarIdol1MeshRenderer.material = afterSacrificeMaterial;
                    altarIdol2MeshRenderer.material = afterSacrificeMaterial;
                    altarIdol3MeshRenderer.material = afterSacrificeMaterial;
                    altarIdol4MeshRenderer.material = afterSacrificeMaterial;
                }
            }
        }

        private Material afterSacrificeMaterial;
        private void Awake()
        {
            afterSacrificeMaterial = Resources.Load("AltarIdolAfterSacrificeMaterial") as Material;
        }
    }
}