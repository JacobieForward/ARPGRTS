using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outliner : MonoBehaviour {
    public Renderer renderer;
    public Material outlineMaterial;

    public void AddOutline() {
        Material[] newMaterials = renderer.materials;
        newMaterials[1] = outlineMaterial;
        renderer.materials = newMaterials;
    }

    public void RemoveOutline() {
        Material[] newMaterials = renderer.materials;
        newMaterials[1] = null;
        renderer.materials = newMaterials;
    }
}
