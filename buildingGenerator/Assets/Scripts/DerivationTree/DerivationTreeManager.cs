using UnityEngine;
using System.Collections.Generic;

public class DerivationTreeManager:MonoBehaviour
{
    public Material nodeRepresentationMaterial;
    //any good tree needs a root
    private DerivationTreeNode root;
    private DerivationTreeNode backupRoot; //backup root for resetting the tree

    //list of all active nodes in the tree
    private HashSet<DerivationTreeNode> activeNodes = new HashSet<DerivationTreeNode>();
    //maps nodes to their representations in the scene (cubes)
    private Dictionary<DerivationTreeNode, GameObject> nodeCubeMap = new Dictionary<DerivationTreeNode, GameObject>();

    private int recipeStep = 0;
    private int recipeLength = 0;
    private Recipe recipe;


    public DerivationTreeManager(Scope rootScope, Recipe recipe)
    {
        root = new DerivationTreeNode(rootScope);
        backupRoot = root;
        activeNodes.Add(root);
        this.recipe = recipe;
        this.recipeLength = recipe.GetLength();
    }
    
    private bool PerformRecipeStep()
    {
        GrammarRule rule = recipe.GetRule(recipeStep);
        if (rule == null)
        {
            Debug.LogError($"No rule found for recipe step {recipeStep}. Cannot perform the step.");
            return false;
        }
        List<DerivationTreeNode> nodesToProcess = new List<DerivationTreeNode>(activeNodes);
        foreach (DerivationTreeNode node in nodesToProcess)
        {
            if (node.HasTag(rule.targetTag))
            {
                node.ApplyRule(rule);
                activeNodes.Remove(node);
                foreach (DerivationTreeNode child in node.GetChildren())
                {
                    activeNodes.Add(child);
                    CreateCubeRepresentation(child);
                }
                DestroyCubeRepresentation(node);
            }
        }
        recipeStep++;
        return true;
        
    }

    public void RunWholeRecipe()
    {
        while (recipeStep < recipeLength)
        {
            if (!PerformRecipeStep())
            {
                Debug.LogError("Failed to perform recipe step. Stopping recipe execution.");
                break;
            }
        }
    }

    private void CreateCubeRepresentation(DerivationTreeNode node)
    {
        Scope scope = node.GetScope();
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.name = string.IsNullOrEmpty(scope.tag) ? "DerivedCube" : scope.tag;

        cube.transform.SetParent(this.transform);
        cube.transform.position = scope.position;
        cube.transform.rotation = scope.rotation;
        cube.transform.localScale = scope.scale;

        if (nodeRepresentationMaterial != null)
        {
            Renderer renderer = cube.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = nodeRepresentationMaterial;
            }
        }

        nodeCubeMap[node] = cube;
    }

    private void DestroyCubeRepresentation(DerivationTreeNode node)
    {
        if (nodeCubeMap.TryGetValue(node, out GameObject cube))
        {
            if (cube != null)
            {
                Destroy(cube);
            }   
            nodeCubeMap.Remove(node);
        }
    }

}