using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pincushion.LD44
{
    public class Altar : MonoBehaviour
    {
        public AltarGroupController altarGroupController;
        public MeshRenderer altarIdolMeshRenderer;


        private bool interactable = true;
        public bool Interactable {
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
                    interactable = false;
                    altarIdolMeshRenderer.material = afterSacrificeMaterial;

                    altarGroupController.SacrificePerformed();
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